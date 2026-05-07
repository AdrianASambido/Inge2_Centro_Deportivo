using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.CanchaUseCase
{
    public class ListarCanchasUseCase
    {
        public async Task<IEnumerable<Cancha>> ejecutar() {
            return await repo.ObtenerTodasAsync();
        }
    }
}
