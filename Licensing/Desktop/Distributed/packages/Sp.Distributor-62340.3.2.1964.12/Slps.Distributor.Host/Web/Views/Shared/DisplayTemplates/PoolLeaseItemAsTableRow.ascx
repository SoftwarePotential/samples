<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Slps.Distributor.Web.Models.PoolUsageModel.LeaseUsage.LeaseItem>" %>
<%@ Import Namespace="Slps.Distributor.Web.Models" %>
<tr>
    <% foreach ( string tagValue in Model.LeaseItemTagValues )
       { %>
    <td><%= tagValue %>
    </td>
    <% } %>
    <td><%= Html.DisplayFor( m => m.ItemTtl, "TimeSpanHMSOrDash" )%></td>
    <td><%= Html.DisplayFor( m=> m.Features) %>
    </td>
</tr>
