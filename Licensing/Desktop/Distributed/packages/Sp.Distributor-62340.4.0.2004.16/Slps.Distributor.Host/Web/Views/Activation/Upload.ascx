<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ManualActivationModel>" %>
<%@ Import Namespace="Resource.Views.Activation" %>
<%@ Import Namespace="Slps.Distributor.Web.Models" %>
<div style="padding-top: 20px">
	<% using ( Html.BeginForm( "Upload", "Activation", FormMethod.Post, new { enctype = "multipart/form-data" } ) )
	{ %>
	<p>
	<h2>
		<%= Html.Encode(Manual.InstallSubheading) %></h2>
	<%= Html.Encode( Manual.InstallInstruction )%></p>
	<input type="file" id="fileUpload" name="fileUpload" size="23" />
	</p>
	<p>
		<input type="submit" value="<%=Manual.Install %>" id="installLicense"/></p>
	<% } %>
</div>
