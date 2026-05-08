using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.DevolucionUseCase
{
    public class ListarDevolucionesPendientesUseCase (IDevolucionRepositorio repo)
    {
        public async Task<IEnumerable<Devolucion>> ejecutar() { 
            return await repo.ObtenerPendientesAsync();
        }
    }
}
