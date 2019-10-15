using System;

namespace LoRa.Message
{
    public sealed class DevEUI : PayloadPartBase<JoinRequestMessage>
    {
        public override Span<byte> RawData => Parent.RawData.Slice(8, 8);
        public string Value => RawData.ToReverseHexString();
        public DevEUI(IPayloadPart parent) : base(parent)
        { /* */ }
    }
}