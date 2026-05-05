using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;

namespace CentroDeportivo.Aplicacion.Validadores
{
    public class ProfesorValidador(IProfesorRepositorio repo)
    {
        public async Task<(bool esValido, string mensaje)> Validar(Profesor p) {
            string mensaje = "";

            // 1. Agrupamos validación de campos vacíos
            if (string.IsNullOrWhiteSpace(p.Nombre) ||
                string.IsNullOrWhiteSpace(p.Apellido) ||
                string.IsNullOrWhiteSpace(p.Dni))
            {
                mensaje += "Error: Debe completar todos los campos obligatorios.\n";
            }

            if (!string.IsNullOrWhiteSpace(p.Dni)){

                if (await repo.YaExiste(p.Dni))
                {
                    mensaje += "Error: ya existe un profesor con ese DNI. \n";
                }

            }
            return (string.IsNullOrEmpty(mensaje),mensaje);
        }
    }
}
