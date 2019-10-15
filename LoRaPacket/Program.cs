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
            byte[] appKey = null;
            if (!String.IsNullOrEmpty(opts.NwkSKey))
                nwkSKey = ConvertExtension.FromHexString(opts.NwkSKey);
            if (!String.IsNullOrEmpty(opts.AppSKey))
                appSKey = ConvertExtension.FromHexString(opts.AppSKey);
            if (!String.IsNullOrEmpty(opts.AppKey))
                appKey = ConvertExtension.FromHexString(opts.AppKey);
            PHYPayload packet = new PHYPayload(data, nwkSKey, appSKey, appKey, 0);
            Console.WriteLine(packet.ToVerboseString());

            return 0;
        }

        static int ShowErrors(IEnumerable<Error> errs)
        {
            return 1;
        }

    }
}
