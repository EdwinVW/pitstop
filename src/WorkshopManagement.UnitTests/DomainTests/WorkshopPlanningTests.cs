namespace WorkshopManagement.UnitTests.DomainTests;

[TestClass]
public class WorkshopPlanningTests
{
    [TestMethod]
    public void Create_Should_Create_A_New_Instance()
    {
        // arrange
        DateTime date = DateTime.Today;

        // act
        WorkshopPlanning sut = WorkshopPlanning.Create(date);
        var events = sut.GetEvents().ToList();

        // assert
        sut.ShouldNotBeNull();
        sut.Id.ShouldNotBeNull();
        sut.Id.ShouldBe(date);
        sut.OriginalVersion.ShouldBe(0);
        sut.Version.ShouldBe(1);
        sut.Jobs.Count.ShouldBe(0);
        events.ShouldAllBe(x => x.ShouldBeOfType<WorkshopPlanningCreated>());
    }

    [TestMethod]
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
        var events = sut.GetEvents().ToList();

        // assert
        sut.ShouldNotBeNull();
        sut.Id.ShouldNotBeNull();
        sut.Id.ShouldBe(date);
        sut.OriginalVersion.ShouldBe(1);
        sut.Version.ShouldBe(2);
        var job = sut.Jobs.First();
        job.Id.ShouldBe(command.JobId);
        job.PlannedTimeslot.StartTime.ShouldBe(command.StartTime);
        job.PlannedTimeslot.EndTime.ShouldBe(command.EndTime);
        job.Customer.Id.ShouldBe(command.CustomerInfo.Id);
        job.Customer.Name.ShouldBe(command.CustomerInfo.Name);
        job.Customer.TelephoneNumber.ShouldBe(command.CustomerInfo.TelephoneNumber);
        job.Vehicle.Id.ShouldBe(command.VehicleInfo.LicenseNumber);
        job.Vehicle.Brand.ShouldBe(command.VehicleInfo.Brand);
        job.Vehicle.Type.ShouldBe(command.VehicleInfo.Type);
        job.Vehicle.OwnerId.ShouldBe(command.CustomerInfo.Id);
        job.Description.ShouldBe(command.Description);
        events.ShouldAllBe(x => x.ShouldBeOfType<MaintenanceJobPlanned>());
    }

    [TestMethod]
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
            Should.Throw<BusinessRuleViolationException>(() => sut.PlanMaintenanceJob(command));

        // assert
        thrownException.Message.ShouldBe("Start-time and end-time of a Maintenance Job must be within a 1 day.");
    }

    [TestMethod]
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
        var thrownException = Should.Throw<BusinessRuleViolationException>(() =>
        {
            sut.PlanMaintenanceJob(command4); // 4th parallel job
        });

        // assert
        thrownException.Message.ShouldBe("Maintenancejob overlaps with more than 3 other jobs.");
    }

    [TestMethod]
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
        var thrownException = Should.Throw<BusinessRuleViolationException>(() =>
        {
            sut.PlanMaintenanceJob(command); // parallel job for same vehicle
        });

        // assert
        thrownException.Message.ShouldBe("Only 1 maintenance job can be executed on a vehicle during a certain time-slot.");
    }

    [TestMethod]
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
        var events = sut.GetEvents().ToList();

        // assert
        sut.ShouldNotBeNull();
        sut.Id.ShouldNotBeNull();
        sut.Id.ShouldBe(date);
        sut.OriginalVersion.ShouldBe(2);
        sut.Version.ShouldBe(3);
        var job = sut.Jobs[0];
        job.Id.ShouldBe(command.JobId);
        job.PlannedTimeslot.StartTime.ShouldBe(startTime);
        job.PlannedTimeslot.EndTime.ShouldBe(endTime);
        job.ActualTimeslot.StartTime.ShouldBe(command.StartTime);
        job.ActualTimeslot.EndTime.ShouldBe(command.EndTime);
        job.Notes.ShouldBe(command.Notes);
        events.ShouldAllBe(x => x.ShouldBeOfType<MaintenanceJobFinished>());
    }

    [TestMethod]
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
            Should.Throw<BusinessRuleViolationException>(() =>
                sut.FinishMaintenanceJob(command));

        // assert
        thrownException.Message.ShouldBe("An already finished job can not be finished.");
    }
}