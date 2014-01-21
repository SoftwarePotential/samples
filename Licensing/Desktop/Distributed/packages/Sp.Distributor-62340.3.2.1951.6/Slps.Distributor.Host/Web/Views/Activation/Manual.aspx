<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ManualActivationModel>" %>

<%@ Import Namespace="Resource.Views.Activation" %>
<%@ Import Namespace="Slps.Distributor.Web.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Manual.PageTitle%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		<%= Html.Encode(Manual.GenerateSubheading) %></h2>
	<p>
		<%= Html.Encode( Manual.GenerateInstruction )%></p>
	<% using ( Html.BeginForm( "Manual", "Activation" ) )
	{ %>
	<div>
		<div class="inputContainer">
			<div class="label">
				<%=Html.Encode( Manual.ActivationKeyLabel )%>
			</div>
			<div style="float: none;">
				<%=Html.TextBoxFor(t => t.ActivationKey, new { size = 35 })%>
				<input type="submit" value="<%=Manual.Submit %>" />
			</div>
		</div>
		<div class="inputContainer">
			<div class="label">
				<%=Html.Encode( Manual.RequestData )%>
			</div>
			<div style="float: none;">
				<%= Html.TextAreaFor( model => model.ManualActivationRequest, new { @class = "textArea", disabled = true, @readonly = true } )%>
			</div>
		</div>
	</div>
	<% } %>
	<% Html.RenderPartial( "Upload" ); %>
</asp:Content>
