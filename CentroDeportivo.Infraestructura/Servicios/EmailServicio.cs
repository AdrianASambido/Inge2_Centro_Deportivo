using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace CentroDeportivo.Infraestructura.Servicios
{
    public class EmailServicio : IEmailServicio
    {
            // Datos sacados de tu captura de pantalla de Mailtrap
            private readonly string _host = "sandbox.smtp.mailtrap.io";
            private readonly int _port = 587;
            private readonly string _username = "cc40ec27feb855";
            private readonly string _password = "64dd5d2e21eb4a"; // El que ves al tocar el ojito en Mailtrap

   
            public async Task EnviarContraseniaTemporalAsync(string emailDestino, string contraseniaTemporal)
            {
            // 1. Creamos el mensaje con MimeKit
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Centro Deportivo", "admin@centrodeportivo.com"));
            mensaje.To.Add(new MailboxAddress("", emailDestino));
            mensaje.Subject = "Bienvenido - Tu contraseña temporal";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <h3>¡Bienvenido al sistema!</h3>
                    <p>Se ha creado tu cuenta de empleado correctamente.</p>
                    <p>Tu contraseña temporal es: <b>{contraseniaTemporal}</b></p>
                    <p>Recordá cambiarla al ingresar por primera vez.</p>"
            };
            mensaje.Body = bodyBuilder.ToMessageBody();

            // 2. Enviamos con el cliente de MailKit
            using var client = new SmtpClient();
            try
            {
                // Conexión con StartTls (el punto débil del SmtpClient de .NET)
                await client.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);

                // Autenticación
                await client.AuthenticateAsync(_username, _password);

                // Envío
                await client.SendAsync(mensaje);

                await client.DisconnectAsync(true);
                Console.WriteLine("Correo enviado exitosamente con MailKit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error con MailKit: {ex.Message}");
                throw;
            }
        }
       

        public Task EnviarLinkRecuperacionAsync(string email, string link)
        {
            throw new NotImplementedException();
        }

        public Task EnviarRecordatorioTurnoAsync(string email, Turno turno)
        {
            Console.WriteLine($"[Email simulado] Recordatorio turno para {email}");
            return Task.CompletedTask;
        }
    }
}
