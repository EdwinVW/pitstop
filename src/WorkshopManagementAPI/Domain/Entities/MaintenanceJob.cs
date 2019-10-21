using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.WorkshopManagementAPI.Domain.Entities
{
    public class MaintenanceJob
    {
        public Guid Id { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public Vehicle Vehicle { get; private set; }
        public Customer Customer { get; private set; }
        public string Description { get; private set; }
        public DateTime? ActualStartTime { get; private set; }
        public DateTime? ActualEndTime { get; private set; }
        public string Notes { get; private set; }
        public string Status => (!ActualStartTime.HasValue && !ActualEndTime.HasValue) ? "Planned" : "Completed";

        public void Plan(Guid id, DateTime startTime, DateTime endTime, Vehicle vehicle, Customer customer, string description)
        {
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
            Vehicle = vehicle;
            Customer = customer;
            Description = description;
        }

        public void Finish(DateTime actualStartTime, DateTime actualEndTime, string notes)
        {
            ActualStartTime = actualStartTime;
            ActualEndTime = actualEndTime;
            Notes = notes;
        }

    }
}
