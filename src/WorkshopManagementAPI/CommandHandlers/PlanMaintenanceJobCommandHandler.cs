using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Domain.Entities;
using Pitstop.WorkshopManagementAPI.Repositories;

namespace WorkshopManagementAPI.CommandHandlers
{
    public class PlanMaintenanceJobCommandHandler : IPlanMaintenanceJobCommandHandler
    {
        IMessagePublisher _messagePublisher;
        IWorkshopPlanningRepository _planningRepo;

        public PlanMaintenanceJobCommandHandler(IMessagePublisher messagePublisher, IWorkshopPlanningRepository planningRepo)
        {
            _messagePublisher = messagePublisher;
            _planningRepo = planningRepo;
        }

        public async Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, PlanMaintenanceJob command)
        {
            // get or create workshop-planning
            WorkshopPlanning planning = await _planningRepo.GetWorkshopPlanningAsync(planningDate);
            if (planning == null)
            {
                planning = WorkshopPlanning.Create(planningDate);
            }

            // handle command
            planning.PlanMaintenanceJob(command);

            // persist
            IEnumerable<Event> events = planning.GetEvents();
            await _planningRepo.SaveWorkshopPlanningAsync(
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