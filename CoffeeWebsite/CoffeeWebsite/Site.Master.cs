using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CoffeeWebsite
{

    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;


        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\aspnet-CoffeeWebsite-20150319013826.mdf;Initial Catalog=aspnet-CoffeeWebsite-20150319013826;Integrated Security=True");


        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }



        public void fillBasket()
        {
            string items2 = Session["basket"].ToString();
            items2.Substring(items2.Length - 1);
            string[] stringArray = items2.Split(',');

            List<string> basketItems = new List<string>();

            for (int i = 0; i < stringArray.Length; i++)
            {
                basketItems.Add(stringArray[i]);
            }


            decimal price = 0;

            for (int i = 0; i < basketItems.Count - 1; i++)
            {
                var query = "SELECT price FROM tProductWeight WHERE unitId = " + basketItems[i] + ";";
                SqlCommand comm = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    price = price + Convert.ToDecimal(reader[0]);
                }
                reader.Close();
                conn.Close();
            }





            basketLink.InnerHtml = "Basket - " + (basketItems.Count - 1).ToString() + " Item(s) - £" + price.ToString();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                Global.user = HttpContext.Current.User.Identity.GetUserId().ToString();
                if (HttpContext.Current.Request.Cookies[Global.user] == null)
                {
                    Response.Cookies[Global.user].Value = "default";
                }

            }

            

   
            if (Session["basket"] != null)
            {
                fillBasket();
            }

            



        }


        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }


    }

}