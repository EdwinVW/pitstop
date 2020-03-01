using System;
using Pitstop.WorkshopManagementAPI.Domain.ValueObjects;

namespace Pitstop.WorkshopManagementAPI.Domain.Entities
{
    public class MaintenanceJob
    {
        public Guid Id { get; private set; }
        public Timeslot PlannedTimeslot { get; private set; }
        public Vehicle Vehicle { get; private set; }
        public Customer Customer { get; private set; }
        public string Description { get; private set; }
        public Timeslot ActualTimeslot { get; private set; }
        public string Notes { get; private set; }
        public string Status => (ActualTimeslot == null) ? "Planned" : "Completed";

        public void Plan(Guid id, Timeslot timeslot, Vehicle vehicle, Customer customer, string description)
        {
            Id = id;
            PlannedTimeslot = timeslot;
            Vehicle = vehicle;
            Customer = customer;
            Description = description;
        }

        public void Finish(Timeslot actualTimeslot, string notes)
        {

            ActualTimeslot = actualTimeslot;
            Notes = notes;
        }

    }
}
