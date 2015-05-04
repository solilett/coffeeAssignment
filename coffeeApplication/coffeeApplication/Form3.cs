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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            int productId = 0;
        }

        

        private void Form3_Load(object sender, EventArgs e)
        {
            string select = "SELECT tProductWeight.unitId AS 'ID', tProduct.name AS 'Name', tProductWeight.netWeight AS 'Weight (KG)', tProductWeight.grind AS 'Grind', tProductWeight.stock AS 'Current Stock' FROM tProduct, tProductWeight WHERE tProduct.id = tProductWeight.productId";

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
            globalVariables.productId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            this.Hide();
            var form4 = new Form4();
            form4.Closed += (s, args) => this.Close();
            form4.Show();
        }




    


        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            var form2 = new Form2();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }

    }
}
