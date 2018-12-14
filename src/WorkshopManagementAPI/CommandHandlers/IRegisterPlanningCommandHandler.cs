using System;
using System.Threading.Tasks;
using Pitstop.WorkshopManagementAPI.Domain;
using WorkshopManagementAPI.Commands;

namespace WorkshopManagementAPI.CommandHandlers
{
    public interface IRegisterPlanningCommandHandler
    {
         Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, RegisterPlanning command);
    }
}