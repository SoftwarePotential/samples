<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<LicenseDetailModel[]>" %>
<%@ Import Namespace="Slps.Distributor.Web.Models" %>
<%@ Import Namespace="Resource.Views.Shared.DisplayTemplates" %>
<h4>
    <%= LicenseDetailsTable.CaptionLicenses %></h4>
<table cellpadding="0" cellspacing="0" border="0">
    <thead>
        <tr>
            <td>
                <%= LicenseDetailsTable.LicenseEvaluation %>
            </td>
            <td>
                <%= LicenseDetailsTable.LicenseRenewable %>
            </td>
            <td>
                <%= LicenseDetailsTable.LicenseExpires %>
            </td>
            <td>
                <%= LicenseDetailsTable.LicenseState %>
            </td>
            <td>
                <%= LicenseDetailsTable.LicenseActivationKey %>
            </td>
            <td>
                <%= LicenseDetailsTable.LicenseRevision%>
            </td>
            <td>
                <%= LicenseDetailsTable.LicenseDistributor%>
            </td>
            <td>
                <%= LicenseDetailsTable.LicenseSeatBased%>
            </td>
            <td>
                <%= LicenseDetailsTable.LicenseCheckoutAllowed%>
            </td>
             <td align="center">
                <%= LicenseDetailsTable.LicenseRequiresNamedUsers %>
            </td>
            <td align="center">
                <%= LicenseDetailsTable.LicenseDetail %>
            </td>
        </tr>
    </thead>
    <% int index = 0; foreach ( LicenseDetailModel detail in Model )
       { %>
    <% if ( ++index % 2 == 0 )
       { %><tr class="alt">
        <% }
       else
       { %><tr>
            <%} %>
            <td>
                <%=Html.DisplayFor( _ => detail.IsEvaluation, "BoolYesNo")%>
            </td>
            <td>
                <%=Html.DisplayFor(_ => detail.IsRenewable, "BoolYesNo")%>
            </td>
            <td>
                <%=Html.DisplayFor(_=> detail.ActualExpirationDate, "DateTimeOrDash")%>
            </td>
            <td>
                <%=Html.Encode(detail.InternalState)%>
            </td>
            <td>
                <%=Html.Encode(detail.ActivationKey)%>
            </td>
            <td>
                <%=Html.Encode(detail.Revision)%>
            </td>
            <td>
                <%=Html.DisplayFor(_ => detail.IsDistributor, "BoolYesNo")%>
            </td>
            <td>
                <%=Html.DisplayFor(_ => detail.IsSeatBased, "BoolYesNo")%>
            </td>
            <td>
                <%=Html.DisplayFor(_ => detail.CanBeCheckedOut, "BoolYesNo")%>
            </td>
            <td>
                <%=Html.DisplayFor(_ => detail.RequiresNamedUsers, "BoolYesNo")%>
            </td>
            <td align="center">
                <% if ( !detail.Features.Any() )
                   { %><%=LicenseDetailsTable.MessageNoFeatures %><%}
                   else
                   {%><a
                    class="expander" data-target="expander_<%=index%>" data-wordmore="<%=Resource.Views.Common.CommonText.More %>"
                    data-wordless="<%=Resource.Views.Common.CommonText.Less %>"><%=Resource.Views.Common.CommonText.More %></a><%} %>
            </td>
        </tr>
        <% if ( detail.Features.Any() )
           { %>
        <tr class="hidden" id="expander_<%=index %>">
            <td colspan="8">
                <h5>
                    <%= LicenseDetailsTable.CaptionFeatures %></h5>
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <thead>
                        <tr>
                            <td>
                                <%= LicenseDetailsTable.LicenseFeatureName %>
                            </td>
                            <td>
                                <%= LicenseDetailsTable.LicenseFeatureConcurrentUsage %>
                            </td>
                            <td>
                                <%= LicenseDetailsTable.LicenseFeatureTotalUsageLimit %>
                            </td>
                            <td>
                                <%= LicenseDetailsTable.LicenseFeatureWorkingTimeLimit %>
                            </td>
                            <td>
                                <%= LicenseDetailsTable.LicenseFeatureExpireDate %>
                            </td>
                        </tr>
                    </thead>
                    <% int innerIndex = 0; foreach ( LicenseFeatureModel feature in detail.Features )
                       { %>
                    <% if ( ++innerIndex % 2 == 0 )
                       { %><tr class="alt">
                        <% }
                       else
                       { %><tr>
                            <%} %>
                            <td>
                                <%= Html.DisplayFor(_ => feature.Name, "ToStringOrNA") %>
                            </td>
                            <td>
                                <%= Html.DisplayFor(_ => feature.ConcurrentUsageLimit, "ToStringOrNA")%>
                            </td>
                            <td>
                                <%= Html.DisplayFor(_ => feature.TotalUsageLimit, "ToStringOrNA")%>
                            </td>
                            <td>
                                <%= Html.DisplayFor(_ => feature.WorkingTimeLimit, "TimeSpanOrDash")%>
                            </td>
                            <td>
                                <%= Html.DisplayFor(_ => feature.ExpireDate, "ToStringOrNA")%>
                            </td>
                        </tr>
                        <%} %>
                </table>
                <br />
            </td>
        </tr>
        <% } %>
        <% } %>
</table>
