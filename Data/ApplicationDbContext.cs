using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Models;


namespace RealEstate_WebAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; }

        public DbSet<Agent> Agents { get; set; }

        public DbSet<PropertyImage> PropertyImages { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Review> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fix the Favorites cascade delete
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Property)
                .WithMany()
                .HasForeignKey(f => f.PropertyId)
                .OnDelete(DeleteBehavior.NoAction);

            // Fix the Property-ApplicationUser relationship
            modelBuilder.Entity<Property>()
                .HasOne(p => p.Agent)
                .WithMany(u => u.Properties)
                .HasForeignKey(p => p.AgentId)
                .OnDelete(DeleteBehavior.SetNull); // Or choose appropriate delete behavior

            base.OnModelCreating(modelBuilder);
        }

    }
}