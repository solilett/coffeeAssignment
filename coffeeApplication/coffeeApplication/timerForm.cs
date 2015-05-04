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
    public partial class timerForm : Form
    {
        public timerForm()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //this.Hide();
            SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\solilett\Desktop\University Work\Year 3\Semester 2\SDAF\Assignment\CoffeeWebsite\CoffeeWebsite\App_Data\aspnet-CoffeeWebsite-20150319013826.mdf';Integrated Security=True;Connect Timeout=30");
            string query = "SELECT tProductWeight.stock, tProduct.name, tProductWeight.netWeight, tProductWeight.grind FROM tProduct, tProductWeight WHERE tProduct.id = tProductWeight.productId AND stock < 1;";
            SqlCommand comm = new SqlCommand(query, conn);
            conn.Open();
            using (SqlDataReader reader = comm.ExecuteReader())
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                int rowCount = dt.Rows.Count;

                if (rowCount > globalVariables.LowStockCountFirst)
                {
                    MessageBox.Show("A product has just ran out of stock");
                    globalVariables.LowStockCountFirst = rowCount;
                }

            }
            conn.Close();
        }

        private void timerForm_Load(object sender, EventArgs e)
        {
            var form2 = new Form2();
            form2.Show();
            timer1.Start();
        }
    }
}
