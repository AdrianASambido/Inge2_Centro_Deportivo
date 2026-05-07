using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.CanchaUseCase
{
<<<<<<< HEAD
    public class ListarCanchasUseCase
=======
    public class ListarCanchasUseCase (ICanchaRepositorio repo)
>>>>>>> d2f48e2d146661fe7a41a53c37d5e54b8d046751
    {
        public async Task<IEnumerable<Cancha>> ejecutar() {
            return await repo.ObtenerTodasAsync();
        }
    }
}
