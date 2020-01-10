<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ManualActivationModel>" %>

<%@ Import Namespace="Resource.Views.Activation" %>
<%@ Import Namespace="Slps.Distributor.Web.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Manual.PageTitle%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="more_info">
        <a class="button help mini">?</a>
        <div id="info_text">
            <p>
                Manual activation will be required in situations where Distributor is unable to connect to the license activation service to process an online activation.
            </p>
            <p>
                First generate a manual activation request which must be sent to your license provider. The license provider will process the request and return a license file which must then be installed locally to complete the activation.
            </p>
            <h3>To generate a manual activation request:</h3>
            <ol>
                <li>Enter an activation key and click "Submit".</li>
                <li>Click on "Save File" to obtain a manual activation request file*.</li>
            </ol>

            <h3>To install a license file:</h3>
            <ol>
                <li>Copy the activated license file to this machine</li>
                <li>Select the license file and click on "Install".</li>
            </ol>
            <p>* Optionally copy and paste the request data text and send this to your license provider.</p>
            <%--     <%=Html.Encode( Manual.Help )%>--%>
        </div>
    </div>
    <h2><%= Html.Encode(Manual.GenerateSubheading) %></h2>
    <p><%= Html.Encode( Manual.GenerateInstruction )%></p>

    <div class="form_layout">
        <% using (Html.BeginForm("Manual", "Activation"))
            { %>
        <div>
            <div class="inputContainer">
                <div class="label">
                    <%=Html.Encode( Manual.ActivationKeyLabel )%>
                </div>
                <div style="float: none;">
                    <%=Html.TextBoxFor(t => t.ActivationKey, new { size = 35 })%>
                    <input type="submit" value="<%=Manual.Submit %>" class="float_right" />
                </div>
            </div>
            <div class="inputContainer">
                <div class="label">
                    <%=Html.Encode( Manual.RequestData )%>
                </div>
                <div style="float: none;">
                    <%  
                        var manualActivationRequestHtmlAttributes = new Dictionary<string, object> { { "class", "textArea" }, { "readonly", true } };
                        if (Model == null)
                            manualActivationRequestHtmlAttributes.Add("disabled", "true"); %>
                    <%= Html.TextAreaFor( model => model.ManualActivationRequest, manualActivationRequestHtmlAttributes ) %>
                </div>
            </div>
        </div>
        <% }
            using (Html.BeginForm("DownloadRequestFile", "Activation"))
            {  %>
        <%= Html.HiddenFor( model => model.ManualActivationRequest) %>
        <div class="inputContainer">
            <input type="submit" id="save_file_button" value="<%=Manual.SaveFile %>" class="float_right" />
        </div>
        <% } %>
    </div>
    <% Html.RenderPartial("Upload"); %>
</asp:Content>
