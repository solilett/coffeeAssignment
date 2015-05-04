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
    public partial class basket : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\aspnet-CoffeeWebsite-20150319013826.mdf;Initial Catalog=aspnet-CoffeeWebsite-20150319013826;Integrated Security=True");

        public class basketItem
        {
            public string id { get; set; }
            public int quantity { get; set; }
        }
        List<basketItem> basketItems = new List<basketItem>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["basket"] != null)
            {
                btnEmpty.Visible = true;
                fillBasket();
                content.InnerHtml = "<table class='table'>";
                    content.InnerHtml += "<thead><tr><th>Name</th><th>Weight</th><th>Quantity</th><th>Price</th></tr></thead><tbody>";
                    decimal total = 0;

                for (int i = 0; i < basketItems.Count -1; i++)
                {
                    var name = "";
                    var weight = "";
                    var price = "";
                    var grind = "";
                    var quantity = basketItems[i].quantity;
                  

                    var query = "SELECT tProduct.name, tProductWeight.netWeight, tProductWeight.price, tProductWeight.grind FROM tProduct,tProductWeight WHERE tProduct.id = tProductWeight.productId AND unitId = " + basketItems[i].id +";";
                    SqlCommand comm = new SqlCommand(query, conn);

                    conn.Open();
                    SqlDataReader reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        name = reader[0].ToString();
                        weight = reader[1].ToString();
                        price = reader[2].ToString();
                        grind = reader[3].ToString();
                        total = total + (Convert.ToDecimal(price) * quantity); 
      
                    }
                    reader.Close();
                    conn.Close();
                    content.InnerHtml += "<tr><td>";
                    content.InnerHtml += name + "</td><td>";
                    content.InnerHtml += weight + "KG </td><td>";
                    content.InnerHtml += grind + "</td><td>";
                    content.InnerHtml += quantity + "</td><td>";
                    content.InnerHtml += "£" + Convert.ToDecimal(price).ToString() + "</td></tr>";
                }
                content.InnerHtml += "<tr><td>£" + total.ToString() + "</td><td>";
                content.InnerHtml += "</tbody></table>";

            }
            else
            {
                lblResult.Text = "Your basket is empty";
                btnOrder.CssClass = "btn btn-default disabled";
            }
        }


        

        public void fillBasket()
        {
            string items2 = Session["basket"].ToString();
            items2.Substring(items2.Length - 1);
            string[] stringArray = items2.Split(',');



            for (int i = 0; i < stringArray.Length; i++)
            {
                foreach (var item in basketItems)
                {
                    if (item.id == stringArray[i])
                    {
                        item.quantity = item.quantity + 1;
                        i++;
                    }


                }
                basketItems.Add(new basketItem
                {
                    id = stringArray[i],
                    quantity = 1,
                });
            }
        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                lblResult.Text = orderProcess();
            }
            else
            {
                lblError.Text = "Please login to order items";
            }
        }


        public string orderProcess()
        {
   
            var currentUserId = User.Identity.GetUserId();
            string orderId = "";

            conn.Open();
            SqlCommand comm = new SqlCommand("INSERT INTO tOrders (userid) VALUES ('" + currentUserId + "')", conn);
            comm.ExecuteNonQuery();
            conn.Close();

            var query = "SELECT orderId FROM tOrders ORDER BY orderId ASC";
            SqlCommand comm2 = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = comm2.ExecuteReader();

            while (reader.Read())
            {
               orderId = reader[0].ToString();
            }
            reader.Close();
            conn.Close();

            for (var i = 0; i < basketItems.Count -1; i++ )
            {
                conn.Open();
                var productId = basketItems[i].id;
                var quantity = basketItems[i].quantity;
                SqlCommand comm3 = new SqlCommand("INSERT INTO tOrderItem (orderId, productId, quantity) VALUES (" + orderId + ", " + productId + ", " + quantity + ")", conn);
                comm3.ExecuteNonQuery();
                conn.Close();

                int currentStock = 0;
                var query2 = "SELECT stock FROM tProductWeight WHERE unitId = " + productId;
                SqlCommand getStockcomm = new SqlCommand(query2, conn);
                conn.Open();
                SqlDataReader readerGetStock = getStockcomm.ExecuteReader();

                while (readerGetStock.Read())
                {
                    currentStock = Convert.ToInt32(readerGetStock[0]);
                }
                reader.Close();
                conn.Close();

                int newStockValue = currentStock - quantity;
                
                //if(newStockValue > 0)
                //{
                    conn.Open();
                    SqlCommand updateComm = new SqlCommand("UPDATE tProductWeight SET stock = " + newStockValue + " WHERE unitId = " + productId + ";", conn);
                    updateComm.ExecuteNonQuery();
                    conn.Close();
                   
                //}
                //else
                //{
                //    errorWithStock = true;
                //}
                
            }


            //if (errorWithStock == true)
            //{
            //    Session["basket"] = null;
            //    return "One of the products you are attempting to purchase is out of stock";
                
            //}
            //else
            //{

                Session["basket"] = null;
                return "Your order has been process <br/> Order Number: " + orderId;
            //}
        }

        protected void btnEmpty_Click(object sender, EventArgs e)
        {
            Session["basket"] = null;
            Response.Redirect(Request.RawUrl);
        }
    }
}