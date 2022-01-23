namespace Pitstop.WorkshopManagementAPI.Mappers;

public static class Mappers
{
    public static MaintenanceJobPlanned MapToMaintenanceJobPlanned(this PlanMaintenanceJob source) => new MaintenanceJobPlanned(
        Guid.NewGuid(),
        source.JobId,
        source.StartTime,
        source.EndTime,
        source.CustomerInfo,
        source.VehicleInfo,
        source.Description
    );

    public static MaintenanceJobFinished MapToMaintenanceJobFinished(this FinishMaintenanceJob source) => new MaintenanceJobFinished
    (
        Guid.NewGuid(),
        source.JobId,
        source.StartTime,
        source.EndTime,
        source.Notes
    );

    public static WorkshopPlanningDTO MapToDTO(this WorkshopPlanning source) =>
        new WorkshopPlanningDTO
        {
            Date = (DateTime)source.Id,
            Jobs = source.Jobs.Select(j => j.MapToDTO()).ToList(),
        };

    public static MaintenanceJobDTO MapToDTO(this MaintenanceJob source) =>
        new MaintenanceJobDTO
        {
            Id = source.Id,
            StartTime = source.PlannedTimeslot.StartTime,
            EndTime = source.PlannedTimeslot.EndTime,
            Vehicle = source.Vehicle.MapToDTO(),
            Customer = source.Customer.MapToDTO(),
            Description = source.Description,
            ActualStartTime = source?.ActualTimeslot?.StartTime,
            ActualEndTime = source?.ActualTimeslot?.EndTime,
            Notes = source.Notes
        };

    public static VehicleDTO MapToDTO(this Vehicle source) =>
        new VehicleDTO
        {
            LicenseNumber = source.Id.Value,
            Brand = source.Brand,
            Type = source.Type
        };

    public static CustomerDTO MapToDTO(this Customer source) =>
        new CustomerDTO
        {
            CustomerId = source.Id,
            Name = source.Name,
            TelephoneNumber = source.TelephoneNumber
        };
}