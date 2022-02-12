using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpawnThat.Core.Network;
using SpawnThat.Lifecycle;
using SpawnThat.World.Locations;

namespace SpawnThat.Tests.World.Locations;

[TestClass]
public class SimpleLocationPackageTests
{
    [TestMethod]
    public void CanSync()
    {
        try
        {
            var locations = new List<SimpleLocation>();

            for(int i = 0; i < 10000; ++i)
            {
                locations.Add(new()
                {
                    LocationName = "MyLocation" + i,
                    Position = new UnityEngine.Vector3(i*1.3f, i*1.7f),
                    ZonePosition = new Vector2i(i,i),
                });
            }

            LocationManager.SetLocations(locations);

            var package = new SimpleLocationPackage();

            var serialized = package.Pack();

            // Reset stream head, to simulate transfer
            serialized.m_stream.Position = 0L;

            CompressedPackage.Unpack<SimpleLocationPackage>(serialized);

            Assert.IsNotNull(LocationManager.GetLocations());
        }
        finally
        {
         
        }
    }
}
