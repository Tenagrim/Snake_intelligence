using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Intelligence
{
    class Snake
    {
        public List<Point> Body { get { return body; } }

        private List<Point> body;  
        public int Point { get; } // 1 - up / -1 - left / 2 - down / -2 - right
        private Point direction;
        private Point tmp;
        public Snake()
        { 
            direction = new Point(1,0);
            tmp = new Point();
            body = new List<Point>();
            body.Add(new Point(0, 0));
            body.Add(new Point(0, 1));
            body.Add(new Point(0, 2));
        }

        public void Step()
        {
            body[0] = body[0] + direction;
            int len = body.Count;
            for (int i = 1; i < len; i++)
            {
                body[i] = body[i - 1];
            }

        }
    }
}
