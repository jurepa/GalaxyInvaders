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
        public string visible { get; set; }
        public int vidas { get; set; }
        public int velocidad { get; set; }
        public int posicionX{ get;set; }

        public NaveAmiga( Uri imagen, string visible, int velocidad, int posicionX)
        {
            this.vidas = 3;
            this.imagen = imagen;
            this.visible = visible;
            this.velocidad = velocidad;
            this.posicionX = posicionX;
        }

    }
}
