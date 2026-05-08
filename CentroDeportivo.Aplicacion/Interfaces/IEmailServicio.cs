using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface IEmailServicio
    {
        Task EnviarRecordatorioTurnoAsync(string email, Turno turno);
        Task EnviarContraseniaTemporalAsync(string email, string contraseniaTemporal);
        Task EnviarLinkRecuperacionAsync(string email, string link);
        Task EnviarCupoListaEsperaAsync(string email, Turno turno, int minutosParaResponder);
    }
}
