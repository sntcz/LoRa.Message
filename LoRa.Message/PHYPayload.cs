using System;
using System.Security.Cryptography;
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
        Span<byte> CMAC()
        {
            switch (Mhdr.MessageType)
            {
                case MessageType.JoinRequest:
                    throw new NotImplementedException();
                case MessageType.JoinAccept:
                    JoinAcceptMessage payload = (JoinAcceptMessage)MacPayload;
                   
                    byte[] cmac = new byte[1 +
                                                           payload.AppNonce.RawData.Length +
                                                           payload.NetID.RawData.Length +
                                                           payload.DevAddr.RawData.Length +
                                                           1 +
                                                           1
                                                           ];
                    Span<byte> bytes = cmac.AsSpan();
                    int offset = 0;
                    Mhdr.RawData.CopyTo(bytes.Slice(offset, Mhdr.RawData.Length));
                    offset+= Mhdr.RawData.Length;
                    payload.AppNonce.RawData.CopyTo(bytes.Slice(offset, payload.AppNonce.RawData.Length));
                    offset += payload.AppNonce.RawData.Length;
                    payload.NetID.RawData.CopyTo(bytes.Slice(offset, payload.NetID.RawData.Length));
                    offset += payload.NetID.RawData.Length;
                    payload.DevAddr.RawData.CopyTo(bytes.Slice(offset, payload.DevAddr.RawData.Length));
                    offset += payload.DevAddr.RawData.Length;
                    payload.DLSettings.RawData.CopyTo(bytes.Slice(offset, payload.DLSettings.RawData.Length));
                    offset += payload.DLSettings.RawData.Length;
                    payload.RxDelay.RawData.CopyTo(bytes.Slice(offset, payload.RxDelay.RawData.Length));

                    return bytes;
                case MessageType.Data:
                    throw new NotImplementedException();
                case MessageType.Proprietary:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        Span<byte> SKey()
        {
            switch (Mhdr.MessageType)
            {
                case MessageType.JoinRequest:
                    throw new NotImplementedException();
                case MessageType.JoinAccept:
                    JoinAcceptMessage payload = (JoinAcceptMessage)MacPayload;
                    byte[] skey = new byte[payload.AppNonce.RawData.Length +
                    payload.NetID.RawData.Length +
                   (payload.JoinRequestDevNonce.RawData.Length) + ("00000000000000".Length / 2)];
                    Span<byte> bytes = skey.AsSpan();

                    int offset = 0;
                    payload.AppNonce.RawData.CopyTo(bytes.Slice(offset, payload.AppNonce.RawData.Length));
                    offset+= payload.AppNonce.RawData.Length;
                    payload.NetID.RawData.CopyTo(bytes.Slice(offset, payload.NetID.RawData.Length));
                    offset += payload.NetID.RawData.Length;
                    payload.JoinRequestDevNonce.RawData.CopyTo(bytes.Slice(offset, payload.JoinRequestDevNonce.RawData.Length));
                    offset += payload.NetID.RawData.Length;
                    payload.JoinRequestDevNonce.RawData.CopyTo(bytes.Slice(offset, payload.JoinRequestDevNonce.RawData.Length));
                    offset += payload.JoinRequestDevNonce.RawData.Length;
                    Span<byte> padd = new Span<byte>(("00000000000000").FromHexString());
                    padd.CopyTo(bytes.Slice(offset, payload.JoinRequestDevNonce.RawData.Length));
                    return bytes;
                case MessageType.Data:
                    throw new NotImplementedException();
                case MessageType.Proprietary:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public Span<byte> ToBytes(byte[] AppKey)
        {
            Crypto.CryptoService service = new Crypto.CryptoService();
            switch (Mhdr.MessageType)
            {
                case MessageType.JoinRequest:
                    throw new NotImplementedException();
                case MessageType.JoinAccept:
                    JoinAcceptMessage payload = (JoinAcceptMessage)MacPayload;
                    byte[] mic = service.AESCMAC(AppKey, CMAC().ToArray()).AsSpan<byte>().Slice(0, 4).ToArray();
                    Span<byte> micSpan = mic.AsSpan();
                    Span<byte> skey = SKey();
                    byte[] nskey = new byte[skey.Length + 1];
                    byte[] askey = new byte[skey.Length + 1];
                    Span<byte> nskeySpan = nskey.AsSpan();
                    Span<byte> askeySpan = askey.AsSpan();
                    nskeySpan[0] = 1;
                    skey.CopyTo(nskeySpan.Slice(1));

                    askeySpan[0] = 1;
                    skey.CopyTo(askeySpan.Slice(1));


                    var nwkSKey = service.AESEncrypt(AppKey, nskey);
                    var appSKey = service.AESEncrypt(AppKey, askey);

                    string nwkSKeyst = nwkSKey.ToHexString();
                    string appSKeyst = appSKey.ToHexString();

                    byte[] txdatabytes = new byte[Mhdr.RawData.Length + payload.AppNonce.RawData.Length +
                        payload.NetID.RawData.Length + payload.DevAddr.RawData.Length + payload.DLSettings.RawData.Length
                        + payload.RxDelay.RawData.Length+mic.Length];
                    Span<byte> txdatabytesSpan = txdatabytes.AsSpan();
                    Mhdr.RawData.CopyTo(txdatabytesSpan);

                    byte[] decodedtxdatabytes = new byte[ payload.AppNonce.RawData.Length +
                        payload.NetID.RawData.Length + payload.DevAddr.RawData.Length + payload.DLSettings.RawData.Length
                        + payload.RxDelay.RawData.Length + mic.Length];

                    Span<byte> decodedtxdatabytesSpan = decodedtxdatabytes.AsSpan();
                    payload.AppNonce.RawData.CopyTo(decodedtxdatabytesSpan);
                    int offset = payload.AppNonce.RawData.Length;
                    payload.NetID.RawData.CopyTo(decodedtxdatabytesSpan.Slice(offset, payload.NetID.RawData.Length));
                    offset += payload.NetID.RawData.Length;
                    payload.DevAddr.RawData.CopyTo(decodedtxdatabytesSpan.Slice(offset, payload.DevAddr.RawData.Length));
                    offset += payload.DevAddr.RawData.Length;
                    payload.DLSettings.RawData.CopyTo(decodedtxdatabytesSpan.Slice(offset, payload.DLSettings.RawData.Length));
                    offset += payload.DLSettings.RawData.Length;
                    payload.RxDelay.RawData.CopyTo(decodedtxdatabytesSpan.Slice(offset, payload.RxDelay.RawData.Length));
                    offset += payload.RxDelay.RawData.Length;
                    micSpan.CopyTo(decodedtxdatabytesSpan.Slice(offset, mic.Length));

                    service.AESDecrypt(AppKey, decodedtxdatabytes).AsSpan().CopyTo(txdatabytesSpan.Slice(Mhdr.RawData.Length));

                    return txdatabytesSpan;
                case MessageType.Data:
                    throw new NotImplementedException();
                case MessageType.Proprietary:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}