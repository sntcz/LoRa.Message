using System;

namespace LoRa.Message
{
    public sealed class AppNonce : PayloadPartBase<JoinAcceptMessage>
    {
        public override Span<byte> RawData => Parent.RawData.Slice(0, 3);
        public string Value => RawData.ToReverseHexString();
        public AppNonce(IPayloadPart parent) : base(parent)
        { /* */ }
    }
}