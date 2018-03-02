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
using Entities;
using Windows.UI.ViewManagement;

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
        public List<Defensa> listaDefensas;
        public List<Image> listaImagenesDefensa;
        public List<NaveEnemiga> listaEnemigos;
        public List<Image> listaImagenesNavesEnemigas;
        public int cantidadNaves;
        public int cantidadDefensas;
        private int dificultad;
        private JugadorConDificultad JugadorConDificultad;
        //private bool haPulsado;
        //private bool haLevantado;
        public DispatcherTimer timer = new DispatcherTimer();
        public DispatcherTimer timerDisparoEnemigo = new DispatcherTimer();
        public DispatcherTimer timerDisparo = new DispatcherTimer();
        public ContentDialog hasGanadoContentDialog = new ContentDialog();
        public Boolean noDispares = false;
        public Boolean haGanado = false;
        public Boolean pausa = false;
        private MediaElement mediaPlayer;
        private string mensajeVictoria = "Victoria!!";
        private string mensajeDerrota = "Ooooh, you lose...";

        //private bool estaDisparando;

        public Game()
        {
            listaDefensas = new List<Defensa>();
            listaEnemigos = new List<NaveEnemiga>();
            listaImagenesNavesEnemigas = new List<Image>();
            //haPulsado = false;
            //haLevantado = false;
            this.InitializeComponent();
            
            cargaDefensas();
            //Window.Current.Content.KeyDown += KeyDownEvent;
            cantidadNaves = 60;
            cantidadDefensas = 32;
            vMGame = (VMGame)this.DataContext;

            //vMGame.canvas = this.canvas;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
            timer.Start();
            timerDisparo.Interval = new TimeSpan(0,0,1);
            timerDisparo.Tick += TimerDisparo_Tick;
            timerDisparo.Start();
            //timerDisparoEnemigo.Interval = new TimeSpan(0, 0, 0, 20, 0);
            //timerDisparoEnemigo.Tick += tickDisparoEnemigo;
            //timerDisparoEnemigo.Start();

            //Si esta en modo tactil
            if (UIViewSettings.GetForCurrentView().UserInteractionMode != UserInteractionMode.Touch)
            {
                this.relativeBotonesTactiles.Children.Remove(this.btnIzq);
                this.relativeBotonesTactiles.Children.Remove(this.btnDcha);
                this.relativeBotonesTactiles.Children.Remove(this.btnDisparo);
            }
        }
        /// <summary>
        /// Este método restringe que solo se pueda disparar una vez por segundo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerDisparo_Tick(object sender, object e)
        {
            noDispares = false;
        }

        /// <summary>
        /// Este se ejecutará más o menos dependiendo la dificultad elegida y se encarga de pintar el disparo en el canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tickDisparoEnemigo(object sender, object e)
        {
            disparoEnemigo();
        }

        /// <summary>
        /// Este método se encargará de mover las naves enemigas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, object e)
        {
            move();
        }

        /// <summary>
        /// Cuando la vista Game obtiene el foco, inicializa estos eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allowfocus_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.Content.KeyDown += this.vMGame.Grid_KeyDown;
            Window.Current.Content.KeyDown += Disparo_KeyDown;
            Window.Current.Content.KeyUp += this.vMGame.Grid_KeyUp;//Para el movimiento de nuestra nave
            Window.Current.Content.KeyUp += Disparo_KeyUp;//Para el disparo de nuestra nave
            //Window.Current.Content.KeyUp += Disparo_KeyUp;
        }

        /// <summary>
        /// Como el key up pero para los botones, se encarga de disparar en pantallas táctiles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnDisparo_PointerExited(object sender, PointerRoutedEventArgs e)//
        {
            Image image = (Image)sender;
            if (image.Name.Equals("btnDisparo") && !noDispares)
            {
                disparar();
                noDispares = true;
            }
        }
        /// <summary>
        /// Evento que se llama cuando se levanta una tecla, en nuestro caso, cuando se levante el espacio para disparar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Disparo_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Space && !noDispares)
            {
                //haLevantado = true;
                disparar();
                noDispares = true;
            }
        }
        /// <summary>
        /// Evento que se llama cuando se pulsa una tecla, en nuestro caso, cuando se pulsa ESC para pausar o quitar la pausa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Disparo_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //if (e.Key == VirtualKey.Space)
            //{
            //    haPulsadoEspacio = true;
            //}
            if (e.Key == VirtualKey.Escape && !pausa) //CUANDO REANUDAS EL JUEGO LOS MISILES QUE HABIAN Y SE HAN PARADO NO SE MUEVEN
            {
                pausar(0);
                /*noDispares = true;
                pausa = true;
                timer.Stop();
                Window.Current.Content.KeyDown -= this.vMGame.Grid_KeyDown;
                this.vMGame.opacidadPausa = 1;*/
            }
            else if (e.Key == VirtualKey.Escape && pausa)
            {
                volverdepausa();
                /*noDispares = false;
                pausa = false;
                timer.Start();
                Window.Current.Content.KeyDown += this.vMGame.Grid_KeyDown;
                this.vMGame.opacidadPausa = 0;*/
            }
        }

        /// <summary>
        /// Este método mueve la bala pintada en el canvas del jugador hacia arriba
        /// </summary>
        /// <param name="velocidad"></param>
        /// <param name="playerBullet"></param>
        private async void moveBullet(int velocidad, Image playerBullet)
        {
            while (this.canvas.Children.Contains(playerBullet))
            {
                await Task.Delay(50);
                Canvas.SetTop(playerBullet, Canvas.GetTop(playerBullet) - velocidad);
                if (!pausa)
                {
                    //Colision Con Defensas
                    for (int i = 0; i < listaImagenesDefensa.Count; i++)
                    {
                        detectaColisionDefensaAliada(i, playerBullet);
                    }
                    if (Canvas.GetTop(playerBullet) < 0)
                    {
                        this.canvas.Children.Remove(playerBullet);
                    }

                    //Colision Con Naves Enemigas
                    for (int i = 0; i < listaImagenesNavesEnemigas.Count; i++)
                    {
                        detectaColision(i, playerBullet);
                    }
                    if (Canvas.GetTop(playerBullet) < 0)
                    {
                        this.canvas.Children.Remove(playerBullet);
                    }
                }
                else if (pausa && Canvas.GetTop(playerBullet) < 0)
                {
                    this.canvas.Children.Remove(playerBullet);
                }
            }
        }

        /// <summary>
        /// Este método detecta el impacto de una bala del jugador con las defensas y las borra al segundo impacto
        /// </summary>
        /// <param name="i"></param>
        /// <param name="playerBullet"></param>
        public async void detectaColisionDefensaAliada(int i, Image playerBullet)
        {
            if (this.canvas.Children.Contains(listaImagenesDefensa.ElementAt(i)))
            {
                if (this.listaDefensas.ElementAt(i).puedeImpactar)
                {
                    if (Canvas.GetTop(listaImagenesDefensa.ElementAt(i)) <= Canvas.GetTop(playerBullet) && Canvas.GetTop(listaImagenesDefensa.ElementAt(i)) + 35 >= Canvas.GetTop(playerBullet) && Canvas.GetLeft(listaImagenesDefensa.ElementAt(i)) <= Canvas.GetLeft(playerBullet) && Canvas.GetLeft(listaImagenesDefensa.ElementAt(i)) + 40 >= Canvas.GetLeft(playerBullet))
                    {
                        reproducirAudioAsync("romperroca.mp3");
                        this.listaDefensas.ElementAt(i).impactos++; //Le Sumamos un Impacto
                        this.canvas.Children.Remove(playerBullet); //Borramos la Bala porque ha chocado.
                        if (this.listaDefensas.ElementAt(i).impactos == 1)
                        {
                            listaImagenesDefensa.ElementAt(i).Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/ExplosionMeteorito.gif"));
                            await Task.Delay(200);
                            listaImagenesDefensa.ElementAt(i).Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/DefendRota.png"));
                        }
                        if (this.listaDefensas.ElementAt(i).impactos == 2)
                        {
                            cantidadDefensas--;
                            this.listaDefensas.ElementAt(i).puedeImpactar = false;
                            listaImagenesDefensa.ElementAt(i).Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/ExplosionMeteorito2.gif"));
                            await Task.Delay(500);
                            this.canvas.Children.Remove(listaImagenesDefensa.ElementAt(i));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Detecta la colisión de la bala del jugador con una nave enemiga, si no impacta en ninguna nave, al llegar arriba del todo se borrará la bala
        /// </summary>
        /// <param name="i"></param>
        /// <param name="playerBullet"></param>
        public async void detectaColision(int i, Image playerBullet)
        {

            if (this.canvas.Children.Contains(listaImagenesNavesEnemigas.ElementAt(i)))
            {
                if (this.listaEnemigos.ElementAt(i).puedeSerGolpeado)
                {
                    if (Canvas.GetTop(listaImagenesNavesEnemigas.ElementAt(i)) <= Canvas.GetTop(playerBullet) && Canvas.GetTop(listaImagenesNavesEnemigas.ElementAt(i)) + 30 >= Canvas.GetTop(playerBullet) && Canvas.GetLeft(listaImagenesNavesEnemigas.ElementAt(i)) <= Canvas.GetLeft(playerBullet) && Canvas.GetLeft(listaImagenesNavesEnemigas.ElementAt(i)) + 38 >= Canvas.GetLeft(playerBullet))
                    {
                        cantidadNaves--;
                        this.listaEnemigos.ElementAt(i).puedeSerGolpeado = false;
                        this.vMGame.jugador.Puntuacion = this.vMGame.jugador.Puntuacion + listaEnemigos.ElementAt(i).valor;
                        listaEnemigos.ElementAt(i).valor = 0;
                        this.puntuacion.Text = Convert.ToString(this.vMGame.jugador.Puntuacion);
                        this.canvas.Children.Remove(playerBullet);
                        listaImagenesNavesEnemigas.ElementAt(i).Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/explosion.gif"));
                        await Task.Delay(500);
                        this.canvas.Children.Remove(listaImagenesNavesEnemigas.ElementAt(i));
                        reproducirAudioAsync("explosion.mp3");

                        //Marcamos la nave enemiga como que no puede disparar
                        listaEnemigos.ElementAt(i).puedeDisparar = false;
                        if (cantidadNaves == 0 && !haGanado)
                        {
                            //Paramos los timer
                            timer.Stop();
                            timerDisparoEnemigo.Stop();
                            noDispares = true;
                            haGanado = true;
                            mostrarGanadorPerdedor(mensajeVictoria);

                        }
                        siguienteNaveQuePuedeDisparar(i);
                    }
                }
            }
        }
        /// <summary>
        /// Muestra si has ganado o has perdido según el mensaje enviado con dos botones, uno para reiniciar partida y otro para subir
        /// la puntuación y volver al menú principal
        /// </summary>
        /// <param name="mensaje">string que indica victoria o derrota</param>
        private async void mostrarGanadorPerdedor(string mensaje)
        {
            //ContentDialog hasGanado = new ContentDialog();
            hasGanadoContentDialog.Title = mensaje;
            hasGanadoContentDialog.Content = "Enhorabuena, has hecho " + this.vMGame.jugador.Puntuacion + " puntos";
            hasGanadoContentDialog.PrimaryButtonText = "Continue";
            hasGanadoContentDialog.SecondaryButtonText = "Restart";
            ContentDialogResult resultado = await hasGanadoContentDialog.ShowAsync();

            if (resultado == ContentDialogResult.Primary /*&& !haPulsadoEspacio*/)
            {
                this.vMGame.submitScore();
                this.Frame.Navigate(typeof(MainPage));
            }
            else if(resultado==ContentDialogResult.Secondary)
            {
                this.Frame.Navigate(typeof(Game),JugadorConDificultad);
            }
        }

        /// <summary>
        /// Este método indica si una nave puede disparar,
        /// ya que no todas las naves pueden disparar, puede que tenga alguna debajo
        /// </summary>
        /// <param name="indiceNave">Índice de la nave en la lista</param>
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

        /// <summary>
        /// Pinta la bala del jugador en el canvas
        /// </summary>
        private /*async*/ void disparar()
        {
            //await Task.Delay(2000);
            if (this.player.Opacity != 0)
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
                reproducirAudioAsync("disparo.wav");
            }
        }
        /// <summary>
        /// Genera un número aleatorio que indica la nave que va a disparar y pinta la bala en el canvas
        /// </summary>
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
            } while (listaEnemigos.ElementAt(numeroGenerado).puedeDisparar == false);

            naveEnemiga = listaEnemigos.ElementAt(numeroGenerado);

            Disparo disparo = new Disparo(naveEnemiga.posY + 37, naveEnemiga.posX + 19, 5, 10, new Uri("ms-appx:///Assets/Images/MisilEnemigoPro.png"));
            Image playerBullet = new Image();
            playerBullet.Source = new BitmapImage(disparo.imagen);
            playerBullet.Height = 20;
            playerBullet.Width = 10;
            Canvas.SetTop(playerBullet, disparo.dirY);
            Canvas.SetLeft(playerBullet, disparo.dirX);
            this.canvas.Children.Add(playerBullet);
            moveBulletEnemigo(disparo.velocidad, playerBullet);
        }

        /// <summary>
        /// Mueve la bala enemiga hacia abajo
        /// </summary>
        /// <param name="velocidad"></param>
        /// <param name="enemyBullet"></param>
        private async void moveBulletEnemigo(int velocidad, Image enemyBullet)
        {
            while (this.canvas.Children.Contains(enemyBullet))
            {
                await Task.Delay(50);
                Canvas.SetTop(enemyBullet, Canvas.GetTop(enemyBullet) + velocidad);
                //Detectar Colision con Defensa
                //Colision Con Defensas
                if (!pausa)
                {
                    for (int i = 0; i < listaImagenesDefensa.Count; i++)
                    {
                        detectaColisionDefensaEnemigo(i, enemyBullet);
                    }
                    /*if (Canvas.GetTop(enemyBullet) < 0)
                    {
                        this.canvas.Children.Remove(enemyBullet);
                    }*/

                    //Detectar colision con nuestra nave
                    if (this.canvas.Children.Contains(this.player))
                    {
                        if (Canvas.GetTop(this.player) <= Canvas.GetTop(enemyBullet) + 15 &&
                            /*Canvas.GetTop(this.player) + 30 >= Canvas.GetTop(enemyBullet) &&*/
                            Canvas.GetLeft(this.player) <= Canvas.GetLeft(enemyBullet) + 10 &&
                            Canvas.GetLeft(this.player) + 70 >= Canvas.GetLeft(enemyBullet))
                        {
                            this.canvas.Children.Remove(enemyBullet);
                            this.player.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/explosion.gif"));
                            //await Task.Delay(500);

                            vMGame.player.vidas--;
                            cambiaVisibilidad();



                            if (vMGame.player.vidas <= 0)
                            {
                                /*this.player.Opacity = 0;
                                timer.Stop();
                                timerDisparoEnemigo.Stop();*/
                                pausar(1);
                                //Mostrar mensaje para reiniciar partida
                                mostrarGanadorPerdedor(mensajeDerrota);
                            }
                            else
                            {
                                await Task.Delay(200);
                                this.player.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/PlayerPro.png"));
                            }
                        }
                        else if (Canvas.GetTop(enemyBullet) + 20 >= 600)//Eliminar Disparo del canvas
                        {//SETA ATOMICA
                            enemyBullet.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/ExplosionMisil.gif"));
                            enemyBullet.Width = 50;
                            enemyBullet.Height = 50;
                            enemyBullet.Margin = new Thickness(0, 0, 100, 100);
                            await Task.Delay(500);
                            this.canvas.Children.Remove(enemyBullet);
                        }
                    }
                    
                }
                else if (Canvas.GetTop(enemyBullet) + 20 >= 600)
                {
                    enemyBullet.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/ExplosionMisil.gif"));
                    enemyBullet.Width = 50;
                    enemyBullet.Height = 50;
                    enemyBullet.Margin = new Thickness(0, 0, 100, 100);
                    await Task.Delay(500);
                    this.canvas.Children.Remove(enemyBullet);
                }
            }
        }
        /// <summary>
        /// Detecta la colisión de la bala enemiga con las defensas, si choca, borra la bala
        /// </summary>
        /// <param name="i"></param>
        /// <param name="playerBullet"></param>
        public async void detectaColisionDefensaEnemigo(int i, Image playerBullet)
        {
            if (this.canvas.Children.Contains(listaImagenesDefensa.ElementAt(i)))
            {
                if (this.listaDefensas.ElementAt(i).puedeImpactar)
                {
                    if (Canvas.GetTop(listaImagenesDefensa.ElementAt(i)) <= Canvas.GetTop(playerBullet) + 20 && Canvas.GetTop(listaImagenesDefensa.ElementAt(i)) + 40 >= Canvas.GetTop(playerBullet) && Canvas.GetLeft(listaImagenesDefensa.ElementAt(i)) <= Canvas.GetLeft(playerBullet) + 10 && Canvas.GetLeft(listaImagenesDefensa.ElementAt(i)) + 38 >= Canvas.GetLeft(playerBullet))
                    {
                        reproducirAudioAsync("romperroca.mp3");
                        this.listaDefensas.ElementAt(i).impactos++; //Le Sumamos un Impacto
                        this.canvas.Children.Remove(playerBullet); //Borramos la Bala porque ha chocado.
                        if (this.listaDefensas.ElementAt(i).impactos == 1)
                        {
                            listaImagenesDefensa.ElementAt(i).Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/ExplosionMeteorito.gif"));
                            await Task.Delay(200);
                            listaImagenesDefensa.ElementAt(i).Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/DefendRota.png"));
                        }
                        if (this.listaDefensas.ElementAt(i).impactos == 2)
                        {
                            cantidadDefensas--;
                            this.listaDefensas.ElementAt(i).puedeImpactar = false;
                            listaImagenesDefensa.ElementAt(i).Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/ExplosionMeteorito2.gif"));
                            await Task.Delay(500);
                            this.canvas.Children.Remove(listaImagenesDefensa.ElementAt(i));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cambia la visibilidad de las vidas del jugador
        /// </summary>
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

        /*public async void mostrarPerdedor()
        {
            ContentDialog perdedorDialog = new ContentDialog();
            perdedorDialog.Title = "Ooooh, you lose...";
            perdedorDialog.Content = $"Has hecho {this.vMGame.jugador.Puntuacion} puntos";
            perdedorDialog.PrimaryButtonText = "Continue";
            perdedorDialog.SecondaryButtonText = "Restart";
            ContentDialogResult contentDialogResult = await perdedorDialog.ShowAsync();

            if (contentDialogResult == ContentDialogResult.Primary)
            {
                this.vMGame.submitScore();
                this.Frame.Navigate(typeof(MainPage));
            }
            else if (contentDialogResult == ContentDialogResult.Secondary)
            {
                this.Frame.Navigate(typeof(Game),jugadorActual);
            }
        }*/
        /// <summary>
        /// Se encarga de pintar las naves en el canvas
        /// </summary>
        private void cargaNaves()
        {
            NaveEnemiga nave = null;
            Image imagenNave = null;
            int posX = 20;
            int posY = 50;

            for (int i = 0; i < 60; i++)
            {
                nave = new NaveEnemiga();
                nave.puedeSerGolpeado = true;
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
                    nave.valor = 40 * dificultad;
                }//Fila 2
                else if (i >= 12 && i <= 23)
                {
                    nave.posY = posY * 2 - 10;
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien1.gif");
                    nave.valor = 30 * dificultad;
                }//Fila 3
                else if (i >= 24 && i <= 35)
                {
                    nave.posY = posY * 3 - 10;
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien1.gif");
                    nave.valor = 30 * dificultad;
                }//Fila 4
                else if (i >= 36 && i <= 47)
                {
                    nave.posY = posY * 4 + 10;
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien3.gif");
                    nave.valor = 20 * dificultad;
                }
                else //Fila 5
                {
                    nave.posY = posY * 5 + 30;
                    nave.imagen = new Uri("ms-appx:///Assets/Images/Alien3.gif");
                    nave.valor = 20 * dificultad;
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
        /// <summary>
        /// Se encarga de pintar las defensas en el canvas
        /// </summary>
        private void cargaDefensas()
        {
            int posXB1 = 100;
            int posYB1 = 480;
            int posXB2 = 410;
            int posYB2 = 480;
            int posXB3 = 750;
            int posYB3 = 480;
            int posXB4 = 1080;
            int posYB4 = 480;

            listaImagenesDefensa = new List<Image>();
            int ultimoIndice;

            /*
             *   3 4
             * 1 2 5 6
             * 0     7
             */
            //Bloques 1
            for (int i = 0; i < 8; i++)
            {
                Defensa defensa = new Defensa();
                Image imagenDefensa = new Image();
                if (i == 0)
                {
                    defensa.posicionX = posXB1;
                    defensa.posicionY = posYB1;
                }
                if (i == 1)
                {
                    defensa.posicionX = posXB1;
                    defensa.posicionY = posYB1 - 40;
                }
                if (i == 2)
                {
                    defensa.posicionX = posXB1 + 40;
                    defensa.posicionY = posYB1 - 40;
                }
                if (i == 3)
                {
                    defensa.posicionX = posXB1 + 40;
                    defensa.posicionY = posYB1 - 40 - 40;
                }
                if (i == 4)
                {
                    defensa.posicionX = posXB1 + 40 + 40;
                    defensa.posicionY = posYB1 - 40 - 40;
                }
                if (i == 5)
                {
                    defensa.posicionX = posXB1 + 40 + 40;
                    defensa.posicionY = posYB1 - 40 - 40 + 40;
                }
                if (i == 6)
                {
                    defensa.posicionX = posXB1 + 40 + 40 + 40;
                    defensa.posicionY = posYB1 - 40 - 40 + 40;
                }
                if (i == 7)
                {
                    defensa.posicionX = posXB1 + 40 + 40 + 40;
                    defensa.posicionY = posYB1 - 40 - 40 + 40 + 40;
                }

                imagenDefensa.Source = new BitmapImage(defensa.imagen);
                imagenDefensa.Height = 40;
                imagenDefensa.Width = 40;
                this.canvas.Children.Add(imagenDefensa);
                Canvas.SetLeft(imagenDefensa, defensa.posicionX);
                Canvas.SetTop(imagenDefensa, defensa.posicionY);
                ultimoIndice = listaImagenesDefensa.Count;
                imagenDefensa.Name = ultimoIndice.ToString();
                listaImagenesDefensa.Add(imagenDefensa);
                listaDefensas.Add(defensa);
            }

            //Bloques 2
            for (int i = 0; i < 8; i++)
            {
                Defensa defensa = new Defensa();
                Image imagenDefensa = new Image();
                if (i == 0)
                {
                    defensa.posicionX = posXB2;
                    defensa.posicionY = posYB2;
                }
                if (i == 1)
                {
                    defensa.posicionX = posXB2;
                    defensa.posicionY = posYB2 - 40;
                }
                if (i == 2)
                {
                    defensa.posicionX = posXB2 + 40;
                    defensa.posicionY = posYB2 - 40;
                }
                if (i == 3)
                {
                    defensa.posicionX = posXB2 + 40;
                    defensa.posicionY = posYB2 - 40 - 40;
                }
                if (i == 4)
                {
                    defensa.posicionX = posXB2 + 40 + 40;
                    defensa.posicionY = posYB2 - 40 - 40;
                }
                if (i == 5)
                {
                    defensa.posicionX = posXB2 + 40 + 40;
                    defensa.posicionY = posYB2 - 40 - 40 + 40;
                }
                if (i == 6)
                {
                    defensa.posicionX = posXB2 + 40 + 40 + 40;
                    defensa.posicionY = posYB2 - 40 - 40 + 40;
                }
                if (i == 7)
                {
                    defensa.posicionX = posXB2 + 40 + 40 + 40;
                    defensa.posicionY = posYB2 - 40 - 40 + 40 + 40;
                }

                imagenDefensa.Source = new BitmapImage(defensa.imagen);
                imagenDefensa.Height = 40;
                imagenDefensa.Width = 40;
                this.canvas.Children.Add(imagenDefensa);
                Canvas.SetLeft(imagenDefensa, defensa.posicionX);
                Canvas.SetTop(imagenDefensa, defensa.posicionY);
                ultimoIndice = listaImagenesDefensa.Count;
                imagenDefensa.Name = ultimoIndice.ToString();
                listaImagenesDefensa.Add(imagenDefensa);
                listaDefensas.Add(defensa);
            }

            //Bloques 3
            for (int i = 0; i < 8; i++)
            {
                Defensa defensa = new Defensa();
                Image imagenDefensa = new Image();
                if (i == 0)
                {
                    defensa.posicionX = posXB3;
                    defensa.posicionY = posYB3;
                }
                if (i == 1)
                {
                    defensa.posicionX = posXB3;
                    defensa.posicionY = posYB3 - 40;
                }
                if (i == 2)
                {
                    defensa.posicionX = posXB3 + 40;
                    defensa.posicionY = posYB3 - 40;
                }
                if (i == 3)
                {
                    defensa.posicionX = posXB3 + 40;
                    defensa.posicionY = posYB3 - 40 - 40;
                }
                if (i == 4)
                {
                    defensa.posicionX = posXB3 + 40 + 40;
                    defensa.posicionY = posYB3 - 40 - 40;
                }
                if (i == 5)
                {
                    defensa.posicionX = posXB3 + 40 + 40;
                    defensa.posicionY = posYB3 - 40 - 40 + 40;
                }
                if (i == 6)
                {
                    defensa.posicionX = posXB3 + 40 + 40 + 40;
                    defensa.posicionY = posYB3 - 40 - 40 + 40;
                }
                if (i == 7)
                {
                    defensa.posicionX = posXB3 + 40 + 40 + 40;
                    defensa.posicionY = posYB3 - 40 - 40 + 40 + 40;
                }

                imagenDefensa.Source = new BitmapImage(defensa.imagen);
                imagenDefensa.Height = 40;
                imagenDefensa.Width = 40;
                this.canvas.Children.Add(imagenDefensa);
                Canvas.SetLeft(imagenDefensa, defensa.posicionX);
                Canvas.SetTop(imagenDefensa, defensa.posicionY);
                ultimoIndice = listaImagenesDefensa.Count;
                imagenDefensa.Name = ultimoIndice.ToString();
                listaImagenesDefensa.Add(imagenDefensa);
                listaDefensas.Add(defensa);
            }

            //Bloques 4
            for (int i = 0; i < 8; i++)
            {
                Defensa defensa = new Defensa();
                Image imagenDefensa = new Image();
                if (i == 0)
                {
                    defensa.posicionX = posXB4;
                    defensa.posicionY = posYB4;
                }
                if (i == 1)
                {
                    defensa.posicionX = posXB4;
                    defensa.posicionY = posYB4 - 40;
                }
                if (i == 2)
                {
                    defensa.posicionX = posXB4 + 40;
                    defensa.posicionY = posYB4 - 40;
                }
                if (i == 3)
                {
                    defensa.posicionX = posXB4 + 40;
                    defensa.posicionY = posYB4 - 40 - 40;
                }
                if (i == 4)
                {
                    defensa.posicionX = posXB4 + 40 + 40;
                    defensa.posicionY = posYB4 - 40 - 40;
                }
                if (i == 5)
                {
                    defensa.posicionX = posXB4 + 40 + 40;
                    defensa.posicionY = posYB4 - 40 - 40 + 40;
                }
                if (i == 6)
                {
                    defensa.posicionX = posXB4 + 40 + 40 + 40;
                    defensa.posicionY = posYB4 - 40 - 40 + 40;
                }
                if (i == 7)
                {
                    defensa.posicionX = posXB4 + 40 + 40 + 40;
                    defensa.posicionY = posYB4 - 40 - 40 + 40 + 40;
                }

                imagenDefensa.Source = new BitmapImage(defensa.imagen);
                imagenDefensa.Height = 40;
                imagenDefensa.Width = 40;
                this.canvas.Children.Add(imagenDefensa);
                Canvas.SetLeft(imagenDefensa, defensa.posicionX);
                Canvas.SetTop(imagenDefensa, defensa.posicionY);
                ultimoIndice = listaImagenesDefensa.Count;
                imagenDefensa.Name = ultimoIndice.ToString();
                listaImagenesDefensa.Add(imagenDefensa);
                listaDefensas.Add(defensa);
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

        /// <summary>
        /// Aumenta el eje Y de una Nave enemiga
        /// </summary>
        /// <param name="posicionNave"></param>
        /// <param name="imagenNave"></param>
        /// <param name="naveEnemiga"></param>
        public void aumentaPosY(int posicionNave, Image imagenNave, NaveEnemiga naveEnemiga)//400 es la posicion Y de la defensa 
        {                                                                                    //mas alta
            Boolean haPerdido = false;
            double pos1 = Canvas.GetTop(imagenNave) + 45;
            //Si hay defensas
            if (cantidadDefensas>0)
            {
                
                //int pos2 = (listaDefensas.ElementAt(getPosicionDefensaMasAlta()).posY);
                double pos2=Canvas.GetTop(listaImagenesDefensa.ElementAt(getPosicionDefensaMasAlta()));
                //Comprobamos altura con la defensa mas alta
                if ( pos1 >= pos2 )
                {
                    haPerdido = true;
                }
            }
            else
            {

                //Sino comprobamos con la parte alta de nuestra nave
                if (pos1 >= 520)//534 es el eje Y de nuestra nave
                {
                    haPerdido = true;
                }
            }
            if (haPerdido)
            {
                //Mostrar mensaje perdedor y pausar timers
                pausar(1);
                mostrarGanadorPerdedor(mensajeDerrota);
            }
            listaEnemigos.ElementAt(posicionNave).posY = listaEnemigos.ElementAt(posicionNave).posY + 20;//20
            Canvas.SetTop(imagenNave, naveEnemiga.posY);            
        }

        /// <summary>
        /// Devuelve el índice de la defensa que esté en una posición más alta
        /// </summary>
        /// <returns></returns>
        public int getPosicionDefensaMasAlta()
        {
            int indiceDefensaAlta = 0;
            double alturaLeida = 0;
            double alturaAlta = 600;
            for (int i=0;i<listaImagenesDefensa.Count;i++)
            {
                //Si el canvas contiene la imagen de la defensa
                if (this.canvas.Children.Contains(listaImagenesDefensa.ElementAt(i)))
                {
                    alturaLeida = Canvas.GetTop(listaImagenesDefensa.ElementAt(i));
                    if (alturaLeida < alturaAlta)
                    {
                        alturaAlta = alturaLeida;
                        indiceDefensaAlta = i;
                    }
                }
            }
            return indiceDefensaAlta;
        }

        /// <summary>
        /// Evento que se da cuando se navega a la página Game
        /// </summary>
        /// <param name="e">Objeto </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (JugadorConDificultad)e.Parameter;
            JugadorConDificultad = parameters;
            this.vMGame.jugador = new Jugador();
            this.vMGame.jugador.ID = parameters.ID;
            this.vMGame.jugador.Nombre = parameters.Nombre;
            this.vMGame.jugador.Puntuacion = parameters.Puntuacion;
            this.vMGame.jugador.FechaJuego = parameters.FechaJuego;
            switch (parameters.dificultad)
            {
                case "Fácil":
                    dificultad = 1;
                    break;
                case "Normal":
                    dificultad = 2;
                    break;
                case "Dificil":
                    dificultad = 4;
                    break;
                default:
                    dificultad = 1;
                    break;
            }
            cargaNaves();
            timerDisparoEnemigo.Interval = new TimeSpan(0, 0, 0, 0, 2000 / dificultad);
            //timerDisparoEnemigo.Interval = new TimeSpan(0, 0, 0, 20, 0);
            timerDisparoEnemigo.Tick += tickDisparoEnemigo;
            timerDisparoEnemigo.Start();
            mediaPlayer = new MediaElement();
            this.grid.Children.Add(mediaPlayer);
            if (parameters.isMuted == true)
            {
                mediaPlayer.IsMuted = true;
            }
            else
            {
                mediaPlayer.Volume = parameters.volumen;
            }
        }
        /// <summary>
        /// Reproduce un sonido dependiendo el nombre de audio pasado, si hay uno sonando, se para ese y se reproduce otro
        /// </summary>
        /// <param name="nombreAudio"></param>
        /// <returns></returns>
        private async Task PlayMusicImpact(String nombreAudio) //se bloquea la ui cuando hay muchos disparos!!!!!!!!!!!!!!!!!!!!!!
        {
            /* mediaPlayer = new MediaPlayerElement();
             mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Music/" + nombreAudio));
             mediaPlayer.AutoPlay = true;*/

            if (mediaPlayer.CurrentState == MediaElementState.Playing)
            {
                mediaPlayer.Stop();
                Uri manifestUri = new Uri("ms-appx:///Assets/Music/" + nombreAudio);
                mediaPlayer.Source = manifestUri;
                mediaPlayer.Play();
            }
            else
            {
                Uri manifestUri = new Uri("ms-appx:///Assets/Music/" + nombreAudio);
                mediaPlayer.Source = manifestUri;
                mediaPlayer.Play();
            }

        }
        /// <summary>
        /// Reproduce un sonido dependiendo el audio pasado
        /// </summary>
        /// <param name="nombreAudio"></param>
        private async void reproducirAudioAsync(String nombreAudio)
        {
            await PlayMusicImpact(nombreAudio);
        }

        /// <summary>
        /// Evento que se da al pulsar el botón back del pausa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnBackMenuClick(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        /// <summary>
        /// Evento que se da al pulsar el botón Resume del pausa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnSeguirclick(object sender, PointerRoutedEventArgs e)
        {
            volverdepausa();
        }
        /// <summary>
        /// Evento que se da al pulsar el botón Restart del pausa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnReiniciarclick(object sender, PointerRoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(Game), JugadorConDificultad);

        }


        /// <summary>
        /// Método que se encarga de quitar la pausa y continuar la partida
        /// </summary>
        public void volverdepausa()
        {
            noDispares = false;
            pausa = false;
            timer.Start();
            Window.Current.Content.KeyDown += this.vMGame.Grid_KeyDown;
            this.vMGame.opacidadPausa = 0;
        }
        /// <summary>
        /// Método que se encarga de pausar el juego y mostrar la imagen de pausa con sus botones
        /// </summary>
        /// <param name="motivo"></param>
        public void pausar(int motivo)//Si esl motivo es 0 mostramos la imagen de pausa
        {
            noDispares = true;
            pausa = true;
            timer.Stop();
            Window.Current.Content.KeyDown -= this.vMGame.Grid_KeyDown;
            if (motivo == 0)
            {
                this.vMGame.opacidadPausa = 1;
            }            
        }



    }
}
