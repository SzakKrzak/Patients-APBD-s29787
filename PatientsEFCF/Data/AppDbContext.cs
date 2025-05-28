using Microsoft.EntityFrameworkCore;
using PatientsEFCF.Models;

namespace PatientsEFCF.Data;

public class AppDbContext : DbContext
{
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Dane początkowe
        var doctors = new List<Doctor>
        {
            new Doctor
            {
                IdDoctor = 1,
                FirstName = "Mario",
                LastName = "Bro",
                Email = "Mario@gmail.com"
            }
        };

        var patients = new List<Patient>
        {
            new Patient
            {
                IdPatient = 1,
                Firstname = "Bowser",
                LastName = "Black",
                Birthdate = DateTime.Parse("2001-10-10")
            }
        };

        var medicaments = new List<Medicament>
        {
            new Medicament
            {
                IdMedicament = 1,
                Name = "Alegra",
                Description = "Opisik",
                Type = "anty-histaminowy"
            },
            new Medicament
            {
                IdMedicament = 2,
                Name = "Alertec",
                Description = "Fajny",
                Type = "anty-histaminowy"
            }
        };

        var prescriptions = new List<Prescription>
        {
            new Prescription
            {
                IdPrescription = 1,
                Date = DateTime.Parse("2024-01-01"),
                DueDate = DateTime.Parse("2024-12-31"),
                IdPatient = 1,
                IdDoctor = 1
            }
        };

        var prescriptionMedicaments = new List<PrescriptionMedicament>
        {
            new PrescriptionMedicament
            {
                IdMedicament = 1,
                IdPrescription = 1,
                Dose = 10,
                Details = "1 tabletka dziennie"
            },
            new PrescriptionMedicament
            {
                IdMedicament = 2,
                IdPrescription = 1,
                Dose = 5,
                Details = "2 razy dziennie"
            }
        };

        modelBuilder.Entity<Doctor>().HasData(doctors);
        modelBuilder.Entity<Patient>().HasData(patients);
        modelBuilder.Entity<Medicament>().HasData(medicaments);
        modelBuilder.Entity<Prescription>().HasData(prescriptions);
        modelBuilder.Entity<PrescriptionMedicament>().HasData(prescriptionMedicaments);

        base.OnModelCreating(modelBuilder);
    }
}
