using GalleriesServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleriesServer.Models
{
    public class GalleriesDbContext : DbContext
    {

        /// <summary>
        /// The database model for the application.
        /// </summary>
        /// <param name="options" description="Used to parse in the actual database instance"></param>
        public GalleriesDbContext(DbContextOptions<GalleriesDbContext> options) : base(options)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<MediaContainer> MediaContainers { get; set; }
        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<Owner> Owners { get; set; }

        //(to run migrations in console):
        //dotnet ef migrations add initial --output-dir Data/Migrations
        //to remove:
        //dotnet ef database update initial (or 'dotnet ef database update 0' to remove all)
        //and
        //dotnet ef migrations remove
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Affect some additionall configuration of the database
            modelBuilder.Entity<Owner>().HasIndex(a => a.ExternalUserId);
        }
    }
}
