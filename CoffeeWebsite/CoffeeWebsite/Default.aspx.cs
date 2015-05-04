using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.SqlClient;

namespace CoffeeWebsite
{
    public partial class _Default : Page
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\solilett\Desktop\University Work\Year 3\Semester 2\SDAF\Assignment\CoffeeWebsite\CoffeeWebsite\App_Data\aspnet-CoffeeWebsite-20150319013826.mdf';Integrated Security=True;Connect Timeout=30");
        List<string> allItems = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                    fillList();
                    displayProductsContainer.InnerHtml += "<h3> Recently Viewed <h3>";
                    int count = 0;
                    displayProductsContainer.InnerHtml += "<div class='row'>";
                            foreach (var displayItem in allItems)
                            {
                                if (displayItem != "default")
                                {
                                    string query = "SELECT  MIN(tProductWeight.price), tProduct.name, tProduct.id FROM tProduct INNER JOIN tProductWeight ON tProduct.id = tProductWeight.productId WHERE tProduct.id = " + displayItem + " GROUP BY tProduct.Id, tProduct.name";
                                    SqlCommand comm = new SqlCommand(query, conn);
                                    conn.Open();

                                    using (SqlDataReader reader = comm.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            displayProductsContainer.InnerHtml += "<a href='getproduct.aspx?ID=" + reader[2] + "'><div class='col-md-3 items'><h3>" + reader[1].ToString() + "</h3>";
                                            displayProductsContainer.InnerHtml += "<img src='Content/Images/productImages/" + reader[2].ToString() + ".png' width='70%' height='160px' />";
                                            displayProductsContainer.InnerHtml += "<p> Prices from £" + reader[0].ToString() + "</p>";
                                            displayProductsContainer.InnerHtml += "</div></a>";
                                        }


                                    }
                                    conn.Close();

                                    count++;
                                    if (count == 4)
                                    {
                                        break;
                                    }
                                }
                            }
                     displayProductsContainer.InnerHtml += "</div>"; 

                     } 
                }




        public void fillList()
        {
            if (Global.user != null)
            {    
                    if (Request.Cookies[Global.user].Value.Contains(','))
                    {
                        string[] splitCookie = Request.Cookies[Global.user].Value.Split(',');

                        foreach (var item in splitCookie.Reverse())
                        {
                            if (item != "")
                            {
                                if (!allItems.Contains(item))
                                {
                                    allItems.Add(item);
                                }
                            }


                        }
                    }
                }

            }

           
        }
        }
    
