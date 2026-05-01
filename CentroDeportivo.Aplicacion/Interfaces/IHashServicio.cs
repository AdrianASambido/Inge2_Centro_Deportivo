using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Interfaces
{
    public interface IHashServicio
    {
        string Hashear(string texto);
        bool Verificar(string texto, string hash);
    }
}
