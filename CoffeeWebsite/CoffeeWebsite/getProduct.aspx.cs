using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;

namespace CoffeeWebsite
{
    public partial class getProduct : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\aspnet-CoffeeWebsite-20150319013826.mdf;Initial Catalog=aspnet-CoffeeWebsite-20150319013826;Integrated Security=True");
        public int id; 
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Convert.ToInt32(Request.Params["ID"]);

            
            if (!IsPostBack)
            {
                fillDdWeight(id);
                ddGrind.Items.Add("Select");

                ddQuantity.Items.Add("1");
                ddQuantity.Items.Add("2");
                ddQuantity.Items.Add("3");
                ddQuantity.Items.Add("4");
                ddQuantity.Items.Add("5");
                ddQuantity.Items.Add("6");
                ddQuantity.Items.Add("7");

            }
            coffeeDatasource.SelectCommand = "SELECT * FROM tProduct WHERE tProduct.Id = " + id;
            repeaterProductInfo.DataSourceID = "coffeeDatasource";

            if (User.Identity.IsAuthenticated)
            {

                if (Request.Cookies[Global.user].Value != "default")
                {
                    Response.Cookies[Global.user].Value = Request.Cookies[Global.user].Value + "," + id.ToString();
                }

                else
                {
                    Response.Cookies[Global.user].Value = id.ToString();
                }
            }


                
        }



        public void fillDdWeight(int productId)
        {
            ddWeight.Items.Clear();
            var query = "SELECT DISTINCT tProductWeight.netWeight FROM tProductWeight WHERE productId = " + productId + " order by netWeight ASC";
            SqlCommand comm = new SqlCommand(query, conn);

            conn.Open();
            SqlDataReader reader = comm.ExecuteReader();

            List<ListItem> items = new List<ListItem>();


            ddWeight.Items.Add("Select");
            while (reader.Read())
            {
                items.Add(new ListItem(reader[0].ToString() + " KG "));
                //ddWeight.Items.Add(reader[0].ToString() + "KG (£" + reader[1].ToString() + ")");
            }
            reader.Close();
            conn.Close();
            ddWeight.Items.AddRange(items.ToArray());        
        }

        public void fillDdGrind(int productId, string weight, string criteria)
        {
            ddGrind.Items.Clear();
            var query2 = "SELECT tProductWeight.grind, tProductWeight.unitId FROM tProductWeight WHERE tProductWeight.productId = " + productId + criteria + "  order by tProductWeight.grind ASC";
            SqlCommand comm = new SqlCommand(query2, conn);

            conn.Open();
            SqlDataReader reader = comm.ExecuteReader();

            List<ListItem> items = new List<ListItem>();


            ddGrind.Items.Add("Select");
            while (reader.Read())
            {
                items.Add(new ListItem(reader[0].ToString(), reader[1].ToString()));
                //ddWeight.Items.Add(reader[0].ToString() + "KG (£" + reader[1].ToString() + ")");
            }
            reader.Close();
            conn.Close();
            ddGrind.Items.AddRange(items.ToArray());
        }


        protected void btnAddToBasket_Click(object sender, EventArgs e)
        {
            if (ddWeight.SelectedItem.Value == "Select")
            {
                lblResult.Text = "Please Select Weight";
            }
            else if (ddGrind.SelectedItem.Value == "Select")
            {
                lblResult.Text = "Please Select a grind";
            }
            else
            {
                string query = "SELECT stock FROM tProductWeight WHERE unitId = " + ddGrind.SelectedValue;
                SqlCommand comm = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if(Convert.ToInt32(reader[0]) < Convert.ToInt32(ddQuantity.Text))
                        {
                            lblResult.Text = "There is currently only " + reader[0].ToString() + " left in stock";
                        }
                        else
                        {
                            string tempProducts = "";
                            for (int i = 0; i < Convert.ToInt32(ddQuantity.Text); i++)
                            {
                                tempProducts += ddGrind.SelectedValue + ",";
                            }
                            Session["basket"] += tempProducts;
                            Response.Redirect(Request.RawUrl);
                        }
                    }
                }           
            }
        }

     

        public class basketItem
        {
            public string productId { get; set; }
            public string quantity { get; set; }
        }

        protected void ddWeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddWeight.SelectedValue != "Select")
            {
                string criteria = "";
                String getWeight = ddWeight.SelectedValue;
                getWeight = getWeight.Substring(0, getWeight.Length - 3);
                criteria = "AND tProductWeight.netWeight = " + getWeight;
                fillDdGrind(id, getWeight, criteria);
            }       
        }

        protected void btnEmail_Click(object sender, EventArgs e)
        {
            if ( !HttpContext.Current.User.Identity.IsAuthenticated)
            {
                lblResult.Text = "Please Login";
            }
            else
            {
                lblResult.Text = notifyUser();
            }
        }

        public string notifyUser()
        {
            var currentUserId = User.Identity.GetUserId();
            var productId = ddWeight.SelectedValue;

            conn.Open();
            SqlCommand comm = new SqlCommand("INSERT INTO tNotify VALUES ('" + currentUserId + "', '" + productId + "')", conn);
            comm.ExecuteNonQuery();
            conn.Close();

            return "You will be notified when product become available";
        }

        protected void ddGrind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (ddGrind.SelectedValue != "Select")
                {
                    var query = "SELECT stock, price FROM tProductWeight WHERE unitId = " + ddGrind.SelectedValue + ";";
                    SqlCommand comm = new SqlCommand(query, conn);

                    conn.Open();
                    SqlDataReader reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader[0].ToString() == "0")
                        {
                            btnAddToBasket.CssClass = "btn btn-default disabled";
                            lblResult.Text = "Product currently out of stock";
                            btnEmail.Visible = true;
                        }
                        else
                        {
                            btnAddToBasket.CssClass = "btn btn-success";
                            lblResult.Text = "£" + reader[1].ToString() + "<br/> Available";
                            btnEmail.Visible = false;
                        }
                    }
                    reader.Close();
                    conn.Close();
                }
                }
                else
                {
                    lblResult.Text = "Please login to purchase";
                }

            
        }
    }
}