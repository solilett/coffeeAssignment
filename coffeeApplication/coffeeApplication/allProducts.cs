using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace coffeeApplication
{
    public partial class allProducts : Form
    {
        public allProducts()
        {
            InitializeComponent();
        }

        private void allProducts_Load(object sender, EventArgs e)
        {
            string select = "SELECT tProduct.Id AS 'ID', tProduct.name AS 'Name', COUNT(tProductWeight.productId) AS 'number of products' FROM tProduct LEFT JOIN tProductWeight ON tProduct.id = tProductWeight.productId GROUP BY tProduct.Id, tProduct.name";

            SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\solilett\Desktop\University Work\Year 3\Semester 2\SDAF\Assignment\CoffeeWebsite\CoffeeWebsite\App_Data\aspnet-CoffeeWebsite-20150319013826.mdf';Integrated Security=True;Connect Timeout=30");
            SqlDataAdapter dataadapter = new SqlDataAdapter(select, conn);
            DataSet ds = new DataSet();
            conn.Open();
            dataadapter.Fill(ds, "tProduct");
            conn.Close();
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "tProduct";
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            globalVariables.justAddProductId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            if (globalVariables.editMainProduct == false)
            {
                globalVariables.editProduct = true;
                this.Hide();
                var form6 = new Form6();
                form6.Show();
            }
            else
            {
                this.Hide();
                var editMainProduct = new editMainProduct();
                editMainProduct.Show();
            }
        }

        private void allProducts_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            var form2 = new Form2();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }
    }
}
