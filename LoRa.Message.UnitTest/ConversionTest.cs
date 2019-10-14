using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoRa.Message.UnitTest
{
    [TestClass]
    public class ConversionTest
    {
        [DataTestMethod]
        [DataRow("0102030405060708090A0B0C0D", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        public void ByteArrayToHexStringTest(string expected, byte[] data)
        {
            Assert.AreEqual(expected, data.ToHexString());
        }

        [DataTestMethod]
        [DataRow("0102030405060708090A0B0C0D", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, 0, 13)]
        [DataRow("030405060708090A0B0C", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, 2, 10)]
        public void SpanToHexStringTest(string expected, byte[] data, int start, int length)
        {
            Assert.AreEqual(expected, (new Span<byte>(data, start, length)).ToHexString());
        }

    }
}
