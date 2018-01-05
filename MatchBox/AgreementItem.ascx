<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgreementItem.ascx.cs" Inherits="MatchBox.AgreementItem" %>

<table>
    <tr>
        <td class="nowrap" style="padding: 0px 10px;"><b>Company</b> : <asp:Label ID="lblCompanyName" runat="server" /></td>
        <td class="nowrap" style="padding: 0px 10px;">&nbsp;</td>
        <td class="nowrap" style="padding: 0px 10px;"><b>Credit</b> : <asp:Label ID="lblCreditName" runat="server" /></td>
        <td class="nowrap" style="padding: 0px 10px; width: 100%;">&nbsp;</td>
        <td class="nowrap" style="padding: 0px 10px;"><a href="javascript: void(0);" onclick="javascript: display_agreement_item_form();">Add Clearing Item</a></td>
    </tr>
</table>

<hr style="margin: 10px;" />

<div style="margin: 10px;">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<div id="divAgreementItemForm" class="no-display">
    <h3 style="margin: 10px;">New Clearing Item</h3>

    <div class="div-form-command">
        <br />
        <asp:Button ID="btnSaveTop" runat="server" ValidationGroup="AgreementItem" Text="Save" OnClick="btnSave_Click" />
        &nbsp;
        <a href="javascript: void(0);" onclick="javascript: display_by_class('divAgreementItemForm');">Close</a>
    </div>

    <asp:UpdatePanel runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:CustomValidator ID="cvDateRange" runat="server" ValidationGroup="AgreementItem" OnServerValidate="cvDateRange_ServerValidate" Display="Dynamic" CssClass="error block div-form-message" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtDateStart" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txtDateEnd" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <%--DateStart--%>
    <div id="divDateStart_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtDateStart.ClientID %>" class="block">Start Date</label>
        <asp:TextBox ID="txtDateStart" runat="server" MaxLength="10" AutoPostBack="true" OnTextChanged="Date_Changed" placeholder="dd/mm/yyyy" />
        <asp:RequiredFieldValidator ID="rfvDateStart" runat="server" ValidationGroup="AgreementItem" ControlToValidate="txtDateStart" ErrorMessage="Enter 'Start Date'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--DateEnd--%>
    <div id="divDateEnd_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtDateEnd.ClientID %>" class="block">End Date</label>
        <asp:TextBox ID="txtDateEnd" runat="server" MaxLength="10" AutoPostBack="true" OnTextChanged="Date_Changed" placeholder="dd/mm/yyyy" />
        <asp:RequiredFieldValidator ID="rfvDateEnd" runat="server" ValidationGroup="AgreementItem" ControlToValidate="txtDateEnd" ErrorMessage="Enter 'End Date'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--Payments--%>
    <div id="divPayments_Change" runat="server" class="float-left div-form-field">
        Payments
        <br />
        <label for="<%= txtPaymentsFrom.ClientID %>">From</label>
        <asp:TextBox ID="txtPaymentsFrom" runat="server" MaxLength="2" Width="93px" Text="1" />
        &nbsp;
        <label for="<%= txtPaymentsTo.ClientID %>">To</label>
        <asp:TextBox ID="txtPaymentsTo" runat="server" MaxLength="2" Width="93px" Text="1" />

        <asp:RequiredFieldValidator ID="rfvPaymentsFrom" runat="server" ValidationGroup="AgreementItem" ControlToValidate="txtPaymentsFrom" ErrorMessage="Enter 'Payments From'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:CompareValidator ID="cvPaymentsFrom" runat="server" ValidationGroup="AgreementItem" ControlToValidate="txtPaymentsFrom" Operator="DataTypeCheck" Type="Integer" ErrorMessage="'Payments From' must be a whole number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:RangeValidator id="rvPaymentsFrom" runat="server" ValidationGroup="AgreementItem" ControlToValidate="txtPaymentsFrom" MinimumValue="1" MaximumValue="99" Type="Integer" Text="'Payments From' must be greater than 0." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />

        <asp:RequiredFieldValidator ID="rfvPaymentsTo" runat="server" ValidationGroup="AgreementItem" ControlToValidate="txtPaymentsTo" ErrorMessage="Enter 'Payments To'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:CompareValidator ID="cvPaymentsTo" runat="server" ValidationGroup="AgreementItem" ControlToValidate="txtPaymentsTo" Operator="DataTypeCheck" Type="Integer" ErrorMessage="'Payments To' must be a whole number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:RangeValidator id="rvPaymentsTo" runat="server" ValidationGroup="AgreementItem" ControlToValidate="txtPaymentsTo" MinimumValue="1" MaximumValue="99" Type="Integer" Text="'Payments To' must be greater than 0." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <div class="clear-both"></div>

    <%--Card--%>
    <asp:UpdatePanel runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divCard_Change" runat="server" class="float-left div-form-field">
                <label for="<%= ddlCard.ClientID %>" class="block">Card</label>
                <asp:DropDownList ID="ddlCard" runat="server">
                    <asp:ListItem Text="" Value="0" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvCard" runat="server" ValidationGroup="AgreementItem" ControlToValidate="ddlCard" InitialValue="0" ErrorMessage="Select 'Card'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtDateStart" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txtDateEnd" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <%--OperationType--%>
    <div id="divOperationType_Change" runat="server" class="float-left div-form-field">
        <label for="<%= ddlOperationType.ClientID %>" class="block">Operation Type</label>
        <asp:DropDownList ID="ddlOperationType" runat="server">
            <asp:ListItem Text="" Value="0" />
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvOperationType" runat="server" ValidationGroup="AgreementItem" ControlToValidate="ddlOperationType" InitialValue="0" ErrorMessage="Select 'Operation Type'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--Commission--%>
    <div id="divCommission_Change" runat="server" class="float-left div-form-field">
        Commission
        <br />
        <label for="<%= txtCommissionLocal.ClientID %>">Local</label>
        <asp:TextBox ID="txtCommissionLocal" runat="server" MaxLength="5" Width="57px" /> %
        &nbsp;
        <label for="<%= txtCommissionAbroad.ClientID %>">Abroad</label>
        <asp:TextBox ID="txtCommissionAbroad" runat="server" MaxLength="5" Width="57px" /> %

        <asp:RequiredFieldValidator ID="rfvCommissionLocal" runat="server" ValidationGroup="AgreementItem" ControlToValidate="txtCommissionLocal" ErrorMessage="Enter 'Local Commission'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:CompareValidator ID="cvCommissionLocal" runat="server" ValidationGroup="AgreementItem" ControlToValidate="txtCommissionLocal" Operator="DataTypeCheck" Type="Double" ErrorMessage="'Local Commission' must be a number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />

        <asp:RequiredFieldValidator ID="rfvCommissionAbroad" runat="server" ValidationGroup="AgreementItem" ControlToValidate="txtCommissionAbroad" ErrorMessage="Enter 'Abroad Commission'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:CompareValidator ID="cvCommissionAbroad" runat="server" ValidationGroup="AgreementItem" ControlToValidate="txtCommissionAbroad" Operator="DataTypeCheck" Type="Double" ErrorMessage="'Abroad Commission' must be a number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <div class="clear-both"></div>

    <%--SupplierGroup--%>
    <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divSupplierGroup_Change" runat="server" class="float-left div-form-field">
                <label for="<%= lstSupplierGroup.ClientID %>" class="block">Supplier Group</label>
                <asp:CheckBox ID="chkSupplierGroup" runat="server" AutoPostBack="true" OnCheckedChanged="chkSupplierGroup_CheckedChanged" Visible="false" Text="Select All" CssClass="block" style="margin: 2px;" />
                <asp:CustomValidator ID="cvSupplierGroup" runat="server" ValidationGroup="AgreementItem" ClientValidationFunction="validate_supplier_group_list" ErrorMessage="Select 'Supplier Group'." Display="Dynamic" SetFocusOnError="true" CssClass="error" />
                <asp:CheckBoxList ID="lstSupplierGroup" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstSupplierGroup_SelectedIndexChanged" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtDateStart" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txtDateEnd" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="chkSupplierGroup" EventName="CheckedChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <%--Supplier--%>
    <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divSupplier_Change" runat="server" class="float-left div-form-field">
                <label for="<%= lstSupplier.ClientID %>" class="block">Supplier</label>
                <asp:CheckBox ID="chkSupplier" runat="server" AutoPostBack="true" OnCheckedChanged="chkSupplier_CheckedChanged" Visible="false" Text="Select All" CssClass="block" style="margin: 2px;" />
                <asp:CustomValidator ID="cvSupplier" runat="server" ValidationGroup="AgreementItem" ClientValidationFunction="validate_supplier_list" ErrorMessage="Select 'Supplier'." Display="Dynamic" SetFocusOnError="true" CssClass="error" />
                <asp:CheckBoxList ID="lstSupplier" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstSupplier_SelectedIndexChanged" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtDateStart" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txtDateEnd" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="chkSupplierGroup" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="chkSupplier" EventName="CheckedChanged" />

            <asp:AsyncPostBackTrigger ControlID="lstSupplierGroup" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <%--Terminal--%>
    <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divTerminal_Change" runat="server" class="float-left div-form-field">
                <label for="<%= lstTerminal.ClientID %>" class="block">Terminal</label>
                <asp:CheckBox ID="chkTerminal" runat="server" AutoPostBack="true" OnCheckedChanged="chkTerminal_CheckedChanged" Visible="false" Text="Select All" CssClass="block" style="margin: 2px;" />
                <asp:CustomValidator ID="cvTerminal" runat="server" ValidationGroup="AgreementItem" ClientValidationFunction="validate_terminal_list" ErrorMessage="Select 'Terminal'." Display="Dynamic" SetFocusOnError="true" CssClass="error" />
                <asp:CheckBoxList ID="lstTerminal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstTerminal_SelectedIndexChanged" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtDateStart" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txtDateEnd" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="chkSupplierGroup" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="chkSupplier" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="chkTerminal" EventName="CheckedChanged" />

            <asp:AsyncPostBackTrigger ControlID="lstSupplierGroup" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="lstSupplier" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="clear-both"></div>

    <%--Terminal + Network > Branch > Cashbox--%>
    <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divCashBox_Change" runat="server" class="div-form-item">
                <label for="<%= lstCashBox.ClientID %>">Terminal &hArr; Network &rarr; Branch &rarr; Cashbox</label>
                <asp:CheckBox ID="chkCashBox" runat="server" AutoPostBack="true" OnCheckedChanged="chkCashBox_CheckedChanged" Visible="false" Text="Select All" CssClass="block" style="margin: 2px;" />
                <asp:CheckBoxList ID="lstCashBox" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstCashBox_SelectedIndexChanged" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtDateStart" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txtDateEnd" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="chkSupplierGroup" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="chkSupplier" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="chkTerminal" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="chkCashBox" EventName="CheckedChanged" />

            <asp:AsyncPostBackTrigger ControlID="lstSupplierGroup" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="lstSupplier" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="lstTerminal" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="div-form-command">
        <br />
        <asp:Button ID="btnSaveBottom" runat="server" ValidationGroup="AgreementItem" Text="Save" OnClick="btnSave_Click" />
        &nbsp;
        <a href="javascript: void(0);" onclick="javascript: display_by_class('divAgreementItemForm');">Close</a>
    </div>

    <hr style="margin: 10px;" />
