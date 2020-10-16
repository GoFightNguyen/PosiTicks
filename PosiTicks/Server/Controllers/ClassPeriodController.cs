using PosiTicks.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PosiTicks.Server.Domain;

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
    }
}
