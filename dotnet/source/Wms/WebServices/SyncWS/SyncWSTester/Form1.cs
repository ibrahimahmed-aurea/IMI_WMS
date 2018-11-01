using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WebServiceTester.ProductSearchService;
using WebServiceTester.WarehouseService;
using WebServiceTester.CustomerService;
using WebServiceTester.OrderService;

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
            using (ProductSearchService.ProductSearchWebService ops = new ProductSearchService.ProductSearchWebService())
            {
                label1.Text = "call";
                ops.Url = textBox1.Text;
                ProductSearchService.productSearchParameters sp = new ProductSearchService.productSearchParameters();
                sp.clientId = "IMISTD"; // "HLM";
                sp.searchString = "72%"; // "721301"; //  "721004"; // "331601"; // "123456";
                sp.firstResult = 20;
                sp.maxResult = 10;
                sp.returnDetails = true;
                sp.stockNo = "910";
                ProductSearchService.productSearchResult sr = ops.findProductsByPartNo(sp);
                label1.Text = "null";
                if (sr != null)
                    label1.Text = Convert.ToString(sr.totalHits);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (ProductSearchService.ProductSearchWebService ops = new ProductSearchService.ProductSearchWebService())
            {
                label2.Text = "call";
                ops.Url = textBox1.Text;
                ProductSearchService.productSearchParameters sp = new ProductSearchService.productSearchParameters();
                sp.clientId = "IMISTD";
                sp.searchString = "%";
                sp.maxResult = null;
                ProductSearchService.productSearchResult sr = ops.findProductsByDescription(sp);
                label2.Text = "null";
                if (sr != null)
                    label2.Text = Convert.ToString(sr.totalHits);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            using (CustomerService.CustomerWebService ops = new CustomerService.CustomerWebService())
            {
                label5.Text = "call";
                ops.Url = textBox3.Text;
                customerSearchParameters sp = new customerSearchParameters();
                sp.clientId = "IMISTD";
                sp.customerId = "171"; // "20001"; // "171";
                sp.stockNo = "910";
                sp.maxResult = null;
                sp.returnDetails = true;
                customer customer = ops.findCustomerById(sp);
                label5.Text = customer.name;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
        }
        private void button5_Click(object sender, EventArgs e)
        {
            using (WarehouseService.WarehouseWebService ops = new WarehouseService.WarehouseWebService())
            {
                label5.Text = "call";
                ops.Url = textBox2.Text;
                warehouseSearchParameters sp = new warehouseSearchParameters();
                sp.clientId = "IMISTD";
                sp.maxResult = null;
                warehouseSearchResult sr = ops.findByClient(sp);
                label5.Text = "null";
                if (sr != null)
                {
                    string s = Convert.ToString(sr.totalHits);
                    foreach (warehouse w in sr.list)
                    {
                        s = s + " " + w.stockNo + "-" + w.name;
                    }
                    label1.Text = s;
                }
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            using (CustomerService.CustomerWebService ops = new CustomerService.CustomerWebService())
            {
                label5.Text = "call";
                ops.Url = textBox3.Text;
                customerSearchParameters sp = new customerSearchParameters();
                sp.clientId = "IMISTD";
                sp.customerId = "171";
                sp.maxResult = null;
                CustomerService.address[] list = ops.getAddresses(sp);
                label5.Text = Convert.ToString(list.GetLength(0));
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            using (OrderService.OrderWebService ops = new OrderService.OrderWebService())
            {
                label5.Text = "call";
                ops.Url = textBox4.Text;
                orderSearchParameters sp = new orderSearchParameters();
                sp.clientId = "IMISTD";
                sp.orderType = orderType.CO3PL;
                sp.orderNo = "OLLA_0810%"; // "tore%";
                //sp.yourCono = "778877";
                sp.ordersFromProduction = true;
                sp.ordersFromHistory = true;
                //sp.firstResult = 0;
                //sp.maxResult = 1;
                //sp.mark = "%";
                sp.returnDetails = true; //= false;
                orderSearchResult sr = ops.getOpenOrders(sp);
                label5.Text = "null";
                if (sr != null)
                    if (sr.list != null)
                        label5.Text = Convert.ToString(sr.list.GetLength(0));
                    else
                        label5.Text = "Empty list";


            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            using (OrderService.OrderWebService ops = new OrderService.OrderWebService())
            {
                label5.Text = "call";
                ops.Url = textBox4.Text;
                orderSaveParameters sp = new orderSaveParameters();
                sp.clientId = "IMISTD";
                sp.stockNo = "910";
                sp.order = new order();
                sp.order.orderType = orderType.CO3PL;
                sp.order.orderTypeSpecified = true;
                sp.order.coNo = "OLLA_081015_CO01";
                sp.order.customer = "171";
                sp.order.text = "Be very careful";
                sp.order.coMark = "The Mark";
                sp.order.customerOrderTypeId = "N";
                sp.order.methodOfShipmentId = "12";
                sp.order.orderLines = new orderLine[2];

                sp.order.orderLines[0] = new orderLine();
                sp.order.orderLines[0].partNo = "721004";
                sp.order.orderLines[0].qtyUnit = 10;
                sp.order.orderLines[0].delDate = DateTime.Now;
                sp.order.orderLines[0].unit = "EH";
                sp.order.orderLines[0].linePos = 1;
                sp.order.orderLines[0].lineSeq = 0;
                sp.order.orderLines[0].lineId = 0;

                sp.order.orderLines[1] = new orderLine();
                sp.order.orderLines[1].partNo = "721512";
                sp.order.orderLines[1].qtyUnit = 12;
                sp.order.orderLines[1].delDate = DateTime.Now;
                sp.order.orderLines[1].unit = "EH";
                sp.order.orderLines[1].linePos = 2;
                sp.order.orderLines[1].lineSeq = 0;
                sp.order.orderLines[1].lineId = 0;

                order order = ops.saveOrder(sp);

                label5.Text = "null";
                if (order != null)
                    label5.Text = order.coNo;
                else
                    label5.Text = "Empty list";


            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            using (CustomerService.CustomerWebService ops = new CustomerService.CustomerWebService())
            {
                label5.Text = "call";
                ops.Url = textBox3.Text;
                customerSearchParameters sp = new customerSearchParameters();
                sp.clientId = "IMISTD";
                sp.stockNo = "910";
                sp.maxResult = null;
                sp.returnDetails = true;
                customer customer = ops.findClientById(sp);
                label5.Text = customer.name;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            using (OrderService.OrderWebService ops = new OrderService.OrderWebService())
            {
                label5.Text = "call";
                ops.Url = textBox4.Text;
                orderSaveParameters sp = new orderSaveParameters();
                sp.clientId = "IMISTD";
                //sp.stockNo = "910";
                sp.order = new order();
                sp.order.orderType = orderType.PO;
                sp.order.orderTypeSpecified = true;
                sp.order.text = "Be very careful";
                sp.order.poid = "OLLA_081015_PO02";
                sp.order.poSeq = 1;
                sp.order.reqDelDate = DateTime.Now;
                sp.order.shipFromPartyid = "900123";
                sp.order.receivingWhid = "910";
                sp.order.orderLines = new orderLine[2];

                sp.order.orderLines[0] = new orderLine();
                sp.order.orderLines[0].partNo = "888888A";
                sp.order.orderLines[0].partDescr1 = "Sprite";
                sp.order.orderLines[0].text = "det kommer jag ärligt talat inte ihåg vad det är. det är sprite eller något sådant där";
                sp.order.orderLines[0].qtyUnit = 888;
                sp.order.orderLines[0].unit = "BTL";
                sp.order.orderLines[0].linePos = 1;
                sp.order.orderLines[0].lineSeq = 0;

                sp.order.orderLines[1] = new orderLine();
                sp.order.orderLines[1].partNo = "888888B";
                sp.order.orderLines[1].partDescr1 = "Fanta";
                sp.order.orderLines[1].text = "instruction2";
                sp.order.orderLines[1].qtyUnit = 333;
                sp.order.orderLines[1].unit = "BTL";
                sp.order.orderLines[1].linePos = 2;
                sp.order.orderLines[1].lineSeq = 0;

                order order = ops.saveOrder(sp);

                label5.Text = "null";
                if (order != null)
                    label5.Text = order.coNo;
                else
                    label5.Text = "Empty list";


            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            using (OrderService.OrderWebService ops = new OrderService.OrderWebService())
            {
                label5.Text = "call";
                ops.Url = textBox4.Text;
                orderSearchParameters sp = new orderSearchParameters();
                sp.clientId = "IMISTD";
                sp.orderType = orderType.PO;
                sp.orderNo = "OLLA_0810%"; // "tore%";
                //sp.yourCono = "778877";
                sp.ordersFromProduction = true;
                sp.ordersFromHistory = true;
                //sp.firstResult = 0;
                //sp.maxResult = 1;
                //sp.mark = "%";
                sp.returnDetails = true; //= false;
                orderSearchResult sr = ops.getOpenOrders(sp);
                label5.Text = "null";
                if (sr != null)
                    if (sr.list != null)
                        label5.Text = Convert.ToString(sr.list.GetLength(0));
                    else
                        label5.Text = "Empty list";


            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            using (CustomerService.CustomerWebService ops = new CustomerService.CustomerWebService())
            {
                label5.Text = "call";
                ops.Url = textBox3.Text;
                partySearchParameters sp = new partySearchParameters();
                sp.clientId = "IMISTD";
                sp.partyId = "900123"; // "20001"; // "171";
                sp.partyType = CustomerService.partyType.SU;
                sp.partyTypeSpecified = true;
                sp.maxResult = null;
                sp.returnDetails = true;
                customer customer = ops.findPartyById(sp);
                label5.Text = customer.name;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            using (OrderService.OrderWebService ops = new OrderService.OrderWebService())
            {
                label5.Text = "call";
                ops.Url = textBox4.Text;
                customerOrderTypeSearchResult sr = ops.getCustomerOrderTypes();
                label5.Text = "null";
                if (sr != null)
                {
                    string s = Convert.ToString(sr.totalHits);
                    foreach (customerOrderType w in sr.list)
                    {
                        s = s + " " + w.customerOrderTypeId + "-" + w.customerOrderTypeName;
                    }
                    label1.Text = s;
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            using (OrderService.OrderWebService ops = new OrderService.OrderWebService())
            {
                label5.Text = "call";
                ops.Url = textBox4.Text;
                methodOfShipmentSearchParameters sp = new methodOfShipmentSearchParameters();
                sp.clientId = "IMISTD";
                methodOfShipmentSearchResult sr = ops.getMethodOfShipments(sp);
                label5.Text = "null";
                if (sr != null)
                {
                    string s = Convert.ToString(sr.totalHits);
                    foreach (methodOfShipment w in sr.list)
                    {
                        s = s + " " + w.methodOfShipmentId + "-" + w.methodOfShipmentName;
                    }
                    label1.Text = s;
                }
            }
        }

        /*
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
                                        */
    }
}