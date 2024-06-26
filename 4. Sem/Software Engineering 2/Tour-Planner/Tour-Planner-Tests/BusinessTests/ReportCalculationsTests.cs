﻿namespace TourPlannerTests;

internal class ReportCalculationsTests
{
    private readonly Tour testTour = new()
    {
        Id = Guid.NewGuid(),
        TourName = "TestTour",
        TourDescription = "nothing",
        StartingPoint = "Vienna",
        Destination = "Berlin",
        TransportType = Tour_Planner_Model.TransportType.byCar,
        TourDistance = 600,
        EstimatedTimeInMin = 6,
        TourType = TourType.Vacation,
    };
    private readonly TourLog testLog = new()
    {
        Id = Guid.NewGuid(),
        TourDateAndTime = DateTime.Now,
        TourComment = null,
        TourDifficulty = TourDifficulty.medium,
        TourTimeInMin = 361,
        TourRating = TourRating.Satisfied,
        RelatedTourID = Guid.NewGuid()
    };
    private readonly TourLog testlog2 = new()
    {
        Id = Guid.NewGuid(),
        TourDateAndTime = DateTime.Now,
        TourComment = null,
        TourDifficulty = TourDifficulty.medium,
        TourTimeInMin = 400,
        TourRating = TourRating.verySatisfied,
        RelatedTourID = Guid.NewGuid()
    };
    //UnitOfWork_StateUnderTest_ExpectedBehavior
    [Test]
    public void GetAverageTime_WhenNumberIsNotEven_RoundsCorrectly()
    {
        //Arrange
        List<TourLog> logList = new() { testLog, testlog2 };
        testTour.TourLogs = logList;

        //Act
        var result = ReportCalculations.GetAverageTime(testTour);

        //Assert
        int actual = 381;
        result.Should().Be(actual);
    }
    //UnitOfWork_StateUnderTest_ExpectedBehavior
    [Test]
    public void GetAverageRating_WhenEnumIsNotEven_RoundsCorrectly()
    {
        //Arrange
        List<TourLog> logList = new() { testLog, testlog2 };
        testTour.TourLogs = logList;

        //Act
        var result = ReportCalculations.GetAverageRating(testTour);

        //Assert
        var actual = TourRating.verySatisfied;
        result.Should().Be((int)actual);
    }
}
