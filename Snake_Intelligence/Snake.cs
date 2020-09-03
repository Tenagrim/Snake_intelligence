using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Snake_Intelligence
{
    class Snake
    {
        public List<Point> Body { get { return body; } }

        private List<Point> body;
        public int Point { get; } // 1 - up / -1 - left / 2 - down / -2 - right
        public Point direction { get; set; }
        private bool alive;
        private Point tmp;
        public NN brain;
        public int Length { get { return length; } }
        private int length;
        public Snake()
        {
            direction = new Point(1, 0);
            body = new List<Point>();
            body.Add(new Point(0, 0));
            body.Add(new Point(0, 1));
            body.Add(new Point(0, 2));
            alive = true;
            brain = new NN();
            tmp = new Point();
            length = 3;
        }
        public Snake(int x, int y)
        {
            direction = new Point(1, 0);
            tmp = new Point();
            body = new List<Point>();
            body.Add(new Point(x, y));
            body.Add(new Point(x, y + 1));
            alive = true;
            brain = new NN();
            length = 3;
        }

        public void Step(Point direction, bool eat)
        {
            this.direction = direction;
            Step(eat);
        }

        public void Step(bool eat)
        {
            Point t = new Point();
            if (!alive)
                return;
            tmp = body[0];
            body[0] = body[0] + direction;
            if (body[1] == body[0])
            {
                Kill();
                return;
            }
            int len = body.Count;
            if (eat)
            //   Debug.WriteLine($"eat  tail: {body[body.Count - 1]}  tmp: {tmp}");
            {
                body.Add(new Point(body[body.Count - 1]));
                length++;
            }
            for (int i = 1; i < len; i++)
            {
                t.x = body[i].x;
                t.y = body[i].y;
                body[i].x = tmp.x;
                body[i].y = tmp.y;
                tmp.x = t.x;
                tmp.y = t.y;
                if (body[i] == body[0])
                    Kill();
            }
            Debug.WriteLine($"Len: {body.Count}");
            foreach (var p in body)
                Debug.WriteLine(p);
        }

        public Point ChooseDirection()
        {
            Point res = new Point();
            float max = brain.Layers[brain.Layers.Length - 1].Max();
            int index = Array.IndexOf(brain.Layers[brain.Layers.Length - 1], max);
            switch (index)
            {
                case 0:
                    res.y = -1;
                    break;
                case 1:
                    res.x = 1;
                    break;
                case 2:
                    res.y = 1;
                    break;
                case 3:
                    res.x = -1;
                    break;
            }
            return res;
        }

        public void Kill()
        {
            alive = false;
            Debug.WriteLine("Kill");
        }

        public void Reload(int x, int y)
        {
            direction = new Point(1, 0);
            body = new List<Point>();
            body.Add(new Point(x, y));
            body.Add(new Point(x, y + 1));
            alive = true;
        }

        public void See(float[] data)
        {
            brain.Input(data);
        }
    }
}
