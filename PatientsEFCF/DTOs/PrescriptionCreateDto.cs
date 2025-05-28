using System.Collections;
using System.ComponentModel.DataAnnotations;
using PatientsEFCF.Controllers;
using PatientsEFCF.Models;

namespace PatientsEFCF.DTOs;

public class PrescriptionCreateDto
{
    [Required] public DateTime Date { get; set; }

    [Required] public DateTime DueDate { get; set; }

    [Required] public PatientCreateSimpleDto Patient { get; set; }

    [Required] public DoctorCreateSimpleDto Doctor { get; set; }

    public ICollection<PrescriptionCreateDtoMedicament> Medicaments { get; set; }
}

public class PatientCreateSimpleDto
{
    public int PatientId { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
}

public class DoctorCreateSimpleDto
{
    public int DoctorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

public class PrescriptionCreateDtoMedicament
{
    public int MedicamentId { get; set; }
    public int? Dose { get; set; }
    public string Details { get; set; } = null!;
}