using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Intelligence
{
    [Serializable]
    class NN
    {
        public int[] Sizes { get { return sizes; } }
        public float[][] Layers { get { return layers; } }
        public float[][,] Weights { get { return weights; } }

        private float[][] layers;
        private int[] sizes;
        private float[][,] weights;
        static Random rand = new Random();
        private float l_c = 0.1F;

        public NN()
        {
            //sizes = new int[] { 13, 15, 15, 4 };
            //rand = new Random();

             sizes = new int[] { 11, 10, 10, 4 };
            //this.sizes = sizes;
            layers = new float[sizes.Length][];
            for (int i = 0; i < sizes.Length; i++)
            {
                layers[i] = new float[sizes[i]];

                /*
                ///
                for (int j = 0; j < sizes[i]; j++)
                {
                    layers[i][j] = j + 1;
                }
                ///
                */
            }

            RandomFill( ref weights);
        }

        public NN(NN reference)
        {
            sizes = new int[] { 11, 10, 10, 4 };

            layers = new float[sizes.Length][];
            for (int i = 0; i < sizes.Length; i++)
            {
                layers[i] = new float[sizes[i]];
            }
            weights = (float[][,])reference.weights.Clone();
            //Mutate();
        }
        private void RandomFill( ref float[][,] weights)
        {
            weights = new float[sizes.Length - 1][,];

            for (int i = 0; i < sizes.Length - 1; i++)
            {
                weights[i] = new float[sizes[i], sizes[i + 1]];
            }

            float tmp;
            for (int k = 0; k < weights.Length; k++)
            {
                for (int i = 0; i < weights[k].GetLength(0); i++)
                {
                    for (int j = 0; j < weights[k].GetLength(1); j++)
                    {
                        tmp = (float)rand.NextDouble();
                        weights[k][i, j] = tmp * 2 - 1.0F;
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

        public float[,] dot(float[,] a, float[,] b)
        {
            float[,] res = new float[a.GetLength(0), b.GetLength(1)];
            float sum;
            for (int i = 0; i < res.GetLength(0); i++) // rows
            {
                for (int j = 0; j < res.GetLength(1); j++) // cols
                {
                    sum = 0;
                    for (int k = 0; k < res.GetLength(0); k++)
                    {
                        sum += a[i, k] * b[j, k];
                    }
                    res[i, j] = sum;
                }
            }

            return res;
        }

        public float[,] minus(float[,] a, float[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                return null;

            float[,] res = new float[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    res[i, j] = a[i, j] - b[i, j];
                }
            }
            return res;
        }
        public float[] minus(float[] a, float[] b)
        {
            if (a.GetLength(0) != b.GetLength(0))
                return null;

            float[] res = new float[a.GetLength(0)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                res[i] = a[i] - b[i];
            }
            return res;
        }

        public float[,] transpose(float[] a)
        {
            float[,] res = new float[a.GetLength(0), 1];
            for (int i = 0; i < a.Length; i++)
            {
                res[i, 0] = a[i];
            }
            return res;
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
            // return 1 / (1 + (float)Math.Pow(Math.E, -x));
            return x;
        }

        public void Calc()
        {
            for (int i = 1; i < layers.Length; i++)
            {
                for (int j = 0; j < layers[i].Length; j++)
                {
                    layers[i][j] = activation(prev_sum(i, j));
                }
            }

            float[,] A = new float[,] { { 1, 2 }, { 1, 2 } };
            float[,] B = new float[,] { { 1, 2 }, { 1, 2 } };
            float[,] C = dot(A, B);
            //Debug.WriteLine(C);
        }

        public void Mutate()
        {
            float tmp = (float)rand.NextDouble();
            int i = rand.Next(0, Sizes.Length -1);
            weights[i][rand.Next(0, weights[i].GetLength(0) - 1), weights[i].GetLength(1) - 1] = tmp * 2 - 1.0F;
        }

        public void backpropagation(float[] ref_values)
        {

        }
    }
}
