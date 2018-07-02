<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ProductModel>" %>

<%@ Import Namespace="Resource_Views.NamedUser" %>
<%@ Import Namespace="Slps.Distributor.Web.Models" %>
<%@ Import Namespace="Slps.Distributor.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"><%=For.PageTitle %></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="<%=Url.Content("~/scripts/moment.min.js")%>"> </script>
    <script type="text/javascript" src="<%=Url.Content("~/scripts/underscore.min.js")%>"> </script>
    <script type="text/javascript" src="<%=Url.Content("~/scripts/mustache.min.js")%>"> </script>
    <script type="text/javascript" src="<%=Url.Content("~/scripts/nameduser.js")%>"> </script>

    <h2><%=For.NamedUsersFor %> <%= Html.DisplayFor(model => model, "Product") %></h2>
    <aside id="more_info">
        <a class="button help mini">?</a>
        <div id="info_text">
            <p>This section allows the management of licenses locked to named users.</p>            
            <p><%=Html.Encode(MvcApplication.Instance.VendorServices.NamedUserInstructions) %></p>
            <p>Users may be assigned by entering a unique username and selecting an available assignment duration. 
                The user will then be locked for the given pre-defined minimum duration. Once the minimum assignment duration has expired, the user can then be unassigned. 
                A user may continue to use the system, even when expired, until such time as they are explicitly unassigned.</p>
            <p>Multiple users may be assigned by using a semicolon separated list e.g. user1;user2;user3</p>
        </div>
    </aside>
    <section id="addNamedUser">
        <header>
            <h3>Available</h3>
        </header>
        <%=Html.HiddenFor(x => x.Name)%>
        <%=Html.HiddenFor(x => x.Version)%>
        <table 
            <thead>
                <th>Minimum Assignment Duration</th>
                <th>Remaining Users</th>
            </thead>
            <tbody id="availabilityTable">           
            </tbody>
        </table>
    </section>
    <section>
        <header>
            <h3>Assign User</h3>
        </header>
        <div>
            <div class="form_pair">
                <label>Username</label>
                <textarea id="username" rows="1" cols="30"></textarea>
            </div>
            <div class="form_pair">
                <label>Minimum Duration</label>
                <select id="duration">         
                </select>
            </div>
            <button id="assign">Assign</button>            
            <label id="assignMessage" style="display:none"></label>
        </div>
    </section>
    <section>
        <header>
            <h3>Assigned Users</h3>
        </header>  
        <label id="unassignErrorMessage" style="display: none; color: red; margin: 5px"></label>
        <table style="margin-top: 10px">
            <thead>
                <th>Username</th>
                <th>Lock Duration</th>
                <th>Expiration</th>
                <th></th>
            </thead>
            <tbody id="namedUsersTable">         

            </tbody>
        </table>
    </section>
    <script id="availability-template" type="text/template">
        {{#availability}}
            <tr>
                <td>{{LockDuration}} days</td>
                <td>{{Available}}</td>
            </tr>
        {{/availability}}
    </script>
    <script id="duration-template" type="text/template">
        {{#durations}}
            <option value="{{LockDuration}}">{{LockDuration}}</option>
        {{/durations}}
    </script>
    <script id="namedUsers-template" type="text/template">
        {{#users}}
            <tr>
                <td class="username">{{Username}}</td>
                <td>{{LockDuration}} days</td>
                <td>{{Expiry}}</td>
                <td>{{#unlocked}}
                        <button class="unassign">Unassign</button>
                    {{/unlocked}}
                    {{^unlocked}}
                        Locked
                    {{/unlocked}}
                </td>
            </tr>
        {{/users}}
    </script>
</asp:Content>
