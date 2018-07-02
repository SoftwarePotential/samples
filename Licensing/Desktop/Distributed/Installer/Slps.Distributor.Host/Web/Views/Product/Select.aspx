<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ProductSelectorModel>" %>
<%@ Import Namespace="Slps.Distributor.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Select</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%=Html.Encode(Model.PageCaption) %></h2>

	<div class="boxpanel">
		<div class="inner">
	<% if (!Model.Products.Any()) { %>
		<h4><%=Resource.Views.Product.For.CaptionMessageNoProductsAvailable %></h4>
		
	<% } else { %>
		<ul>
		<% foreach (ProductModel product in Model.Products) { %>
			<li><%= Html.DisplayFor( _ => product, "Product") %> <% if (!string.IsNullOrEmpty(Request.QueryString["redir"])) { %>[ <a href="..<%=Request.QueryString["redir"]%>/<%=product.Name %>/<%=product.Version %>"><%=Resource.Views.Product.For.ActionSelect %></a> ]<% } %></li>			
		<% } %>
		</ul>
	<% } %>
		</div>
	</div>	
</asp:Content>