<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="MatchBox.UserDetails" %>

<%@ Register TagPrefix="uc" TagName="UserForm" Src="~/UserForm.ascx" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc:UserForm ID="ucUserForm" runat="server" />
</asp:Content>
