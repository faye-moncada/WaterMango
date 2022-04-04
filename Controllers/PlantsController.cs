using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterMango.Data;
using WaterMango.Models;

namespace WaterMango.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlantsController : ControllerBase
    {
        private readonly PlantContext _context;

        public PlantsController(PlantContext context)
        {
            _context = context;
            //populate default data
            if (_context.Plant.Count() == 0)
            {
                _context.Plant.Add(new Plant
                {
                    Id = 1
                    ,
                    Name = "Camellia"
                    ,
                    Description = "Dense shrubs with brilliant foliage."
                    ,
                    Image = ImageHelper.GetImage(@"Images\camellia.jpg")
                    ,
                    LastWatered = DateTime.Now.AddHours(-1)
                });
                _context.Plant.Add(new Plant
                {
                    Id = 2
                    ,
                    Name = "Pothos"
                    ,
                    Description = "Tropical vine with shiny, heart-shaped leaves."
                    ,
                    Image = ImageHelper.GetImage(@"Images\pothos.jpg")
                    ,
                    LastWatered = DateTime.Now.AddHours(-6)
                });
                _context.Plant.Add(new Plant
                {
                    Id = 3
                    ,
                    Name = "Jasmine"
                    ,
                    Description = "Delicate and dainty vine with small flowers known around the world for its unique tropical smell."
                    ,
                    Image = ImageHelper.GetImage(@"Images\jasmine.jpg")
                    ,
                    LastWatered = DateTime.Now.AddHours(-3)
                });
                _context.Plant.Add(new Plant
                {
                    Id = 4
                    ,
                    Name = "Tineke"
                    ,
                    Description = "Variegated rubber tree – is a popular variety of ficus known for its beautiful pink, green, and yellow leaves."
                    ,
                    Image = ImageHelper.GetImage(@"Images\tineke.jpg")
                    ,
                    LastWatered = DateTime.Now.AddHours(-2)
                });
                _context.Plant.Add(new Plant
                {
                    Id = 5
                    ,
                    Name = "Snake Plant"
                    ,
                    Description = "West African sansevieria with stiff leaves and grown especially as a houseplant."
                    ,
                    Image = ImageHelper.GetImage(@"Images\snake_plant.jpg")
                    ,
                    LastWatered = DateTime.Now.AddHours(-8)
                });
                _context.SaveChangesAsync();
            }
        }

        // GET: api/Plants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlant()
        {
            return await _context.Plant.ToListAsync();
        }

        // GET: api/Plants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(int id)
        {
            var plant = await _context.Plant.FindAsync(id);

            if (plant == null)
            {
                return NotFound();
            }

            return plant;
        }

        // PUT: api/Plants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlant(int id, Plant plant)
        {
            if (id != plant.Id)
            {
                return BadRequest();
            }
            //update last watering started
            plant.LastWatered = DateTime.Now;
            _context.Entry(plant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Plants

        [HttpPost]
        public async Task<ActionResult<Plant>> PostPlant(Plant plant)
        {
            _context.Plant.Add(plant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlant", new { id = plant.Id }, plant);
        }

        // DELETE: api/Plants/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Plant>> DeletePlant(int id)
        {
            var plant = await _context.Plant.FindAsync(id);
            if (plant == null)
            {
                return NotFound();
            }

            _context.Plant.Remove(plant);
            await _context.SaveChangesAsync();

            return plant;
        }

        private bool PlantExists(int id)
        {
            return _context.Plant.Any(e => e.Id == id);
        }
    }
}
