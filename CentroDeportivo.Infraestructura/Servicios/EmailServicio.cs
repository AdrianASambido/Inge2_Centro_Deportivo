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
            <p>Si habías realizado la reserva con un credito o por adelantado, se genero un credito a tu favor. De lo contrario se te devolvera el dinero</p>"
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
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Centro Deportivo", _username));
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

        public async Task EnviarAvisoVacanteListaEsperaAsync(string email, Turno turno)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Centro Deportivo", _username));
            mensaje.To.Add(new MailboxAddress("", email));

            mensaje.Subject = $"¡Lugar disponible para {turno.Actividad?.Nombre ?? "tu clase"}!";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px;'>
                        <h2 style='color: #2c3e50; border-bottom: 2px solid #3498db; padding-bottom: 10px;'>¡Se liberó una vacante!</h2>
                        <p>Hola,</p>
                        <p>Te notificamos que se canceló una reserva en la clase que estabas esperando. Al ser el primero en la fila, el lugar es tuyo:</p>
                        
                        <div style='background-color: #f8f9fa; padding: 15px; border-left: 4px solid #2ecc71; margin: 20px 0; border-radius: 4px;'>
                            <p style='margin: 5px 0;'><b>Actividad:</b> {turno.Actividad?.Nombre ?? "Clase del Centro"}</p>
                            <p style='margin: 5px 0;'><b>Fecha:</b> {turno.Fecha.ToShortDateString()}</p>
                            <p style='margin: 5px 0;'><b>Horario:</b> {turno.HoraInicio} hs</p>
                        </div>

                        <p style='color: #e74c3c;'><b>⚠️ Importante:</b> Ingresá a la aplicación a la sección de tus listas de espera para confirmar tu asistencia. ¡Apurate antes de que expire tu tiempo de prioridad!</p>
                        
                        <hr style='border: 0; border-top: 1px solid #eee; margin: 20px 0;' />
                        <p style='font-size: 12px; color: #7f8c8d; text-align: center;'>Este es un mensaje automático del Sistema de Gestión del Centro Deportivo.</p>
                    </div>"
            };
            mensaje.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_username, _password);
                await client.SendAsync(mensaje);
                await client.DisconnectAsync(true);

                Console.WriteLine($"Aviso de lista de espera enviado exitosamente a {email}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar aviso de lista de espera a {email}: {ex.Message}");
                throw;
            }
        }

        public Task EnviarRecordatorioTurnoAsync(string email, Turno turno)
        {
            throw new NotImplementedException();
        }
    }
}