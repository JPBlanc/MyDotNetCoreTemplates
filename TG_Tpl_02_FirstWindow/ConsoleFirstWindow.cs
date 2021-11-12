using System;
using System.Collections.Generic;

using Terminal.Gui;

namespace TG_Tpl_02_FirstWindow
{
  partial class ConsoleFirstWindow
  {
    static int Main(string[] args)
    {
      int rc = 0;
      try
      {
        Console.WriteLine("Hello World!");
        AppParams appParams;
        Dictionary<string,string> cmdLineParams;
        GetManagedParams (args, out appParams, out cmdLineParams);

        string ACmdLineParam = cmdLineParams["ACmdLineParam"];

        Application.Init();

        Application.HeightAsBuffer = false;
        Colors.Base.Normal = Application.Driver.MakeAttribute (Color.Green, Color.Black);

        AppGui appGui = new AppGui(appParams, ACmdLineParam);
        Application.Run(appGui, ErrorHandler);
        Application.Shutdown();
      }
      catch(Exception e)
      {
        Console.WriteLine(e.Message);
        rc = -1;
      }

      return rc;
    }

    static bool ErrorHandler(Exception e)
    {
      return true;
    }
  }
}
