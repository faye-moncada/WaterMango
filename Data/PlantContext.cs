using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WaterMango.Models;

namespace WaterMango.Data
{
    public class PlantContext : DbContext
    {
        public PlantContext (DbContextOptions<PlantContext> options)
            : base(options)
        {
        }

        public DbSet<WaterMango.Models.Plant> Plant { get; set; }
    }
}
