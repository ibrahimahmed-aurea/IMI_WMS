using System;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Xml.Serialization;
using System.Reflection;
using Imi.Framework.Shared.Xml;
using Imi.Framework.Shared.IO;

namespace Warehouse.Config.Server
{
  public delegate void ConfigChangedEventDelegate();

  /// <summary>
  /// Summary description for ApplicationConfig.
  /// </summary>
  public class ApplicationConfig
  {
    private static ApplicationConfig me = null;
    private InstanceList      il;
    private ServerConfig      currentInstance;
    private String            currentFileName;
    private String            repoFileName;
    private String            defaultNameSpace = "Warehouse.Config.Server";
    private FileSystemWatcher fsw;
    private String            reloadError;

    public ConfigChangedEventDelegate ConfigChanged;
    
    public void OnConfigChanged() 
    {
      if(ConfigChanged != null) 
      {
        ConfigChanged();
      }
    }

    public static ServerConfig CurrentInstance 
    {
      get 
      {
        return(me.currentInstance);
      }
    }



    public ApplicationConfig()
    {
      me = this;

      repoFileName = FileIO.FindAppConfigFile("InstanceRepositoryFile","instance.config");

      #region oldcode
//      ConfigurationSettings.AppSettings["InstanceRepositoryFile"];
//
//      if(repoFileName == null)  
//      {
//        // Try to locate the file manually
//        String initDir = FindInitDirectory();
//
//        if(FileIO.FileExists(initDir +@"\instance.config")) 
//        {
//          repoFileName = initDir + @"\instance.config";
//        }
//        else 
//        {
//          String configFile = Application.ExecutablePath + ".config";
//
//          if(!FileIO.FileExists(configFile)) 
//          {
//            throw(new ConfigurationErrorsException(
//              String.Format("The application config file is missing, file = \"{0}\"", configFile)));
//          }
//
//          // Show sample file
//          String message = "Application config file does not contain the InstanceRepositoryFile value.";
//
//          try 
//          {
//            String sampleFileName = String.Format("{0}.sample.sample.config",defaultNameSpace);
//            StreamReader sample = FileIO.GetFileFromResources(sampleFileName);
//                  
//            String sampleText = sample.ReadToEnd().Replace("\r","");
//
//            message = String.Format("{0}\nExample:\n{1}\n",message,sampleText);
//          }
//          catch(Exception) 
//          {
//          }
//
//          message = String.Format("{0}\nFileName = \"{1}\"",message,configFile);
//
//          throw(new ConfigurationErrorsException(message));
//        }
//      }
      #endregion
      //
      // Check that instance file exists and is readable
      //

      // Read content
      XmlSerializer s = new XmlSerializer(typeof(InstanceList));

      StreamReader r = null;

      try 
      {
        try
        {
          r = new StreamReader(repoFileName);
        }
        catch (Exception e) 
        {
          throw(new ConfigurationErrorsException(
            String.Format(
            "Problems reading the Instance Repository File, file = \"{0}\"", repoFileName),e));
        }

        // Validate the contents
        String schemaFileName = String.Format("{0}.xsd.InstanceList.xsd",defaultNameSpace);
        StreamReader schema;

        try 
        {
          schema = FileIO.GetFileFromResources(schemaFileName);
        }
        catch(Exception e) 
        {
          throw(new ConfigurationErrorsException(
            String.Format(
            "Failed to load embedded schema file, cannot validate instance repository file. Schema file = \"{0}\"", schemaFileName),e));
        }

        // Validate file, get 10 errors max
        XmlValidator xv = new XmlValidator();
        String errors = xv.ValidateStream(r.BaseStream,schema.BaseStream,10);

        if(errors != "") 
        {
          throw(new ConfigurationErrorsException(
            String.Format(
            "The instance repository file contains syntax errors, file = \"{0}\"\n{1}", repoFileName,errors)));
        }

        il = null;

        try 
        {
          r.BaseStream.Position = 0;
          il = s.Deserialize(r) as InstanceList;
        }
        catch (Exception e) 
        {
          throw(new ConfigurationErrorsException(
            String.Format(
            "Problems reading the instance repository file, file = \"{0}\"", repoFileName),e));
        }
      } 
      finally 
      {
        if(r != null) 
        {
          r.Close();
        }
      }
    }

