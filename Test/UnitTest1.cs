using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;

namespace Test
{
    [TestClass]
    public class Core
    {
        private const string Expected = "";
        [TestMethod]
        public void TestMethod1()
        {
            byte[] bytes = { 10, 0, 11 };
            BuildProsses x = new BuildProsses(bytes);
            x.InjectBin = bytes;
            string outp = x.InjectCode;
            //Assert.AreEqual(Expected, outp);
        }
    }
}
