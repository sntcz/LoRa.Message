using System;
using System.Text;

namespace LoRa.Message
{
    public class JoinRequestMessage : MACPayload
    {
        public JoinRequestMessage(IPayloadPart parent) : base(parent)
        { /* NOP */}

        public override string ToVerboseString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" (MACPayload = AppEUI[8] | DevEUI[8] | DevNonce[2])").AppendLine();
            //sb.AppendFormat("      AppEUI = 70B3D57ED00000DC").AppendLine();
            //sb.AppendFormat("      DevEUI = 00AFEE7CF5ED6F1E").AppendLine();
            //sb.AppendFormat("    DevNonce = 86C8 ").AppendLine();
            return sb.ToString();
        }
    }
}