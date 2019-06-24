using System;
using AutoMapper;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Events;

namespace Pitstop.WorkshopManagementAPI
{
    public static class AutomapperConfigurator
    {
        public static void SetupAutoMapper()
        {
            // setup automapper
            var cfg = new AutoMapper.Configuration.MapperConfigurationExpression();
            cfg.CreateMap<PlanMaintenanceJob, MaintenanceJobPlanned>()
                .ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid()));
            cfg.CreateMap<FinishMaintenanceJob, MaintenanceJobFinished>()
                .ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid()));
            Mapper.Initialize(cfg);
        } 
    }
}