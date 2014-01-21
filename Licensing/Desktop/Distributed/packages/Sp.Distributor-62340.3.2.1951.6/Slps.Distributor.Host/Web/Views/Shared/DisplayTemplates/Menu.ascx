<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Resource.Views.Shared"%>

<ul id="menu">              
	<% if ((string)ViewContext.RouteData.Values["controller"] != "Home") {%>	<li><%= Html.ActionLink(Navigation.Home, "Index", "Home")%></li><%} %>
	<li><%= Html.ActionLink(Navigation.Activation, "Add", "Activation")%></li>
	<li><%= Html.ActionLink(Navigation.Products, "For", "Product")%></li>
	<li><%= Html.ActionLink(Navigation.Usage, "For", "Usage")%></li>
</ul>		
