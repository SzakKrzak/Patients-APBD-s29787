using PatientsEFCF.Models;

namespace PatientsEFCF.DTOs;

public class DoctorGetDto
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public ICollection<PrescriptionGetDto>? Prescriptions { get; set; } = null!;
}