using System;

namespace LoRa.Message
{
    public sealed class DevNonce : PayloadPartBase<JoinRequestMessage>
    {
        public override Span<byte> RawData => Parent.RawData.Slice(16, 2);
        public string Value => RawData.ToReverseHexString();
        public DevNonce(IPayloadPart parent) : base(parent)
        { /* */ }
    }
}