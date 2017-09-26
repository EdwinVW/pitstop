using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;

namespace Pitstop.WorkshopManagementAPI.Repositories
{
    public class EventStream
    {
        public DateTime Date { get; set; }
        public List<Event> Events { get; set; }
    }
}
