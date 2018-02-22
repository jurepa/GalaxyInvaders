using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Models
{
    public class Defensa
    {
        public int posicionX { get; set; }
        public int posicionY { get; set; }
        public Boolean puedeImpactar { get; set; }
        public Uri imagen { get; set; }
        public int impactos { get; set; }

        public Defensa(int posicionX, int posicionY)
        {
            this.posicionX = posicionX;
            this.posicionY = posicionY;
            this.imagen = new Uri("ms-appx:///Assets/Images/Defend.png");
            this.puedeImpactar = true;
            this.impactos = 0; //Si impactos es 2. Ya se quitaría.
        }
        public Defensa()
        {
            this.posicionX = 0;
            this.posicionY = 0;
            this.imagen = new Uri("ms-appx:///Assets/Images/Defend.png");
            this.puedeImpactar = true;
            this.impactos = 0; //Si impactos es 2. Ya se quitaría.
        }
    }
}
