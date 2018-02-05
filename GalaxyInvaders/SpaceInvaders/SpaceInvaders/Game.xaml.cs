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
        public List<NaveEnemiga> listaEnemigos;
        public List<Image> listaImagenesNavesEnemigas;
        //private bool haPulsado;
        //private bool haLevantado;
        private DispatcherTimer timer = new DispatcherTimer();
        //private bool estaDisparando;

        public Game()
        {
            listaEnemigos = new List<NaveEnemiga>();
            listaImagenesNavesEnemigas = new List<Image>();
            //haPulsado = false;
            //haLevantado = false;
            this.InitializeComponent();
            cargaNaves();
            //Window.Current.Content.KeyDown += KeyDownEvent;

            vMGame = (VMGame)this.DataContext;

            //vMGame.canvas = this.canvas;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            move();
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
                for (int i = 0; i < listaImagenesNavesEnemigas.Count; i++)
                {
                    if (this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(i)))
                    {
                        if (Canvas.GetTop(listaImagenesNavesEnemigas.ElementAt(i)) + 49 >= Canvas.GetTop(playerBullet) && Canvas.GetLeft(listaImagenesNavesEnemigas.ElementAt(i)) <= Canvas.GetLeft(playerBullet) && Canvas.GetLeft(listaImagenesNavesEnemigas.ElementAt(i)) + 50 > Canvas.GetLeft(playerBullet))
                        {
                            this.canvas.Children.Remove(playerBullet);
                            listaImagenesNavesEnemigas.ElementAt(i).Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/explosion.gif"));
                            await Task.Delay(500);
                            this.canvas.Children.Remove(listaImagenesNavesEnemigas.ElementAt(i));

                        }
                    }
                }
                if (Canvas.GetTop(playerBullet) < 0)
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

        private void cargaNaves()
        {
            NaveEnemiga nave = null;
            Image imagenNave = null;
            int posX = 20;
            int posY = 50;

            for (int i = 0; i < 60; i++)
            {
                nave = new NaveEnemiga();
                imagenNave = new Image();
                if (i == 0 || i == 12 || i == 24 || i == 36 || i == 48)
                {
                    nave.posX = 20;
                }
                else
                {
                    nave.posX = posX + 70;
                }

                //Fila 1
                if (i >= 0 && i <= 11)
                {
                    nave.posY = posY - 30;
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien1.gif");
                }//Fila 2
                else if (i >= 12 && i <= 23)
                {
                    nave.posY = posY * 2 - 10;
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien2Pro.png");
                }//Fila 3
                else if (i >= 24 && i <= 35)
                {
                    nave.posY = posY * 3 - 10;
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien2Pro.png");
                }//Fila 4
                else if (i >= 36 && i <= 47)
                {
                    nave.posY = posY * 4 + 10;
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien3Pro.png");
                }
                else //Fila 5
                {
                    nave.posY = posY * 5 + 30;
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien3Pro.png");
                }

                //Actualizamos valor de posX
                posX = nave.posX;

                imagenNave.Source = new BitmapImage(nave.imagen);
                imagenNave.Height = 37;
                imagenNave.Width = 37;
                this.canvas.Children.Add(imagenNave);
                Canvas.SetTop(imagenNave, nave.posY);
                Canvas.SetLeft(imagenNave, nave.posX);

                nave.dirX = 1;
                listaEnemigos.Add(nave);
                listaImagenesNavesEnemigas.Add(imagenNave);
            }
        }
        public void move()
        {
            Image imagenNave = new Image();
            NaveEnemiga naveEnemiga = new NaveEnemiga();
            //var childs = this.canvas.Children;
            int moveX = 5;
            int direccion = 0;//1 derecha, 2 Izquierda

            for (int i = 0; i < listaImagenesNavesEnemigas.Count; i++)
            {
                //Todas las naves no llegan hasta el final

                //Comprobar posX para ir a la izq o dcha
                naveEnemiga = listaEnemigos.ElementAt(i);
                imagenNave = listaImagenesNavesEnemigas.ElementAt(i);

                if ((naveEnemiga.posX + (37 + moveX)) >= 1350)//Borde derecho
                {
                    naveEnemiga.dirX = 2;
                    aumentaPosY(i, imagenNave, naveEnemiga);
                    //Canvas.SetTop(imagenNave, naveEnemiga.posY);
                }
                else if ((naveEnemiga.posX - moveX) <= 0)//Borde izquierdo
                {
                    naveEnemiga.dirX = 1;
                    aumentaPosY(i, imagenNave, naveEnemiga);
                    //Canvas.SetTop(imagenNave, naveEnemiga.posY);
                }

                imagenNave = listaImagenesNavesEnemigas.ElementAt(i);
                if (naveEnemiga.dirX == 1)//Borde derecho
                {
                    //moveX = 2;
                    naveEnemiga.posX = naveEnemiga.posX + moveX;
                    Canvas.SetLeft(imagenNave, naveEnemiga.posX);
                }
                else if (naveEnemiga.dirX == 2)//Borde izquierdo
                {
                    naveEnemiga.posX = naveEnemiga.posX - moveX;
                    Canvas.SetLeft(imagenNave, naveEnemiga.posX);
                }
                listaEnemigos.ElementAt(i).dirX = naveEnemiga.dirX;
                listaEnemigos.ElementAt(i).posX = naveEnemiga.posX;
            }
        }

        public void aumentaPosY(int posicionNave, Image imagenNave, NaveEnemiga naveEnemiga)
        {
            listaEnemigos.ElementAt(posicionNave).posY = listaEnemigos.ElementAt(posicionNave).posY + 10;
            Canvas.SetTop(imagenNave, naveEnemiga.posY);
        }
    }
}
