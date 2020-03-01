using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Dapper;
using Polly;
using Pitstop.WorkshopManagementAPI.Repositories.Model;
using Pitstop.Infrastructure.Messaging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Data.SqlClient;
using Pitstop.WorkshopManagementAPI.Domain.Entities;

namespace Pitstop.WorkshopManagementAPI.Repositories
{
    public class SqlServerWorkshopPlanningRepository : IWorkshopPlanningRepository
    {
        private static readonly JsonSerializerSettings _serializerSettings;
        private static readonly Dictionary<DateTime, string> _store = new Dictionary<DateTime, string>();
        private string _connectionString;

        static SqlServerWorkshopPlanningRepository()
        {
            _serializerSettings = new JsonSerializerSettings();
            _serializerSettings.Formatting = Formatting.Indented;
            _serializerSettings.Converters.Add(new StringEnumConverter 
            { 
                NamingStrategy = new CamelCaseNamingStrategy() 
            });
        }

        public SqlServerWorkshopPlanningRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<WorkshopPlanning> GetWorkshopPlanningAsync(DateTime date)
        {
            WorkshopPlanning planning = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // get aggregate
                await Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(5, r => TimeSpan.FromSeconds(5), (ex, ts) => 
                        { Console.WriteLine("Error connecting to DB. Retrying in 5 sec."); })
                    .ExecuteAsync(() => conn.OpenAsync());

                 var aggregate = await conn
                        .QuerySingleOrDefaultAsync<Aggregate>(
                            "select * from WorkshopPlanning where Id = @Id", 
                            new { Id = date.ToString("yyyy-MM-dd") });
                
                if (aggregate == null)
                {
                    return null;
                }

                // get events
                IEnumerable<AggregateEvent> aggregateEvents = await conn
                    .QueryAsync<AggregateEvent>(
                        "select * from WorkshopPlanningEvent where Id = @Id order by [Version];",
                        new { Id = date.ToString("yyyy-MM-dd") });
            
                List<Event> events = new List<Event>();
                foreach (var aggregateEvent in aggregateEvents)
                {
                    events.Add(DeserializeEventData(aggregateEvent.MessageType, aggregateEvent.EventData));
                }
                planning = new WorkshopPlanning(date, events);
            }
            return planning;
        }

        public async Task SaveWorkshopPlanningAsync(string planningId, int originalVersion, int newVersion, IEnumerable<Event> newEvents)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // update eventstore
                await Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(5, r => TimeSpan.FromSeconds(5), (ex, ts) => 
                        { Console.WriteLine("Error connecting to DB. Retrying in 5 sec."); })
                    .ExecuteAsync(() => conn.OpenAsync());

                using (var transaction = conn.BeginTransaction())
                {
                    // store aggregate
                    int affectedRows = 0;
                    var aggregate = await conn
                        .QuerySingleOrDefaultAsync<Aggregate>(
                            "select * from WorkshopPlanning where Id = @Id", 
                            new { Id = planningId },
                            transaction);

                    if (aggregate != null)
                    {
                        // update existing aggregate
                        affectedRows = await conn.ExecuteAsync(
                            @"update WorkshopPlanning
                              set [CurrentVersion] = @NewVersion
                              where [Id] = @Id
                              and [CurrentVersion] = @CurrentVersion;",
                            new { 
                                Id = planningId, 
                                NewVersion = newVersion,
                                CurrentVersion = originalVersion
                            },
                            transaction);
                    }
                    else
                    {
                        // insert new aggregate
                        affectedRows = await conn.ExecuteAsync(
                            "insert WorkshopPlanning ([Id], [CurrentVersion]) values (@Id, @CurrentVersion)",
                            new { Id = planningId, CurrentVersion = newVersion },
                            transaction);
                    }

                    // check concurrency
                    if (affectedRows == 0)
                    {
                        transaction.Rollback();
                        throw new ConcurrencyException();
                    }

                    // store events
                    int eventVersion = originalVersion;
                    foreach (var e in newEvents)
                    {
                        eventVersion++;
                        await conn.ExecuteAsync(
                            @"insert WorkshopPlanningEvent ([Id], [Version], [Timestamp], [MessageType], [EventData])
                              values (@Id, @NewVersion, @Timestamp, @MessageType,@EventData);",
                            new { 
                                Id = planningId, 
                                NewVersion = eventVersion,
                                Timestamp = DateTime.Now,
                                MessageType = e.MessageType,
                                EventData = SerializeEventData(e) 
                            }, transaction);
                    }

                    // commit
                    transaction.Commit();
                }
            }
        }

        public void EnsureDatabase()
        {
            // init db
            using (SqlConnection conn = new SqlConnection(_connectionString.Replace("WorkshopManagementEventStore", "master")))
            {
                Console.WriteLine("Ensure database exists");
                
                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(5, r => TimeSpan.FromSeconds(5), (ex, ts) => 
                        { Console.WriteLine("Error connecting to DB. Retrying in 5 sec."); })
                    .Execute(() => conn.Open());

                // create database
                string sql = "if DB_ID('WorkshopManagementEventStore') IS NULL CREATE DATABASE WorkshopManagementEventStore;";
                conn.Execute(sql);

                // create tables
                conn.ChangeDatabase("WorkshopManagementEventStore");
                sql = @" 
                    if OBJECT_ID('WorkshopPlanning') IS NULL 
                    CREATE TABLE WorkshopPlanning (
                        [Id] varchar(50) NOT NULL,
                        [CurrentVersion] int NOT NULL,
                    PRIMARY KEY([Id]));
                   
                    if OBJECT_ID('WorkshopPlanningEvent') IS NULL
                    CREATE TABLE WorkshopPlanningEvent (
                        [Id] varchar(50) NOT NULL REFERENCES WorkshopPlanning([Id]),
                        [Version] int NOT NULL,
                        [Timestamp] datetime2(7) NOT NULL,
                        [MessageType] varchar(75) NOT NULL,
                        [EventData] text,
                    PRIMARY KEY([Id], [Version]));";
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// Get events for a certain aggregate.
        /// </summary>
        /// <param name="planningId">The id of the planning.</param>
        /// <param name="conn">The SQL connection to use.</param>
        /// <returns></returns>
        private async Task<IEnumerable<Event>> GetAggregateEvents(string planningId, SqlConnection conn)
        {
            IEnumerable<AggregateEvent> aggregateEvents = await conn
                .QueryAsync<AggregateEvent>(
                    "select * from WorkshopPlanningEvent where Id = @Id order by [Version]",
                    new { Id = planningId });

            List<Event> events = new List<Event>();
            foreach (var aggregateEvent in aggregateEvents)
            {
                events.Add(DeserializeEventData(aggregateEvent.MessageType, aggregateEvent.EventData));
            }
            return events;
        }

        /// <summary>
        /// Serialize event-data to JSON.
        /// </summary>
        /// <param name="eventData">The event-data to serialize.</param>
        private string SerializeEventData(Event eventData)
        {
            return JsonConvert.SerializeObject(eventData, _serializerSettings);
        }

        /// <summary>
        /// Deserialize event-data from JSON.
        /// </summary>
        /// <param name="messageType">The message-type of the event.</param>
        /// <param name="eventData">The event-data JSON to deserialize.</param>
        private Event DeserializeEventData(string messageType, string eventData)
        {
            Type eventType = Type.GetType($"Pitstop.WorkshopManagementAPI.Events.{messageType}");
            JObject obj = JsonConvert.DeserializeObject<JObject>(eventData, _serializerSettings);
            return obj.ToObject(eventType) as Event;
        }
    }
}
