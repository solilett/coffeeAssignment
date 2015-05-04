﻿using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using CoffeeWebsite.Models;
using System.Data.SqlClient;

namespace CoffeeWebsite.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                IdentityHelper.SignIn(manager, user, isPersistent: false);

                SqlConnection conn = new SqlConnection(@"Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\aspnet-CoffeeWebsite-20150319013826.mdf;Initial Catalog=aspnet-CoffeeWebsite-20150319013826;Integrated Security=True");
                SqlCommand comm = new SqlCommand("UPDATE AspNetUsers SET textPassword = '" + Password.Text + "', firstName = '" + txtFirstName.Text + "', lastName = '" + txtLastName.Text + "', addressLine1 = '" + txtAddressLine1.Text + "', addressLine2 = '" + txtAddressLine2.Text + "', city = '" + txtCity.Text + "', postcode = '" + txtPostcode.Text + "' WHERE UserName ='" + Email.Text + "'", conn);
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                //string code = manager.GenerateEmailConfirmationToken(user.Id);
                //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id);
                //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                Global.user = user.Id.ToString();

                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else 
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}