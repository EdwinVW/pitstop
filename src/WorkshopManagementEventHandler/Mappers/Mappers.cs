using Pitstop.WorkshopManagementEventHandler.Events;
using Pitstop.WorkshopManagementEventHandler.Model;

namespace Pitstop.WorkshopManagementEventHandler.Mappers
{
    public static class Mappers
    {
        public static Vehicle ToEntity(this VehicleUpdated e) => FromVehicleInfo((e.LicenseNumber, e.Brand, e.Type), e.OwnerId);
        public static Vehicle FromVehicleInfo(this (string LicenseNumber, string Brand, string Type) vehicleInfo, string OwnerId = null)
        {
            return new Vehicle()
            {
                LicenseNumber = vehicleInfo.LicenseNumber,
                Brand = vehicleInfo.Brand,
                OwnerId = OwnerId,
                Type = vehicleInfo.Type
            };
        }

        public static Customer ToEntity(this CustomerUpdated e) => FromCustomerInfo((e.MessageId.ToString(), e.Name, e.TelephoneNumber));

        public static Customer FromCustomerInfo(this (string Id, string Name, string TelephoneNumber) customerInfo)
        {
            return new Customer()
            {
                CustomerId = customerInfo.Id,
                Name = customerInfo.Name,
                TelephoneNumber = customerInfo.TelephoneNumber
            };
        }

        public static MaintenanceJob ToEntity(this MaintenanceJobUpdated e, Customer customer, Vehicle vehicle)
        {
            return new MaintenanceJob
            {
                Id = e.JobId,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                Customer = customer,
                Vehicle = vehicle,
                WorkshopPlanningDate = e.StartTime.Date,
                Description = e.Description
            };
        }
    }
}
