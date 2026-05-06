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
        public UsuarioClienteValidador(IUsuarioRepositorio repo) : base(repo)
        {
        }

        protected override (bool esValidoPass, string mensajePass) ValidarPassword(Usuario u)
        {
            string mensaje = "";

            if (string.IsNullOrWhiteSpace(u.Password))
            {
                mensaje += "La contraseña es obligatoria. \n";
            }
            

            
            else if (!Regex.IsMatch(u.Password, @"^(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                mensaje += "La contraseña debe tener al menos 8 caracteres, una mayúscula y un número. \n";
            }

            return (string.IsNullOrEmpty(mensaje), mensaje);
        }
    }
}
