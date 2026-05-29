using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using CentroDeportivo.Aplicacion.Validadores;
using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Validadores
{
    public class UsuarioClienteValidador : UsuarioValidadorBase
    {
        public UsuarioClienteValidador(IUsuarioRepositorio repo, IProfesorRepositorio repoProfe) : base(repo, repoProfe)
        {
        }

        // En edición no validamos password
        protected override (bool esValidoPass, string mensajePass) ValidarPassword(Usuario u)
        {
            return (true, "");
        }


    }
}