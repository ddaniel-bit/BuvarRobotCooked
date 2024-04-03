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
    /// Interaction logic for EndingView.xaml
    /// </summary>
    public partial class EndingView : Window
    {
        List<Parbeszed> parbeszedek = new List<Parbeszed>();
        string name = "default_name";
        int storyindex = 0;
        public EndingView()
        {
            InitializeComponent();
            StreamReader sr = new StreamReader("../../../name.txt");
            name = sr.ReadLine();
            sr.Close();
            kiirAsync("Parancsnok", $"Gratulálok {name}! Köszönjük hősies fáradozását.", "Images/tisztelges.jpg");
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", "Amit a hazánkért tett, példaértékű.", "Images/tisztelges.jpg"));
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", "Az ön segítségével sikerült Nagy Britannia energia és gazdasági válságát megoldani.", "Images/tisztelges.jpg"));
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", "Példás fegyelméért, kitartásáért és szaktudásáért fogadja szeretettel a \"HAZA MEGMENTŐJE\" kitüntetést.", "Images/kituntet.jpg"));

        }
        public async Task kiirAsync(string title, string szoveg, string kep)
        {
            txtDialog.Text = $"{title}: ";
            btnNext.Visibility = Visibility.Hidden;
            try
            {
                string newImagePath = kep;

                BitmapImage bitmapImage = new BitmapImage(new Uri(newImagePath, UriKind.Relative));
                imgKep.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt a kép cseréje közben: " + ex.Message);
            }
            foreach (char c in szoveg)
            {


                txtDialog.Text += c;
                await Task.Delay(25);
            }
            btnNext.Visibility = Visibility.Visible;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (parbeszedek.Count > storyindex)
            {
                if (parbeszedek[storyindex].Jatekos)
                {
                    kiirAsync(name, parbeszedek[storyindex].Szoveg, parbeszedek[storyindex].Kep);
                }
                else
                {
                    kiirAsync(parbeszedek[storyindex].Title, parbeszedek[storyindex].Szoveg, parbeszedek[storyindex].Kep);
                }
                storyindex++;

            }
            else
            {

                MainWindow openWindow = new MainWindow();
                openWindow.Show();
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
    }
}
