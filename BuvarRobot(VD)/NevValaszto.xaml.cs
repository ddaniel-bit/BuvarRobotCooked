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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string nev = txtNev.Text;
            File.WriteAllText("../../../name.txt", nev);
            this.Close();
        }
    }
}
