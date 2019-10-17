using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace LoRaPacket
{
    [Verb("decode", HelpText = "Decodes and prints message payload content")]
    class DecodePacketOptions
    {
        [Option(SetName = "base64", HelpText = "Payload in BASE-64 format.")]
        public string Base64 { get; set; }

        [Option(SetName = "hex", HelpText = "Payload in HEX format")]
        public string Hex { get; set; }

        [Option(HelpText = "Network session key in HEX format, (for data message decryption)")]
        public string NwkSKey { get; set; }

        [Option(HelpText = "Application session key in HEX format (for data message decryption)")]
        public string AppSKey { get; set; }

        [Option(HelpText = "Application key in HEX format (used for Join-Accept message)")]
        public string AppKey { get; set; }

    }
}
