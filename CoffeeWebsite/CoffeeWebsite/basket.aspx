<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="basket.aspx.cs" Inherits="CoffeeWebsite.basket" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="jumbotron">
            <asp:Label ID="lblResult" runat="server" ></asp:Label>
            <div id="content" runat="server"></div>

            <br />
            <asp:Label ID="lblError" runat="server" ></asp:Label>
            <br />
            <asp:Button ID="btnOrder" runat="server" Text="Order" OnClick="btnOrder_Click" CssClass="btn btn-success" />
            <br />
            <asp:Button ID="btnEmpty" runat="server" Visible="false" Text="Empty Basket" CssClass="btn btn-danger" OnClick="btnEmpty_Click" />
        </div>
    </div>
</asp:Content>
