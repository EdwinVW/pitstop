using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pitstop.WorkshopManagementAPI.Domain;
using Newtonsoft.Json;
using Dapper;
using Polly;
using Pitstop.WorkshopManagementAPI.Repositories.Model;
using Pitstop.Infrastructure.Messaging;
using Newtonsoft.Json.Linq;
using Pitstop.WorkshopManagementAPI.Events;
using Newtonsoft.Json.Converters;
using System.Data.SqlClient;

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
            //_serializerSettings.TypeNameHandling = TypeNameHandling.All;
            _serializerSettings.Formatting = Formatting.Indented;
            _serializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
        }

        public SqlServerWorkshopPlanningRepository(string connectionString)
        {
            _connectionString = connectionString;

            // init db
            using (SqlConnection conn = new SqlConnection(_connectionString.Replace("WorkshopManagementEventStore", "master")))
            {
                conn.Open();

                // create database
                string sql = "if DB_ID('WorkshopManagementEventStore') IS NULL CREATE DATABASE WorkshopManagementEventStore;";

                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(5, r => TimeSpan.FromSeconds(5), (ex, ts) => 
                        { Console.WriteLine("Error connecting to DB. Retrying in 5 sec."); })
                    .Execute(() => conn.Execute(sql));

                // create tables
                conn.ChangeDatabase("WorkshopManagementEventStore");
                sql =
                    "if OBJECT_ID('WorkshopPlanning') IS NULL " +
                    "CREATE TABLE WorkshopPlanning (" +
                    "  Id varchar(50) NOT NULL," +
                    "  Version int NOT NULL," +
                    "  EventData text," +
                    "  PRIMARY KEY(Id));";

                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(5, r => TimeSpan.FromSeconds(5), (ex, ts) => 
                        { Console.WriteLine("Error connecting to DB. Retrying in 5 sec."); })
                    .Execute(() => conn.Execute(sql));
            }
        }

        public async Task<WorkshopPlanning> GetWorkshopPlanningAsync(DateTime date)
        {
            WorkshopPlanning planning = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                Aggregate aggregate = await conn
                    .QueryFirstOrDefaultAsync<Aggregate>("select * from WorkshopPlanning where Id = @Id", new { Id = date.ToString("yyyy-MM-dd") });

                if (aggregate != null)
                {
                    IEnumerable<Event> events = DeserializeState(aggregate.EventData);
                    planning = new WorkshopPlanning(events);
                }
            }

            return planning;
        }

        public async Task SaveWorkshopPlanningAsync(WorkshopPlanning planning, IEnumerable<Event> newEvents)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                Aggregate aggregate = await conn
                   .QueryFirstOrDefaultAsync<Aggregate>("select * from WorkshopPlanning where Id = @Id", new { Id = planning.Id });

                if (aggregate != null)
                {
                    // add new events to existing events
                    var aggregateEvents = DeserializeState(aggregate.EventData);
                    aggregateEvents.AddRange(newEvents);

                    // determine versions
                    int currentVersion = planning.Version;
                    int newVersion = aggregateEvents.Count;

                    // update aggregate
                    await conn.ExecuteAsync(
                        "update WorkshopPlanning set Version = @NewVersion, EventData = @EventData where Id = @Id and Version = @CurrentVersion",
                        new { Id = planning.Id, CurrentVersion = currentVersion, NewVersion = newVersion, EventData = SerializeState(aggregateEvents) });
                }
                else
                {
                    // insert new aggregate
                    var aggregateEvents = new List<Event>(newEvents);
                    await conn.ExecuteAsync(
                        "insert WorkshopPlanning (Id, Version, EventData) values (@Id, @Version, @EventData)",
                        new { Id = planning.Id, Version = aggregateEvents.Count, EventData = SerializeState(aggregateEvents) });
                }
            }
        }

        /// <summary>
        /// Serialize events to JSON.
        /// </summary>
        /// <param name="state">The events to serialize.</param>
        private string SerializeState(List<Event> state)
        {
            JObject[] jsonData = state.Select(e => JObject.FromObject(e)).ToArray();
            return JsonConvert.SerializeObject(state, _serializerSettings);
        }

        /// <summary>
        /// Deserialize state from JSON.
        /// </summary>
        /// <param name="state">The JSON to deserialize.</param>
        private List<Event> DeserializeState(string state)
        {
            List<Event> events = new List<Event>();

            // parse objects
            JObject[] objects = JsonConvert.DeserializeObject<JObject[]>(state, _serializerSettings);
            foreach(JObject obj in objects)
            {
                string messageTypeString = obj.Value<string>("MessageType");
                MessageTypes messageType = MessageTypes.Unknown;
                Enum.TryParse<MessageTypes>(messageTypeString, true, out messageType);
                switch (messageType)
                {
                    case MessageTypes.WorkshopPlanningCreated:
                        events.Add(obj.ToObject<WorkshopPlanningCreated>());
                        break;
                    case MessageTypes.MaintenanceJobPlanned:
                        events.Add(obj.ToObject<MaintenanceJobPlanned>());
                        break;
                    case MessageTypes.MaintenanceJobFinished:
                        events.Add(obj.ToObject<MaintenanceJobFinished>());
                        break;
                    default:
                        break;
                }
            }

            return events;
        }
    }
}
