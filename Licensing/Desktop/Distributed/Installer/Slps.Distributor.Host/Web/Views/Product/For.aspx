<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ProductLicenseModel>" %>
<%@ Import Namespace="Slps.Distributor.Web.Models"%>
<%@ Import Namespace="Resource.Views.Product" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"><%=For.PageTitle %></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<h2><%= For.CaptionDetailsFor%> <%= Html.DisplayFor(_ => Model.Product, "Product")%></h2>
	
	<% if (Model.Licenses.Any()) { %>
	<a class="expander" data-target="expander_licensediv" data-wordmore='<%=For.ActionShowLicenses %>' data-wordless='<%=For.ActionHideLicenses %>'><%=For.ActionShowLicenses %></a>	
	<div class="hidden" id="expander_licensediv" >
		<%= Html.DisplayFor( _ => Model.Licenses, "LDTable") %>
	</div>
	<%} %>
	
</asp:Content>
