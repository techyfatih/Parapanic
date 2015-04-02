using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace Parapanic
{
    class Utilities
    {
        public static double NormAngle(double a)
        {
            double b = a;
            while (b > 2*Math.PI)
            {
                b -= 2*Math.PI;
            }
            while (b < 0)
            {
                b += 2*Math.PI;
            }
            return b;
        }

        public static int Minimum(int[] arr)
        {
            int min = arr[0];
            foreach (int n in arr) min = Math.Min(min, n);
            return min;
        }
        public static int Maximum(int[] arr)
        {
            int max = arr[0];
            foreach (int n in arr) max = Math.Max(max, n);
            return max;
        }

        public static bool CheckCollisionX(Rectangle r1, Rectangle r2)
        {
            int left = Math.Max(r1.Left, r2.Left);
            int right = Math.Min(r1.Right, r2.Right);
            return left < right;
        }

        public static bool CheckCollisionY(Rectangle r1, Rectangle r2)
        {
            int top = Math.Max(r1.Top, r2.Top);
            int bottom = Math.Min(r1.Bottom, r2.Bottom);
            return top < bottom;
        }

        public static bool CheckCollisionX(int num, Rectangle r)
        {
            return (num > r.Left) && (num < r.Right);
        }

        public static bool CheckCollisionY(int num, Rectangle r)
        {
            return (num > r.Top) && (num < r.Bottom);
        }

        public static bool CheckCollision(Point p, Rectangle r)
        {
            return (p.X >= r.Left && p.X <= r.Right) &&
                   (p.Y >= r.Top && p.Y <= r.Bottom);
        }

        public static float Round(float x, int places)
        {
            int tempf = (int)(x * (float)Math.Pow(10, places));
            return tempf / (float)Math.Pow(10, places);

        }

        public static Vector2 speedToVector(float speed, float direction)
        {
            return new Vector2((float)(Math.Cos(direction) * speed), (float)(Math.Sin(direction) * speed));
        }

        public static float speedToFloat(Vector2 speedV, bool positive)
        {
            return (float)((positive) ? Math.Sqrt(speedV.Y * speedV.Y + speedV.X * speedV.X) : -Math.Sqrt(speedV.Y * speedV.Y + speedV.X * speedV.X));
        }

        public static Vector2 AbsVector(Vector2 v)
        {
            return new Vector2(Math.Abs(v.X), Math.Abs(v.Y));
        }
    }
}
