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

        [DataTestMethod]
        [DataRow("40F17DBE4900020001954378762B11FF0D", "40F17DBE4900020001954378762B11FF0D", "40", "F17DBE490002000195437876", "2B11FF0D",
            "", "00", "F17DBE49000200", "49BE7DF1", 2, 1, "95437876")]
        [DataRow("40F17DBE49000300012A3518AF", "40F17DBE49000300012A3518AF", "40", "F17DBE4900030001", "2A3518AF",
            "", "00", "F17DBE49000300", "49BE7DF1", 3, 1, "", DisplayName = "Empty payload")]
        [DataRow("40F17DBE490004000155332DE41A11ADC072553544429CE7787707D1C316E027E7E5E334263376AFFB8AA17AD30075293F28DEA8A20AF3C5E7",
            "40F17DBE490004000155332DE41A11ADC072553544429CE7787707D1C316E027E7E5E334263376AFFB8AA17AD30075293F28DEA8A20AF3C5E7", "40",
            "F17DBE490004000155332DE41A11ADC072553544429CE7787707D1C316E027E7E5E334263376AFFB8AA17AD30075293F28DEA8A2", "0AF3C5E7",
            "", "00", "F17DBE49000400", "49BE7DF1", 4, 1, "55332DE41A11ADC072553544429CE7787707D1C316E027E7E5E334263376AFFB8AA17AD30075293F28DEA8A2",
            DisplayName = "Large data packet")]
        [DataRow("40061fc09a865a0303070307030701ece1102559e0153802027f281db84267f6483bebd2c96cbf96483b878aa4300aa0000e6b7b9dfcad5e574742d15844b37c1a8f308e5bb7be80",
            "40061FC09A865A0303070307030701ECE1102559E0153802027F281DB84267F6483BEBD2C96CBF96483B878AA4300AA0000E6B7B9DFCAD5E574742D15844B37C1A8F308E5BB7BE80", "40",
            "061FC09A865A0303070307030701ECE1102559E0153802027F281DB84267F6483BEBD2C96CBF96483B878AA4300AA0000E6B7B9DFCAD5E574742D15844B37C1A8F308E", "5BB7BE80",
            "030703070307", "86", "061FC09A865A03030703070307", "9AC01F06", 858, 1, "ECE1102559E0153802027F281DB84267F6483BEBD2C96CBF96483B878AA4300AA0000E6B7B9DFCAD5E574742D15844B37C1A8F308E",
            DisplayName = "Frame Header with Frame Options")]
        public void ParsingDataMessage(string hexData, string phyPayload, string mhdr, string macPayload, string mic,
            string fopts, string fctrl, string fhdr, string devAddr, int fcnt, int fport, string frmPayload)
        {
            PHYPayload payload = new PHYPayload(hexData.FromHexString());

            Assert.AreEqual(phyPayload, payload.RawData.ToHexString());
            Assert.AreEqual(mhdr, payload.Mhdr.RawData.ToHexString());
            Assert.AreEqual(macPayload, payload.MacPayload.RawData.ToHexString());
            Assert.AreEqual(mic, payload.Mic.RawData.ToHexString());
            DataMessage dataMessage = (DataMessage)payload.MacPayload;
            Assert.AreEqual(fopts, dataMessage.Fhdr.FOpts.RawData.ToHexString());
            Assert.AreEqual(fctrl, dataMessage.Fhdr.FCtrl.RawData.ToHexString());
            Assert.AreEqual(fhdr, dataMessage.Fhdr.RawData.ToHexString());
            Assert.AreEqual(devAddr, dataMessage.Fhdr.DevAddr.Address);
            Assert.AreEqual(fcnt, dataMessage.Fhdr.FCnt.Value);
            Assert.AreEqual(fport, dataMessage.FPort.Value);
            Assert.AreEqual(frmPayload, dataMessage.FrmPayload.RawData.ToHexString());
        }

    }
}
