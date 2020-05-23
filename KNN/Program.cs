using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace KNN
{
    class Program
    {
        
        static void Main(string[] args)
        {

            var list = new List<double[]>()
            {
                new double[]{4,4},
                new double[]{5,1},
                new double[]{3,6},
                new double[]{6,3},
                new double[]{2,5}
            };

            KDTree<double[]> tree = new KDTree<double[]>();


            tree.CreateBalancedTree(list);
            var point=tree.NearestPoint(new double[] { 3,5});

            //string str = JsonSerializer.Serialize(tree.Root);

            //StreamWriter writer = new StreamWriter("mon.txt");
            //writer.Write(str);
            //writer.Close();
        }
    }
}
