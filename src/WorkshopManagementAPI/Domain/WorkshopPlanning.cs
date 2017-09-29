using AutoMapper;
using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Domain.Exceptions;
using Pitstop.WorkshopManagementAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.WorkshopManagementAPI.Domain
{
    public class WorkshopPlanning
    {
        private const int MAX_PARALLEL_JOBS = 3;
        private bool IsReplaying { get; set; } = false;

        public DateTime Date { get; private set; }
        public List<MaintenanceJob> Jobs { get; private set; }
        public int Version { get; private set; }
        public string Id
        {
            get
            {
                return Date.ToString("yyy-MM-dd");
            }
        }

        public WorkshopPlanning()
        {

        }

        public WorkshopPlanning(IEnumerable<Event> events)
        {
            Version = 0;

            IsReplaying = true;
            foreach (Event e in events)
            {
                Handle((dynamic)e);
                Version++;
            }
            IsReplaying = false;
        }

        public IEnumerable<Event> Create(DateTime date)
        {
            List<Event> events = new List<Event>();
            WorkshopPlanningCreated e = new WorkshopPlanningCreated(Guid.NewGuid(), Date = date);
            events.AddRange(Handle(e));
            return events;
        }

        public IEnumerable<Event> PlanMaintenanceJob(PlanMaintenanceJob command)
        {
            // check business rules

            // maintenance jobs may not span multiple days
            if (command.StartTime.Date != command.EndTime.Date)
            {
                throw new BusinessRuleViolationException("Start-time and end-time of a Maintenance Job must be within a 1 day.");
            }

            // no more than 3 jobs can be planned at the same time (limited resources)
            if (Jobs.Count(j => j.StartTime >= command.StartTime && j.StartTime <= command.EndTime || 
                                j.EndTime >= command.StartTime && j.EndTime <= command.EndTime) > MAX_PARALLEL_JOBS)
            {
                throw new BusinessRuleViolationException($"Maintenancejob overlaps with more than {MAX_PARALLEL_JOBS} other jobs.");
            }

            // only 1 maintenance job can be executed on a vehicle during a certain time-slot
            if (Jobs.Any(j => j.Vehicle.LicenseNumber == command.VehicleInfo.LicenceNumber && 
                    (j.StartTime >= command.StartTime && j.StartTime <= command.EndTime ||
                    j.EndTime >= command.StartTime && j.EndTime <= command.EndTime) ))
            {
                throw new BusinessRuleViolationException($"Only 1 maintenance job can be executed on a vehicle during a certain time-slot.");
            }

            // handle event
            MaintenanceJobPlanned e = Mapper.Map<MaintenanceJobPlanned>(command);
            return Handle(e);
        }

        public IEnumerable<Event> FinishMaintenanceJob(FinishMaintenanceJob command)
        {
            // find job
            MaintenanceJob job = Jobs.FirstOrDefault(j => j.Id == command.JobId);
            if (job == null)
            {
                throw new MaintenanceJobNotFoundException($"Maintenance job with id {command.JobId} found.");
            }

            // handle event
            MaintenanceJobFinished e = Mapper.Map<MaintenanceJobFinished>(command);
            return Handle(e);
        }

        private IEnumerable<Event> Handle(WorkshopPlanningCreated e)
        {
            Jobs = new List<MaintenanceJob>();
            Date = e.Date.Date;

            return new List<Event>(new Event[] { e });
        }

        private IEnumerable<Event> Handle(MaintenanceJobPlanned e)
        {
            MaintenanceJob job = new MaintenanceJob();
            Customer customer = new Customer(e.CustomerInfo.Id, e.CustomerInfo.Name, e.CustomerInfo.TelephoneNumber);
            Vehicle vehicle = new Vehicle(e.VehicleInfo.LicenseNumber, e.VehicleInfo.Brand, e.VehicleInfo.Type, customer.CustomerId);
            job.Plan(e.JobId, e.StartTime, e.EndTime, vehicle, customer, e.Description);
            Jobs.Add(job);

            return new List<Event>(new Event[] { e });
        }

        private IEnumerable<Event> Handle(MaintenanceJobFinished e)
        {
            MaintenanceJob job = Jobs.FirstOrDefault(j => j.Id == e.JobId);
            job.Finish(e.StartTime, e.EndTime, e.Notes);

            return new List<Event>(new Event[] { e });
        }
    }
}
