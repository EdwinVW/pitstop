using Pitstop.WorkshopManagementAPI;
using Xunit;

namespace WorkshopManagement.UnitTests
{
    public class AutomapperFixture
    {
        public AutomapperFixture()
        {
            AutomapperConfigurator.SetupAutoMapper();
        }
    }
    
    [CollectionDefinition("AutomapperCollection")]
    public class AutpmapperCollection : ICollectionFixture<AutomapperFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}