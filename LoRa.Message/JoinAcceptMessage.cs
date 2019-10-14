using System;
using System.Text;

namespace LoRa.Message
{
    public class JoinAcceptMessage : MACPayload
    {
        public PayloadPart<JoinAcceptMessage> AppNonce { get; }

        public JoinAcceptMessage(IPayloadPart parent) : base(parent)
        {
            AppNonce = new PayloadPart<JoinAcceptMessage>(this, 0, 3);
        }

        public override string ToVerboseString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" (MACPayload = AppNonce[3] | NetID[3] | DevAddr[4] | DLSettings[1] | RxDelay[1] | CFList[0 | 15])").AppendLine();
            sb.AppendFormat("    AppNonce = {0}", AppNonce.RawData.ToHexString()).AppendLine();
            //sb.AppendFormat("       NetID = FF08F5").AppendLine();
            //sb.AppendFormat("     DevAddr = 6E0B67A2").AppendLine();
            //sb.AppendFormat("  DLSettings = 23").AppendLine();
            //sb.AppendFormat("     RxDelay = E0").AppendLine();
            //sb.AppendFormat("      CFList = 1F84B9E25D9C4115F02EEA0B3DD3E20B").AppendLine();
            //sb.AppendLine();
            //sb.AppendFormat("DLSettings.RX1DRoffset = 2").AppendLine();
            //sb.AppendFormat("DLSettings.RX2DataRate = 3").AppendLine();
            //sb.AppendFormat("           RxDelay.Del = 0").AppendLine();
            return sb.ToString();
        }
    }
}