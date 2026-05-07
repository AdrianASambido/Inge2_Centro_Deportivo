using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase
{
    public class ListarEmpleadosUseCase (IUsuarioRepositorio repo)
    {
        public async Task<IEnumerable<Usuario>> ejecutar() { 
            return await repo.ObtenerEmpleadosAsync();
        }
    }
}
