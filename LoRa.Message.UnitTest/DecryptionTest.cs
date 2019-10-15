using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoRa.Message.UnitTest
{
    [TestClass]
    public class DecryptionTest
    {

        [DataTestMethod]
        [DataRow("40F17DBE4900020001954378762B11FF0D", "44024241ED4CE9A68C6A8BC055233FD3", "EC925802AE430CA77FD3DD73CB2CC588", "test")]
        [DataRow("40F17DBE490004000155332DE41A11ADC072553544429CE7787707D1C316E027E7E5E334263376AFFB8AA17AD30075293F28DEA8A20AF3C5E7", 
            "44024241ED4CE9A68C6A8BC055233FD3", "EC925802AE430CA77FD3DD73CB2CC588", "The quick brown fox jumps over the lazy dog.")]
        public void DecryptPacketTest(string hexData, string nwkSKey, string appSKey, string expected)
        {
            PHYPayload packet = new PHYPayload(hexData.FromHexString(), nwkSKey.FromHexString(), appSKey.FromHexString(), null, 0);
            Assert.AreEqual(expected, System.Text.Encoding.ASCII.GetString(((DataMessage)packet.MacPayload).FrmPayload.Decrypt()));
        }

    }
}
