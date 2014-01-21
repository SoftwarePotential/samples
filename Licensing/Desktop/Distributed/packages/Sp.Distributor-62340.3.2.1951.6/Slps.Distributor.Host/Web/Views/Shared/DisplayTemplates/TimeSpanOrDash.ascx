<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TimeSpan?>" %>
<% if (Model == null) {%>-<% }else { %><%= Html.Encode(Model) %><% } %>