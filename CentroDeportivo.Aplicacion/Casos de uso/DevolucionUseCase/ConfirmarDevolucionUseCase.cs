using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.DevolucionUseCase
{
    public class ConfirmarDevolucionUseCase(IDevolucionRepositorio repo)
    {
        public async Task Ejecutar(int idDevolucion) { 
            var devolucion = await repo.ObtenerPorIdAsync(idDevolucion);

            if (devolucion == null) {
                throw new Exception("Error: devolucion inexistente.");
            }

            if (devolucion.Estado == DevolucionEstado.Confirmado || devolucion.Estado == DevolucionEstado.Cancelado) {
                throw new Exception("Error: no se puede confirmar esta devolucion.");
            }

            devolucion.Estado = DevolucionEstado.Confirmado;
            devolucion.FechaDevolucion = DateTime.Now;
            await repo.ActualizarAsync(devolucion);
        }
    }
}
