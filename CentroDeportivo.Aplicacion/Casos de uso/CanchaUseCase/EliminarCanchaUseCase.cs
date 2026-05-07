using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.CanchaUseCase
{
<<<<<<< HEAD
    public class EliminarCanchaUseCase
=======
    public class EliminarCanchaUseCase(ICanchaRepositorio repo, CanchaValidador validador)
>>>>>>> d2f48e2d146661fe7a41a53c37d5e54b8d046751
    {
        public async Task ejecutar(int id) {
            var (esValido, mensaje) = await validador.ValidarEliminacion(id);
            if (!esValido) {
                throw new Exception(mensaje);
            }
            await repo.EliminarAsync(id);
        }
    }
}
