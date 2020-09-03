using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Snake_Intelligence
{
    [Serializable]
    class Field
    {
        public Snake snake { get; set; }

        public Snake[] Population { get; set; }

        public int Width { get; }
        public int Height { get; }

        public int PopulationCount { get; set; }
        public int GenerationCount { get; set; }
        public int[,] field { get; set; }
        private Point food;
        private Random rand;
        private int prev_len;
        private int step_limit = 1000;
        public Field(Point size) : this(size.x, size.y)
        {
        }
        public Field(int width, int heigth)
        {
            rand = new Random();
            Height = heigth;
            Width = width;

            PopulationCount = 500;
            GenerationCount = 0;
            Population = new Snake[PopulationCount];
            for (int i = 0; i < PopulationCount; i++)
                Population[i] = new Snake(Width / 2, Height / 2);
            snake = snake = Population[0];
            field = new int[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                    field[i, j] = (i == 0 || j == 0 || i == Height - 1 || j == Width - 1) ? 1 : 0;
            }
            projectSnake();
        }

        private float EvalSnake(Snake snake)
        {
            return snake.Length + snake.Way / 5;
        }

        private Snake GetBestSnake()
        {
            Snake best = Population[0];
            //int max_len = Population[0].Length;
            foreach (var s in Population.Skip(1))
            {
                if (EvalSnake(s) > EvalSnake(best))
                    best = s;
            }
            return best;
        }

        public void NextGen()
        {
            Debug.WriteLine("next gen");
            Snake best = GetBestSnake();
            Population = new Snake[PopulationCount];
            for (int i = 0; i < PopulationCount; i++)
            {
                Population[i] = new Snake(Width / 2, Height / 2, best);
                //if (i != 0)
                Population[i].Mutate();
                Population[i].Mutate();
                Population[i].Mutate();

            }
            snake = Population[0];
            GenerationCount++;
        }

        public void reloadField()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                    field[i, j] = (i == 0 || j == 0 || i == Height - 1 || j == Width - 1) ? 1 : 0;
            }
            projectSnake();
            MakeFood();
        }

        public void RunPopulation()
        {
            foreach (var s in Population.Skip(1))
            {
                RunSnake(s);
            }
        }

        public bool checkLoop()
        {
            return (snake.Length == prev_len && snake.Way > step_limit);
        }

        private void RunSnake(Snake snake)
        {
            this.snake = snake;
            reloadField();
            while (snake.Alive)
            {
                //prev_len = snake.Length;
                Step();
                if(checkLoop())
                    snake.Kill();
                // Debug.WriteLine(snake);
            }
        }

        private void projectSnake()
        {
            foreach (var p in snake.Body)
            {
                field[p.y, p.x] = 3;
            }
        }
        private int LookTo(Point direction, bool food)
        {
            Point step = new Point(snake.Body[0]);
            int dist = 1;
            step = step + direction;
            while (step.x >= 0 && step.x < Width && step.y >= 0 && step.y < Height)
            {

                if (!food)
                {
                    if (field[step.y, step.x] == 1 || field[step.y, step.x] == 3)
                        return dist;
                }
                else
                {
                    if (field[step.y, step.x] == 2)
                        return dist;
                }
                dist++;
                step = step + direction;
            }
            if (food)
                return (0);
            return dist;
        }
        private float[] CollectData()
        {
            float[] res = new float[11];
            res[0] = LookTo(new Point(1, 0), false);
            res[1] = LookTo(new Point(-1, 0), false);
            res[2] = LookTo(new Point(0, 1), false);
            res[3] = LookTo(new Point(0, -1), false);
            res[4] = LookTo(new Point(1, 0), true);
            res[5] = LookTo(new Point(-1, 0), true);
            res[6] = LookTo(new Point(0, 1), true);
            res[7] = LookTo(new Point(0, -1), true);
            res[8] = snake.Body[0].x;
            res[9] = snake.Body[0].y;
            res[10] = 1.0F;

            return res;
        }

        private float[] NormedData()
        {
            float[] res = CollectData();

            res[0] = res[0] / Width;
            res[1] = res[1] / Width;
            res[2] = res[2] / Height;
            res[3] = res[3] / Height;
            res[4] = res[4] / Width;
            res[5] = res[5] / Width;
            res[6] = res[6] / Height;
            res[7] = res[7] / Height;
            res[8] = res[8] / Width;
            res[9] = res[9] / Height;

            return res;
        }
        public void MakeFood()
        {
            bool f = true;
            Point pos = new Point();
            while (f)
            {
                f = false;
                pos.x = rand.Next(1, Width - 1);
                pos.y = rand.Next(1, Height - 1);
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

        public void Show()
        {
            snake.See(NormedData());
        }

        public void Step()
        {
            prev_len = snake.Length;
            Step(snake);
        }

        public void Step(Snake snake, Point direction)
        {

            //Point direction = snake.ChooseDirection();
            Point new_pos = snake.Body[0] + direction;
            Point tail = snake.Body.Last();
            switch (field[new_pos.y, new_pos.x])
            {
                case 2:
                    field[tail.y, tail.x] = 0;
                    field[new_pos.y, new_pos.x] = 3;
                    snake.Step(direction, true);
                    //field[new_pos.y, new_pos.x] = 0;
                    MakeFood();
                    break;
                case 1:
                    snake.Kill();
                    break;
                case 0:
                    field[tail.y, tail.x] = 0;
                    field[new_pos.y, new_pos.x] = 3;
                    snake.Step(direction, false);
                    //field[new_pos.y, new_pos.x] = 0;
                    break;
                case 3:
                    field[tail.y, tail.x] = 0;
                    field[new_pos.y, new_pos.x] = 3;
                    snake.Step(direction, false);
                    //field[new_pos.y, new_pos.x] = 0;
                    break;
            }

        }

        public void Step(Snake snake)
        {
            Show();
            snake.brain.Calc();
            Point direction = snake.ChooseDirection();
            Step(snake, direction);
        }
        public void ReloadSnake()
        {
            snake.Reload(Width / 2, Height / 2);
        }


    }
}
