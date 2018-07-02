<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AddActivationModel>" %>

<%@ Import Namespace="Resource.Views.Activation" %>
<%@ Import Namespace="Slps.Distributor.Web.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Add.PageTitle %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		<%= Html.Encode(Add.PageTitle) %></h2>
	<p>
		<%= Html.Encode(Add.Instruction) %></p>
	<% using ( Html.BeginForm() )
	{ %>
	<%=Html.HiddenFor(x => x.PostToken)%>
	<div>
		<div>
			<%=Html.Encode(Add.ActivationKeyLabel) %>
			<%=Html.TextBoxFor(t => t.ActivationKey, new { size = 35 })%>
			<input type="submit" value="<%=Add.Submit %>" id="activateLicense"/>
		</div>
	</div>
	<%= Html.ValidationMessage("ActivationKey")%>
	<% } %>
	<div style="margin-top: 10px";>
		<div style="float:left; margin-right: 10px">
			<%= Html.ActionLink(Add.History, "History", new { Back ="Add"}) %>
		</div>
		<div>
			<%= Html.ActionLink(Add.ManualActivation, "Manual", new { Back ="Add"}) %>
		</div>
	</div> 
</asp:Content>

