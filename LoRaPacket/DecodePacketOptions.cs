using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace LoRaPacket
{
    [Verb("decode")]
    class DecodePacketOptions
    {
        [Option(SetName = "base64")]
        public string Base64 { get; set; }

        [Option(SetName = "hex")]
        public string Hex { get; set; }

        [Option]
        public string NwkSKey { get; set; }

        [Option]
        public string AppSKey { get; set; }

    }
}
