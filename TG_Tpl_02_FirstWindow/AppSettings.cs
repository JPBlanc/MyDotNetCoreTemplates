using System;
using System.Collections.Generic;

namespace TG_Tpl_02_FirstWindow
{
  public class AppParams
  {
    public string defaultValue { get; set; }
    public List<UsefullObject> usefullObjects { get; set; }

    public AppParams(){
      usefullObjects = new List<UsefullObject>();
    }
  }

  public class UsefullObject
  {
    public string Name { get; set; }
    public string Type { get; set; }
    public int StartingValue { get; set; }
    public int Increment {get; set;}
    public int  SleepingTime {get; set;}
  }

}