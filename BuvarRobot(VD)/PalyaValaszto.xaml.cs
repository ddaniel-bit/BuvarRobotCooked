using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for PalyaValaszto.xaml
    /// </summary>
    public partial class PalyaValaszto : Window
    {
        string leveldata = "";
        public PalyaValaszto()
        {
            InitializeComponent();
            StreamReader sr = new StreamReader("../../../level_data.txt");
            leveldata = sr.ReadLine();
            sr.Close();
            //MessageBox.Show($"level_data.txt erteke: {leveldata}\n" +
            //    $"new: beadja a storyt" +
            //    $"1: az első pályát adja be story nelkul");
            
            
        }

        private void level1Button_Click(object sender, RoutedEventArgs e)
        {
            if (leveldata == "new")
            {
                StoryView1 story = new StoryView1();
                story.Show();
                this.Close();
            }
            else if (leveldata == "1" || leveldata == "2" || leveldata == "3")
            {
                Level1View level1 = new Level1View();
                level1.Show();
                this.Close();
            }
            
        }

        private void level2Button_Click(object sender, RoutedEventArgs e)
        {
            if (leveldata == "2" || leveldata == "3")
            {
                Level2View level2 = new Level2View();
                level2.Show();
                this.Close();
            }
        }

        private void level3Button_Click(object sender, RoutedEventArgs e)
        {
            if (leveldata == "3")
            {
                Level3View level3 = new Level3View();
                level3.Show();
                this.Close();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }




        private void imgpalya1_MouseEnter(object sender, MouseEventArgs e)
        {
            imgpalya1.Source = new BitmapImage(new Uri("/Images/palya1hover.png", UriKind.Relative));
        }
        private void imgpalya1_MouseLeave(object sender, MouseEventArgs e)
        {
            imgpalya1.Source = new BitmapImage(new Uri("/Images/palya1.png", UriKind.Relative));
        }


        private void imgpalya2_MouseEnter(object sender, MouseEventArgs e)
        {
            imgpalya2.Source = new BitmapImage(new Uri("/Images/palya2hover.png", UriKind.Relative));
        }
        private void imgpalya2_MouseLeave(object sender, MouseEventArgs e)
        {
            imgpalya2.Source = new BitmapImage(new Uri("/Images/palya2.png", UriKind.Relative));
        }


        private void imgpalya3_MouseEnter(object sender, MouseEventArgs e)
        {
            imgpalya3.Source = new BitmapImage(new Uri("/Images/palya3hover.png", UriKind.Relative));
        }
        private void imgpalya3_MouseLeave(object sender, MouseEventArgs e)
        {
            imgpalya3.Source = new BitmapImage(new Uri("/Images/palya3.png", UriKind.Relative));
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
