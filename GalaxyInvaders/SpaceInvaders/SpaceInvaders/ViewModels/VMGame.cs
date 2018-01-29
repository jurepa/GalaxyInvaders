using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Input;

namespace SpaceInvaders.ViewModels
{
    public class VMGame : clsVMBase
    {
        #region privados
        private Double _posX;
        private int _velocidad;
        #endregion
        #region publicos
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
            _posX = 450;
            _velocidad = 50;
            NotifyPropertyChanged("posX");

        }


        public void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.A)
            {
                if (_posX > 40)
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
