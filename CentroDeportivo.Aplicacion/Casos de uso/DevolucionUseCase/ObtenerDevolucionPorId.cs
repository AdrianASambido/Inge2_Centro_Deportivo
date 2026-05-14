using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.DevolucionUseCase
{
    public class ObtenerDevolucionPorId(IDevolucionRepositorio repo)
    {
        public async Task<Devolucion> Ejecutar(int idDevolucion) { 
            var devolucion = await repo.ObtenerPorIdAsync(idDevolucion);
            if (devolucion == null) {
                throw new Exception("Error: devolucion inexistente");
            }
            return devolucion;
        }
    }
}