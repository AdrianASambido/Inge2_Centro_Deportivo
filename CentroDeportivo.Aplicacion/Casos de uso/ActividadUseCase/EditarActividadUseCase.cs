using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ActividadUseCase
{
    public class EditarActividadUseCase
    {
        private readonly IActividadRepositorio _repo;
        private readonly ActividadValidador _validador;

        public EditarActividadUseCase(IActividadRepositorio repo)
        {
            _repo = repo;
            _validador = new ActividadValidador(_repo);
        }
        public async Task Ejecutar(Actividad actividad, int idActividad)
        {
            var existente = await _repo.ObtenerPorIdAsync(idActividad);
            if (existente == null)
                throw new Exception("Actividad no encontrada");

            var (esValido, mensaje) = await _validador.ValidarEdicion(actividad, idActividad);
            if (!esValido)
                throw new Exception(mensaje);

            existente.Nombre = actividad.Nombre;
            existente.Descripcion = actividad.Descripcion;
            existente.Precio = actividad.Precio;

            await _repo.ActualizarAsync(existente);
        }
    }
}