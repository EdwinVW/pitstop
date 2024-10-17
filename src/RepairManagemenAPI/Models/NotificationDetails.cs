namespace Pitstop.RepairManagemenAPI.Models
{
    public class NotificationDetails
    {
        public string CustomerEmail { get; set; }
        public VehiclePart[] Parts { get; set; }
        public decimal LaborCost { get; set; }
        public decimal TotalCost
        {
            get
            {
                return Parts.Sum(p => p.PartCost) + LaborCost;
            }
        }
    }
}