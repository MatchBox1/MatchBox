<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NetworkForm.ascx.cs" Inherits="MatchBox.NetworkForm" %>

<h2 id="hedTitle" runat="server" class="hed-form"></h2>

<div class="div-form-item">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<%--NetworkNumber--%>
<div id="divNetworkNumber_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtNetworkNumber.ClientID %>" class="block">Network Number</label>
    <asp:TextBox ID="txtNetworkNumber" runat="server" MaxLength="9" />
    <asp:RequiredFieldValidator ID="rfvNetworkNumber" runat="server" ControlToValidate="txtNetworkNumber" ErrorMessage="Enter 'Network Number'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--NetworkName--%>
<div id="divNetworkName_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtNetworkName.ClientID %>" class="block">Network Name</label>
    <asp:TextBox ID="txtNetworkName" runat="server" MaxLength="50" />
    <asp:RequiredFieldValidator ID="rfvNetworkName" runat="server" ControlToValidate="txtNetworkName" ErrorMessage="Enter 'Network Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--Company--%>
<div id="divCompany_Change" runat="server" class="float-left div-form-field">
    <label for="<%= ddlCompany.ClientID %>" class="block">Company</label>
    <asp:DropDownList ID="ddlCompany" runat="server">
        <asp:ListItem Text="" Value="0" />
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="ddlCompany" InitialValue="0" ErrorMessage="Select 'Company'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--<div id="div1" runat="server" class="float-left div-form-field">
    <label for="<%= ddlCompany.ClientID %>" class="block">Company</label>
    <asp:DropDownList ID="DropDownList1" runat="server">
        <asp:ListItem Text="" Value="0" />
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCompany" InitialValue="0" ErrorMessage="Select 'Company'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>--%>

<div class="clear-both"></div>

<div class="div-form-command">
    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
</div>
