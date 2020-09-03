using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Intelligence
{
    class NN
    {
        public int[] Sizes { get { return sizes; } }
        public float[][] Layers { get { return layers; } }
        public float[][,] Weights { get { return weights; } }

        private float[][] layers;
        private int[] sizes;
        private float[][,] weights;
        Random rand;
        private float l_c = 0.1F;

        public NN()
        {
            //sizes = new int[] { 13, 15, 15, 4 };
            rand = new Random();

            sizes = new int[] { 11, 10, 10, 4 };

            layers = new float[sizes.Length][];
            for (int i = 0; i < sizes.Length; i++)
            {
                layers[i] = new float[sizes[i]];

                ///
                for (int j = 0; j < sizes[i]; j++)
                {
                    layers[i][j] = j + 1;
                }
                ///
            }

            weights = new float[sizes.Length - 1][,];

            for (int i = 0; i < sizes.Length - 1; i++)
            {
                weights[i] = new float[sizes[i], sizes[i] + 1];
            }

            float tmp;
            for (int k = 0; k < weights.Length; k++)
            {
                for (int i = 0; i < weights[k].GetLength(0); i++)
                {
                    for (int j = 0; j < weights[k].GetLength(1); j++)
                    {
                        tmp = (float)rand.NextDouble();
                        weights[k][i, j] = tmp / 10 - 0.5F;
                    }
                }
            }
        }
        public void Input(float[] data)
        {
            if (data.Length != layers[0].Length)
                return;
            for (int i = 0; i < data.Length; i++)
            {
                layers[0][i] = data[i];
            }
        }

        private float prev_sum(int i, int j)
        {
            float res = 0;
            for (int k = 0; k < sizes[i - 1]; k++)
            {
                res += layers[i - 1][k] * weights[i - 1][k, j];
            }
            return res;
        }

        private float activation(float x)
        {
            return 1 / (1 + (float)Math.Pow(Math.E, -x));
        }

        public void Calc()
        {
            for (int i = 1; i < layers.Length; i++)
            {
                for (int j = 0; j < layers[i].Length; j++)
                {
                    layers[i][j] = activation( prev_sum(i, j));
                }
            }
        }

        public void Mutate()
        {
            float tmp = (float)rand.NextDouble();
            int i = rand.Next(0, Sizes.Length);
            weights[i][rand.Next(0,weights[i].GetLength(0) -1), weights[i].GetLength(1) - 1] = tmp / 10 - 0.5F;
        }

        private void backpropagation()
        { 
            
        }
    }
}
