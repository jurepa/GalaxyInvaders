using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Jugador
    {
        #region Propiedes
        private int _id;
        private string _nombre;
        private int _puntuacion;
        private DateTime _fechaJuego;

        #endregion Propiedades



        #region Contructores
        public Jugador()
        {
            _id = -1;
            _nombre = "";
            _puntuacion = 0;
            _fechaJuego = new DateTime();
        }

        public Jugador(string nombre, int puntuacion, DateTime fechaJuego)
        {
            _nombre = nombre;
            _puntuacion = puntuacion;
            _fechaJuego = fechaJuego;
        }

        #endregion Contructores

        #region Getters and Setters

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string Nombre
        {
            get
            {
                return _nombre;
            }
            set
            {
                _nombre = value;               
            }
        }

        public int Puntuacion
        {
            get
            {
                return _puntuacion;
            }
            set
            {
                _puntuacion = value;
            }
        }

        public DateTime FechaJuego
        {
            get
            {
                return _fechaJuego;
            }
            set
            {
                _fechaJuego = value;
            }
        }

        #endregion Getters and Setters

    }
}
