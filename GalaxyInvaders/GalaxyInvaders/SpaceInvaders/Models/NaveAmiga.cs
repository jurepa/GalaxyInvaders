using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Models
{
    public class NaveAmiga
    {
        public Uri imagen { get; set; }
        public Double visible { get; set; }
        public int vidas { get; set; }
        public Double vida1 { get; set; }
        public Double vida2 { get; set; }
        public Double vida3 { get; set; }
        public int velocidad { get; set; }
        public int posicionX{ get;set; }
        public int puntuacion { get; set; }

        public NaveAmiga( Uri imagen, Double visible, Double vida1, Double vida2, Double vida3 , int velocidad, int posicionX)
        {
            this.puntuacion = 0;
            this.vidas = 3;
            this.vida1 = vida1;
            this.vida2= vida2;
            this.vida3 = vida3;
            this.imagen = imagen;
            this.visible = visible;
            this.velocidad = velocidad;
            this.posicionX = posicionX;
        }

    }
}
