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
        void EnsureDatabase();
        Task<WorkshopPlanning> GetWorkshopPlanningAsync(DateTime date);
        Task SaveWorkshopPlanningAsync(string planningId, int originalVersion, int newVersion, IEnumerable<Event> newEvents);
    }
}
