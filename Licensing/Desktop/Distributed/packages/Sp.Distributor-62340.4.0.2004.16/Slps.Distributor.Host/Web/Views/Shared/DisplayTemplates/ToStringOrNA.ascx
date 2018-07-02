<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<% if (Model == null) { %>-<% } else { %> <%=Html.Encode(Model.ToString()) %><%} %>