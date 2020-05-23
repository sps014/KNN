using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KNN
{
    public class KDNode<T>
    {
        public KDNode<T> Left { get; set; }
        public KDNode<T> Right { get; set; }
        public KDNode<T> Parent { get; set; }
        public T Value { get; set; }

    }
    public class KDTree<T>
    {
        public KDNode<T> Root { get; private set; } = null;
        public int K { get; set; } = 2;
        public void CreateBalancedTree(List<T> Data)
        {
            Root = BuildTree(Data.ToArray(), null);
        }
        private KDNode<T> BuildTree(T[] data, KDNode<T> parent, int depth = 0)
        {
            if (data.Length is 0)
                return null;

            int axis = depth % K;

            var sorted = data.OrderBy(i => (i as dynamic)[axis]).ToList();

            int mid = sorted.Count / 2;

            var lower = Split(sorted, 0, mid - 1);
            var upper = Split(sorted, mid + 1, sorted.Count - 1);

            KDNode<T> node = new KDNode<T>();

            node.Value = sorted[mid];
            node.Parent = parent;
            node.Left = BuildTree(lower, node, depth + 1);
            node.Right = BuildTree(upper, node, depth + 1);

            return node;

        }
        public T NearestPoint(T point)
        {
            return GetNearestPoint(point, Root);
        }
        private T GetNearestPoint(T point, KDNode<T> parent, int depth = 0)
        {
            if (parent is null)
                return default;

            int axis = depth % K;

            KDNode<T> nextBranch, oppositeBranch;

            if ((point as dynamic)[axis] < (parent.Value as dynamic)[axis])
            {
                nextBranch = parent.Left;
                oppositeBranch = parent.Right;
            }
            else
            {
                nextBranch = parent.Right;
                oppositeBranch = parent.Left;
            }

            T br = GetNearestPoint(point, nextBranch, depth + 1);
            T best = CloserDistance(point, br, parent.Value);

            var distancePlane = Math.Abs((point as dynamic)[axis] - (parent.Value as dynamic)[axis]);

            if (Distance(point, best) > distancePlane)
            {
                T opposite = GetNearestPoint(point, oppositeBranch, depth + 1);
                best = CloserDistance(point, opposite, best);
            }

            return best;
        }
        private T[] Split(List<T> data, int start, int end)
        {
            int size = end - start + 1;
            T[] split = new T[size];

            Parallel.For(start, end + 1, (i) =>
            {
                split[i - start] = data[i];
            });
            return split;
        }

        T CloserDistance(T pivot, T p1, T p2)
        {

            if (p1 is null)
                return p2;

            if (p2 is null)
                return p1;


            if (Distance(pivot, p2) < Distance(pivot, p1))
                return p2;
            else
                return p1;
        }

        public double EuclidianDistance(dynamic a, dynamic b)
        {
            double dx = a[0] - b[0];
            double dy = a[1] - b[1];

            return Math.Sqrt(dx * dx + dy * dy);
        }
        private Func<dynamic, dynamic, double> distance = null;
        public Func<dynamic, dynamic, double> Distance
        {
            get
            {
                if (distance is null)
                    distance = EuclidianDistance;
                return EuclidianDistance;
            }
            set
            {
                distance=value;
            }
        }
    }
}
