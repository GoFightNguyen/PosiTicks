# PosiTicks
A Progressive Web Application built using Blazor WebAssembly and hosted using ASP.NET Core.

## Logging
Logging is achieved using [serilog](https://serilog.net/).
This decision was made because it supports structured logging and many sinks (providers in aspnetcore), including New Relic.

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
[serilog-aspnetcore github](https://github.com/serilog/serilog-aspnetcore)

[Successful production diagnostics in aspnetcore](https://nblumhardt.com/2019/10/serilog-in-aspnetcore-3/)