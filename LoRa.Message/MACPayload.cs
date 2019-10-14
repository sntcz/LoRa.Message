using System;

namespace LoRa.Message
{
    public abstract class MACPayload : PayloadPartBase<PHYPayload>, IContentPart
    {
        public override Span<byte> RawData => Parent.RawData.Slice(1, Parent.RawData.Length - 5);

        protected MACPayload(IPayloadPart parent) : base(parent)
        { /* NOP */ }

        public abstract string ToVerboseString();
    }
}