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
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using static HelixToolkit.Wpf.Viewport3DHelper;
using System.Runtime.ConstrainedExecution;
using System.Timers;

namespace BuvarRobot_VD_
{
    /// <summary>
    /// Interaction logic for Level1View.xaml
    /// </summary>
    public partial class Level1View : Window
    {
        List<Gyongy> gyongyoklista = new List<Gyongy>();
        int szorzo = 3;
        List<Parbeszed> parbeszedek = new List<Parbeszed>();
        string name = "default_name";
        int storyindex = 0;
        private Timer timer;
        int remainingSeconds;
        string filePath = "../../../level_data.txt";
        public Level1View()
        {
            InitializeComponent();
            
            StreamReader sr = new StreamReader(filePath);
            if (sr.ReadLine() != "2" && sr.ReadLine() != "3")
            {
                sr.Close();
                // Fájl tartalmának beolvasása
                string content = File.ReadAllText(filePath);

                // Tartalom módosítása (például cseréljük le a teljes tartalmat)
                string modifiedContent = "1";

                // Módosított tartalom kiírása a fájlba
                File.WriteAllText(filePath, modifiedContent);
            }
            


            
            gyongyoklista = File.ReadAllLines("../../../level1.txt").Skip(1).Select(x => new Gyongy(x)).ToList();
            List<EllipsoidVisual3D> gyongyok3d = new List<EllipsoidVisual3D>();
            gyongyok3d.Add(new EllipsoidVisual3D());
            foreach (var gyongy in gyongyoklista)
            {
                EllipsoidVisual3D gyongy3D = new EllipsoidVisual3D();
                gyongy3D.RadiusX = gyongy.E;
                gyongy3D.RadiusY = gyongy.E;
                gyongy3D.RadiusZ = gyongy.E;
                gyongy3D.Center = new Point3D(gyongy.Y * szorzo, gyongy.X * szorzo, gyongy.Z * szorzo);
                gyongy3D.Fill = new SolidColorBrush(Color.FromRgb(245, 203, 66));
                gyongyok3d.Add(gyongy3D);
                ter.Children.Add(gyongy3D);
            }
//            MessageBox.Show(Convert.ToString(gyongyok3d.Count-1));


            //megkeressuk a 0,0,0-tol (tehat robottol) a legmesszebb eso gyongyot es kiszamitjuk a kozepet a gyongyoknek
            double legnagyobbX = 0;
            foreach (var item in gyongyoklista)
            {
                if (item.X > legnagyobbX)
                {
                    legnagyobbX = item.X;
                }
            }
            double legnagyobbY = 0;
            foreach (var item in gyongyoklista)
            {
                if (item.Y > legnagyobbY)
                {
                    legnagyobbY = item.Y;
                }
            }
            double legnagyobbZ = 0;
            foreach (var item in gyongyoklista)
            {
                if (item.Z > legnagyobbZ)
                {
                    legnagyobbZ = item.Z;
                }
            }
            double legnagyobbE = 0;
            foreach (var item in gyongyoklista)
            {
                if (item.E > legnagyobbE)
                {
                    legnagyobbE = item.E;
                }
            }

            //vonalakból rajzolok egy akvariumot
            double width = legnagyobbY * szorzo;
            double height = legnagyobbX * szorzo;
            double depth = legnagyobbZ * szorzo;

            // Akvárium bal alsó hátsó sarka
            Point3D bottomBackLeft = new Point3D(0 - legnagyobbE, 0 - legnagyobbE, 0 - legnagyobbE);
            // Akvárium jobb alsó hátsó sarka
            Point3D bottomBackRight = new Point3D(width + legnagyobbE, 0 - legnagyobbE, 0 - legnagyobbE);
            // Akvárium bal felső hátsó sarka
            Point3D topBackLeft = new Point3D(0 - legnagyobbE, height + legnagyobbE, 0 - legnagyobbE);
            // Akvárium jobb felső hátsó sarka
            Point3D topBackRight = new Point3D(width + legnagyobbE, height + legnagyobbE, 0 - legnagyobbE);
            // Akvárium bal alsó elülső sarka
            Point3D bottomFrontLeft = new Point3D(0 - legnagyobbE, 0 - legnagyobbE, depth + legnagyobbE);
            // Akvárium jobb alsó elülső sarka
            Point3D bottomFrontRight = new Point3D(width + legnagyobbE, 0 - legnagyobbE, depth + legnagyobbE);
            // Akvárium bal felső elülső sarka
            Point3D topFrontLeft = new Point3D(0 - legnagyobbE, height + legnagyobbE, depth + legnagyobbE);
            // Akvárium jobb felső elülső sarka
            Point3D topFrontRight = new Point3D(width + legnagyobbE, height + legnagyobbE, depth + legnagyobbE);

            // Vonalak rajzolása
            DrawLine(bottomBackLeft, bottomBackRight);
            DrawLine(bottomBackRight, bottomFrontRight);
            DrawLine(bottomFrontRight, bottomFrontLeft);
            DrawLine(bottomFrontLeft, bottomBackLeft);

            DrawLine(topBackLeft, topBackRight);
            DrawLine(topBackRight, topFrontRight);
            DrawLine(topFrontRight, topFrontLeft);
            DrawLine(topFrontLeft, topBackLeft);

            DrawLine(bottomBackLeft, topBackLeft);
            DrawLine(bottomBackRight, topBackRight);
            DrawLine(bottomFrontLeft, topFrontLeft);
            DrawLine(bottomFrontRight, topFrontRight);

            // Vonal rajzolása
            void DrawLine(Point3D startPoint, Point3D endPoint)
            {
                var line = new LinesVisual3D();
                line.Points.Add(startPoint);
                line.Points.Add(endPoint);
                line.Color = Colors.Black;
                line.Thickness = 3;
                ter.Children.Add(line);
            }

            //Betölti a tengeralattjaro obj-t érdemes lenne a Point3D robotPozicio = new Point3D(0, 0, 0); helyere valahogy betenni
            var importer = new ModelImporter();
            var model = importer.Load("../../../Submarine.obj");

            // Méretezd kisebbre a modellt (pl.: 0.5-es arányban)
            var scaleTransform = new ScaleTransform3D(0.035, 0.035, 0.035);

            // Elforgatás 90 fokkal az Y tengely mentén
            var rotateTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90));

