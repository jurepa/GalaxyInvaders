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
        public DispatcherTimer timer = new DispatcherTimer();
        public DispatcherTimer timerDisparoEnemigo = new DispatcherTimer();
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

            timerDisparoEnemigo.Interval = new TimeSpan(0, 0, 0, 2, 0);
            timerDisparoEnemigo.Tick += tickDisparoEnemigo;
            timerDisparoEnemigo.Start();
        }

        private void tickDisparoEnemigo(object sender, object e)
        {
            disparoEnemigo();
        }

        private void Timer_Tick(object sender, object e)
        {
            move();
        }

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
                await Task.Delay(50);
                Canvas.SetTop(playerBullet, Canvas.GetTop(playerBullet) - velocidad);
                for (int i = 0; i < listaImagenesNavesEnemigas.Count; i++)
                {
                    detectaColision(i, playerBullet);
                }
                if (Canvas.GetTop(playerBullet) < 0)
                {
                    this.canvas.Children.Remove(playerBullet);
                }
            }
        }

        public async void detectaColision(int i, Image playerBullet)
        {
            if (this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(i)))
            {
                if (Canvas.GetTop(listaImagenesNavesEnemigas.ElementAt(i)) <= Canvas.GetTop(playerBullet) && Canvas.GetTop(listaImagenesNavesEnemigas.ElementAt(i)) + 30 >= Canvas.GetTop(playerBullet) && Canvas.GetLeft(listaImagenesNavesEnemigas.ElementAt(i)) <= Canvas.GetLeft(playerBullet) && Canvas.GetLeft(listaImagenesNavesEnemigas.ElementAt(i)) + 38 >= Canvas.GetLeft(playerBullet))
                {
                    this.canvas.Children.Remove(playerBullet);
                    listaImagenesNavesEnemigas.ElementAt(i).Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/explosion.gif"));
                    await Task.Delay(500);
                    this.canvas.Children.Remove(listaImagenesNavesEnemigas.ElementAt(i));

                    //Marcamos la nave enemiga como que no puede disparar
                    listaEnemigos.ElementAt(i).puedeDisparar = false;

                    siguienteNaveQuePuedeDisparar(i);
                }
            }
        }

        public void siguienteNaveQuePuedeDisparar(int indiceNave)
        {
        if (indiceNave >= 12 && indiceNave <= 23)
            {
                if (!this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave + 12))
                    && !this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave + 24))
                   && !this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave + 36))
                   && this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave - 12)))
                {
                    listaEnemigos.ElementAt(indiceNave - 12).puedeDisparar = true;
                }
            }//Fila 3
            else if (indiceNave >= 24 && indiceNave <= 35)
            {
                if (!this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave + 12))
                    && !this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave + 24))
                    && this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave - 12)))             
                {
                    listaEnemigos.ElementAt(indiceNave - 12).puedeDisparar = true;
                }
                else if (!this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave + 12))
                    && !this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave + 24))
                    && this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave - 24)))
                {
                    listaEnemigos.ElementAt(indiceNave - 24).puedeDisparar = true;
                }
            }//Fila 4
            else if (indiceNave >= 36 && indiceNave <= 47)
            {
                if (!this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave + 12))
                    && this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave - 12)))
                {
                    listaEnemigos.ElementAt(indiceNave - 12).puedeDisparar = true;//Aqui esta el lio
                }
                else if (!this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave + 12))
                   && this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave - 24)))
                {
                    listaEnemigos.ElementAt(indiceNave - 24).puedeDisparar = true;
                }
                else if (!this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave + 12))
                  && this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave - 36)))
                {
                    listaEnemigos.ElementAt(indiceNave - 36).puedeDisparar = true;
                }
            }
            else if (indiceNave >= 48 && indiceNave <= 59) //Fila 5
            {
                if (this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave - 12)))
                {
                    listaEnemigos.ElementAt(indiceNave - 12).puedeDisparar = true;
                }
                else if (this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave - 24)))
                {
                    listaEnemigos.ElementAt(indiceNave - 24).puedeDisparar = true;
                }
                else if (this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave - 36)))
                {
                    listaEnemigos.ElementAt(indiceNave - 36).puedeDisparar = true;
                }
                else if (this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(indiceNave - 48)))
                {
                    listaEnemigos.ElementAt(indiceNave - 48).puedeDisparar = true;
                }
            }
        }

        private void disparar()
        {
            if (this.player.Opacity!=0)
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
        }

        private void disparoEnemigo()
        {
            //Generar aleatorio para disparo
            Random random = new Random();
            int numeroGenerado = 0;
            NaveEnemiga naveEnemiga = new NaveEnemiga();

            //Conseguir la posicion de la nave que va a disparar
            do
            {
                numeroGenerado = random.Next(0, 59);
            } while (listaEnemigos.ElementAt(numeroGenerado).puedeDisparar==false);

            naveEnemiga = listaEnemigos.ElementAt(numeroGenerado);

            Disparo disparo = new Disparo(naveEnemiga.posY+37, naveEnemiga.posX+19, 5, 10, new Uri("ms-appx:///Assets/Images/MisilEnemigoPro.png"));
            Image playerBullet = new Image();
            playerBullet.Source = new BitmapImage(disparo.imagen);
            playerBullet.Height = 20;
            playerBullet.Width = 10;
            Canvas.SetTop(playerBullet, disparo.dirY);
            Canvas.SetLeft(playerBullet, disparo.dirX);
            this.canvas.Children.Add(playerBullet);
            moveBulletEnemigo(disparo.velocidad, playerBullet);

            /*Disparo disparo = new Disparo(vMGame.posYMisil, vMGame.player.posicionX, 20, 10, new Uri("ms-appx:///Assets/Images/MisilEnemigoPro.png"));
            Image playerBullet = new Image();
            playerBullet.Source = new BitmapImage(disparo.imagen);
            playerBullet.Height = 20;
            playerBullet.Width = 10;
            Canvas.SetTop(playerBullet, Canvas.GetTop(this.player));
            Canvas.SetLeft(playerBullet, Canvas.GetLeft(this.player) + 35);
            this.canvas.Children.Add(playerBullet);
            moveBullet(disparo.velocidad, playerBullet);*/
        }

        private async void moveBulletEnemigo(int velocidad, Image enemyBullet)
        {
            while (this.canvas.Children.Contains(enemyBullet))
            {
                await Task.Delay(50);
                Canvas.SetTop(enemyBullet, Canvas.GetTop(enemyBullet) + velocidad);

                //Detectar colision con nuestra nave
                if (this.canvas.Children.Contains(this.player))
                {
                    if (Canvas.GetTop(this.player) <= Canvas.GetTop(enemyBullet)+15 && 
                        /*Canvas.GetTop(this.player) + 30 >= Canvas.GetTop(enemyBullet) &&*/
                        Canvas.GetLeft(this.player) <= Canvas.GetLeft(enemyBullet)+10 && 
                        Canvas.GetLeft(this.player) + 70 >= Canvas.GetLeft(enemyBullet))
                    {
                        this.canvas.Children.Remove(enemyBullet);
                        this.player.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/explosion.gif"));
                        //await Task.Delay(500);
                        
                        vMGame.player.vidas--;
                        cambiaVisibilidad();

                        await Task.Delay(800);

                        if (vMGame.player.vidas==0)
                        {
                            //
                            this.player.Opacity = 0;                           
                            timer.Stop();
                            timerDisparoEnemigo.Stop();

                            //Mostrar mensaje 

                        }
                        else
                        {                           
                            this.player.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/PlayerPro.png"));
                        }                       
                    }
                }
            }
        }


        public void cambiaVisibilidad()
        {
            switch (vMGame.player.vidas)
            {
                case 2:
                    vMGame.player.vida3 = 0;
                break;

                case 1:
                    vMGame.player.vida2 = 0;
                break;

                case 0:
                    vMGame.player.vida1 = 0;

                break;
            }
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
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien2.gif");
                    nave.valor = 40;
                }//Fila 2
                else if (i >= 12 && i <= 23)
                {
                    nave.posY = posY * 2 - 10;
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien1.gif");
                    nave.valor = 30;
                }//Fila 3
                else if (i >= 24 && i <= 35)
                {
                    nave.posY = posY * 3 - 10;
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien1.gif");
                    nave.valor = 30;
                }//Fila 4
                else if (i >= 36 && i <= 47)
                {
                    nave.posY = posY * 4 + 10;
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien3.gif");
                    nave.valor = 20;
                }
                else //Fila 5
                {
                    nave.posY = posY * 5 + 30;
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien3.gif");
                    nave.valor = 20;
                    nave.puedeDisparar = true;
                }

                nave.estado = true;

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
