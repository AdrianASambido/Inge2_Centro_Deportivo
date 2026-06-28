using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroDeportivo.Aplicacion.Entidades
{
    public class Cancha
    {
        public int Id { get; private set; }
        public int Numero { get; set; }
        public int Capacidad { get; set; }
        public bool Existe { get; set; } = true;

        public Cancha(int numero, int capacidad)
        {
            Numero = numero;
            Capacidad = capacidad;
           
        }

        public Cancha() { }
    }
}
