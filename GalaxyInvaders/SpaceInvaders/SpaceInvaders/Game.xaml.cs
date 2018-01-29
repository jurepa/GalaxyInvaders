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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace SpaceInvaders
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Game : Page
    {
        public VMGame vMGame { get; }
        private DispatcherTimer timer = new DispatcherTimer();
        private bool estaDisparando;

        public Game()
        {
            this.InitializeComponent();
            //Window.Current.Content.KeyDown += KeyDownEvent;
            vMGame =(VMGame) this.DataContext;
            //vMGame.canvas = this.canvas;
            timer.Interval = new TimeSpan(0,0,0,0,100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            if (estaDisparando)
            {
                Disparo disparo = new Disparo(vMGame.posYMisil, vMGame.posX, 20, 10, new Uri("ms-appx:///Assets/Images/MisilPro.png"));
                Image playerBullet = new Image();
                playerBullet.Source = new BitmapImage(disparo.imagen);
                Canvas.SetTop(playerBullet, Canvas.GetTop(this.player));
                Canvas.SetLeft(playerBullet, Canvas.GetLeft(this.player));
                this.canvas.Children.Add(playerBullet);
                if (Canvas.GetTop(playerBullet) >= 0)
                {
                    Canvas.SetTop(playerBullet, Canvas.GetTop(playerBullet) - disparo.velocidad);
                }
            }
        }

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
            Window.Current.Content.KeyDown += Disparo_KeyDown;
            Window.Current.Content.KeyUp += Disparo_KeyUp;
        }

        private void Disparo_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Space)
            {
                estaDisparando = false;
            }
        }

        private void Disparo_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Space)
            {
                //Disparo disparo = new Disparo(vMGame.posYMisil,vMGame.posX,20,10,new Uri("ms-appx:///Assets/Images/MisilPro.png"));
                //Image playerBullet = new Image();
                //playerBullet.Source = new BitmapImage(disparo.imagen);
                //Canvas.SetTop(playerBullet, Canvas.GetTop(this.player));
                //Canvas.SetLeft(playerBullet, Canvas.GetLeft(this.player));
                //this.canvas.Children.Add(playerBullet);
                //moveBullet(disparo.velocidad,playerBullet);
                estaDisparando = true;
            }
        }
        //private void moveBullet(int velocidad, Image playerBullet)
        //{
        //    while (Canvas.GetTop(playerBullet) >=0)
        //    {
                
        //        Canvas.SetTop(playerBullet, Canvas.GetTop(playerBullet) - velocidad);
        //    }
        //    this.canvas.Children.Remove(playerBullet);
        //}
    }
}
