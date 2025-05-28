using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PatientsEFCF.Models;

[Table("PrescriptionMedicament")]
[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
public class PrescriptionMedicament
{
    [Column("IdMedicament")]
    public int IdMedicament { get; set; }

    [Column("IdPrescription")]
    public int IdPrescription { get; set; }
    
    public int? Dose { get; set; }

    [MaxLength(100)] public string Details { get; set; } = null!;

    [ForeignKey(nameof(IdMedicament))] 
    public Medicament Medicament { get; set; } = null!;

    [ForeignKey(nameof(IdPrescription))]
    public Prescription Prescription { get; set; } = null!;
}