</div>

<asp:Repeater ID="repAgreementPeriod" runat="server" OnItemDataBound="repAgreementPeriod_ItemDataBound">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
            <li>
                <b>Start Date</b> : <%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateStart")) %>
                &nbsp;
                <b>End Date</b> : <%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateEnd")) %>

                <asp:Repeater ID="repAgreementItem" runat="server" OnItemDataBound="repAgreementItem_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table-sub" style="margin: 10px 0px;">
                            <tr>
                                <th rowspan="2">Start Date</th>
                                <th rowspan="2">End Date</th>
                                <th colspan="2">Payments</th>
                                <th rowspan="2">Card</th>
                                <th rowspan="2">Operation Type</th>
                                <th colspan="2">Commission</th>
                                <th rowspan="2">Group / Supplier / Terminal</th>
                                <th rowspan="2">Copy</th>
                                <th rowspan="2">Discount</th>
                                <th rowspan="2">x</th>
                            </tr>
                            <tr>
                                <th>From</th>
                                <th>To</th>
                                <th>Local</th>
                                <th>Abroad</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                            <tr>
                                <td style="vertical-align: top;"><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateStart")) %></td>
                                <td style="vertical-align: top;"><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateEnd")) %></td>
                                <td style="vertical-align: top;"><%# DataBinder.Eval(Container.DataItem, "PaymentsFrom") %></td>
                                <td style="vertical-align: top;"><%# DataBinder.Eval(Container.DataItem, "PaymentsTo") %></td>
                                <td style="vertical-align: top;"><%# DataBinder.Eval(Container.DataItem, "CardName") %></td>
                                <td style="vertical-align: top;"><%# DataBinder.Eval(Container.DataItem, "OperationTypeName") %></td>
                                <td style="vertical-align: top;"><%# DataBinder.Eval(Container.DataItem, "CommissionLocal") %>%</td>
                                <td style="vertical-align: top;"><%# DataBinder.Eval(Container.DataItem, "CommissionAbroad") %>%</td>
                                <td style="vertical-align: top;">
                                    <a id="lnkAgreementItemTerminal" runat="server" href="javascript: void(0);" onclick="javascript: display_agreement_item_terminal(this.id);">Display</a>

                                    <div id="divAgreementItemTerminal" runat="server" class="no-display">
                                        <asp:Repeater ID="repAgreementItemTerminal" runat="server">
                                            <HeaderTemplate>
                                                <table class="table-list">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                    <tr>
                                                        <td><%# DataBinder.Eval(Container.DataItem, "SupplierGroupNumber") %></td>
                                                        <td><%# DataBinder.Eval(Container.DataItem, "SupplierNumber") %></td>
                                                        <td><%# DataBinder.Eval(Container.DataItem, "TerminalNumber") %></td>
                                                    </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </td>
                                <td style="vertical-align: top;" class="nowrap text-center"><a id="lnkCopyItem" runat="server" class="no-line" title="Copy Clearing Item">Copy</a></td>
                                <td style="vertical-align: top;" class="nowrap text-center"><a id="lnkAddDiscount" runat="server" class="no-line" title="Add Discount Item">Add</a></td>
                                <td style="vertical-align: top;" class="nowrap text-center"><asp:LinkButton ID="btnDeleteAgreementItem" runat="server" OnCommand="btnDeleteAgreementItem_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="x" OnClientClick="javascript: return confirm('Delete Item?');" ToolTip="Delete" ForeColor="Red" CssClass="no-line" /></td>
                            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>

