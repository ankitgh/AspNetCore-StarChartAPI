using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpGet("{id:int}", Name ="GetById")]
        public IActionResult GetById(int id)
        {
            CelestialObject celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();
            return Ok(celestialObject);
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (celestialObjects == null || !celestialObjects.Any())
            {
                return NotFound();
            }

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialObject.Id).ToList();
            }
            
            return Ok(celestialObjects);
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.ToList();
            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(_context.CelestialObjects);
        }


        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] int id, CelestialObject celestialObject)
        {
            var lookedObject = _context.CelestialObjects.Find(id);

            if(lookedObject == null)
            {
                return NotFound();
            }

            lookedObject.Name = celestialObject.Name;
            lookedObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            lookedObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(lookedObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject([FromBody] int id, string name)
        {
            var lookedObject = _context.CelestialObjects.Find(id);

            if (lookedObject == null)
            {
                return NotFound();
            }

            lookedObject.Name = name;
            _context.CelestialObjects.Update(lookedObject);
            _context.SaveChanges();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete([FromBody] int id)
        {
            var lookedObject = _context.CelestialObjects.Where(x => x.Id == id || x.OrbitedObjectId == id).ToList();

            if (lookedObject == null)
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(lookedObject);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
