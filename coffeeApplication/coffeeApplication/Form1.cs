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
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\solilett\Desktop\University Work\Year 3\Semester 2\SDAF\Assignment\CoffeeWebsite\CoffeeWebsite\App_Data\aspnet-CoffeeWebsite-20150319013826.mdf';Integrated Security=True;Connect Timeout=30");

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM AspNetUsers, AspNetRoles, AspNetUserRoles WHERE AspNetUsers.Id = AspNetUserRoles.UserId AND AspNetRoles.Id = AspNetUserRoles.RoleId AND AspNetRoles.Name = 'admin' AND Email = '" + txtEmail.Text + "' AND textPassword = '" + txtPassword.Text + "'";
            SqlCommand comm = new SqlCommand(query, conn);
            conn.Open();

            using (SqlDataReader reader = comm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    this.Hide();
                    var timerForm = new timerForm();
                    timerForm.Show();
                    timerForm.Hide();
                }
                else
                {
                    lblError.Text = "Invalid credentials";
                }

            }
            conn.Close();

            
        }




    }
}
