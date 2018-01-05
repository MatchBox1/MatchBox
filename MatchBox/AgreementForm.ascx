<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgreementForm.ascx.cs" Inherits="MatchBox.AgreementForm" %>

<div class="div-form-item">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<%--Company--%>
<div id="divCompany_Change" runat="server" class="float-left div-form-field">
    <label for="<%= ddlCompany.ClientID %>" class="block">Company</label>
    <asp:DropDownList ID="ddlCompany" runat="server">
        <asp:ListItem Text="" Value="0" />
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="ddlCompany" InitialValue="0" ErrorMessage="Select 'Company'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--Credit--%>
<div id="divCredit_Change" runat="server" class="float-left div-form-field">
    <label for="<%= ddlCredit.ClientID %>" class="block">Credit</label>
    <asp:DropDownList ID="ddlCredit" runat="server">
        <asp:ListItem Text="" Value="0" />
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvCredit" runat="server" ControlToValidate="ddlCredit" InitialValue="0" ErrorMessage="Select 'Credit'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<div class="float-left div-form-command">
    <br />
    <asp:Button ID="btnSave" runat="server" Text="Save Agreement" OnClick="btnSave_Click" />
</div>

<div class="clear-both">
    <hr style="margin: 10px;" />
</div>

<div id="divSupplier_Change" runat="server" visible="false" style="margin: 10px;">
    <b>Suppliers</b>

    <div style="margin: 10px 0px;">
        <asp:Label ID="lblMessageSupplier" runat="server" EnableViewState="false" CssClass="message block" />
        <asp:Label ID="lblErrorSupplier" runat="server" EnableViewState="false" CssClass="error block" />
    </div>

    <ul>
        <li>
            <a id="lnkSupplierGroup" href="javascript: void(0);" onclick="javascript: display_supplier_form('group');">Add Supplier Group</a>
            &nbsp;/&nbsp;
            <a id="lnkSupplier" href="javascript: void(0);" onclick="javascript: display_supplier_form('supplier');">Add Supplier</a>
            &nbsp;/&nbsp;
            <a id="lnkSupplierUpload" href="javascript: void(0);" onclick="javascript: display_supplier_form('table');">Upload Table</a>
            &nbsp;/&nbsp;
            <a href="Download/Supplier.xlsx">Download Template</a>

            <div id="divSupplierGroup" class="no-display" style="margin: 10px 0px;">
                <asp:TextBox ID="txtSupplierGroupNumber" runat="server" MaxLength="9" Width="200px" placeholder="Supplier Group Number" style="margin-bottom: 5px;" />
                <asp:TextBox ID="txtSupplierGroupName" runat="server" MaxLength="20" Width="200px" placeholder="Supplier Group Name" style="margin-bottom: 5px;" />
                <asp:Button ID="btnSaveSupplierGroup" runat="server" ValidationGroup="SupplierGroup" OnClick="btnSaveSupplierGroup_Click" Text="Save" />
                &nbsp;
                <a href="javascript: void(0);" onclick="javascript: display_supplier_form('');">Close</a>
                <br />
                <asp:RequiredFieldValidator ID="rfvSupplierGroupNumber" runat="server" ValidationGroup="SupplierGroup" ControlToValidate="txtSupplierGroupNumber" ErrorMessage="Enter 'Supplier Group Number'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
                <asp:CompareValidator ID="cvSupplierGroupNumber" runat="server" ValidationGroup="SupplierGroup" ControlToValidate="txtSupplierGroupNumber" Operator="DataTypeCheck" Type="Integer" ErrorMessage="'Supplier Group Number' must be a whole number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
                <asp:RangeValidator ID="rvSupplierGroupNumber" runat="server" ValidationGroup="SupplierGroup" ControlToValidate="txtSupplierGroupNumber" MinimumValue="1" MaximumValue="1000000000" Type="Integer" ErrorMessage="'Supplier Group Number' must be a positive number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
            </div>

            <div id="divSupplier" class="no-display" style="margin: 10px 0px;">
                <asp:TextBox ID="txtSupplierNumber" runat="server" MaxLength="9" Width="200px" placeholder="Supplier Number" style="margin-bottom: 5px;" />
                <asp:TextBox ID="txtSupplierName" runat="server" MaxLength="20" Width="200px" placeholder="Supplier Name" style="margin-bottom: 5px;" />
                <asp:DropDownList ID="ddlSupplierGroup" runat="server" Width="200px" style="margin-bottom: 5px;" />
                <asp:Button ID="btnSaveSupplier" runat="server" ValidationGroup="Supplier" OnClick="btnSaveSupplier_Click" Text="Save" />
                &nbsp;
                <a href="javascript: void(0);" onclick="javascript: display_supplier_form('');">Close</a>
                <br />
                <asp:RequiredFieldValidator ID="rfvSupplierNumber" runat="server" ValidationGroup="Supplier" ControlToValidate="txtSupplierNumber" ErrorMessage="Enter 'Supplier Number'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
                <asp:CompareValidator ID="cvSupplier" runat="server" ValidationGroup="Supplier" ControlToValidate="txtSupplierNumber" Operator="DataTypeCheck" Type="Integer" ErrorMessage="'Supplier Number' must be a whole number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
                <asp:RangeValidator ID="rvSupplier" runat="server" ValidationGroup="Supplier" ControlToValidate="txtSupplierNumber" MinimumValue="1" MaximumValue="1000000000" Type="Integer" ErrorMessage="'Supplier Number' must be a positive number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
                <asp:RequiredFieldValidator ID="rfvSupplierGroup" runat="server" ValidationGroup="Supplier" ControlToValidate="ddlSupplierGroup" InitialValue="0" ErrorMessage="Select 'Supplier Group'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
            </div>

            <div id="divSupplierUpload" class="no-display" style="margin: 10px 0px;">
                <asp:Label ID="lblSupplierUpload" runat="server" AssociatedControlID="fuSupplierUpload" Text="Suppliers File" />
                <asp:FileUpload ID="fuSupplierUpload" runat="server" />
                <asp:Button ID="btnUploadSupplier" runat="server" ValidationGroup="SupplierUpload" OnClick="btnUploadSupplier_Click" Text="Upload" />
                &nbsp;
                <a href="javascript: void(0);" onclick="javascript: display_supplier_form('');">Close</a>
                <br />
                <asp:RequiredFieldValidator ID="rfvSupplierUpload" runat="server" ValidationGroup="SupplierUpload" ControlToValidate="fuSupplierUpload" ErrorMessage="Select 'Suppliers File'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
            </div>
        </li>
    </ul>

    <asp:Repeater ID="repSupplierGroup" runat="server" OnItemDataBound="repSupplierGroup_ItemDataBound">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
                <li>
                    [ <asp:LinkButton ID="btnDeleteSupplierGroup" runat="server" OnCommand="btnDeleteSupplierGroup_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="x" OnClientClick="javascript: return confirm('Delete Item?');" ToolTip="Delete" ForeColor="Red" CssClass="no-line" /> ]
                    &nbsp;
                    <%# DataBinder.Eval(Container.DataItem, "SupplierGroup") %>

                    <asp:Repeater ID="repSupplier" runat="server">
                        <HeaderTemplate>
                            <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <li>
                                    [ <asp:LinkButton ID="btnDeleteSupplier" runat="server" OnCommand="btnDeleteSupplier_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="x" OnClientClick="javascript: return confirm('Delete Item?');" ToolTip="Delete" ForeColor="Red" CssClass="no-line" /> ]
                                    &nbsp;
                                    <%# DataBinder.Eval(Container.DataItem, "Supplier") %>
                                </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>

<script>
    function display_supplier_form(s_form) {
        document.getElementById("divSupplierGroup").className = "no-display";
        document.getElementById("divSupplier").className = "no-display";
        document.getElementById("divSupplierUpload").className = "no-display";

        document.getElementById("lnkSupplierGroup").style.removeProperty("color");
        document.getElementById("lnkSupplier").style.removeProperty("color");
        document.getElementById("lnkSupplierUpload").style.removeProperty("color");

        switch (s_form) {
            case "group":
                document.getElementById("divSupplierGroup").className = "";
                document.getElementById("lnkSupplierGroup").style.color = "red";
                document.getElementById("<%= txtSupplierGroupNumber.ClientID %>").focus();
                break;
            case "supplier":
                document.getElementById("divSupplier").className = "";
                document.getElementById("lnkSupplier").style.color = "red";
                document.getElementById("<%= txtSupplierNumber.ClientID %>").focus();
                break;
            case "table":
                document.getElementById("divSupplierUpload").className = "";
                document.getElementById("lnkSupplierUpload").style.color = "red";
                document.getElementById("<%= fuSupplierUpload.ClientID %>").focus();
                break;
        }
    }
</script>