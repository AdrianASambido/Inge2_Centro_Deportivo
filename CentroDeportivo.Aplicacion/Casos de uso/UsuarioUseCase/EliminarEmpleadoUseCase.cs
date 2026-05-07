using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase
{
    public class EliminarEmpleadoUseCase (IUsuarioRepositorio repo, UsuarioEmpleadoValidador validador)
    {
        public async Task ejecutar(int idEmpleado) { 
            var (esValido, mensaje) = await validador.validarEliminar(idEmpleado);

            if (!esValido) {
                throw new Exception(mensaje);
            }

            await repo.EliminarAsync(idEmpleado);
        }
    }
}
