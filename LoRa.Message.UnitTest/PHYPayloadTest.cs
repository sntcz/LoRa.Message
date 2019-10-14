using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoRa.Message.UnitTest
{
    [TestClass]
    public class PHYPayloadTest
    {
        [DataTestMethod]
        [DataRow("QCkuASaAAAAByFaF53Iu+vzmwQ==", DisplayName = "Unconfirmed Data Up")]
        [DataRow("ANwAANB+1bNwHm/t9XzurwDIhgMK8sk=", DisplayName = "Join Request")]
        public void ToVerboseStringFake(string base64)
        {
            byte[] data = Convert.FromBase64String(base64);
            PHYPayload payload = new PHYPayload(data);
            payload.ToVerboseString();
        }

        [DataTestMethod]
        [DataRow("QCkuASaAAAAByFaF53Iu+vzmwQ==", "40", "292E012680000001C85685E7722E", "FAFCE6C1", DisplayName = "Unconfirmed Data Up")]
        [DataRow("ANwAANB+1bNwHm/t9XzurwDIhgMK8sk=", "00", "DC0000D07ED5B3701E6FEDF57CEEAF00C886", "030AF2C9", DisplayName = "Join Request")]
        public void BasicParsingTest(string base64, string mhdr, string macPayload, string mic)
        {
            byte[] data = Convert.FromBase64String(base64);
            PHYPayload payload = new PHYPayload(data);
            Assert.AreEqual(mhdr, payload.Mhdr.RawData.ToHexString());
            Assert.AreEqual(macPayload, payload.MacPayload.RawData.ToHexString());
            Assert.AreEqual(mic, payload.Mic.RawData.ToHexString());
        }

    }
}
