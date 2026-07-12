using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase
{
    public class ObtenerDetalleRenovacionUseCase(
     IReservaRepositorio repoReserva,
     ITurnoRepositorio repoTurno,
     IUsuarioRepositorio repoUsuario)
    {
        public async Task<DetalleRenovacionDTO> Ejecutar(int idUsuario, Guid codigoPaquete)
        {
            var reservasPaquete = await repoReserva.ObtenerPorCodigoPaqueteAsync(codigoPaquete);
            var turnoReferencia = reservasPaquete.First().Turno!;

            var fechaReferencia = turnoReferencia.Fecha;
            int anioSiguiente = fechaReferencia.Month == 12 ? fechaReferencia.Year + 1 : fechaReferencia.Year;
            int mesSiguiente = fechaReferencia.Month == 12 ? 1 : fechaReferencia.Month + 1;

            var turnosSiguienteMes = await repoTurno.ObtenerTurnosSiguienteMesPorClaseAsync(
                turnoReferencia.Id_Actividad,
                turnoReferencia.Fecha.DayOfWeek,
                turnoReferencia.HoraInicio,
                turnoReferencia.Id_Profesor,
                turnoReferencia.Id_Cancha,
                anioSiguiente,
                mesSiguiente);

            var usuario = await repoUsuario.ObtenerPorIdAsync(idUsuario)!;
            bool aplicaDescuento = !usuario!.TieneSancionVigente();

            decimal precioUnitario = turnosSiguienteMes.Any()
                ? turnosSiguienteMes.First().PrecioTurno
                : turnoReferencia.PrecioTurno;

            int cantidadClases = turnosSiguienteMes.Count;
            decimal precioTotal = precioUnitario * cantidadClases;
            decimal precioConDescuento = aplicaDescuento ? precioTotal * 0.80m : precioTotal;

            bool yaRenovo = await repoReserva.YaRenovoParaSiguienteMesAsync(
    idUsuario,
    turnoReferencia.Id_Actividad,
    turnoReferencia.Fecha.DayOfWeek,
    turnoReferencia.HoraInicio,
    anioSiguiente,
    mesSiguiente);

            return new DetalleRenovacionDTO
            {
                CantidadClases = cantidadClases,
                PrecioUnitario = precioUnitario,
                PrecioTotal = precioTotal,
                PrecioConDescuento = precioConDescuento,
                YaRenovo = yaRenovo,
                AplicaDescuento = aplicaDescuento,
                TurnosDisponibles = turnosSiguienteMes.Any(),
                NombreMes = new DateTime(anioSiguiente, mesSiguiente, 1)
                    .ToString("MMMM yyyy", new CultureInfo("es-AR"))
            };
        }
    }

    public class DetalleRenovacionDTO
    {
        public int CantidadClases { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioTotal { get; set; }
        public decimal PrecioConDescuento { get; set; }
        public bool AplicaDescuento { get; set; }
        public bool TurnosDisponibles { get; set; }
        public string NombreMes { get; set; } = "";
        public bool YaRenovo { get; set; }
    }
}
