using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.Models
{
    public class FootballContext : IdentityDbContext
    {
        //public DbSet<Team> Teams { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Team>().ToTable("Teams");
            modelBuilder.Entity<Field>().ToTable("Fields");
            modelBuilder.Entity<Player>().ToTable("Players");
            modelBuilder.Entity<Game>().ToTable("Games");
        }
    }
}
