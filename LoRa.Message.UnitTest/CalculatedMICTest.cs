using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoRa.Message.UnitTest
{
    [TestClass]
    public class CalculatedMICTest
    {
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
