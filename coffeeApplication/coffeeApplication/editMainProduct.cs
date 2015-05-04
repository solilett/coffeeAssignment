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
    public partial class editMainProduct : Form
    {
        public editMainProduct()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\solilett\Desktop\University Work\Year 3\Semester 2\SDAF\Assignment\CoffeeWebsite\CoffeeWebsite\App_Data\aspnet-CoffeeWebsite-20150319013826.mdf';Integrated Security=True;Connect Timeout=30");


        private void editMainProduct_Load(object sender, EventArgs e)
        {

             string query = "SELECT * FROM tProduct WHERE id = " + globalVariables.justAddProductId;
            SqlCommand comm = new SqlCommand(query, conn);
            conn.Open();

            using (SqlDataReader reader = comm.ExecuteReader())
            {
                while(reader.Read())
                {
                    txtName.Text = reader[0].ToString();
                    cboStrength.Text = reader[2].ToString();
                    txtOrigin.Text = reader[6].ToString();
                    txtDescription.Text = reader[5].ToString();

                }

            }
            conn.Close();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string query = "UPDATE tProduct SET name = '" + txtName.Text + "', strength = " + cboStrength.Text + ", origin = '" + txtOrigin.Text + "', description = '" + txtDescription.Text + "' WHERE id = " + globalVariables.justAddProductId + "";
            SqlCommand comm = new SqlCommand(query, conn);
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Product updated");
            this.Hide();
            var form2 = new Form2();
            form2.Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "BEGIN TRANSACTION [Delete] DELETE FROM tProduct WHERE id = " + globalVariables.justAddProductId + " DELETE FROM tProductWeight WHERE productId = " + globalVariables.justAddProductId + " COMMIT TRANSACTION[Delete] ";
            SqlCommand comm = new SqlCommand(query, conn);
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Products Deleted");
            this.Hide();
            var form2 = new Form2();
            form2.Show();   
        }
    }
}
