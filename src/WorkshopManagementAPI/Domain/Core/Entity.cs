namespace Pitstop.WorkshopManagementAPI.Domain.Core;

/// <summary>
/// Represents an Entity in the domain (DDD).
/// </summary>
/// <typeparam name="TId">The type of the Id of the entity.</typeparam>
/// <remarks>In a real-world project, this class should be shared over domains in a NuGet package.</remarks>
public class Entity<TId>
{
    public TId Id { get; private set; }

    public Entity(TId id)
    {
        Id = id;
    }
}