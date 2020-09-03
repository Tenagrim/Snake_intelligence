using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Snake_Intelligence
{
    public partial class Form1 : Form
    {
        private Field field;
        private Graphics graphics;
        private Graphics n_graphics;
        private int point_size = 10;
        private Point offset;
        private Point n_offset;
        private int n_layer_width = 70;
        private int n_neuron_Heigth = 15;
        private int n_neuron_Size = 20;
        private Point initialSize = new Point(35, 45);
        bool running = false;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox2.Image = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            n_graphics = Graphics.FromImage(pictureBox2.Image);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void DrawPoint(Brush brush, Point pos)
        {
            graphics.FillRectangle(brush, pos.x * point_size + offset.x, pos.y * point_size + offset.y, point_size - 1, point_size - 1);
        }

        private void DisplayField()
        {
            var brush = Brushes.White;
            graphics.Clear(Color.Black);
            for (int i = 0; i < field.Height; i++)
            {
                for (int j = 0; j < field.Width; j++)
                {

                    switch (field.field[i, j])
                    {
                        case 1:
                            brush = Brushes.White;
                            DrawPoint(brush, new Point(j, i));
                            break;
                        case 2:
                            brush = Brushes.Green;
                            DrawPoint(brush, new Point(j, i));
                            break;
                        case 3:
                            //break;
                            brush = Brushes.Blue;
                            DrawPoint(brush, new Point(j, i));
                            break;
                    }
                }
            }
            foreach (var p in field.snake.Body.Skip(1))
                DrawPoint(Brushes.Gray, p);
            DrawPoint(Brushes.SlateGray, field.snake.Body[0]);
            pictureBox1.Refresh();
            label1.Text = $"Generatoin: {field.GenerationCount}";
            label3.Text = $"Length    : {field.snake.Length}";
            label4.Text = $"Way       : {field.snake.Way}";
        }

        private void DisplayNN()
        {
            n_graphics.Clear(Color.Black);

            int off_y;
            for (int i = 0; i < field.snake.brain.Sizes.Length; i++)
            {
                for (int j = 0; j < field.snake.brain.Sizes[i]; j++)
                {
                    off_y = n_offset.y + ((j - field.snake.brain.Sizes[i]) * n_neuron_Heigth);
                    //  Debug.Write($"{off_y}  ");


                    n_graphics.FillEllipse(Brushes.White, i * n_layer_width + n_offset.x, j * n_neuron_Heigth + off_y, n_neuron_Size, n_neuron_Size);
                    if (i == field.snake.brain.Sizes.Length - 1 && !(field.snake.brain.Layers[i][j] == field.snake.brain.Layers[i].Max()))
                        n_graphics.FillEllipse(Brushes.Gray, i * n_layer_width + n_offset.x, j * n_neuron_Heigth + off_y, n_neuron_Size, n_neuron_Size);
                    n_graphics.DrawString(field.snake.brain.Layers[i][j].ToString(), SystemFonts.DefaultFont, Brushes.Blue, i * n_layer_width + n_offset.x, j * n_neuron_Heigth + 4 + off_y);
                }
                //Debug.WriteLine("");
            }
            pictureBox2.Refresh();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            field = new Field(initialSize);
            offset = new Point((pictureBox1.Width / 2) - (field.Width * point_size / 2), (pictureBox1.Height / 2) - (field.Height * point_size / 2));
            n_offset = new Point((pictureBox2.Width / 2) - (field.snake.brain.Sizes.Length * n_layer_width / 2) + pictureBox2.Width / 10, (pictureBox2.Height / 2) - (field.snake.brain.Sizes.Max() * n_neuron_Heigth / 2) + pictureBox2.Height / 6);
            field.MakeFood();
            DisplayField();
            DisplayNN();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            /*
            if (timer1.Enabled)
                return base.ProcessCmdKey(ref msg, keyData);
            Point dir = new Point(1, 0);
            Debug.WriteLine("key");
            switch (keyData)
            {
                case Keys.W:
                    dir.x = 0;
                    dir.y = -1;
                    break;
                case Keys.A:
                    dir.x = -1;
                    dir.y = 0;
                    break;
                case Keys.D:
                    dir.x = 1;
                    dir.y = 0;
                    break;
                case Keys.S:
                    dir.x = 0;
                    dir.y = 1;
                    break;
            }
            field.Step(field.snake, dir);
            field.Show();
            field.snake.brain.Calc();
            DisplayField();
            DisplayNN();
            */
            return base.ProcessCmdKey(ref msg, keyData);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //field = new Field(initialSize);
            //field.MakeFood();
            field.ReloadSnake();
            field.reloadField();
            DisplayField();
            field.Show();
            field.snake.brain.Calc();
            DisplayNN();
            //button1.Focus( = false;
            pictureBox1.Focus();
        }

        private void SaveField(string filename, Field world)
        {
            //  FileStream fs = new FileStream(filename, FileMode.Truncate);
            //  fs.Close();

            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, world);
            fs.Close();
        }
        private Field LoadField(string filename)
        {
            Field res;
            FileStream fs = new FileStream(filename, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            res = (Field)bf.Deserialize(fs);
            fs.Close();
            return res;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (!running)
                timer1.Start();
            else
                timer1.Stop();
            running = !running;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!field.snake.Alive)
            {
                timer1.Stop();
                field.RunPopulation();
                field.NextGen();
                field.reloadField();
                timer1.Start();
            }

            field.Step();
            if (field.checkLoop())
                field.snake.Kill();
            DisplayField();
            DisplayNN();

            if (field.GenerationCount % 300 == 0)
            {
                string save = $"autosave{field.GenerationCount % 1000}";
                SaveField(save, field);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveField(textBox1.Text, field);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            field = LoadField(textBox1.Text);
            DisplayField();
            DisplayNN();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDown1.Value;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
