using System;
using System.Collections.Generic;
using System.Linq;
using Pitstop.Infrastructure.Messaging;

namespace Pitstop.WorkshopManagementAPI.Domain.Core
{
    /// <summary>
    /// Represents an aggregate-root of a domain aggregate (DDD). An aggregate-root is always an entity.
    /// </summary>
    /// <typeparam name="TId">The type of the Id of the entity.</typeparam>
    /// <remarks>In a real-world project, this class should be shared over domains in a NuGet package.</remarks>
    public class AggregateRoot<TId> : Entity<TId>
    {
        /// <summary>
        /// The list of events that occur while handling commands.
        /// </summary>
        private List<Event> _events;

        /// <summary>
        /// Indication whether the aggregate is replaying events (true) or not (false).
        /// </summary>
        private bool IsReplaying { get; set; } = false;
                
        /// <summary>
        /// The original version of the aggregate after replaying all events in the event-store.
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// The current version after handling any commands.
        /// </summary>
        public int OriginalVersion { get; private set; }

        /// <summary>
        /// Constructor for creating an empty aggregate.
        /// </summary>
        /// <param name="id">The unique id of the aggregate-root.</param>
        public AggregateRoot(TId id) : base(id)
        {
            OriginalVersion = 0;
            Version = 0;
            _events = new List<Event>();
        }

        /// <summary>
        /// Constructor for creating an aggregate of which the state is intialized by 
        /// replaying the list of events specified.
        /// </summary>
        /// <param name="id">The unique Id of the aggregate.</param>
        /// <param name="events">The events to replay.</param>
        public AggregateRoot(TId id, IEnumerable<Event> events) : this(id)
        {
            IsReplaying = true;
            foreach (Event e in events)
            {
                HandleEvent(e);
                OriginalVersion++;
                Version++;
            }
            IsReplaying = false;
        }

        /// <summary>
        /// Get the list of events that occured while handling commands.
        /// </summary>
        public IEnumerable<Event> GetEvents()
        {
            return _events;
        }

        /// <summary>
        /// Add an (or more) event(s) that occured while handling an event 
        /// to the list of events.
        /// </summary>
        /// <param name="events">The list of events to add.</param>
        protected void AddEvents(IEnumerable<Event> events)
        {
            _events.AddRange(events);
            Version += events.Count();
        }

        /// <summary>
        /// Clear the list of events that occorred while handling a command.
        /// </summary>
        public void ClearEvents()
        {
            _events.Clear();
        }

        /// <summary>
        /// Handle a specific event. Derived classes should overide this method and implement 
        /// the handling of different types of events.
        /// </summary>
        /// <param name="eventToHandle">The event to handle.</param>
        protected virtual void HandleEvent(Event eventToHandle)
        {
            throw new NotImplementedException();
        }
    }
}