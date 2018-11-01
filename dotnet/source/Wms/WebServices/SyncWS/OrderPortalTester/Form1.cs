using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WebServiceTester.OrderPortalService;

namespace WebServiceTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OrderPortalService.OrderPortalService ops = new OrderPortalService.OrderPortalService())
            {
                ops.Url = textBox1.Text;
                label1.Text = ops.WhoAmI();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OrderPortalService.OrderPortalService ops = new OrderPortalService.OrderPortalService())
            {
                ops.Url = textBox1.Text;
                try
                {
                    CustomerOrderInfoDoc coi = ops.GetCustomerOrderInfo("partner", "EN", "IMISTD", "JOPL_060725_02", null);
                    if (coi != null)
                    {
                        if (coi.aCustomerOrderList != null)
                            label2.Text = Convert.ToString(coi.aCustomerOrderList.GetLength(0));
                        else
                            label2.Text = "null";
                    }
                    else
                    {
                        label2.Text = "Failed";
                    }
                }
                catch (Exception ex)
                {
                    label2.Text = ex.ToString();
                }
            }
        }

        private bool callit(string partnername)
        {
            bool result = false;
            using (OrderPortalService.OrderPortalService ops = new OrderPortalService.OrderPortalService())
            {
                ops.Url = textBox1.Text;
                try
                {
                    CustomerOrderInfoDoc coi = ops.GetCustomerOrderInfo(partnername, "EN", "IMISTD", "JOPL_060725_02", null);
                    if (coi != null)
                    {
                        if (coi.aCustomerOrderList != null)
                            result = true;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label3.Text = "";
            int count = 100;
            for (int i = 0; i < count; i++)
            {
                if (!callit("partnername"))
                {
                    label3.Text = "Failed after " + Convert.ToString(i);
                    break;
                }
                label3.Text = Convert.ToString(i);
            }
            label3.Text = Convert.ToString(count) + " called ok";
        }

        private bool callwhoami()
        {
            using (OrderPortalService.OrderPortalService ops = new OrderPortalService.OrderPortalService())
            {
                ops.Url = textBox1.Text;
                try
                {
                    string s = ops.WhoAmI();
                    return true;
                }
                catch
                {
                }

            }
            return false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label4.Text = "";
            int count = 1000;
            for (int i = 0; i < count; i++)
            {
                if (!callwhoami())
                {
                    label4.Text = "Failed after " + Convert.ToString(i);
                    break;
                }
                label4.Text = Convert.ToString(i);
            }
            label4.Text = Convert.ToString(count) + " called ok";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OrderPortalService.OrderPortalService ops = new OrderPortalService.OrderPortalService())
            {
                ops.Url = textBox1.Text;
                try
                {
                    CustomerOrderInfoDoc coi = ops.GetCustomerOrderInfo(null, "EN", "IMISTD", "JOPL_060725_02", null);
                    if (coi != null)
                    {
                        if (coi.aCustomerOrderList != null)
                            label5.Text = Convert.ToString(coi.aCustomerOrderList.GetLength(0));
                        else
                            label5.Text = "null";
                    }
                    else
                    {
                        label5.Text = "Failed";
                    }
                }
                catch (Exception ex)
                {
                    label5.Text = ex.ToString();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            int count = 1000;
            for (int i = 0; i < count; i++)
            {
                if (!callit(null))
                {
                    label6.Text = "Failed after " + Convert.ToString(i);
                    break;
                }
                label6.Text = Convert.ToString(i);
            }
            label6.Text = Convert.ToString(count) + " called ok";
        }

    }
}