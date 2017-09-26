using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.WorkshopManagementAPI.Repositories
{
    public interface IWorkshopPlanningRepository
    {
        Task<WorkshopPlanning> GetWorkshopPlanningAsync(DateTime date);
        Task SaveWorkshopPlanningAsync(WorkshopPlanning planning, IEnumerable<Event> newEvents);
    }
}
