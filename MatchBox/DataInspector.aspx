<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DataInspector.aspx.cs" Inherits="MatchBox.DataInspector" %>
<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <table style="width: 100%;">
        <tr class="nowrap">
            <td>
                <h1 id="hedMain" runat="server" class="float-left">Data Inspector</h1>
            </td>
            <td class="nowrap">&nbsp; &nbsp; - &nbsp; &nbsp;</td>
            <td><a href="javascript: void(0);" onclick="javascript: display_section('section-search', 'block');">Show search</a></td>
            <td class="nowrap">&nbsp; &nbsp; - &nbsp; &nbsp;</td>
            <td><a href="DataSummary.aspx">Data Summary</a></td>
            <td>&nbsp;|&nbsp;</td>
            <td><a href="DataFieldPriority.aspx">Columns Settings</a></td>
            <td class="nowrap">&nbsp; &nbsp; - &nbsp; &nbsp;</td>
            <td>
                <asp:Label ID="lblModeInfo" runat="server" Font-Bold="true" />
                :
                <asp:Label ID="lblNote" runat="server" />
            </td>
            <td style="width: 100%;">&nbsp;</td>
        </tr>
    </table>

    <div id="divMessage" runat="server" enableviewstate="false" visible="false" class="div-message">
        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
        <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
    </div>

    <section id="secSearch" runat="server" class="section-search" style="padding: 10px; font-size: 11pt;">
        <div class="div-form-command">
            <asp:Button ID="btnSearchTop" runat="server" OnClick="Search_Click" OnClientClick="javascript: show_wait();" Text="Search" />
            &nbsp; &nbsp;
           <%-- Page Size
            <asp:DropDownList ID="ddlPageSize" runat="server">
                <asp:ListItem Value="100" Text="100" />
                <asp:ListItem Value="250" Text="250" />
                <asp:ListItem Value="500" Text="500" />
                <asp:ListItem Value="1000" Text="1000" />
                <asp:ListItem Value="2500" Text="2500" />
                <asp:ListItem Value="5000" Text="5000" />
                <asp:ListItem Value="10000" Text="10000" />
            </asp:DropDownList>--%>
            &nbsp; &nbsp;
            <a href="DataInspector.aspx">Reset</a>
            &nbsp;
            <a href="javascript: void(0);" onclick="javascript: display_section('section-search', 'none');">Close</a>
            &nbsp; &nbsp;
            <span id="spnSearchTop" class="message" style="display: none;">Please wait..</span>
        </div>

        <hr />

        <div class="clear-both"></div>

        <%--Inside Sort--%>
        <div class="float-left div-form-item">
            <b>Inside Sort Order</b>
            <br />
            <asp:DropDownList ID="ddlInsideSort_1" runat="server" />
            <asp:DropDownList ID="ddlInsideOrder_1" runat="server">
                <asp:ListItem Value="ASC" Text="Asc" />
                <asp:ListItem Value="DESC" Text="Desc" />
            </asp:DropDownList>
            &nbsp;
            <asp:DropDownList ID="ddlInsideSort_2" runat="server" />
            <asp:DropDownList ID="ddlInsideOrder_2" runat="server">
                <asp:ListItem Value="ASC" Text="Asc" />
                <asp:ListItem Value="DESC" Text="Desc" />
            </asp:DropDownList>
            &nbsp;
            <asp:DropDownList ID="ddlInsideSort_3" runat="server" />
            <asp:DropDownList ID="ddlInsideOrder_3" runat="server">
                <asp:ListItem Value="ASC" Text="Asc" />
                <asp:ListItem Value="DESC" Text="Desc" />
            </asp:DropDownList>
        </div>

        <%--Outside Sort--%>
        <div class="float-left div-form-item">
            <b>Outside Sort Order</b> :
            <br />
            <asp:DropDownList ID="ddlOutsideSort_1" runat="server" />
            <asp:DropDownList ID="ddlOutsideOrder_1" runat="server">
                <asp:ListItem Value="ASC" Text="Asc" />
                <asp:ListItem Value="DESC" Text="Desc" />
            </asp:DropDownList>
            &nbsp;
            <asp:DropDownList ID="ddlOutsideSort_2" runat="server" />
            <asp:DropDownList ID="ddlOutsideOrder_2" runat="server">
                <asp:ListItem Value="ASC" Text="Asc" />
                <asp:ListItem Value="DESC" Text="Desc" />
            </asp:DropDownList>
            &nbsp;
            <asp:DropDownList ID="ddlOutsideSort_3" runat="server" />
            <asp:DropDownList ID="ddlOutsideOrder_3" runat="server">
                <asp:ListItem Value="ASC" Text="Asc" />
                <asp:ListItem Value="DESC" Text="Desc" />
            </asp:DropDownList>
        </div>

        <div class="clear-both"></div>

        <hr />

        <%--Transactions--%>
        <div id="divTransactions" runat="server" class="float-left div-form-field">
            <label for="<%= ddlTransactions.ClientID %>" class="block bold">Transactions</label>
            <asp:DropDownList ID="ddlTransactions" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlTransactions_SelectedIndexChanged">
                <asp:ListItem Value="all" Text="All Transactions" />
                <asp:ListItem Value="matching" Text="Matching Transactions" />
                <asp:ListItem Value="not-matching" Text="Not Matching Transactions" />
            </asp:DropDownList>
        </div>

        <%--OperationType--%>
        <div id="divOperationType" runat="server" class="float-left div-form-field">
            <label for="<%= cblOperationType.ClientID %>" class="block bold">Operation Type</label>
            <div class="div-form-item-list" style="max-height: 60px; padding: 0px;">
                <asp:CheckBoxList ID="cblOperationType" runat="server" />
            </div>
        </div>

        <%--TransactionType--%>
        <div id="divTransactionType" runat="server" class="float-left div-form-field">
            <b>Transaction Type</b>
            <br />
            <asp:CheckBox ID="chkIsSplitted" runat="server" Text="Splitted" />
            <br />
            <asp:CheckBox ID="chkIsBalance" runat="server" Text="Balance" />
            <br />
            <asp:CheckBox ID="chkIsAbroad" runat="server" Text="Abroad" />
        </div>

        <%--Status--%>
        <asp:UpdatePanel ID="upStatus" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divStatus" runat="server" visible="false" class="float-left div-form-field">
                    <label for="<%= cblStatus.ClientID %>" class="block bold">Status</label>
                    <asp:CheckBoxList ID="cblStatus" runat="server" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlTransactions" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>

        <%--MatchingAction--%>
        <asp:UpdatePanel ID="upMatchingAction" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divMatchingAction" runat="server" visible="false" class="float-left div-form-field">
                    <label for="<%= cblMatchingAction.ClientID %>" class="block bold">Matching Action</label>
                    <asp:CheckBoxList ID="cblMatchingAction" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblMatchingAction_SelectedIndexChanged" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlTransactions" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>

        <%--MatchingType--%>
        <asp:UpdatePanel ID="upMatchingType" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divMatchingType" runat="server" visible="false" class="float-left div-form-field">
                    <label for="<%= cblMatchingType.ClientID %>" class="block bold">Matching Type</label>
                    <asp:CheckBoxList ID="cblMatchingType" runat="server" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlTransactions" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>

        <div class="clear-both"></div>

        <%--Template--%>
        <div id="divTemplate" runat="server" class="float-left div-form-item">
            <label for="<%= cblTemplate.ClientID %>" class="block bold">Template</label>
            <div class="div-form-item-list font-mono nowrap">
                <asp:CheckBoxList ID="cblTemplate" runat="server" />
            </div>
        </div>

        <%--DataFile--%>
        <div id="divDataFile" runat="server" class="float-left div-form-item">
            <label for="<%= cblDataFile.ClientID %>" class="block bold">Data File</label>
            <div class="div-form-item-list font-mono nowrap">
                <asp:CheckBoxList ID="cblDataFile" runat="server" />
            </div>
        </div>

        <%--Strategy--%>
        <asp:UpdatePanel ID="upStrategy" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divStrategy" runat="server" visible="false" class="float-left div-form-item">
                    <label for="<%= cblStrategy.ClientID %>" class="block bold">Strategy</label>
                    <div class="div-form-item-list font-mono nowrap">
                        <asp:CheckBoxList ID="cblStrategy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblStrategy_SelectedIndexChanged" />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlTransactions" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="cblMatchingAction" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>

        <%--Matching--%>
        <asp:UpdatePanel ID="upMatching" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divMatching" runat="server" visible="false" class="float-left div-form-item">
                    <label for="<%= cblMatching.ClientID %>" class="block bold">Matching</label>
                    <div class="div-form-item-list font-mono nowrap">
                        <asp:CheckBoxList ID="cblMatching" runat="server" />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlTransactions" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="cblMatchingAction" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="cblStrategy" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>

        <div class="clear-both"></div>

        <%--TransactionDate--%>
        <div id="divTransactionDate" runat="server" class="float-left div-form-field-long">
            <b>Transaction Date</b>
            &nbsp;
            <asp:CheckBox ID="chk_txtTransactionDateOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            &nbsp;
            <asp:CheckBox ID="chkExcludeTransactionDate" runat="server" Text="Exclude" />
            &nbsp;
            <asp:CheckBox ID="chkEmptyTransactionDate" runat="server" Text="Include Empty Dates" />
            <br />
            <asp:TextBox ID="txtTransactionDate" runat="server" Width="98%" />
            <asp:TextBox ID="txtTransactionDateOutside" runat="server" Width="98%" CssClass="no-display" />
            <asp:Label ID="lblTransactionDateError" runat="server" EnableViewState="false" CssClass="error block" />
        </div>

        <%--TransmissionDate--%>
        <div id="divTransmissionDate" runat="server" class="float-left div-form-field-long">
            <b>Transmission Date</b>
            &nbsp;
            <asp:CheckBox ID="chk_txtTransmissionDateOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            &nbsp;
            <asp:CheckBox ID="chkExcludeTransmissionDate" runat="server" Text="Exclude" />
            &nbsp;
            <asp:CheckBox ID="chkEmptyTransmissionDate" runat="server" Text="Include Empty Dates" />
            <br />
            <asp:TextBox ID="txtTransmissionDate" runat="server" Width="98%" />
            <asp:TextBox ID="txtTransmissionDateOutside" runat="server" Width="98%" CssClass="no-display" />
            <asp:Label ID="lblTransmissionDateError" runat="server" EnableViewState="false" CssClass="error block" />
        </div>

        <%--PaymentDate--%>
        <div id="divPaymentDate" runat="server" class="float-left div-form-field-long">
            <b>Payment Date</b>
            &nbsp;
            <asp:CheckBox ID="chk_txtPaymentDateOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            &nbsp;
            <asp:CheckBox ID="chkExcludePaymentDate" runat="server" Text="Exclude" />
            &nbsp;
            <asp:CheckBox ID="chkEmptyPaymentDate" runat="server" Text="Include Empty Dates" />
            <br />
            <asp:TextBox ID="txtPaymentDate" runat="server" Width="98%" />
            <asp:TextBox ID="txtPaymentDateOutside" runat="server" Width="98%" CssClass="no-display" />
            <asp:Label ID="lblPaymentDateError" runat="server" EnableViewState="false" CssClass="error block" />
        </div>

        <%--MatchingDate--%>
        <asp:UpdatePanel ID="upMatchingDate" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divMatchingDate" runat="server" visible="false" class="float-left div-form-field-long">
                    <b>Matching Date</b>
                    &nbsp;
                    <asp:CheckBox ID="chk_txtMatchingDateOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
                    &nbsp;
                    <asp:CheckBox ID="chkExcludeMatchingDate" runat="server" Text="Exclude" />
                    <br />
                    <asp:TextBox ID="txtMatchingDate" runat="server" Width="98%" />
                    <asp:TextBox ID="txtMatchingDateOutside" runat="server" Width="98%" CssClass="no-display" />
                    <asp:Label ID="lblMatchingDateError" runat="server" EnableViewState="false" CssClass="error block" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlTransactions" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>

        <div class="clear-both"></div>

        <%--CardPrefix--%>
        <div id="divCardPrefix" runat="server" class="float-left div-form-field">
            <label for="<%= txtCardPrefix.ClientID %>" class="bold">Card Prefix</label>
            <asp:CheckBox ID="chk_txtCardPrefixOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeCardPrefix" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtCardPrefix" runat="server" CssClass="block" />
            <asp:TextBox ID="txtCardPrefixOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblCardPrefixError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--CardNumber--%>
        <div id="divCardNumber" runat="server" class="float-left div-form-field">
            <label for="<%= txtCardNumber.ClientID %>" class="bold">Card Number</label>
            <asp:CheckBox ID="chk_txtCardNumberOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeCardNumber" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtCardNumber" runat="server" CssClass="block" />
            <asp:TextBox ID="txtCardNumberOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblCardNumberError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--TransmissionNumber--%>
        <div id="divTransmissionNumber" runat="server" class="float-left div-form-field">
            <label for="<%= txtTransmissionNumber.ClientID %>" class="bold">Transmission <u>N</u></label>
            <asp:CheckBox ID="chk_txtTransmissionNumberOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeTransmissionNumber" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtTransmissionNumber" runat="server" CssClass="block" />
            <asp:TextBox ID="txtTransmissionNumberOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblTransmissionNumberError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--VoucherNumber--%>
        <div id="divVoucherNumber" runat="server" class="float-left div-form-field">
            <label for="<%= txtVoucherNumber.ClientID %>" class="bold">Voucher <u>N</u></label>
            <asp:CheckBox ID="chk_txtVoucherNumberOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeVoucherNumber" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtVoucherNumber" runat="server" CssClass="block" />
            <asp:TextBox ID="txtVoucherNumberOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblVoucherNumberError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--ConfirmationNumber--%>
        <div id="divConfirmationNumber" runat="server" class="float-left div-form-field">
            <label for="<%= txtConfirmationNumber.ClientID %>" class="bold">Confirmation <u>N</u></label>
            <asp:CheckBox ID="chk_txtConfirmationNumberOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeConfirmationNumber" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtConfirmationNumber" runat="server" CssClass="block" />
            <asp:TextBox ID="txtConfirmationNumberOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblConfirmationNumberError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <div class="clear-both"></div>

        <%--PaymentsCount--%>
        <div id="divPaymentsCount" runat="server" class="float-left div-form-field">
            <label for="<%= txtPaymentsCount.ClientID %>" class="bold">Payments Count</label>
            <asp:CheckBox ID="chk_txtPaymentsCountOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludePaymentsCount" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtPaymentsCount" runat="server" CssClass="block" />
            <asp:TextBox ID="txtPaymentsCountOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblPaymentsCountError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--DutyPaymentNumber--%>
        <div id="divDutyPaymentNumber" runat="server" class="float-left div-form-field">
            <label for="<%= txtDutyPaymentNumber.ClientID %>" class="bold">Duty Payment <u>N</u></label>
            <asp:CheckBox ID="chk_txtDutyPaymentNumberOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeDutyPaymentNumber" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtDutyPaymentNumber" runat="server" CssClass="block" />
            <asp:TextBox ID="txtDutyPaymentNumberOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblDutyPaymentNumberError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--TransactionGrossAmount--%>
        <div id="divTransactionGrossAmount" runat="server" class="float-left div-form-field">
            <label for="<%= txtTransactionGrossAmount.ClientID %>" class="bold" title="Transaction Gross Amount">Gross Amount</label>
            <asp:CheckBox ID="chk_txtTransactionGrossAmountOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeTransactionGrossAmount" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtTransactionGrossAmount" runat="server" CssClass="block" />
            <asp:TextBox ID="txtTransactionGrossAmountOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblTransactionGrossAmountError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--DutyPaymentAmount--%>
        <div id="divDutyPaymentAmount" runat="server" class="float-left div-form-field">
            <label for="<%= txtDutyPaymentAmount.ClientID %>" class="bold">Duty Amount</label>
            <asp:CheckBox ID="chk_txtDutyPaymentAmountOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeDutyPaymentAmount" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtDutyPaymentAmount" runat="server" CssClass="block" />
            <asp:TextBox ID="txtDutyPaymentAmountOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblDutyPaymentAmountError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--RemainingPaymentsAmount--%>
        <div id="divRemainingPaymentsAmount" runat="server" class="float-left div-form-field">
            <label for="<%= txtRemainingPaymentsAmount.ClientID %>" class="bold" title="Remaining Payments Amount">Remaining <u>Am</u></label>
            <asp:CheckBox ID="chk_txtRemainingPaymentsAmountOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeRemainingPaymentsAmount" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtRemainingPaymentsAmount" runat="server" CssClass="block" />
            <asp:TextBox ID="txtRemainingPaymentsAmountOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblRemainingPaymentsAmountError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <div class="clear-both"></div>

        <%--CompanyNumber--%>
        <div id="divCompanyNumber" runat="server" class="float-left div-form-field">
            <label for="<%= txtCompanyNumber.ClientID %>" class="bold">Company <u>N</u></label>
            <asp:CheckBox ID="chk_txtCompanyNumberOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeCompanyNumber" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtCompanyNumber" runat="server" CssClass="block" />
            <asp:TextBox ID="txtCompanyNumberOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblCompanyNumberError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--NetworkNumber--%>
        <div id="divNetworkNumber" runat="server" class="float-left div-form-field">
            <label for="<%= txtNetworkNumber.ClientID %>" class="bold">Network <u>N</u></label>
            <asp:CheckBox ID="chk_txtNetworkNumberOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeNetworkNumber" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtNetworkNumber" runat="server" CssClass="block" />
            <asp:TextBox ID="txtNetworkNumberOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblNetworkNumberError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--BranchNumber--%>
        <div id="divBranchNumber" runat="server" class="float-left div-form-field">
            <label for="<%= txtBranchNumber.ClientID %>" class="bold">Branch <u>N</u></label>
            <asp:CheckBox ID="chk_txtBranchNumberOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeBranchNumber" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtBranchNumber" runat="server" CssClass="block" />
            <asp:TextBox ID="txtBranchNumberOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblBranchNumberError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--CashBoxNumber--%>
        <div id="divCashBoxNumber" runat="server" class="float-left div-form-field">
            <label for="<%= txtCashBoxNumber.ClientID %>" class="bold">Cashbox <u>N</u></label>
            <asp:CheckBox ID="chk_txtCashBoxNumberOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeCashBoxNumber" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtCashBoxNumber" runat="server" CssClass="block" />
            <asp:TextBox ID="txtCashBoxNumberOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblCashBoxNumberError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--SupplierGroupNumber--%>
        <div id="divSupplierGroupNumber" runat="server" class="float-left div-form-field">
            <label for="<%= txtSupplierGroupNumber.ClientID %>" class="bold">Group <u>N</u></label>
            <asp:CheckBox ID="chk_txtSupplierGroupNumberOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeSupplierGroupNumber" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtSupplierGroupNumber" runat="server" CssClass="block" />
            <asp:TextBox ID="txtSupplierGroupNumberOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblSupplierGroupNumberError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--SupplierNumber--%>
        <div id="divSupplierNumber" runat="server" class="float-left div-form-field">
            <label for="<%= txtSupplierNumber.ClientID %>" class="bold">Supplier <u>N</u></label>
            <asp:CheckBox ID="chk_txtSupplierNumberOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeSupplierNumber" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtSupplierNumber" runat="server" CssClass="block" />
            <asp:TextBox ID="txtSupplierNumberOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblSupplierNumberError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--TerminalNumber--%>
        <div id="divTerminalNumber" runat="server" class="float-left div-form-field">
            <label for="<%= txtTerminalNumber.ClientID %>" class="bold">Terminal <u>N</u></label>
            <asp:CheckBox ID="chk_txtTerminalNumberOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeTerminalNumber" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtTerminalNumber" runat="server" CssClass="block" />
            <asp:TextBox ID="txtTerminalNumberOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblTerminalNumberError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--Comment--%>
        <div id="divComment" runat="server" class="float-left div-form-field">
            <label for="<%= txtComment.ClientID %>" class="bold">Comment</label>
            <asp:CheckBox ID="chk_txtCommentOutside" runat="server" Checked="true" Text="Common" onclick="javascript: display_outside(this.id, this.checked);" />
            <asp:CheckBox ID="chkExcludeComment" runat="server" Text="Exclude" />
            <asp:TextBox ID="txtComment" runat="server" CssClass="block" />
            <asp:TextBox ID="txtCommentOutside" runat="server" CssClass="no-display" />
            <asp:Label ID="lblCommentError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <%--ID--%>
        <div id="divID" runat="server" class="float-left div-form-field">
            <label for="<%= txtIDInside.ClientID %>" class="bold">ID</label>
            <asp:CheckBox ID="chkExcludeID" runat="server" Text="Exclude" />
            <br />
            <asp:TextBox ID="txtIDInside" runat="server" MaxLength="36" Width="45%" placeholder="Inside" />
            <asp:TextBox ID="txtIDOutside" runat="server" MaxLength="36" Width="45%" placeholder="Outside" />
            <asp:Label ID="lblIDError" runat="server" EnableViewState="false" CssClass="error" />
        </div>

        <div class="clear-both"></div>

        <%--Company--%>
        <div id="divCompany" runat="server" class="float-left div-form-field">

            <asp:UpdatePanel ID="upSelectBy" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Block">
                <ContentTemplate>
                    <b>Company</b> by
                    <asp:RadioButtonList ID="rblSelectBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblSelectBy_SelectedIndexChanged" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Value="cashbox" Text="cashbox" Selected="True" />
                        <asp:ListItem Value="terminal" Text="terminal" />
                    </asp:RadioButtonList>

                    <asp:Repeater ID="repCompany" runat="server">
                        <HeaderTemplate>
                            <table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="vertical-align: top">
                                    <asp:CheckBox ID="chkCompany" runat="server" AutoPostBack="true" OnCheckedChanged="chkCompany_CheckedChanged" />
                                    <asp:HiddenField ID="hidCompanyID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                                </td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblCompany" runat="server" AssociatedControlID="chkCompany" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>' />

                                    <asp:UpdatePanel ID="upNetwork" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Block">
                                        <ContentTemplate>
                                            <asp:Repeater ID="repNetwork" runat="server">
                                                <HeaderTemplate>
                                                    <table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="vertical-align: top">
                                                            <asp:CheckBox ID="chkNetwork" runat="server" AutoPostBack="true" OnCheckedChanged="chkNetwork_CheckedChanged" />
                                                            <asp:HiddenField ID="hidNetworkID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                                                        </td>
                                                        <td style="vertical-align: top">
                                                            <asp:Label ID="lblNetwork" runat="server" AssociatedControlID="chkNetwork" Text='<%# DataBinder.Eval(Container.DataItem, "NetworkName") %>' />

                                                            <asp:UpdatePanel ID="upBranch" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Block">
                                                                <ContentTemplate>
                                                                    <asp:Repeater ID="repBranch" runat="server">
                                                                        <HeaderTemplate>
                                                                            <table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td style="vertical-align: top">
                                                                                    <asp:CheckBox ID="chkBranch" runat="server" AutoPostBack="true" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                                                    <asp:HiddenField ID="hidBranchID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                                                                                </td>
                                                                                <td style="vertical-align: top">
                                                                                    <asp:Label ID="lblBranch" runat="server" AssociatedControlID="chkBranch" Text='<%# DataBinder.Eval(Container.DataItem, "BranchName") %>' />

                                                                                    <asp:UpdatePanel ID="upCashBox" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Block">
                                                                                        <ContentTemplate>
                                                                                            <asp:Repeater ID="repCashBox" runat="server">
                                                                                                <HeaderTemplate>
                                                                                                    <table>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <tr>
                                                                                                        <td style="vertical-align: top">
                                                                                                            <asp:CheckBox ID="chkCashBox" runat="server" />
                                                                                                            <asp:HiddenField ID="hidCashBoxID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                                                                                                        </td>
                                                                                                        <td style="vertical-align: top">
                                                                                                            <asp:Label ID="lblCashBox" runat="server" AssociatedControlID="chkCashBox" Text='<%# DataBinder.Eval(Container.DataItem, "CashBoxName") %>' />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    </table>
                                                                                                </FooterTemplate>
                                                                                            </asp:Repeater>
                                                                                        </ContentTemplate>
                                                                                        <Triggers>
                                                                                            <asp:AsyncPostBackTrigger ControlID="chkBranch" EventName="CheckedChanged" />
                                                                                        </Triggers>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="chkNetwork" EventName="CheckedChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="chkCompany" EventName="CheckedChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>

                                    <asp:UpdatePanel ID="upSupplierGroup" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Block">
                                        <ContentTemplate>
                                            <asp:Repeater ID="repSupplierGroup" runat="server" Visible="false">
                                                <HeaderTemplate>
                                                    <table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="vertical-align: top">
                                                            <asp:CheckBox ID="chkSupplierGroup" runat="server" AutoPostBack="true" OnCheckedChanged="chkSupplierGroup_CheckedChanged" />
                                                            <asp:HiddenField ID="hidSupplierGroupID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                                                        </td>
                                                        <td style="vertical-align: top">
                                                            <asp:Label ID="lblSupplierGroup" runat="server" AssociatedControlID="chkSupplierGroup" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierGroup") %>' />

                                                            <asp:UpdatePanel ID="upSupplier" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Block">
                                                                <ContentTemplate>
                                                                    <asp:Repeater ID="repSupplier" runat="server">
                                                                        <HeaderTemplate>
                                                                            <table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td style="vertical-align: top">
                                                                                    <asp:CheckBox ID="chkSupplier" runat="server" AutoPostBack="true" OnCheckedChanged="chkSupplier_CheckedChanged" />
                                                                                    <asp:HiddenField ID="hidSupplierID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                                                                                </td>
                                                                                <td style="vertical-align: top">
                                                                                    <asp:Label ID="lblSupplier" runat="server" AssociatedControlID="chkSupplier" Text='<%# DataBinder.Eval(Container.DataItem, "Supplier") %>' />

                                                                                    <asp:UpdatePanel ID="upTerminal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Block">
                                                                                        <ContentTemplate>
                                                                                            <asp:Repeater ID="repTerminal" runat="server">
                                                                                                <HeaderTemplate>
                                                                                                    <table>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <tr>
                                                                                                        <td style="vertical-align: top">
                                                                                                            <asp:CheckBox ID="chkTerminal" runat="server" />
                                                                                                            <asp:HiddenField ID="hidTerminalID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                                                                                                        </td>
                                                                                                        <td style="vertical-align: top">
                                                                                                            <asp:Label ID="lblTerminal" runat="server" AssociatedControlID="chkTerminal" Text='<%# DataBinder.Eval(Container.DataItem, "Terminal") %>' />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    </table>
                                                                                                </FooterTemplate>
                                                                                            </asp:Repeater>
                                                                                        </ContentTemplate>
                                                                                        <Triggers>
                                                                                            <asp:AsyncPostBackTrigger ControlID="chkSupplier" EventName="CheckedChanged" />
                                                                                        </Triggers>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="chkSupplierGroup" EventName="CheckedChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="chkCompany" EventName="CheckedChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="rblSelectBy" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <%--Credit--%>
        <div id="divCredit" runat="server" class="float-left div-form-field">
            <label for="<%= cblCredit.ClientID %>" class="block bold">Credit</label>
            <asp:CheckBoxList ID="cblCredit" runat="server" />
        </div>

        <%--Card--%>
        <div id="divCard" runat="server" class="float-left div-form-field">
            <label for="<%= cblCard.ClientID %>" class="block bold">Card</label>
            <asp:CheckBoxList ID="cblCard" runat="server" />
        </div>

        <%--TransactionCurrency--%>
        <div id="divTransactionCurrency" runat="server" class="float-left div-form-field">
            <label for="<%= cblTransactionCurrency.ClientID %>" class="block bold">Transaction Currency</label>
            <asp:CheckBoxList ID="cblTransactionCurrency" runat="server" />
        </div>

        <div class="clear-both"></div>

        <hr />

        <div class="div-form-command">
            <asp:Button ID="btnSearchBottom" runat="server" ValidationGroup="Search" OnClick="Search_Click" OnClientClick="javascript: show_wait();" Text="Search" />
            &nbsp;
            <a href="DataInspector.aspx">Reset</a>
            &nbsp;
            <a href="javascript: void(0);" onclick="javascript: display_section('section-search', 'none');">Close</a>
            &nbsp; &nbsp;
            <span id="spnSearchBottom" class="message" style="display: none;">Please wait..</span>
        </div>
    </section>

    <asp:Panel ID="pnlSearchResult" runat="server" Visible="false">
        <asp:HiddenField ID="hidSelectInside" runat="server" />
        <asp:HiddenField ID="hidSelectOutside" runat="server" />
        <asp:HiddenField ID="hidSelectSource" runat="server" />

        <%--Table--%>
        <asp:HiddenField ID="hidTable" runat="server" />

        <%--Payment--%>
        <asp:HiddenField ID="hidUniqueID" runat="server" ClientIDMode="Static" OnValueChanged="hidUniqueID_ValueChanged" />
        <asp:Button ID="btnPayment" runat="server" Text="Payment" OnClick="Payment_Click" CssClass="no-display" />

        <%--Match--%>
        <asp:HiddenField ID="hidQueryID" ClientIDMode="Static" runat="server" />
        <asp:Button ID="btnMatch" runat="server" Text="Match" OnClick="Match_Click" CssClass="no-display" />

        <section class="bg-gray" style="margin: 10px 0px; padding: 5px;">
            <table class="table-list" style="width: 100%;">
                <tr class="nowrap">
                    <td id="tdDisplayInside" runat="server" style="padding: 0px 5px;">
                        <input id="chkInside" type="checkbox" checked="checked" onclick="javascript: display_tables();" />
                        <label for="chkInside">Display Inside</label>
                    </td>
                    <td id="tdDisplayOutside" runat="server" style="padding: 0px 5px;">
                        <input id="chkOutside" type="checkbox" checked="checked" onclick="javascript: display_tables();" />
                        <label for="chkOutside">Display Outside</label>
                    </td>
                    <td id="tdDisplaySource" runat="server" visible="false" style="padding: 0px 5px;">
                        <input id="chkSource" type="checkbox" checked="checked" onclick="javascript: display_tables();" />
                        <label for="chkSource">Display Source</label>
                    </td>
                    <td id="tdRecalculate" runat="server" visible="false" style="padding: 0px 15px;">
                        <asp:LinkButton ID="btnRecalculate" runat="server" OnClick="Recalculate_Click" OnClientClick="javascript: return recalculate_click();" Text="Recalculate" />
                    </td>
                    <td id="tdPayment" runat="server" visible="false" style="padding: 0px 15px;">
                        <asp:LinkButton ID="btnPaymentRestore" runat="server" OnClick="Payment_Restore" Text="Restore" />

                        <asp:Label ID="lblPaymentChange" runat="server" Visible="false">
                            <asp:Button ID="btnPaymentRestoreExecute" runat="server" OnClick="Payment_Change" Text="Restore Payment" />
                            &nbsp;
                            <asp:LinkButton ID="btnPaymentRestoreCancel" runat="server" OnClick="Cancel_Changes" Text="Cancel" />
                        </asp:Label></td>
                    <td id="tdMatchingAuto" runat="server" visible="false" style="padding: 0px 15px;">
                        <asp:LinkButton ID="btnMatchingAuto" runat="server" OnCommand="Matching_Command" CommandArgument="Confirm" Text="Auto Matching" />
                    </td>
                    <td id="tdReturn" runat="server" visible="false" style="padding: 0px 15px;">
                        <asp:LinkButton ID="btnReturn" runat="server" OnClick="Return_Click" Text="Return" />
                    </td>
                    <td id="tdStatus" runat="server" visible="false" style="padding: 0px 15px;">
                        <asp:DropDownList ID="ddlStatusChange" runat="server" />
                        <asp:Button ID="btnStatusChange" runat="server" ValidationGroup="StatusChange" OnClientClick="return status_change();" OnClick="Status_Change" Text="Change" />
                    </td>
                    <td id="tdComment" runat="server" visible="false" style="padding: 0px 15px;">
                        <asp:TextBox ID="txtCommentChange" runat="server" MaxLength="50" placeholder="Comments" />
                        <asp:Button ID="btnCommentChange" runat="server" ValidationGroup="CommentChange" OnClientClick="return comment_change();" OnClick="Comment_Change" Text="Change" />
                    </td>
                    <td style="padding: 0px 15px;">
                        <a href="DataInspector.aspx">Reset</a> </td>
                    <td style="width: 100%;">&nbsp;</td>
                </tr>
            </table>
        </section>
        <div id="divMatchingAuto" runat="server" visible="false" class="section-form" style="padding: 10px;">
            <b>Create Auto Matching</b>
            <table id="tblMatchingField" runat="server" class="table-sub" style="margin: 10px 0px;">
                <tr id="trMatchingFieldName" runat="server">
                    <td class="bg-gray nowrap bold">Field Name:</td>
                    <th>TerminalNumber</th>
                    <th>SupplierNumber</th>
                    <th>CreditBrand</th>
                    <th>CardBrand</th>
                    <th>TransactionCurrency</th>
                    <th>CardNumber</th>
                    <th>CardPrefix</th>
                    <th>PaymentsCount</th>
                    <th>DutyPaymentNumber</th>
                    <th>TransmissionNumber</th>
                    <th>TransactionGrossAmount</th>
                    <th>DutyPaymentAmount</th>
                    <th>TransactionDate</th>
                    <th>PaymentDate</th>
                    <th>VoucherNumber</th>
                    <th>ConfirmationNumber</th>
                </tr>
                <tr>
                    <td class="bg-gray nowrap bold">Included:</td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkTerminalNumber" runat="server" Checked="true" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkSupplierNumber" runat="server" Checked="true" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkCreditBrand" runat="server" Checked="true" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkCardBrand" runat="server" Checked="true" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkTransactionCurrency" runat="server" Checked="true" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkCardNumber" runat="server" Checked="true" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkCardPrefix" runat="server" Checked="true" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkPaymentsCount" runat="server" Checked="true" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkDutyPaymentNumber" runat="server" Checked="true" Enabled="false" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkTransmissionNumber" runat="server" Checked="true" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkTransactionGrossAmount" runat="server" Checked="true" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkDutyPaymentAmount" runat="server" Checked="true" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkTransactionDate" runat="server" Checked="true" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkPaymentDate" runat="server" Checked="true" Enabled="false" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkVoucherNumber" runat="server" Checked="true" /></td>
                    <td class="text-center">
                        <asp:CheckBox ID="chkConfirmationNumber" runat="server" Checked="true" /></td>
                </tr>
                <tr>
                    <td class="bg-gray nowrap bold">Tolerance:</td>
                    <td class="text-center"></td>
                    <td class="text-center"></td>
                    <td class="text-center"></td>
                    <td class="text-center"></td>
                    <td class="text-center"></td>
                    <td class="text-center"></td>
                    <td class="text-center"></td>
                    <td class="text-center"></td>
                    <td class="text-center"></td>
                    <td class="text-center"></td>
                    <td class="text-center">
                        <asp:TextBox ID="txtToleranceTransactionGrossAmount" runat="server" Width="40px" MaxLength="5" /></td>
                    <td class="text-center">
                        <asp:TextBox ID="txtToleranceDutyPaymentAmount" runat="server" Width="40px" MaxLength="5" /></td>
                    <td class="text-center">
                        <asp:TextBox ID="txtToleranceTransactionDate" runat="server" Width="40px" MaxLength="3" /></td>
                    <td class="text-center">
                        <asp:TextBox ID="txtTolerancePaymentDate" runat="server" Width="40px" MaxLength="3" /></td>
                    <td class="text-center">
                        <asp:TextBox ID="txtToleranceVoucherNumber" runat="server" Width="40px" MaxLength="1" /></td>
                    <td class="text-center">
                        <asp:TextBox ID="txtToleranceConfirmationNumber" runat="server" Width="40px" MaxLength="1" /></td>
                </tr>
            </table>

            <asp:Button ID="btnMatchingAutoSave" runat="server" OnClientClick="javascript: return confirm('Create Auto Matching?');" OnCommand="Matching_Command" CommandArgument="Save" Text="Create Auto Matching" />
            &nbsp;
            <asp:LinkButton ID="btnMatchingAutoCancel" runat="server" OnClick="Cancel_Changes" Text="Cancel" />
        </div>


        <div id="divInside" runat="server" class="float-left" style="width: 49%; position: relative;">
            <b>Inside</b> &nbsp; - &nbsp;
            <asp:LinkButton ID="btnDownloadInside" runat="server" ValidationGroup="none" Text="Download Excel" OnClick="Download_Excel" />

            <style>
                #loader {
                    border: 16px solid #f3f3f3; /* Light grey */
                    border-top: 16px solid #3498db; /* Blue */
                    border-radius: 50%;
                    width: 120px;
                    height: 120px;
                    animation: spin 2s linear infinite;
                }

                @keyframes spin {
                    0% {
                        transform: rotate(0deg);
                    }

                    100% {
                        transform: rotate(360deg);
                    }
                }
            </style>
            <div id="dvGrid" style="width: 100%; overflow: scroll; position: relative; height: 500px;">
                <div style="position: sticky; top: 0; z-index: 9; height: 57px;">
                    <table id="tblInsideHead" class="table-data"></table>
                </div>
                <div style="position: relative;">
                    <asp:GridView ID="gvInside" runat="server" AutoGenerateColumns="true" OnRowDataBound="Inside_RowDataBound" CssClass="table-data" />
                </div>
            </div>

            <%--<div style="overflow: hidden;">
                <table id="tblInsideHead" class="table-data"></table>
            </div>
            <div style="height: 500px; overflow-x: hidden; overflow-y: auto;">
                <asp:GridView ID="gvInside" runat="server" AutoGenerateColumns="true" OnRowDataBound="Inside_RowDataBound" CssClass="table-data" />
            </div>--%>
            <div class="div-form-message" style="margin: 5px 0px;">
                <table class="nowrap" style="width: 100%; font-size: 11pt;">
                    <tr class="bold">
                        <td>&nbsp; &nbsp;</td>
                        <td>&nbsp; &nbsp;</td>
                        <td>Rows</td>
                        <td>&nbsp; &nbsp;</td>
                        <td>Amount</td>
                        <td style="width: 100%;">&nbsp; &nbsp;</td>
                        <td></td>
                        <%--Page--%>
                    </tr>
                    <tr id="trInsideSelected" runat="server" visible="false" style="height: 25px;">
                        <td class="bold">Selected</td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:Label ID="lblInsideRowsSelected" runat="server" /></td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:Label ID="lblInsideAmountSelected" runat="server" /></td>
                        <td>&nbsp; &nbsp;</td>
                        <td>&nbsp; &nbsp;</td>
                    </tr>
                    <tr id="trInsideRemaining" runat="server" visible="false" style="height: 25px;">
                        <td class="bold">Remaining</td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:Label ID="lblInsideRowsRemaining" runat="server" /></td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:Label ID="lblInsideAmountRemaining" runat="server" /></td>
                        <td>&nbsp; &nbsp;</td>
                        <td>&nbsp; &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="bold">Total</td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:Label ID="lblInsideRows" ClientIDMode="Static" runat="server" /></td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:Label ID="lblInsideAmount" runat="server" /></td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:DropDownList ID="ddlInsidePage" visible="false" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Page_Changed" Width="100px" /></td>
                    </tr>
                </table>
            </div>
            <div class="modal">
                <!-- Place at bottom of page -->
            </div>
        </div>

        <div id="divDataSeparator" runat="server" class="float-left nowrap" style="width: 2%;">&nbsp;</div>
        <div id="divOutside" runat="server" class="float-left" style="width: 49%; position: relative;">
            <b>Outside</b> &nbsp; - &nbsp;
            <asp:LinkButton ID="btnDownloadOutside" runat="server" ValidationGroup="none" Text="Download Excel" OnClick="Download_Excel" />


            <div id="dvGridOutside" style="width: 100%; overflow: scroll; position: relative; height: 500px;">
                <div style="position: sticky; top: 0; z-index: 9; height: 57px;">
                    <table id="tblOutsideHead" class="table-data"></table>
                </div>
                <div style="position: relative;">
                    <asp:GridView ID="gvOutside" runat="server" AutoGenerateColumns="true" OnRowDataBound="Outside_RowDataBound" CssClass="table-data" />
                </div>
            </div>

            <%-- <div style="overflow: hidden;">
                <table id="tblOutsideHead" class="table-data"></table>
            </div>
            <div style="height: 500px; overflow-x: hidden; overflow-y: auto;">
                <asp:GridView ID="gvOutside" runat="server" AutoGenerateColumns="true" OnRowDataBound="Outside_RowDataBound" CssClass="table-data" />
            </div>--%>

            <div class="div-form-message" style="margin: 5px 0px;">
                <table class="nowrap" style="width: 100%; font-size: 11pt;">
                    <tr class="bold">
                        <td>&nbsp; &nbsp;</td>
                        <td>&nbsp; &nbsp;</td>
                        <td>Rows</td>
                        <td>&nbsp; &nbsp;</td>
                        <td>Amount</td>
                        <td style="width: 100%;">&nbsp; &nbsp;</td>
                        <td></td><%--Page--%>
                    </tr>
                    <tr id="trOutsideSelected" runat="server" visible="false" style="height: 25px;">
                        <td class="bold">Selected</td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:Label ID="lblOutsideRowsSelected" runat="server" /></td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:Label ID="lblOutsideAmountSelected" runat="server" /></td>
                        <td>&nbsp; &nbsp;</td>
                        <td>&nbsp; &nbsp;</td>
                    </tr>
                    <tr id="trOutsideRemaining" runat="server" visible="false" style="height: 25px;">
                        <td class="bold">Remaining</td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:Label ID="lblOutsideRowsRemaining" runat="server" /></td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:Label ID="lblOutsideAmountRemaining" runat="server" /></td>
                        <td>&nbsp; &nbsp;</td>
                        <td>&nbsp; &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="bold">Total</td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:Label ID="lblOutsideRows" runat="server" /></td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:Label ID="lblOutsideAmount" runat="server" /></td>
                        <td>&nbsp; &nbsp;</td>
                        <td>
                            <asp:DropDownList ID="ddlOutsidePage" Visible="false" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Page_Changed" Width="100px" /></td>
                    </tr>
                </table>
            </div>
            <div class="modalOutside">
                <!-- Place at bottom of page -->
            </div>
        </div>

        <div id="divSourceSeparator" runat="server" visible="false" class="float-left nowrap" style="width: 2%;">&nbsp;</div>
        <div id="divSource" runat="server" visible="false" class="float-left nowrap" style="width: 49%;">
            <b>Source</b>
            <div style="max-height: 500px; overflow: auto;">
                <asp:GridView ID="gvSource" runat="server" AutoGenerateColumns="true" OnRowDataBound="Source_RowDataBound" CssClass="table-data" />
            </div>
            <%--
            <div class="div-form-message" style="margin: 5px 0px;">
                <b>Rows</b> : <asp:Label ID="lblSourceRows" runat="server" />
                &nbsp;&nbsp;&nbsp;
                <b>Amount</b> : <asp:Label ID="lblSourceAmount" runat="server" />
                &nbsp;&nbsp;&nbsp;
                <b>Page</b> : <asp:DropDownList ID="ddlSourcePage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Page_Changed" Width="100px" />
            </div>
            --%>
        </div>

        <div class="clear-both"></div>

        <asp:Panel ID="pnlMatchingBalance" runat="server" Visible="false" DefaultButton="btnMatchingBalanceChange" CssClass="section-form nowrap" Style="padding: 10px;">
            <b>Change Matching</b> &nbsp; : &nbsp;
            <div id="divMatchingBalanceRow" runat="server" visible="false" style="display: inline;">
                Company
                <asp:TextBox ID="txtCompanyName" runat="server" ReadOnly="true" Width="120px" />
                <asp:HiddenField ID="hidCompanyID" runat="server" />
                &nbsp;
                Balance Amount
                <asp:TextBox ID="txtBalanceAmount" runat="server" ReadOnly="true" Width="120px" />
                <asp:HiddenField ID="hidBalanceAmount" runat="server" />
                &nbsp;
                Operation Type
                <asp:DropDownList ID="ddlOperationType_Balance" runat="server" />
                <asp:RequiredFieldValidator ID="rfvOperationType_Balance" runat="server" ValidationGroup="MatchingBalance" ControlToValidate="ddlOperationType_Balance" InitialValue="0" ErrorMessage="Select 'Operation Type'." Display="Dynamic" SetFocusOnError="true" CssClass="error" />
                &nbsp;
            </div>
            Comment
            <asp:TextBox ID="txtMatchingComment" runat="server" MaxLength="50" />
            &nbsp;
            <asp:Button ID="btnMatchingBalanceChange" runat="server" ValidationGroup="MatchingBalance" OnClientClick="javascript: return matching_change();" OnClick="Save_Changes" Text="Save Changes" />
            &nbsp;
            <asp:LinkButton ID="btnMatchingBalanceCancel" runat="server" OnClick="Cancel_Changes" Text="Cancel" />
        </asp:Panel>

        <asp:Panel ID="pnlPaymentChange" runat="server" Visible="false" DefaultButton="btnPaymentRecalculate" class="section-form nowrap" Style="padding: 10px;">
            <b>Change Payments Count</b> &nbsp; : &nbsp;
            <asp:HiddenField ID="hidPaymentsAmount_Selected" runat="server" />

            New Payments Count :
            <asp:TextBox ID="txtPaymentsCount_Split" runat="server" MaxLength="2" Width="50px" onchange="javascript: disable_payment_change();" />
            &nbsp;
            First Payment Amount :
            <asp:TextBox ID="txtFirstPaymentAmount_Split" runat="server" onchange="javascript: disable_payment_change();" />
            &nbsp;
            <asp:Button ID="btnPaymentRecalculate" runat="server" ValidationGroup="PaymentChange" OnClientClick="return payment_recalculate();" OnClick="Payment_Recalculate" Text="Recalculate" Style="margin: 0px 5px;" />
            <asp:Button ID="btnPaymentChange" runat="server" Visible="false" ValidationGroup="PaymentChange" OnClientClick="javascript: return confirm('Save Changes?');" OnClick="Payment_Change" Text="Save Changes" Style="margin: 0px 5px;" />
            <asp:LinkButton ID="btnPaymentCancel" runat="server" OnClick="Cancel_Changes" Text="Cancel" Style="margin: 0px 5px;" />

            <div id="divBalance" runat="server" visible="false" style="overflow: auto; margin-top: 15px;">
                <b>Balance</b>
                <asp:GridView ID="gvBalance" runat="server" AutoGenerateColumns="true" OnRowDataBound="Payment_RowDataBound" CssClass="table-sub" />
            </div>

            <div id="divSplit" runat="server" visible="false" style="overflow: auto; margin-top: 15px;">
                <b>Split</b>
                <asp:GridView ID="gvSplit" runat="server" AutoGenerateColumns="true" OnRowDataBound="Payment_RowDataBound" CssClass="table-sub" />
            </div>
        </asp:Panel>
    </asp:Panel>

    <style>
        .modal {
            display: none;
            position: absolute;
            z-index: 1000;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            background: rgba( 255, 255, 255, .01 ) url(Images/FhHRx.gif) 50% 50% no-repeat;
        }

        body.loading {
            overflow: hidden;
        }

            body.loading .modal {
                display: block;
            }

        .modalOutside {
            display: none;
            position: absolute;
            z-index: 1000;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            background: rgba( 255, 255, 255, .01 ) url(Images/FhHRx.gif) 50% 50% no-repeat;
        }

        body.loadingOutside {
            overflow: hidden;
        }

            body.loadingOutside .modalOutside {
                display: block;
            }
    </style>
    <script src="/App_JS/jquery.min.js"></script>
    <script type="text/javascript">

        var pageIndex = 1;
        var pageCount = 20;
        var pageSize = 20;
        var pageIndexOutside = 1;
        var pageCountOutside = 20;
        var IsSorted = false;
        var ISOutSorted = false;
        $(document).ready(function () {
            $("#dvGrid").on("scroll", function (e) {
                var $o = $(e.currentTarget);
                if ($o[0].scrollHeight - $o.scrollTop() <= $o.outerHeight()) {
                    GetRecords();
                }
            });
            $("#dvGridOutside").on("scroll", function (e) {
                var $o = $(e.currentTarget);
                if ($o[0].scrollHeight - $o.scrollTop() <= $o.outerHeight()) {
                    GetOutsideRecords();
                }
            });
            $("#tblInsideHead").on("click", 'td', function (event) {

                var htmldata = '';
                var index = getIndex(this);
                $("[id$=gvInside] tbody>tr:not(:first-child)").sortElements(function (a, b) {
                    try {
                        if (IsSorted == false) {
                            if (isvalidDate(a.children[index].innerText)) {
                                return getcorrectDate(a.children[index].innerText.replace(',', '')) > getcorrectDate(b.children[index].innerText.replace(',', '')) ? 1 : -1;
                            }
                            else if (!isNaN(parseInt(a.children[index].innerText))) {
                                return parseInt(a.children[index].innerText.replace(',', '')) > parseInt(b.children[index].innerText.replace(',','')) ? 1 : -1;
                            }
                            else {
                                return a.children[index].innerText > b.children[index].innerText.replace(',','') ? 1 : -1;
                            }
                        }
                        else
                        {
                            if (isvalidDate(a.children[index].innerText)) {
                                return getcorrectDate(a.children[index].innerText.replace(',', '')) > getcorrectDate(b.children[index].innerText.replace(',', '')) ? -1 : 1;
                            }
                            else if (!isNaN(parseInt(a.children[index].innerText))) {
                                return parseInt(a.children[index].innerText.replace(',', '')) > parseInt(b.children[index].innerText.replace(',','')) ? -1 : 1;
                            }
                            else {
                                return a.children[index].innerText > b.children[index].innerText.replace(',','') ? -1 : 1;
                            }
                        }
                    }
                    catch (err)
                    {
                    }
                });
                IsSorted=IsSorted ? false:true; 
            });
            
            $("#tblOutsideHead").on("click", 'td', function (event) {

                var htmldata = '';
                var index = getIndex(this);

                $("[id$=gvOutside] tbody>tr:not(:first-child)").sortElements(function (a, b) {
                    try {
                        if (ISOutSorted == false) {
                            if (isvalidDate(a.children[index].innerText))
                            {
                                return getcorrectDate(a.children[index].innerText.replace(',', '')) > getcorrectDate(b.children[index].innerText.replace(',', '')) ? 1 : -1;
                            }
                            else if (!isNaN(parseInt(a.children[index].innerText))) {
                                return parseInt(a.children[index].innerText.replace(',', '')) > parseInt(b.children[index].innerText.replace(',', '')) ? 1 : -1;
                            }
                            else {
                                return a.children[index].innerText > b.children[index].innerText.replace(',', '') ? 1 : -1;
                            }
                        }
                        else
                        {
                            if (isvalidDate(a.children[index].innerText)) {
                                return getcorrectDate(a.children[index].innerText.replace(',', '')) > getcorrectDate(b.children[index].innerText.replace(',', '')) ? -1 : 1;
                                //return new Date(Date.parse(a.children[index].innerText.replace(',', ''), "dd-MM-yyyy")) > new Date(Date.parse(b.children[index].innerText.replace(',', ''), "dd-MM-yyyy")) ? -1 :1;
                            }
                            else if (!isNaN(parseInt(a.children[index].innerText))) {
                                return parseInt(a.children[index].innerText.replace(',', '')) > parseInt(b.children[index].innerText.replace(',', '')) ? -1 : 1;
                            }
                            else {
                                return a.children[index].innerText > b.children[index].innerText.replace(',', '') ? -1 : 1;
                            }
                        }
                    }
                    catch (err) {
                    }
                });
                ISOutSorted = ISOutSorted ? false : true; 
            });
           
        });
       //function isvalidDate(dateString)
       // {

       //    try {
       //        if (!isNaN(new Date(Date.parse(dateString, "dd-MM-yyyy"))))
       //            return true;
       //        else
       //            return false;
       //     }
       //     catch (err) {
       //         return false;
       //     }
            
       // }
        function getcorrectDate(dateString)
        {
            dataparts = dateString.split('-');            
            return new Date(dataparts[2], dataparts[1] - 1, dataparts[0]);
           
        }
        function isvalidDate(dateString) {
            dataparts = dateString.split('-');
            if (dataparts.length > 1)
                return true;
            return false;
        }
        function getIndex(elm) {
            var parent = elm.parentElement;
            for (var i = 0; i < parent.children.length; i++) {
                if (parent.children[i].isEqualNode(elm)) {
                    return i;
                }
            }
        }
        function GetRecords() {
            var rowText = $("[id$=lblInsideRows]").text();
            rowText = rowText.replace(",", "");
            var lblInsideRows = parseInt(rowText);
            pageIndex++;
            var trs = $('#dvGrid .table-data[rules="all"] tr');
            var trClass = 0;
            trs.each(function (idx, ele) {
                if (ele.attributes["class"] && ele.attributes["class"].value == '') trClass++;
            })
            if (trClass < lblInsideRows && lblInsideRows >= pageSize && pageIndex <= pageCount ) {
                $("body").addClass("loading");
                var hidUniqueID = $('#hidUniqueID').val();
                var hidQueryID = $('#hidQueryID').val();
                var ddlTransactions = $('#ddlTransactions option:selected').val();
                $.ajax({
                    type: "POST",
                    url: "/DataInspector.aspx/GetInsidedata",
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ pageIndex: pageIndex, userId: 1, hidUniqueID: hidUniqueID, hidQueryID: hidQueryID, ddlTransactions: ddlTransactions }),
                    success: OnSuccess,
                    failure: function (response) {
                        alert(response.d);
                    },
                    error: function (response) {
                        alert(response.d);
                    }


                });
            }
        }

        jQuery.fn.sortElements = (function () {

            var sort = [].sort;

            return function (comparator, getSortable) {

                getSortable = getSortable || function () { return this; };

                var placements = this.map(function () {

                    var sortElement = getSortable.call(this),
                        parentNode = sortElement.parentNode,

                        // Since the element itself will change position, we have
                        // to have some way of storing its original position in
                        // the DOM. The easiest way is to have a 'flag' node:
                        nextSibling = parentNode.insertBefore(
                            document.createTextNode(''),
                            sortElement.nextSibling
                        );

                    return function () {

                        if (parentNode === this) {
                            throw new Error(
                                "You can't sort elements if any one is a descendant of another."
                            );
                        }

                        // Insert before flag:
                        parentNode.insertBefore(this, nextSibling);
                        // Remove flag:
                        parentNode.removeChild(nextSibling);

                    };

                });

                return sort.call(this, comparator).each(function (i) {
                    placements[i].call(getSortable.call(this));
                });

            };

        })();

        function OnSuccess(response) {
            $("body").removeClass("loading");
            $("[id$=gvInside] tbody").append(response.d);

        }

        function GetOutsideRecords() {
            var rowText = $("[id$=lblOutsideRows]").text();
            rowText = rowText.replace(",", "");
            var lblOutsideRows = parseInt(rowText);
            pageIndexOutside++;
            var trs = $('#dvGridOutside .table-data[rules="all"] tr');
            var trClassCount = 0;
            trs.each(function (idx, ele) {
                if (ele.attributes["class"] && ele.attributes["class"].value == '') trClassCount++;
            })
            if (trClassCount < lblOutsideRows && lblOutsideRows >= pageSize && pageIndexOutside <= pageCountOutside) {
                $("body").addClass("loadingOutside");
                var hidUniqueID = $('#hidUniqueID').val();
                var hidQueryID = $('#hidQueryID').val();
                var ddlTransactions = $('#ddlTransactions option:selected').val();
                $.ajax({
                    type: "POST",
                    url: "/DataInspector.aspx/GetOutsidedata",
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ pageIndex: pageIndexOutside, userId: 1, hidUniqueID: hidUniqueID, hidQueryID: hidQueryID, ddlTransactions: ddlTransactions  }),
                    success: OnOutsideSuccess,
                    failure: function (response) {
                        alert(response.d);
                        $("body").removeClass("loadingOutside");
                    },
                    error: function (response) {
                        alert(response.d);
                        $("body").removeClass("loadingOutside");
                    }
                });
            }
        }

        function OnOutsideSuccess(response) {
            $("body").removeClass("loadingOutside");
            var finalData = JSON.parse(response.d);
            $("[id$=gvOutside] tbody").append(finalData.data);

        }
    </script>


    <script>
        // TABLES

        try {
            document.getElementById("<%= gvInside.ClientID %>").attributes.removeNamedItem("style");
            copy_table_head("<%= gvInside.ClientID %>", "tblInsideHead");
            highlight_data_item_all("checkbox-inside");
        } catch (ex) { }

        try {
            document.getElementById("<%= gvOutside.ClientID %>").attributes.removeNamedItem("style");
            copy_table_head("<%= gvOutside.ClientID %>", "tblOutsideHead");
            highlight_data_item_all("checkbox-outside");
        } catch (ex) { }

        try { document.getElementById("<%= gvSource.ClientID %>").attributes.removeNamedItem("style"); } catch (ex) { }
        try { document.getElementById("<%= gvBalance.ClientID %>").attributes.removeNamedItem("style"); } catch (ex) { }
        try { document.getElementById("<%= gvSplit.ClientID %>").attributes.removeNamedItem("style"); } catch (ex) { }

        // SEARCH

        if (document.getElementById("<%= chk_txtTransactionDateOutside.ClientID %>").checked == false) { document.getElementById("<%= txtTransactionDateOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtTransmissionDateOutside.ClientID %>").checked == false) { document.getElementById("<%= txtTransmissionDateOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtPaymentDateOutside.ClientID %>").checked == false) { document.getElementById("<%= txtPaymentDateOutside.ClientID %>").className = ""; }

        try {
            if (document.getElementById("<%= chk_txtMatchingDateOutside.ClientID %>").checked == false) { document.getElementById("<%= txtMatchingDateOutside.ClientID %>").className = ""; }
        } catch (ex) { }

        if (document.getElementById("<%= chk_txtCardPrefixOutside.ClientID %>").checked == false) { document.getElementById("<%= txtCardPrefixOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtCardNumberOutside.ClientID %>").checked == false) { document.getElementById("<%= txtCardNumberOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtTransmissionNumberOutside.ClientID %>").checked == false) { document.getElementById("<%= txtTransmissionNumberOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtVoucherNumberOutside.ClientID %>").checked == false) { document.getElementById("<%= txtVoucherNumberOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtConfirmationNumberOutside.ClientID %>").checked == false) { document.getElementById("<%= txtConfirmationNumberOutside.ClientID %>").className = ""; }
          if (document.getElementById("<%= chk_txtPaymentsCountOutside.ClientID %>").checked == false) { document.getElementById("<%= txtPaymentsCountOutside.ClientID %>").className = ""; }
          if (document.getElementById("<%= chk_txtDutyPaymentNumberOutside.ClientID %>").checked == false) { document.getElementById("<%= txtDutyPaymentNumberOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtTransactionGrossAmountOutside.ClientID %>").checked == false) { document.getElementById("<%= txtTransactionGrossAmountOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtDutyPaymentAmountOutside.ClientID %>").checked == false) { document.getElementById("<%= txtDutyPaymentAmountOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtRemainingPaymentsAmountOutside.ClientID %>").checked == false) { document.getElementById("<%= txtRemainingPaymentsAmountOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtCompanyNumberOutside.ClientID %>").checked == false) { document.getElementById("<%= txtCompanyNumberOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtNetworkNumberOutside.ClientID %>").checked == false) { document.getElementById("<%= txtNetworkNumberOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtBranchNumberOutside.ClientID %>").checked == false) { document.getElementById("<%= txtBranchNumberOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtCashBoxNumberOutside.ClientID %>").checked == false) { document.getElementById("<%= txtCashBoxNumberOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtSupplierGroupNumberOutside.ClientID %>").checked == false) { document.getElementById("<%= txtSupplierGroupNumberOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtSupplierNumberOutside.ClientID %>").checked == false) { document.getElementById("<%= txtSupplierNumberOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtTerminalNumberOutside.ClientID %>").checked == false) { document.getElementById("<%= txtTerminalNumberOutside.ClientID %>").className = ""; }
        if (document.getElementById("<%= chk_txtCommentOutside.ClientID %>").checked == false) { document.getElementById("<%= txtCommentOutside.ClientID %>").className = ""; }

        // FUNCTIONS

        function copy_table_head(s_source_id, s_destination_id) {
            var tbl_source = document.getElementById(s_source_id);
            var tbl_destination = document.getElementById(s_destination_id);

            tbl_source.style.position = "relative";
            tbl_source.style.top = "-61px";

            tbl_destination.style.position = "relative";

            var tr_source = tbl_source.rows[0];
            var tr_destination = tbl_destination.insertRow(0);

            tr_destination.className = "bg-gray";

            for (var i = 0; i < tr_source.cells.length; i++) {
                var td_source = tr_source.cells[i];
                var td_destination = tr_destination.insertCell(i);

                td_destination.innerHTML = td_source.innerHTML;
                td_destination.style.fontWeight = "bold";

                if (i == 0) { td_source.innerHTML = ""; }
            }

            tr_source.style.visibility = "hidden";
            tr_source.style.height = "60px";
        }

        function display_tables() {
            var chk_inside = document.getElementById("chkInside");
            var chk_outside = document.getElementById("chkOutside");
            var chk_source = document.getElementById("chkSource");

            var chk_first, chk_second, div_first, div_second, div_separator;

            if (chk_source == null) {
                chk_first = chk_inside;
                chk_second = chk_outside;

                div_first = document.getElementById("<%= divInside.ClientID %>");
                div_second = document.getElementById("<%= divOutside.ClientID %>");
                div_separator = document.getElementById("<%= divDataSeparator.ClientID %>");
            }
            else {
                if (chk_inside != null) {
                    chk_first = chk_inside;
                    div_first = document.getElementById("<%= divInside.ClientID %>");
                }
                else {
                    chk_first = chk_outside;
                    div_first = document.getElementById("<%= divOutside.ClientID %>");
                }

                chk_second = chk_source;
                div_second = document.getElementById("<%= divSource.ClientID %>");
                div_separator = document.getElementById("<%= divSourceSeparator.ClientID %>");
            }

            if (chk_first.checked == chk_second.checked) {
                div_first.style.display = "";
                div_first.style.width = "49%";

                div_separator.style.display = "";

                div_second.style.display = "";
                div_second.style.width = "49%";
            }
            else {
                div_separator.style.display = "none";

                if (chk_first.checked == true) {
                    div_first.style.width = "100%";
                    div_second.style.display = "none";
                }
                else if (chk_second.checked == true) {
                    div_second.style.width = "100%";
                    div_first.style.display = "none";
                }
            }
        }

        function display_outside(s_id, b_checked) {
            var o_textbox = document.getElementById(s_id.replace("chk_", ""));

            if (b_checked == true) {
                o_textbox.value = "";
                o_textbox.className = "no-display";
            }
            else {
                o_textbox.className = "";
            }
        }

        function get_mode() {
            var s_mode = "";

            if (document.getElementById("<%= hidUniqueID.ClientID %>").value != "") {
                s_mode = "payment";
            }
            else if (document.getElementById("<%= hidQueryID.ClientID %>").value != "") {
                s_mode = "match";
            }
            else {
                s_mode = document.getElementById("<%= ddlTransactions.ClientID %>").value
            }

            return s_mode;
        }

        function recalculate_click() {
            var s_inside = document.getElementById("<%= hidSelectInside.ClientID %>").value;
            var s_outside = document.getElementById("<%= hidSelectOutside.ClientID %>").value;
            var s_source = document.getElementById("<%= hidSelectSource.ClientID %>").value;

            var b_valid = (s_inside != "" || s_outside != "" || s_source != "");

            if (b_valid == false) {
                var s_mode = get_mode();
                var s_action = (s_mode == "match" || s_mode == "matching") ? "Unselect" : "Select";
                var s_alert = s_action + " item/s to recalculate.";

                alert(s_alert);
            }

            return b_valid;
        }

        function payment_click(s_unique_id, s_table) {
            document.getElementById("<%= hidUniqueID.ClientID %>").value = s_unique_id;
            document.getElementById("<%= hidTable.ClientID %>").value = s_table;
            document.getElementById("<%= btnPayment.ClientID %>").click();            
        }

        function match_click(s_query_id) {
            document.getElementById("<%= hidQueryID.ClientID %>").value = s_query_id;
            document.getElementById("<%= btnMatch.ClientID %>").click();
        }

        function matching_change() {
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate("MatchingBalance") != true) { return false; }
            }

            return confirm('Save Changes?');
        }

        function payment_recalculate() {
            var txt_payments_count = document.getElementById("<%= txtPaymentsCount_Split.ClientID %>");
            var txt_first_payment_amount = document.getElementById("<%= txtFirstPaymentAmount_Split.ClientID %>");

            var s_payments_count = trim_string(txt_payments_count.value);
            var s_first_payment_amount = trim_string(txt_first_payment_amount.value);

            var s_payments_count_error = "";

            if (s_payments_count == "") {
                s_payments_count_error = "Enter 'New Payments Count'.";
            }
            else {
                var n_payments_count = parseInt(s_payments_count);

                if (isNaN(n_payments_count) || n_payments_count <= 0) {
                    s_payments_count_error = "'New Payments Count' must be a whole number greater than zero.";
                }
            }

            if (s_payments_count_error != "") {
                alert(s_payments_count_error);
                txt_payments_count.focus();
                txt_payments_count.select();
                return false;
            }

            var s_first_payment_amount_error = "";

            if (s_first_payment_amount == "") {
                s_first_payment_amount_error = "Enter 'First Payment Amount'.";
            }
            else {
                var n_first_payment_amount = parseFloat(s_first_payment_amount);

                if (isNaN(n_first_payment_amount) || n_first_payment_amount == 0) {
                    s_first_payment_amount_error = "'First Payment Amount' must be a number not equal to zero.";
                }
            }

            if (s_first_payment_amount_error != "") {
                alert(s_first_payment_amount_error);
                txt_first_payment_amount.focus();
                txt_first_payment_amount.select();
                return false;
            }

            return true;
        }

       function status_change() {
            var s_inside = document.getElementById("<%= hidSelectInside.ClientID %>").value;
            var s_outside = document.getElementById("<%= hidSelectOutside.ClientID %>").value;

            if (s_inside == "" && s_outside == "") {
                alert("Select item/s to change status.");
                return false;
            }

            var o_select = document.getElementById("<%= ddlStatusChange.ClientID %>");

            if (o_select.value == "-1") {
                alert("Select 'Status' to change selected item/s status.");
                o_select.focus();
                return false;
            }

            return confirm("Change selected item/s status?");
        }

        function comment_change() {
            var s_inside = document.getElementById("<%= hidSelectInside.ClientID %>").value;
            var s_outside = document.getElementById("<%= hidSelectOutside.ClientID %>").value;

            if (s_inside == "" && s_outside == "") {
                alert("Select item/s to change comments.");
                return false;
            }

            return confirm("Change selected item/s comments?");
        }

        function show_wait() {
            try {
                document.getElementById("<%= lblError.ClientID %>").innerHTML = "";
                document.getElementById("<%= lblMessage.ClientID %>").innerHTML = "";
            }
            catch (ex) { }

            document.getElementById("spnSearchTop").style.display = "";
            document.getElementById("spnSearchBottom").style.display = "";

            document.getElementById("<%= btnSearchTop.ClientID %>").style.visibility = "hidden";
            document.getElementById("<%= btnSearchBottom.ClientID %>").style.visibility = "hidden";
        }

        function select_data_item_all(s_class, b_checked) {
            var chkSelectItemArray = document.getElementsByClassName(s_class);

            var s_mode = get_mode();
            var b_return_checked = (s_mode == "match" || s_mode == "matching") ? false : true;

            disable_buttons(s_mode);

            checkbox_check_all(chkSelectItemArray, b_checked, b_return_checked);

            // main checkbox not included

            for (var i = 1; i < chkSelectItemArray.length; i++) {
                var o_checkbox = chkSelectItemArray[i];

                update_hidden_field(o_checkbox, s_class, b_return_checked);

                highlight_data_item(o_checkbox, b_return_checked);
            }
        }

        function select_data_item(s_class, o_checkbox) {
            var chkSelectItemArray = document.getElementsByClassName(s_class);

            var s_mode = get_mode();
            var b_return_checked = (s_mode == "match" || s_mode == "matching") ? false : true;

            disable_buttons(s_mode);

            update_hidden_field(o_checkbox, s_class, b_return_checked);

            highlight_data_item(o_checkbox, b_return_checked);

            checkbox_check(chkSelectItemArray, b_return_checked);
        }

        function highlight_data_item(o_checkbox, b_return_checked) {
            var o_row = o_checkbox.parentElement.parentElement;
            var s_data_class = (o_row.attributes["data-class"] != null) ? o_row.attributes["data-class"].value : "";

            if (b_return_checked == true) {
                o_row.className = (o_checkbox.checked == true) ? "bg-yellow" : s_data_class;
            }
            else {
                o_row.className = (o_checkbox.checked == false) ? "bg-yellow" : s_data_class;
            }
        }

        function highlight_data_item_all(s_class) {
            var s_mode = get_mode();
            var b_return_checked = (s_mode == "match" || s_mode == "matching") ? false : true;

            if (s_mode == "all") { return; }

            var chkSelectItemArray = document.getElementsByClassName(s_class);

            // main checkbox not included

            for (var i = 1; i < chkSelectItemArray.length; i++) {
                highlight_data_item(chkSelectItemArray[i], b_return_checked);
            }
        }

        function disable_buttons(s_mode) {
            if (s_mode == "payment") {
                var btnPaymentRecalculate = document.getElementById("<%= btnPaymentRecalculate.ClientID %>");
                var btnPaymentChange = document.getElementById("<%= btnPaymentChange.ClientID %>");

                if (btnPaymentRecalculate != null) { btnPaymentRecalculate.disabled = true; }
                if (btnPaymentChange != null) { btnPaymentChange.disabled = true; }
            }
            else if (s_mode == "match" || s_mode == "matching" || s_mode == "not-matching") {
                var btnMatchingBalanceChange = document.getElementById("<%= btnMatchingBalanceChange.ClientID %>");

                if (btnMatchingBalanceChange != null) { btnMatchingBalanceChange.disabled = true; }
            }
        }

        function disable_payment_change() {
            var btnPaymentChange = document.getElementById("<%= btnPaymentChange.ClientID %>");

            if (btnPaymentChange != null) { btnPaymentChange.disabled = true; }
        }

        function update_hidden_field(o_checkbox, s_class, b_return_checked) {
            var s_id = "";

            switch (s_class) {
                case "checkbox-inside":
                    s_id = "<%= hidSelectInside.ClientID %>";
                    break;
                case "checkbox-outside":
                    s_id = "<%= hidSelectOutside.ClientID %>";
                    break;
                case "checkbox-source":
                    s_id = "<%= hidSelectSource.ClientID %>";
                    break;
            }

            var o_hidden = document.getElementById(s_id);
            var s_value = o_hidden.value;

            if (o_checkbox.checked == b_return_checked) {
                // add
                if (s_value == "") {
                    s_value = o_checkbox.value;
                }
                else {
                    s_value = s_value + "," + o_checkbox.value;
                }
            }
            else {
                // del
                if (s_value != "") {
                    var arr_value = s_value.split(",");

                    s_value = "";

                    for (var i = 0; i < arr_value.length; i++) {
                        if (arr_value[i] != o_checkbox.value) {
                            if (s_value != "") { s_value += ","; }

                            s_value += arr_value[i];
                        }
                    }
                }
            }

            o_hidden.value = s_value;
        }
    </script>


</asp:Content>

