using CentroDeportivo.Aplicacion.Entidades;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
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
                            ?? "Sin Actividad Específica";

            return PDF.Create(container =>
            {
                container.Page(page =>
                {
                    // Configuración de página y márgenes estructurados
                    page.Size(PageSizes.A5.Landscape()); // Formato ticket digital apaisado
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontFamily("Helvetica").FontSize(10).FontColor("#333333"));

                    // --- ENCABEZADO ---
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("CENTRO DEPORTIVO").FontSize(16).Bold().FontColor("#0d6efd"); // Azul UI
                            col.Item().Text("Comprobante oficial de pago").FontSize(9).Italic().FontColor("#6c757d");
                        });

                        row.ConstantItem(100).AlignRight().Column(col =>
                        {
                            col.Item().Text($"N° #{pago.Id:D6}").FontSize(12).Bold().FontColor("#212529");
                            col.Item().Text($"{pago.Fecha:dd/MM/yyyy}").FontSize(9).FontColor("#6c757d");
                        });
                    });

                    // --- CONTENIDO PRINCIPAL ---
                    page.Content().PaddingTop(15).Column(c =>
                    {
                        // Línea divisoria superior
                        c.Item().BorderBottom(1).BorderColor("#e9ecef").PaddingBottom(10);

                        // Fila de datos del cliente / Operación
                        c.Item().PaddingVertical(10).Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Detalle del Servicio:").Bold().FontColor("#495057").FontSize(10);
                                col.Item().PaddingTop(2).Text($"{actividad}").FontSize(12).Bold();
                                col.Item().Text($"Fecha de registro: {pago.Fecha:HH:mm} hs").FontSize(9).FontColor("#6c757d");
                            });

                            if (!string.IsNullOrEmpty(pago.MercadoPagoTransactionId))
                            {
                                row.RelativeItem().AlignRight().Column(col =>
                                {
                                    col.Item().Text("Método de Pago:").Bold().FontColor("#495057").FontSize(10);
                                    col.Item().PaddingTop(2).Text("Mercado Pago").FontSize(11);
                                    col.Item().Text($"Transacción: {pago.MercadoPagoTransactionId}").FontSize(9).FontColor("#6c757d");
                                });
                            }
                        });

                        // Espacio intermedio controlado por padding
                        c.Item().PaddingBottom(15);

                        // --- CUADRO DE MONTO TOTAL DESTACADO ---
                        c.Item().Background("#f8f9fa").Border(1).BorderColor("#e9ecef").Padding(12).Row(row =>
                        {
                            row.RelativeItem().AlignLeft().AlignMiddle().Text("TOTAL ABONADO").Bold().FontSize(11).FontColor("#495057");

                            row.RelativeItem().AlignRight().AlignMiddle().Text($"$ {pago.Monto:N2}")
                                .FontSize(16)
                                .Bold()
                                .FontColor("#198754"); // Verde éxito de Bootstrap
                        });
                    });

                    // --- PIE DE PÁGINA ---
                    page.Footer().AlignBottom().Column(col =>
                    {
                        col.Item().BorderTop(1).BorderColor("#e9ecef").PaddingTop(8);
                        col.Item().AlignCenter().Text("Gracias por entrenar con nosotros.").FontSize(9).FontColor("#6c757d");
                        col.Item().AlignCenter().Text("Este documento sirve como constancia válida de pago digital.").FontSize(8).Italic().FontColor("#adb5bd");
                    });
                });
            }).GeneratePdf();
        }
    }
}