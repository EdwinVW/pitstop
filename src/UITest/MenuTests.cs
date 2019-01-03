using System;
using Xunit;
using Xunit.Abstractions;
using Pitstop.UITest.PageModel;

namespace Pitstop.UITest
{
    public class MenuTests
    {
        private readonly ITestOutputHelper _output;

        public MenuTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void MainMenu_Home()
        {
            // arrange
            Guid testrunId = Guid.NewGuid();
            PitstopApp pitstop = new PitstopApp(testrunId, TestConstants.PitstopStartUrl);
            var homePage = pitstop.Start();

            // act
            var page = pitstop.Menu
                .Home();

            // assert
            Assert.True(page.IsActive());

            // cleanup
            pitstop.Stop();
        }

        [Fact]
        public void MainMenu_CustomerManagement()
        {
            // arrange
            Guid testrunId = Guid.NewGuid();
            PitstopApp pitstop = new PitstopApp(testrunId, TestConstants.PitstopStartUrl);
            var homePage = pitstop.Start();

            // act
            var page = pitstop.Menu
                .CustomerManagement();

            // assert
            Assert.True(page.IsActive());

            // cleanup
            pitstop.Stop();
        }

        [Fact]
        public void MainMenu_VehicleManagement()
        {
            // arrange
            Guid testrunId = Guid.NewGuid();
            PitstopApp pitstop = new PitstopApp(testrunId, TestConstants.PitstopStartUrl);
            var homePage = pitstop.Start();

            // act
            var page = pitstop.Menu
                .VehicleManagement();

            // assert
            Assert.True(page.IsActive());

            // cleanup
            pitstop.Stop();
        }

        [Fact]
        public void MainMenu_About()
        {
            // arrange
            Guid testrunId = Guid.NewGuid();
            PitstopApp pitstop = new PitstopApp(testrunId, TestConstants.PitstopStartUrl);
            var homePage = pitstop.Start();

            // act
            var page = pitstop.Menu
                .About();

            // assert
            Assert.True(page.IsActive());

            // cleanup
            pitstop.Stop();
        }        
    }
}
