using System;


namespace pkgcc2
{
  /// <summary>
  /// Summary description for TextUtility.
  /// </summary>
  public class TextUtility
  {
    private static char[] delimiterArr = {'\r','\n'};

    /// <summary>
    /// Capitalizes a caption made up of several words separated by spaces.
    /// example:
    /// 
    /// Ship-To Party is converted to ShipToParty
    /// </summary>
    /// <param name="text">The text to convert</param>
    /// <returns>Converted String</returns>
    public static String CapitalizeName(String text)
    {
      String capitalizedName = "";
      bool flag = false;

      foreach(char c in text) 
      {
        if ((c == ' ') || (c == '(') || (c == ')'))
          flag = true;
        else
        {
          if ((c == '.') || (c == '-') || (c == '=')) 
            continue;
        
          if (flag) 
            capitalizedName = capitalizedName + Convert.ToString(c).ToUpper();
          else
            capitalizedName = capitalizedName + c;

          flag = false;
        }
      }

      return( capitalizedName);
    }

    /// <summary>
    /// Returns a nicely formatted Oracle data type declaration based on the provided parameters.
    /// </summary>
    /// <param name="datatype"></param>
    /// <param name="length"></param>
    /// <param name="precision"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public static String GetDataTypeDeclaration(String datatype, String length, String precision,  String scale)
    {

      String typeDeclaration = datatype;

      switch(datatype) 
      {
        case "VARCHAR2":
          typeDeclaration = datatype + "(" + length + ")";
          break;
        case "NUMBER":
          if ((scale != "") && (scale != "0")) 
            typeDeclaration =datatype + "(" + precision + "," + scale + ")";
          else
          {
            if (precision != "" )
              typeDeclaration =datatype + "(" + precision + ")";
          }
          break;

      }

      return(typeDeclaration);
    }

    public static String[] splitComment(String aComment) 
    {
      return(aComment.Split(delimiterArr,100));
    }
  }
}
