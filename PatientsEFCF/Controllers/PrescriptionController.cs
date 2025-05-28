using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PatientsEFCF.DTOs;
using PatientsEFCF.Exceptions;
using PatientsEFCF.Models;
using PatientsEFCF.Services;

namespace PatientsEFCF.Controllers;


[ApiController]
[Route("[controller]")]
public class PrescriptionController(IDbService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionCreateDto prescriptionData)
    {
        try
        {
            var prescription = await service.CreatePrescriptionAsync(prescriptionData);
            return CreatedAtAction(nameof(GetPrescriptionDetails), new { id = prescription.IdPrescription }, prescription);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPrescriptionDetails(int id)
    {
        try
        {
            return Ok(await service.GetPrescriptionDetailsByIdAsync(id));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

}