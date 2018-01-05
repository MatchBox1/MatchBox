<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataFileForm.ascx.cs" Inherits="MatchBox.DataFileForm" %>

<h2 id="hedTitle" runat="server" class="hed-form"></h2>

<div class="div-form-item">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<div class="div-form-command">
    <asp:Button ID="btnSaveTop" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="javascript: show_wait();" />
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
    Status:
    <asp:Label ID="lblStatusTop" runat="server" Text="New" />
</div>

<asp:HiddenField ID="hidDataSourceID" runat="server" />
<asp:HiddenField ID="hidCreditID" runat="server" />
<asp:HiddenField ID="hidDiscountID" runat="server" />

<%--DateFrom--%>
<div id="divDateFrom_Change" runat="server" class="float-left div-form-field">
    <asp:Label ID="lblDateFrom" runat="server" />
    <br />
    <asp:DropDownList ID="ddlDayFrom" runat="server" style="width: 32%;">
        <asp:ListItem Text="Day" Value="0" />
    </asp:DropDownList>
    <asp:DropDownList ID="ddlMonthFrom" runat="server" style="width: 32%;">
        <asp:ListItem Text="Month" Value="0" />
    </asp:DropDownList>
    <asp:DropDownList ID="ddlYearFrom" runat="server" style="width: 32%;">
        <asp:ListItem Text="Year" Value="0" />
    </asp:DropDownList>
</div>

<%--DateTo--%>
<div id="divDateTo_Change" runat="server" class="float-left div-form-field">
    <asp:Label ID="lblDateTo" runat="server" />
    <br />
    <asp:DropDownList ID="ddlDayTo" runat="server" style="width: 32%;">
        <asp:ListItem Text="Day" Value="0" />
    </asp:DropDownList>
    <asp:DropDownList ID="ddlMonthTo" runat="server" style="width: 32%;">
        <asp:ListItem Text="Month" Value="0" />
    </asp:DropDownList>
    <asp:DropDownList ID="ddlYearTo" runat="server" style="width: 32%;">
        <asp:ListItem Text="Year" Value="0" />
    </asp:DropDownList>
</div>

<div id="divCheckDate" runat="server" class="float-left div-form-field">
    <asp:RadioButtonList ID="lstDateMode" runat="server" CellPadding="0" CellSpacing="0">
        <asp:ListItem Value="month" Text="By Month" onclick="javascript: date_mode_change();" Selected="True" />
        <asp:ListItem Value="range" Text="Date Range" onclick="javascript: date_mode_change();" />
        <asp:ListItem Value="auto" Text="Auto Check Dates" onclick="javascript: date_mode_change();" />
    </asp:RadioButtonList>
</div>

<div class="clear-both"></div>

<div style="margin: 0px 10px;">
    <asp:CustomValidator ID="cvDateMode" runat="server" OnServerValidate="cvDateMode_ServerValidate" ClientValidationFunction="date_mode_validate" CssClass="error block" />
</div>

<div class="clear-both"></div>

<%--Template--%>
<div id="divTemplate_Change" runat="server" class="float-left div-form-field">
    <label for="<%= ddlTemplate.ClientID %>" class="block">Template</label>
    <asp:DropDownList ID="ddlTemplate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged" />
    <asp:RequiredFieldValidator ID="rfvTemplate" runat="server" ControlToValidate="ddlTemplate" InitialValue="0" ErrorMessage="Select 'Template'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--HeaderRowsCount--%>
<div id="divHeaderRowsCount_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtHeaderRowsCount.ClientID %>" class="block">Header Rows Count</label>
    <asp:TextBox ID="txtHeaderRowsCount" runat="server" MaxLength="2" />
    <asp:RequiredFieldValidator ID="rfvHeaderRowsCount" runat="server" ControlToValidate="txtHeaderRowsCount" ErrorMessage="Enter 'Header Rows Count'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    <asp:CompareValidator ID="cvHeaderRowsCount" runat="server" ControlToValidate="txtHeaderRowsCount" Operator="DataTypeCheck" Type="Integer" ErrorMessage="Rows count must be a whole number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    <asp:RangeValidator id="rvHeaderRowsCount" runat="server" ControlToValidate="txtHeaderRowsCount" MinimumValue="1" MaximumValue="99" Type="Integer" Text="Rows count must be greater than 0." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--UploadExcel--%>
