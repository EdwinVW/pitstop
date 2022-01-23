namespace Pitstop.WebApp.Models
{
    public class MaintenanceJob
    {
        public Guid Id { get; set; }

        public string Status
        {
            get
            {
                return ActualEndTime != null ? "Completed" : "Planned";
            }
        }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Vehicle")]
        public Vehicle Vehicle { get; set; }

        [Display(Name = "Customer")]
        public Customer Customer { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Started at")]
        public DateTime? ActualStartTime { get; set; }

        [Display(Name = "Completed at")]
        public DateTime? ActualEndTime { get; set; }

        [Display(Name = "Mechanic notes")]
        public string Notes { get; set; }

        [Display(Name = "Date")]
        public DateTime WorkshopPlanningDate { get; set; }
    }
}
