﻿namespace Tour_Planner_Service.Controllers;

[Route("Tour")]
[ApiController]
public class TourController : ControllerBase
{
    private readonly ILog log = LogManager.GetLogger(typeof(TourController));
    private readonly DBTour myDB;
    public TourController(IConfiguration configuration)
    {
        myDB = DBTour.GetInstance(configuration);
    }
    [HttpGet("GetAll")]
    public ActionResult<IEnumerable<Tour>> Get()
    {
        try
        {
            log.Info("Get All Tours Successful!");
            return Ok(myDB.GetAllTours() ?? throw new HttpRequestException());
        }
        catch (Exception ex)
        {
            log.Fatal(ex.Message);
            return StatusCode(500);
        }
    }
    [HttpGet("GetByID")]
    public ActionResult<Tour> Get(Guid id)
    {
        try
        {
            if (myDB.GetTourByID(id) is null)
            {
                log.Error("Tour Not Found: " + id);
                return NotFound();
            }
            log.Info("Get Tour By ID Successful: " + id);
            return Ok(myDB.GetTourByID(id));
        }
        catch (Exception ex)
        {
            log.Fatal(ex.Message);
            return StatusCode(500);
        }
    }
    [HttpPost("AddTour")]
    public ActionResult Post(TourDTO item)
    {
        ValidationContext vc = new(item);
        ICollection<ValidationResult> results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(item, vc, results, true))
        {
            log.Fatal("Tour is not complete!");
            return StatusCode(500);
        }
        try
        {
            Tour newTour = new()
            {
                Id = Guid.NewGuid(),
                TourName = item.TourName,
                TourDescription = item.TourDescription,
                StartingPoint = item.StartingPoint,
                Destination = item.Destination,
                TransportType = item.TransportType,
                TourDistance = item.TourDistance,
                EstimatedTimeInMin = item.EstimatedTimeInMin,
                TourType = item.TourType
            };
            if (myDB.AddTour(newTour))
            {
                log.Info("Tour Added Successfully: " + newTour.Id);
                return Ok("Added Successfully!");
            }
            throw new HttpRequestException();
        }
        catch (Exception ex)
        {
            log.Fatal(ex.Message);
            return StatusCode(500);
        }
    }
    [HttpPut("UpdateTour")]
    public ActionResult Put(TourDTO item)
    {
        ValidationContext vc = new(item);
        ICollection<ValidationResult> results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(item, vc, results, true))
        {
            log.Fatal("Tour is not complete!");
            return StatusCode(500);
        }
        try
        {
            Tour newTour = new()
            {
                Id = item.Id,
                TourName = item.TourName,
                TourDescription = item.TourDescription,
                StartingPoint = item.StartingPoint,
                Destination = item.Destination,
                TransportType = item.TransportType,
                TourDistance = item.TourDistance,
                EstimatedTimeInMin = item.EstimatedTimeInMin,
                TourType = item.TourType
            };
            Tour? existingItem = myDB.GetTourByID(newTour.Id);
            if (existingItem is null)
            {
                log.Error("Tour Not Found: " + item.Id);
                return NotFound();
            }
            if (myDB.UpdateTour(newTour))
            {
                log.Info("Tour Updated Successfully: " + item.Id);
                return Ok("Updated Successfully!");
            }
            throw new HttpRequestException();
        }
        catch (Exception ex)
        {
            log.Fatal(ex.Message);
            return StatusCode(500);
        }
    }
    [HttpDelete("DeleteTour")]
    public ActionResult DeleteTour(Guid deleteID)
    {
        try
        {
            Tour? existingItem = myDB.GetTourByID(deleteID);
            if (existingItem is null)
            {
                log.Error("Tour Not Found: " + deleteID);
                return NotFound();
            }
            if (myDB.DeleteTour(deleteID))
            {
                log.Info("Tour Deleted Successfully: " + deleteID);
                return Ok("Deleted Successfully!");
            }
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            log.Fatal(ex.Message);
            return StatusCode(500);
        }
    }
}