using System;

namespace LoRa.Message
{
    public sealed class AppEUI : PayloadPartBase<JoinRequestMessage>
    {
        public override Span<byte> RawData => Parent.RawData.Slice(0, 8);
        public string Value => RawData.ToReverseHexString();
        public AppEUI(IPayloadPart parent) : base(parent)
        { /* */ }
    }
}