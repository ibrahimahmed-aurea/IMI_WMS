using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Imi.SupplyChain.Server.UI.Controls
{
  /// <summary>
  /// Summary description for WhDataGrid.
  /// </summary>
  [ToolboxBitmap(typeof(WhHeaderPanel),"vivaldi.ico")]
  public class WhDataGrid : System.Windows.Forms.DataGrid
  {
    private bool     fFullRowSelect   = false;
    private DataView fDataView        = null;
    private String   fFullRowKeyField = null;
    private String   fFullRowKeyValue = null;

    private bool   fCaptionCountVisible  = false;
    private String fCaptionTextOriginal  = "";
    private String fCaptionCountText     = "(%s Items)";
    private int    fCurrCount            = -1;

    private DataView DefaultDataView 
    {
      get 
      {
        if(fDataView == null) 
        {
          if(this.DataSource is DataView) 
          {
            fDataView = (this.DataSource as DataView);
          }
          else 
          {
            if(this.DataSource is DataTable)
              fDataView = (this.DataSource as DataTable).DefaultView;
          }    
        }
        return(fDataView);
      }
    }


    private String FullRowKeyValue
    {
      get 
      {
        return (fFullRowKeyValue);
      }
      set
      {
        fFullRowKeyValue = value;
      }
    }


    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public WhDataGrid()
    {
      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();

      // TODO: Add any initialization after the InitComponent call
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if( components != null )
          components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Component Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WhDataGrid));
    }
    #endregion

    [
    Browsable(true),
    EditorBrowsable(EditorBrowsableState.Always),
    ]
    public bool CaptionCountVisible
    {
      get 
      {
        return (fCaptionCountVisible);
      }
      set
      {
        fCaptionCountVisible = value;
      }
    }

    [
    Browsable(true),
    EditorBrowsable(EditorBrowsableState.Always),
    ]
    public String CaptionCountText 
    {
      get 
      {
        return (fCaptionCountText);
      }
      set
      {
        fCaptionCountText = value;
      }
    }

    [
    Browsable(true),
    EditorBrowsable(EditorBrowsableState.Always),
    ]
    public bool FullRowSelect
    {
      get 
      {
        return (fFullRowSelect);
      }
      set
      {
        fFullRowSelect = value;
      }
    }


    [
    Browsable(true),
    EditorBrowsable(EditorBrowsableState.Always),
    ]
    public String FullRowKeyField
    {
      get 
      {
        return (fFullRowKeyField);
      }
      set
      {
        fFullRowKeyField = value;
      }
    }

    private void SetFullRowKeyValue()
    {
      if(FullRowSelect) 
      {
        if(DefaultDataView != null) 
        {
          if((FullRowKeyField != null) && (FullRowKeyField != "")) 
          {
            try 
            {
              FullRowKeyValue = DefaultDataView[this.CurrentCell.RowNumber][FullRowKeyField].ToString();
            }
            catch(Exception) 
            {
            }
          }
        }
      }
      else
        FullRowKeyValue = null;
    }

    protected override void OnCreateControl()
    {
      fCaptionTextOriginal = this.CaptionText;
      base.OnCreateControl();
    }

    private void LocalMouseDown(MouseEventArgs e)
    {
      System.Drawing.Point pt = new Point(e.X,e.Y);
      DataGrid.HitTestInfo hti = this.HitTest(pt);
      if(hti.Type == DataGrid.HitTestType.Cell)
      {
        SelectRow(hti.Row);
        SetFullRowKeyValue();
      }
    }

    private void LocalCurrentCellChanged(EventArgs e)
    {
      SelectRow(this.CurrentRowIndex);
      SetFullRowKeyValue();
    }

    protected override void OnCurrentCellChanged(EventArgs e)
    {
      if(!DesignMode)
        LocalCurrentCellChanged(e);

      base.OnCurrentCellChanged(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      if(!DesignMode)
        LocalMouseDown(e);

      base.OnMouseDown(e);
    }

    public void SelectRow(int row) 
    {
      if(row < 0)
        return;

      if(FullRowSelect) 
      {
        if(row != this.CurrentRowIndex) 
        {
          this.UnSelect(this.CurrentRowIndex);
        }
        this.CurrentRowIndex = row;
        this.Select(row);
      }
    }

    private void LocalOnPaint(PaintEventArgs pe)
    {
      if(CaptionCountVisible) 
      {
        if(fCaptionCountText != "") 
        {
          if((DefaultDataView != null) && (DefaultDataView.Table != null)) 
          {
            if(fCurrCount != DefaultDataView.Table.Rows.Count) 
            {
              fCurrCount = DefaultDataView.Table.Rows.Count; 
              this.CaptionText = fCaptionTextOriginal + " " + fCaptionCountText.Replace("%s", fCurrCount.ToString());
            }
          }
        }
      }

      if(FullRowKeyValue != null) 
      {
        DataView dv = DefaultDataView;

        int foundRow = this.CurrentRowIndex;

        for(int i = 0; i < dv.Table.Rows.Count; i++)
        {
          if( dv[i][FullRowKeyField].ToString() == FullRowKeyValue) 
          {
            foundRow = i;
            break;
          }
        }

        SelectRow(foundRow);
      }

    }

    protected override void OnLayout(LayoutEventArgs levent)
    {
      SelectRow(this.CurrentRowIndex);
      base.OnLayout(levent);
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
      if(!DesignMode)
        LocalOnPaint(pe);

      base.OnPaint(pe);
    }
  }
}
