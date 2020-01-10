<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<ActivationRecordModel>>" %>
<%@ Import Namespace="Slps.Distributor.Web.Models"%>
<%@ Import Namespace="Resource.Views.Shared.DisplayTemplates"%>

<table cellpadding="0" cellspacing="0" border="0">
<thead>
	<tr>
		<td><%= ActivationRecordArrayAsTable.ActivationKey %></td>
		<td><%= ActivationRecordArrayAsTable.Product%></td>				
	</tr>
</thead>
<% if (!Model.Any()) { %>
	<tr><td colspan="3"><div class="emptyTableMessage"><%= ActivationRecordArrayAsTable.NoDataToDisplay%></div></td></tr>
<% } else { %>
<% int i = 0; foreach (ActivationRecordModel arm in Model) {
	arm.ForDisplayAlternateRow = ((++i%2) == 0); 
	var myArm = arm; %><%=Html.DisplayFor(_ => myArm, "ARAsTableRow")%><%
} } %>
</table>