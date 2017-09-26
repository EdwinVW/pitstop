using Pitstop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.ViewModels
{
    public class WorkshopManagementViewModel
    {
        public DateTime Date { get; set; }
        public List<MaintenanceJob> MaintenanceJobs { get; set; }
    }
}
