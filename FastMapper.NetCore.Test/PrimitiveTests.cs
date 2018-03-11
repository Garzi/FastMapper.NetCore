using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FastMapper.NetCore.Adapters;

namespace FastMapper.NetCore.Test
{
    [TestClass]
    public class PrimitiveTests
    {
        [TestMethod]
        public void TestPrimitiveTypes()
        {
            byte b = PrimitiveAdapter<int, byte>.Adapt(5);

            Assert.IsTrue(b == 5);
        }
    }
}
