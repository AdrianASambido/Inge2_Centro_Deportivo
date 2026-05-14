using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Entidades;
using System.Text.RegularExpressions;


namespace CentroDeportivo.Aplicacion.Validadores
{
    public class UsuarioClienteValidador : UsuarioValidadorBase
    {
        public UsuarioClienteValidador(IUsuarioRepositorio repo, IProfesorRepositorio repoProfe) : base(repo, repoProfe)
        {
        }

        protected override (bool esValidoPass, string mensajePass) ValidarPassword(Usuario u)
        {
            return UsuarioValidadorBase.ValidarFormatoPassword(u.Password);
        }
    }
}
