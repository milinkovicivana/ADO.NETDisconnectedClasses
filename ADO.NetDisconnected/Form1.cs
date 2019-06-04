using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADO.NetDisconnected
{
    public partial class Form1 : Form
    {

        DataTable customers, orders;
        public static DataTable orderDetails;
        DataSet sales;

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = customers.NewRow();
                if (String.IsNullOrWhiteSpace(txtCompanyName.Text))
                {
                    MessageBox.Show("Unesite ime kupca");
                    txtCompanyName.Focus();
                    return;
                }
                else
                {
                    dr["CompanyName"] = txtCompanyName.Text;
                }

                dr["Address"] = txtAddress.Text;
                customers.Rows.Add(dr);
                MessageBox.Show("Novi kupac je dodat!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = orders.NewRow();
                dr["Custid"] = cbCustomer.SelectedValue;
                orders.Rows.Add(dr);
                MessageBox.Show("Nova faktura je dodata!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void btnAddOrderDetails_Click(object sender, EventArgs e)
        {
            string selectedOrder;

            try
            {
                if (gridOrders.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Izaberite fakturu!");
                    return;
                }
                else
                {
                    selectedOrder = gridOrders.SelectedRows[0].Cells[0].Value.ToString();
                }

                Form2 x = new Form2(selectedOrder);
                x.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                customers = new DataTable("Customers");

                DataColumn custid = new DataColumn("Custid")
                {
                    DataType = typeof(int),
                    AllowDBNull = false,
                    AutoIncrement = true,
                    AutoIncrementSeed = 1,
                    AutoIncrementStep = 1
                };

                DataColumn companyName = new DataColumn("CompanyName")
                {
                    DataType = typeof(string),
                    AllowDBNull = false,
                    MaxLength = 50
                };

                DataColumn address = new DataColumn("Address")
                {
                    DataType = typeof(string),
                    AllowDBNull = true,
                    MaxLength = 200
                };

                customers.Columns.Add(custid);
                customers.Columns.Add(companyName);
                customers.Columns.Add(address);

                customers.PrimaryKey = new DataColumn[] { custid };

                orders = new DataTable("Orders");

                DataColumn orderid = new DataColumn("Orderid")
                {
                    DataType = typeof(int),
                    AllowDBNull = false,
                    AutoIncrement = true,
                    AutoIncrementSeed = 1,
                    AutoIncrementStep = 1
                };

                DataColumn customerid = new DataColumn("Custid")
                {
                    DataType = typeof(int),
                    AllowDBNull = false
                };

                DataColumn date = new DataColumn("Date")
                {
                    DataType = typeof(DateTime),
                    AllowDBNull = false,
                    DefaultValue = DateTime.Now
                };

                orders.Columns.Add(orderid);
                orders.Columns.Add(customerid);
                orders.Columns.Add(date);

                orders.PrimaryKey = new DataColumn[] { orderid };

                orderDetails = new DataTable("OrderDetails");

                DataColumn ordid = new DataColumn("Orderid")
                {
                    DataType = typeof(int),
                    AllowDBNull = false,
                };

                DataColumn productName = new DataColumn("ProductName")
                {
                    DataType = typeof(string),
                    AllowDBNull = false,
                    MaxLength = 40
                };

                DataColumn unitPrice = new DataColumn("UnitPrice")
                {
                    DataType = typeof(decimal),
                    AllowDBNull = false
                };

                orderDetails.Columns.Add(ordid);
                orderDetails.Columns.Add(productName);
                orderDetails.Columns.Add(unitPrice);

                orderDetails.PrimaryKey = new DataColumn[] { ordid, productName };

                sales = new DataSet("Sales");
                sales.Tables.Add(customers);
                sales.Tables.Add(orders);
                sales.Tables.Add(orderDetails);

                DataRelation rel1 = new DataRelation("RelationCustomersOrders", customers.Columns["Custid"], orders.Columns["Custid"], true);
                sales.Relations.Add(rel1);

                DataRelation rel2 = new DataRelation("RelationOrdersOrderDetails", orders.Columns["Orderid"], orderDetails.Columns["Orderid"], true);
                sales.Relations.Add(rel2);

                ForeignKeyConstraint fk1 = (ForeignKeyConstraint)orders.Constraints["RelationCustomersOrders"];
                fk1.UpdateRule = Rule.None;
                fk1.DeleteRule = Rule.None;

                ForeignKeyConstraint fk2 = (ForeignKeyConstraint)orderDetails.Constraints["RelationOrdersOrderDetails"];
                fk2.UpdateRule = Rule.None;
                fk2.DeleteRule = Rule.None;

                gridCustomers.DataSource = customers;
                gridOrders.DataSource = orders;
                gridOrderDetails.DataSource = orderDetails;

                cbCustomer.DataSource = customers;
                cbCustomer.DisplayMember = "CompanyName";
                cbCustomer.ValueMember = "Custid";
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void btnSaveToXML_Click(object sender, EventArgs e)
        {
            sales.WriteXml(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\sales.xml", XmlWriteMode.WriteSchema);
            MessageBox.Show("Podaci su uspesno sacuvani!");
        }

        public Form1()
        {
            InitializeComponent();

        }
            
    }
}
