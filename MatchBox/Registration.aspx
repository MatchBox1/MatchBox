<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="MatchBox.Registration" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <h1>Registration</h1>

    <section id="secRegistration">
        <asp:Label ID="lblMsg" runat="server" CssClass="error" />

        <div class="div-form-field">
            <label for="<%= txtFullName.ClientID %>" class="block">Full Name</label>
            <asp:TextBox ID="txtFullName" runat="server" MaxLength="25" />
            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Enter 'Full Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        </div>
        <div class="div-form-field">
            <label for="<%= txtUserName.ClientID %>" class="block">User Name</label>
            <asp:TextBox ID="txtUserName" runat="server" MaxLength="25" />
            <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName" ErrorMessage="Enter 'User Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        </div>
        <div class="div-form-field">
            <label for="<%= txtMail.ClientID %>" class="block">Mail</label>
            <asp:TextBox ID="txtMail" runat="server" MaxLength="50" TextMode="Email" />
            <asp:RequiredFieldValidator ID="rfvMail" runat="server" ControlToValidate="txtMail" ErrorMessage="Enter 'Mail'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
            <asp:RegularExpressionValidator ID="revMail" runat="server" ControlToValidate="txtMail" Display="Dynamic" SetFocusOnError="true" CssClass="error block"
                ErrorMessage="Enter valid mail." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
        </div>
        <div class="div-form-field">
            <label for="<%= txtPassword.ClientID %>" class="block">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" MaxLength="25" TextMode="Password" />
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Enter 'Password'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        </div>
        <div class="div-form-field">
            <label for="<%= txtPasswordRepeat.ClientID %>" class="block">Repeat Password</label>
            <asp:TextBox ID="txtPasswordRepeat" runat="server" MaxLength="25" TextMode="Password" />
            <asp:RequiredFieldValidator ID="rfvPasswordRepeat" runat="server" ControlToValidate="txtPasswordRepeat" ErrorMessage="Please repeat 'Password'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
            <asp:CompareValidator ID="cvPasswordRepeat" runat="server" ControlToValidate="txtPasswordRepeat" ControlToCompare="txtPassword" Display="Dynamic" SetFocusOnError="true" CssClass="error block"
                Type="String" Operator="Equal" ErrorMessage="'Password' and 'Repeat Password' mast be a same." />
        </div>

        <div class="div-form-command">
            <br />
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
        </div>
    </section>
</asp:Content>
