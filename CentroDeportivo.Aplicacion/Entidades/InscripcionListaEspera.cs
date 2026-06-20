using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Entidades
{
    public class InscripcionListaEspera
    {
        public int Id { get; private set; }
        public int Id_Turno { get; set; }
        public int Id_Usuario { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public EstadoListaEspera Estado {  get; set; }
        public DateTime? FechaNotificacion {  get; set; }
        public Usuario Usuario { get; set; }
        public Turno Turno { get; set; }

        public InscripcionListaEspera() { }

        public int ObtenerMinutosRestantes()
        {
            if (Estado != EstadoListaEspera.Notificado || !FechaNotificacion.HasValue)
                return 0;

            var tiempoTranscurrido = DateTime.Now - FechaNotificacion.Value;
            var restantes = 10 - (int)tiempoTranscurrido.TotalMinutes;

            return restantes > 0 ? restantes : 0;
        }

    }
}