using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace LoRa.Message
{
    public sealed class PHYPayload : IContentPart, IPayloadPart
    {
        public byte[] Packet { get; }
        public Span<byte> RawData => Packet.AsSpan();

        public MHDR Mhdr { get; }
        public MACPayload MacPayload { get; }
        public MIC Mic { get; }

        public PHYPayload(byte[] packet) : this(packet, null, null, null, 0)
        { /*NOP */ }

        public PHYPayload(byte[] packet, byte[] nwkSKey, byte[] appSKey, byte[] appKey, int fCntMsbSeed)
        {
            Packet = packet;
            Mhdr = new MHDR(this);
            switch (Mhdr.MessageType)
            {
                case MessageType.JoinRequest:
                    MacPayload = new JoinRequestMessage(this, appKey);
                    break;
                case MessageType.JoinAccept:
                    MacPayload = new JoinAcceptMessage(this, appKey);
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

          byte[] CMAC()
        {
            switch (Mhdr.MessageType)
            {
                case MessageType.JoinRequest:
                    throw new NotImplementedException();
                    break;
                case MessageType.JoinAccept:
                    JoinAcceptMessage payload = (JoinAcceptMessage)MacPayload;
                   
                    byte[] cmac = new byte[1 +
                                                           payload.AppNonce.RawData.Length +
                                                           payload.NetID.RawData.Length +
                                                           payload.DevAddr.RawData.Length +
                                                           1 +
                                                           1
                                                           ];
                    Crypto.CryptoService service = new Crypto.CryptoService();
                    System.Buffer.BlockCopy(Mhdr.RawData.ToArray(), 0, cmac, 0, Mhdr.RawData.Length);
                    System.Buffer.BlockCopy(payload.AppNonce.RawData.ToArray(), 0, cmac, Mhdr.RawData.Length, payload.AppNonce.RawData.Length);
                    System.Buffer.BlockCopy(payload.NetID.RawData.ToArray(), 0, cmac, Mhdr.RawData.Length + payload.AppNonce.RawData.Length, payload.NetID.RawData.Length);
                    System.Buffer.BlockCopy(payload.DevAddr.RawData.ToArray(), 0, cmac, Mhdr.RawData.Length + payload.AppNonce.RawData.Length + payload.NetID.RawData.Length, payload.DevAddr.RawData.Length);
                    System.Buffer.BlockCopy(payload.DLSettings.RawData.ToArray(), 0, cmac, Mhdr.RawData.Length + payload.AppNonce.RawData.Length + payload.NetID.RawData.Length + payload.DevAddr.RawData.Length, payload.DLSettings.RawData.Length);
                    System.Buffer.BlockCopy(payload.RxDelay.RawData.ToArray(), 0, cmac, Mhdr.RawData.Length + payload.AppNonce.RawData.Length + payload.NetID.RawData.Length + payload.DevAddr.RawData.Length + payload.DLSettings.RawData.Length, payload.RxDelay.RawData.Length);
                    return cmac;
                    break;
                case MessageType.Data:
                    throw new NotImplementedException();
                    break;
                case MessageType.Proprietary:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

          byte[] SKey()
        {
            switch (Mhdr.MessageType)
            {
                case MessageType.JoinRequest:
                    throw new NotImplementedException();
                    break;
                case MessageType.JoinAccept:
                    JoinAcceptMessage payload = (JoinAcceptMessage)MacPayload;
                    byte[] skey = new byte[payload.AppNonce.RawData.Length +
                    payload.NetID.RawData.Length +
                   (payload.joinRequestMessage.DevNonce.Value.Length / 2) + ("00000000000000".Length / 2)];

                    System.Buffer.BlockCopy(payload.AppNonce.RawData.ToArray(), 0, skey, 0, payload.AppNonce.RawData.Length);
                    System.Buffer.BlockCopy(payload.NetID.RawData.ToArray(), 0, skey, payload.AppNonce.RawData.Length, payload.NetID.RawData.Length);
                    System.Buffer.BlockCopy(payload.joinRequestMessage.DevNonce.Value.FromHexString().Reverse().ToArray(), 0, skey, payload.AppNonce.RawData.Length + payload.NetID.RawData.Length, payload.joinRequestMessage.DevNonce.Value.Length / 2);

                    System.Buffer.BlockCopy(("00000000000000").FromHexString(), 0, skey, payload.AppNonce.RawData.Length + payload.NetID.RawData.Length + (payload.joinRequestMessage.DevNonce.Value.Length / 2), "00000000000000".Length / 2);
                    return skey;
                    break;
                case MessageType.Data:
                    throw new NotImplementedException();
                    break;
                case MessageType.Proprietary:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public byte[] ToBytes( byte[] AppKey, byte[] appKey)
        {
            Crypto.CryptoService service = new Crypto.CryptoService();
            switch (Mhdr.MessageType)
            {
                case MessageType.JoinRequest:
                    throw new NotImplementedException();
                    break;
                case MessageType.JoinAccept:
                    JoinAcceptMessage payload = (JoinAcceptMessage)MacPayload;
                    byte[] mic = service.AESCMAC(AppKey, CMAC()).AsSpan<byte>().Slice(0, 4).ToArray();

                    byte[] skey = SKey();
                    byte[] nskey = new byte[skey.Length + 1];
                    byte[] askey = new byte[skey.Length + 1];

                    System.Buffer.BlockCopy(new byte[] { 01 }, 0, nskey, 0, 1);
                    System.Buffer.BlockCopy(skey, 0, nskey, 1, skey.Length);

                    System.Buffer.BlockCopy(new byte[] { 02 }, 0, askey, 0, 1);
                    System.Buffer.BlockCopy(skey, 0, askey, 1, skey.Length);

                    var nwkSKey = service.AESEncrypt(AppKey, nskey);
                    var appSKey = service.AESEncrypt(AppKey, askey);

                    string nwkSKeyst = nwkSKey.ToHexString();
                    string appSKeyst = appSKey.ToHexString();


                         
                        List<byte> txdatabytes = new List<byte>();
                        txdatabytes.AddRange(Mhdr.RawData.ToArray());

                        List<byte> decodedtxdatabytes = new List<byte>();
                        decodedtxdatabytes.AddRange(payload.AppNonce.RawData.ToArray());
                        decodedtxdatabytes.AddRange(payload.NetID.RawData.ToArray());
                        decodedtxdatabytes.AddRange(payload.DevAddr.RawData.ToArray());
                        decodedtxdatabytes.AddRange(payload.DLSettings.RawData.ToArray());
                        decodedtxdatabytes.AddRange(payload.RxDelay.RawData.ToArray());

                        decodedtxdatabytes.AddRange(mic);

                        txdatabytes.AddRange(service.AESDecrypt(AppKey, decodedtxdatabytes.ToArray()));

                      return txdatabytes.ToArray();


                    break;
                case MessageType.Data:
                    throw new NotImplementedException();
                    break;
                case MessageType.Proprietary:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }



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