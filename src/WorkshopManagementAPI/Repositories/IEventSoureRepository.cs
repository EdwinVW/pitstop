using Pitstop.Infrastructure.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitstop.WorkshopManagementAPI.Repositories
{
    public interface IEventSourceRepository<T>
    {
        Task<T> GetByIdAsync(string id);
        Task SaveAsync(string id, int originalVersion, int newVersion, IEnumerable<Event> newEvents);
    }
}
