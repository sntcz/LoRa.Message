using System;
using System.Collections.Generic;
using System.Text;

namespace LoRa.Message.Crypto
{
    public class B0Message
    {
        public byte[] Data { get; }

        public int FCntMSB
        {
            get
            {
                return ((Data[13] << 8) + Data[12]);
            }
            set
            {
                Data[12] = (byte)(value & 0xFF); //BitConverter.GetBytes(i)[0];
                Data[13] = (byte)((value >> 8) & 0xFF); //BitConverter.GetBytes(i)[1];
            }
        }

        public B0Message(DataMessage dataMessage)
        {
            Data = new byte[16 + dataMessage.Parent.RawData.Length - 4];
            // B0
            Data[0] = 0x49;
            Data[1] =
                Data[2] =
                Data[3] =
                Data[4] = 0;
            Data[5] = (byte)(dataMessage.Parent.Mhdr.LinkDirection == LinkDirection.Up ? 0 : 1);
            dataMessage.Fhdr.DevAddr.RawData.CopyTo(Data.AsSpan(6, 4));
            dataMessage.Fhdr.FCnt.RawData.CopyTo(Data.AsSpan(10, 2));
            Data[12] =
                Data[13] = 0;
            Data[14] = 0;
            Data[15] = (byte)(dataMessage.Parent.RawData.Length - 4);
            dataMessage.Parent.Mhdr.RawData.CopyTo(Data.AsSpan(16, 1));
            dataMessage.Parent.MacPayload.RawData.CopyTo(Data.AsSpan(17));
        }
    }
}
