using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;

namespace Tutorial5.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly DatabaseContext _context;

    public PatientsController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientDetails(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        if (patient == null)
            return NotFound($"patient not found with id {id}");

        var result = new
        {
            patient.IdPatient,
            patient.FirstName,
            patient.LastName,
            patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new
                {
                    p.IdPrescription,
                    p.Date,
                    p.DueDate,
                    Medicaments = p.PrescriptionMedicaments.Select(pm => new
                    {
                        pm.IdMedicament,
                        pm.Medicament.Name,
                        pm.Dose,
                        pm.Description
                    }),
                    Doctor = new
                    {
                        p.Doctor.IdDoctor,
                        p.Doctor.FirstName,
                        p.Doctor.LastName
                    }
                })
        };

        return Ok(result);
    }
}