<script>
    var b_visible_item_form = (("<%= IsVisibleItemForm %>").toLowerCase() == "true");

    if (b_visible_item_form == true) {
        document.getElementById("divAgreementItemForm").className = "";
    }

    function display_agreement_item_form() {
        display_by_class('divAgreementItemForm');
        document.getElementById("<%= txtDateStart.ClientID %>").focus();
    }

    function display_agreement_item_terminal(s_link_id) {
        var s_div_id = new String(s_link_id);

        s_div_id = s_div_id.replace("lnkAgreementItemTerminal", "divAgreementItemTerminal");

        display_by_class(s_div_id);

        var o_link = document.getElementById(s_link_id);
        var o_div = document.getElementById(s_div_id);

        if (o_div.className == "no-display") {
            o_link.innerHTML = "Display";
        }
        else {
            o_link.innerHTML = "Hide";
        }
    }

    function validate_supplier_group_list(source, args) {
        var b_valid = false;
        var s_error = "";

        try {
            var o_check_list = document.getElementById("<%= lstSupplierGroup.ClientID %>").getElementsByTagName("input");

            for (var i = 0; i < o_check_list.length; i++) {
                if (o_check_list[i].checked) {
                    b_valid = true;
                    return;
                }
            }
        }
        catch (ex) {
            s_error = ex.message;
        }

        if (s_error == "" && b_valid == false) {
            try {
                document.getElementById("<%= chkSupplierGroup.ClientID %>").focus();
            }
            catch (ex) {
                s_error = ex.message;
            }
        }

        if (s_error != "") { return; }

        args.IsValid = b_valid;
    }

    function validate_supplier_list(source, args) {
        var b_valid = false;
        var s_error = "";

        try {
            var o_check_list = document.getElementById('<%= lstSupplier.ClientID %>').getElementsByTagName("input");

            for (var i = 0; i < o_check_list.length; i++) {
                if (o_check_list[i].checked) {
                    b_valid = true;
                    return;
                }
            }
        }
        catch (ex) {
            s_error = ex.message;
        }

        if (s_error == "" && b_valid == false) {
            try {
                document.getElementById("<%= chkSupplier.ClientID %>").focus();
            }
            catch (ex) {
                s_error = ex.message;
            }
        }

        if (s_error != "") { return; }

        args.IsValid = b_valid;
    }

    function validate_terminal_list(source, args) {
        var b_valid = false;
        var s_error = "";

        try {
            var o_check_list = document.getElementById('<%= lstTerminal.ClientID %>').getElementsByTagName("input");

            for (var i = 0; i < o_check_list.length; i++) {
                if (o_check_list[i].checked) {
                    b_valid = true;
                    return;
                }
            }
        }
        catch (ex) {
            s_error = ex.message;
        }

        if (s_error == "" && b_valid == false) {
            try {
                document.getElementById("<%= chkTerminal.ClientID %>").focus();
            }
            catch (ex) {
                s_error = ex.message;
            }
        }

        if (s_error != "") { return; }

        args.IsValid = b_valid;
    }
</script>
