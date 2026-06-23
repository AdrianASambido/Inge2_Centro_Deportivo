using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Entidades
{
    public class Credito
    {
        public int Id { get; private set; }
        public int Id_Usuario { get; set; }
        public int Id_Actividad { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public EstadoCredito Estado {  get; set; }
        public Usuario Usuario { get; set; }
        public Actividad Actividad { get; set; }

        public Credito() { }

        public Credito(int idUsuario, int idActividad) { 
            this.Id_Usuario = idUsuario;
            this.Id_Actividad = idActividad;
            this.FechaGeneracion = DateTime.Now;
            this.FechaVencimiento = this.FechaGeneracion.AddDays(30);
            this.Estado = EstadoCredito.Disponible;
        }

    }
}