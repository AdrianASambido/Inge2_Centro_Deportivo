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
            return PDF.Create(container =>
            {
                container.Page(page =>
                {
                    page.Header().Text("Comprobante de Pago");
                    page.Content().Column(c =>
                    {
                        c.Item().Text($"ID Transacción: {pago.Id}");
                        c.Item().Text($"Monto: ${pago.Monto}");
                        c.Item().Text($"Fecha: {pago.Fecha}");
                    });
                });
            }).GeneratePdf();
        }
    }
}