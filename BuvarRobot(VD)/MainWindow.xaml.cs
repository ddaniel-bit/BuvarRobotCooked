using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO;

namespace BuvarRobot_VD_
{
    public class AppMusicPlayer
    {
        public static System.Windows.Media.MediaPlayer musicplayer = new System.Windows.Media.MediaPlayer();
        public static double sldvolumevalue=0.2;
        public static bool musicStarted = false;
        public static string choosenmusic = "../../../Audios/mainmusic1.mp3";



        public static void StartPlayback()
        {
            musicplayer.Open(new Uri(choosenmusic, UriKind.RelativeOrAbsolute));
            musicplayer.MediaEnded += Media_Ended;
            musicplayer.Play();
        }

        public static void StopPlayback()
        {
            musicplayer.Stop();
            musicplayer.Close();
            musicplayer = null;
        }

        private static void Media_Ended(object sender, EventArgs e)
        {
            musicplayer.Open(new Uri(choosenmusic, UriKind.Relative));
            musicplayer.Play();          
        }
    }







    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double utolsohangero= 0;

        public MainWindow()
        {
            InitializeComponent();  
            sldVolume.Value = AppMusicPlayer.sldvolumevalue;
            if (!AppMusicPlayer.musicStarted)
            {
                AppMusicPlayer.StartPlayback();
                AppMusicPlayer.musicStarted = true;
            }
            changeAudioimg();
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            AppMusicPlayer.StopPlayback();
        }



        private void btnaudio_Click(object sender, RoutedEventArgs e)
        {
            if (AppMusicPlayer.musicplayer.Volume == 0)
            {

                AppMusicPlayer.musicplayer.Volume = utolsohangero; // Visszaállítja a hangot
                sldVolume.Value = utolsohangero;

                if (AppMusicPlayer.musicplayer.Volume > 0)
                {
                    changeAudioimg();
                }
                else
                {
                    sldVolume.Value = 0.5;
                }


                sldVolume.Visibility = Visibility.Visible;
            }
            else
            {
                AppMusicPlayer.musicplayer.Volume = 0;// Elnémítja a hangot
                sldVolume.Value = 0;
                imgaudio.Source = new BitmapImage(new Uri("/Images/audiooff.png", UriKind.Relative));
                sldVolume.Visibility = Visibility.Hidden;
            }

        }

        private void audio_MouseEnter(object sender, MouseEventArgs e)
        {
                sldVolume.Visibility = Visibility.Visible;
        }
        private void audio_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!sldVolume.IsMouseOver)
            {
                sldVolume.Visibility = Visibility.Hidden;
            }
        }


        private void sldVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AppMusicPlayer.musicplayer.Volume = sldVolume.Value;
            if (sldVolume.Value != 0)
            {
                utolsohangero = sldVolume.Value;
            }


            if (imgaudio != null)
            {
                changeAudioimg();
            }

        }

        public void changeAudioimg()
        {
            if (sldVolume.Value > 0.66)
            {
                imgaudio.Source = new BitmapImage(new Uri("/Images/audioon.png", UriKind.Relative));
            }
            else if (sldVolume.Value > 0.33)
            {
                imgaudio.Source = new BitmapImage(new Uri("/Images/audioon2.png", UriKind.Relative));
            }
            else if (sldVolume.Value > 0)
            {
                imgaudio.Source = new BitmapImage(new Uri("/Images/audioon1.png", UriKind.Relative));
            }
            else
            {
                imgaudio.Source = new BitmapImage(new Uri("/Images/audiooff.png", UriKind.Relative));
            }
        }








        private void sldVolume_MouseLeave(object sender, MouseEventArgs e)
        {
            sldVolume.Visibility = Visibility.Hidden;
        }





        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnAlgorimusok_Click(object sender, RoutedEventArgs e)
        {
            AppMusicPlayer.sldvolumevalue = sldVolume.Value;
            algoritmus_valaszto openWindow = new algoritmus_valaszto();
            openWindow.Show();
            this.Close();
        }
        private void btnStorymod_Click(object sender, RoutedEventArgs e)
        {
            AppMusicPlayer.sldvolumevalue = sldVolume.Value;
            string filePath = "../../../name.txt";
            if (File.ReadAllLines(filePath).Length == 0)
            {
                NevValaszto openWindow1 = new NevValaszto();
                openWindow1.ShowDialog();
            }
            else
            {
                PalyaValaszto openWindow = new PalyaValaszto();
                openWindow.Show();
            }
            this.Close();
        }

        private void btnTalcaraTesz_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }




        private void imgkilepes_MouseEnter(object sender, MouseEventArgs e)
        {
            imgkilepes.Source = new BitmapImage(new Uri("/Images/kilepeshover.png", UriKind.Relative));
        }
        private void imgkilepes_MouseLeave(object sender, MouseEventArgs e)
        {
            imgkilepes.Source = new BitmapImage(new Uri("/Images/kilepes.png", UriKind.Relative));
        }


        private void imgbeallitasok_MouseEnter(object sender, MouseEventArgs e)
        {
            imgbeallitasok.Source = new BitmapImage(new Uri("/Images/beallitasokhover.png", UriKind.Relative));
        }
        private void imgbeallitasok_MouseLeave(object sender, MouseEventArgs e)
        {
            imgbeallitasok.Source = new BitmapImage(new Uri("/Images/beallitasok.png", UriKind.Relative));
        }


        private void imgstorymod_MouseEnter(object sender, MouseEventArgs e)
        {
            imgstorymod.Source = new BitmapImage(new Uri("/Images/storymodhover.png", UriKind.Relative));
        }
        private void imgstorymod_MouseLeave(object sender, MouseEventArgs e)
        {
            imgstorymod.Source = new BitmapImage(new Uri("/Images/storymod.png", UriKind.Relative));
        }


        private void imgalgoritmusok_MouseEnter(object sender, MouseEventArgs e)
        {
            imgalgoritmusok.Source = new BitmapImage(new Uri("/Images/algoritmusokhover.png", UriKind.Relative));
        }
        private void imgalgoritmusok_MouseLeave(object sender, MouseEventArgs e)
        {
            imgalgoritmusok.Source = new BitmapImage(new Uri("/Images/algoritmusok.png", UriKind.Relative));
        }

        private void beallitasok_Click(object sender, RoutedEventArgs e)
        {
            AppMusicPlayer.sldvolumevalue = sldVolume.Value;
            Settings openWindow = new Settings();
            openWindow.Show();
            this.Close();
        }
    }
}
