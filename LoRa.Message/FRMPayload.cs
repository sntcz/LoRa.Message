using System;

namespace LoRa.Message
{
    public sealed class FRMPayload : PayloadPartBase<DataMessage>
    {
        public override Span<byte> RawData => (Parent.RawData.Length > Parent.Fhdr.Length + 1) ? Parent.RawData.Slice(Parent.Fhdr.Length + 1) : Span<byte>.Empty;

        /// <summary>
        /// NwkSKey
        /// </summary>
        private readonly byte[] NwkSKey;
        /// <summary>
        /// AppSKey
        /// </summary>
        private readonly byte[] AppSKey;


        public FRMPayload(IPayloadPart parent, byte[] nwkSKey, byte[] appSKey) : base(parent)
        {
            NwkSKey = nwkSKey;
            AppSKey = appSKey;
        }

        public byte[] Decrypt()
        {
            Crypto.CryptoService crypto = new Crypto.CryptoService();

            byte[] key = Parent.FPort.Value == 0 ? NwkSKey : AppSKey;
            int blocks = RawData.Length / 16;
            if (RawData.Length % 16 != 0)
                blocks++;
            byte[] plainS = new byte[blocks * 16];
            for (int i = 0; i < blocks; i++)
            {
                Ai(plainS.AsSpan().Slice(i * 16, 16), i + 1);
            }
            byte[] cipher = crypto.AESEncrypt(key, plainS);
            byte[] data = RawData.ToArray(); ;

            for (var j = 0; j < data.Length; j++)
            {
                data[j] = (byte)(data[j] ^ cipher[j]);
            }
            return data;
        }

        private void Ai(Span<byte> block, int i)
        {
            block[0] = 0x01;
            block[1] =
                block[2] =
                block[3] =
                block[4] = 0;
            block[5] = (byte)(Parent.Parent.Mhdr.LinkDirection == LinkDirection.Up ? 0 : 1);
            ((DataMessage)Parent.Parent.MacPayload).Fhdr.DevAddr.RawData.CopyTo(block.Slice(6, 4));
            ((DataMessage)Parent.Parent.MacPayload).Fhdr.FCnt.RawData.CopyTo(block.Slice(10, 2));
            //block[12] = (byte)(((DataMessage)Parent.Parent.MacPayload).CalculatedMIC.FCntMSB & 0xFF);
            //block[13] = (byte)((((DataMessage)Parent.Parent.MacPayload).CalculatedMIC.FCntMSB >> 8) & 0xFF);
            block[14] = 0;
            block[15] = (byte)(i);

        }
    }
}