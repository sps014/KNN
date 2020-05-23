using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNNs
{
    public class PointF
    {
        public double X { get; set; }
        public double Y { get; set; }
        public PointF(double x,double y)
        {
            X = x;
            Y = y;
        }

    }
    public class KDNode
    {
        public KDNode Left { get; set; }
        public KDNode Right { get; set; }
        public KDNode Parent { get; set; }
        public PointF Value { get; set; }
    }
    public class KDTree
    {
        public KDNode Root { get; set; } = null;
        public int K { get; set; }
        public KDTree(int k=2)
        {
            K = k;
        }
        public double Distance(PointF a,PointF b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;

            return Math.Sqrt(dx * dx + dy * dy);

        }
        public void BuildKDTree(List<PointF> list)
        {
            Root=CreateTree(Root,list);
        }
        private KDNode CreateTree(KDNode parent,List<PointF> list,int depth=0)
        {
            if (list.Count == 0)
                return null;

            int axis = depth % K;
            var sorted = list.OrderBy(i => axis == 0 ? i.X : i.Y).ToList();
            int mid =sorted.Count/2;
            var lower=SplitList(sorted, 0, mid - 1);
            var upper = SplitList(sorted, mid + 1,list.Count-1);


            KDNode node = new KDNode();
            node.Value = list[mid];
            node.Parent = parent;
            node.Left = CreateTree(node, lower, depth+1);
            node.Right = CreateTree(node, upper, depth + 1);

            return node;
        }
        public PointF NearestPoint(PointF point)
        {
            return GetNearestPoint(point,Root);
        }
        private PointF GetNearestPoint(PointF point, KDNode parent, int depth = 0)
        {
            if (parent == null)
                return null;
            int axis = depth % K;

            KDNode nextBranch, oppositeBranch;

            if (axis == 0)
            {
                if (point.X < parent.Value.X)
                {
                    nextBranch = parent.Left;
                    oppositeBranch = parent.Right;
                }
                else
                {
                    nextBranch = parent.Right;
                    oppositeBranch = parent.Left;
                }
            }
            else
            {
                if (point.Y < parent.Value.Y)
                {
                    nextBranch = parent.Left;
                    oppositeBranch = parent.Right;
                }
                else
                {
                    nextBranch = parent.Right;
                    oppositeBranch = parent.Left;
                }
            }

            PointF br=GetNearestPoint(point, nextBranch, depth + 1);
            PointF best = CloserDistance(point, br, parent.Value);

            var distancePlane=0.0;
            if(axis==0)
                distancePlane=Math.Abs(point.X - parent.Value.X);
            else
                distancePlane = Math.Abs(point.Y - parent.Value.Y);

            if(Distance(point,best)>distancePlane)
            {
                PointF opposite = GetNearestPoint(point, oppositeBranch, depth + 1);
                best = CloserDistance(point, opposite, best);
            }

            return best;
        }
        PointF CloserDistance(PointF pivot,PointF p1,PointF p2)
        {
            if (p1 == null)
                return p2;
            if (p2 == null)
                return p1;


            if (Distance(pivot , p2)<Distance(pivot,p1))
                return p2;
            else
                return p1;
        }
        private static List<PointF> SplitList(List<PointF> list,int start,int end)
        {
            List<PointF> points = new List<PointF>();
            for (int i = start; i <= end; i++)
            {
                points.Add(list[i]);
            }
            return points;
        }
    }
}
