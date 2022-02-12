using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpawnThat.Utilities.Spatial;
using System.Collections.Generic;

namespace SpawnThat.Utilities
{
    [TestClass]
    public class SpatialListExtensionsTests
    {
        [TestMethod]
        public void IndexLeftMost()
        {
            var points = new List<TestPoint>
            {
                new TestPoint(1, 1),
                new TestPoint(2, 1),
                new TestPoint(2, 1),
                new TestPoint(3, 1),
                new TestPoint(4, 1),
                new TestPoint(5, 1),
            };

            int leftMostIndex = points.IndexLeft(points[2]);

            Assert.AreEqual(1, leftMostIndex);
        }

        [TestMethod]
        public void IndexLeftMost_Fuzzy()
        {
            var points = new List<TestPoint>
            {
                new TestPoint(-2, 1),
                new TestPoint(-1, 1),
                new TestPoint(2, 1),
                new TestPoint(3, 1),
                new TestPoint(4, 1),
                new TestPoint(5, 1),
            };

            int leftMostIndex = points.IndexLeft(new TestPoint(1.5f, 1));

            Assert.AreEqual(1, leftMostIndex);
        }

        [TestMethod]
        public void IndexRightMost()
        {
            var points = new List<TestPoint>
            {
                new TestPoint(1, 1),
                new TestPoint(2, 1),
                new TestPoint(2, 1),
                new TestPoint(3, 1),
                new TestPoint(4, 1),
                new TestPoint(5, 1),
            };

            int leftMostIndex = points.IndexRight(points[1]);

            Assert.AreEqual(2, leftMostIndex);
        }

        [TestMethod]
        public void IndexRightMost_Fuzzy()
        {
            var points = new List<TestPoint>
            {
                new TestPoint(1, 1),
                new TestPoint(2, 1),
                new TestPoint(2, 1),
                new TestPoint(3, 1),
                new TestPoint(4, 1),
                new TestPoint(5, 1),
            };

            int leftMostIndex = points.IndexRight(new TestPoint(1.5f, 0));

            Assert.AreEqual(1, leftMostIndex);
        }


        [TestMethod]
        public void IndexUp()
        {
            var points = new List<TestPoint>
            {
                new TestPoint(1, 1),
                new TestPoint(2, 2),
                new TestPoint(2, 2),
                new TestPoint(2, 3),
                new TestPoint(2, 4),
                new TestPoint(5, 1),
            };

            int index = points.IndexUp(1, 4, points[2]);

            Assert.AreEqual(1, index);
        }

        [TestMethod]
        public void IndexDown()
        {
            var points = new List<TestPoint>
            {
                new TestPoint(1, 1),
                new TestPoint(2, 1),
                new TestPoint(2, 2),
                new TestPoint(2, 2),
                new TestPoint(2, 2),
                new TestPoint(5, 1),
            };

            int index = points.IndexDown(1, points[2]);

            Assert.AreEqual(4, index);
        }


        [TestMethod]
        public void Insert()
        {
            var points = new List<TestPoint>
            {
                new TestPoint(1, 1),
                new TestPoint(3, 1),
            };

            var newPoint = new TestPoint(2, 1);

            points.Insert(newPoint);

            Assert.AreSame(newPoint, points[1]);
        }


        [TestMethod]
        public void Insert_IntoMiddle()
        {
            var points = new List<TestPoint>
            {
                new TestPoint(-1, -1),
                new TestPoint(-1, 1),
                new TestPoint(3, 3),
                new TestPoint(4, 1),
                new TestPoint(4, 3),
            };

            var newPoint = new TestPoint(2, 25);

            points.Insert(newPoint);

            Assert.AreSame(newPoint, points[2]);
        }

        [TestMethod]
        public void Insert_EndOfList()
        {
            var points = new List<TestPoint>
            {
                new TestPoint(1, 1),
            };

            var newPoint = new TestPoint(2, 1);

            points.Insert(newPoint);

            Assert.AreSame(newPoint, points[1]);
        }

        [TestMethod]
        public void Insert_EmptyList()
        {
            var points = new List<TestPoint>();

            var newPoint = new TestPoint(2, 1);

            points.Insert(newPoint);

            Assert.AreSame(newPoint, points[0]);
        }

        private class TestPoint : IPoint
        {
            public float X { get; set; }
            public float Y { get; set; }

            public TestPoint(float x, float y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