<div id="divUploadExcel_Change" runat="server" class="float-left div-form-field">
    <label for="<%= fuUploadExcel.ClientID %>" class="block">Upload Excel - Formatted as text</label>
    <asp:FileUpload ID="fuUploadExcel" runat="server" EnableViewState="false" />
    <asp:RequiredFieldValidator ID="rfvUploadExcel" runat="server" ControlToValidate="fuUploadExcel" ErrorMessage="Select excel file." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<div class="clear-both"></div>

<%--Company--%>
<div id="divCompany_Change" runat="server" class="float-left div-form-field">
    <label for="<%= ddlCompany.ClientID %>" class="block">Company</label>
    <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
        <asp:ListItem Text="" Value="0" />
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="ddlCompany" InitialValue="0" ErrorMessage="Select 'Company'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--Card--%>
<div id="divCard_Change" runat="server" visible="false" class="float-left div-form-field">
    <label for="<%= ddlCard.ClientID %>" class="block">Card</label>
    <asp:DropDownList ID="ddlCard" runat="server" />
    <asp:RequiredFieldValidator ID="rfvCard" runat="server" ControlToValidate="ddlCard" InitialValue="0" ErrorMessage="Select 'Card'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<div class="clear-both"></div>

<div id="divCashBox" runat="server" visible="false" class="bg-gray">
    <%--Network--%>
    <asp:UpdatePanel ID="upNetwork" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divNetwork_Change" runat="server" visible="false" class="float-left div-form-field">
                <label for="<%= ddlNetwork.ClientID %>" class="block">Network</label>
                <asp:DropDownList ID="ddlNetwork" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNetwork_SelectedIndexChanged">
                    <asp:ListItem Text="" Value="0" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvNetwork" runat="server" ControlToValidate="ddlNetwork" InitialValue="0" ErrorMessage="Select 'Network'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlCompany" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <%--NetworkNumber--%>
    <div id="divNetworkNumber_Change" runat="server" visible="false" class="float-left div-form-field">
        <label for="<%= txtNetworkNumber.ClientID %>" class="block">Network Number</label>
        <asp:TextBox ID="txtNetworkNumber" runat="server" MaxLength="9" />
        <asp:RequiredFieldValidator ID="rfvNetworkNumber" runat="server" ControlToValidate="txtNetworkNumber" ErrorMessage="Enter 'Network Number'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--Branch--%>
    <asp:UpdatePanel ID="upBranch" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divBranch_Change" runat="server" visible="false" class="float-left div-form-field">
                <label for="<%= ddlBranch.ClientID %>" class="block">Branch</label>
                <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                    <asp:ListItem Text="" Value="0" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch" InitialValue="0" ErrorMessage="Select 'Branch'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlCompany" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlNetwork" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <%--BranchNumber--%>
    <div id="divBranchNumber_Change" runat="server" visible="false" class="float-left div-form-field">
        <label for="<%= txtBranchNumber.ClientID %>" class="block">Branch Number</label>
        <asp:TextBox ID="txtBranchNumber" runat="server" MaxLength="9" />
        <asp:RequiredFieldValidator ID="rfvBranchNumber" runat="server" ControlToValidate="txtBranchNumber" ErrorMessage="Enter 'Branch Number'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--CashBox--%>
    <asp:UpdatePanel ID="upCashBox" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divCashBox_Change" runat="server" visible="false" class="float-left div-form-field">
                <label for="<%= ddlCashBox.ClientID %>" class="block">CashBox</label>
                <asp:DropDownList ID="ddlCashBox" runat="server">
                    <asp:ListItem Text="" Value="0" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvCashBox" runat="server" ControlToValidate="ddlCashBox" InitialValue="0" ErrorMessage="Select 'CashBox'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlCompany" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlNetwork" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlBranch" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <%--CashBoxNumber--%>
    <div id="divCashBoxNumber_Change" runat="server" visible="false" class="float-left div-form-field">
        <label for="<%= txtCashBoxNumber.ClientID %>" class="block">Cashbox Number</label>
        <asp:TextBox ID="txtCashBoxNumber" runat="server" MaxLength="9" />
        <asp:RequiredFieldValidator ID="rfvCashBoxNumber" runat="server" ControlToValidate="txtCashBoxNumber" ErrorMessage="Enter 'Cashbox Number'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <div class="clear-both"></div>
