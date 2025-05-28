using Microsoft.EntityFrameworkCore;
using PatientsEFCF.Data;
using PatientsEFCF.DTOs;
using PatientsEFCF.Exceptions;
using PatientsEFCF.Models;

namespace PatientsEFCF.Services;

public interface IDbService
{
    public Task<PrescriptionGetDto> CreatePrescriptionAsync(PrescriptionCreateDto prescriptionData);
    public Task<PrescriptionGetDto> GetPrescriptionDetailsByIdAsync(int id);
    public Task<PatientGetDto> GetPatientDetailsByIdAsync(int id);
    public Task<PatientGetDto> CreatePatientAsync(PatientCreateDto patientData);
}

public class DbService(AppDbContext data) : IDbService
{
    public async Task<PrescriptionGetDto> CreatePrescriptionAsync(PrescriptionCreateDto prescriptionData)
    {
        List<Medicament> medicaments = [];

        if (prescriptionData.Medicaments is not null && prescriptionData.Medicaments.Count != 0)
        {
            foreach (var medicamentD in prescriptionData.Medicaments)
            {
                var medicament = await data.Medicaments.FirstOrDefaultAsync(m => m.IdMedicament == medicamentD.MedicamentId);
                if (medicament is null)
                {
                    throw new NotFoundException($"Medicament with id {medicamentD.MedicamentId} not found");
                }
                medicaments.Add(medicament);
            }
        }

        if (medicaments.Count > 10)
        {
            throw new ArgumentException(
                $"Too many medicaments assigned to a single prescription. Max is 10. Is {medicaments.Count}");
        }

        Doctor doctor = await data.Doctors.FirstOrDefaultAsync(d => d.IdDoctor == prescriptionData.Doctor.DoctorId);
        if (doctor is null)
        {
            await CreateDoctorAsync(new DoctorCreateDto
            {
                FirstName = prescriptionData.Doctor.FirstName,
                LastName = prescriptionData.Doctor.LastName,
                Email = prescriptionData.Doctor.Email
            });
        }

        Patient patient = await data.Patients.FirstOrDefaultAsync(p => p.IdPatient == prescriptionData.Patient.PatientId);
        if (patient is null)
        {
            await CreatePatientAsync(new PatientCreateDto
            {
                FirstName = prescriptionData.Patient.FirstName,
                LastName = prescriptionData.Patient.LastName,
                Birthdate = prescriptionData.Patient.Birthdate,
            });
        }

        var prescription = new Prescription
        {
            Date = prescriptionData.Date,
            DueDate = prescriptionData.DueDate,
            IdDoctor = doctor.IdDoctor,
            IdPatient = patient.IdPatient,
            PrescriptionMedicaments = prescriptionData.Medicaments.Select(medicament =>
                new PrescriptionMedicament
                {
                    IdMedicament = medicament.MedicamentId,
                    Dose = medicament.Dose,
                    Details = medicament.Details
                }).ToList()
        };


        await data.Prescriptions.AddAsync(prescription);
        await data.SaveChangesAsync();

        return new PrescriptionGetDto
        {
            IdPrescription = prescription.IdPrescription,
            Date = prescription.Date,
            DueDate = prescription.DueDate,
            IdDoctor = prescription.IdDoctor,
            IdPatient = prescription.IdPatient,
            Medicaments = medicaments.Select(m => new MedicamentGetDto
            {
                IdMedicament = m.IdMedicament
            }).ToList()
        };
    }

