using System;

namespace LoRa.Message
{
    public sealed class DevAddr : PayloadPartBase<FHDR>
    {
        public override Span<byte> RawData => Parent.RawData.Slice(0, 4);
        public string Address => RawData.ToReverseHexString();
        public DevAddr(IPayloadPart parent) : base(parent)
        { /* NOP */ }
    }
}