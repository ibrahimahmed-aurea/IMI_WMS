using System;
using Warehouse.Logging;
using Warehouse.Runtime;
using Warehouse.Config;
using System.Configuration;
using System.Windows.Forms;

namespace WMSWinServer
{
  // Simple threading scenario:  Start a static method running
  // on a second thread.
  public class ThreadExample 
  {
    private JobManager jm;
    private String     name;

    public void Server(String SystemId) 
    {
      Log.Put(this.name,"Start Job Manager.");
      jm = new JobManager(SystemId);

      Log.Put(this.name,"Server is started.");
      Log.Put(this.name,"Hit enter to stop all jobs.");
      Console.ReadLine();

      Log.Put(this.name,"Shutdown started.");
      jm.ShutDown();
      Log.Put(this.name,"Shutdown finished.");

      Log.Put(this.name,"Press Enter to end program."); 
      Console.ReadLine();

      // Ensure that all objects finalize before the application ends 
      // by calling the garbage collector and waiting.
      GC.Collect();
      GC.WaitForPendingFinalizers();
    }

    public static void Main(String[] args) 
    {
      CommandLine c = new CommandLine(args);
     
      String SystemId = c["SystemId"];

      if((SystemId == null) || (SystemId == ""))
      {
        Console.Error.WriteLine("Error 0x0001 - No SystemId was supplied.\nUsage: {0} /SystemId <SystemId>",Application.ProductName);
        return;
      }

      ThreadExample me = new ThreadExample();
      me.name = "WMSServer";
      me.Server(SystemId);
    }

  }
}