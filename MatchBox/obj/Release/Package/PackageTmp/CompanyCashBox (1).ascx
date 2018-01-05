<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyCashBox.ascx.cs" Inherits="MatchBox.CompanyCashBox" %>

<div style="margin: 10px;">
    <b>Number</b> : <asp:Label ID="lblCompanyNumber" runat="server" />
    &nbsp; &nbsp; &nbsp;
    <b>Name</b> : <asp:Label ID="lblCompanyName" runat="server" />
</div>

<hr style="margin: 10px;" />

<div style="margin: 10px;">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

