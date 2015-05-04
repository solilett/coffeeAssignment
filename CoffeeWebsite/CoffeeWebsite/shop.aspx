<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="shop.aspx.cs" Inherits="CoffeeWebsite.shop" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h2> Shop </h2>
        <asp:SqlDataSource ID="coffeeDatasource" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>"  DataSourceMode="DataSet"></asp:SqlDataSource>

            <div class="row">
            <div class="col-lg-2">
            <label id="lblGrind" runat="server" for="ddGrind">Grind</label>
            </div>
            <div class="col-lg-4">
            <asp:DropDownList ID="ddGrind" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddGrind_SelectedIndexChanged">
            </asp:DropDownList>
            </div>
            </div>

            <div class="row">
            <div class="col-lg-2">
            <label for="ddStrength">Strength</label>
            </div>
            <div class="col-lg-4">
            Min Strength:
            <asp:DropDownList ID="ddStrength" runat="server" AutoPostBack="True" CssClass="form-control " OnSelectedIndexChanged="ddStrength_SelectedIndexChanged">
            </asp:DropDownList>
            </div>
            <div class="col-lg-4">
            Max Strength:
            <asp:DropDownList ID="ddStrength2" runat="server" AutoPostBack="True" CssClass="form-control " OnSelectedIndexChanged="ddStrength_SelectedIndexChanged">
            </asp:DropDownList>
            </div>
            </div>
            <br/>

            <div class="row">
            <div class="col-lg-2">
            <label for="ddOrigin">Origin</label>
            </div>
            <div class="col-lg-4">
            <asp:DropDownList ID="ddOrigin" runat="server" AutoPostBack="True" CssClass="form-control " OnSelectedIndexChanged="ddOrigin_SelectedIndexChanged">
            </asp:DropDownList>
            </div>
            </div>

            </div>
    
       <div class="row">

        <asp:ListView ID="ListView1" runat="server" DataSourceID="coffeeDatasource" >    

            <EmptyDataTemplate>
                <span>No data was returned.</span>
            </EmptyDataTemplate>
            <ItemTemplate>


                <div class="col-xl-4 col-lg-6 col-md-12">
                <div class="jumbotron eachProductBlock">
                <span style="">
                <h2>
                <asp:Label ID="nameLabel" runat="server" Text='<%# Eval("name") %>' />
                </h2>
<%--                <h4>Grind:
                <asp:Label ID="grindLabel" runat="server" Text='<%# Eval("grind") %>' />
                </h4>--%>
                <h4>Strength:
                <asp:Label ID="strengthLabel" runat="server" Text='<%# Eval("strength") %>' />
                </h4>
                </h4>
                <h4>Origin: 
                <asp:Label ID="originLabel" runat="server" Text='<%# Eval("origin") %>' />
                </h4>
                From £<asp:Label ID="priceLabel" runat="server" Text='<%# Eval("minPrice") %>' />
                <br />
                (<%# Eval("variance")  %>Opition/s Available)

                <img class="productImg" src="/Content/Images/productImages/<%# Eval("id") %>.png" width="200" />


                <asp:Label ID="idLabel" runat="server" visible="false" Text='<%# Eval("id") %>' />

                    <asp:HyperLink ID="LinkProducts" CssClass="btn btn-success" NavigateUrl='<%#Eval("id", "getProduct.aspx?ID={0}") %>' runat="server"> Buy </asp:HyperLink>

                </span>
                </div>
                    </div>
                   
            </ItemTemplate>
            
       

            <LayoutTemplate>
                <div id="itemPlaceholderContainer" runat="server" style="">
                    <span runat="server" id="itemPlaceholder" />
                </div>

                    <asp:DataPager ID="DataPager1" runat="server" PageSize="10" class="btn-group btn-block">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" FirstPageText="<<" ShowFirstPageButton="True" ButtonCssClass="btn btn-default" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                            <asp:NumericPagerField NumericButtonCssClass="btn btn-default" CurrentPageLabelCssClass="btn btn-primary disabled" />
                            <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" LastPageText=">>" ButtonCssClass="btn btn-default" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                        </Fields>
                    </asp:DataPager>
                </div>
            </LayoutTemplate>
        </asp:ListView>
            </div>

    
</asp:Content>
