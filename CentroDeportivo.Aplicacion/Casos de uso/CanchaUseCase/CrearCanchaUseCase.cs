using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.CanchaUseCase
{
    public class CrearCanchaUseCase(ICanchaRepositorio repo, CanchaValidador validador)
    {
        public async Task ejecutar(Cancha c)
        {
            var (esValido, mensaje) = await validador.validar(c);

            if (!esValido)
            {
                throw new Exception(mensaje);
            }

            c.Existe = true;
            await repo.AgregarAsync(c);
        }
    }
}