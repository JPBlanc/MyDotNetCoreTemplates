using System;
using System.Collections.Generic;

namespace TG_Tpl_01_Params
{
  partial class ConsoleWithParams
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

        Console.WriteLine($"Default value : {appParams.defaultValue}");
        foreach(var usefullObject in appParams.usefullObjects){
          Console.WriteLine($"\tUsefull object name : {usefullObject.Name}");
        }

        foreach(var key in cmdLineParams.Keys){
          Console.WriteLine($"{key} : {cmdLineParams[key]}");
        }
      }
      catch(Exception e)
      {
        Console.WriteLine(e.Message);
        rc = -1;
      }

      return rc;
    }
  }
}
