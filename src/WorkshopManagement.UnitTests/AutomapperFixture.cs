using Pitstop.WorkshopManagementAPI;
using Xunit;

namespace WorkshopManagement.UnitTests
{
    /// <summary>
    /// Defines a fixture for classes that need an initialized Automapper instance.
    /// </summary>
    public class AutomapperFixture
    {
        public AutomapperFixture()
        {
            AutomapperConfigurator.SetupAutoMapper();
        }
    }
    
    /// <summary>
    /// Collection definition. Decorate all classes that need an initialized Automapper 
    /// instance with a Collection("AutomapperCollection") attribute.
    /// </summary>
    [CollectionDefinition("AutomapperCollection")]
    public class AutomapperCollection : ICollectionFixture<AutomapperFixture>
    {
    }
}