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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\solilett\Desktop\University Work\Year 3\Semester 2\SDAF\Assignment\CoffeeWebsite\CoffeeWebsite\App_Data\aspnet-CoffeeWebsite-20150319013826.mdf';Integrated Security=True;Connect Timeout=30");


        private void Form6_Load(object sender, EventArgs e)
        {
            fillDatagrid();
            btnDelete.Visible = false;
        }



        public void fillDatagrid() {
            string select = "SELECT tProductWeight.unitId, tProduct.name, tProductWeight.Stock, tProductWeight.price, tProductWeight.grind, tProductWeight.netWeight FROM tProductWeight, tProduct WHERE tProduct.id = tProductWeight.productId AND tProductWeight.productId = " + globalVariables.justAddProductId;
            SqlDataAdapter dataadapter = new SqlDataAdapter(select, conn);
            DataSet ds = new DataSet();
            conn.Open();
            dataadapter.Fill(ds, "tProductWeight");
            conn.Close();

            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "tProductWeight";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(txtWeight.Text == "")
            {
                lblError.Text = "Please enter a product weight";
            }
            else if (txtStock.Text == "")
            {
                lblError.Text = "Please enter a stock level";
            }
            else if (txtPrice.Text == "")
            {
                lblError.Text = "Please enter a price";
            }
            else if(cbGrind.Text == "Select")
            {
                lblError.Text = "Plase select a grind type";
            }
            else
            {
                if (btnAdd.Text == "Add")
                {
                        SqlCommand comm = new SqlCommand("INSERT INTO tProductWeight (productId, netWeight, stock, price, grind) VALUES (" + globalVariables.justAddProductId + ", " + txtWeight.Text + "," + txtStock.Text + ", " + txtPrice.Text + ", '" + cbGrind.Text + "')", conn);
                        conn.Open();
                        comm.ExecuteNonQuery();
                        conn.Close();
                }
                else
                {
                        string query = "UPDATE tProductWeight SET  netWeight = " + txtWeight.Text + ", stock = " + txtStock.Text + ", price = " + txtPrice.Text + ", grind = '" + cbGrind.Text + "'   WHERE unitId =  " + dataGridView1.CurrentRow.Cells[0].Value.ToString() +";";
                        SqlCommand comm = new SqlCommand(query, conn);
                        conn.Open();
                        comm.ExecuteNonQuery();
                        conn.Close();

                        btnDelete.Visible = false;
                        this.Text = "Add Product";
                        btnAdd.Text = "Add";
                }
                fillDatagrid();
                clearControls();

            }
 
        }


        public void clearControls()
        {
            txtPrice.Text = "";
            txtStock.Text = "";
            txtWeight.Text = "";
            cbGrind.Text = "";
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnDelete.Visible = true;
            this.Text = "Edit Product";
            btnAdd.Text = "Update";

            txtPrice.Text =   dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtStock.Text =  dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtWeight.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            cbGrind.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
                SqlCommand comm = new SqlCommand("DELETE FROM tProductWeight WHERE unitId =  " + dataGridView1.CurrentRow.Cells[0].Value.ToString() + ";", conn);
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();

                fillDatagrid();
                clearControls();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            btnDelete.Visible = false;
            this.Text = "Add Product Types";
            btnAdd.Text = "Add";
        }

        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            var form2 = new Form2();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }


    }
}
