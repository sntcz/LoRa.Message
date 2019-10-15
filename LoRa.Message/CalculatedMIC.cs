using System;
using LoRa.Message.Crypto;

namespace LoRa.Message
{
    public sealed class CalculatedMIC
    {

        public int FCntMSB { get; }
        public byte[] RawData { get; }
        public bool IsValid { get; }

        public CalculatedMIC(DataMessage dataMessage, byte[] nwkSKey, int fCntMsbSeed)
        {
            if (nwkSKey != null && nwkSKey.Length == 16)
            {
                CryptoService crypto = new CryptoService();
                B0Message b0msg = new B0Message(dataMessage)
                {
                    FCntMSB = fCntMsbSeed
                };

                for (int i = 0; i <= ushort.MaxValue; i++)
                {
                    byte[] fullCAMC = crypto.AESCMAC(nwkSKey, b0msg.Data);
                    if (fullCAMC.AsSpan(0, 4).SequenceCompareTo(dataMessage.Parent.Mic.RawData) == 0)
                    {
                        FCntMSB = b0msg.FCntMSB;
                        RawData = fullCAMC.AsSpan(0, 4).ToArray();
                        IsValid = true;
                        return;
                    }
                    b0msg.FCntMSB = (b0msg.FCntMSB + 1) & 0xFFFF;
                }
            }
            RawData = Array.Empty<byte>();
            IsValid = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="joinAcceptMessage"></param>
        /// <param name="appKey"></param>
        public CalculatedMIC(JoinRequestMessage joinRequestMessage, byte[] appKey)
        {
            if (appKey != null && appKey.Length == 16)
            {
                CryptoService crypto = new CryptoService();
                // cmac = aes128_cmac(AppKey, MHDR | AppEUI | DevEUI | DevNonce)
                // MIC = cmac[0..3]
                byte[] data = new byte[joinRequestMessage.RawData.Length + 1];
                joinRequestMessage.Parent.Mhdr.RawData.CopyTo(data.AsSpan()); // [1]
                joinRequestMessage.RawData.CopyTo(data.AsSpan(1));
                byte[] fullCAMC = crypto.AESCMAC(appKey, data);
                RawData = fullCAMC.AsSpan(0, 4).ToArray();
                IsValid = true;
            }
            else
            {
                RawData = Array.Empty<byte>();
                IsValid = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="joinAcceptMessage"></param>
        /// <param name="appKey"></param>
        public CalculatedMIC(JoinAcceptMessage joinAcceptMessage, byte[] appKey)
        {
            if (appKey != null && appKey.Length == 16)
            {
                CryptoService crypto = new CryptoService();
                // cmac = aes128_cmac(AppKey, MHDR | AppNonce | NetID | DevAddr | DLSettings | RxDelay | CFList)
                // MIC = cmac[0..3]
                byte[] data = new byte[joinAcceptMessage.RawData.Length + 1]; 
                joinAcceptMessage.Parent.Mhdr.RawData.CopyTo(data.AsSpan()); // [1]
                joinAcceptMessage.RawData.CopyTo(data.AsSpan(1)); 
                byte[] fullCAMC = crypto.AESCMAC(appKey, data);
                RawData = fullCAMC.AsSpan(0, 4).ToArray();
                IsValid = true;
            }
            else
            {
                RawData = Array.Empty<byte>();
                IsValid = false;
            }

        }
    }
}