using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.ViewModels
{
    class MainPageGameVM : clsVMBase
    {

        #region Atributos
        private ObservableCollection<String> _mDificultades;
        private DelegateCommand _cerrarAbrirSplit;
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
            _mVolumeSlider =(int)((_mVolumeMedia-(int)_mVolumeMedia)*100);
            NotifyPropertyChanged("mVolumeMedia");
            NotifyPropertyChanged("mVolumeSlider");
        }


        #endregion

        #region Propiedades Publicas
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
        public void ExecuteSplit()
        {
            _splitAbierto = !_splitAbierto;
            NotifyPropertyChanged("splitAbierto");
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
            _mDificultades = new ObservableCollection<String>();
            _mDificultades.Add("Fácil");
            _mDificultades.Add("Normal");
            _mDificultades.Add("Dificil");
            _mIndexDificultadSeleccionada = 1;

            NotifyPropertyChanged("mIndexDificultadSeleccionada");
            NotifyPropertyChanged("mDificultades");
        }
    }
}