</div>

<table style="margin: 5px;">
    <tr id="trStopIfCardIsEmpty" runat="server" visible="false">
        <td><asp:CheckBox ID="chkStopIfCardIsEmpty" runat="server" /></td>
        <td><label for="<%= chkStopIfCardIsEmpty.ClientID %>">Stop If Card Is Empty</label></td>
    </tr>
    <tr id="trStopIfTerminalIsZero" runat="server" visible="false">
        <td><asp:CheckBox ID="chkStopIfTerminalIsZero" runat="server" /></td>
        <td><label for="<%= chkStopIfTerminalIsZero.ClientID %>">Stop If Terminal Is Zero / Is Empty / Not Is Numeric</label></td>
    </tr>
    <tr id="trStopIfSupplierIsZero" runat="server" visible="false">
        <td><asp:CheckBox ID="chkStopIfSupplierIsZero" runat="server" /></td>
        <td><label for="<%= chkStopIfSupplierIsZero.ClientID %>">Stop If Supplier Is Zero / Is Empty / Not Is Numeric</label></td>
    </tr>
    <tr id="trStopIfTerminalNotExists" runat="server" visible="false">
        <td><asp:CheckBox ID="chkStopIfTerminalNotExists" runat="server" /></td>
        <td><label for="<%= chkStopIfTerminalNotExists.ClientID %>">Stop If Terminal Not Exists</label></td>
    </tr>
    <tr id="trStopIfSupplierNotExists" runat="server" visible="false">
        <td><asp:CheckBox ID="chkStopIfSupplierNotExists" runat="server" /></td>
        <td><label for="<%= chkStopIfSupplierNotExists.ClientID %>">Stop If Supplier Not Exists</label></td>
    </tr>
    <tr id="trStopIfAgreementNotExists" runat="server" visible="false">
        <td>
            <asp:CheckBox ID="chkStopIfAgreementNotExists" runat="server" AutoPostBack="true" OnCheckedChanged="chkStopIfAgreementNotExists_CheckedChanged" />
        </td>
        <td><label for="<%= chkStopIfAgreementNotExists.ClientID %>">Stop If Agreement Not Exists</label></td>
    </tr>
    <tr id="trRemoveEmptyRows" runat="server">
        <td>
            <asp:CheckBox ID="chkRemoveEmptyRows" runat="server" />
        </td>
        <td><label for="<%= chkRemoveEmptyRows.ClientID %>">Remove Empty Rows</label></td>
    </tr>
    <tr id="trRemoveDuplicateRows" runat="server">
        <td>
            <asp:CheckBox ID="chkRemoveDuplicateRows" runat="server" />
        </td>
        <td><label for="<%= chkRemoveDuplicateRows.ClientID %>">Remove Duplicate Rows</label></td>
    </tr>
</table>

