<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgreementDiscount.ascx.cs" Inherits="MatchBox.AgreementDiscount" %>

<table>
    <tr>
        <td class="nowrap" style="padding: 0px 10px;"><b>Company</b> : <asp:Label ID="lblCompanyName" runat="server" /></td>
        <td class="nowrap" style="padding: 0px 10px;">&nbsp;</td>
        <td class="nowrap" style="padding: 0px 10px;"><b>Credit</b> : <asp:Label ID="lblCreditName" runat="server" /></td>
    </tr>
</table>

<hr style="margin: 10px;" />

<div style="margin: 10px;">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<div id="divAgreementDiscountForm" runat="server" visible="false">
    <h3 style="margin: 10px;">New Discount Item</h3>

    <div style="margin: 10px;">
        <h3 style="font-weight: normal;"><u>Clearing Item Details</u></h3>
    </div>

    <table class="table-list" style="margin: 5px;">
        <tr>
            <td><b>Start Date</b>: <asp:Label ID="lblDateStart" runat="server" /></td>
            <td>&nbsp;</td>
            <td><b>End Date</b>: <asp:Label ID="lblDateEnd" runat="server" /></td>
            <td>&nbsp;</td>
            <td><b>Payments From</b>: <asp:Label ID="lblPaymentsFrom" runat="server" /></td>
            <td>&nbsp;</td>
            <td><b>Payments To</b>: <asp:Label ID="lblPaymentsTo" runat="server" /></td>
        </tr>
        <tr>
            <td><b>Card</b>: <asp:Label ID="lblCard" runat="server" /></td>
            <td>&nbsp;</td>
            <td><b>Operation Type</b>: <asp:Label ID="lblOperationType" runat="server" /></td>
            <td>&nbsp;</td>
            <td><b>Commission Local</b>: <asp:Label ID="lblCommissionLocal" runat="server" /></td>
            <td>&nbsp;</td>
            <td><b>Commission Abroad</b>: <asp:Label ID="lblCommissionAbroad" runat="server" /></td>
        </tr>
    </table>

    <div style="margin: 10px;">
        <h3 style="font-weight: normal;"><u>Discount Item Details</u></h3>
    </div>

    <div style="margin: 0px 10px;">
        <asp:CustomValidator ID="cvDateRange" runat="server" ValidationGroup="AgreementDiscount" OnServerValidate="cvDateRange_ServerValidate" Display="Dynamic" CssClass="error" />
    </div>

    <%--Discount--%>
    <div id="divDiscount_Change" runat="server" class="float-left div-form-field">
        <label for="<%= ddlDiscount.ClientID %>" class="block">Discount</label>
        <asp:DropDownList ID="ddlDiscount" runat="server">
            <asp:ListItem Text="" Value="0" />
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvDiscount" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="ddlDiscount" InitialValue="0" ErrorMessage="Select 'Discount'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--DateStart--%>
    <div id="divDateStart_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtDateStart.ClientID %>" class="block">Start Date</label>
        <asp:TextBox ID="txtDateStart" runat="server" MaxLength="10" placeholder="dd/mm/yyyy" />
        <asp:RequiredFieldValidator ID="rfvDateStart" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="txtDateStart" ErrorMessage="Enter 'Start Date'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--DateEnd--%>
    <div id="divDateEnd_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtDateEnd.ClientID %>" class="block">End Date</label>
        <asp:TextBox ID="txtDateEnd" runat="server" MaxLength="10" placeholder="dd/mm/yyyy" />
        <asp:RequiredFieldValidator ID="rfvDateEnd" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="txtDateEnd" ErrorMessage="Enter 'End Date'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <div class="clear-both"></div>

    <%--BaseDate--%>
    <div id="divBaseDate_Change" runat="server" class="float-left div-form-field">
        <label for="<%= ddlBaseDate.ClientID %>" class="block">Base Date</label>
        <asp:DropDownList ID="ddlBaseDate" runat="server">
            <asp:ListItem Text="" Value="0" />
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvBaseDate" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="ddlBaseDate" InitialValue="0" ErrorMessage="Select 'Base Date'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--Payments--%>
    <div id="divPayments_Change" runat="server" class="float-left div-form-field">
        Payments
        <br />
        <label for="<%= txtPaymentsFrom.ClientID %>">From</label>
        <asp:TextBox ID="txtPaymentsFrom" runat="server" MaxLength="2" Width="93px" />
        &nbsp;
        <label for="<%= txtPaymentsTo.ClientID %>">To</label>
        <asp:TextBox ID="txtPaymentsTo" runat="server" MaxLength="2" Width="93px" />

        <asp:RequiredFieldValidator ID="rfvPaymentsFrom" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="txtPaymentsFrom" ErrorMessage="Enter 'Payments From'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:CompareValidator ID="cvPaymentsFrom" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="txtPaymentsFrom" Operator="DataTypeCheck" Type="Integer" ErrorMessage="'Payments From' must be a whole number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:RangeValidator id="rvPaymentsFrom" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="txtPaymentsFrom" MinimumValue="1" MaximumValue="99" Type="Integer" Text="'Payments From' must be greater than 0." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />

        <asp:RequiredFieldValidator ID="rfvPaymentsTo" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="txtPaymentsTo" ErrorMessage="Enter 'Payments To'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:CompareValidator ID="cvPaymentsTo" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="txtPaymentsTo" Operator="DataTypeCheck" Type="Integer" ErrorMessage="'Payments To' must be a whole number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:RangeValidator id="rvPaymentsTo" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="txtPaymentsTo" MinimumValue="1" MaximumValue="99" Type="Integer" Text="'Payments To' must be greater than 0." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />

        <asp:CustomValidator ID="cvPaymentsRange" runat="server" ValidationGroup="AgreementDiscount" OnServerValidate="cvPaymentsRange_ServerValidate" Display="Dynamic" CssClass="error block" />
    </div>

    <%--Commission--%>
    <div id="divCommission_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtCommission.ClientID %>" class="block">Commission</label>
        <asp:TextBox ID="txtCommission" runat="server" MaxLength="5" Width="250px" /> %
        <asp:RequiredFieldValidator ID="rfvCommission" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="txtCommission" ErrorMessage="Enter 'Commission'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:CompareValidator ID="cvCommission" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="txtCommission" Operator="DataTypeCheck" Type="Double" ErrorMessage="'Commission' must be a number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <div class="clear-both"></div>

    <%--DiscountPeriodType--%>
    <div id="divDiscountPeriodType_Change" runat="server" class="float-left div-form-field">
        <label for="<%= ddlDiscountPeriodType.ClientID %>" class="block">Discount Period Type</label>
        <asp:DropDownList ID="ddlDiscountPeriodType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDiscountPeriodType_SelectedIndexChanged">
            <asp:ListItem Text="" Value="0" />
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvDiscountPeriodType" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="ddlDiscountPeriodType" InitialValue="0" ErrorMessage="Select 'Discount Period Type'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--WeekStart--%>
    <div id="divWeekStart_Change" runat="server" visible="false" class="float-left div-form-field">
        <div>
            <label for="<%= ddlWeekStart.ClientID %>">Week Start</label>
            &nbsp;
            ( End: <asp:Label ID="lblWeekEnd" runat="server" Text="---" /> )
        </div>
        <asp:DropDownList ID="ddlWeekStart" runat="server" onchange="javascript: ddl_weekday_change();">
                <asp:ListItem Text="" Value="-1" />
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvWeekStart" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="ddlWeekStart" InitialValue="-1" ErrorMessage="Select 'Week Start'." Display="Dynamic" SetFocusOnError="true" CssClass="error" />
    </div>

    <%--PaymentAfterDays--%>
    <div id="divPaymentAfterDays_Change" runat="server" visible="false" class="float-left div-form-field">
        <label for="<%= txtPaymentAfterDays.ClientID %>" class="block">Payment After (+) Days</label>
        <asp:TextBox ID="txtPaymentAfterDays" runat="server" MaxLength="2" />
        <asp:RequiredFieldValidator ID="rfvPaymentAfterDays" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="txtPaymentAfterDays" ErrorMessage="Enter 'Payment After Days'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:CompareValidator ID="cvPaymentAfterDays" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="txtPaymentAfterDays" Operator="DataTypeCheck" Type="Integer" ErrorMessage="'Payment Days' must be a whole number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:RangeValidator id="rvPaymentAfterDays" runat="server" ValidationGroup="AgreementDiscount" ControlToValidate="txtPaymentAfterDays" MinimumValue="1" MaximumValue="99" Type="Integer" Text="'Payment Days' must be greater than 0." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <div id="divDiscountPeriodOneThirdMonthly_Change" runat="server" visible="false" class="float-left">
        <asp:Repeater ID="repOneThirdMonthly" runat="server">
            <HeaderTemplate>
                <table style="margin: 5px;">
                    <tr>
                        <td>Month Day From</td>
                        <td>Month Day To</td>
                        <td>Payment After (+) Days</td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtDayFrom" runat="server" MaxLength="2" Text='<%# DataBinder.Eval(Container.DataItem, "DayFrom") %>' />
                            <asp:RangeValidator id="rvDayFrom" runat="server" ValidationGroup="AgreementPayment" ControlToValidate="txtDayFrom" MinimumValue="1" MaximumValue="31" Type="Integer" Text="<br />not valid day" Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDayTo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DayTo") %>' />
                            <asp:RangeValidator id="rvDayTo" runat="server" ValidationGroup="AgreementPayment" ControlToValidate="txtDayTo" MinimumValue="1" MaximumValue="31" Type="Integer" Text="<br />not valid day" Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPaymentAfterDays" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DayPayment") %>' />
                            <asp:RangeValidator id="rvDayPayment" runat="server" ValidationGroup="AgreementPayment" ControlToValidate="txtPaymentAfterDays" MinimumValue="1" MaximumValue="31" Type="Integer" Text="<br />not valid days count" Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
                        </td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div class="clear-both"></div>

    <asp:Repeater ID="repAgreementItemTerminal" runat="server">
        <HeaderTemplate>
            <table class="table-list" style="margin: 10px; border: 1px solid #cccccc;">
                <tr class="bg-gray">
                    <th><input type="checkbox" onclick="javascript: select_terminal_all(this.checked);" /></th>
                    <%--
                    <th>SupplierTerminalID</th>
                    <th>AgreementPeriodID</th>
                    <th>SupplierGroupID</th>
                    <th>SupplierID</th>
                    <th>TerminalID</th>
                    --%>
                    <th>Group</th>
                    <th>Supplier</th>
                    <th>Terminal</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td>
                        <input type="checkbox" id="chkIsSelected" runat="server" data-select-terminal="" />
                        <%--<asp:CheckBox ID="chkIsSelected" runat="server" data-select-terminal="" />--%>
                        <asp:HiddenField ID="hidAgreementItemTerminalID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                    </td>
                    <%--
                    <td><%# DataBinder.Eval(Container.DataItem, "SupplierTerminalID") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "AgreementPeriodID") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "SupplierGroupID") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "SupplierID") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "TerminalID") %></td>
                    --%>
                    <td><%# DataBinder.Eval(Container.DataItem, "SupplierGroup") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Supplier") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Terminal") %></td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    
    <div class="div-form-command">
        <br />
        <asp:Button ID="btnSave" runat="server" ValidationGroup="AgreementDiscount" Text="Save" OnClick="btnSave_Click" />
        &nbsp;
        <a id="lnkClose" runat="server">Close</a>
    </div>
    
    <hr style="margin: 10px;" />
