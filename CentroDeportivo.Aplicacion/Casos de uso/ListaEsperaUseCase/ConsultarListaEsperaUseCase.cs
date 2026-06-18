using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ListaEsperaUseCase
{
    public class ConsultarListaEsperaUseCase (IListaDeEsperaRepositorio repoLista)
    {
        public async Task<IEnumerable<InscripcionListaEspera>> Ejecutar(int idUsuario)
        {
            var todas = await repoLista.ObtenerTodosPorUsuarioAsync(idUsuario);

            return todas
                .Where(x => x.Estado == EstadoListaEspera.Esperando || x.Estado == EstadoListaEspera.Notificado)
                .OrderByDescending(x => x.Estado)
                .ToList();
        }
    }
}
