using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.CanchaUseCase
{
    public class EditarCanchaUseCase
    {
        private readonly ICanchaRepositorio repo;

        public EditarCanchaUseCase(ICanchaRepositorio repo)
        {
            this.repo = repo;
        }

        public async Task Ejecutar(Cancha canchaEditada, int idCancha)
        {
            var canchaExistente = await repo.ObtenerPorIdAsync(idCancha);

            if (canchaExistente == null)
            {
                throw new Exception("La cancha no existe.");
            }

            // Regla de negocio:
            // no se puede editar si tiene turnos asignados
            if (await repo.TieneTurnosAsignadosAsync(canchaExistente.Id))
            {
                throw new Exception(
                    "Edición fallida. Esta cancha tiene turnos agendados"
                );
            }

            // Validar número repetido
            if (await repo.YaExisteNumeroParaEditar(
                    canchaEditada.Numero,
                    idCancha))
            {
                throw new Exception(
                    "Edición fallida. El número de cancha ingresado ya existe"
                );
            }

            // Validaciones básicas
            if (canchaEditada.Numero <= 0)
            {
                throw new Exception(
                    "Ingrese un número de cancha válido."
                );
            }

            if (canchaEditada.Capacidad < 1)
            {
                throw new Exception(
                    "Ingrese una capacidad válida."
                );
            }

            canchaExistente.Numero = canchaEditada.Numero;
            canchaExistente.Capacidad = canchaEditada.Capacidad;

            await repo.ActualizarAsync(canchaExistente);
        }
    }
}