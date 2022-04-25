using System;
using System.Data.Entity;

namespace Samples.IntegrationLayer.Models
{
    public class Member
    {
        public Guid MemberId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class MemberDBContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
    }
}