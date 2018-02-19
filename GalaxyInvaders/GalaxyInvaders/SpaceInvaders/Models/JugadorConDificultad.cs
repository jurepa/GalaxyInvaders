using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Models
{
    public class JugadorConDificultad:Jugador
    {
        public String dificultad;

        public JugadorConDificultad(Jugador jugador,String dificultad)
        {
            this.ID = jugador.ID;
            this.Nombre = jugador.Nombre;
            this.Puntuacion = jugador.Puntuacion;
            this.FechaJuego = jugador.FechaJuego;
            this.dificultad = dificultad;
        }
    }
}
