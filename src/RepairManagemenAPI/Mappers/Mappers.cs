using System;
using System.Linq;

namespace Pitstop.RepairManagementAPI.Mappers
{
    public static class Mappers
    {
        public static CustomerNotificationSent MapToCustomerNotificationSent(this SendCustomerNotification command) => 
            new CustomerNotificationSent
            (

            );

        public static NotificationDetails MapToNotificationDetails(this SendCustomerNotification command) => 
            new NotificationDetails
            {

            };
    }
}

