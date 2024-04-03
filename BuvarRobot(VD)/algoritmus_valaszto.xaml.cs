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
using Microsoft.Win32;

namespace BuvarRobot_VD_
{
    /// <summary>
    /// Interaction logic for algoritmus_valaszto.xaml
    /// </summary>
    public partial class algoritmus_valaszto : Window
    {
        string map_path = "../../../gyongyok.txt";
        public algoritmus_valaszto()
        {
            InitializeComponent();
            spRandom.IsEnabled = false;
            sldX.IsEnabled = false;
            sldY.IsEnabled = false;
            sldZ.IsEnabled = false;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(TimeTextBox.Text, out double ido) && double.TryParse(SpeedTextBox.Text, out double sebesseg))
            {
                string algoritmus = "bruteforce";
                bool random = false;
                if (CbRandom.IsChecked == true)
                {
                    random = true;
                }
                else if (CbImport.IsChecked == true)
                {
                    random = false;
                }
                // Második ablak létrehozása és megjelenítése
                _3d_Submarine_View secondWindow = new _3d_Submarine_View(ido, sebesseg, algoritmus, random, Convert.ToInt32(lbGyongyok.Content), map_path, Convert.ToInt32(sldX.Value), Convert.ToInt32(sldY.Value), Convert.ToInt32(sldZ.Value));
                secondWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Érvénytelen érték!");
            }
        }


        private void CbRandom_Click(object sender, RoutedEventArgs e)
        {
            spRandom.IsEnabled = true;
            sldX.IsEnabled = true;
            sldY.IsEnabled = true;
            sldZ.IsEnabled = true;
        }

        private void sldGyongyok_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lbGyongyok != null)
            {
                lbGyongyok.Content = Convert.ToString(sldGyongyok.Value);
            }

        }

        private void imgfuttatas_MouseEnter(object sender, MouseEventArgs e)
        {
            imgkilepes.Source = new BitmapImage(new Uri("/Images/futtatashover.png", UriKind.Relative));
        }
        private void imgfuttatas_MouseLeave(object sender, MouseEventArgs e)
        {
            imgkilepes.Source = new BitmapImage(new Uri("/Images/futtatas.png", UriKind.Relative));
        }

        private void BruteforceAlgorithmRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CbImport_Click(object sender, RoutedEventArgs e)
        {
            spRandom.IsEnabled = false;
            sldX.IsEnabled = false;
            sldY.IsEnabled = false;
            sldZ.IsEnabled = false;
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (spRandom.IsEnabled == false)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; // Választható fájltípusok
                openFileDialog.InitialDirectory = @"C:\"; // Alapértelmezett könyvtár

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    map_path = openFileDialog.FileName;
                    lblPath.Content = map_path;
                }
            }
            
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
        private void imgfajlmegnyitasa_MouseEnter(object sender, MouseEventArgs e)
        {
            imgfajlmegnyitasa.Source = new BitmapImage(new Uri("/Images/fajlmegnyitasahover.png", UriKind.Relative));
        }
        private void imgfajlmegnyitasa_MouseLeave(object sender, MouseEventArgs e)
        {
            imgfajlmegnyitasa.Source = new BitmapImage(new Uri("/Images/fajlmegnyitasa.png", UriKind.Relative));
        }

        private void sldX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lbX != null)
            {
                lbX.Content = Convert.ToString(sldX.Value);
            }
        }

        private void sldY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lbY != null)
            {
                lbY.Content = Convert.ToString(sldY.Value);
            }
        }

        private void sldZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lbZ != null)
            {
                lbZ.Content = Convert.ToString(sldZ.Value);
            }
        }

    }
}
