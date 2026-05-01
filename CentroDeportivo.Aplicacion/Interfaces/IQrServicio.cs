using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface IQrServicio
    {
        string GenerarToken();
        byte[] GenerarImagenQr(string token);
    }
}
