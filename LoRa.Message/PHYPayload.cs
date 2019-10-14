using System;
using System.Text;

namespace LoRa.Message
{
    public sealed class PHYPayload : IContentPart, IPayloadPart
    {
        public byte[] Packet { get; }
        public Span<byte> RawData => Packet.AsSpan();

        public MHDR Mhdr { get; }
        public MACPayload MacPayload { get; }
        public MIC Mic { get; }

        public PHYPayload(byte[] packet) : this(packet, null, null, 0)
        { /*NOP */ }

        public PHYPayload(byte[] packet, byte[] nwkSKey, byte[] appSKey, int fCntMsbSeed)
        {
            Packet = packet;
            Mhdr = new MHDR(this);
            switch (Mhdr.MessageType)
            {
                case MessageType.JoinRequest:
                    MacPayload = new JoinRequestMessage(this);
                    break;
                case MessageType.JoinAccept:
                    MacPayload = new JoinAcceptMessage(this);
                    break;
                case MessageType.Data:
                    MacPayload = new DataMessage(this, nwkSKey, appSKey, fCntMsbSeed);
                    break;
                case MessageType.Proprietary:
                    throw new NotSupportedException();
                default:
                    throw new NotSupportedException();
            }
            Mic = new MIC(this);
        }

        public string ToVerboseString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Message Type = {0}", Mhdr.MessageType).AppendLine();
            sb.AppendFormat("  PHYPayload = {0}", Packet.ToHexString()).AppendLine();
            sb.AppendLine();
            sb.AppendFormat(" (PHYPayload = MHDR[1] | MACPayload[..] | MIC[4])").AppendLine();
            sb.AppendFormat("        MHDR = {0}", Mhdr.RawData.ToHexString()).AppendLine();
            sb.AppendFormat("  MACPayload = {0}", MacPayload.RawData.ToHexString()).AppendLine();
            sb.AppendFormat("         MIC = {0}", Mic.RawData.ToHexString()).AppendLine();
            sb.AppendLine();
            sb.Append(MacPayload.ToVerboseString());
            return sb.ToString();
        }
    }
}