using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestAwait
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        // Define the cancellation token.
        CancellationTokenSource source;
        PauseTokenSource SourcePausa;



        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Test1_Click(object sender, RoutedEventArgs e)
        {
            Texto.Text = "Llamada async basica";
            Progreso.Visibility = Visibility.Visible;
            bool b = await Prueba1();
            Progreso.Visibility = Visibility.Hidden;
            Texto.Text = "Fin Llamada async basica";
        }

        private async Task<bool> Prueba1()
        {
            await Task.Run(() => Thread.Sleep(10000));
            return true;
        }

        private async Task<bool> ProcesoMuyLargo(PauseToken EstaPausado)
        {
            for (int i = 0; i < 10; i++)
            {
                await EstaPausado.WaitWhilePausedAsync();
                Thread.Sleep(1000);
                if (source.IsCancellationRequested)
                    return false;
            }
            return true;
        }

        private async void Test2_Click(object sender, RoutedEventArgs e)
        {
            source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            SourcePausa = new PauseTokenSource();
            PauseToken tokenpausa = SourcePausa.Token;
            Test1.IsEnabled = false;
            Test2.IsEnabled = false;
            Pausa.IsEnabled = true;
            Continuar.IsEnabled = true;
            Cancelar.IsEnabled = true;
            Texto.Text = "Se esta ejecutando un proceso muy largo";
            Progreso.Visibility = Visibility.Visible;
            bool b = await Task.Run(() => ProcesoMuyLargo(tokenpausa), token);
            if (b)
            {
                Texto.Text = "Se termino de ejecutar un proceso muy largo normalmente";
            }
            else
            {
                Texto.Text = "Se termino de ejecutar un proceso muy largo cancelado";
            }
            Test1.IsEnabled = true;
            Test2.IsEnabled = true;
            Pausa.IsEnabled = false;
            Continuar.IsEnabled = false;
            Cancelar.IsEnabled = false;
            Progreso.Visibility = Visibility.Hidden;


        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            source.Cancel();
        }

        private void Pausa_Click(object sender, RoutedEventArgs e)
        {
            Texto.Text = "Proceso en pausa";
            SourcePausa.IsPaused = true;
        }

        private void Continuar_Click(object sender, RoutedEventArgs e)
        {
            Texto.Text = "Proceso continuando";
            SourcePausa.IsPaused = false;

        }
    }
}
