using CentroDeportivo.Infraestructura.Persistencia.Contexto;
using Microsoft.AspNetCore.Mvc;

namespace CentroDeportivo.UI.Controllers;

[ApiController]
[Route("api/dev")]
public class DevController : ControllerBase
{
    private readonly CentroDeportivoContext _context;

    public DevController(CentroDeportivoContext context)
    {
        _context = context;
    }

    [HttpPost("insertpago")] 
    public IActionResult InsertPago([FromQuery] int reservaId = 1)
    {
        var reserva = _context.Reservas.Find(reservaId);
        if (reserva == null) return NotFound("Reserva no encontrada");

        var pago = new CentroDeportivo.Aplicacion.Entidades.Pago(reserva.Id_Usuario, reserva.PrecioPagado, reserva.Id, reserva.Id_Turno);
        _context.Pagos.Add(pago);
        _context.SaveChanges();

        return Ok(new { message = "Pago insertado", pagoId = pago.Id });
    }
}
