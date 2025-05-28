using PatientsEFCF.Models;

namespace PatientsEFCF.DTOs;

public class PatientGetDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime Birthdate { get; set; }
    public ICollection<PrescriptionGetDto>? Prescriptions { get; set; }
}