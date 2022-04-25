using System;
using System.Data.Entity;

namespace Samples.IntegrationLayer.Models
{
    public class Claim
    {
        public Guid ClaimId { get; set; }
        public Guid MemberId { get; set; }
        public DateTime IssuedDate { get; set; }
        public string Description { get; set; }
    }

    public class ClaimDBContext : DbContext
    {
        public DbSet<Claim> Claims { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("insurance");
        }
    }
}