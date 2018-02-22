using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Models
{
    public class JugadorConDificultad : Jugador
    {
        public String dificultad;
        public double volumen;
        public bool isMuted;

        public JugadorConDificultad(Jugador jugador, String dificultad,double volumen,bool isMuted)
        {
            this.ID = jugador.ID;
            this.Nombre = jugador.Nombre;
            this.Puntuacion = jugador.Puntuacion;
            this.FechaJuego = jugador.FechaJuego;
            this.dificultad = dificultad;
            this.volumen = volumen;
            this.isMuted = isMuted;
        }
    }
}
