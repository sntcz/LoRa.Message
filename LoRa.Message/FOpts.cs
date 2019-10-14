using System;

namespace LoRa.Message
{
    public sealed class FOpts : PayloadPartBase<FHDR>
    {
        public override Span<byte> RawData => Parent.RawData.Slice(7, Parent.Length - 7);
        public FOpts(IPayloadPart parent) : base(parent)
        { /* NOP */ }
    }
}