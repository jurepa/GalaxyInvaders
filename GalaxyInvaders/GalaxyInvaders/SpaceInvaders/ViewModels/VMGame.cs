using Capa_BL.Gestoras;
using Entities;
using SpaceInvaders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SpaceInvaders.ViewModels
{
    public class VMGame : clsVMBase
    {
        #region privados
        private NaveAmiga _player;
        private Double _posYMisil;
        private Jugador _jugador;
        private string _puntuacion;
        private double _opacidadPausa;
        private DispatcherTimer dispatcherTimer { get; set; }
        //private Canvas _canvas;
        #endregion
        #region publicos
        /*public Canvas canvas
        {
            get
            {
                return _canvas;
            }
            set
            {
                _canvas = value;
            }
        }*/
        public double opacidadPausa
        {
            get
            {
                return _opacidadPausa;
            }
            set
            {
                _opacidadPausa = value;
                NotifyPropertyChanged("opacidadPausa");
            }
        }
        public string puntuacion
        {
            get
            {
                return _puntuacion;
            }
            set
            {
                _puntuacion = value;
                NotifyPropertyChanged("puntuacion");
            }
        }
        public Jugador jugador
        {
            get
            {
                return _jugador;
            }
            set
            {
                _jugador = value;
                NotifyPropertyChanged("jugador");
            }
        }
        public Double posYMisil
        {
            get
            {
                return _posYMisil;
            }
            set
            {
                _posYMisil = value;
                NotifyPropertyChanged("posYMisil");
            }
        }
        public NaveAmiga player
        {
            set
            {
                _player = value;
                NotifyPropertyChanged("player");
            }
            get
            {
                return _player;
            }
        }
        #endregion

        public VMGame()
        {
            _player = new NaveAmiga(new Uri("ms-appx:///Assets/Images/PlayerPro.png"),1, 1, 1, 1, 50,639);
            _posYMisil = 540;
            _opacidadPausa = 0;
            NotifyPropertyChanged("player");
            NotifyPropertyChanged("posYMisil");
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Tick += timerTick;
            NotifyPropertyChanged("opacidadPausa");
            //Defensas


        }
        private void timerTick(object sender, object e)
        {
            move();
        }

        public void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.A)
            {
                left();
                dispatcherTimer.Start();
            }

            if (e.Key == VirtualKey.D)
            {
                right();
                dispatcherTimer.Start();              
            }

        }
        public void Grid_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.A || e.Key == VirtualKey.D)
            {
                dispatcherTimer.Stop();
            }
        }
        public void move()
        {
            Double posicionFutura = _player.posicionX + _player.velocidad;
            if ( posicionFutura > 0 && posicionFutura<1275)
            {
                _player.posicionX += _player.velocidad;
            }
            NotifyPropertyChanged("player");
        }
        public void right()
        {
            //_velocidad = 10;
            if (_player.posicionX < 1275)
            {
                _player.velocidad = 10;
            }
            else
            {
                _player.velocidad = 0;
            }
        }
        public void left()
        {
            //_velocidad = -10;
            if (_player.posicionX > 0 && _player.posicionX - 10>0)
            {
                _player.velocidad = -10;
            }
            else
            {
                _player.velocidad = 0;
            }
        }

        public async void submitScore()
        {
            GestoraJugadoresBL gestoraBL = new GestoraJugadoresBL();
            await gestoraBL.insertJugador(this.jugador);
        }

        //Eventos Botones tactiles
        public void btnPointerPressed(object sender, PointerRoutedEventArgs e)//
        {
            Image image = (Image)sender;
            if (image.Name.Equals("btnIzq"))//Comprobar qué boton es el que llega
            {
                left();
                dispatcherTimer.Start();
            }

            if (image.Name.Equals("btnDcha"))
            {
                right();
                dispatcherTimer.Start();
            }
        }
        public void btnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            Image image = (Image)sender;
            if (image.Name.Equals("btnIzq") || image.Name.Equals("btnDcha"))//Comprobar qué boton es el que llega
            {
                dispatcherTimer.Stop();
            }
        }

    }
}
