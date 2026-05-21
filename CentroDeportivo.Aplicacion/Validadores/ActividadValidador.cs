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

            
            if (string.IsNullOrWhiteSpace(actividad.Nombre) || string.IsNullOrWhiteSpace(actividad.Descripcion))
            {
                mensaje += "Error: Debe completar todos los campos.\n";
            }

            
            if (actividad.Precio <= 0)
            {
                mensaje += "Error: El precio debe ser mayor a 0.\n";
            }


            if (!string.IsNullOrWhiteSpace(actividad.Nombre))
            {
                // Normalizo el nombre
                string nombreLimpio = NormalizarTexto(actividad.Nombre);

                // El Repo ahora buscade indiferente a mayúsculas y acentos
                if (await repo.YaExiste(nombreLimpio))
                {
                    mensaje += $"Error: ya existe una actividad con ese nombre.\n";
                }
            }


            return (string.IsNullOrEmpty(mensaje), mensaje);
        }

        private string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "";

            // Pasa a mayús y normaliza para separar caracteres de sus acentos
            string textoNormalizado = texto.ToUpper().Normalize(NormalizationForm.FormD);

            // Filtra solo los caracteres que no sean acentos
            var sinAcentos = textoNormalizado
                .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                .ToArray();

            return new string(sinAcentos).Normalize(NormalizationForm.FormC);
        }

        public async Task<(bool esValido, string mensaje)> validarEliminacion(int idActividad)
        {
            var actividad = await repo.ObtenerPorIdAsync(idActividad);

            if (actividad == null)
            {
                return (false, "Actividad inexistente.");
            }

            if (await repo.TieneInscriptosAsync(idActividad))
            {
                return (false, "Error: esta actividad registra turnos activos, no puede eliminarse");
            }

            return (true, "");
        }
        public async Task<(bool esValido, string mensaje)> ValidarEdicion(Actividad actividad, int idActividad)
        {
            string mensaje = "";

           
            if (string.IsNullOrWhiteSpace(actividad.Nombre) ||
                string.IsNullOrWhiteSpace(actividad.Descripcion))
            {
                mensaje += "Error: Debe completar todos los campos.\n";
            }

           
            if (actividad.Precio <= 0)
            {
                mensaje += "Error: El precio debe ser mayor a 0.\n";
            }

            
            if (!string.IsNullOrWhiteSpace(actividad.Nombre))
            {
                
                

             
                if (await repo.YaExisteParaEditar(actividad.Nombre, idActividad))
                {
                    mensaje += $"Error: ya existe una actividad con ese nombre.'\n";
                }
            }

            return (string.IsNullOrEmpty(mensaje), mensaje);
        }
    }
}