using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Validadores
{
    public class UsuarioEmpleadoValidador : UsuarioValidadorBase
    {
        public UsuarioEmpleadoValidador(IUsuarioRepositorio repo) : base(repo)
        {
        }

        protected override (bool esValidoPass,string mensajePass) ValidarPassword(Usuario u)
        {
            return (true, "");
        }
    }
}
