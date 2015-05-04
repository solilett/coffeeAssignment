using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace CoffeeWebsite.Account
{
    public partial class orders : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\solilett\Desktop\University Work\Year 3\Semester 2\SDAF\Assignment\CoffeeWebsite\CoffeeWebsite\App_Data\aspnet-CoffeeWebsite-20150319013826.mdf';Integrated Security=True;Connect Timeout=30");

        protected void Page_Load(object sender, EventArgs e)
        {


            string query = "SELECT tOrders.orderId, SUM(tOrderItem.quantity) AS 'Quantity of Items',  SUM(tProductWeight.price * tOrderItem.quantity) AS 'Total', tOrders.orderDate FROM tOrderItem, tOrders, tProductWeight WHERE tOrders.orderId = tOrderItem.orderId AND tProductWeight.unitId = tOrderItem.productId AND tOrders.userId = '" + Global.user.ToString() + "' GROUP BY tOrders.orderId, tOrders.orderDate  ORDER BY tOrders.orderDate DESC;";
            SqlCommand comm = new SqlCommand(query, conn);
            conn.Open();


            using (SqlDataReader reader = comm.ExecuteReader())
            {
                while (reader.Read())
                {
                    DateTime orderDate = Convert.ToDateTime(reader[3]);
                    orderContent.InnerHtml += "<div class='panel panel-default'><div class='panel-heading'>Order " +  orderDate.ToShortDateString() + "</div>";
                    orderContent.InnerHtml += "<div class='table-responsive'>";
                    orderContent.InnerHtml += "<table class='table table-bordered'>";
                    orderContent.InnerHtml += "<tr><td> Order ID </td><td> Number of Products </td><td> Total Price </td><td> Order Date </td></tr>";
                    orderContent.InnerHtml += "<tr><td>" + reader[0] + "</td><td>" + reader[1] + "</td><td>" + reader[2] + "</td><td>" + reader[3] + "</td></tr>";
                    orderContent.InnerHtml += "</table>";
                    orderContent.InnerHtml += "</div>";
                    orderContent.InnerHtml += "</div>";
                }
                conn.Close();


              

            }
        }



    }
}