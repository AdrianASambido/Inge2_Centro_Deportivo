using CentroDeportivo.Aplicacion.Entidades; 
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroDeportivo.Aplicacion.Interfaces;
using QuestPDF.Fluent;
using PDF = QuestPDF.Fluent.Document;

namespace CentroDeportivo.Infraestructura.Servicios
{
    public class PdfServicio : IPDFServicio
    {
        public byte[] GenerarComprobante(Pago pago)
        {
            var actividad = pago.Reserva?.Turno?.Actividad?.Nombre
                            ?? pago.Turno?.Actividad?.Nombre
                            ?? "Sin actividad";

            return PDF.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Comprobante de Pago").FontSize(20).Bold();
                    page.Content().Column(c =>
                    {
                        c.Item().Text($"ID: {pago.Id}");
                        c.Item().Text($"Actividad: {actividad}");
                        c.Item().Text($"Monto: ${pago.Monto}");
                        c.Item().Text($"Fecha: {pago.Fecha:dd/MM/yyyy HH:mm}");
                        if (!string.IsNullOrEmpty(pago.MercadoPagoTransactionId))
                            c.Item().Text($"Transacción MP: {pago.MercadoPagoTransactionId}");
                    });
                });
            }).GeneratePdf();
        }
    }
}