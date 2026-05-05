using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace CentroDeportivo.Infraestructura.Servicios
{
    public class ServicioHash : IHashServicio
    {
        public string Hashear(string texto)
        {
            return BCrypt.Net.BCrypt.HashPassword(texto);
        }

        public bool Verificar(string texto, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(texto, hash);
        }
    }
}
