# Brogdogg.Configuration.Xml

## Overview

This project provides an extension library for use with `IConfigurationBuilder`, providing 
a **writable** XML configuration source/provider.

It extends the `XmlConfigurationSource` and `XmlConfigurationProvider` defined in
the [Microsoft.Extensions.Configuration.Xml](https://github.com/dotnet/runtime/tree/master/src/libraries/Microsoft.Extensions.Configuration.Xml) library.

Due to the nature of how the `XmlConfigurationProvider` reads the data from the XML file,
the writer will not preserve XML attributes when writing the XML file.


## Building

### Prerequisites

At least .NET 6.0.100 is required.

### CLI

```PowerShell
Brogdogg.Configuration.Xml> dotnet build
Brogdogg.Configuration.Xml> dotnet test
Microsoft (R) Test Execution Command Line Tool Version 17.0.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    13, Skipped:     0, Total:    13, Duration: 385 ms - Brogdogg.Configuration.Xml.Tests.dll (net6.0)
```

## Usage Example

```csharp
using Brogdogg.Configuration.Xml;

namespace Test 
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
      => Host.CreateDefaultBuilder(args)
                 .ConfigureHostConfiguration(config =>
                 {
                   // Add the writable XML configuration source for the
                   // example.xml file.
                   config.AddWritableXml(@"C:\example.xml");
                 });

  }


  public class ExampleService
  {
    private IConfiguration Configuration { get; }

    public ExampleService(IConfiguration configuration) => Configuration = configuration;

    public void SetVersion()
    {
      // Set the value of the version to 1.0.0.0, which persists the changes
      // to the XML source for reading next time.
      Configuration["Version"] = "1.0.0.0";
    }
  }
}
```