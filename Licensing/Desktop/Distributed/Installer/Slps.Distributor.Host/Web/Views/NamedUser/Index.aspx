<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" %>

<%@ Import Namespace="Slps.Distributor.Web.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Select</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="<%=Url.Content("~/scripts/mustache.min.js")%>"> </script>
    <script type="text/javascript" src="<%=Url.Content("~/scripts/nameduserproducts.js")%>"> </script>
    <h2>Select a product to manage named users</h2>

    <div class="boxpanel">
        <div class="inner">

            <h4 id="noProducts" style="display:none">There are no products that require named users installed for this service.</h4>

            <ul id="productList">
            </ul>
        </div>
    </div>
    <script id="product-template" type="text/template">
        {{#products}}
            <li>{{Name}}, {{Version}} [ <a href='./NamedUser/For/{{Name}}/{{Version}}' >Select ]</a></li>
        {{/products}}
    </script>
</asp:Content>
