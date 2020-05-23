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

            var list = new List<PointF>()
            {
                new PointF(4,4),
                new PointF(5,1),
                new PointF(3,6),
                new PointF(6,3),
                new PointF(2,5)
            };

            KDTree tree = new KDTree();


            tree.BuildKDTree(list);
            var point=tree.NearestPoint(new PointF(3, 5));

        }
    }
}
