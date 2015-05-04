<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="getProduct.aspx.cs" Inherits="CoffeeWebsite.getProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:SqlDataSource ID="coffeeDatasource" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>"  DataSourceMode="DataSet"></asp:SqlDataSource>
    <div class="container">
    <div class="jumbotron">

        <asp:Repeater ID="repeaterProductInfo" runat="server">
            <ItemTemplate>
                            <%# Eval("name") %>
                <%# Eval("description") %>

            </ItemTemplate>
        </asp:Repeater>

        

    <asp:DropDownList ID="ddWeight" runat="server" AutoPostBack="True" CssClass="form-control" width="180px" OnSelectedIndexChanged="ddWeight_SelectedIndexChanged" ></asp:DropDownList>
    <br />
    <asp:DropDownList ID="ddGrind" runat="server" AutoPostBack="True" CssClass="form-control" width="180px" OnSelectedIndexChanged="ddGrind_SelectedIndexChanged"  ></asp:DropDownList>
    <br />
    <asp:DropDownList ID="ddQuantity" runat="server" CssClass="form-control" width="180px" ></asp:DropDownList>

        <asp:Label ID="lblResult" runat="server" ></asp:Label>
        <br />
        <asp:Button ID="btnAddToBasket" runat="server" Text="Add To Basket"  CssClass="btn btn-success" OnClick="btnAddToBasket_Click" />
        <asp:Button ID="btnEmail" runat="server" Text="Notify When Available" Visible="false" OnClick="btnEmail_Click" CssClass="btn btn-success" />

    </div>
    </div>
</asp:Content>
