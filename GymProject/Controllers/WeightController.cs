using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using GymProject.Domain;
using GymProject.Application;

namespace GymProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeightController : ControllerBase
    {
        private WeightManager _weightManager;

        public WeightController()
        {
            _weightManager = new WeightManager();
        }

        [HttpGet("{weightId}", Name = "GetWeightById")]
        public ActionResult<Weight> GetWeightById(int weightId)
        {
            try
            {
                var weight = _weightManager.GetWeightById(weightId);
                if (weight == null)
                {
                    return NotFound();
                }
                return Ok(weight);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("latest", Name = "GetLatestWeightsForDifferentExercise")]
        public ActionResult<List<Weight>> GetLatestWeightsForDifferentExercise([FromQuery] int userId)
        {
            try
            {
                var weights = _weightManager.GetLatestWeightsForDifferentExercise(userId);
                return Ok(weights);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("average/{userID}", Name = "GetAverageWeightByExercise")]
        public ActionResult<List<Weight>> GetAverageWeightByExercise(int userID)
        {
            try
            {
                var averageWeights = _weightManager.GetAverageWeightByExercise(userID);
                return Ok(averageWeights);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("InsertWeight", Name = "InsertWeight")]
        public IActionResult CreateWeight([FromHeader(Name = "UserID")] int userId, [FromBody] Weight newWeight)
        {
            try
            {
                _weightManager.CreateWeight(newWeight,userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("bulk", Name = "CreateWeights")]
        public IActionResult CreateWeights([FromHeader(Name = "UserID")] int userId, List<Weight> newWeights)
        {
            try
            {
                _weightManager.CreateWeights(newWeights,userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{weightId}", Name = "UpdateWeightById")]
        public IActionResult UpdateWeightById(int weightId, Weight updatedWeight)
        {
            try
            {
                _weightManager.UpdateWeightById(weightId, updatedWeight);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{weightId}", Name = "DeleteWeightById")]
        public IActionResult DeleteWeightById(int weightId)
        {
            try
            {
                _weightManager.DeleteWeightById(weightId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
