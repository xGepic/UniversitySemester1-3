﻿namespace Tour_Planner_Service.Controllers;

[Route("TourLog")]
[ApiController]
public class TourLogController : ControllerBase
{
    private readonly ILog log = LogManager.GetLogger(typeof(TourLogController));
    private readonly DBTourLog myDB;
    public TourLogController(IConfiguration configuration)
    {
        myDB = DBTourLog.GetInstance(configuration);
    }
    [HttpGet("GetAll")]
    public ActionResult<IEnumerable<TourLog>> Get(Guid id)
    {
        try
        {
            if (!myDB.CheckRelatedTourID(id))
            {
                log.Fatal("Tour not found!");
                return NotFound();
            }
            log.Info("Get All Tourlogs Successful!");
            return Ok(myDB.GetAllTourLogs(id) ?? throw new HttpRequestException());
        }
        catch (Exception ex)
        {
            log.Fatal(ex.Message);
            return StatusCode(500);
        }
    }
    [HttpPost("AddTourLog")]
    public ActionResult Post(TourLogDTO item)
    {
        ValidationContext vc = new(item);
        ICollection<ValidationResult> results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(item, vc, results, true))
        {
            log.Fatal("TourLog is not complete!");
            return StatusCode(500);
        }
        try
        {
            if (!myDB.CheckRelatedTourID(item.RelatedTourID))
            {
                log.Fatal("Related Tour not Found: " + item.RelatedTourID);
                return NotFound();
            }
            TourLog newLog = new()
            {
                Id = Guid.NewGuid(),
                TourDateAndTime = item.TourDateAndTime,
                TourComment = item.TourComment,
                TourDifficulty = item.TourDifficulty,
                TourTimeInMin = item.TourTimeInMin,
                TourRating = item.TourRating,
                RelatedTourID = item.RelatedTourID
            };
            if (myDB.AddTourLog(newLog))
            {
                log.Info("TourLog Added Successfully: " + newLog.Id);
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
    [HttpPut("UpdateTourLog")]
    public ActionResult Put(TourLogDTO item)
    {
        ValidationContext vc = new(item);
        ICollection<ValidationResult> results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(item, vc, results, true))
        {
            log.Fatal("TourLog is not complete!");
            return StatusCode(500);
        }
        try
        {
            if (!myDB.CheckRelatedTourID(item.RelatedTourID))
            {
                throw new Exception("RelatedTourID doesnt match with a Tour!");
            }
            TourLog newLog = new()
            {
                Id = item.Id,
                TourDateAndTime = item.TourDateAndTime,
                TourComment = item.TourComment,
                TourDifficulty = item.TourDifficulty,
                TourTimeInMin = item.TourTimeInMin,
                TourRating = item.TourRating,
                RelatedTourID = item.RelatedTourID
            };
            TourLog? existingItem = myDB.GetTourLogByID(newLog.Id);
            if (existingItem is null)
            {
                log.Error("TourLog Not Found: " + item.Id);
                return NotFound();
            }
            if (myDB.UpdateTourLog(newLog))
            {
                log.Info("TourLog Updated Successfully: " + item.Id);
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
    [HttpDelete("DeleteTourLog")]
    public ActionResult DeleteTourLog(Guid deleteID)
    {
        try
        {
            TourLog? existingItem = myDB.GetTourLogByID(deleteID);
            if (existingItem is null)
            {
                log.Error("TourLog Not Found: " + deleteID);
                return NotFound();
            }
            if (myDB.DeleteTourLog(deleteID))
            {
                log.Info("TourLog Deleted Successfully: " + deleteID);
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
