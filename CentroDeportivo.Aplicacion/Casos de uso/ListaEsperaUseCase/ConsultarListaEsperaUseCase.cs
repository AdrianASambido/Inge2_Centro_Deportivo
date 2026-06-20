using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ListaEsperaUseCase
{
    public class ConsultarListaEsperaUseCase(IListaDeEsperaRepositorio repoLista)
    {
        public async Task<List<InscripcionListaEspera>> Ejecutar(int idUsuario)
        {
            var todas = await repoLista.ObtenerTodosPorUsuarioAsync(idUsuario);

            DateTime ahora = DateTime.Now;

            return todas
                .Where(x =>

                    (x.Estado == EstadoListaEspera.Esperando || x.Estado == EstadoListaEspera.Notificado)


                    && x.Turno != null
                    && x.Turno.Estado != EstadoTurno.Cancelado

                    && x.Turno.Fecha.ToDateTime(x.Turno.HoraInicio) > ahora)
                .OrderByDescending(x => x.Estado)
                .ToList();
        }
    }
}