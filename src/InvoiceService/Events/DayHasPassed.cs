using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.InvoiceService.Events
{
    public class DayHasPassed : Event
    {
        public DayHasPassed(Guid messageId) : base(messageId)
        {
        }
    }
}
