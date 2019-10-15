using System;
using System.Text;

namespace LoRa.Message
{
    public sealed class DLsettings : PayloadPartBase<JoinAcceptMessage>
    {
        public override Span<byte> RawData => Parent.RawData.Slice(10, 1);
        public byte RFU => (byte)((RawData[0] >> 7) & 0x01);
        public byte RX1DRoffset => (byte)((RawData[0] >> 4) & 0x07);
        public byte RX2DataRate => (byte)((RawData[0]) & 0x0F);

        public DLsettings(IPayloadPart parent) : base(parent)
        { /* */ }

        public string ToVerboseString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("  DLsettings :").AppendLine();
            sb.AppendFormat(".RX1DRoffset = {0}", RX1DRoffset).AppendLine();
            sb.AppendFormat(".RX2DataRate = {0}", RX2DataRate).AppendLine();
            sb.AppendFormat("        .RFU = {0}", RFU).AppendLine();
            return sb.ToString();
        }

    }
}