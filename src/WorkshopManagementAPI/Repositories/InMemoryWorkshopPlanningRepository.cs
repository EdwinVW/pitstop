using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pitstop.WorkshopManagementAPI.Domain;
using Newtonsoft.Json;
using Pitstop.Infrastructure.Messaging;

namespace Pitstop.WorkshopManagementAPI.Repositories
{
    public class InMemoryWorkshopPlanningRepository : IWorkshopPlanningRepository
    {
        private static readonly JsonSerializerSettings _serializerSettings;
        private static readonly Dictionary<DateTime, string> _store = new Dictionary<DateTime, string>();

        static InMemoryWorkshopPlanningRepository()
        {
            _serializerSettings = new JsonSerializerSettings();
            _serializerSettings.TypeNameHandling = TypeNameHandling.All;
            _serializerSettings.Formatting = Formatting.Indented;
        }

        public Task<WorkshopPlanning> GetWorkshopPlanningAsync(DateTime date)
        {
            if (_store.ContainsKey(date.Date))
            {
                string json = _store[date.Date];
                List<Event> events = JsonConvert.DeserializeObject<List<Event>>(json, _serializerSettings);
                return Task.FromResult(new WorkshopPlanning(events));
            }
            return Task.FromResult(default(WorkshopPlanning));
        }

        public Task SaveWorkshopPlanningAsync(WorkshopPlanning planning, IEnumerable<Event> newEvents)
        {
            if (_store.ContainsKey(planning.Date.Date))
            {
                string json = _store[planning.Date.Date];
                List<Event> currentEvents = JsonConvert.DeserializeObject<List<Event>>(json, _serializerSettings);
                currentEvents.AddRange(newEvents);
                json = JsonConvert.SerializeObject(currentEvents, _serializerSettings);
                _store[planning.Date.Date] = json;
                return Task.CompletedTask;
            }
            else
            {
                List<Event> events = new List<Event>(newEvents);
                string json = JsonConvert.SerializeObject(events, _serializerSettings);
                _store.Add(planning.Date.Date, json);
            }

            return Task.CompletedTask;
        }
    }
}
