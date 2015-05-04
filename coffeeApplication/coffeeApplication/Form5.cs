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
    public partial class Form5 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\solilett\Desktop\University Work\Year 3\Semester 2\SDAF\Assignment\CoffeeWebsite\CoffeeWebsite\App_Data\aspnet-CoffeeWebsite-20150319013826.mdf';Integrated Security=True;Connect Timeout=30");

        public Form5()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                lblError.Text = "Please enter a coffee name";
            }
            else if (txtStrength.Text == "")
            {
                lblError.Text = "Please enter a coffee strength";
            }
            else if (txtOrigin.Text == "")
            {
                lblError.Text = "Please enter a coffee origin";
            }
            else if (txtDescription.Text == "")
            {
                lblError.Text = "Please enter a brief description";
            }
            else
            {
                SqlCommand comm = new SqlCommand("INSERT INTO tProduct (name, strength, description, origin) VALUES ('" + txtName.Text + "'," + txtStrength.Text + ", '" + txtDescription.Text + "', '" + txtOrigin.Text + "')", conn);
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();

                string query = "SELECT TOP 1 tProduct.id FROM tProduct ORDER BY tProduct.id DESC ";
                var productId = "";

                SqlCommand comm2 = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = comm2.ExecuteReader();

                while (reader.Read())
                {
                    productId = reader[0].ToString();
                    globalVariables.justAddProductId = Convert.ToInt32(productId);
                }
                reader.Close();
                conn.Close();


                string saveLocation = @"C:\Users\solilett\Desktop\University Work\Year 3\Semester 2\SDAF\Assignment\CoffeeWebsite\CoffeeWebsite\Content\Images\productImages\" + productId + ".png";
                System.IO.File.Copy(openFileDialog1.FileName, saveLocation);
                lblImageSelected.Text = "";
                DialogResult dialogResult = MessageBox.Show("Do you want to continue adding types of this product?", "Success", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    globalVariables.editProduct = false;
                    this.Hide();
                    Form6 form6 = new Form6();
                    form6.Show();
                }
                else if (dialogResult == DialogResult.No)
                {
                    this.Hide();
                    Form2 form2 = new Form2();
                    form2.Show();
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select Image File";
            openFileDialog1.Filter = "All Files (*.*)|*.*";
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string sFilePath;
            sFilePath = openFileDialog1.FileName;
            lblImageSelected.Text = openFileDialog1.SafeFileName;
            if (sFilePath == "")
                return;
        }

    }
}