    public async Task<PatientGetDto> GetPatientDetailsByIdAsync(int id)
    {
        var result = await data.Patients
            .Where(p => p.IdPatient == id)
            .Select(p => new PatientGetDto
        {
            IdPatient = p.IdPatient,
            Birthdate = p.Birthdate,
            FirstName = p.Firstname,
            LastName = p.LastName,
            Prescriptions = p.Prescriptions.Select(pr=> new PrescriptionGetDto
            {
                IdPrescription = pr.IdPrescription,
                Date = pr.Date,
                DueDate = pr.DueDate,
                IdDoctor = pr.IdDoctor,
                IdPatient = pr.IdPatient,
                Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentGetDto
                {
                    Description = pm.Medicament.Description,
                    IdMedicament = pm.IdMedicament,
                    Name = pm.Medicament.Name,
                    Type = pm.Medicament.Type
                }).ToList()
            }).ToList()
        }).FirstOrDefaultAsync();
        return result ?? throw new NotFoundException($"Patient with id: {id} not found");
    }

    public async Task<DoctorGetDto> CreateDoctorAsync(DoctorCreateDto doctorData)
    {
        List<Prescription> prescriptions = [];

        if (doctorData.Prescriptions is not null && doctorData.Prescriptions.Count != 0)
        {
            foreach (var prescriptionId in doctorData.Prescriptions)
            {
                var prescription = await data.Prescriptions.FirstOrDefaultAsync(p => p.IdPrescription == prescriptionId);
                if (prescriptions is null)
                {
                    throw new NotFoundException($"Prescription with id {prescriptionId} not found");
                }

                prescriptions.Add(prescription);
            }
        }

        Doctor doctor = new Doctor
        {
            Email = doctorData.Email,
            FirstName = doctorData.FirstName,
            LastName = doctorData.LastName,
            Prescriptions = (doctorData.Prescriptions ?? []).Select(prescriptionId=>new Prescription
            {
                IdPrescription = prescriptionId
            }).ToList()
        };

        await data.Doctors.AddAsync(doctor);
        await data.SaveChangesAsync();

        return new DoctorGetDto()
        {
            IdDoctor = doctor.IdDoctor,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Email = doctor.Email,
            Prescriptions = prescriptions.Select(prescription => new PrescriptionGetDto
            {
                Date = prescription.Date,
                DueDate = prescription.DueDate,
                IdPrescription = prescription.IdPrescription
            }).ToList()
        };
    }
    public async Task<PatientGetDto> CreatePatientAsync(PatientCreateDto patientData)
    {
        List<Prescription> prescriptions = [];

        if (patientData.Prescriptions is not null && patientData.Prescriptions.Count != 0)
        {
            foreach (var prescriptionId in patientData.Prescriptions)
            {
                var prescription = await data.Prescriptions.FirstOrDefaultAsync(p => p.IdPrescription == prescriptionId);
                if (prescriptions is null)
                {
                    throw new NotFoundException($"Prescription with id {prescriptionId} not found");
                }

                prescriptions.Add(prescription);
            }
        }

        Patient patient = new Patient
        {
            Birthdate = patientData.Birthdate,
            Firstname = patientData.FirstName,
            LastName = patientData.LastName,
            Prescriptions = (patientData.Prescriptions ?? []).Select(prescriptionId=>new Prescription
            {
                IdPrescription = prescriptionId
            }).ToList()
        };

        await data.Patients.AddAsync(patient);
        await data.SaveChangesAsync();

        return new PatientGetDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.Firstname,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = prescriptions.Select(prescription => new PrescriptionGetDto
            {
                Date = prescription.Date,
                DueDate = prescription.DueDate,
                IdPrescription = prescription.IdPrescription
            }).ToList()
        };
    }

    public async Task<PrescriptionGetDto> GetPrescriptionDetailsByIdAsync(int prescriptionId)
    {
        var result = await data.Prescriptions
            .Where(p => p.IdPrescription == prescriptionId)
            .Select(pr => new PrescriptionGetDto
        {
            IdPrescription = pr.IdPrescription,
            Date = pr.Date,
            DueDate = pr.DueDate,
            IdDoctor = pr.IdDoctor,
            IdPatient = pr.IdPatient,
            Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentGetDto
            {
                IdMedicament = pm.IdMedicament,
                Description = pm.Medicament.Description,
                Name = pm.Medicament.Name,
                Type = pm.Medicament.Type
            }).ToList()
        }).FirstOrDefaultAsync(e => e.IdPrescription == prescriptionId);
        return result ?? throw new NotFoundException($"Prescription with id: {prescriptionId} not found");
    }
}