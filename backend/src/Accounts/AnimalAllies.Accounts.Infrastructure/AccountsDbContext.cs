using System.Text.Json;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.SharedKernel.Constraints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Infrastructure;

public class AccountsDbContext(IConfiguration configuration)
    : IdentityDbContext<User,Role,Guid>
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            .UseLoggerFactory(CreateLoggerFactory)
            .EnableSensitiveDataLogging()
            .UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(u => u.SocialNetworks)
            .HasConversion(
                u => JsonSerializer.Serialize(u, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<SocialNetwork>>(json, JsonSerializerOptions.Default)!,
                new ValueComparer<List<SocialNetwork>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                    c => c.ToList()));

        modelBuilder.Entity<ParticipantAccount>()
            .ComplexProperty(pa => pa.FullName, pab =>
            {
                pab.Property(f => f.FirstName)
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                    .HasColumnName("first_name");
                pab.Property(f => f.SecondName)
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                    .HasColumnName("second_name");
                pab.Property(f => f.Patronymic)
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                    .HasColumnName("patronymic");
            });

        modelBuilder.Entity<ParticipantAccount>()
            .HasOne(pa => pa.User)
            .WithOne();

        modelBuilder.Entity<ParticipantAccount>()
            .HasKey(pa => pa.UserId);
        
        
        modelBuilder.Entity<AdminProfile>()
            .ComplexProperty(ap => ap.FullName, apb =>
            {
                apb.Property(f => f.FirstName)
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                    .HasColumnName("first_name");
                apb.Property(f => f.SecondName)
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                    .HasColumnName("second_name");
                apb.Property(f => f.Patronymic)
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                    .HasColumnName("patronymic");
            });

        modelBuilder.Entity<AdminProfile>()
            .HasOne(ap => ap.User)
            .WithOne();
        
        modelBuilder.Entity<AdminProfile>()
            .HasKey(pa => pa.UserId);
        
        modelBuilder.Entity<VolunteerAccount>()
            .ComplexProperty(va => va.FullName, vab =>
            {
                vab.Property(f => f.FirstName)
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                    .HasColumnName("first_name");
                vab.Property(f => f.SecondName)
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                    .HasColumnName("second_name");
                vab.Property(f => f.Patronymic)
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                    .HasColumnName("patronymic");
            });
        
        modelBuilder.Entity<VolunteerAccount>()
            .Property(u => u.Requisites)
            .HasConversion(
                u => JsonSerializer.Serialize(u, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<Requisite>>(json, JsonSerializerOptions.Default)!,
                new ValueComparer<List<Requisite>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                    c => c.ToList()));
        
        modelBuilder.Entity<VolunteerAccount>()
            .Property(u => u.Certificates)
            .HasConversion(
                u => JsonSerializer.Serialize(u, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<Certificate>>(json, JsonSerializerOptions.Default)!,
                new ValueComparer<List<Certificate>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                    c => c.ToList()));
        
        modelBuilder.Entity<VolunteerAccount>()
            .HasOne(va => va.User)
            .WithOne();
        
        modelBuilder.Entity<VolunteerAccount>()
            .HasKey(pa => pa.UserId);
        
        modelBuilder.Entity<Role>()
            .ToTable("roles");

        modelBuilder.Entity<Permission>()
            .ToTable("permissions");

        modelBuilder.Entity<Permission>()
            .HasIndex(p => p.Code)
            .IsUnique();

        modelBuilder.Entity<Permission>()
            .Property(p => p.Description)
            .HasMaxLength(300);
        
        modelBuilder.Entity<RolePermission>()
            .ToTable("role_permissions");

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);
        
        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId);
        
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new {rp.RoleId, rp.PermissionId });
        
        modelBuilder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("claims");

        modelBuilder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");

        modelBuilder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");

        modelBuilder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");
        
        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");
        
    }

    private static readonly ILoggerFactory CreateLoggerFactory
        = LoggerFactory.Create(builder => { builder.AddConsole(); });
    
}