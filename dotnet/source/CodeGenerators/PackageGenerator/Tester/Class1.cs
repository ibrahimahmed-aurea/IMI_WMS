using System;
using System.Data;
using PLSQLInterface;

namespace testapp
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//
			// TODO: Add code to start application here
			//
      OWDB owdb = new OWDB( "WHTRUNK", "owuser", "owuser" );
      Wlsystem wlsystem = new Wlsystem( owdb );
      
      Console.WriteLine( wlsystem._Debug() );

      string almtxt_o = "";
      string almid = "PRT001";
      string nlangcod = "EN";
      wlsystem.Getalmtxt( almid, nlangcod, ref almtxt_o );
      Console.WriteLine( almtxt_o );
		}
	}
}
