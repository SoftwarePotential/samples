<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<ProductModel>>" %>
<%@ Import Namespace="Slps.Distributor.Web.Models"%>
<% if (!Model.Any()) { %>
<ul><li>You do not have any products installed.</li></ul><%} else { %>
<ul><% foreach (ProductModel product in Model) { %>
	<li><%= Html.DisplayFor( _ => product, "Product") %>. [ <a href="product/for/<%=product.Name %>/<%=product.Version %>?back=../../..">Details</a>  | <a href="usage/for/<%=product.Name %>/<%=product.Version %>?back=../../..">Usage</a> ]</li>
<% } %></ul><% } %>