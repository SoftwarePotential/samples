<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Slps.Distributor.Web.Models.PoolUsageModel>" %>
<%@ Import Namespace="Slps.Distributor.Web.App_LocalResources.Views.Usage" %>
<div style="width:100%">
<%if (Model != null && Model.Leases.Any()){ %>
	<div class="flush-right"><h4><%= PoolUsageTable.TotalResources %>: <%= Model.Summary.TotalLimitedResourcesDisplayString %></h4></div>
	<table cellpadding="0" cellspacing="0" border="0" style="width:100%">
	<thead>
		<tr>
			<td><%= PoolLeaseUsageHeaders.AllocatedTo%></td>
			<%
			foreach (string tagHeader in Model.LeaseTagHeaders)
			{%>
					<td><%= PoolLeaseUsageHeaders.ResourceManager.GetString(tagHeader) ?? tagHeader %></td>	
					<%
			}%>
			<td><%= PoolLeaseUsageHeaders.Remaining%></td>
			<td><%= PoolLeaseUsageHeaders.Details%></td>
		</tr>
	</thead>
		<% for ( int index = 0; index < Model.Leases.Length; ++index ) {%>
			<% =Html.DisplayFor( _ => Model.Leases[ index ], "PoolLeaseUsageAsTableRow", new { LeaseUsageIndex = index } )%>
		<% } %>
	</table>
	<% if ( Model.Summary.HasLimitedResources )
	{%>
		<div class="flush-right"><h4><%= PoolUsageTable.ResourcesAvailable %>: <%= Model.Summary.Available.ToString() %></h4></div>
		<%
	} %>
<% } %>
<% else { %>
		<div class="emptyTableMessage"><%= PoolUsageTable.NoDataToDisplay %></div>
	<% } %>
</div>
