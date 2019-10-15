using System;

namespace LoRa.Message
{
    public sealed class DevAddr : PayloadPartBase<IPayloadPart>
    {
        int start = 0;
        public override Span<byte> RawData => Parent.RawData.Slice(start, 4);
        public string Address => RawData.ToReverseHexString();
        public DevAddr(IPayloadPart parent) : base(parent)
        {
            if (parent is FHDR)
                start = 0;
            else if (parent is JoinAcceptMessage)
                start = 6;
            else
                throw new InvalidOperationException("Unsupported parent of DevAddr");
        }
    }
}