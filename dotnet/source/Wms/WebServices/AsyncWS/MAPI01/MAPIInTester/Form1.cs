using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MAPIInTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MAPIIn.InboundInterface ops = new MAPIIn.InboundInterface())
            {
                {
                    ops.Url = textBox1.Text;
                    string mhId = "01";
                    string Transid = "1241";
                    MAPIIn.ProductConfirmation_01Doc m = new MAPIInTester.MAPIIn.ProductConfirmation_01Doc();
                    m.OPCODE = "1";
                    m.ProductNumber = "1000009";
                    m.MaterialHandlingSystemId = "01";
                    m.ReturnCode = "1";
                    ops.ProductConfirmation_01(mhId, Transid, m);
                    if (m.ReturnCode != "12342")
                    {
                    }
                }
                {
                    ops.Url = textBox1.Text;
                    string mhId = "01";
                    string Transid = "1242";
                    MAPIIn.MovementInConfirmation_01Doc m = new MAPIInTester.MAPIIn.MovementInConfirmation_01Doc();
                    m.OPCODE = "1";
                    m.ItemLoadIdentity = "370704630070035027";
                    m.MaterialHandlingSystemId = "01";
                    m.ToLocationAddress = "A02B";
                    m.ReturnCode = "1";
                    ops.MovementInConfirmation_01(mhId, Transid, m);
                    if (m.ReturnCode != "12342")
                    {
                    }
                }
            }
            /*using (MAPIIn.InboundInterface ops = new MAPIIn.InboundInterface())
            {
                ops.Url = textBox1.Text;
                string mhId = "01";
                string Transid = "1238";
                MAPIIn.MovementOutConfirmation_01Doc m = new MAPIInTester.MAPIIn.MovementOutConfirmation_01Doc();
                m.OPCODE = "1";
                m.ItemLoadIdentity = "370704630070035027";
                m.MaterialHandlingSystemId = "01";
                m.MovementOrderId = 660505;
                m.MovementOrderType = "FP";
                m.ToLocationAddress = "OUT";
                m.ReturnCode = "1";
                ops.MovementOutConfirmation_01(mhId, Transid, m);
                if (m.ReturnCode != "12342")
                {
                }
            }*/
            /*using (MAPIIn.InboundInterface ops = new MAPIIn.InboundInterface())
            {
                ops.Url = textBox1.Text;
                string mhId = "01";
                string Transid = "1237";
                MAPIIn.StatusUpdateConfirmation_01Doc m = new MAPIInTester.MAPIIn.StatusUpdateConfirmation_01Doc();
                m.OPCODE = "1";
                m.ItemLoadIdentity = "370704630070035027";
                m.MaterialHandlingSystemId = "01";
                m.FifoDate = System.Convert.ToDateTime("2008-01-01");
                m.HoldCode = "H";
                m.ReturnCode = "1";
                ops.StatusUpdateConfirmation_01(mhId, Transid, m);
                if (m.ReturnCode != "12342")
                {
                }
            }*/
            /*using (MAPIIn.InboundInterface ops = new MAPIIn.InboundInterface())
            {
                ops.Url = textBox1.Text;
                string mhId = "01";
                string Transid = "1236";
                MAPIIn.MovementInConfirmation_01Doc m = new MAPIInTester.MAPIIn.MovementInConfirmation_01Doc();
                m.OPCODE = "1";
                m.ItemLoadIdentity = "456";
                m.MaterialHandlingSystemId = "01";
                m.ToLocationAddress = "A04B";
                m.ReturnCode = "0";
                ops.MovementInConfirmation_01(mhId, Transid, m);
                if (m.ReturnCode != "12342")
                {
                }
            }*/
        }

    }
}