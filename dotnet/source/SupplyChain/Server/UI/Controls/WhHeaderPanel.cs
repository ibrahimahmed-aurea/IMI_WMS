using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Imi.SupplyChain.Server.UI.Controls
{
  /// <summary>
  /// Summary description for WhHeaderPanel.
  /// </summary>
  [ToolboxBitmap(typeof(WhHeaderPanel),"vivaldi.ico")]
  public class WhHeaderPanel : System.Windows.Forms.Panel
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;
    private System.Windows.Forms.PictureBox TheLine;
    private System.Windows.Forms.Label TheLabel;

    public WhHeaderPanel()
    {
      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();

      // TODO: Add any initialization after the InitComponent call
      AdjustHeader();
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WhHeaderPanel));
      this.TheLine = new System.Windows.Forms.PictureBox();
      this.TheLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // TheLine
      // 
      this.TheLine.BackColor = System.Drawing.Color.Transparent;
      this.TheLine.Image = ((System.Drawing.Image)(resources.GetObject("TheLine.Image")));
      this.TheLine.Location = new System.Drawing.Point(100, 12);
      this.TheLine.Name = "TheLine";
      this.TheLine.Size = new System.Drawing.Size(192, 18);
      this.TheLine.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.TheLine.TabIndex = 0;
      this.TheLine.TabStop = false;
      // 
      // TheLabel
      // 
      this.TheLabel.AutoSize = true;
      this.TheLabel.Location = new System.Drawing.Point(17, 17);
      this.TheLabel.Name = "TheLabel";
      this.TheLabel.Size = new System.Drawing.Size(0, 16);
      this.TheLabel.TabIndex = 1;
      this.TheLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // WhHeaderPanel
      // 
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.TheLine);
      this.Controls.Add(this.TheLabel);
      this.Size = new System.Drawing.Size(280, 40);
      this.Dock = DockStyle.Top;
      this.ResumeLayout(false);
    }
    #endregion

    protected override void OnPaint(PaintEventArgs pe)
    {
      // TODO: Add custom paint code here

      // Calling the base class OnPaint
      base.OnPaint(pe);
    }

    private void AdjustHeader() 
    {
      TheLabel.Left = 8;
      TheLabel.Top  = 12;
      TheLine.Width = Width - TheLabel.Width - TheLabel.Left - 1;
      TheLine.Left = TheLabel.Width + TheLabel.Left;
      TheLine.Top  = 12;
    }

    protected override void OnResize(EventArgs eventargs) 
    {
      base.OnResize(eventargs);
      AdjustHeader();
    }

    protected override void OnCreateControl() 
    {
      base.OnCreateControl();
      AdjustHeader();
    }

    [
    Browsable(true),
    EditorBrowsable(EditorBrowsableState.Always),
    ]
    public override string Text 
    {
      get 
      {
        return base.Text;
      }
      set 
      {
        base.Text = value;
        TheLabel.Text = base.Text;;
        AdjustHeader();
      }
    }

  }

}
