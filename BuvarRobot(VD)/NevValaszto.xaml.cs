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
using System.IO;

namespace BuvarRobot_VD_
{
    /// <summary>
    /// Interaction logic for NevValaszto.xaml
    /// </summary>
    public partial class NevValaszto : Window
    {
        public NevValaszto()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnRendben_MouseEnter(object sender, MouseEventArgs e)
        {
            imgrendben.Source = new BitmapImage(new Uri("/Images/rendbenhover.png", UriKind.Relative));
        }

        private void btnRendben_MouseLeave(object sender, MouseEventArgs e)
        {
            imgrendben.Source = new BitmapImage(new Uri("/Images/rendben.png", UriKind.Relative));
        }
        private void imgkilepes_MouseEnter(object sender, MouseEventArgs e)
        {
            imgigazikilepes.Source = new BitmapImage(new Uri("/Images/kilepeshover.png", UriKind.Relative));
        }
        private void imgkilepes_MouseLeave(object sender, MouseEventArgs e)
        {
            imgigazikilepes.Source = new BitmapImage(new Uri("/Images/kilepes.png", UriKind.Relative));
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
        private void btnRendben_Click(object sender, RoutedEventArgs e)
        {
            string nev = txtNev.Text;
            File.WriteAllText("../../../name.txt", nev);
            PalyaValaszto palyavalaszt = new PalyaValaszto();
            palyavalaszt.Show();
            this.Close();
        }
    }
}