<div id="divFileName" runat="server" visible="false" class="div-form-message">
    <div id="divDownload" style="margin: 5px;">
        <asp:LinkButton ID="btnDownloadOriginalData" runat="server" ValidationGroup="none" Text="Download Original Data" OnClick="Download_Excel" />
        &nbsp; | &nbsp;
        <asp:LinkButton ID="btnDownloadReceivedData" runat="server" ValidationGroup="none" Text="Download Received Data" OnClick="Download_Excel" />
    </div>

    <table class="table-list">
        <tr>
            <td class="bold">File Name</td>
            <td><asp:Label ID="lblFileName" runat="server" CssClass="read-only" /></td>
        </tr>
        <tr>
            <td class="bold">Upload Date</td>
            <td>
                <asp:Label ID="lblDateUpload" runat="server" CssClass="read-only" />
                <asp:Label ID="lblTimeUpload" runat="server" CssClass="read-only" />
            </td>
        </tr>
        <tr>
            <td class="bold">Execution Duration</td>
            <td>
                <asp:Label ID="lblExecutionDuration" runat="server" CssClass="read-only" />
            </td>
        </tr>
        <tr>
            <td class="bold">Data Rows Count</td>
            <td><asp:Label ID="lblDataRowsCount" runat="server" CssClass="read-only" /></td>
        </tr>
        <tr>
            <td class="bold">Splitted Rows Count</td>
            <td><asp:Label ID="lblSplittedRowsCount" runat="server" CssClass="read-only" /></td>
        </tr>
        <tr>
            <td class="bold">Excel Rows Count</td>
            <td><asp:Label ID="lblExcelRowsCount" runat="server" CssClass="read-only" /></td>
        </tr>
        <tr>
            <td class="bold">Excluded Rows Count</td>
            <td><asp:Label ID="lblExcludedRowsCount" runat="server" CssClass="read-only" /></td>
        </tr>
        <tr>
            <td class="bold">Data Total Amount</td>
            <td><asp:Label ID="lblDataMoneyAmount" runat="server" CssClass="read-only" /></td>
        </tr>
        <tr>
            <td class="bold">Excel Total Amount</td>
            <td><asp:Label ID="lblExcelMoneyAmount" runat="server" CssClass="read-only" /></td>
        </tr>
        <tr>
            <td class="bold">Excluded Total Amount</td>
            <td><asp:Label ID="lblExcludedMoneyAmount" runat="server" CssClass="read-only" /></td>
        </tr>
    </table>
</div>

