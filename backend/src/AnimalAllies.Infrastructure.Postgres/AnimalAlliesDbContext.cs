using AnimalAllies.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AnimalAllies.Infrastructure;

public class AnimalAlliesDbContext: DbContext
{
    private readonly IConfiguration _configuration;

    public AnimalAlliesDbContext(){}
    
    /*public AnimalAlliesDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }*/

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        optionsBuilder.UseNpgsql("Host = localhost; Database = animalAllies; Username = postgres; Password = 345890; ");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnimalAlliesDbContext).Assembly);
    }

    public DbSet<Volunteer> Volunteers { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<PetPhoto> PetPhotos { get; set; }
}