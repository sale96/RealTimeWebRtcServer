using Microsoft.EntityFrameworkCore;
using WebRtc.Api.DataAccess.Entities;

namespace WebRtc.Api.DataAccess;

public class ServerDbContext(DbContextOptions<ServerDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Meeting>().HasKey(m => m.Id);
        modelBuilder.Entity<Meeting>()
            .HasIndex(x => x.IdempotencyKey)
            .IsUnique();

        modelBuilder.Entity<Meeting>()
            .HasMany(x => x.Clients)
            .WithOne(x => x.Meeting)
            .HasForeignKey(x => x.MeetingId);
        
        modelBuilder.Entity<Client>().HasKey(m => m.Id);
        
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Client> Clients { get; set; }
}