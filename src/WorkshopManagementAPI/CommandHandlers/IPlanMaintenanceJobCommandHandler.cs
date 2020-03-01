using System;
using System.Threading.Tasks;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Domain.Entities;

namespace WorkshopManagementAPI.CommandHandlers
{
    public interface IPlanMaintenanceJobCommandHandler
    {
        Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, PlanMaintenanceJob command);
    }
}