</div>

<asp:Repeater ID="repAgreementDiscount" runat="server" OnItemDataBound="repAgreementDiscount_ItemDataBound">
    <HeaderTemplate>
        <table class="table-sub" style="margin: 10px;">
            <tr>
                <th rowspan="2">Discount</th>
                <th rowspan="2">Start Date</th>
                <th rowspan="2">End Date</th>
                <th rowspan="2">Base Date</th>
                <th rowspan="2">Period Type</th>
                <th rowspan="2">Period Settings</th>
                <th colspan="2">Payments</th>
                <th rowspan="2">Commission</th>
                <th rowspan="2">Group / Supplier / Terminal</th>
                <th rowspan="2">x</th>
            </tr>
            <tr>
                <th>From</th>
                <th>To</th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
            <tr>
                <td><%# DataBinder.Eval(Container.DataItem, "DiscountName") %></td>
                <td><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateStart")) %></td>
                <td><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateEnd")) %></td>
                <td><%# DataBinder.Eval(Container.DataItem, "BaseDateName") %></td>
                <td><%# DataBinder.Eval(Container.DataItem, "DiscountPeriodTypeName") %></td>
                <td class="nowrap">
                    <asp:Label ID="lblPeriodSettings" runat="server" Visible="false" />
                    <asp:Repeater ID="repAgreementDiscountSettings" runat="server" Visible="false">
                        <HeaderTemplate>
                            <table class="table-list">
                        </HeaderTemplate>
                        <ItemTemplate>
                                <tr>
                                    <td><%# DataBinder.Eval(Container.DataItem, "DayFrom") %></td>
                                    <td><%# DataBinder.Eval(Container.DataItem, "DayTo") %></td>
                                    <td>+<%# DataBinder.Eval(Container.DataItem, "PaymentAfterDays") %></td>
                                </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
                <td><%# DataBinder.Eval(Container.DataItem, "PaymentsFrom") %></td>
                <td><%# DataBinder.Eval(Container.DataItem, "PaymentsTo") %></td>
                <td><%# DataBinder.Eval(Container.DataItem, "Commission") %>%</td>
                <td>
                    <asp:Repeater ID="repAgreementDiscountTerminal" runat="server">
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
                </td>
                <td class="nowrap text-center"><asp:LinkButton ID="btnDeleteAgreementDiscount" runat="server" OnCommand="btnDeleteAgreementDiscount_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="x" OnClientClick="javascript: return confirm('Delete Item?');" ToolTip="Delete" ForeColor="Red" CssClass="no-line" /></td>
            </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>

<script>
    function select_terminal_all(b_checked) {
        var chk_select_terminal_all = document.querySelectorAll("input[data-select-terminal]");

        for (var i = 0; i < chk_select_terminal_all.length; i++) {
            chk_select_terminal_all[i].checked = b_checked;
        }
    }

    function ddl_weekday_change() {
        var ddl_week_start = document.getElementById("<%= ddlWeekStart.ClientID %>");
        var lbl_week_end = document.getElementById("<%= lblWeekEnd.ClientID %>");

        var n_index = ddl_week_start.selectedIndex;

        if (n_index == 0) {
            lbl_week_end.innerHTML = "---";
        }
        else if (n_index == 1) {
            lbl_week_end.innerHTML = ddl_week_start.options[7].text;
        }
        else {
            lbl_week_end.innerHTML = ddl_week_start.options[n_index - 1].text;
        }
    }
</script>
