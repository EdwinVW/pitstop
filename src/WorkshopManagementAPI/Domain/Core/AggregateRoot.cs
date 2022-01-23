namespace Pitstop.WorkshopManagementAPI.Domain.Core;

/// <summary>
/// Represents an aggregate-root of a domain aggregate (DDD). An aggregate-root is always an entity.
/// </summary>
/// <typeparam name="TId">The type of the Id of the entity.</typeparam>
/// <remarks>In a real-world project, this class should be shared over domains in a NuGet package.</remarks>
public abstract class AggregateRoot<TId> : Entity<TId>
{
    /// <summary>
    /// The list of events that occur while handling commands.
    /// </summary>
    private readonly List<Event> _events;

    /// <summary>
    /// Indication whether the aggregate is replaying events (true) or not (false).
    /// </summary>
    private bool IsReplaying { get; set; } = false;

    /// <summary>
    /// The current version after handling any commands.
    /// </summary>
    public int Version { get; private set; }

    /// <summary>
    /// The original version of the aggregate after replaying all events in the event-store.
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
            When(e);
            OriginalVersion++;
            Version++;
        }
        IsReplaying = false;
    }

    /// <summary>
    /// Get the list of events that occurred while handling commands.
    /// </summary>
    public IEnumerable<Event> GetEvents()
    {
        return _events;
    }

    /// <summary>
    /// Let the aggregate handle an event and save it in the list of events 
    /// so it can be used outside the aggregate (persisted, published on a bus, ...).
    /// </summary>
    /// <param name="event">The event to handle.</param>
    /// <remarks>Use GetEvents to retrieve the list of events.</remarks>
    protected void RaiseEvent(Event @event)
    {
        // let the derived aggregate handle the event
        When(@event);

        // save the event so it can be published outside the aggregate
        _events.Add(@event);
        Version += 1;
    }

    /// <summary>
    /// Clear the list of events that occurred while handling a command.
    /// </summary>
    public void ClearEvents()
    {
        _events.Clear();
    }

    /// <summary>
    /// Handle a specific event. Derived classes should  implement this method 
    ///for every event type.
    /// </summary>
    /// <param name="@event">The event to handle.</param>
    /// <remarks>Because the parameter type of the specified event is dynamic, 
    /// the appropriate overload of the When method is called.</remarks>
    protected abstract void When(dynamic @event);
}