<div>
    <div id="divCurrency" runat="server" visible="false" class="float-left" style="margin: 10px;">
        Currency in Template
        <asp:Repeater ID="repCurrency" runat="server" OnItemDataBound="repCurrency_ItemDataBound">
            <HeaderTemplate>
                <table class="table-sub">
                    <tr>
                        <th>Currency in XL</th>
                        <th>Currency in DB</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "CurrencyFromUser") %>
                            <asp:HiddenField ID="hidCurrencyFromUser" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CurrencyFromUser") %>' />
                        </td>
                        <td><asp:DropDownList ID="ddlCurrency" runat="server" /></td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div id="divCredit" runat="server" visible="false" class="float-left" style="margin: 10px;">
        Credit in Template
        <asp:UpdatePanel ID="upCredit" runat="server">
            <ContentTemplate>
                <asp:Repeater ID="repCredit" runat="server" OnItemDataBound="repCredit_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table-sub">
                            <tr>
                                <th>Credit in XL</th>
                                <th>Credit in DB</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                            <tr>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "CreditFromUser") %>
                                    <asp:HiddenField ID="hidCreditFromUser" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CreditFromUser") %>' />
                                </td>
                                <td><asp:DropDownList ID="ddlCredit" runat="server" /></td>
                            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="chkStopIfAgreementNotExists" EventName="CheckedChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <div id="divCard" runat="server" visible="false" class="float-left" style="margin: 10px;">
        Card in Template &nbsp;&mdash;&nbsp; <asp:CheckBox ID="chkIgnoreCard" runat="server" Text="Ignore" Font-Bold="true" />
        <asp:UpdatePanel ID="upCard" runat="server">
            <ContentTemplate>
                <asp:Repeater ID="repCard" runat="server" OnItemDataBound="repCard_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table-sub">
                            <tr>
                                <th>Card in XL</th>
                                <th>Card in DB</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                            <tr>
                                <td>
                                    <%# DataBinder.Eval(Container.DataItem, "CardFromUser") %>
                                    <asp:HiddenField ID="hidCardFromUser" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CardFromUser") %>' />
                                </td>
                                <td><asp:DropDownList ID="ddlCard" runat="server" /></td>
                            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="chkStopIfAgreementNotExists" EventName="CheckedChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <div id="divOperationType" runat="server" visible="false" class="float-left" style="margin: 10px;">
        Operation Type in Template
        <asp:Repeater ID="repOperationType" runat="server" OnItemDataBound="repOperationType_ItemDataBound">
            <HeaderTemplate>
                <table class="table-sub">
                    <tr>
                        <th>Operation Type in XL</th>
                        <th>Operation Type in DB</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "OperationTypeFromUser") %>
                            <asp:HiddenField ID="hidOperationTypeFromUser" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "OperationTypeFromUser") %>' />
                        </td>
                        <td><asp:DropDownList ID="ddlOperationType" runat="server" /></td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <%--
    <asp:Repeater ID="rep" runat="server">
        <HeaderTemplate>
            <table>
                <tr>
                    <th>CompanyID</th>
                    <th>TerminalNumber</th>
                    <th>TerminalNumber_Converted</th>
                    <th>SupplierNumber</th>
                    <th>SupplierNumber_Converted</th>
                    <th>CardID</th>
                    <th>CardBrand</th>
                    <th>CreditID</th>
                    <th>CreditBrand</th>
                    <th>OperationTypeID</th>
                    <th>OperationType</th>
                    <th>TransactionDate</th>
                    <th>TransactionDate_Converted</th>
                    <th>PaymentsCount</th>
                    <th>PaymentsCount_Converted</th>
                    <th>Error</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "CompanyID") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "TerminalNumber") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "TerminalNumber_Converted") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "SupplierNumber") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "SupplierNumber_Converted") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "CardID") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "CardBrand") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "CreditID") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "CreditBrand") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "OperationTypeID") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "OperationType") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "TransactionDate") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "TransactionDate_Converted") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "PaymentsCount") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "PaymentsCount_Converted") %></td>
                    <td class="error"><%# DataBinder.Eval(Container.DataItem, "Error") %></td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    --%>

    <div id="divAgreementFromCashBox" runat="server" visible="false" class="float-left" style="margin: 10px;">
        <span class="error">Error on validate related agreement items from cashbox</span>

        <asp:Repeater ID="repAgreementFromCashBox" runat="server" OnItemDataBound="repAgreementFromCashBox_ItemDataBound">
            <HeaderTemplate>
                <table class="table-sub">
                    <tr>
                        <th>Company</th>
                        <th>Network</th>
                        <th>Branch</th>
                        <th>CashBox</th>
                        <th>Card</th>
                        <th>Date</th>
                        <th>Payments</th>
                        <%--
                        <th id="thGroup" runat="server">Group</th>
                        <th id="thSupplier" runat="server">Supplier</th>
                        <th id="thTerminal" runat="server">Terminal</th>
                        <th id="thAgreement" runat="server">Agreement</th>
                        --%>
                        <th>Error</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td><asp:Label ID="lblCompany" runat="server" /></td>
                        <td><asp:Label ID="lblNetwork" runat="server" /></td>
                        <td><asp:Label ID="lblBranch" runat="server" /></td>
                        <td><asp:Label ID="lblCashBox" runat="server" /></td>
                        <td><asp:Label ID="lblCard" runat="server" /></td>
                        <td><asp:Label ID="lblDate" runat="server" /></td>
                        <td><asp:Label ID="lblPayments" runat="server" /></td>
                        <%--
                        <td id="tdGroup" runat="server"><asp:Label ID="lblGroup" runat="server" /></td>
                        <td id="tdSupplier" runat="server"><asp:Label ID="lblSupplier" runat="server" /></td>
                        <td id="tdTerminal" runat="server"><asp:Label ID="lblTerminal" runat="server" /></td>
                        <td id="tdAgreement" runat="server"><asp:Label ID="lblAgreement" runat="server" /></td>
                        --%>
                        <td class="error"><%# DataBinder.Eval(Container.DataItem, "Error") %></td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div id="divAgreementFromTerminal" runat="server" visible="false" class="float-left" style="margin: 10px;">
        <span class="error">Error on validate related agreement items from terminal</span>

        <asp:Repeater ID="repAgreementFromTerminal" runat="server" OnItemDataBound="repAgreementFromTerminal_ItemDataBound">
            <HeaderTemplate>
                <table class="table-sub">
                    <tr>
                        <th>Terminal</th>
                        <th>Supplier</th>
                        <th>Card</th>
                        <th>Credit</th>
                        <th>Operation</th>
                        <th>Date</th>
                        <th>Payments</th>
                        <%--
                        <th id="thGroup" runat="server">Group</th>
                        <th id="thSupplier" runat="server">Supplier</th>
                        <th id="thTerminal" runat="server">Terminal</th>
                        <th id="thAgreement" runat="server">Agreement</th>
                        --%>
                        <th>Error</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td><asp:Label ID="lblTerminal" runat="server" /></td>
                        <td><asp:Label ID="lblSupplier" runat="server" /></td>
                        <td><asp:Label ID="lblCard" runat="server" /></td>
                        <td><asp:Label ID="lblCredit" runat="server" /></td>
                        <td><asp:Label ID="lblOperation" runat="server" /></td>
                        <td><asp:Label ID="lblDate" runat="server" /></td>
                        <td><asp:Label ID="lblPayments" runat="server" /></td>
                        <%--
                        <td id="tdGroup" runat="server"><asp:Label ID="lblGroup" runat="server" /></td>
                        <td id="tdSupplier" runat="server"><asp:Label ID="lblSupplier" runat="server" /></td>
                        <td id="tdTerminal" runat="server"><asp:Label ID="lblTerminal" runat="server" /></td>
                        <td id="tdAgreement" runat="server"><asp:Label ID="lblAgreement" runat="server" /></td>
                        --%>
                        <td class="error"><%# DataBinder.Eval(Container.DataItem, "Error") %></td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div class="clear-both"></div>
