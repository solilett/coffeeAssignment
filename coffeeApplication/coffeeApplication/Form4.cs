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
using System.Net;
using System.Net.Mail;

namespace coffeeApplication
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\solilett\Desktop\University Work\Year 3\Semester 2\SDAF\Assignment\CoffeeWebsite\CoffeeWebsite\App_Data\aspnet-CoffeeWebsite-20150319013826.mdf';Integrated Security=True;Connect Timeout=30");


        private void Form4_Load(object sender, EventArgs e)
        {
           
            string query = "SELECT * FROM tProductWeight WHERE unitId = " + globalVariables.productId +";";
            
             SqlCommand comm = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        txtCurrent.Text = reader[3].ToString();
                    }
                }
                conn.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlCommand comm = new SqlCommand("UPDATE tProductWeight SET stock = " + txtNewStock.Text + " WHERE unitId = " + globalVariables.productId + ";", conn);
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();

             string query = "SELECT * FROM tProductWeight WHERE stock > 0;";
             conn.Open();
             SqlCommand comm2 = new SqlCommand(query, conn);


             List <string> productList = new List <string>();


             using (SqlDataReader reader = comm2.ExecuteReader())
             {
                 while (reader.Read())
                 {
                     productList.Add(reader[0].ToString());

                 }
                 reader.Close();
             }
             conn.Close();

             string notifyUserEmail;
             string productName = "";
             string productWeight = "";
            foreach (var id in productList)
            {
                string query2 = "SELECT AspNetUsers.Email, tNotify.productId, tNotify.id, tProductWeight.netWeight, tProduct.name FROM AspNetUsers, tNotify, tProductWeight, tProduct WHERE AspNetUsers.Id = tNotify.userId AND tNotify.productId = tProductWeight.unitId AND tProductWeight.productId = tProduct.id AND tNotify.productId = " + id + ";";
                conn.Open();
                SqlCommand comm3 = new SqlCommand(query2, conn);

                MailMessage mail = new MailMessage();
                NetworkCredential cred = new NetworkCredential("SDIAF1415@gmail.com", "Software1415");


                using (SqlDataReader reader2 = comm3.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        notifyUserEmail = reader2[0].ToString();
                        productWeight = reader2[3].ToString();
                        productName = reader2[4].ToString();
                        mail.To.Add(notifyUserEmail);      
                    }

                    if (mail.To.Count != 0)
                    {
                        mail.Subject = "Product in Stock";
                        mail.Body = "The coffee " + productName.ToString() + " (" + productWeight + "), is now available please visit www.theCoffeeWarehouse.com";
                        mail.From = new MailAddress("SDIAF1415@gmail.com");

                        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        smtp.Credentials = cred;

                        smtp.Send(mail);
                    }
                   
                }

                conn.Close();

                SqlCommand commDelete = new SqlCommand("DELETE FROM tNotify WHERE id = " + id + ";", conn);
                conn.Open();
                commDelete.ExecuteNonQuery();
                conn.Close();
            }

            productList.Clear();
            MessageBox.Show("Stock Updated");
            this.Hide();
            Form3 form3 = new Form3();
            form3.Show();
        }
    }
}
