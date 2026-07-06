using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase.DTOsTurno
{
    public class ResumenEliminarInscripto
    {
        public TipoReserva TipoReserva { get; init; }
        public bool ConCredito { get; init; }
        public decimal MontoDineroDevuelto { get; init; }
    }
}
