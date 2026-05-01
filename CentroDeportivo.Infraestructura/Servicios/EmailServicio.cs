using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Servicios
{
    public class EmailServicio : IEmailServicio
    {
        public Task EnviarContraseniaTemporalAsync(string email, string contraseniaTemporal)
        {
            throw new NotImplementedException();
        }

        public Task EnviarLinkRecuperacionAsync(string email, string link)
        {
            throw new NotImplementedException();
        }

        public Task EnviarRecordatorioTurnoAsync(string email, Turno turno)
        {
            throw new NotImplementedException();
        }
    }
}
