using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data.Sql;

namespace CoffeeWebsite
{
   
    public partial class shop : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\aspnet-CoffeeWebsite-20150319013826.mdf;Initial Catalog=aspnet-CoffeeWebsite-20150319013826;Integrated Security=True");
        protected void Page_Load(object sender, EventArgs e)
        {

            coffeeDatasource.SelectCommand = "SELECT tProduct.name, tProduct.strength, tProduct.id, tProduct.origin, MIN(tProductWeight.price) AS 'minPrice', COUNT(tProductWeight.productId) AS 'variance' FROM tProduct, tProductWeight WHERE tProduct.id = tProductWeight.productId GROUP BY  tProduct.name, tProduct.strength, tProduct.id, tProduct.origin ORDER BY strength; ";
            if (!IsPostBack)
            {
                fillStrengthDropDown("");
                fillStrength2DropDown("");
                fillGrindDropDown("");
                fillOriginDropDown("");


            }
        }

        public void getGrinds()
        {
            var query = "SELECT DISTINCT grind FROM tProductWeight WHERE tProductWeight.productId = 1;";
           
            foreach(ListViewItem item in ListView1.Items)
            {
                lblGrind.InnerText = item.ToString();
            }

        }

        public void fillStrengthDropDown(string constraint)
        {
            ddStrength.Items.Clear();
            var query = "SELECT DISTINCT strength FROM tProduct " + constraint;
            SqlCommand comm = new SqlCommand(query, conn);

            conn.Open();
            SqlDataReader reader = comm.ExecuteReader();

            ddStrength.Items.Add("Select");
            while (reader.Read())
            {
                ddStrength.Items.Add(reader[0].ToString());
            }
            reader.Close();
            conn.Close();
        }

        public void fillStrength2DropDown(string constraint)
        {
            ddStrength2.Items.Clear();
            var query = "SELECT DISTINCT strength FROM tProduct " + constraint;
            SqlCommand comm = new SqlCommand(query, conn);

            conn.Open();
            SqlDataReader reader = comm.ExecuteReader();

            ddStrength2.Items.Add("Select");
            while (reader.Read())
            {
                ddStrength2.Items.Add(reader[0].ToString());
            }
            reader.Close();
            conn.Close();
        }

        public void fillGrindDropDown(string constraint)
        {
            ddGrind.Items.Clear();
            var query = "SELECT DISTINCT grind FROM tProductWeight " + constraint;
            SqlCommand comm = new SqlCommand(query, conn);

            conn.Open();
            SqlDataReader reader = comm.ExecuteReader();

            ddGrind.Items.Add("Select");
            while (reader.Read())
            {
                ddGrind.Items.Add(reader[0].ToString());
            }

            reader.Close();
            conn.Close();

        }

        public void fillOriginDropDown(string constraint)
        {
            ddOrigin.Items.Clear();
            var query = "SELECT DISTINCT origin FROM tProduct " + constraint;
            SqlCommand comm = new SqlCommand(query, conn);

            conn.Open();
            SqlDataReader reader = comm.ExecuteReader();

            ddOrigin.Items.Add("Select");
            while (reader.Read())
            {
                ddOrigin.Items.Add(reader[0].ToString());
            }
            reader.Close();
            conn.Close();
        }

        string filterString;

        protected void ddGrind_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterDataSource();
        }

        protected void ddStrength_SelectedIndexChanged(object sender, EventArgs e)
        {

            filterByStrengths();
        }


        public void filterByStrengths()
        {
            if (ddStrength.SelectedValue == "Select" && ddStrength2.SelectedValue != "Select")
            {
                filterString = "strength <= '" + ddStrength2.SelectedValue + "'";
                fillStrengthDropDown("WHERE strength <= " + ddStrength2.SelectedValue );
                fillOriginDropDown("WHERE strength <= " + ddStrength2.SelectedValue);
            }
            if (ddStrength.SelectedValue != "Select" && ddStrength2.SelectedValue == "Select")
            {
                filterString = "strength >= '" + ddStrength.SelectedValue + "'";
                fillStrength2DropDown("WHERE strength >= " + ddStrength.SelectedValue);
                fillOriginDropDown("WHERE strength >= " + ddStrength.SelectedValue);
            }
            
            if (ddStrength.SelectedValue != "Select" && ddStrength2.SelectedValue != "Select")
            {
                filterString = "strength <= '" + ddStrength2.SelectedValue + "' AND strength >= '" + ddStrength.SelectedValue + "'";
                fillOriginDropDown("WHERE strength <= " + ddStrength2.SelectedValue + " AND strength >= " + ddStrength.SelectedValue);
            }

            if (ddStrength.SelectedValue == "Select" && ddStrength2.SelectedValue == "Select")
            {
                fillStrengthDropDown("");
                fillStrength2DropDown("");
                fillOriginDropDown("");
            }
            
        
            coffeeDatasource.FilterExpression = filterString;


        }

        public void filterDataSource()
        {
            if(ddGrind.SelectedValue != "Select")
            {
                filterString = "strength > " + ddStrength.SelectedValue;
                coffeeDatasource.FilterExpression = filterString;
            }

            //if (ddStrength.SelectedValue == "Select" && ddGrind.SelectedValue == "Select")
            //{
            //    filterString = null;
            //    fillGrindDropDown("");
            //    fillStrengthDropDown("");
            //}
            //else if (ddStrength.SelectedValue != "Select" && ddGrind.SelectedValue == "Select")
            //{
            //    fillGrindDropDown("WHERE strength = '" + ddStrength.SelectedValue + "'");
            //    filterString = "strength > '" + ddStrength.SelectedValue + "'";
            //}
            //else if (ddStrength.SelectedValue == "Select" && ddGrind.SelectedValue != "Select")
            //{
            //    fillStrengthDropDown("WHERE grind = '" + ddGrind.SelectedValue + "'");
            //    filterString = "grind LIKE '" + ddGrind.SelectedValue + "'";
            //}
            //else if (ddStrength.SelectedValue != "Select" && ddGrind.SelectedValue != "Select")
            //{
            //    filterString = "strength > '" + ddStrength.SelectedValue + "' AND grind LIKE '" + ddGrind.SelectedValue + "'";
            //}

            //coffeeDatasource.FilterExpression = filterString;
        }

        protected void ddOrigin_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterString = "origin LIKE '" + ddOrigin.SelectedValue + "'";
            coffeeDatasource.FilterExpression = filterString;
        }



    }
}