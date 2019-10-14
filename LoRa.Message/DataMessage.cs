using System;
using System.Text;

namespace LoRa.Message
{
    public class DataMessage : MACPayload
    {
        public FHDR Fhdr { get; }
        public FPort FPort { get; }
        public FRMPayload FrmPayload { get; }

        private byte[] nwkSKey;
        private Lazy<CalculatedMIC> _calculatedMIC;
        public CalculatedMIC CalculatedMIC => _calculatedMIC.Value;       

        public DataMessage(IPayloadPart parent, byte[] nwkSKey, byte[] appSKey, int fCntMsbSeed) : base(parent)
        {
            this.nwkSKey = nwkSKey;
            Fhdr = new FHDR(this);
            FPort = new FPort(this);
            FrmPayload = new FRMPayload(this, nwkSKey, appSKey);
            _calculatedMIC = new Lazy<CalculatedMIC>(() => new CalculatedMIC(this, nwkSKey, fCntMsbSeed));
        }

        public override string ToVerboseString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" (MACPayload = FHDR[7..22] | FPort[0..1] | FRMPayload[0..N])").AppendLine();
            sb.AppendFormat("        FHDR = {0}", Fhdr.RawData.ToHexString()).AppendLine();
            sb.AppendFormat("       FPort = {0:X2}", FPort.Value).AppendLine();
            sb.AppendFormat("  FRMPayload = {0}", FrmPayload.RawData.ToHexString()).AppendLine();
            if (nwkSKey != null)
            {
                if (CalculatedMIC.IsValid)
                    sb.AppendFormat("    Calc MIC = {0} (assumed FCntMSB {1:X4})", CalculatedMIC.RawData.ToHexString(), CalculatedMIC.FCntMSB).AppendLine();
                else
                    sb.AppendFormat("    Calc MIC = INVALID ", CalculatedMIC.RawData.ToHexString(), CalculatedMIC.FCntMSB).AppendLine();
            }
            if (nwkSKey != null)
            {
                sb.AppendFormat("  FRMPayload = {0} (DECRYPTED)", FrmPayload.Decrypt().ToHexString()).AppendLine();
            }
            sb.AppendLine();
            sb.Append(Fhdr.ToVerboseString());
            return sb.ToString();
        }
    }
}