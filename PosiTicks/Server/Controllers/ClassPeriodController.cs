using PosiTicks.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PosiTicks.Server.Domain;
using System.Net;

namespace PosiTicks.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassPeriodController : ControllerBase
    {
        private readonly ILogger<ClassPeriodController> _logger;
        private readonly ClassPeriodService _service;

        public ClassPeriodController(ILogger<ClassPeriodController> logger, ClassPeriodService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassPeriod>>> GetClassPeriods()
        {
            _logger.LogInformation("Getting Class Periods at {RequestTime}", DateTime.UtcNow);

            var classPeriods = await _service.GetAllAsync();
            return classPeriods.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassPeriod>> GetClassPeriod(int id)
        {
            _logger.LogInformation("Getting Class Period {Id} at {RequestTime}", id, DateTime.UtcNow);
            return await _service.GetAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<ClassPeriod>> CreateClassPeriod(ClassPeriod classPeriod)
        {
            _logger.LogInformation("Creating Class Period {@ClassPeriod} at {RequestTime}", classPeriod, DateTime.UtcNow);

            try
            {
                classPeriod = await _service.CreateAsync(classPeriod.Name);
                return CreatedAtAction(
                    nameof(GetClassPeriod),
                    new { id = classPeriod.Id },
                    classPeriod
                );
            }
            catch (DuplicateClassPeriodException ex)
            {
                ModelState.AddModelError(nameof(classPeriod.Name), ex.Message);
                return BadRequest(ModelState); // TODO: return ValidationProblem instead?
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Creating Class Period {@ClassPeriod} failed", classPeriod);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateClassPeriod(int id, ClassPeriod classPeriod)
        {
            _logger.LogInformation("Updating Class Period {Id} to {@ClassPeriod} at {RequestTime}", id, classPeriod, DateTime.UtcNow);
            try
            {
                await _service.UpdateAsync(classPeriod);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Updating Class Period {Id} to {@ClassPeriod} failed", id, classPeriod);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}