<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<PoolUsageModel.LeaseUsage>" %>
<%@ Import Namespace="Slps.Distributor.Web.Models"%>
<%@ Import Namespace="Slps.Distributor.Web.App_LocalResources.Views.Usage" %>

<% int leasedUsageIndex = Convert.ToInt32( ViewData[ "LeaseUsageIndex" ] ); %>

<tr>
	<td><%= Html.Encode(Model.Identity) %></td>
	<% foreach (object leaseTagValue in Model.LeaseTagValues) { %>
		<td><% =leaseTagValue.ToString() %></td>
	<% } %>
	<td><%= Html.DisplayFor( m => m.SummaryTtl, "TimeSpanHMSOrDash" ) %></td>
	<td align="center"><a class="expander" data-target="expander-<%=leasedUsageIndex%>" data-wordmore="<%=Resource.Views.Common.CommonText.More %>"
                    data-wordless="<%=Resource.Views.Common.CommonText.Less %>"><%=Resource.Views.Common.CommonText.More %></a>
	</td>
</tr>
 <tr class="hidden" id="expander-<%=leasedUsageIndex %>">
	<td colspan="<%= Model.LeaseItemTagHeaders.Length + 1 %>" style="padding-left: 15px">
		<h5><%= PoolUsageTable.LeasedFeatures %></h5>
		<table width="100%" cellspacing="0" cellpadding="0" border="0">
		<thead>
			<tr>
				<% foreach (string tagHeader in Model.LeaseItemTagHeaders)
				{%>
						<td><%= PoolLeaseItemHeaders.ResourceManager.GetString(tagHeader) ?? tagHeader %></td>	
						<%
				}%>
				<td><%= PoolLeaseItemHeaders.Remaining%></td>
			</tr>
		</thead>
		<% foreach (var leaseItem in Model.Items){%>
			<%= Html.DisplayFor( _ => leaseItem, "PoolLeaseItemAsTableRow")  %>
		<%} %>
		</table>
	</td>
</tr>