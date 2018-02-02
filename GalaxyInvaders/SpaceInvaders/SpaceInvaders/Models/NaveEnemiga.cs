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
        public Disparo disparo { get; set; }
        public Uri imagen { get; set; }
        public Boolean visible { get; set; }
        public Double dirX { get; set; }
        public Double dirY { get; set; }
        public int velocidad { get; set; }
        public Boolean puedeDisparar { get; set; }
        public int valor { get; set; }
    }
}
