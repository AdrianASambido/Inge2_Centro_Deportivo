using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ListaEsperaUseCase
{
    public class UnirseListaDeEsperaUseCase(IListaDeEsperaRepositorio repoLista, ITurnoRepositorio repoTurno)
    {
        public async Task Ejecutar(int idUsuario, int idTurno)
        {
            var turno = await repoTurno.ObtenerPorIdAsync(idTurno);
            if (turno == null)
            {
                throw new Exception("Error, turno inexistente");
            }

            if (turno.Estado != EstadoTurno.Lleno)
            {
                throw new Exception("Error: este turno no esta lleno");
            }

            var estaLista = await repoLista.ObtenerTodosPorUsuarioAsync(idUsuario);
            foreach (var i in estaLista)
            {
                if (i.Id_Turno == idTurno && i.Estado == EstadoListaEspera.Esperando || i.Estado == EstadoListaEspera.Notificado)
                {
                    throw new Exception("Error: ya te encuentras en la lista");
                }
            }

            var nuevaInscripcion = new InscripcionListaEspera
            {
                Id_Turno = idTurno,
                Id_Usuario = idUsuario,
                FechaInscripcion = DateTime.Now,
                Estado = EstadoListaEspera.Esperando,
                FechaNotificacion = null
            };

            await repoLista.AgregarAsync(nuevaInscripcion);
        }
    }
}