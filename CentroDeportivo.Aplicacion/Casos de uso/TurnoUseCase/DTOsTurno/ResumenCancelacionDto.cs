using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase.DTOsTurno
{
    public class ResumenCancelacionDto
    {
        public int TotalInscriptosAfectados { get; set; }
        public int CreditosDevueltos { get; set; }
        public decimal MontoTotalDineroDevuelto { get; set; }
    }
}
