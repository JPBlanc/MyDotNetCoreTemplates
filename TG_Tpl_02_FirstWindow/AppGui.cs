using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// NuGet Packages
using Terminal.Gui;
using Terminal.Gui.Graphs;

namespace TG_Tpl_02_FirstWindow
{
  public partial class AppGui : Window
  {
    public int Rc { get; set;} // For return code

    private AppParams appParams; //  External params
    private List<string> usefullObjectLst;
    private string currentUsefullObjectName;
    private UsefullObject currentusefullObject;


    // Thread Part
    CancellationTokenSource cancelTokenSource;
    CancellationToken cancellationToken;
    TaskFactory factory = new TaskFactory();
    public EventWaitHandle SynvEvent { get; set; }
    public Task currentTask;


    // Graphic objects
    private MenuBar menu;
    private Window winTop;
    private Label lblName;
    private TextField tfName;
    private Label lblType;
    private TextField tfType;
    private Label lblUsefullObjectWorking;
    private TextField tfUsefullObjectWorking;
    private Button btStart;
    private Label lblUsefullObjects;
    private ComboBox cbUsefullObjects;
    private Button btChangeUsefullObject;

    

    public AppGui(AppParams appParams, string ACmdLineParam)
    {
      //  Compute current UsefullObject
      this.appParams = appParams;
      usefullObjectLst = (from u in appParams.usefullObjects select u.Name).ToList<string>();
      if (ACmdLineParam == string.Empty)
      {
        currentUsefullObjectName = appParams.defaultValue;
        currentusefullObject = (from u in appParams.usefullObjects where u.Name == appParams.defaultValue select u).FirstOrDefault();
      }
      else
      {
        currentUsefullObjectName = ACmdLineParam;
        currentusefullObject = (from u in appParams.usefullObjects where u.Name == ACmdLineParam select u).FirstOrDefault();
      }

      if (currentusefullObject == null)
      {
        throw new ArgumentException("Unable to choose default useful object !");
      }

      // Gui
      Init();

      // Show current
      SetCurrentusefullObject();

      // Processing
      // Create a ManualReset EventWaitHandle.
      SynvEvent = new EventWaitHandle(false, EventResetMode.ManualReset);

      // Start the threads
      cancelTokenSource = new CancellationTokenSource();
      cancellationToken = cancelTokenSource.Token;
      currentTask = factory.StartNew(ThreadProc);
    }

    void Init()
    {
      this.ColorScheme = Colors.Base;

      // Creates a menubar, the item "New" has a help menu.
      menu = new MenuBar(new MenuBarItem[] {
        new MenuBarItem ("_File", new MenuItem [] {
          new MenuItem ("_Quit", "", () => Quit ()  )
        })
      });
      menu.ColorScheme = Colors.Base;

      // Create a Window, leave one row for the toplevel menu
      winTop = new Window($" {currentUsefullObjectName} ") { X = 0, Y = 1, Width = Dim.Percent(100, false), Height = 7};
      winTop.ColorScheme = Colors.Base;
      lblName = new Label("Name: ") { X = Pos.Left(winTop), Y = 0 };
      tfName = new TextField("Premier nom") { X = Pos.Right(lblName), Y = Pos.Top(lblName), Width = 15 };
      lblType = new Label("Type: ") { X = Pos.Left(winTop), Y = 1 };
      tfType = new TextField("Premier type") { X = Pos.Right(lblType), Y = Pos.Top(lblType), Width = 15 };
      lblUsefullObjectWorking = new Label("Usefull Object is working here: ") { X = Pos.Left(winTop), Y = 4 };
      tfUsefullObjectWorking = new TextField("") { X = Pos.Right(lblUsefullObjectWorking), Y = Pos.Top(lblUsefullObjectWorking), Width = 6};

      btStart = new Button("Start") { X = Pos.Center(), Y = 0 };
      SetButtonState(btStart, "Start", false, Colors.Base);
      btStart.Clicked += OnbtStartClicked;

      lblUsefullObjects = new Label("UsefullObjects: ") { X = Pos.Right(winTop) - 30, Y = 0 };
      cbUsefullObjects = new ComboBox() { X = Pos.Right(lblUsefullObjects), Y = 0, Width = 10, Height = 6 };
      cbUsefullObjects.SetSource(usefullObjectLst);
      cbUsefullObjects.SelectedItemChanged += OncbUsefullObjectsSelectedItemChanged;

      btChangeUsefullObject = new Button("Change UsefullObject") { X = Pos.Center(), Y = 1 };
      btChangeUsefullObject.ColorScheme = Colors.Base;
      btChangeUsefullObject.Data = false;
      btChangeUsefullObject.Clicked += OnbtChangeUsefullObjectClicked;

      winTop.Add(lblName, tfName, btStart, tfType, tfType, lblUsefullObjectWorking, tfUsefullObjectWorking, lblUsefullObjects, cbUsefullObjects, btChangeUsefullObject); ;

      // Insert the menu
      Add(menu);

      // Creates the left top-level window to show
      Add(winTop);
    }
  }
}
