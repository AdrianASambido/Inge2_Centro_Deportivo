using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Servicios
{
    public class QrServicio : IQrServicio
    {
        public byte[] GenerarImagenQr(string token)
        {
            throw new NotImplementedException();
        }

        public string GenerarToken()
        {
            throw new NotImplementedException();
        }
    }
}
