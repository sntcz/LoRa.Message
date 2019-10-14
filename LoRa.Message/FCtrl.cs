using System;
using System.Text;

namespace LoRa.Message
{
    public sealed class FCtrl : PayloadPartBase<FHDR>
    {
        public bool ADR => (RawData[0] & 0x80) != 0;
        public bool RFU => (RawData[0] & 0x40) != 0;
        public bool ADRACKReq => (RawData[0] & 0x40) != 0;
        public bool ACK => (RawData[0] & 0x20) != 0;
        public bool FPending => (RawData[0] & 0x10) != 0;
        public bool ClassB => (RawData[0] & 0x10) != 0;


        public int FOptsLen => RawData[0] & 0x0F;
        public override Span<byte> RawData => Parent.RawData.Slice(4, 1);
        public FCtrl(IPayloadPart parent) : base(parent)
        { /* NOP */ }

        public string ToVerboseString()
        {
            LinkDirection direction = Parent.Parent.Parent.Mhdr.LinkDirection;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("       FCtrl :").AppendLine();
            sb.AppendFormat("        .ADR = {0}", ADR).AppendLine();
            sb.AppendFormat("        .ACK = {0}", ACK).AppendLine();
            if (direction == LinkDirection.Down)
            {
                sb.AppendFormat("        .RFU = {0}", RFU).AppendLine();
                sb.AppendFormat("   .FPending = {0}", FPending).AppendLine();
            }
            if (direction == LinkDirection.Up)
            {
                sb.AppendFormat("  .ADRACKReq = {0}", ADRACKReq).AppendLine();
                sb.AppendFormat("     .ClassB = {0}", ClassB).AppendLine();
            }
            return sb.ToString();
        }
    }
}