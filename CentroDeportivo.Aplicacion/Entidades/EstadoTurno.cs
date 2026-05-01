using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Entidades
{
    public enum EstadoTurno
    {
        Disponible,   // Hay cupo o el turno está activo.
        Lleno,        // Se completó el cupo máximo.
        Cancelado,    // El negocio decidió dar de baja este turno (ej: mantenimiento).
        Finalizado    // La hora ya pasó.
    }
}
