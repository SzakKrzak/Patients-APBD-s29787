using Microsoft.AspNetCore.Mvc;
using PatientsEFCF.DTOs;
using PatientsEFCF.Exceptions;
using PatientsEFCF.Services;

namespace PatientsEFCF.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientController(IDbService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePatient([FromBody] PatientCreateDto patientData)
    {
        var patient = await service.CreatePatientAsync(patientData);
        return CreatedAtAction(nameof(GetPatientDetailsById), new { id = patient.IdPatient }, patient);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientDetailsById(int id)
    {
        try
        {
            return Ok(await service.GetPatientDetailsByIdAsync(id));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}