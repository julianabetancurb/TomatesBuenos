using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TomatesBuenos.Models;

namespace TomatesBuenos.Data
{
    public class TomatesBuenosContext : DbContext
    {
        public TomatesBuenosContext (DbContextOptions<TomatesBuenosContext> options)
            : base(options)
        {
        }

        public DbSet<TomatesBuenos.Models.Movie> Movie { get; set; } = default!;

        public DbSet<TomatesBuenos.Models.TVshow> TVshow { get; set; } = default!;

        public DbSet<TomatesBuenos.Models.TopMovie> TopMovie { get; set; } = default!;

        public DbSet<TomatesBuenos.Models.TopTVshow> TopTVshow { get; set; } = default!;

    }
}
