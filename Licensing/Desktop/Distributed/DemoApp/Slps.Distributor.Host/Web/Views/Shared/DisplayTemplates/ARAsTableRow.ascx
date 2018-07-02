<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ActivationRecordModel>" %>
<%@ Import Namespace="Slps.Distributor.Web.Models"%>
<% if (Model.ForDisplayAlternateRow){ %><tr class="alt"><% } else { %><tr><%} %>
		<td><%= Model.ActivationKey %></td>
		<td><%= Html.DisplayFor(model => model.ProductId, "Product") %></td>
</tr>
