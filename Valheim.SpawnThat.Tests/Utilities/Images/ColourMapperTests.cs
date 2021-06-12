using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SpawnThat.Utilities.Images
{
    [TestClass]
    public class ColourMapperTests
    {
        [TestMethod]
        public void IntegerToColour255()
        {
            var (r, g, b) = ColourMapper.IntegerToColor255(0);

            Assert.AreEqual(0, r);
            Assert.AreEqual(0, g);
            Assert.AreEqual(0, b);
        }

        [TestMethod]
        public void IntegerToColour255_WhenIdAboveByte_ShouldCarryRemainder()
        {
            var (r, g, b) = ColourMapper.IntegerToColor255(300);

            Assert.AreEqual(0, r);
            Assert.AreEqual(300 - 255, g);
            Assert.AreEqual(255, b);
        }
    }
}
