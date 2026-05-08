using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Validadores
{
    public class ActividadValidador(IActividadRepositorio repo)
    {
        public async Task<(bool esValido, string mensaje)> Validar(Actividad actividad)
        {
            string mensaje = "";

            // 1. Validaciones de campos obligatorios
            if (string.IsNullOrWhiteSpace(actividad.Nombre) || string.IsNullOrWhiteSpace(actividad.Descripcion))
            {
                mensaje += "Error: Debe completar todos los campos.\n";
            }

            // 2. Validación de lógica de negocio (Precio)
            if (actividad.Precio < 0)
            {
                mensaje += "Error: El precio debe ser mayor a 0.\n";
            }

            // 3. Validación contra la Base de Datos (Asincrónica)
            // Solo chequeamos si existe si el nombre no está vacío para evitar consultas innecesarias
            if (!string.IsNullOrWhiteSpace(actividad.Nombre))
            {
                if (await repo.YaExiste(actividad.Nombre))
                {
                    mensaje += "Error: Esta actividad ya existe.\n";
                }
            }

            // Retornamos la tupla: es válido si el mensaje quedó vacío
            return (string.IsNullOrEmpty(mensaje), mensaje);
        }

        public async Task<(bool esValido, string mensaje)> validarEliminacion(int idActividad) { 
            var actividad = await repo.ObtenerPorIdAsync(idActividad);

            if (actividad == null) {
                return (false, "Actividad inexistente.");
            }

            if (await repo.TieneInscriptosAsync(idActividad)) {
                return (false, "Error: la actividad posee turnos programados.");
            }

            return (true, "");
        }
    }
}
