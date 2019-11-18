using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Domain;
using Pitstop.WorkshopManagementAPI.Repositories;

namespace WorkshopManagementAPI.CommandHandlers
{
    public class UpdateMaintenanceJobCommandHandler : IUpdateMaintenanceJobCommandHandler
    {
        private readonly IMessagePublisher messagePublisher;
        private readonly IWorkshopPlanningRepository planningRepository;

        public UpdateMaintenanceJobCommandHandler(IMessagePublisher messagePublisher, IWorkshopPlanningRepository planningRepository)
        {
            this.messagePublisher = messagePublisher;
            this.planningRepository = planningRepository;
        }

        public async Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, UpdateMaintenanceJob command)
        { 
            // get or create workshop-planning
            WorkshopPlanning planning = await planningRepository.GetWorkshopPlanningAsync(planningDate);

            if (planning == null)
                return null;

            // handle command
            planning.UpdateMaintenanceJob(command);

            // persist
            IEnumerable<Event> events = planning.GetEvents();
            await planningRepository.SaveWorkshopPlanningAsync(
                planning.Id, planning.OriginalVersion, planning.Version, events);

            // publish event
            foreach (var e in events)
            {
                await messagePublisher.PublishMessageAsync(e.MessageType, e, "");
            }

            // return result
            return planning;
        }
    }
}