using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoRa.Message.UnitTest
{
    [TestClass]
    public class CalculatedMICTest
    {

        [DataTestMethod]
        [DataRow("40F17DBE4900020001954378762B11FF0D", "44024241ED4CE9A68C6A8BC055233FD3", "2B11FF0D")]
        [DataRow("40F17DBE49000300012A3518AF", "44024241ED4CE9A68C6A8BC055233FD3", "2A3518AF")]
        [DataRow("40F17DBE4900020001954378762B11FF0D", "44024241ED4CE9A68C6A8BC055233FD3", "2B11FF0D", DisplayName = "32-bit FCnts are used")]
        public void CalculateMICTest(string hexData, string nwkSKey, string expected)
        {
            PHYPayload packet = new PHYPayload(hexData.FromHexString(), nwkSKey.FromHexString(), null, 0);
            Assert.AreEqual(expected, ((DataMessage)packet.MacPayload).CalculatedMIC.RawData.ToHexString());
        }

        [DataTestMethod]
        [DataRow("QFMeASaAZkYBRXCQ7SU=", "7A47F143D7CEF033DFA0D4B75E04A316", 0, 7, DisplayName = "Initial seed 0")]
        [DataRow("QFMeASaAZkYBRXCQ7SU=", "7A47F143D7CEF033DFA0D4B75E04A316", 7, 7, DisplayName = "Initial seed 7")]
        [DataRow("QFMeASaAZkYBRXCQ7SU=", "7A47F143D7CEF033DFA0D4B75E04A316", 8, 7, DisplayName = "Initial seed 8")]
        public void SeedFCntMSBTest(string base64, string nwkSKey, int fCntMsbSeed, int expectedSeedResult)
        {
            PHYPayload packet = new PHYPayload(base64.FromBase64String(), nwkSKey.FromHexString(), null, fCntMsbSeed);
            Assert.AreEqual(expectedSeedResult, ((DataMessage)packet.MacPayload).CalculatedMIC.FCntMSB);
        }

    }
}
