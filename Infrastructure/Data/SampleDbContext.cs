using System.Collections.Generic;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using WExporter.Entities.SampleConcrete;

namespace Infrastructure.Data
{
    public class SampleDbContext : BaseDbContext
    {
        public SampleDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Sample> Samples { get; set; }
        public DbSet<Mample> Mamples { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sample>()
                .Property(s => s.Id)
                .IsRequired();
            modelBuilder.Entity<Sample>()
                .HasMany(x => x.Mamples)
                .WithOne(x => x.Sample)
                .HasForeignKey(x => x.SampleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}