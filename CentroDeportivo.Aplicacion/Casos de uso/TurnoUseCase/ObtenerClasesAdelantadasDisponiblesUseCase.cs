using CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase.DTOsTurno;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase
{
    public class ObtenerClasesAdelantadasDisponiblesUseCase(ITurnoRepositorio repoTurno)
    {
        //para mostrarle en la vista al cliente, si elige pesta;ia para reservar por adelantado
        public async Task<List<ClaseAdelantada>> Ejecutar(int idActividad, DayOfWeek diaSemana, int idUsuario, int anio, int mes)
        {
            DateOnly inicioDeMes = new DateOnly(anio, mes, 1);
            DateOnly finDeMes = new DateOnly(anio, mes, DateTime.DaysInMonth(anio, mes));
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Now);

            // si el mes elegido es el actual, empezamos desde hoy
            // si es un mes futuro, empezamos desde el primer día del mes
            DateOnly desde = (anio == hoy.Year && mes == hoy.Month) ? hoy : inicioDeMes;

            int diasEsperados = CantidadDeDiasRestantesEnElMes(desde, finDeMes, diaSemana);

            if (diasEsperados == 0) return new List<ClaseAdelantada>();

            var turnosFisicos = await repoTurno.ObtenerTurnosDisponiblesRangoAsync(
                idActividad, diaSemana, idUsuario, desde, finDeMes);

            var clasesAgrupadas = turnosFisicos
                .GroupBy(t => new { t.Id_Profesor, t.HoraInicio, t.Id_Cancha })
                .Where(g => g.Count() == diasEsperados)
                .Select(g => new ClaseAdelantada
                {
                    TurnoRepresentativo = g.First(),
                    TurnosDelMes = g.ToList(),
                    CupoMinimoDisponible = g.Min(t => t.CupoDisponible)
                })
                .ToList();

            return clasesAgrupadas;
        }
        private int CantidadDeDiasRestantesEnElMes(DateOnly desde, DateOnly hasta, DayOfWeek diaBuscado)
        {
            int conteo = 0;

            for (DateOnly fecha = desde; fecha <= hasta; fecha = fecha.AddDays(1))
            {
                if (fecha.DayOfWeek == diaBuscado)
                {
                    conteo++;
                }
            }
            return conteo;
        }
    }
}