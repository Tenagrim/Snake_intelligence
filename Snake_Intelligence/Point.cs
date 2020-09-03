using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Intelligence
{
    [Serializable]
    class Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Point()
        {
            x = 0;
            y = 0;
        }

        public Point(Point re)
        {
            x = re.x;
            y = re.y;
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.x + b.x, a.y + b.y);
        }

        public static bool operator ==(Point a, Point b)
        {
            return (a.x == b.x && a.y == b.y);
        }
        public static bool operator !=(Point a, Point b)
        {
            return !(a.x != b.x || a.y != b.y);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"x = {x}  y = {y} ";
        }
    }
}
