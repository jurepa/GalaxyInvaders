using SpaceInvaders.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using SpaceInvaders.Models;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace SpaceInvaders
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Game : Page
    {
        public VMGame vMGame { get; }
        public Image imagenEnemiga;
        //private bool haPulsado;
        //private bool haLevantado;
        //private DispatcherTimer timer = new DispatcherTimer();
        //private bool estaDisparando;

        public Game()
        {
            //haPulsado = false;
            //haLevantado = false;
            this.InitializeComponent();
            //Window.Current.Content.KeyDown += KeyDownEvent;

            vMGame =(VMGame) this.DataContext;
            imagenEnemiga = new Image();
            NaveEnemiga naveEnemiga = new NaveEnemiga();
            naveEnemiga.imagen = new Uri("ms-appx:///Assets/Images/Alien3Pro.png");
            naveEnemiga.dirX = 639;
            naveEnemiga.dirY = 20;
            imagenEnemiga.Source = new BitmapImage(naveEnemiga.imagen);
            imagenEnemiga.Height = 50;
            imagenEnemiga.Width = 50;
            this.canvas.Children.Add(imagenEnemiga);
            Canvas.SetTop(imagenEnemiga, naveEnemiga.dirY);
            Canvas.SetLeft(imagenEnemiga,naveEnemiga.dirX);

            //vMGame.canvas = this.canvas;
            //timer.Interval = new TimeSpan(0,0,0,0,100);
            //timer.Tick += Timer_Tick;
            //timer.Start();
        }

        //private void Timer_Tick(object sender, object e)
        //{
        //    if (estaDisparando)
        //    {
        //        Disparo disparo = new Disparo(vMGame.posYMisil, vMGame.posX, 20, 10, new Uri("ms-appx:///Assets/Images/MisilPro.png"));
        //        Image playerBullet = new Image();
        //        playerBullet.Source = new BitmapImage(disparo.imagen);
        //        Canvas.SetTop(playerBullet, Canvas.GetTop(this.player));
        //        Canvas.SetLeft(playerBullet, Canvas.GetLeft(this.player));
        //        this.canvas.Children.Add(playerBullet);
        //        if (Canvas.GetTop(playerBullet) >= 0)
        //        {
        //            Canvas.SetTop(playerBullet, Canvas.GetTop(playerBullet) - disparo.velocidad);
        //        }
        //    }
        //}

        /*private void KeyDownEvent(object sender, KeyRoutedEventArgs e)
        {
        
        }*/

        /// <summary>
        /// Si este evento no hace caso al KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allowfocus_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.Content.KeyDown += this.vMGame.Grid_KeyDown;
            //Window.Current.Content.KeyDown += Disparo_KeyDown;
            Window.Current.Content.KeyUp += this.vMGame.Grid_KeyUp;
            Window.Current.Content.KeyUp += Disparo_KeyUp;
            //Window.Current.Content.KeyUp += Disparo_KeyUp;
        }



        private void Disparo_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Space)
            {
                //haLevantado = true;
                disparar();
            }
        }

        private void Disparo_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Space)
            {
                //haPulsado = true;
                disparar();
                //estaDisparando = true;
            }
        }
        private async void moveBullet(int velocidad, Image playerBullet)
        {
            while (this.canvas.Children.Contains(playerBullet))
            {
                Canvas.SetTop(playerBullet, Canvas.GetTop(playerBullet) - velocidad);
                if (this.canvas.Children.Contains(imagenEnemiga))
                {
                    if (Canvas.GetTop(imagenEnemiga) + 49 >= Canvas.GetTop(playerBullet) && Canvas.GetLeft(imagenEnemiga) <= Canvas.GetLeft(playerBullet) && Canvas.GetLeft(imagenEnemiga) + 50 > Canvas.GetLeft(playerBullet))
                    {
                        this.canvas.Children.Remove(imagenEnemiga);
                        this.canvas.Children.Remove(playerBullet);
                    }
                }
                if (Canvas.GetTop(playerBullet)<0)
                {
                    this.canvas.Children.Remove(playerBullet);
                }
                await Task.Delay(50);
            }
        }
        private void disparar()
        {
            Disparo disparo = new Disparo(vMGame.posYMisil, vMGame.player.posicionX, 20, 10, new Uri("ms-appx:///Assets/Images/MisilPro.png"));
            Image playerBullet = new Image();
            playerBullet.Source = new BitmapImage(disparo.imagen);
            playerBullet.Height = 20;
            playerBullet.Width = 10;
            Canvas.SetTop(playerBullet, Canvas.GetTop(this.player));
            Canvas.SetLeft(playerBullet, Canvas.GetLeft(this.player) + 35);
            this.canvas.Children.Add(playerBullet);
            moveBullet(disparo.velocidad, playerBullet);
        }

        private void comprobarImpacto()
        {

        }
    }
}
