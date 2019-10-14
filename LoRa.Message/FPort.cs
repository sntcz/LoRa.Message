using System;

namespace LoRa.Message
{
    public sealed class FPort : PayloadPartBase<DataMessage>
    {
        public override Span<byte> RawData => Parent.RawData.Slice(Parent.Fhdr.Length, (Parent.RawData.Length > Parent.Fhdr.Length) ? 1 : 0);
        public int Value => RawData.Length == 0 ? 0 : RawData[0];
        public FPort(IPayloadPart parent) : base(parent)
        { /* NOP */ }
    }
}
