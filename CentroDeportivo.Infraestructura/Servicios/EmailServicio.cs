using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.Aplicacion.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Servicios
{
    public class EmailServicio : IEmailServicio
    {

        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        
        public EmailServicio(IConfiguration configuration)
        {
            _host = configuration["EmailSettings:Host"] ?? "smtp.gmail.com";
            _port = int.Parse(configuration["EmailSettings:Port"] ?? "587");
            _username = configuration["EmailSettings:Username"] ?? "";
            _password = configuration["EmailSettings:Password"] ?? "";
        }

        public async Task EnviarAvisoCancelacionMasivo(IEnumerable<string> emailsDestinatarios, Turno turno)
        {
            if (emailsDestinatarios == null || !emailsDestinatarios.Any()) return;

            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Centro Deportivo", _username));

            mensaje.To.Add(new MailboxAddress("Socios Centro Deportivo", _username));

            foreach (var email in emailsDestinatarios)
            {
                mensaje.Bcc.Add(new MailboxAddress("", email));
            }

            mensaje.Subject = $"AVISO: Cancelación de clase - {turno.Actividad?.Nombre}";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
            <h3>Clase Cancelada</h3>
            <p>Lamentamos informarle que la clase de <b>{turno.Actividad?.Nombre}</b> del día <b>{turno.Fecha}</b> a las <b>{turno.HoraInicio} hs</b> ha sido cancelada por el establecimiento.</p>
            <p>Si habías realizado la reserva por adelantado, se genero un credito a tu favor. De lo contrario se te devolvera el dinero</p>"
            };
            mensaje.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_username, _password);
                await client.SendAsync(mensaje); 
                await client.DisconnectAsync(true);
                Console.WriteLine("Correos masivos enviados exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en envío masivo: {ex.Message}");
                throw;
            }
        }

        public async Task EnviarContraseniaTemporalAsync(string emailDestino, string contraseniaTemporal)
        {
            //  Crea mensaje con MimeKit
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Centro Deportivo",_username));
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


            using var client = new SmtpClient();
            try
            {

                await client.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);


                await client.AuthenticateAsync(_username, _password);


                await client.SendAsync(mensaje);

                await client.DisconnectAsync(true);
                Console.WriteLine("Correo enviado exitosamente con Gmail.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error con Gmail: {ex.Message}");
                throw;
            }
        }


        public async Task EnviarLinkRecuperacionAsync(string emailDestino, string link)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Centro Deportivo", _username));
            mensaje.To.Add(new MailboxAddress("", emailDestino));
            mensaje.Subject = "Recuperación de contraseña";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
            <h3>Recuperación de contraseña</h3>
            <p>Recibimos una solicitud para restablecer tu contraseña.</p>
            <p>Hacé click en el siguiente link para continuar:</p>
            <a href='{link}'>Restablecer contraseña</a>
            <p>Este link vence en 10 minutos.</p>
            <p>Si no solicitaste este cambio, ignorá este mensaje.</p>"
            };
            mensaje.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_username, _password);
                await client.SendAsync(mensaje);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enviando email de recuperación: {ex.Message}");
                throw;
            }
        }

        public Task EnviarRecordatorioTurnoAsync(string email, Turno turno)
        {
            Console.WriteLine($"[Email simulado] Recordatorio turno para {email}");
            return Task.CompletedTask;
        }
    }
}