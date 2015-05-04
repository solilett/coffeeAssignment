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
    public partial class Form2 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\solilett\Desktop\University Work\Year 3\Semester 2\SDAF\Assignment\CoffeeWebsite\CoffeeWebsite\App_Data\aspnet-CoffeeWebsite-20150319013826.mdf';Integrated Security=True;Connect Timeout=30");
        public int numRowsFirstCount = 0;
        public int messageCount = 0;
        public Form2()
        {
            InitializeComponent();
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form3 = new Form3();
            form3.Closed += (s, args) => this.Close();
            form3.Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form5 = new Form5();
            form5.Show();
        }

        private void btnEditProducts_Click(object sender, EventArgs e)
        {
            this.Hide();
            var allProducts = new allProducts();
            allProducts.Show();
        }

        public void checkLowStock() {
            string query = "SELECT tProductWeight.stock, tProduct.name, tProductWeight.netWeight, tProductWeight.grind FROM tProduct, tProductWeight WHERE tProduct.id = tProductWeight.productId AND stock < 1;";
            SqlCommand comm = new SqlCommand(query, conn);
            conn.Open();
            using (SqlDataReader reader = comm.ExecuteReader())
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                numRowsFirstCount = dt.Rows.Count;
                globalVariables.LowStockCountFirst = numRowsFirstCount;

                    if (numRowsFirstCount > 1)
                    {
                            MessageBox.Show("There are currently " + numRowsFirstCount.ToString() + " product out of stock");
                    }
    
            }
            conn.Close();
        }


   

        private void Form2_Load(object sender, EventArgs e)
        {
            if(globalVariables.loadCount == 0)
            {
                checkLowStock();
                globalVariables.loadCount++;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            globalVariables.editMainProduct = true;
            this.Hide();
            var allProducts = new allProducts();
            allProducts.Show();
        }

    }
}
