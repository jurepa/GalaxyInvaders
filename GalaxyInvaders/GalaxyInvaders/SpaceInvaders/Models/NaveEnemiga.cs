using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Models
{
    public class NaveEnemiga
    {
        /*DireccionX: Entero; DireccionY:Entero; Velocidad: Entero 
		      PuedeDisparar:Boolean; Valor:Entero   */
        public Uri imagen { get; set; }
        public Boolean estado { get; set; }
        public int dirX { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public Double dirY { get; set; }
        public int velocidad { get; set; }
        public Boolean puedeDisparar { get; set; }
        public int valor { get; set; }
    }
}
