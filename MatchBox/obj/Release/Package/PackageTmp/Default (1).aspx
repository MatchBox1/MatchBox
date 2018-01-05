<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MatchBox.Default" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <p id="pUserData" runat="server" visible="false">
        Mail: <asp:Label ID="lblMail" runat="server" />
        <br />
        Phone: <asp:Label ID="lblPhone" runat="server" />
        <br />
        Mobile: <asp:Label ID="lblMobile" runat="server" />
        <br />
        City: <asp:Label ID="lblCity" runat="server" />
        <br />
        Address: <asp:Label ID="lblAddress" runat="server" />
    </p>
</asp:Content>
