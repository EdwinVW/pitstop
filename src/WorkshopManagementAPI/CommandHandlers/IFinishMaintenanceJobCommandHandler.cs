using System;
using System.Threading.Tasks;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Domain.Entities;

namespace WorkshopManagementAPI.CommandHandlers
{
    public interface IFinishMaintenanceJobCommandHandler
    {
        Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, FinishMaintenanceJob command);
    }
}