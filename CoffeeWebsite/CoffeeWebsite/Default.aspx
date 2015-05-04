<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CoffeeWebsite._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

  <div class="jumbotron">
        <h1 class="defaultBanner">Coffee WareHouse <img src="Content/Images/coffeeBean.png" width="60px" /></h1>
        <p class="lead" class="defaultBanner">Welcome to The Coffee WareHouse, where we take great pride in sourcing the best coffee beans, roasting them with care and skill then sending them out to you quickly so you can experience the superb aromas and distinctive flavours of our fresh coffee.</p>
      
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Espresso</h2>
                <img src="Content/Images/1.png"  width="100%"  height="250px;"/> 
        </div>
        <div class="col-md-4">
            <h2>Spiced Beans</h2>
             <img src="Content/Images/3.png"  width="100%"  height="250px;"/> 
        </div>
        <div class="col-md-4">
            <h2>Our Beans</h2>
             <img src="Content/Images/2.png"  width="100%" height="250px;"/> 
        </div>
    </div>

    <hr />

    <div id="displayProductsContainer" class="recentProducts" runat="server" >
    </div>
</asp:Content>
