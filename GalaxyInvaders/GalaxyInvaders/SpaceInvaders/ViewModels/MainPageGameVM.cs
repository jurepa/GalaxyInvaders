﻿using Entities;
using SpaceInvaders.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SpaceInvaders.ViewModels
{
    public class MainPageGameVM:clsVMBase
    {
        #region Atributos
        private ObservableCollection<String> _mDificultades;
        private DelegateCommand _cerrarAbrirSplit;
        private DelegateCommand _backCerrarSplit;
        private DelegateCommand _mostrarInstrucciones;
        private String _mDificultadSeleccionada;
        private int _mIndexDificultadSeleccionada;
        private string _username1;
        private string _username2;
        private string _username3;
        private double _mVolumeMedia;
        private int _mVolumeSlider;
        private bool _splitAbierto;
        private Jugador jugador;

        #endregion
        #region contructor
        public MainPageGameVM()
        {
            _username1 = "";
            _username2 = "";
            _username3 = "";
            getLevels();
            _splitAbierto = false;
            _mVolumeMedia = 1;
            _mVolumeSlider = 100;
            NotifyPropertyChanged("mVolumeMedia");
            NotifyPropertyChanged("mVolumeSlider");
            NotifyPropertyChanged("username1");
            NotifyPropertyChanged("username2");
            NotifyPropertyChanged("username3");
        }


        #endregion

        #region Propiedades Publicas
        public String mDificultadSeleccionada
        {
            get
            {
                return _mDificultadSeleccionada;
            }
            set
            {
                _mDificultadSeleccionada = value;
                NotifyPropertyChanged("mDificultadSeleccionada");
            }
        }
        public string userName1
        {

            set
            {
                _username1 = value;
                NotifyPropertyChanged("username1");
            }
            get
            {
                return _username1;
            }
        }
        public string userName2
        {

            set
            {
                _username2 = value;
                NotifyPropertyChanged("username1");
            }
            get
            {
                return _username2;
            }
        }
        public string userName3
        {

            set
            {
                _username3 = value;
                NotifyPropertyChanged("username1");
            }
            get
            {
                return _username3;
            }
        }
        public DelegateCommand mostrarInstrucciones
        {
            set
            {
                _mostrarInstrucciones = value;
                NotifyPropertyChanged("mostrarInstrucciones");
            }
            get
            {
                _mostrarInstrucciones = new DelegateCommand(ExecuteInstrucciones);
                return _mostrarInstrucciones;
            }
        }
        public DelegateCommand backCerrarSplit
        {
            set
            {
                _backCerrarSplit = value;
                NotifyPropertyChanged("mostrarInstrucciones");
            }
            get
            {
                _backCerrarSplit = new DelegateCommand(ExecuteCerrarSplit);
                return _backCerrarSplit;
            }
        }
        public bool splitAbierto
        {
            set
            {
                _splitAbierto = value;
                NotifyPropertyChanged("splitAbierto");
            }
            get
            {
                return _splitAbierto;
            }
        }
        public DelegateCommand cerrarAbrirSplit
        {
            set
            {
                _cerrarAbrirSplit = value;
                NotifyPropertyChanged("cerrarAbrirSplit");
            }
            get
            {
                _cerrarAbrirSplit = new DelegateCommand(ExecuteSplit);
                return _cerrarAbrirSplit;
            }
        }
        public ObservableCollection<String> mDificultades
        {
            get { return this._mDificultades; }
            set
            {
                this._mDificultades = value;
                NotifyPropertyChanged("mDificultades");
            }
        }

        public int mIndexDificultadSeleccionada
        {
            get { return this._mIndexDificultadSeleccionada; }
            set
            {
                this._mIndexDificultadSeleccionada = value;
                NotifyPropertyChanged("mIndexDificultadSeleccionada");
            }
        }

        public double mVolumeMedia
        {
            get { return this._mVolumeMedia; }
            set
            {
                this._mVolumeMedia = value;

                NotifyPropertyChanged("mVolumeMedia");
            }
        }
        public int mVolumeSlider
        {
            set
            {
                this._mVolumeSlider = value;
                _mVolumeMedia = Convert.ToDouble(_mVolumeSlider) / 100;
                NotifyPropertyChanged("mVolumeSlider");
                NotifyPropertyChanged("mVolumeMedia");
            }
            get
            {
                return this._mVolumeSlider;
            }
        }

        #endregion

        public void getLevels()
        {
            _mDificultades = new System.Collections.ObjectModel.ObservableCollection<String>();
            _mDificultades.Add("Fácil");
            _mDificultades.Add("Normal");
            _mDificultades.Add("Dificil");
            _mIndexDificultadSeleccionada = 1;
            jugador = new Jugador();
            NotifyPropertyChanged("mIndexDificultadSeleccionada");
            NotifyPropertyChanged("mDificultades");
        }
        public void ExecuteSplit()
        {
            _splitAbierto = !_splitAbierto;
            NotifyPropertyChanged("splitAbierto");
        }
        public void ExecuteCerrarSplit()
        {
            _splitAbierto = false;
            NotifyPropertyChanged("splitAbierto");
        }
        public void ExecuteInstrucciones()
        {
            muestraInstrucciones();
        }
        private async void muestraInstrucciones()
        {
            MessageDialog instrucciones = new MessageDialog("A y D para moverse, Espacio para disparar y ESC para pausa \n" +
                "Para hacks, preguntar al GodHacker de segundo");
            await instrucciones.ShowAsync();
        }
        public void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            JugadorConDificultad jugadorConDificultad;
            Frame navigationFrame = Window.Current.Content as Frame;
            if(userName1==""||userName2==""||userName3=="")
            {
                userName1 = "U";
                userName2 = "S";
                userName3 = "R";
            }
            jugador.Nombre = userName1 + userName2 + userName3;
            jugadorConDificultad = new JugadorConDificultad(jugador, mDificultadSeleccionada);
            navigationFrame.Navigate(typeof(Game),jugadorConDificultad);
            
        }
        public void btnscore(object sender, RoutedEventArgs e)
        {
            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(Scores));

            Window.Current.Content = rootFrame;
            Window.Current.Activate();

        }
    }
}
