using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Domain.Entities;
using Pitstop.WorkshopManagementAPI.Domain.ValueObjects;
using Pitstop.WorkshopManagementAPI.Repositories;

namespace WorkshopManagementAPI.CommandHandlers
{
    public class PlanMaintenanceJobCommandHandler : IPlanMaintenanceJobCommandHandler
    {
        IMessagePublisher _messagePublisher;
        IEventSourceRepository<WorkshopPlanning> _planningRepo;

        public PlanMaintenanceJobCommandHandler(IMessagePublisher messagePublisher, IEventSourceRepository<WorkshopPlanning> planningRepo)
        {
            _messagePublisher = messagePublisher;
            _planningRepo = planningRepo;
        }

        public async Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, PlanMaintenanceJob command)
        {
            // get or create workshop-planning
            var aggregateId = WorkshopPlanningId.Create(planningDate);
            var planning = await _planningRepo.GetByIdAsync(aggregateId);
            if (planning == null)
            {
                planning = WorkshopPlanning.Create(planningDate);
            }

            // handle command
            planning.PlanMaintenanceJob(command);

            // persist
            IEnumerable<Event> events = planning.GetEvents();
            await _planningRepo.SaveAsync(
                planning.Id, planning.OriginalVersion, planning.Version, events);

            // publish event
            foreach (var e in events)
            {
                await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
            }

            // return result
            return planning;
        }
    }
}