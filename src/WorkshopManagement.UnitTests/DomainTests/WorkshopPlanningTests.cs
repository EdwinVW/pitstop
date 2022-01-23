namespace WorkshopManagement.UnitTests.DomainTests;

[Collection("AutomapperCollection")]
public class WorkshopPlanningTests
{
    [Fact]
    public void Create_Should_Create_A_New_Instance()
    {
        // arrange
        DateTime date = DateTime.Today;

        // act
        WorkshopPlanning sut = WorkshopPlanning.Create(date);
        IEnumerable<Event> events = sut.GetEvents();

        // assert
        Assert.NotNull(sut);
        Assert.NotNull(sut.Id);
        Assert.Equal(date, sut.Id);
        Assert.Equal(0, sut.OriginalVersion);
        Assert.Equal(1, sut.Version);
        Assert.Empty(sut.Jobs);
        Assert.Collection(events, item0 => Assert.IsAssignableFrom<WorkshopPlanningCreated>(item0));
    }

    [Fact]
    public void Plan_MaintenanceJob_Should_Add_A_New_MaintenanceJob()
    {
        // arrange
        DateTime date = DateTime.Today;
        var initializingEvents = new Event[] {
                new WorkshopPlanningCreatedEventBuilder().WithDate(date).Build()
            };
        WorkshopPlanning sut = new WorkshopPlanning(date, initializingEvents);

        PlanMaintenanceJob command = new PlanMaintenanceJobCommandBuilder()
            .Build();

        // act
        sut.PlanMaintenanceJob(command);
        IEnumerable<Event> events = sut.GetEvents();

        // assert
        Assert.NotNull(sut);
        Assert.NotNull(sut.Id);
        Assert.Equal(date, sut.Id);
        Assert.Equal(1, sut.OriginalVersion);
        Assert.Equal(2, sut.Version);
        Assert.Collection(sut.Jobs,
            item0 =>
            {
                Assert.Equal(command.JobId, item0.Id);
                Assert.Equal(command.StartTime, item0.PlannedTimeslot.StartTime);
                Assert.Equal(command.EndTime, item0.PlannedTimeslot.EndTime);
                Assert.Equal(command.CustomerInfo.Id, item0.Customer.Id);
                Assert.Equal(command.CustomerInfo.Name, item0.Customer.Name);
                Assert.Equal(command.CustomerInfo.TelephoneNumber, item0.Customer.TelephoneNumber);
                Assert.Equal(command.VehicleInfo.LicenseNumber, item0.Vehicle.Id);
                Assert.Equal(command.VehicleInfo.Brand, item0.Vehicle.Brand);
                Assert.Equal(command.VehicleInfo.Type, item0.Vehicle.Type);
                Assert.Equal(command.CustomerInfo.Id, item0.Vehicle.OwnerId);
                Assert.Equal(command.Description, item0.Description);
            }
        );
        Assert.Collection(events, item0 => Assert.IsAssignableFrom<MaintenanceJobPlanned>(item0));
    }

    [Fact]
    public void Plan_MaintenanceJob_That_Spans_Two_Days_Should_Throw_Exception()
    {
        // arrange
        DateTime date = DateTime.Today;
        var initializingEvents = new Event[] {
                new WorkshopPlanningCreatedEventBuilder().WithDate(date).Build()
            };
        WorkshopPlanning sut = new WorkshopPlanning(date, initializingEvents);

        MaintenanceJobBuilder maintenanceJobBuilder = new MaintenanceJobBuilder();
        maintenanceJobBuilder
            .WithStartTime(DateTime.Today.AddDays(-2)); // to make the job span multiple days
        PlanMaintenanceJob command = new PlanMaintenanceJobCommandBuilder()
            .WithMaintenanceJobBuilder(maintenanceJobBuilder)
            .Build();

        // act
        var thrownException =
            Assert.Throws<BusinessRuleViolationException>(() => sut.PlanMaintenanceJob(command));

        // assert
        Assert.Equal("Start-time and end-time of a Maintenance Job must be within a 1 day.",
            thrownException.Message);
    }

    [Fact]
    public void Planning_Too_Much_MaintenanceJobs_In_Parallel_Should_Throw_Exception()
    {
        // arrange
        DateTime date = DateTime.Today;
        var initializingEvents = new Event[] {
                new WorkshopPlanningCreatedEventBuilder().WithDate(date).Build()
            };
        WorkshopPlanning sut = new WorkshopPlanning(date, initializingEvents);

        VehicleBuilder vehicleBuilder = new VehicleBuilder();
        PlanMaintenanceJobCommandBuilder commandBuilder = new PlanMaintenanceJobCommandBuilder()
            .WithVehicleBuilder(vehicleBuilder);

        PlanMaintenanceJob command1 = commandBuilder.Build();
        vehicleBuilder.WithRandomLicenseNumber();
        PlanMaintenanceJob command2 = commandBuilder.Build();
        vehicleBuilder.WithRandomLicenseNumber();
        PlanMaintenanceJob command3 = commandBuilder.Build();
        vehicleBuilder.WithRandomLicenseNumber();
        PlanMaintenanceJob command4 = commandBuilder.Build();

        // act
        sut.PlanMaintenanceJob(command1);
        sut.PlanMaintenanceJob(command2);
        sut.PlanMaintenanceJob(command3);
        var thrownException = Assert.Throws<BusinessRuleViolationException>(() =>
        {
            sut.PlanMaintenanceJob(command4); // 4th parallel job
        });

        // assert
        Assert.Equal("Maintenancejob overlaps with more than 3 other jobs.",
            thrownException.Message);
    }

