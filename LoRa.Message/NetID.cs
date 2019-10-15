using System;

namespace LoRa.Message
{
    public sealed class NetID : PayloadPartBase<JoinAcceptMessage>
    {
        public override Span<byte> RawData => Parent.RawData.Slice(3, 3);
        public string Value => RawData.ToReverseHexString();
        public NetID(IPayloadPart parent) : base(parent)
        { /* */ }

    }
}