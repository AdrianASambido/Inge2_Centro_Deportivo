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
        public async Task<List<ClaseAdelantada>> Ejecutar(int idActividad, DayOfWeek diaSemana, int idUsuario)
        {
            DateTime ahora = DateTime.Now;
            DateOnly hoy = DateOnly.FromDateTime(ahora);
            DateOnly finDeMes = new DateOnly(hoy.Year, hoy.Month, DateTime.DaysInMonth(hoy.Year, hoy.Month));


            int diasRestantesEsperados = CantidadDeDiasRestantesEnElMes(hoy, finDeMes, diaSemana);


            if (diasRestantesEsperados == 0) return new List<ClaseAdelantada>();

            var turnosFisicos = await repoTurno.ObtenerTurnosDisponiblesRangoAsync(idActividad, diaSemana, idUsuario, hoy, finDeMes);


            var clasesAgrupadas = turnosFisicos
                .GroupBy(t => new { t.Id_Profesor, t.HoraInicio, t.Id_Cancha })
                .Where(g => g.Count() == diasRestantesEsperados)
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