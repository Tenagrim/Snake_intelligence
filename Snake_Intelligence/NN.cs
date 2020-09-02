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

        public NN()
        {
            //sizes = new int[] { 13, 15, 15, 4 };

            sizes = new int[] { 13, 10, 3, 3 };

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
            Random rand = new Random();

            for (int k = 0; k < weights.Length; k++)
            {
                for (int i = 0; i < weights[k].GetLength(0); i++)
                {
                    for (int j = 0; j < weights[k].GetLength(1); j++)
                    {
                        weights[k][i, j] = (float)rand.NextDouble();
                    }
                }
            }
        }
    }
}
