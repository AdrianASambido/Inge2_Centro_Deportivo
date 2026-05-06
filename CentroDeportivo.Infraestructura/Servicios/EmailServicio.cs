using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace CentroDeportivo.Infraestructura.Servicios
{
    public class EmailServicio : IEmailServicio
    {
        // En un entorno profesional, estos datos vendrían del appsettings.json
        private readonly string _emailEmisor = "tu-centro-deportivo@gmail.com";
        private readonly string _passwordApp = "tu-contraseña-de-aplicacion"; // No es tu pass real, es un token
        public async Task EnviarContraseniaTemporalAsync(string email, string contraseniaTemporal)
        {
            var mensaje = new MailMessage();
            mensaje.From = new MailAddress(_emailEmisor, "Centro Deportivo - Administración");
            mensaje.To.Add(new MailAddress(email));
            mensaje.Subject = "Bienvenido al Equipo - Contraseña Temporal";

            mensaje.Body = $@"
            <h1>Bienvenido al Centro Deportivo</h1>
            <p>Se ha creado tu cuenta de empleado con éxito.</p>
            <p>Tu contraseña temporal es: <strong>{contraseniaTemporal}</strong></p>
            <br>
            <p>Por motivos de seguridad, deberás cambiarla al ingresar por primera vez.</p>";

            mensaje.IsBodyHtml = true;

            using var clienteSmtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_emailEmisor, _passwordApp),
                EnableSsl = true,
            };

            try
            {
                await clienteSmtp.SendMailAsync(mensaje);
            }
            catch (Exception ex)
            {
                // En Ingeniería II, es vital loguear el error pero no frenar el alta del usuario
                // Podrías lanzar una excepción personalizada o simplemente loguear
                Console.WriteLine($"Error enviando email: {ex.Message}");
            }
        }

        public Task EnviarLinkRecuperacionAsync(string email, string link)
        {
            throw new NotImplementedException();
        }

        public Task EnviarRecordatorioTurnoAsync(string email, Turno turno)
        {
            throw new NotImplementedException();
        }
    }
}
