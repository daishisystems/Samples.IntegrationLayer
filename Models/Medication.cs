using System;
using System.Data.Entity;

namespace Samples.IntegrationLayer.Models
{
    public class Medication
    {
        public Guid MedicationId { get; set; }
        public Guid MemberId { get; set; }
        public string Name { get; set; }
        public string Expiry { get; set; }
    }

    public class MedicationDBContext : DbContext
    {
        public DbSet<Medication> Medications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("treatment");
        }
    }
}