using System;

namespace LoRa.Message
{
    public sealed class FCnt : PayloadPartBase<FHDR>
    {
        public override Span<byte> RawData => Parent.RawData.Slice(5, 2);
        public int Value => RawData[0] + (RawData[1] << 8);
        public FCnt(IPayloadPart parent) : base(parent)
        { /* NOP */ }
    }
}