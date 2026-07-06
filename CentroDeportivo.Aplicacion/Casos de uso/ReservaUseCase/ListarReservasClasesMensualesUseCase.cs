using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class ListarReservasClasesMensualesUseCase(IReservaRepositorio repoReserva)
    {
        public async Task<List<Reserva>> Ejecutar(int idUsuario)
        {
            return await repoReserva.ObtenerPaquetesAdelantatadosPorUsuarioAsync(idUsuario);
        }
    }
}
