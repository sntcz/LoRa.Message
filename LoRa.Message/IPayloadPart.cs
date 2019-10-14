using System;

namespace LoRa.Message
{
    public interface IPayloadPart
    {
        Span<byte> RawData { get; }
    }
}
