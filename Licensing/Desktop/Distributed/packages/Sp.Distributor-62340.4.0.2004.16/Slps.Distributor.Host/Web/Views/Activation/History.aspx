<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<ActivationRecordModel>>" %>
<%@ Import Namespace="Resource.Views.Activation "%>
<%@ Import Namespace="Slps.Distributor.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"><%=History.PageTitle  %></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3><%=History.ContentTitle %></h3>

	<%=Html.DisplayFor( _ => Model, "ARArrayAsTable")%>

</asp:Content>
