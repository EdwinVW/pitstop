using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Domain;
using System;
using System.Threading.Tasks;

namespace WorkshopManagementAPI.CommandHandlers
{
    public interface IUpdateMaintenanceJobCommandHandler
    {
        Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, UpdateMaintenanceJob command);
    }
}