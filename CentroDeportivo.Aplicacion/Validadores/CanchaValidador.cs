using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;

namespace CentroDeportivo.Aplicacion.Validadores
{
    public class CanchaValidador (ICanchaRepositorio repo)
    {
        public async Task<(bool esValido, string mensaje)> validar(Cancha c) { 

            string mensaje = "";

            
            if (c.Numero <= 0) 
            {
                mensaje += "Ingrese un número de cancha válido (mayor a 0).\n";
            }
            else
            {
                
                if (await repo.YaExiste(c.Numero))
                {
                    mensaje += "El número de cancha ya existe.\n";
                }
            }
            if (c.Capacidad < 1)
            {
                mensaje += "Ingrese una capacidad valida.\n";
            }

            return (string.IsNullOrEmpty(mensaje),mensaje);
        }
        public async Task<(bool esValido, string mensaje)> ValidarEliminacion(int id) {
            var cancha = await repo.ObtenerPorIdAsync(id);
            if (cancha == null) {
                return (false, "Cancha inexistente");
            }
            if (await repo.TieneTurnosAsignadosAsync(id)){
                return (false, "Error al eliminar, la cancha tiene turnos asignados");
            }
            return (true, "");
        }
    }
    }
