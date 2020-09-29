using System;
using System.Text;

namespace LoRa.Message
{
    public sealed class FHDR : PayloadPartBase<DataMessage>, IContentPart
    {
        public int Length { get; set; }
        public DevAddr DevAddr { get; }
        public FCtrl FCtrl { get; }
        public FCnt FCnt { get; }
        public FOpts FOpts { get; }
        public override Span<byte> RawData => Parent.RawData.Slice(0, Length);
        public FHDR(IPayloadPart parent) : base(parent)
        {
            Length = 7 + (Parent.RawData[4] & 0x0f);
            DevAddr = new DevAddr(this);
            FCtrl = new FCtrl(this);
            FCnt = new FCnt(this);
            FOpts = new FOpts(this);
        }

        public string ToVerboseString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("       (FHDR = DevAddr[4] | FCtrl[1] | FCnt[2] | FOpts[0..15])").AppendLine();
            sb.AppendFormat("     DevAddr = {0} (Big Endian)", DevAddr.Address).AppendLine();
            sb.AppendFormat("       FCtrl = {0}", FCtrl.RawData.ToHexString()).AppendLine();
            sb.AppendFormat("        FCnt = {0} 0x{0:X4} (Big Endian)", FCnt.Value).AppendLine();
            sb.AppendFormat("       FOpts = {0}", FOpts.RawData.ToHexString()).AppendLine();
            sb.AppendLine();
            sb.Append(FCtrl.ToVerboseString());
            return sb.ToString();
        }
    }
}