using System;
using LoRa.Message.Crypto;

namespace LoRa.Message
{
    /// <summary>
    /// Message Integrity Code (MIC)
    /// </summary>
    public sealed class MIC : PayloadPartBase<PHYPayload>
    {
        public override Span<byte> RawData => Parent.RawData.Slice(Parent.RawData.Length - 4, 4);
        public MIC(IPayloadPart parent) : base(parent)
        { /* NOP */ }
    }
}