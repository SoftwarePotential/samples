<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Slps.Distributor.Web.Models.ProductModel>" %>
<%=Html.Encode(Model.Name) %>, <%=Html.Encode(Model.Version) %>