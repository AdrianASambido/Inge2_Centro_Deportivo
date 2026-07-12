using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase.DTOsTurno
{
    public class ClaseAdelantada
    {
        public Turno TurnoRepresentativo { get; set; } = null!;
        public List<Turno> TurnosDelMes { get; set; } = new();
        public int CupoMinimoDisponible { get; set; }
        public decimal PrecioTotal { get; set; }
        public decimal PrecioTotalConDescuento { get; set; }
        public bool AplicaDescuento { get; set; }


    }
}