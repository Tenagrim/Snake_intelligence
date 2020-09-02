using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Intelligence
{
    class Field
    {
        public Snake snake { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int[,] field { get; set; }
        private Point food;
        private Random rand;

        public Field(Point size) : this(size.x, size.y)
        {
        }
        public Field(int width, int heigth)
        {

            rand = new Random();
            Height = heigth;
            Width = width;
            snake = new Snake(Width / 2, Height / 2);
            field = new int[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                    field[i, j] = (i == 0 || j == 0 || i == Height - 1 || j == Width - 1) ? 1 : 0;
            }
        }

        public float[] CollectData()
        {
            float[] res = new float[13];
            res[0] = 
            res[13] = 1.0F;

            return res;
        }
        public void MakeFood()
        {
            bool f = true;
            Point pos = new Point();
            while (f)
            {
                f = false;
                pos.x = rand.Next(1,Width-1);
                pos.y = rand.Next(1,Height-1);
                foreach (var p in snake.Body)
                {
                    if (p == pos)
                    {
                        f = true;
                        break;
                    }
                }
            }
            field[pos.y, pos.x] = 2;
            food = pos;
        }

        public void Step(Snake snake, Point direction)
        {

            //Point direction = snake.ChooseDirection();
            Point new_pos = snake.Body[0] + direction;
            switch (field[new_pos.y, new_pos.x])
            {
                case 2:
                    snake.Step(direction, true);
                    field[new_pos.y, new_pos.x] = 0;
                    MakeFood();
                    break;
                case 1:
                    snake.Kill();
                    break;
                case 0:
                    snake.Step(direction, false);
                    break;
            }

        }
    }
}
