using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class RenovarReservaAdelantadaUseCase(
    CrearReservaAdelantadaUseCase crearReservaUseCase,
    ITurnoRepositorio repoTurno,
    IUsuarioRepositorio repoUsuario)
    {
        public async Task Ejecutar(int idUsuario, List<int> idsTurnos, string paymentId)
        {
            var turnos = new List<Turno>();
            foreach (var id in idsTurnos)
            {
                var turno = await repoTurno.ObtenerPorIdAsync(id)
                    ?? throw new Exception($"Turno {id} no encontrado.");
                turnos.Add(turno);
            }

 
            var usuario = await repoUsuario.ObtenerPorIdAsync(idUsuario)
                ?? throw new Exception("Usuario no encontrado.");


            await crearReservaUseCase.Ejecutar(idUsuario, turnos, paymentId);
        }
    }
}
