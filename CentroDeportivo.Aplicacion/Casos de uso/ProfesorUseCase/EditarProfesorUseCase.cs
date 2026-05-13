using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.ProfesorUseCase
{
    public class EditarProfesorUseCase
    {
        private readonly IProfesorRepositorio repo;
        private readonly ProfesorValidador validador;

        public EditarProfesorUseCase(IProfesorRepositorio repo, ProfesorValidador validador)
        {
            this.repo = repo;
            this.validador = validador;
        }

        public async Task Ejecutar(Profesor profesorEditado, int idProfesor)
        {
            var existente = await repo.ObtenerPorIdAsync(idProfesor);

            if (existente == null)
                throw new Exception("El profesor no existe");

            // 🔴 VALIDACIÓN DE DNI DUPLICADO (excepto el mismo profesor)
            if (await repo.YaExisteDniParaEditar(profesorEditado.Dni, idProfesor))
                throw new Exception("El DNI ingresado ya pertenece a otro profesor registrado");

            var (esValido, mensaje) = await validador.ValidarEdicion(profesorEditado);

            if (!esValido)
                throw new Exception(mensaje);

            // actualizar datos
            existente.Nombre = profesorEditado.Nombre;
            existente.Apellido = profesorEditado.Apellido;
            existente.Dni = profesorEditado.Dni;

            await repo.ActualizarAsync(existente);
        }
    }
}