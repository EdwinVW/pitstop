using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementAPI.Domain;
using Pitstop.WorkshopManagementAPI.Repositories;
using WorkshopManagementAPI.Commands;

namespace WorkshopManagementAPI.CommandHandlers
{
    public class RegisterPlanningCommandHandler : IRegisterPlanningCommandHandler
    {
        IMessagePublisher _messagePublisher;
        IWorkshopPlanningRepository _planningRepo;

        public RegisterPlanningCommandHandler(IMessagePublisher messagePublisher, IWorkshopPlanningRepository planningRepo)
        {
            _messagePublisher = messagePublisher;
            _planningRepo = planningRepo;
        }

        public async Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, RegisterPlanning command)
        {
            WorkshopPlanning planning = new WorkshopPlanning();

            // handle command
            IEnumerable<Event> events = planning.RegisterPlanning(command);

            // persist
            await _planningRepo.SaveWorkshopPlanningAsync(planning, events);

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