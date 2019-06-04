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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(string orderid)
        {
            InitializeComponent();

            this.Text = orderid;
            
        }

        private void btnAddOrdDetails_Click(object sender, EventArgs e)
        {

            try
            {
                DataRow dr = Form1.orderDetails.NewRow();
                dr["Orderid"] = this.Text;

                if (String.IsNullOrWhiteSpace(txtProductName.Text))
                {
                    MessageBox.Show("Unesite naziv stavke");
                    txtProductName.Focus();
                    return;
                }
                else
                {
                    dr["ProductName"] = txtProductName.Text;
                }

                if (String.IsNullOrWhiteSpace(txtUnitPrice.Text))
                {
                    MessageBox.Show("Unesite cenu");
                    txtUnitPrice.Focus();
                    return;
                }
                else
                {
                    dr["UnitPrice"] = Convert.ToDecimal(txtUnitPrice.Text);
                }
                               
                Form1.orderDetails.Rows.Add(dr);

                MessageBox.Show("Stavke za fakturu sa id-jem " + this.Text + " su uspesno dodate!");

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            
        }
    }
}
