using System;
using System.Collections.Generic;

namespace TG_Tpl_01_Params
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
    public Type Type { get; set; }
  }

}