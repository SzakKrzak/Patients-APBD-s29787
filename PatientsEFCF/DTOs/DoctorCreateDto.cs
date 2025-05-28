using System.ComponentModel.DataAnnotations;

namespace PatientsEFCF.DTOs;

public class DoctorCreateDto
{
    [Required] 
    public string FirstName { get; set; } = null!;

    [Required] 
    public string LastName { get; set; } = null!;

    [Required] public string Email { get; set; } = null!;

    public ICollection<int> Prescriptions { get; set; } = null!;
}