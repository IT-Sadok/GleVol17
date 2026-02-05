using MedicTrack.Domain.Entities;
using MedicTrack.Infrastructure.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicTrack.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<AppUser>(options)
{
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<DoctorUnavailability> DoctorUnavailabilities => Set<DoctorUnavailability>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Patient>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            e.Property(x => x.LastName).IsRequired().HasMaxLength(100);

        });

        builder.Entity<Doctor>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            e.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            e.Property(x => x.Specialization).IsRequired().HasMaxLength(150);
        });

        builder.Entity<Appointment>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.StartTimeUtc).IsRequired();
            e.Property(x => x.EndTimeUtc).IsRequired();

            e.Property(x => x.Status).IsRequired();

            e.Property(x => x.Note).HasMaxLength(500);

            e.HasOne(x => x.Patient)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Doctor)
                .WithMany(x => x.Appointments)
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<DoctorUnavailability>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.StartUtc).IsRequired();
            e.Property(x => x.EndUtc).IsRequired();

            e.Property(x => x.Reason).HasMaxLength(300);

            e.HasOne(x => x.Doctor)
                .WithMany(x => x.Unavailabilities)
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        builder.Entity<AppUser>()
            .Property(x => x.BirthDate)
            .HasColumnType("date");
    }
}