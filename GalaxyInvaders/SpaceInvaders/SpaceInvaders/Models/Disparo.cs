using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Models
{
    public class Disparo
    {
        public Double dirY{ get; set; }
        public Double dirX { get; set; }
        public int velocidad { get; set; }
        public Uri imagen { get; set; }
        public int valor { get; set; }

        public Disparo(Double dirY,Double dirX, int velocidad, int valor, Uri imagen)
        {
            this.dirY = dirY;
            this.dirX = dirX;
            this.velocidad = velocidad;
            this.valor = valor;
            this.imagen = imagen;
        }
    }
}
