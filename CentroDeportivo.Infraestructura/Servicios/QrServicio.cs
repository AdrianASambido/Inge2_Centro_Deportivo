using CentroDeportivo.Aplicacion.Interfaces;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Infraestructura.Servicios
{
    public class QrServicio : IQrServicio
    {
        public byte[] GenerarImagenQr(string token)
        {
            // se crea el generador de QR
            using var qrGenerator = new QRCodeGenerator();

            // se crea la data del QR 
            using var qrCodeData = qrGenerator.CreateQrCode(token, QRCodeGenerator.ECCLevel.Q);

            // genera la representación gráfica 
            using var qrCode = new PngByteQRCode(qrCodeData);

            // obtiene los bytes en formato PNG
            // El parámetro '20' es el tamaño de los pixeles
            return qrCode.GetGraphic(20);
        }

        public string GenerarToken()
        {
            // Crea un código único como por ejem: "f47ac10b-58cc-4372-a567-0e02b2c3d479"
            return Guid.NewGuid().ToString();
        }
    }
}