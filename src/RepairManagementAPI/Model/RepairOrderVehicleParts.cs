namespace Pitstop.RepairManagementAPI.Model;

public class RepairOrderVehicleParts
{
    public Guid RepairOrderId { get; set; }
    public RepairOrder RepairOrder { get; set; }

    public Guid VehiclePartsId { get; set; }
    public VehicleParts VehicleParts { get; set; }
    
}