    [Fact]
    public void Plan_Two_MaintenanceJobs_In_Parallel_For_The_Same_Vehicle_Should_Throw_Exception()
    {
        // arrange
        DateTime date = DateTime.Today;
        var initializingEvents = new Event[] {
                new WorkshopPlanningCreatedEventBuilder().WithDate(date).Build()
            };
        WorkshopPlanning sut = new WorkshopPlanning(date, initializingEvents);
        PlanMaintenanceJob command = new PlanMaintenanceJobCommandBuilder()
            .Build();

        // act
        sut.PlanMaintenanceJob(command);
        var thrownException = Assert.Throws<BusinessRuleViolationException>(() =>
        {
            sut.PlanMaintenanceJob(command); // parallel job for same vehicle
        });

        // assert
        Assert.Equal("Only 1 maintenance job can be executed on a vehicle during a certain time-slot.",
            thrownException.Message);
    }

    [Fact]
    public void Finish_MaintenanceJob_Should_Finish_An_Unfinished_MaintenanceJob()
    {
        // arrange
        DateTime date = DateTime.Today;
        Guid jobId = Guid.NewGuid();
        DateTime startTime = date.AddHours(8);
        DateTime endTime = date.AddHours(11);
        DateTime actualStartTime = date.AddHours(9);
        DateTime actualEndTime = date.AddHours(12);
        var initializingEvents = new Event[] {
                new WorkshopPlanningCreatedEventBuilder()
                    .WithDate(date)
                    .Build(),
                new MaintenanceJobPlannedEventBuilder()
                    .WithStartTime(startTime)
                    .WithEndTime(endTime)
                    .WithJobId(jobId)
                    .Build()
            };
        WorkshopPlanning sut = new WorkshopPlanning(date, initializingEvents);

        FinishMaintenanceJob command = new FinishMaintenanceJobCommandBuilder()
            .WithJobId(jobId)
            .WithActualStartTime(actualStartTime)
            .WithActualEndTime(actualEndTime)
            .Build();

        // act
        sut.FinishMaintenanceJob(command);
        IEnumerable<Event> events = sut.GetEvents();

        // assert
        Assert.NotNull(sut);
        Assert.NotNull(sut.Id);
        Assert.Equal(date, sut.Id);
        Assert.Equal(2, sut.OriginalVersion);
        Assert.Equal(3, sut.Version);
        Assert.Collection(sut.Jobs,
            item0 =>
            {
                Assert.Equal(command.JobId, item0.Id);
                Assert.Equal(startTime, item0.PlannedTimeslot.StartTime);
                Assert.Equal(endTime, item0.PlannedTimeslot.EndTime);
                Assert.Equal(command.StartTime, item0.ActualTimeslot.StartTime);
                Assert.Equal(command.EndTime, item0.ActualTimeslot.EndTime);
                Assert.Equal(command.Notes, item0.Notes);
            }
        );
        Assert.Collection(events, item0 => Assert.IsAssignableFrom<MaintenanceJobFinished>(item0));
    }

    [Fact]
    public void Finish_An_Already_Finished_MaintenanceJob_Should_Throw_Exception()
    {
        // arrange
        DateTime date = DateTime.Today;
        Guid jobId = Guid.NewGuid();
        DateTime startTime = date.AddHours(8);
        DateTime endTime = date.AddHours(11);
        DateTime actualStartTime = date.AddHours(9);
        DateTime actualEndTime = date.AddHours(12);
        string notes = "Ok";
        var initializingEvents = new Event[] {
                new WorkshopPlanningCreatedEventBuilder()
                    .WithDate(date)
                    .Build(),
                new MaintenanceJobPlannedEventBuilder()
                    .WithStartTime(startTime)
                    .WithEndTime(endTime)
                    .WithJobId(jobId)
                    .Build(),
                new MaintenanceJobFinishedEventBuilder()
                    .WithJobId(jobId)
                    .WithActualStartTime(actualStartTime)
                    .WithActualEndTime(actualEndTime)
                    .WithNotes(notes)
                    .Build()
            };
        WorkshopPlanning sut = new WorkshopPlanning(date, initializingEvents);

        FinishMaintenanceJob command = new FinishMaintenanceJobCommandBuilder()
            .WithJobId(jobId)
            .WithActualStartTime(actualStartTime)
            .WithActualEndTime(actualEndTime)
            .WithNotes(notes)
            .Build();

        // act
        var thrownException =
            Assert.Throws<BusinessRuleViolationException>(() =>
                sut.FinishMaintenanceJob(command));

        // assert
        Assert.Equal("An already finished job can not be finished.",
            thrownException.Message);
    }
}