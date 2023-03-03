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
        public DevNonce JoinRequestDevNonce { get; set; }

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