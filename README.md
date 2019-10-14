# LoRa.Message

A pure C# library to decode and encode packets for LoRa/LoRaWAN<sup>TM</sup> 
radio communication, based on the specification from 
the [LoRa Alliance](https://www.lora-alliance.org/) (based on V1.0.3)

Thanks for huge inspiration to [lora-packet](https://github.com/anthonykirby/lora-packet), 
a node.js library to decode and encode packets for LoRa/LoRaWAN<sup>TM</sup>

---------------------------------------

Packet decoding is also wrapped in a simple command-line tool that 
accepts input in hex and base-64

## Features

* [x] LoRa packet parsing & analysis
* [x] MIC (Message Integrity Check) checking
* [x] payload decryption
* [x] decode uplink & downlink packets
* [ ] unit tests for everything
  * [x] MIC calculation
  * [x] message decryption
  * [ ] utility conversion 
* [ ] decode join request packets
* [ ] decode join accept packets
* [ ] generate NwkSKey and AppSKey from AppID

## Usage (in your code)

```csharp
PHYPayload packet = new PHYPayload(data, nwkSKey, appSKey, 0);
```


## Usage (command-line packet decoding)

```dos
dotnet LoRaPacket.dll decode --hex 40F17DBE4900020001954378762B11FF0D
```

```dos
dotnet LoRaPacket.dll decode --base64 QK4TBCaAAAABb4ldmIEHFOMmgpU= \
    --nwkskey 99D58493D1205B43EFF938F0F66C339E \
    --appskey 0A501524F8EA5FCBF9BDB5AD7D126F75
```

## Contribute

First off, *Thank you!* All contributions are welcome.

Please follow the project's tabs settings for easiest diff compares.

### Contributor Code of Conduct

This project adheres to [No Code of Conduct](CODE_OF_CONDUCT.md).

## License
[MIT](LICENSE.md)