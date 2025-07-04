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
        Assert.IsNotNull(sut);
        Assert.IsNotNull(sut.Id);
        Assert.AreEqual(date, sut.Id);
        Assert.AreEqual(0, sut.OriginalVersion);
        Assert.AreEqual(1, sut.Version);
        Assert.AreEqual(0, sut.Jobs.Count);
        CollectionAssert.AllItemsAreInstancesOfType(events, typeof(WorkshopPlanningCreated));
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
        Assert.IsNotNull(sut);
        Assert.IsNotNull(sut.Id);
        Assert.AreEqual(date, sut.Id);
        Assert.AreEqual(1, sut.OriginalVersion);
        Assert.AreEqual(2, sut.Version);
        var job = sut.Jobs.First();
        Assert.AreEqual(command.JobId, job.Id);
        Assert.AreEqual(command.StartTime, job.PlannedTimeslot.StartTime);
        Assert.AreEqual(command.EndTime, job.PlannedTimeslot.EndTime);
        Assert.AreEqual(command.CustomerInfo.Id, job.Customer.Id);
        Assert.AreEqual(command.CustomerInfo.Name, job.Customer.Name);
        Assert.AreEqual(command.CustomerInfo.TelephoneNumber, job.Customer.TelephoneNumber);
        Assert.AreEqual(command.VehicleInfo.LicenseNumber, job.Vehicle.Id);
        Assert.AreEqual(command.VehicleInfo.Brand, job.Vehicle.Brand);
        Assert.AreEqual(command.VehicleInfo.Type, job.Vehicle.Type);
        Assert.AreEqual(command.CustomerInfo.Id, job.Vehicle.OwnerId);
        Assert.AreEqual(command.Description, job.Description);
        CollectionAssert.AllItemsAreInstancesOfType(events, typeof(MaintenanceJobPlanned));
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
            Assert.ThrowsException<BusinessRuleViolationException>(() => sut.PlanMaintenanceJob(command));

        // assert
        Assert.AreEqual("Start-time and end-time of a Maintenance Job must be within a 1 day.",
            thrownException.Message);
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
        var thrownException = Assert.ThrowsException<BusinessRuleViolationException>(() =>
        {
            sut.PlanMaintenanceJob(command4); // 4th parallel job
        });

        // assert
        Assert.AreEqual("Maintenancejob overlaps with more than 3 other jobs.",
            thrownException.Message);
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
        var thrownException = Assert.ThrowsException<BusinessRuleViolationException>(() =>
        {
            sut.PlanMaintenanceJob(command); // parallel job for same vehicle
        });

        // assert
        Assert.AreEqual("Only 1 maintenance job can be executed on a vehicle during a certain time-slot.",
            thrownException.Message);
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
        Assert.IsNotNull(sut);
        Assert.IsNotNull(sut.Id);
        Assert.AreEqual(date, sut.Id);
        Assert.AreEqual(2, sut.OriginalVersion);
        Assert.AreEqual(3, sut.Version);
        var job = sut.Jobs[0];
        Assert.AreEqual(command.JobId, job.Id);
        Assert.AreEqual(startTime, job.PlannedTimeslot.StartTime);
        Assert.AreEqual(endTime, job.PlannedTimeslot.EndTime);
        Assert.AreEqual(command.StartTime, job.ActualTimeslot.StartTime);
        Assert.AreEqual(command.EndTime, job.ActualTimeslot.EndTime);
        Assert.AreEqual(command.Notes, job.Notes);
        CollectionAssert.AllItemsAreInstancesOfType(events, typeof(MaintenanceJobFinished));
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
            Assert.ThrowsException<BusinessRuleViolationException>(() =>
                sut.FinishMaintenanceJob(command));

        // assert
        Assert.AreEqual("An already finished job can not be finished.",
            thrownException.Message);
    }
}