using System.Collections.Generic;

namespace Pitstop.WorkshopManagementAPI.Domain.Core
{
    public static class ObjectAsEnumerableWrapper
    {
        /// <summary>
        /// Wrap an object in an Enumerable. This is a convenience method that is handy when 
        /// you need to return an IEnumerable but only have 1 item. You don't have to wrap 
        /// the item yourself.
        /// </summary>
        /// <param name="item">The item to wrap.</param>
        /// <typeparam name="T">The type of the item to wrap.</typeparam>
        /// <returns>An IEnumerable of type T.</returns>
        /// <remarks>In a real-world project, this class should be shared over domains in a NuGet package.</remarks>
        public static IEnumerable<T> AsEnumerable<T>(this T item)
        {
            yield return item;
        }
    }        
}