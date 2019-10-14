using System;

namespace LoRa.Message
{
    public enum MessageType
    {
        JoinRequest,
        JoinAccept,
        Data,
        Proprietary
    }
}