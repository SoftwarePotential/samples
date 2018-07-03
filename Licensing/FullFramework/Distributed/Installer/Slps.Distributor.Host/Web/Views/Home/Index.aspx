<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="Slps.Distributor.Web" %>
<%@ Import Namespace="Resource.Views.Shared" %>
<%@ Import Namespace="Ninject" %>
<%@ Import Namespace="Resource.Views.Home" %>
<%@ Import Namespace="Slps.Distributor.Web.Domain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Index.PageTitle%></asp:Content>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
    <div id="homepage">
        <div id="header">
            <asp:Image ImageUrl="~/Content/logo_company.jpg" CssClass="companylogo" runat="server" />
            <h1>
                <%=Html.Encode(MvcApplication.Instance.VendorServices.VendorName) %></h1>
        </div>
        <%=Html.DisplayFor(_ => _, "Menu") %>
    </div>
    <div class="clear borderline">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%=Index.Dashboard%></h2>
    <div class="front-page">
        <div class="boxpanel">
            <div class="inner">
                <h3>
                    Products</h3>
                <%= Html.Action ("ListWithNavigation", "Product") %>
            </div>
        </div>
      
        <div style="clear: both;">
            <span style="float: right"><%=Index.CaptionVersion %>: <%= Html.Encode( FileVersionInfo.GetVersionInfo( typeof( MvcApplication ).Assembly.Location ).TriPartiteVersionNumber()) %></span>
            &nbsp;
        </div>
    </div>
</asp:Content>
