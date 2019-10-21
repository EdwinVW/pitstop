using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Domain.BusinessRules;
using Pitstop.WorkshopManagementAPI.Domain.Core;
using Pitstop.WorkshopManagementAPI.Domain.Entities;
using Pitstop.WorkshopManagementAPI.Domain.Exceptions;
using Pitstop.WorkshopManagementAPI.Domain.ValueObjects;
using Pitstop.WorkshopManagementAPI.Events;
using Pitstop.WorkshopManagementAPI.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitstop.WorkshopManagementAPI.Domain
{
    public class WorkshopPlanning : AggregateRoot<WorkshopPlanningId>
    {
        /// <summary>
        /// The list of maintenance-jobs for this day. 
        /// </summary>
        public List<MaintenanceJob> Jobs { get; private set; }

        public WorkshopPlanning(DateTime date) : base(new WorkshopPlanningId(date)) { }

        public WorkshopPlanning(DateTime date, IEnumerable<Event> events) : base(new WorkshopPlanningId(date), events) { }

        /// <summary>
        /// Creates a new instance of a workshop-planning for the specified date.
        /// </summary>
        /// <param name="date">The date to create the planning for.</param>
        public static WorkshopPlanning Create(DateTime date)
        {
            WorkshopPlanning planning = new WorkshopPlanning(date);
            WorkshopPlanningCreated e = new WorkshopPlanningCreated(Guid.NewGuid(), date);
            planning.RaiseEvent(e);
            return planning;
        }

        public void PlanMaintenanceJob(PlanMaintenanceJob command)
        {
            // check business rules
            this.PlannedMaintenanceJobShouldFallWithinOneBusinessDay(command);
            this.NumberOfParallelMaintenanceJobsMustNotExceedAvailableWorkStations(command);
            this.NumberOfParallelMaintenanceJobsOnAVehicleIsOne(command);

            // handle event
            MaintenanceJobPlanned e = command.MapToMaintenanceJobPlanned();
            RaiseEvent(e);
        }

        public void FinishMaintenanceJob(FinishMaintenanceJob command)
        {
            // find job
            MaintenanceJob job = Jobs.FirstOrDefault(j => j.Id == command.JobId);
            if (job == null)
            {
                throw new MaintenanceJobNotFoundException($"Maintenance job with id {command.JobId} found.");
            }

            // check business rules
            job.FinishedMaintenanceJobCanNotBeFinished();

            // handle event
            MaintenanceJobFinished e = command.MapToMaintenanceJobFinished();
            RaiseEvent(e);
        }

        /// <summary>
        /// Handles an event and updates the aggregate version.
        /// </summary>
        /// <remarks>Caution: this handles is also called while replaying events to restore state.
        /// So, do not execute any checks that could fail or introduce any side-effects in this handler.</remarks>
        protected override void When(dynamic @event)
        {
            Handle(@event);
        }

        private void Handle(WorkshopPlanningCreated e)
        {
            Jobs = new List<MaintenanceJob>();
        }

        private void Handle(MaintenanceJobPlanned e)
        {
            MaintenanceJob job = new MaintenanceJob();
            Customer customer = new Customer(e.CustomerInfo.Id, e.CustomerInfo.Name, e.CustomerInfo.TelephoneNumber);
            Vehicle vehicle = new Vehicle(e.VehicleInfo.LicenseNumber, e.VehicleInfo.Brand, e.VehicleInfo.Type, customer.Id);
            job.Plan(e.JobId, e.StartTime, e.EndTime, vehicle, customer, e.Description);
            Jobs.Add(job);
        }

        private void Handle(MaintenanceJobFinished e)
        {
            MaintenanceJob job = Jobs.FirstOrDefault(j => j.Id == e.JobId);
            job.Finish(e.StartTime, e.EndTime, e.Notes);
        }
    }
}
