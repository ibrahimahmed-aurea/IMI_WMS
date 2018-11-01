using System;

namespace Imi.CodeGenerators.WebServices.Framework
{
  public class StringUtils
  {
    // Name:        CapitalizeRemoveUnderscore
    // Description: Function for capitalizing a string to make first characters after an underscore
    //              to uppercase and remove the underscore. The first character will however be lowercase.
    //
    // Example:     string mystring = StringUtils.CapitalizeRemoveUnderscore("this_is_a_test");
    //              mystring will contain "thisIsATest".
    static public string CapitalizeRemoveUnderscore( string inStr )
    {
      string    workStr;
      bool      uflag;
      char      x;

      if ( inStr == null )
        return "";

      inStr = inStr.ToLower();

      uflag = true;
      workStr = "";

      foreach ( char c in inStr )
      {
        x = c;

        // check if prev char was _ if so then uppercase
        if (uflag)
          x = Char.ToUpper( x );
        
        // remeber current char to next lap
        uflag = (c == '_');

        // if current char is not _ then add to string
        if (!uflag)
          workStr += x;
      }

      return workStr;
    }

  }

}
