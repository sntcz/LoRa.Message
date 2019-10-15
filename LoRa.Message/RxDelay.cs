using System;
using System.Text;

namespace LoRa.Message
{
    public sealed class RxDelay : PayloadPartBase<JoinAcceptMessage>
    {
        public override Span<byte> RawData => Parent.RawData.Slice(11, 1);
        public byte RFU => (byte)((RawData[0] >> 4) & 0x0F);
        public byte Del => (byte)((RawData[0]) & 0x0F);

        public RxDelay(IPayloadPart parent) : base(parent)
        { /* */ }

        public string ToVerboseString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("     RxDelay :").AppendLine();
            sb.AppendFormat("        .Del = {0}", Del).AppendLine();
            sb.AppendFormat("        .RFU = {0}", RFU).AppendLine();
            return sb.ToString();
        }

    }
}