    public ServerConfig ReadServerConfig(String name,String fileName) 
    {
      ServerConfig sc = null;

      // Init
      XmlSerializer s = new XmlSerializer(typeof(ServerConfig));
      StreamReader r = null;

      try 
      {
        try
        {
          int retryCount = 1;

          if(FileIO.FileExists(fileName)) 
          {
            retryCount = 10;
          }

          while(true) 
          {
            try
            {
              r = new StreamReader(fileName);
              break;
            }
            catch (IOException ioe) 
            {
              retryCount--;
              if(retryCount > 0) 
              {
                Thread.Sleep(200);
              }
              else
                throw(ioe);
            }
          }
        }
        catch (Exception e) 
        {
          throw(new ConfigurationErrorsException(
            String.Format(
            "Problems reading the instance {0} configuration file, file = \"{1}\"", name, fileName),e));
        }

        // Validate the contents
        String schemaFileName = String.Format("{0}.xsd.ServerConfig.xsd",defaultNameSpace);
        StreamReader schema;

        try 
        {
          schema = FileIO.GetFileFromResources(schemaFileName);
        }
        catch(Exception e) 
        {
          throw(new ConfigurationErrorsException(
            String.Format(
            "Failed to load embedded schema file, cannot validate server configuration file. Schema file = \"{0}\"", schemaFileName),e));
        }

        // Validate file, get 10 errors max
        XmlValidator xv = new XmlValidator();
        String errors = xv.ValidateStream(r.BaseStream,schema.BaseStream,10);

        if(errors != "") 
        {
          throw(new ConfigurationErrorsException(
            String.Format(
            "The instance {0} configuration file contains syntax errors, file = \"{1}\"\n{2}", name, fileName,errors)));
        }

        try 
        {
          r.BaseStream.Position = 0;
          sc = s.Deserialize(r) as ServerConfig;
        }
        catch (Exception e) 
        {
          throw(new ConfigurationErrorsException(
            String.Format(
            "Problems reading the instance {0} configuration file, file = \"{1}\"", name, fileName),e));
        }
      } 
      finally 
      {
        if(r != null) 
        {
          r.Close();
        }
      }

      return(sc);
    }

    public String ReloadError
    {
      get
      {
        return(reloadError);
      }
    }

    private void ReloadInstance() 
    {
      String fileName = currentFileName;
      currentInstance = ReadServerConfig(currentInstance.SystemId,fileName);
    }


    public void LoadInstance(String name) 
    {
      // Lookup file that is referenced using instancelist
      String fileName = null;

      foreach(InstanceType i in il.Instance) 
      {
        if(i.Id == name)
        {
          fileName = i.File;
          break;
        }
      }

      if(fileName == null) 
      {
        throw(new ConfigurationErrorsException(
          String.Format(
          "The instance {0} was not found in the instance repository file, file = \"{1}\"", name, repoFileName)));
      }

      if(fileName == "") 
      {
        throw(new ConfigurationErrorsException(
          String.Format(
          "The instance {0} configuration file name was not defined in the instance repository file, file = \"{1}\"", name, repoFileName)));
      }

      currentInstance = ReadServerConfig(name,fileName);
      currentFileName = fileName;

      // set up a watch for changes
      if(fsw == null) 
      {
        FileInfo currentFile = new FileInfo(fileName);
        fsw = new FileSystemWatcher();
        fsw.IncludeSubdirectories = false;
        fsw.Path = currentFile.DirectoryName;
        fsw.Filter = currentFile.Name;
        fsw.NotifyFilter = NotifyFilters.LastWrite;
        fsw.Changed += new FileSystemEventHandler(ConfigFileChanged);
        fsw.EnableRaisingEvents = true;
        currentFile = null;
      }
    }

    private void ConfigFileChanged(object sender, FileSystemEventArgs e)
    {
      if(e.ChangeType != WatcherChangeTypes.Changed)
        return;

      try 
      {
        fsw.EnableRaisingEvents = false;

        reloadError = "";

        if(currentInstance != null) 
        {
          try 
          {
            ReloadInstance();
          }
          catch(Exception ex) 
          {
            reloadError = String.Format("The server configuration file was changed, the file contains errors.\nThe old configuration will be used until the error has been corrected\nor the server has been restarted.\nError: {0}",ex.Message);
          }
        }

        OnConfigChanged();
      }
      finally 
      {
        fsw.EnableRaisingEvents = true;
      }
    }

  }
}
