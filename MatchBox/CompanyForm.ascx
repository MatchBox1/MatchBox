<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyForm.ascx.cs" Inherits="MatchBox.CompanyForm" %>

<div class="div-form-item">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<%--CompanyNumber--%>
<div id="divCompanyNumber_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtCompanyNumber.ClientID %>" class="block">Company Number</label>
    <asp:TextBox ID="txtCompanyNumber" runat="server" MaxLength="15" />
    <asp:RequiredFieldValidator ID="rfvCompanyNumber" runat="server" ControlToValidate="txtCompanyNumber" ErrorMessage="Enter 'Company Number'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    <asp:CompareValidator ID="cvCompanyNumber" runat="server" ControlToValidate="txtCompanyNumber" Type="Integer" Operator="GreaterThan" ValueToCompare="0" ErrorMessage="Value must be a whole number greater than zero." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--CompanyName--%>
<div id="divCompanyName_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtCompanyName.ClientID %>" class="block">Company Name</label>
    <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="50" />
    <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" ControlToValidate="txtCompanyName" ErrorMessage="Enter 'Company Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

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
    <asp:RegularExpressionValidator ID="revMail" runat="server" ControlToValidate="txtMail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Enter valid mail." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
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

<br />

<%--Terminal--%>
<div id="divTerminal_Change" runat="server" style="margin: 10px;">
    <b>Terminals</b>

    <div style="margin: 10px 0px;">
        <asp:Label ID="lblMessageTerminal" runat="server" EnableViewState="false" CssClass="message block" />
        <asp:Label ID="lblErrorTerminal" runat="server" EnableViewState="false" CssClass="error block" />
    </div>

    <ul>
        <li>
            <a id="lnkTerminal" href="javascript: void(0);" onclick="javascript: display_terminal_form('single');">Add Terminal</a>
            &nbsp;/&nbsp;
            <a id="lnkTerminalUpload" href="javascript: void(0);" onclick="javascript: display_terminal_form('table');">Upload Table</a>
            &nbsp;/&nbsp;
            <a href="Download/Terminal.xlsx">Download Template</a>

            <div id="divTerminal" class="no-display" style="margin: 10px 0px;">
                <asp:TextBox ID="txtTerminalNumber" runat="server" MaxLength="9" Width="200px" placeholder="Terminal Number" />
                <asp:TextBox ID="txtTerminalName" runat="server" MaxLength="20" Width="200px" placeholder="Terminal Name" style="margin: 5px 0px;" />
                <asp:Button ID="btnSaveTerminal" runat="server" ValidationGroup="Terminal" OnClick="btnSaveTerminal_Click" Text="Save" />
                &nbsp;
                <a href="javascript: void(0);" onclick="javascript: display_terminal_form('');">Close</a>
                <br />
                <asp:RequiredFieldValidator ID="rfvTerminalNumber" runat="server" ValidationGroup="Terminal" ControlToValidate="txtTerminalNumber" ErrorMessage="Enter 'Terminal Number'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
                <asp:CompareValidator ID="cvTerminalNumber" runat="server" ValidationGroup="Terminal" ControlToValidate="txtTerminalNumber" Operator="DataTypeCheck" Type="Integer" ErrorMessage="'Terminal Number' must be a whole number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
                <asp:RangeValidator ID="rvTerminalNumber" runat="server" ValidationGroup="Terminal" ControlToValidate="txtTerminalNumber" MinimumValue="1" MaximumValue="1000000000" Type="Integer" ErrorMessage="'Terminal Number' must be a positive number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
            </div>

            <div id="divTerminalUpload" class="no-display" style="margin: 10px 0px;">
                <asp:Label ID="lblTerminalUpload" runat="server" AssociatedControlID="fuTerminalUpload" Text="Terminals File" />
                <asp:FileUpload ID="fuTerminalUpload" runat="server" />
                <asp:Button ID="btnUploadTerminal" runat="server" ValidationGroup="TerminalUpload" OnClick="btnUploadTerminal_Click" Text="Upload" />
                &nbsp;
                <a href="javascript: void(0);" onclick="javascript: display_terminal_form('');">Close</a>
                <br />
                <asp:RequiredFieldValidator ID="rfvTerminalUpload" runat="server" ValidationGroup="TerminalUpload" ControlToValidate="fuTerminalUpload" ErrorMessage="Select 'Terminals File'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
            </div>
        </li>
    </ul>

    <asp:Repeater ID="repTerminal" runat="server">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
                <li>
                    [ <asp:LinkButton ID="btnDeleteTerminal" runat="server" OnCommand="btnDeleteTerminal_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="x" OnClientClick="javascript: return confirm('Delete Item?');" ToolTip="Delete" ForeColor="Red" CssClass="no-line" /> ]
                    &nbsp;
                    <%# DataBinder.Eval(Container.DataItem, "Terminal") %>
                </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>

<script>
    function display_terminal_form(s_form) {
        document.getElementById("divTerminal").className = "no-display";
        document.getElementById("divTerminalUpload").className = "no-display";

        document.getElementById("lnkTerminal").style.removeProperty("color");
        document.getElementById("lnkTerminalUpload").style.removeProperty("color");

        if (s_form == "single") {
            document.getElementById("divTerminal").className = "";
            document.getElementById("lnkTerminal").style.color = "red";
            document.getElementById("<%= txtTerminalNumber.ClientID %>").focus();
        }
        else if (s_form == "table") {
            document.getElementById("divTerminalUpload").className = "";
            document.getElementById("lnkTerminalUpload").style.color = "red";
            document.getElementById("<%= fuTerminalUpload.ClientID %>").focus();
        }
    }
</script>