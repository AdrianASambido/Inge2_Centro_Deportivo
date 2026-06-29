using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Entidades
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Domicilio { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool TieneSancionDescuento { get; set; } = false;
        public string? TokenRecuperacion {  get; set; } = string.Empty;
        public DateTime? TokenRecuperacionVencimiento {  get; set; } 
        public bool DebeCambiarPassword { get; set; } = false;
        public Rol Rol { get; set; }

        public Usuario(string nombre, string apellido, string domicilio, string dni, string password, string email, bool debeCambiarPassword, Rol rol)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Domicilio = domicilio;
            this.Dni = dni;
            this.Password = password;
            this.Email = email;
            this.DebeCambiarPassword = debeCambiarPassword;
            this.Rol = rol;
        }

        public Usuario() { }
    }
}