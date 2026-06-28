using CentroDeportivo.Aplicacion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Casos_de_uso.PagoUseCase
{
    public class DescargarComprobanteUseCase
    {
        private readonly IPagoRepositorio pagoRepositorio;
        private readonly IPDFServicio pdfService; 

        public DescargarComprobanteUseCase(IPagoRepositorio pagoRepositorio, IPDFServicio pdfService)
        {
            this.pagoRepositorio = pagoRepositorio;
            this.pdfService = pdfService;
        }

        public async Task<byte[]> Ejecutar(int pagoId, int usuarioId)
        {
            var pago = await pagoRepositorio.ObtenerPorIdAsync(pagoId);

            if (pago == null || pago.Id_Usuario != usuarioId)
                throw new Exception("Comprobante no encontrado o no autorizado");

            return pdfService.GenerarComprobante(pago);
        }
    }
}
