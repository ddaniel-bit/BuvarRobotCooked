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
using System.Windows.Shapes;

namespace BuvarRobot_VD_
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        double utolsohangero = 0;
   
        public Settings()
        {
            InitializeComponent();
            sldVolume.Value = AppMusicPlayer.sldvolumevalue;
            changeAudioimg();
        }
        private void cbMusicChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dictionary<ComboBoxItem, string> musicDictionary = new Dictionary<ComboBoxItem, string>()
            {
            { Item1, "../../../Audios/mainmusic1.mp3" },
            { Item2, "../../../Audios/mainmusic2.mp3" },
            { Item3, "../../../Audios/mainmusic3.mp3" },
            { Item4, "../../../Audios/mainmusic4.mp3" },
            { Item5, "../../../Audios/mainmusic5.mp3" }
            };

            if (musicDictionary.ContainsKey(cbMusicChoose.SelectedItem as ComboBoxItem))
            {
                AppMusicPlayer.choosenmusic = musicDictionary[cbMusicChoose.SelectedItem as ComboBoxItem];
                AppMusicPlayer.StartPlayback();
            }
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
                    sldVolume.Value = 0.05;
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
        private void btnTalcaraTesz_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            AppMusicPlayer.sldvolumevalue = sldVolume.Value;
            MainWindow openWindow = new MainWindow();
            openWindow.Show();
            this.Close();
        }
        private void imgkilepes_MouseEnter(object sender, MouseEventArgs e)
        {
            imgkilepes.Source = new BitmapImage(new Uri("/Images/menteseskilepeshover.png", UriKind.Relative));
        }
        private void imgkilepes_MouseLeave(object sender, MouseEventArgs e)
        {
            imgkilepes.Source = new BitmapImage(new Uri("/Images/menteseskilepes.png", UriKind.Relative));
        }

 
       
    }
}
