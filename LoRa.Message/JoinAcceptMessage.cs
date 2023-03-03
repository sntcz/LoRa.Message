using System;
using System.Text;

namespace LoRa.Message
{
    public class JoinAcceptMessage : MACPayload
    {
        private Lazy<CalculatedMIC> _calculatedMIC;
        public override CalculatedMIC CalculatedMIC => _calculatedMIC.Value;

        public AppNonce AppNonce { get; }       
        public NetID NetID { get; }
        public DevAddr DevAddr { get; }
        public DLsettings DLSettings { get; }
        public RxDelay RxDelay { get; }
        public PayloadPart<JoinAcceptMessage> CFList { get; }
        public JoinRequestMessage joinRequestMessage { get; set; }
        //public JoinAcceptMessage(JoinRequestMessage joinRequest)
        //{
        //    joinRequestMessage = joinRequest;
        //    AppNonce = Utils.IntToByte((uint)Utils.GetAppNounce(), 3).Reverse().ToArray();// new byte[] { (byte)ran.Next(0, 255), (byte)ran.Next(0, 255), (byte)ran.Next(0, 255) }.Reverse().ToArray();
        //    i += 3;
        //    netID = new byte[] { 0, 0, 0 }.Reverse().ToArray();
        //    i += 3;
        //    devAddr = Utils.IntToByte((uint)Selected.DevID, 4).Reverse().ToArray(); //new byte[] { 0x26, 0x01, 0x2E, 0x43 };
        //    i += 4;
        //    dlSettings = new byte[] { 0x00 };
        //    i += 1;

        //    rxDelay = new byte[] { 0x05 };

        //    i += 1;
        //    //join.cfList = Utils.StringToByteArray("184f84e85684b85e84886684586e8400");

        //    //join.cfList = Utils.StringToByteArray("008780b372f79de783ed575eff6426a7");
        //    join.Tx = new TxPacket();
        //    join.DelayMiliSecond = 5000000;
           
        //        join.Tx.freq = Convert.ToDouble(rx.freq);
           

        //    Tx.codr = rx.codr;
        //    Tx.datr = rx.datr;
        //    Tx.powe = 14;

        //    cmac = new byte[1 +
        //    appNonce.Length +
        //    netID.Length +
        //    devAddr.Length +
        //    1 +
        //    1
        //    ];
        //}

        public JoinAcceptMessage(IPayloadPart parent, byte[] appKey) : base(parent)
        {
            AppNonce = new AppNonce(this);
            NetID = new NetID(this);
            DevAddr = new DevAddr(this);
            DLSettings = new DLsettings(this);
            RxDelay = new RxDelay(this);
            CFList = new PayloadPart<JoinAcceptMessage>(this, 12, RawData.Length - 12);
            _calculatedMIC = new Lazy<CalculatedMIC>(() => new CalculatedMIC(this, appKey));
        }
     

        public override string ToVerboseString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" (MACPayload = AppNonce[3] | NetID[3] | DevAddr[4] | DLSettings[1] | RxDelay[1] | CFList[0 | 15])").AppendLine();
            sb.AppendFormat("    AppNonce = {0}", AppNonce.Value).AppendLine();
            sb.AppendFormat("       NetID = {0}", NetID.Value).AppendLine();
            sb.AppendFormat("     DevAddr = {0}", DevAddr.Address).AppendLine();
            sb.AppendFormat("  DLSettings = {0}", DLSettings.RawData.ToHexString()).AppendLine();
            sb.AppendFormat("     RxDelay = {0}", RxDelay.RawData.ToHexString()).AppendLine();
            sb.AppendFormat("      CFList = {0}", CFList.RawData.ToHexString()).AppendLine();
            sb.AppendLine();
            sb.Append(DLSettings.ToVerboseString());
            sb.AppendLine();
            sb.Append(RxDelay.ToVerboseString());
            if (CalculatedMIC.IsValid)
                sb.AppendFormat("    Calc MIC = {0})", CalculatedMIC.RawData.ToHexString()).AppendLine();

            return sb.ToString();
        }
    }
}