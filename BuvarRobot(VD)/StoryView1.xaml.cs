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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BuvarRobot_VD_
{
    /// <summary>
    /// Interaction logic for StoryView1.xaml
    /// </summary>
    public partial class StoryView1 : Window
    {
        List<Parbeszed> parbeszedek = new List<Parbeszed>();
        string name = "default_name";
        int storyindex = 0;
        public StoryView1()
        {
            InitializeComponent();
            StreamReader sr = new StreamReader("../../../name.txt");
            name = sr.ReadLine();
            sr.Close();
            kiirAsync("(1917 Nagy-Britannia) Parancsnok", $"Ahh {name}, mindenütt magát kerestem, nagyon fontos feladattal kell megbíznom önt.", "Images/kapitany.jpg");
            parbeszedek.Add(new Parbeszed(true,"","Mégis miféle feladattal?", "Images/jatekos.jpg"));
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", "A németek korlátlan tengeralattjáró hadjáratot hirdettek, lebombáznak minden ellenséges hajót a térségben.", "Images/kapitany.jpg"));
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", "Az országunknak alig maradt urán-tartaléka amivel fenntarthatja a hadsereg és a lakosság áramellátását.", "Images/kapitany.jpg"));
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", "Franciaország küldött nekünk egy hajót 3 hónapra elegendű dúsított urán gyöngyökkel.", "Images/kapitany.jpg"));
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", "A németek lebombázták a hajót mielőtt ideért volna, a legújabb tengeralattjárójukkal, az U–86 -al.", "Images/terngeralattjaro.jpg"));
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", "A maga dolga hogy az újonnan tervezett tengeralattjáró robotunkat irányítsa.", "Images/kapitany.jpg"));
            //mielőtt a németek újra bombáznak
            parbeszedek.Add(new Parbeszed(true, "", "Hogy az enyém? Dehét még csak most kerültem a legénységbe.", "Images/jatekos.jpg"));
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", "Valóban kicsit mélyvíz, de bízok magában.", "Images/kapitany.jpg"));
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", "Sitenie kell, a németek folyamatosan újrabombáznak ami egy óra fog jelezni.", "Images/kapitany.jpg"));
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", $"Magának a lehető legtöbb gyöngyöt fel kell szednie amig az óra le nem telik, sok sikert {name}!", "Images/kapitany.jpg"));

        }
        public async Task kiirAsync(string title, string szoveg, string kep)
        {
            txtDialog.Text = $"{title}: ";
            btnNext.Visibility = Visibility.Hidden ;
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
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
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
                
                Level1View openWindow = new Level1View();
                openWindow.Show();
                this.Close();
            }
        }
    }
}
