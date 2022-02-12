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
                new TestPoint(-1065.6f, 31.3f, 2821.5f),
                new TestPoint(-1064.2f, 32.9f, 2811.9f),
            };

            var closestPoint = points.FindClosest(new Vector3(-1063.7f, 33.2f, 2813.0f));

            Assert.AreSame(points[1], closestPoint);
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
    }
}
