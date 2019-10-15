using System;
using System.Text;

namespace LoRa.Message
{
    public class JoinRequestMessage : MACPayload
    {
        private Lazy<CalculatedMIC> _calculatedMIC;
        public override CalculatedMIC CalculatedMIC => _calculatedMIC.Value;

        public AppEUI AppEUI { get; }
        public DevEUI DevEUI { get; }
        public DevNonce DevNonce { get; }

        public JoinRequestMessage(IPayloadPart parent, byte[] appKey) : base(parent)
        {
            AppEUI = new AppEUI(this);
            DevEUI = new DevEUI(this);
            DevNonce = new DevNonce(this);
            _calculatedMIC = new Lazy<CalculatedMIC>(() => new CalculatedMIC(this, appKey));
        }

        public override string ToVerboseString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" (MACPayload = AppEUI[8] | DevEUI[8] | DevNonce[2])").AppendLine();
            sb.AppendFormat("      AppEUI = {0}", AppEUI.Value).AppendLine();
            sb.AppendFormat("      DevEUI = {0}", DevEUI.Value).AppendLine();
            sb.AppendFormat("    DevNonce = {0}", DevNonce.Value).AppendLine();
            if (CalculatedMIC.IsValid)
                sb.AppendFormat("    Calc MIC = {0})", CalculatedMIC.RawData.ToHexString()).AppendLine();
            return sb.ToString();
        }
    }
}