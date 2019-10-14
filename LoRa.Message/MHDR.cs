using System;

namespace LoRa.Message
{
    /// <summary>
    /// MAC Header (MHDR field).
    /// The MAC header specifies the message type (MType) and according to which major version
    /// (Major) of the frame format of the LoRaWAN layer specification the frame has been encoded.
    /// </summary>
    public sealed class MHDR : PayloadPartBase<PHYPayload>
    {
        public override Span<byte> RawData => Parent.RawData.Slice(0, 1);

        /// <summary>
        /// Message type (MType bit field)
        /// </summary>
        public MType MType => (MType)(Parent.RawData[0] >> 5);
        public byte RFU => (byte)((Parent.RawData[0] >> 2) & 0x07);
        /// <summary>
        /// Major version of data message (Major bit field). 00 - LoRaWAN R1, 01..11 - RFU
        /// </summary>
        public byte Major => (byte)(Parent.RawData[0] & 0x03);

        public MessageType MessageType =>
            MType == MType.JoinRequest ? MessageType.JoinRequest :
            MType == MType.JoinAccept ? MessageType.JoinAccept :
            MType == MType.ConfirmedDataDown || MType == MType.ConfirmedDataUp || MType == MType.UnconfirmedDataDown || MType == MType.UnconfirmedDataUp ? MessageType.Data :
            MessageType.Proprietary;

        public LinkDirection LinkDirection =>
            MType == MType.ConfirmedDataUp || MType == MType.UnconfirmedDataUp ? LinkDirection.Up :
            MType == MType.ConfirmedDataDown || MType == MType.UnconfirmedDataDown ? LinkDirection.Down :
            LinkDirection.Join;

        public MHDR(IPayloadPart parent) : base(parent)
        { /* NOP */ }

    }

}