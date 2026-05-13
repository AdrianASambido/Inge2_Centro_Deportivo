using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.CanchaUseCase
{
    public class ObtenerCanchaPorId(ICanchaRepositorio repo)
    {
        public async Task<Cancha> Ejecutar(int idCancha) { 
            var cancha = await repo.ObtenerPorIdAsync(idCancha);
            if (cancha == null) {
                throw new Exception("Error: cancha inexistente");
            }
            return cancha;
        }
    }
}
