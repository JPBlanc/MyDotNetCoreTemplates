using System;
using System.Collections.Generic;

// NuGet Packages
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;


namespace TG_Tpl_01_Params
{
  partial class ConsoleWithParams
  {
    static IConfigurationRoot configurationRoot; // For parameters

    static bool GetManagedParams (string[] args, out AppParams appParams, out Dictionary<string,string> cmdLineParams)
    {
      bool blRc = true;

      appParams = new AppParams();
      using IHost host = CreateHostBuilder(args).Build();
      configurationRoot.GetSection(nameof(AppParams)).Bind(appParams);
      IConfigurationSection cmdLineStagingSection = configurationRoot.GetSection("ACmdLineParam");

      cmdLineParams = new();
      if (cmdLineStagingSection != null && cmdLineStagingSection.Value != null)
      {
        cmdLineParams.Add("ACmdLineParam", cmdLineStagingSection.Value);
      }

      return blRc;
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
      IHostBuilder iHostBuilderRc = Host.CreateDefaultBuilder(args);
      iHostBuilderRc.ConfigureAppConfiguration(AppConfiguration);
      return iHostBuilderRc;
    }

    static void AppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder configuration)
    {
      //configuration.Sources.Clear();

      IHostEnvironment env = hostingContext.HostingEnvironment;

      //Console.WriteLine($"{env.EnvironmentName}");

      configuration
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

      configurationRoot = configuration.Build();
    }
  }
}
