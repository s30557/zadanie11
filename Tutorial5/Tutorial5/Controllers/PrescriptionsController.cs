using Microsoft.AspNetCore.Mvc;
using Tutorial5.DTO;
using Tutorial5.Services;

namespace Tutorial5.Controllers;

[ApiController]
[Route("api/prescriptions")]
public class PrescriptionsController : ControllerBase
{
    private readonly IDbService _Service;

    public PrescriptionsController(IDbService Service)
    {
        _Service = Service;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionRequestDto dto)
    {
        if (dto == null)
            return BadRequest();

        try
        {
            await _Service.AddPrescriptionAsync(dto);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}
