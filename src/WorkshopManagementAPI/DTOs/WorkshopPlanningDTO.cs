using System;
using System.Collections.Generic;
using Pitstop.WorkshopManagementAPI.Domain.Entities;

namespace Pitstop.WorkshopManagementAPI.DTOs
{
    public class WorkshopPlanningDTO
    {
        public DateTime Date { get; set; }
        public List<MaintenanceJobDTO> Jobs { get; set; }
    }
}
