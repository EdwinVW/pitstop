using Pitstop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.Models
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

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Vehicle")]
        public Vehicle Vehicle { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public Customer Customer { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Started at")]
        [DataType(DataType.Time)]
        public DateTime? ActualStartTime { get; set; }

        [Display(Name = "Completed at")]
        [DataType(DataType.Time)]
        public DateTime? ActualEndTime { get; set; }

        [Display(Name = "Mechanic notes")]
        public string Notes { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime WorkshopPlanningDate { get; set; }
    }
}
