using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface IPagoServicio
    {
        Task<bool> ProcesarReembolsoAsync(int idUsuario, decimal montoADevolver);
        Task<bool> ProcesarCobroAsync(int idUsuario, decimal monto, string token);
        // Devuelve la URL de la preferencia de Mercado Pago (el SandboxInitPoint)
        Task<string> CrearPreferenciaPagoAsync(int idUsuario, decimal monto, string nombreActividad, string urlexito,string urlfallo);
    }
}