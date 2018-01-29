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
        private int _mIndexDificultadSeleccionada;
        private double _mVolumeMedia;
        private int _mVolumeSlider;
        private bool _splitAbierto;

        #endregion
        #region contructor
        public MainPageGameVM()
        {
            getLevels();
            _splitAbierto = false;
            _mVolumeMedia = 1;
            _mVolumeSlider = 100;
            NotifyPropertyChanged("mVolumeMedia");
            NotifyPropertyChanged("mVolumeSlider");
        }


        #endregion

        #region Propiedades Publicas

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
            MessageDialog instrucciones = new MessageDialog("A y D para moverse y Espacio para disparar. \n" +
                "Para hacks, preguntar al GodHacker de segundo");
            await instrucciones.ShowAsync();
        }
        public void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Frame navigationFrame = Window.Current.Content as Frame;

            navigationFrame.Navigate(typeof(Game));
            
        }
    }
}
