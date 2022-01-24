using System;
using System.Linq;
using System.Collections.Generic;

namespace SpawnThat.Utilities
{
    public static class SpatialListExtensions
    {

        public static void Insert<T>(this List<T> points, T point) where T : IPoint
        {
            int index = points.IndexLeft(point.X) + 1;

            for(int i = index; i < points.Count; ++i)
            {
                T checkPoint = points[i];

                if(checkPoint.X > point.X)
                {
                    break;
                }
                if(checkPoint.Y > point.Y)
                {
                    break;
                }

                index = i;
            }

            if (index >= points.Count)
            {
                points.Add(point);
            }
            else
            {
                points.Insert(index, point);
            }
        }

        public static int IndexLeft<T>(this IList<T> points, T point) where T : IPoint
        {
            return IndexLeft(points, point.X);
        }

        public static int IndexLeft<T>(this IList<T> points, float x) where T : IPoint
        {
            int n = points.Count;
            int left = 0;
            int right = n;

            while (left < right)
            {
                int middle = (left + right) / 2;

                float value = points[middle].X;
                float nextValue = points[Math.Min(middle + 1, n - 1)].X;

                if (value < x)
                {
                    if (nextValue > x)
                    {
                        return middle;
                    }
                    else
                    {
                        left = middle + 1;
                    }
                }
                else
                {
                    right = middle;
                }
            }

            return left;
        }

        public static int IndexRight<T>(this IList<T> points, T point) where T : IPoint
        {
            return IndexRight(points, point.X);
        }

        public static int IndexRight<T>(this IList<T> points, float x) where T : IPoint
        {
            int n = points.Count;
            int left = 0;
            int right = n;

            while (left < right)
            {
                int middle = (left + right) / 2;

                var value = points[middle].X;
                var prevValue = points[Math.Max(middle - 1, 0)].X;

                if (value > x)
                {
                    if (prevValue < x)
                    {
                        //This won't actually return the rightmost in the case of fuzzy match, and value having duplicates
                        //But we don't really care, since this is just used to return an index that will correctly put the input into the correct order.
                        return middle;
                    }

                    right = middle;
                }
                else
                {
                    left = middle + 1;
                }
            }

            return right - 1;
        }

        public static int IndexUp<T>(this IList<T> points, int xLower, int xUpper, T point) where T : IPoint
        {
            return IndexUp(points, xLower, xUpper, point.Y);
        }

        public static int IndexUp<T>(this IList<T> points, int xLower, int xUpper, float y) where T : IPoint
        {
            int n = points.Count;
            int left = xLower;
            int right = xUpper;

            while (left < right)
            {
                int middle = (left + right) / 2;

                float value = points[middle].Y;
                float nextValue = points[Math.Min(middle + 1, n - 1)].Y;

                if (value < y)
                {
                    if (nextValue > y)
                    {
                        return middle;
                    }
                    else
                    {
                        left = middle + 1;
                    }
                }
                else
                {
                    right = middle;
                }
            }

            return left;
        }

        public static int IndexDown<T>(this IList<T> points, int xIndex, T point) where T : IPoint
        {
            return IndexDown(points, xIndex, point.Y);
        }

        public static int IndexDown<T>(this IList<T> points, int xIndex, float y) where T : IPoint
        {
            if (xIndex >= points.Count)
            {
                return xIndex;
            }

            var xValue = points[xIndex].X;

            int left = xIndex;
            int right = points.Count;

            while (left < right)
            {
                int middle = (left + right) / 2;

                if(points[middle].X > xValue)
                {
                    return middle - 1;
                }
                else if (points[middle].Y > y)
                {
                    right = middle;
                }
                else
                {
                    left = middle + 1;
                }
            }

            return right - 1;
        }
    }
}
