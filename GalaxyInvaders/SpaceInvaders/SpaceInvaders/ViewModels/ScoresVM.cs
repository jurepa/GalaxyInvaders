using Capa_BL.Listados;
using Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SpaceInvaders.ViewModels
{
    public class ScoresVM : clsVMBase
    {

        private ObservableCollection<Jugador> _prueba;
        private bool _mHabiliteProgressring = true;

        #region contructor
        public ScoresVM()
        {
             cargarJugadoresAsync();
        }



        #endregion
        public ObservableCollection<Jugador> prueba
        {
            set
            {
                _prueba = value;
                NotifyPropertyChanged("prueba");
            }
            get
            {
                return _prueba;
            }
        }

        public bool mHabiliteProgressring
        {
            get { return _mHabiliteProgressring; }
            set
            {
                _mHabiliteProgressring = value;
            }
        }


        public async void cargarJugadoresAsync()
        {
            ListadoJugadoresBL manejadoraBL = new ListadoJugadoresBL();
            _prueba = new ObservableCollection<Jugador>(await manejadoraBL.getListadoJugadores());
           
            NotifyPropertyChanged("prueba");


            _mHabiliteProgressring = false;
            NotifyPropertyChanged("mHabiliteProgressring");
        }
        public void btnBackScore(object sender, RoutedEventArgs e)
        {
            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(MainPage));

            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

    }


}