</div>

<div class="div-form-command">
    <asp:Button ID="btnSaveBottom" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="javascript: show_wait();" />
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
    Status:
    <asp:Label ID="lblStatusBottom" runat="server" Text="New" />
</div>

<script>
    date_mode_change();

    function date_mode_change() {
        var b_month = document.getElementById("<%= lstDateMode.ClientID %>_0").checked;
        var b_range = document.getElementById("<%= lstDateMode.ClientID %>_1").checked;
        var b_auto = document.getElementById("<%= lstDateMode.ClientID %>_2").checked;

        var ddl_day_from = document.getElementById("<%= ddlDayFrom.ClientID %>");
        var ddl_month_from = document.getElementById("<%= ddlMonthFrom.ClientID %>");
        var ddl_year_from = document.getElementById("<%= ddlYearFrom.ClientID %>");

        var ddl_day_to = document.getElementById("<%= ddlDayTo.ClientID %>");
        var ddl_month_to = document.getElementById("<%= ddlMonthTo.ClientID %>");
        var ddl_year_to = document.getElementById("<%= ddlYearTo.ClientID %>");

        var s_date_from = "", s_date_to = "";
        var b_day_from_disabled = true, b_month_from_disabled = true, b_year_from_disabled = true, b_day_to_disabled = true, b_month_to_disabled = true, b_year_to_disabled = true;

        if (b_month) {
            s_date_from = "Month & Year";

            b_month_from_disabled = false;
            b_year_from_disabled = false;
        }
        else if (b_range) {
            s_date_from = "Date From";
            s_date_to = "Date To";

            b_day_from_disabled = false;
            b_month_from_disabled = false;
            b_year_from_disabled = false;

            b_day_to_disabled = false;
            b_month_to_disabled = false;
            b_year_to_disabled = false;
        }

        ddl_day_from.disabled = b_day_from_disabled;
        ddl_day_from.className = (b_day_from_disabled ? "bg-gray" : "");

        ddl_month_from.disabled = b_month_from_disabled;
        ddl_month_from.className = (b_month_from_disabled ? "bg-gray" : "");

        ddl_year_from.disabled = b_year_from_disabled;
        ddl_year_from.className = (b_year_from_disabled ? "bg-gray" : "");

        ddl_day_to.disabled = b_day_to_disabled;
        ddl_day_to.className = (b_day_to_disabled ? "bg-gray" : "");

        ddl_month_to.disabled = b_month_to_disabled;
        ddl_month_to.className = (b_month_to_disabled ? "bg-gray" : "");

        ddl_year_to.disabled = b_year_to_disabled;
        ddl_year_to.className = (b_year_to_disabled ? "bg-gray" : "");

        document.getElementById("<%= lblDateFrom.ClientID %>").innerHTML = s_date_from;
        document.getElementById("<%= lblDateTo.ClientID %>").innerHTML = s_date_to;
    }

    function date_mode_validate(source, args) {
        var b_valid = true;

        var b_month = document.getElementById("<%= lstDateMode.ClientID %>_0").checked;
        var b_range = document.getElementById("<%= lstDateMode.ClientID %>_1").checked;
        var b_auto = document.getElementById("<%= lstDateMode.ClientID %>_2").checked;

        if (b_month == true) {
            var n_month = parseInt(document.getElementById("<%= ddlMonthFrom.ClientID %>").value);
            var n_year = parseInt(document.getElementById("<%= ddlYearFrom.ClientID %>").value);

            if (n_month == 0 || n_year == 0) {
                b_valid = false;
                source.innerHTML = "Select 'Month' and 'Year'.";

                if (n_month == 0) {
                    document.getElementById("<%= ddlMonthFrom.ClientID %>").focus();
                }
                else if (n_year == 0) {
                    document.getElementById("<%= ddlYearFrom.ClientID %>").focus();
                }
            }
        }
        else if (b_range == true) {
            var n_day_from = parseInt(document.getElementById("<%= ddlDayFrom.ClientID %>").value);
            var n_month_from = parseInt(document.getElementById("<%= ddlMonthFrom.ClientID %>").value);
            var n_year_from = parseInt(document.getElementById("<%= ddlYearFrom.ClientID %>").value);
            var n_day_to = parseInt(document.getElementById("<%= ddlDayTo.ClientID %>").value);
            var n_month_to = parseInt(document.getElementById("<%= ddlMonthTo.ClientID %>").value);
            var n_year_to = parseInt(document.getElementById("<%= ddlYearTo.ClientID %>").value);

            if (n_day_from == 0 || n_month_from == 0 || n_year_from == 0 || n_day_to == 0 || n_month_to == 0 || n_year_to == 0) {
                b_valid = false;
                source.innerHTML = "Select 'Date From' and 'Date To'.";

                if (n_day_from == 0) {
                    document.getElementById("<%= ddlDayFrom.ClientID %>").focus();
                }
                else if (n_month_from == 0) {
                    document.getElementById("<%= ddlMonthFrom.ClientID %>").focus();
                }
                else if (n_year_from == 0) {
                    document.getElementById("<%= ddlYearFrom.ClientID %>").focus();
                }
                else if (n_day_to == 0) {
                    document.getElementById("<%= ddlDayTo.ClientID %>").focus();
                }
                else if (n_month_to == 0) {
                    document.getElementById("<%= ddlMonthTo.ClientID %>").focus();
                }
                else if (n_year_to == 0) {
                    document.getElementById("<%= ddlYearTo.ClientID %>").focus();
                }
            }
        }

        args.IsValid = b_valid;

        if (args.IsValid == false) { source.style.visibility = "visible"; }
    }

    function show_wait() {
        if (Page_ClientValidate() == true) {
            document.getElementById("<%= lblError.ClientID %>").innerHTML = "";
            document.getElementById("<%= lblMessage.ClientID %>").innerHTML = "Please wait..";

            document.getElementById("<%= btnSaveTop.ClientID %>").style.visibility = "hidden";
            document.getElementById("<%= btnSaveBottom.ClientID %>").style.visibility = "hidden";
        }
    }
</script>
