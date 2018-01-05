<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MatchBox.Login" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <h1>Login</h1>

    <section id="secLogin">
        <asp:Label ID="lblMsg" runat="server" CssClass="error" />
        <div class="div-form-field">
            <label for="<%= txtUserName.ClientID %>" class="block">User Name</label>
            <asp:TextBox ID="txtUserName" runat="server" MaxLength="25" />
            <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName" ErrorMessage="Enter 'User Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        </div>
        <div class="div-form-field">
            <label for="<%= txtPassword.ClientID %>" class="block">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" MaxLength="25" TextMode="Password" />
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Enter 'Password'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        </div>
        <div class="div-form-command">
            <br />
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
        </div>
    </section>
</asp:Content>
