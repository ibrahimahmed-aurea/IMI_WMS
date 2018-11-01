using System;
using System.Collections.Specialized;
using System.Collections;

// ----------------------------------------------------------------------------

namespace Imi.CodeGenerators.WebServices.Framework
{
  
  // ----------------------------------------------------------------------------

  public class Tokenizer
  {
    public StringCollection Tokens;
    
    // ----------------------------------------------------------------------------
    
    public Tokenizer( string s, string delimStr )
    {
      char[] delims = delimStr.ToCharArray();
      int nOldPos = 0;
      int nPos = 0;
      Tokens = new StringCollection();

      foreach ( char c in s )
      {
        foreach ( char d in delims )
        {
          if ( d == c )
          {
            // OldPos is start of token
            // Pos is character after token
            AddNotEmpty( s.Substring( nOldPos, nPos - nOldPos ) );
            AddNotEmpty( s.Substring( nPos, 1 ) );

            nOldPos = nPos + 1;
            break;
          }
        }
        nPos++;
      }
      AddNotEmpty( s.Substring( nOldPos ) );
    }

    // ----------------------------------------------------------------------------
    
    private void AddNotEmpty( String token )
    {
      if ( token.Length != 0 )
        Tokens.Add( token );
    }

    // ----------------------------------------------------------------------------
    
    public string Peek( int ixcurrent, int step )
    {
      if ( Tokens.Count > ( ixcurrent + step ) )
        return Tokens[ ( ixcurrent + step ) ];
      else
        return "";
    }

    // ----------------------------------------------------------------------------
    
    public bool EOF( int ixcurrent )
    {
      return ( Tokens.Count <= ixcurrent );
    }
  }

  // ----------------------------------------------------------------------------

  public class SourceTokenizer : Tokenizer
  {
    // ----------------------------------------------------------------------------
    
    public SourceTokenizer( string s, string delimStr ) : base( s, delimStr )
    {}

    // ----------------------------------------------------------------------------
    
    public bool IsString( int ixCurrent, string StringDelim, ref string TheString, ref int ixLast )
    {
      // String delimiter in string NOT supported
      TheString = "";
      ixLast = ixCurrent;

      if ( Peek( ixCurrent, 0 ) == StringDelim )
      {
        int step = 1;
        
        while ( !EOF( ixCurrent + step ) )
        {
          if ( Peek( ixCurrent, step ) == StringDelim )
          {
            ixLast = ixCurrent + step;
            return true;
          }
          else
            TheString += Peek( ixCurrent, step );

          step++;
        }
        return false;
      }
      else
      {
        return false;
      }
    }

    // ----------------------------------------------------------------------------
    
    public bool IsComment( int ixCurrent, string CommentStartDelim, 
      string CommentEndDelim, ref string TheComment, ref int ixLast )
    {
      int i;
      TheComment = "";
      ixLast = ixCurrent;

      for ( i = 0; i < CommentStartDelim.Length; i++ )
        if ( CommentStartDelim[ i ].ToString() != Peek( ixCurrent, i ) )
          return false;

      int step = CommentStartDelim.Length;
        
      while ( !EOF( ixCurrent + step ) )
      {
        bool EndComment = true;

        for ( i = 0; i < CommentEndDelim.Length; i++ )
          if ( CommentEndDelim[ i ].ToString() != Peek( ixCurrent, step + i ) )
          {
            EndComment = false;
            break;
          }

        if ( EndComment )
        {
          ixLast = ixCurrent + step + ( CommentEndDelim.Length - 1 );
          return true;
        }
        else
          TheComment += Peek( ixCurrent, step );

        step++;
      }
      return false;
    }
    
    // ----------------------------------------------------------------------------
    
    public bool EatSpace( ref int ixCurrent )
    {
      bool bRet = false;

      while ( !EOF( ixCurrent ) )
      {
        if ( ( Peek( ixCurrent, 0 ) == " " ) || 
          ( Peek( ixCurrent, 0 ) == "\r" ) || 
          ( Peek( ixCurrent, 0 ) == "\n" ) || 
          ( Peek( ixCurrent, 0 ) == "\t" ) )
        {
          ixCurrent++;
          bRet = true;
        }
        else
          break;
      }
      return bRet;
    }
  }

  // ----------------------------------------------------------------------------
}
