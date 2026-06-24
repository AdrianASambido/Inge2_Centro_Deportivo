using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ListaEsperaUseCase
{
    public class AbandonarListaEsperaUseCase(
        IListaDeEsperaRepositorio repoLista,
        ITurnoRepositorio repoTurno,
        IEmailServicio emailServicio
    )
    {
        public async Task Ejecutar(int idUsuario, int idTurno)
        {

            var inscripciones = await repoLista.ObtenerTodosPorUsuarioAsync(idUsuario);
            InscripcionListaEspera? inscripcionActual = null;

            foreach (var ins in inscripciones)
            {

                if (ins.Id_Turno == idTurno && (ins.Estado == EstadoListaEspera.Esperando || ins.Estado == EstadoListaEspera.Notificado))
                {
                    inscripcionActual = ins;
                    break;
                }
            }

            if (inscripcionActual == null)
            {
                throw new Exception("Error: No se encontró una inscripción activa en la lista de espera para este turno.");
            }


            var estadoAnterior = inscripcionActual.Estado;


            inscripcionActual.Estado = EstadoListaEspera.Expirado;
            await repoLista.ActualizarAsync(inscripcionActual);

            if (estadoAnterior == EstadoListaEspera.Notificado)
            {
                DateTime ahora = DateTime.Now;

                var siguienteEnFila = await repoLista.ObtenerPrimeroEnFilaAsync(idTurno);
                var turno = await repoTurno.ObtenerPorIdAsync(idTurno);

                if (siguienteEnFila != null)
                {
                    siguienteEnFila.Estado = EstadoListaEspera.Notificado;
                    siguienteEnFila.FechaNotificacion = ahora;

                    await emailServicio.EnviarAvisoVacanteListaEsperaAsync(siguienteEnFila.Usuario.Email, turno!);
                    await repoLista.ActualizarAsync(siguienteEnFila);

                    Console.WriteLine($"[Abandonar] Usuario {idUsuario} abandonó estando notificado. Se notificó al siguiente: {siguienteEnFila.Id_Usuario}");
                }
                else
                {
                    if (turno != null)
                    {
                        turno.CupoDisponible++;
                        if (turno.Estado == EstadoTurno.Lleno)
                        {
                            turno.Estado = EstadoTurno.Disponible;
                        }
                        await repoTurno.ActualizarAsync(turno);
                    }

                    Console.WriteLine($"[Abandonar] Usuario {idUsuario} abandonó estando notificado. Lista vacía, cupo devuelto al turno.");
                }
            }
            else
            {
                Console.WriteLine($"[Abandonar] Usuario {idUsuario} abandonó la lista mientras estaba en espera pasiva.");
            }
        }
    }
}