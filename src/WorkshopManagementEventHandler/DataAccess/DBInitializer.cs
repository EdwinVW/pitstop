using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.WorkshopManagementEventHandler.DataAccess
{
    public static class DBInitializer
    {
        public static void Initialize(WorkshopManagementDBContext context)
        {
            context.Database.Migrate();
        }
    }
}
