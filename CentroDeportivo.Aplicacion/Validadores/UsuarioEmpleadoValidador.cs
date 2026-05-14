using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Validadores
{
    public class UsuarioEmpleadoValidador(IUsuarioRepositorio repo, IProfesorRepositorio repoProfe) : UsuarioValidadorBase(repo, repoProfe)
    {
       

        protected override (bool esValidoPass,string mensajePass) ValidarPassword(Usuario u)
        {
            return (true, "");
        }

        public async Task<(bool esValido,string mensaje)> validarEliminar(int idEmpleado) { 
            var usuario = await repo.ObtenerPorIdAsync(idEmpleado);

            if (usuario == null) {
                return (false, "Empleado no encontrado");
            }

            if (usuario.Rol != Rol.Empleado) {
                return (false, "El usuario seleccionado no es Empleado");
            }

            return (true, "");
        }
    }
}
