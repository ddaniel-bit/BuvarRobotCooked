using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace BuvarRobot_VD_
{
    public partial class _3d_Submarine_View : Window
    {
        List<Gyongy> gyongyoklista = new List<Gyongy>();
        int szorzo = 3;
        double ido = 60;
        double sebesseg = 1;
        string algoritmus = "moho";


        List<Gyongy> utvonalanim = new List<Gyongy>();
        ModelVisual3D robotmodel = new ModelVisual3D();

        public _3d_Submarine_View(double ido, double sebesseg, string algoritmus, bool random, int gyongyokszama, string map_path, int hatarX, int hatarY, int hatarZ)
        {
            this.ido = ido;
            this.sebesseg = sebesseg;
            this.algoritmus = algoritmus;
            InitializeComponent();
            if (random)
            {
                Random vel = new Random();

                // Véletlenszerűen generált gyöngyök hozzáadása
                for (int i = 0; i < gyongyokszama; i++) // Példaként 10 véletlenszerű gyöngyöt hozzáadunk
                {
                    int x = vel.Next(1, hatarX+1); // Véletlenszerű x koordináta 1 és 100 között
                    int y = vel.Next(1, hatarY + 1); // Véletlenszerű y koordináta 1 és 100 között
                    int z = vel.Next(1, hatarZ + 1); // Véletlenszerű z koordináta 1 és 100 között
                    int e = vel.Next(1, 16);   // Véletlenszerű energiaérték 1 és 10 között

                    // Az újonnan létrehozott véletlenszerű gyöngy hozzáadása a listához
                    gyongyoklista.Add(new Gyongy(x, y, z, e));
                }
            }
            else
            {
                gyongyoklista = File.ReadAllLines(map_path).Skip(1).Select(x => new Gyongy(x)).ToList();
            }

            // Gyöngyök hozzáadása a viewport-hoz
            foreach (var gyongy in gyongyoklista)
            {
                EllipsoidVisual3D gyongy3D = new EllipsoidVisual3D();
                gyongy3D.RadiusX = gyongy.E;
                gyongy3D.RadiusY = gyongy.E;
                gyongy3D.RadiusZ = gyongy.E;
                gyongy3D.Center = new Point3D(gyongy.Y * szorzo, gyongy.X * szorzo, gyongy.Z * szorzo);
                gyongy3D.Fill = new SolidColorBrush(Color.FromRgb(245, 203, 66));
                ter.Children.Add(gyongy3D);
            }

            // Robot hozzáadása a viewport-hoz
            CubeVisual3D robot = new CubeVisual3D();
            robot.SideLength = 1;
            robot.Center = new Point3D(0, 0, 0);
            ter.Children.Add(robot);

            // Útvonal meghatározása és megjelenítése
            MegjelenitUtvonal(); // Eredetileg csak egyszer hívtad meg, itt ismét meg kell hívni a frissítéshez
            tbIdo.Text = ido.ToString() + "s";
            tbSebesseg.Text = sebesseg.ToString() + "m/s";
            tbosszesgyongyok.Text = gyongyoklista.Count.ToString();
            tbOsszespontok.Text = gyongyoklista.Sum(x => x.E).ToString();
        }

        private async void MegjelenitUtvonal()
        {
            // A robot pozíciója
            Point3D robotPozicio = new Point3D(0, 0, 0);

            //akvarium

            Color color = Colors.Blue;
            double opacity = 0.2;

            //legkisebb x,y,z koordinatak megkeresebe
            Gyongy legkozelebbigyongy = gyongyoklista[0];
            List<Gyongy> modositottgyongyoklista = gyongyoklista;
            modositottgyongyoklista.Insert(0, new Gyongy(0, 0, 0, 0));
            int erteke = 0;
            foreach (var item in modositottgyongyoklista)
            {
                if (item.X + item.Z + item.Y > erteke)
                {
                    erteke = item.X + item.Z + item.Y;
                    legkozelebbigyongy = item;
                }
            }

            //megkeressuk a 0,0,0-tol (tehat robottol) a legmesszebb eso gyongyot es kiszamitjuk a kozepet a gyongyoknek
            double legnagyobbX = 0;
            foreach (var item in modositottgyongyoklista)
            {
                if (item.X > legnagyobbX)
                {
                    legnagyobbX = item.X;
                }
            }
            double legnagyobbY = 0;
            foreach (var item in modositottgyongyoklista)
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



           // TranslateTransform3D translateTransform = new TranslateTransform3D();
           // modelVisual.Transform = translateTransform;
            robotmodel = modelVisual; //en irtam


            ter.Children.Add(modelVisual);



















            // Legközelebbi gyöngy keresése
            Gyongy legkozelebbiGyongy = null;
            double legkisebbTavolsag = double.MaxValue;
            foreach (var gyongy in gyongyoklista)
            {
                double tavolsag = Math.Sqrt(Math.Pow(gyongy.X * szorzo - robotPozicio.X, 2) +
                                            Math.Pow(gyongy.Y * szorzo - robotPozicio.Y, 2) +
                                            Math.Pow(gyongy.Z * szorzo - robotPozicio.Z, 2));
                if (tavolsag < legkisebbTavolsag)
                {
                    legkisebbTavolsag = tavolsag;
                    legkozelebbiGyongy = gyongy;
                }
            }

            // Útvonal meghatározása
            List<Gyongy> utvonal = new List<Gyongy>();
            List<int> felvettIndexek = new List<int>(); // Gyöngyök indexeinek tárolása
            utvonal.Add(new Gyongy(0, 0, 0, 0)); //robot az elso "gyongy", hogy onnan induljon a vonal az elsohoz es az indexe 0
            felvettIndexek.Add(0);
            if (algoritmus == "moho")
            {
                utvonal = MohoAlgoritmus(utvonal, ido, sebesseg);
            }
            else if (algoritmus == "bruteforce")
            {
                utvonal = Bruteforce(utvonal, ido, sebesseg);
            }




            // Útvonal megjelenítése és visszafelé mozgatása
            for (int i = 0; i < utvonal.Count - 1; i++)
            {
                var pont1 = utvonal[i];
                var pont2 = utvonal[i + 1];

                var lineVisual = new LinesVisual3D();
                lineVisual.Points.Add(new Point3D(pont1.Y * szorzo, pont1.X * szorzo, pont1.Z * szorzo));
                lineVisual.Points.Add(new Point3D(pont2.Y * szorzo, pont2.X * szorzo, pont2.Z * szorzo));
                lineVisual.Color = Colors.Green;
                lineVisual.Thickness = 2;
                ter.Children.Add(lineVisual);

                // Számok hozzáadása a 3D térbe a felvett gyöngyök helyéhez
                var textVisual = new BillboardTextVisual3D
                {
                    Position = new Point3D(pont2.Y * szorzo, pont2.X * szorzo, pont2.Z * szorzo + 20), // +20 Y tengelyen eltolva
                    Text = (i + 1).ToString(), // Gyöngyök indexének számozása
                    Foreground = Brushes.Black,
                    Background = Brushes.White
                };
                ter.Children.Add(textVisual);

                // Visszafelé mozgatás
                if (!(i + 2 == utvonal.Count))
                {
                    tbGyongyok.Text = Convert.ToString(i + 1);
                }

                tbPontok.Text = Convert.ToString(Convert.ToInt32(tbPontok.Text) + utvonal[i + 1].E);
                tbPozicio.Text = $"{utvonal[i + 1].X},{utvonal[i + 1].Y},{utvonal[i + 1].Z}";
                await Task.Delay(100);
                robotPozicio = new Point3D(pont2.Y * szorzo, pont2.X * szorzo, pont2.Z * szorzo);
            }





            utvonalanim = utvonal;
        }










        private async void AnimacioFuttatas(ModelVisual3D robotmodel, int i)
        {

            if (i < utvonalanim.Count - 1)
            {
                // Az aktuális transzformációk meghatározása
                Transform3DGroup currentTransform = (Transform3DGroup)robotmodel.Transform;

                // Az előző elmozdulások törlése
                for (int j = currentTransform.Children.Count - 1; j >= 0; j--)
                {
                    if (currentTransform.Children[j] is TranslateTransform3D)
                    {
                        currentTransform.Children.RemoveAt(j);
                    }
                }

                var pont1 = utvonalanim[i];
                var pont2 = utvonalanim[i + 1];

                // Az elmozdulás transzformáció létrehozása és hozzáadása a transzformációs láncunkhoz
                TranslateTransform3D translation = new TranslateTransform3D(pont1.Y * szorzo, pont1.X * szorzo, pont1.Z * szorzo);
                currentTransform.Children.Add(translation);
                robotmodel.Transform = currentTransform;

                // Animációs tulajdonságok beállítása
                DoubleAnimation animX = new DoubleAnimation(pont2.Y * szorzo, TimeSpan.FromSeconds(utvonalanim[i].DistanceTo(utvonalanim[i + 1]) / sebesseg));
                DoubleAnimation animY = new DoubleAnimation(pont2.X * szorzo, TimeSpan.FromSeconds(utvonalanim[i].DistanceTo(utvonalanim[i + 1]) / sebesseg));
                DoubleAnimation animZ = new DoubleAnimation(pont2.Z * szorzo, TimeSpan.FromSeconds(utvonalanim[i].DistanceTo(utvonalanim[i + 1]) / sebesseg));

                // Animáció elindítása
                translation.BeginAnimation(TranslateTransform3D.OffsetXProperty, animX);
                translation.BeginAnimation(TranslateTransform3D.OffsetYProperty, animY);
                translation.BeginAnimation(TranslateTransform3D.OffsetZProperty, animZ);

                // Várakozás az animáció befejezéséig
                await Task.Delay((int)((utvonalanim[i].DistanceTo(utvonalanim[i + 1]) / sebesseg) * 1000));

                // Következő lépés indítása
                AnimacioFuttatas(robotmodel, i + 1);
            }
        }




        private void btnAnimacio_Click(object sender, RoutedEventArgs e)
        {
            AnimacioFuttatas(robotmodel, 0);
        }

        private List<Gyongy> MohoAlgoritmus(List<Gyongy> utvonal, double idolimit, double haladasi_sebesseg)
        {
            //az utvonal egy gyongylista amiben csak a robot van benne, ezt masolja a generaltlistaba es azt adja vissz a fuggveny
            List<Gyongy> gyongyokmoho = new List<Gyongy>(gyongyoklista); // Másolat készítése a gyöngyök listájáról
            List<Gyongy> generaltutvonal = new List<Gyongy>(utvonal); // Másolat készítése az útvonal listáról
            bool vanido = true;
            double maxtav = haladasi_sebesseg * idolimit;

            while (vanido && gyongyokmoho.Count > 0)
            {
                Gyongy legnagyobb = new Gyongy(0, 0, 0, 0);
                double legnagyobb_tavolsaga = double.MaxValue;

                foreach (var gyongy in gyongyokmoho)
                {
                    // Távolság kiszámítása a jelenlegi és a gyöngy között
                    double tavolsag = Math.Sqrt(Math.Pow(generaltutvonal[generaltutvonal.Count - 1].X - gyongy.X, 2) +
                                                Math.Pow(generaltutvonal[generaltutvonal.Count - 1].Y - gyongy.Y, 2) +
                                                Math.Pow(generaltutvonal[generaltutvonal.Count - 1].Z - gyongy.Z, 2));

                    // Ellenőrizze, hogy a gyöngy elérhető-e a megengedett időkorlát és sebesség alapján
                    if (gyongy.E > legnagyobb.E && (maxtav - tavolsag - gyongy.getTavolsag000) >= 0)
                    {
                        legnagyobb = gyongy;
                        legnagyobb_tavolsaga = tavolsag;
                    }
                }

                // Ha nem találunk több megfelelő gyöngyöt, akkor kilépünk a ciklusból
                if (legnagyobb_tavolsaga == double.MaxValue)
                {
                    vanido = false;
                }
                else
                {
                    // Az útvonalhoz hozzáadjuk a legközelebbi gyöngyöt
                    generaltutvonal.Add(legnagyobb);
                    //MessageBox.Show($"{legnagyobb.X},{legnagyobb.Y},{legnagyobb.Z},{legnagyobb.E}");
                    // A már hozzáadott gyöngyöt eltávolítjuk a lehetséges gyöngyök listájából
                    gyongyokmoho.Remove(legnagyobb);
                    // A maradék távolságot csökkentjük
                    maxtav -= legnagyobb_tavolsaga;
                }
            }

            // Az útvonal végéhez hozzáadjuk a kiindulópontot (0,0,0)
            generaltutvonal.Add(new Gyongy(0, 0, 0, 0));
            return generaltutvonal;
        }
        private List<Gyongy> Bruteforce(List<Gyongy> utvonal, double idolimit, double haladasi_sebesseg)
        {
            Range range = new Range(1, gyongyoklista.Count);
            List<Gyongy> masoltLista = gyongyoklista.Take(range).ToList();
            double tavolsag = idolimit * haladasi_sebesseg;
            //az utvonal egy gyongylista amiben csak a robot van benne, ezt masolja a generaltlistaba es azt adja vissz a fuggveny

            //IDE IRJAL ZSOLTIKAM

            double atlag = gyongyoklista.Average(x => x.E);

            for (int i = 0; i < gyongyoklista.Count - 1; i++)
            {
                if (FindClosestGyongies(masoltLista, utvonal.Last()).Count == 2)
                {
                    Gyongy elsoLegkozelebbi = FindClosestGyongies(masoltLista, utvonal.Last())[0];
                    Gyongy masodikLegkozelebbi = FindClosestGyongies(masoltLista, utvonal.Last())[1];
                    if (masodikLegkozelebbi.E > elsoLegkozelebbi.E * 2.9 && tavolsag - masodikLegkozelebbi.DistanceTo(utvonal.Last()) >= 0 && tavolsag - masodikLegkozelebbi.getTavolsag000 >= 0)
                    {
                        tavolsag -= masodikLegkozelebbi.DistanceTo(utvonal.Last());
                        utvonal.Add(masodikLegkozelebbi);
                        masoltLista.Remove(masodikLegkozelebbi);
                    }
                    else
                    {
                        if (tavolsag - elsoLegkozelebbi.DistanceTo(utvonal.Last()) >= 0 && tavolsag - elsoLegkozelebbi.getTavolsag000 >= 0)
                        {
                            tavolsag -= elsoLegkozelebbi.DistanceTo(utvonal.Last());
                            utvonal.Add(elsoLegkozelebbi);
                            masoltLista.Remove(elsoLegkozelebbi);
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    Gyongy elsoLegkozelebbi = FindClosestGyongies(masoltLista, utvonal.Last())[0];
                    if (tavolsag - elsoLegkozelebbi.DistanceTo(utvonal.Last()) >= 0 && tavolsag - elsoLegkozelebbi.getTavolsag000 >= 0)
                    {
                        tavolsag -= elsoLegkozelebbi.DistanceTo(utvonal.Last());
                        utvonal.Add(elsoLegkozelebbi);
                        masoltLista.Remove(elsoLegkozelebbi);
                    }
                    else
                    {

                    }
                }
            }
            utvonal.Add(utvonal[0]);
            return utvonal;
        }
        static Gyongy FindClosestGyongy(List<Gyongy> gyongyList, Gyongy givenGyongy)
        {
            Gyongy closestGyongy = null;
            double minDistance = double.MaxValue;

            foreach (Gyongy gyongy in gyongyList)
            {
                double distance = gyongy.DistanceTo(givenGyongy);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestGyongy = gyongy;
                }
            }

            return closestGyongy;
        }
        static List<Gyongy> FindClosestGyongies(List<Gyongy> gyongyList, Gyongy givenGyongy)
        {
            List<Gyongy> closestGyongies = new List<Gyongy>();
            SortedDictionary<double, Gyongy> distanceGyongyMap = new SortedDictionary<double, Gyongy>();

            if (gyongyList.Count > 1)
            {
                foreach (Gyongy gyongy in gyongyList)
                {
                    double distance = gyongy.DistanceTo(givenGyongy);
                    distanceGyongyMap[distance] = gyongy;
                }

                int count = 0;
                foreach (var kvp in distanceGyongyMap)
                {
                    closestGyongies.Add(kvp.Value);
                    count++;
                    if (count == 2) break; // Return only the two closest Gyongy objects
                }
            }
            else
            {
                closestGyongies.Add(FindClosestGyongy(gyongyList, givenGyongy));
            }

            return closestGyongies;
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow openWindow = new MainWindow();
            openWindow.Show();
            this.Close();
        }




        private void imgkilepes_MouseEnter(object sender, MouseEventArgs e)
        {
            imgkilepes.Source = new BitmapImage(new Uri("/Images/kilepeshover.png", UriKind.Relative));
        }
        private void imgkilepes_MouseLeave(object sender, MouseEventArgs e)
        {
            imgkilepes.Source = new BitmapImage(new Uri("/Images/kilepes.png", UriKind.Relative));
        }


        private void imganimacio_MouseEnter(object sender, MouseEventArgs e)
        {
            imganimacio.Source = new BitmapImage(new Uri("/Images/animaciohover.png", UriKind.Relative));
        }
        private void imganimacio_MouseLeave(object sender, MouseEventArgs e)
        {
            imganimacio.Source = new BitmapImage(new Uri("/Images/animacio.png", UriKind.Relative));
        }


    }
}
