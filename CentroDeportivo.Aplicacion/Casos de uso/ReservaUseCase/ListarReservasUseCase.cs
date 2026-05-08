using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class ListarReservasUseCase (IReservaRepositorio repo)
    {
        public async Task<IEnumerable<Reserva>> Ejecutar(int idUsuario,int? idActividad = null,EstadoReserva? estado = null,bool pasadas = false) {
            return await repo.ObtenerPorUsuarioAsync(idUsuario,idActividad,estado,pasadas);
        }
    }
}
