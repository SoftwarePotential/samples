<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ProductModel>" %>
<%@ Import Namespace="Resource.Views.Usage.For"%>
<%@ Import Namespace="Slps.Distributor.Web.Models"%>

<%@ Register src="../Shared/Controls/Refresher.ascx" tagname="Refresher" tagprefix="cc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"><%=For.PageTitle %></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">

		<h2><%=For.UsagesFor %> <%= Html.DisplayFor(model => model, "Product") %></h2>
		<div id="update"><%= Html.Action ("PoolUsageTable", "Usage") %></div>

		<script type="text/javascript">

			$(document).ready(markAlternatingRows);

			function updateUsage() {
				$.post('<%= Url.Action("PoolUsageTable", "Usage") +"/" + HttpUtility.UrlEncode(Model.Name).Replace("+", "%20") + "/" + HttpUtility.UrlEncode(Model.Version)%>',
						function (data) {
							$('div#update').html(data);
							markAlternatingRows();
						}
				);
			}

			function markAlternatingRows() {
				$("table").each(function () {
					$("> tbody > tr", this).filter(':not([id^="expander"])').filter(':odd').addClass("alt");
				});
			}


		</script>
		<cc:Refresher ID="UsageRefresher" runat="server" OnTimerTick="updateUsage();" />	

	</form> 

</asp:Content>
