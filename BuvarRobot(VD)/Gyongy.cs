using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuvarRobot_VD_
{
    class Gyongy
    {
        int x, y, z, e;

        public Gyongy(string csvSor)
        {
            int[] mezok = csvSor.Split(';').Take(4).Select(x => Convert.ToInt32(x)).ToArray();
            this.X = mezok[0];
            this.Y = mezok[1];
            this.Z = mezok[2];
            this.E = mezok[3];
        }
        public Gyongy(int x, int y, int z, int e)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.E = e;
        }
        public double DistanceTo(Gyongy otherGyongy)
        {
            double dx = X - otherGyongy.X;
            double dy = Y - otherGyongy.Y;
            double dz = Z - otherGyongy.Z;
            return Math.Round(Math.Sqrt(dx * dx + dy * dy + dz * dz), 4);
        }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Z { get => z; set => z = value; }
        public int E { get => e; set => e = value; }
        public double getTavolsag000 { get => Math.Sqrt((Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2))); }
    }
}
