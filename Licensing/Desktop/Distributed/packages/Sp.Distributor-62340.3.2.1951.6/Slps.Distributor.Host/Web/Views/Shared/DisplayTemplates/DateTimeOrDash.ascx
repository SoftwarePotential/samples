<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DateTime?>" %>
<% if (Model == null) {%>-<% }else { %><%= Html.Encode(Model) %><% } %>