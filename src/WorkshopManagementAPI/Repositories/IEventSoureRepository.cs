using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitstop.WorkshopManagementAPI.Repositories
{
    public interface IEventSourceRepository<T>
    {
        void EnsureDatabase();
        Task<T> GetByIdAsync(string id);
        Task SaveAsync(string id, int originalVersion, int newVersion, IEnumerable<Event> newEvents);
    }
}
