using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherApi.Models;

namespace WeatherApi.Data
{
    public class WeatherApiContext : DbContext
    {
        public WeatherApiContext (DbContextOptions<WeatherApiContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherApi.Models.Weather> Weather { get; set; } = default!;
        public DbSet<WeatherApi.Models.Station> Station { get; set; } = default!;
    }
}
