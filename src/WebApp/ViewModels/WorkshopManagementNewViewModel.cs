using Microsoft.AspNetCore.Mvc.Rendering;
using Pitstop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.ViewModels
{
    public class WorkshopManagementNewViewModel
    {
        public DateTime Date { get; set; }

        public MaintenanceJob MaintenanceJob { get; set; }

        public IEnumerable<SelectListItem> Vehicles { get; set; }

        [Required(ErrorMessage = "Vehicle is required")]
        public string SelectedVehicleLicenseNumber { get; set; }

        public string Error { get; set; }
    }
}
