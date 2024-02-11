using NStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
// NuGet Packages
using Terminal.Gui;
using static Terminal.Gui.RadioGroup;

namespace TG_Tpl_02_FirstWindow
{
  public partial class AppGui
  {
    public void SetTextField(TextField tf, string value)
    {
      Application.MainLoop.Invoke(new Action(() => { if (value != null) { tf.Text = value; } }));
    }

    void SetCurrentusefullObject()
    {
      SetTextField(tfName, currentusefullObject.Name);
      SetTextField(tfType, currentusefullObject.Type);
    }

    void OnbtChangeUsefullObjectClicked()
    {
      // Show current
      SetCurrentusefullObject();

      // stop previous Thread
      cancelTokenSource.Cancel();
      SynvEvent.Set();
      currentTask.Wait();

      // Start the threads
      SetButtonState(btStart, "Start", false, base.ColorScheme);
      SynvEvent.Reset();
      cancelTokenSource = new CancellationTokenSource();
      cancellationToken = cancelTokenSource.Token;
      factory.StartNew(ThreadProc);
    }

    void OnbtStartClicked()
    {
      if ((bool)btStart.Data)
      {
        SetButtonState(btStart, "Start", false, base.ColorScheme);
        SynvEvent.Reset();
      }
      else
      {
        SetButtonState(btStart, "Stop", true, base.ColorScheme);
        SynvEvent.Set();
      }

      return;
    }

    void SetButtonState(Button bt, string text, bool onOff, ColorScheme cs)
    {
      Application.MainLoop.Invoke(new Action(() => {btStart.Data = onOff;
                                                    btStart.Text = text;
                                                    btStart.ColorScheme = cs;  }));
      
    }


    void OncbUsefullObjectsSelectedItemChanged(ListViewItemEventArgs lvie)
    {
      currentUsefullObjectName = usefullObjectLst[cbUsefullObjects.SelectedItem];
      currentusefullObject = (from u in appParams.usefullObjects where u.Name == currentUsefullObjectName select u).FirstOrDefault();
    }



    void Quit()
    {
      int queryRc = MessageBox.Query(50, 7, "Quit ConsoleFirstWindow", "Are you sure you want to say good by ?", "Yes", "No");
      if (queryRc == 0)
      {
        Running = false;
      }
    }


    public void ThreadProc()
    {
      int currentCounter = 0;
      try
      {
        currentCounter = currentusefullObject.StartingValue;
        while (true)
        {
          try
          {
            // Stay able to suspend running
            if (SynvEvent != null)
            {
              SynvEvent.WaitOne();
              if (cancellationToken.IsCancellationRequested)
              {
                // cancellation fire the thread
                return;
              }
            }else
            {
              // No event existing fire the thread
              return;
            }
            SetTextField(tfUsefullObjectWorking, Convert.ToString(currentCounter));
            currentCounter += currentusefullObject.Increment;
            Thread.Sleep(currentusefullObject.SleepingTime);
          }
          catch (Exception e)
          {
            SetTextField(tfUsefullObjectWorking, e.Message);
            break;
          }
        }

      }
      catch (AggregateException e)
      {
        SetTextField(tfUsefullObjectWorking, e.Message);
      }
    }
  }
}
