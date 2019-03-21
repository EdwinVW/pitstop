using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.WorkshopManagementAPI.Events
{
    public class WorkshopPlanningCreated : Event
    {
        public readonly DateTime Date;

        public WorkshopPlanningCreated(Guid messageId, DateTime date) : base(messageId)
        {
            Date = date;
        }
    }
}
