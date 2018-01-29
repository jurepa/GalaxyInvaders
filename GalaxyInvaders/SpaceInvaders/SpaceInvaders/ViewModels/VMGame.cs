﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SpaceInvaders.ViewModels
{
    public class VMGame : clsVMBase
    {
        #region privados
        private Double _posX;
        private int _velocidad;
        private Double _posYMisil;
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
        public Double posX
        {
            get
            {
                return _posX;
            }
            set
            {
                _posX = value;
                NotifyPropertyChanged("posX");
            }
        }
        #endregion

        public VMGame()
        {
            _posYMisil = 540;
            _posX = 450;
            _velocidad = 50;
            NotifyPropertyChanged("posX");
            NotifyPropertyChanged("posYMisil");

        }


        public void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.A)
            {
                if (_posX >= 50)
                {
                    _posX = _posX - _velocidad;
                    NotifyPropertyChanged("posX");
                }

            }

            if (e.Key == VirtualKey.D)
            {
                if (posX <= 850)
                {
                    _posX = _posX + _velocidad;
                    NotifyPropertyChanged("posX");
                }
            }

        }

    }
}