using System.Text.Json;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
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
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<AdminProfile> AdminProfiles => Set<AdminProfile>();
    public DbSet<ParticipantAccount> ParticipantAccounts => Set<ParticipantAccount>();
    public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();
    
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
            .ToTable("users");
        
        modelBuilder.Entity<User>()
            .Property(u => u.SocialNetworks)
            .HasConversion(
                u => JsonSerializer.Serialize(u, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IReadOnlyList<SocialNetwork>>(json, JsonSerializerOptions.Default)!,
                new ValueComparer<IReadOnlyList<SocialNetwork>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                    c => c.ToList()));

        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users);
        
        modelBuilder.Entity<ParticipantAccount>()
            .ToTable("participant_accounts");
        
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
            .WithOne()
            .HasForeignKey<ParticipantAccount>(pa => pa.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ParticipantAccount>()
            .HasKey(pa => pa.Id);
        
        modelBuilder.Entity<AdminProfile>()
            .ToTable("admin_profiles");
        
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
            .WithOne()
            .HasForeignKey<AdminProfile>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<AdminProfile>()
            .HasKey(pa => pa.Id);

        modelBuilder.Entity<VolunteerAccount>()
            .ToTable("volunteer_accounts");
        
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
            .WithOne()
            .HasForeignKey<VolunteerAccount>(va => va.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<VolunteerAccount>()
            .HasKey(pa => pa.Id);
        
        modelBuilder.Entity<Role>()
            .ToTable("roles");

        modelBuilder.Entity<Role>()
            .Navigation(r => r.RolePermissions)
            .AutoInclude();
        
        modelBuilder.Entity<Permission>()
            .ToTable("permissions");

        modelBuilder.Entity<Permission>()
            .HasIndex(p => p.Code)
            .IsUnique();
        
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

        modelBuilder.Entity<RolePermission>()
            .Navigation(rp => rp.Permission)
            .AutoInclude();

        modelBuilder.Entity<RefreshSession>()
            .ToTable("refresh_sessions");

        modelBuilder.Entity<RefreshSession>()
            .HasOne(rs => rs.User)
            .WithMany()
            .HasForeignKey(rs => rs.UserId);
        
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
        
        modelBuilder.HasDefaultSchema("accounts");
    }

    private static readonly ILoggerFactory CreateLoggerFactory
        = LoggerFactory.Create(builder => { builder.AddConsole(); });
    
}