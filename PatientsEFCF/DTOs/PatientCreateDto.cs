using System.ComponentModel.DataAnnotations;

namespace PatientsEFCF.DTOs;

public class PatientCreateDto
{
    [Required] 
    public string FirstName { get; set; } = null!;

    [Required] 
    public string LastName { get; set; } = null!;
    
    [Required]
    public DateTime Birthdate { get; set; }

    public ICollection<int> Prescriptions { get; set; } = null!;
}