            // Ha a modellt a (0, 0, 0) koordinátára akarod helyezni
            var modelVisual = new ModelVisual3D
            {
                Content = model,
                Transform = new Transform3DGroup()
                {
                    Children = new Transform3DCollection() { scaleTransform, rotateTransform }
                }
            };
            ter.Children.Add(modelVisual);

            bool elso = true;
            var submarineVisual = new ModelVisual3D { };
            // Viewport3D eseményfigyelő hozzáadása
            ter.MouseLeftButtonDown += (sender, e) =>
            {

                // Kattintási pont meghatározása a Viewport3D-ben
                Point mousePos = e.GetPosition(ter);

                // HitTest futtatása a kattintási pontra
                var hitResult = VisualTreeHelper.HitTest(ter, mousePos);

                // A hitResult vizsgálata, hogy a kattintás mely objektumhoz tartozik
                if (hitResult.VisualHit is EllipsoidVisual3D gyongy3D)
                {

                    if (elso)
                    {
                        ter.Children.Remove(modelVisual);
                        elso = false;
                    }
                    else
                    {
                        ter.Children.Remove(submarineVisual);
                    }

                    // Gyöngy eltávolítása
                    ter.Children.Remove(gyongy3D);
                    gyongyok3d.Remove(gyongy3D);
                    tbPontok.Text = Convert.ToString(Convert.ToInt32(tbPontok.Text) + gyongy3D.RadiusX);

                    // Tengeralattjáró modell betöltése a gyöngy helyére
                    var importer = new ModelImporter();
                    var model = importer.Load("../../../Submarine.obj");

                    // Méretezés
                    var scaleTransform = new ScaleTransform3D(0.035, 0.035, 0.035);

                    // Elforgatás
                    var rotateTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90));

                    // Eltolás
                    var translateTransform = new TranslateTransform3D(gyongy3D.Center.X, gyongy3D.Center.Y, gyongy3D.Center.Z);

                    // A tengeralattjáró modelljének létrehozása
                    submarineVisual = new ModelVisual3D
                    {
                        Content = model,
                        Transform = new Transform3DGroup
                        {
                            Children = new Transform3DCollection { scaleTransform, rotateTransform, translateTransform }
                        }
                    };

                    // A modelVisual hozzáadása a ter-hez
                    ter.Children.Add(submarineVisual);
                    if (gyongyok3d.Count == 1)
                    {
                        dialogGrid.Visibility = Visibility.Visible;
                        kiirAsync("Parancsnok", $"Gratulálok {name}! Sikerült összegyűjtened az összes gyöngyöt.", "");
                        parbeszedek.Add(new Parbeszed(false, "Parancsnok", "Ez könnyű volt, de most akkor jöjjön egy nehezebb.", ""));
                        timer.Stop();
                    }
                }
            };


            //STORY-ÉRT FELELŐS RÉSZ

            StreamReader sr2 = new StreamReader("../../../name.txt");
            name = sr2.ReadLine();
            sr2.Close();
            kiirAsync("Parancsnok", $"Már össze is állítottuk magának a tengeralattjáró robotunk kezelőfelületét", "");
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", "A bal felső sarokban vannak a fontosabb dolgok, az óra és a pontszámláló.", ""));
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", "Minden gyöngy más méretű, a nagyobb gyöngyök többet érnek mint a kisebbek.", ""));
            parbeszedek.Add(new Parbeszed(false, "Parancsnok", "Úgy tudja összeszedni a gyöngyöket ha rájuk kattint, a robot automatikusan odanavigál. Sok sikert!", ""));
        }
        public async Task kiirAsync(string title, string szoveg, string kep)
        {
            txtDialog.Text = $"{title}: ";
            btnNext.Visibility = Visibility.Hidden;
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
                dialogGrid.Visibility = Visibility.Collapsed;
                if (storyindex == 4)
                {
                    string content = File.ReadAllText(filePath);

                    // Tartalom módosítása (például cseréljük le a teljes tartalmat)
                    string modifiedContent = "2";

                    // Módosított tartalom kiírása a fájlba
                    File.WriteAllText(filePath, modifiedContent);
                    PalyaValaszto openWindow = new PalyaValaszto();
                    openWindow.Show();
                    this.Close();
                }
                else
                {
                    ter.IsEnabled = true;
                    timer = new Timer(1000); // 1 másodperc = 1000 milliszekundum
                    timer.Elapsed += TimerElapsed;
                    timer.AutoReset = true;
                    remainingSeconds = 30; 
                    UpdateTimeLabel();

                    // Időzítő indítása
                    timer.Start();
                }
            }
        }
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (remainingSeconds > 0)
            {
                remainingSeconds--;
                Dispatcher.Invoke(UpdateTimeLabel);
            }
            else
            {
                timer.Stop();
                Dispatcher.Invoke(() =>
                {
                    loseGrid.Visibility = Visibility.Visible;
                    ter.IsEnabled = false;
                });
            }
        }
        private void UpdateTimeLabel()
        {
            tbTime.Text = $"{remainingSeconds} s";
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            PalyaValaszto openWindow = new PalyaValaszto();
            openWindow.Show();
            this.Close();
        }
    }
}
