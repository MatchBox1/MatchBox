<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CashBoxForm.ascx.cs" Inherits="MatchBox.CashBoxForm" %>

<div class="div-form-item">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

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
            <asp:DropDownList ID="ddlNetwork" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNetwork_SelectedIndexChanged">
                <asp:ListItem Text="" Value="0" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvNetwork" runat="server" ControlToValidate="ddlNetwork" InitialValue="0" ErrorMessage="Select 'Network'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlCompany" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>

<%--Branch--%>
<asp:UpdatePanel ID="upBranch" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divBranch_Change" runat="server" class="float-left div-form-field">
            <label for="<%= ddlBranch.ClientID %>" class="block">Branch</label>
            <asp:DropDownList ID="ddlBranch" runat="server">
                <asp:ListItem Text="" Value="0" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch" InitialValue="0" ErrorMessage="Select 'Branch'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlCompany" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="ddlNetwork" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>

<div class="clear-both"></div>

<%--CashBoxNumber--%>
<div id="divCashBoxNumber_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtCashBoxNumber.ClientID %>" class="block">Cashbox Number</label>
    <asp:TextBox ID="txtCashBoxNumber" runat="server" MaxLength="9" />
    <asp:RequiredFieldValidator ID="rfvCashBoxNumber" runat="server" ControlToValidate="txtCashBoxNumber" ErrorMessage="Enter 'Cashbox Number'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--CashBoxName--%>
<div id="divCashBoxName_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtCashBoxName.ClientID %>" class="block">Cashbox Name</label>
    <asp:TextBox ID="txtCashBoxName" runat="server" MaxLength="50" />
    <asp:RequiredFieldValidator ID="rfvCashBoxName" runat="server" ControlToValidate="txtCashBoxName" ErrorMessage="Enter 'Cashbox Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<div class="clear-both"></div>

<div class="div-form-command">
    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
</div>
