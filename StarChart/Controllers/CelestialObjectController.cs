using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController :ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name ="GetById")]
        public IActionResult GetById(int id)
        {
            if (!_context.CelestialObjects.Any(x => x.Id == id))
            {
                return NotFound();
            }
            else
            {
                return Ok(_context.CelestialObjects.First(x => x.Id == id));
            }

        }
    }
}
