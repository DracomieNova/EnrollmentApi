using Microsoft.EntityFrameworkCore;
using EnrollmentApi.Model;

namespace EnrollmentApi.Data
{
    public class EnrollmentDbContext : DbContext
    {
        public EnrollmentDbContext(DbContextOptions<EnrollmentDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Programs { get; set; }
        public DbSet<Parish> Parishes { get; set; }
        public DbSet<ShirtSize> ShirtSizes { get; set; }
        
    }
}
