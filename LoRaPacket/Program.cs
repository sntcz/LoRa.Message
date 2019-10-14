using System;
using System.Collections.Generic;
using CommandLine;
using LoRa.Message;

namespace LoRaPacket
{
    class Program
    {
        static int Main(string[] args)
        {

            //WritePacket("ANwAANB+1bNwHm/t9XzurwDIhgMK8sk=");
            //WritePacket("QCkuASaAAAAByFaF53Iu+vzmwQ==");
            //WritePacket("QFMeASaAZkYBRXCQ7SU=", "7A47F143D7CEF033DFA0D4B75E04A316", "F1B0B1D3CC529C55C3019A46EF4582EA"); // 40531E012680664601457090ED25
            //WritePacket("QK4TBCaAAAABb4ldmIEHFOMmgpU=", "99D58493D1205B43EFF938F0F66C339E", "0A501524F8EA5FCBF9BDB5AD7D126F75");

            //WritePacket(
            //"40392C0126803A3302EBF3495EEF258A8BD38D13E8172199FE70F025".FromHexString().ToBase64String(),
            //"44D4A5DA7A9507F036C5A2750211F050", "5505CA3E4620843B324502A5676BADD7");

            Parser parser = new Parser(config =>
            {
                config.CaseSensitive = false;
                config.CaseInsensitiveEnumValues = true;
                config.AutoHelp = true;
                config.HelpWriter = Console.Out;
                config.AutoVersion = true;
            });
            int result = 255;
            try
            {
                result = parser.ParseArguments<DecodePacketOptions>(args)
                    .MapResult(
                        (DecodePacketOptions opts) => RunCommand(opts),
                        errs => ShowErrors(errs));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(true);
            }
            return result;
        }

        private static int RunCommand(DecodePacketOptions opts)
        {
            byte[] data = !String.IsNullOrEmpty(opts.Hex) ? opts.Hex.FromHexString()
                : !String.IsNullOrEmpty(opts.Base64) ? opts.Base64.FromBase64String()
                : null;

            byte[] nwkSKey = null;
            byte[] appSKey = null;
            if (!String.IsNullOrEmpty(opts.NwkSKey))
                nwkSKey = ConvertExtension.FromHexString(opts.NwkSKey);
            if (!String.IsNullOrEmpty(opts.AppSKey))
                appSKey = ConvertExtension.FromHexString(opts.AppSKey);
            PHYPayload packet = new PHYPayload(data, nwkSKey, appSKey, 0);
            Console.WriteLine(packet.ToVerboseString());

            return 0;
        }

        static int ShowErrors(IEnumerable<Error> errs)
        {
            return 1;
        }

        static void WritePacket(string base64, string n = null, string a = null)
        {
            Console.WriteLine("Base64 encoded packet {0}", base64);
            byte[] nwkSKey = null;
            byte[] appSKey = null;
            if (!String.IsNullOrEmpty(n))
                nwkSKey = ConvertExtension.FromHexString(n);
            if (!String.IsNullOrEmpty(a))
                appSKey = ConvertExtension.FromHexString(a);
            PHYPayload packet = new PHYPayload(Convert.FromBase64String(base64), nwkSKey, appSKey, 0);
            Console.WriteLine(packet.ToVerboseString());
        }
    }
}
