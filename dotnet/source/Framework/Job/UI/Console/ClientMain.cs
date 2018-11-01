using System;
using System.IO;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Lifetime;
using System.Collections.Specialized;
using System.Xml.Serialization;
using Warehouse.Remoting;
using Warehouse.Config.Server;

namespace Warehouse.Server.Client
{
  /// <summary>
  /// Summary description for CmdGUI.
  /// </summary>
  class CmdGUI
  {
    private RemoteInstanceCtrl ric;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      CmdGUI me = new CmdGUI();
      me.Menu();
    }
    
    public bool Connect(String host, int port) 
    {
      bool connected = false;

      try
      {
        ric = (RemoteInstanceCtrl)RemotingServices.Connect(typeof(RemoteInstanceCtrl), String.Format("http://{0}:{1}/IMIServer", host, port));
        ric.Time();
        connected = true;
      }
      catch(Exception e)
      {
        Console.WriteLine("Failed to connect, error {0}",e.Message);
        ric = null;
      }

      return(connected);
    }

    private String FormatDateTime(DateTime value)
    {
      if(value == DateTime.MinValue)
        return("");

      String r = String.Format("{0}{1}{2}", 
        value.ToString("MMM").ToUpper().Substring(0,1),
        value.ToString("MMM").Substring(1,2),
        value.ToString(" d HH:mm:ss"));

      return(r);
    }
    private String FormatTimeSpan(String ts) 
    {
      if((ts == null) || (ts == ""))
        return "0:00:000";

      TimeSpan t = TimeSpan.Parse(ts);

      int minutes = (int)Math.Floor(t.TotalMinutes);


      return(String.Format("{0:0}:{1:00}:{2:000}",
        minutes,
        t.Seconds,
        t.Milliseconds));
    }

    public String AdjustPsList(String xmlList) 
    {
//      StreamWriter ss = new StreamWriter(@"c:\temp\ps3.xml");
//      ss.WriteLine(xmlList);
//      ss.Close();

      XmlSerializer s = new XmlSerializer(typeof(JobRuntimeType[]));
      JobRuntimeType[] jArr = s.Deserialize(new StringReader(xmlList)) as JobRuntimeType[];

      String[] keys = new String[jArr.Length];
      int i = 0;

      foreach(JobRuntimeType j in jArr) 
      {
        keys[i++] = j.Name;
      }

      Array.Sort(keys,jArr);

      StringBuilder sb = new StringBuilder();

      sb.Append("Name                      Status   Run # Last Start      CPU Time    Realtime\n");
      sb.Append("========================= ======== ===== =============== =========== ===========\n");

      foreach(JobRuntimeType j in jArr) 
      {
        sb.Append(String.Format("{0} {1} {2} {3} {4} {5}\n",
          j.Name.PadRight(25), 
          j.Status.ToString().PadRight(8), 
          j.RunCount.ToString().PadLeft(5), 
          FormatDateTime(j.RunStarted).PadRight(15), 
          FormatTimeSpan(j.TotalProcessorTime).PadRight(11),
          FormatTimeSpan(j.TotalRealTime).PadRight(11)
          ));
      }

      return(sb.ToString());
    }

    public String Help() 
    {
      StringBuilder h = new StringBuilder();

      h.Append("\n Command               Descricption\n");
      h.Append(" ===================== ==================================\n");
      h.Append(" connect <host> <port> connects to a specific instance.\n");
      h.Append(" help                  displays this help text.\n");
      h.Append(" startall              starts all enabled jobs.\n");
      h.Append(" stopall               stops all running jobs.\n");
      h.Append(" start <jobname>       starts a single job.\n");
      h.Append(" stop <jobname>        stops a single job.\n");
      h.Append(" ps                    displays a list of running jobs.\n");
      h.Append(" exit                  exits this program.\n");

      return(h.ToString());
    }
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public void Menu()
    {
      Console.WriteLine("Server Manager Client");
      Console.WriteLine("version 1.0\n");

      Console.WriteLine("type help for available commands.");

      
      bool goOn = true;
      bool connected = false;

      while(goOn)
      {
        Console.Write("> ");
        String command = Console.ReadLine();
        String[] cArr = command.Split(new char[] {' '});
        String result = "";

        try 
        {
          if(connected) 
          {
            switch (cArr[0].ToLower()) 
            {
              case "help":
                Console.WriteLine(Help());
                break;
              case "ps":
                result = ric.Ps();
                Console.WriteLine(AdjustPsList(result));
                break;
              case "startall":
                result = ric.StartAll();
                Console.WriteLine(result);
                break;
              case "stopall":
                result = ric.StopAll();
                Console.WriteLine(result);
                break;
              case "start":
                if(cArr.Length >= 2) 
                {
                  foreach(String jb in cArr) 
                  {
                    if(jb != "start") 
                    {
                      result = ric.StartJob(jb);
                      Console.WriteLine(String.Format("Start of {0} {1}",jb,result));
                    }
                  }
                }
                else
                  Console.WriteLine("{0} is an illegal command. Correct syntax is {1} <jobname>.", command, cArr[0]);
                break;
              case "stop":
                if(cArr.Length >= 2)
                {
                  foreach(String jb in cArr) 
                  {
                    if(jb != "stop") 
                    {
                      result = ric.StopJob(jb);
                      Console.WriteLine(String.Format("Stop of {0} {1}",jb,result));
                    }
                  }
                }
                else
                  Console.WriteLine("{0} is an illegal command. Correct syntax is {1} <jobname>.", command, cArr[0]);
                break;
              case "connect":
                if(cArr.Length == 3) 
                {
                  int port;
                  try 
                  {
                    port = Convert.ToInt32(cArr[2]);
                  }
                  catch(Exception) 
                  {
                    Console.WriteLine("Port number must be numeric.");
                    break;
                  }

                  if(!Connect(cArr[1],port)) 
                  {
                    break;
                  }
                  connected = true;
                  Console.WriteLine("ok, connected");
                  break;
                }
                else
                  Console.WriteLine("{0} is an illegal command. Correct syntax is {1} <hostname> <port>.", command, cArr[0]);
                break;
              case "exit":
                goOn = false;
                break;
              case "":
                break;
              default:
                Console.WriteLine("{0} is an illegal command", command);
                break;
            }
          }
          else 
          {
            switch (cArr[0].ToLower()) 
            {
              case "connect":
                if(cArr.Length == 3) 
                {
                  int port;

                  try 
                  {
                    port = Convert.ToInt32(cArr[2]);
                  }
                  catch(Exception) 
                  {
                    Console.WriteLine("Port number must be numeric.");
                    break;
                  }

                  if(!Connect(cArr[1],port)) 
                  {
                    break;
                  }
                  connected = true;
                  Console.WriteLine("ok, connected");
                  break;
                }
                else
                  Console.WriteLine("{0} is an illegal command. Correct syntax is {1} <hostname> <port>.", command, cArr[0]);
                break;
              case "help":
                Console.WriteLine(Help());
                break;
              case "exit":
                goOn = false;
                break;
              default:
                Console.WriteLine("You must connect before issuing any commands except help and exit.", command);
                break;
            }
          }
        }
        catch(Exception e) 
        {
          Console.WriteLine("Error when issuing command to server\n{0}", e.Message);
        }
      }

      GC.Collect();
      GC.WaitForPendingFinalizers();
    }
  }
}
