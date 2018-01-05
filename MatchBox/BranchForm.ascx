<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BranchForm.ascx.cs" Inherits="MatchBox.BranchForm" %>

<h2 id="hedTitle" runat="server" class="hed-form"></h2>

<div class="div-form-item">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<%--BranchNumber--%>
<div id="divBranchNumber_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtBranchNumber.ClientID %>" class="block">Branch Number</label>
    <asp:TextBox ID="txtBranchNumber" runat="server" MaxLength="9" />
    <asp:RequiredFieldValidator ID="rfvBranchNumber" runat="server" ControlToValidate="txtBranchNumber" ErrorMessage="Enter 'Branch Number'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--BranchName--%>
<div id="divBranchName_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtBranchName.ClientID %>" class="block">Branch Name</label>
    <asp:TextBox ID="txtBranchName" runat="server" MaxLength="50" />
    <asp:RequiredFieldValidator ID="rfvBranchName" runat="server" ControlToValidate="txtBranchName" ErrorMessage="Enter 'Branch Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<div class="clear-both"></div>

<%--Company--%>
<div id="divCompany_Change" runat="server" class="float-left div-form-field">
    <label for="<%= ddlCompany.ClientID %>" class="block">Company</label>
    <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
        <asp:ListItem Text="" Value="0" />
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="ddlCompany" InitialValue="0" ErrorMessage="Select 'Company'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--Network--%>
<asp:UpdatePanel ID="upNetwork" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divNetwork_Change" runat="server" class="float-left div-form-field">
            <label for="<%= ddlNetwork.ClientID %>" class="block">Network</label>
            <asp:DropDownList ID="ddlNetwork" runat="server">
                <asp:ListItem Text="" Value="0" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvNetwork" runat="server" ControlToValidate="ddlNetwork" InitialValue="0" ErrorMessage="Select 'Network'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlCompany" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>

<div class="clear-both"></div>

<%--Phone--%>
<div id="divPhone_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtPhone.ClientID %>" class="block">Phone</label>
    <asp:TextBox ID="txtPhone" runat="server" MaxLength="15" />
</div>

<%--Fax--%>
<div id="divFax_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtFax.ClientID %>" class="block">Fax</label>
    <asp:TextBox ID="txtFax" runat="server" MaxLength="15" />
</div>

<%--Mail--%>
<div id="divMail_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtMail.ClientID %>" class="block">Mail</label>
    <asp:TextBox ID="txtMail" runat="server" MaxLength="50" TextMode="Email" />
    <asp:RegularExpressionValidator ID="revMail" runat="server" ControlToValidate="txtMail" Display="Dynamic" SetFocusOnError="true" CssClass="error block"
        ErrorMessage="Enter valid mail." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
</div>

<div class="clear-both"></div>

<%--City--%>
<div id="divCity_Change" runat="server" class="float-left div-form-field">
    <label for="<%= ddlCity.ClientID %>" class="block">City</label>
    <asp:DropDownList ID="ddlCity" runat="server">
        <asp:ListItem Text="" Value="0" />
    </asp:DropDownList>
</div>

<%--Address--%>
<div id="divAddress_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtAddress.ClientID %>" class="block">Address</label>
    <asp:TextBox ID="txtAddress" runat="server" MaxLength="50" />
</div>

<div class="clear-both"></div>

<div class="div-form-command">
    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
</div>
