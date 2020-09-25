# PosiTicks
A Progressive Web Application built using Blazor WebAssembly and hosted using ASP.NET Core.

## Logging
Logging is achieved using [serilog](https://serilog.net/).
This decision was made because it supports structured logging and many sinks (think providers in dotnetcore) including New Relic.

Logging occurs from both the server and the client (Blazor/WASM).
The client actually relays the logs to the server.

The default logging providers in aspnetcore have been disabled, which is why the `appsettings.json` file(s) do not not contain the standard
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    },
    "some_other_provider": {
        ...
    }
  }
}
```

## Resources
### Serilog
#### Server
[Successful production diagnostics in aspnetcore](https://nblumhardt.com/2019/10/serilog-in-aspnetcore-3/)

[serilog-aspnetcore](https://github.com/serilog/serilog-aspnetcore)

#### Client
[Relay Blazor client logs to Serilog in ASP.NET Core](https://nblumhardt.com/2019/11/serilog-blazor/)

[serilog-sinks-browserhttp](https://github.com/nblumhardt/serilog-sinks-browserhttp/)

[serilog-sinks-browserconsole](https://github.com/serilog/serilog-sinks-browserconsole)