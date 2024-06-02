using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpawnThat.Utilities.Spatial;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnThat.Tests.Utilities
{
    [TestClass]
    public class SearchSpatialTests
    {
        [TestMethod]
        public void FindClosest()
        {
            var points = new List<TestPoint>
            {
                new TestPoint(-3358, 5033, 1038),
                new TestPoint(-3324, 5039, 1021),
            };

            var closestPoint = points.FindClosest(new Vector3(-3326, 5036, 1022));

            Assert.AreSame(points[1], closestPoint);
        }

        [TestMethod]
        public void FindClosest2()
        {
            var points = new List<TestPoint>
            {
                new TestPoint(-10, 10, 10),
                new TestPoint(10, 10, 10),
                new TestPoint(20, 10, 10),
            };

            var closestPoint = points.FindClosest(new Vector3(5, 10, 10));

            Assert.AreSame(points[1], closestPoint);
        }

        /// <summary>
        /// Quaternion calculated from: https://quaternions.online/
        /// </summary>
        [TestMethod]
        public void Contains()
        {
            var box = new TestBox(5, 5, 5)
            {
                Size = new Vector3Int(1, 2, 2),
                //Rotation = new Quaternion(0, 0.383f, 0, 0.924f), // 45 degrees
                //Rotation = new Quaternion(0, -0.924f , 0, -0.383f), // 135 degrees
                Rotation = new Quaternion(0, -0.383f, 0, 0.924f), // -45 degrees
            };

            var rectangle = SearchSpatial.CreateRectangle(box);

            //var p1 = new Vector3(4, 5, 4);
            var p1 = new Vector3(6, 5, 2.7f);

            var p2 = new Vector3(3, 5, 6);

            bool p1Contained = SearchSpatial.Contains(box, p1);
            bool p2Contained = SearchSpatial.Contains(box, p2);

            Assert.IsFalse(p1Contained);
            Assert.IsTrue(p2Contained);
        }

        /// <summary>
        /// Quaternion calculated from: https://quaternions.online/
        /// </summary>
        [TestMethod]
        [DataRow(3f, 5f, false)]
        [DataRow(5f, 3.4f, false)]
        [DataRow(5f, 4.4f, true)]
        [DataRow(5f, 7f, false)]
        [DataRow(6f, 2.9f, false)]
        [DataRow(6f, 3f, false)]
        [DataRow(6f, 4f, true)]
        [DataRow(6f, 6f, false)]
        [DataRow(8f, 5f, false)]
        public void Contains2(float x, float y, bool contained)
        {
            var box = new TestBox(5, 5, 5)
            {
                Size = new Vector3Int(1, 2, 2),
                Rotation = new Quaternion(0, -0.383f, 0, 0.924f), // -45 degrees
            };

            var rectangle = SearchSpatial.CreateRectangle(box);

            var p1 = new Vector3(x, 5, y);

            bool p1Contained = SearchSpatial.Contains(box, p1);

            Assert.AreEqual(contained, p1Contained);
        }


        private class TestPoint : IPoint, IHaveVector3
        {
            public Vector3 Pos { get; }

            public float X { get; set; }
            public float Y { get; set; }

            public TestPoint(float x, float y, float z)
            {
                X = x;
                Y = z;

                Pos = new Vector3(X, y, z);
            }
        }

        private class TestBox : IPoint, IHaveVector3, IBox
        {
            public Vector3 Pos { get; }

            public float X { get; set; }
            public float Y { get; set; }

            public Quaternion Rotation { get; set; }

            public Vector3Int Size { get; set; }

            public TestBox(float x, float y, float z)
            {
                X = x;
                Y = z;

                Pos = new Vector3(X, y, z);
            }
        }
    }
}
