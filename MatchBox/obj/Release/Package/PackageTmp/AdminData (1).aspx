<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AdminData.aspx.cs" Inherits="MatchBox.AdminData" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <h1>Admin Data</h1>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="error block" />

    <section id="secCompany">
        <%--CompanyName--%>
        <div id="divCompanyName_Change" runat="server" visible="true" class="float-left div-form-field">
            <label for="<%= txtCompanyName.ClientID %>" class="block">Company Name</label>
            <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="50" />
            <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" ControlToValidate="txtCompanyName" ErrorMessage="Enter 'Company Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        </div>

        <%--Phone--%>
        <div id="divPhone_Change" runat="server" visible="true" class="float-left div-form-field">
            <label for="<%= txtPhone.ClientID %>" class="block">Phone</label>
            <asp:TextBox ID="txtPhone" runat="server" MaxLength="10" />
        </div>

        <%--Fax--%>
        <div id="divFax_Change" runat="server" visible="true" class="float-left div-form-field">
            <label for="<%= txtFax.ClientID %>" class="block">Fax</label>
            <asp:TextBox ID="txtFax" runat="server" MaxLength="10" />
        </div>

        <%--Mail--%>
        <div id="divMail_Change" runat="server" visible="true" class="float-left div-form-field">
            <label for="<%= txtMail.ClientID %>" class="block">Mail</label>
            <asp:TextBox ID="txtMail" runat="server" MaxLength="50" TextMode="Email" />
            <asp:RegularExpressionValidator ID="revMail" runat="server" ControlToValidate="txtMail" Display="Dynamic" SetFocusOnError="true" CssClass="error block"
                ErrorMessage="Enter valid mail." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
        </div>

        <%--City--%>
        <div id="divCity_Change" runat="server" visible="true" class="float-left div-form-field">
            <label for="<%= ddlCity.ClientID %>" class="block">City</label>
            <asp:DropDownList ID="ddlCity" runat="server">
                <asp:ListItem Text="" Value="0" />
            </asp:DropDownList>
        </div>

        <%--Address--%>
        <div id="divAddress_Change" runat="server" visible="true" class="float-left div-form-field">
            <label for="<%= txtAddress.ClientID %>" class="block">Address</label>
            <asp:TextBox ID="txtAddress" runat="server" MaxLength="50" />
        </div>

        <div class="clear-both"></div>

        <div class="div-form-command">
            <br />
            <asp:Button ID="btnSave" runat="server" Text="Submit" OnClick="btnSave_Click" />
        </div>
    </section>
</asp:Content>
