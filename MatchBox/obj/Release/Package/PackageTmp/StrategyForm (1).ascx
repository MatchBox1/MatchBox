<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StrategyForm.ascx.cs" Inherits="MatchBox.StrategyForm" %>

<div class="div-form-item">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<div class="div-form-command">
    <asp:Button ID="btnSaveTop" runat="server" Text="Save" OnClick="Save_Click" />
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
    Status:
    <asp:Label ID="lblStatusTop" runat="server" />
</div>

<%--StrategyName--%>
<div id="divStrategyName_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtStrategyName.ClientID %>" class="block">Strategy Name</label>
    <asp:TextBox ID="txtStrategyName" runat="server" MaxLength="50" />
    <asp:RequiredFieldValidator ID="rfvStrategyName" runat="server" ControlToValidate="txtStrategyName" ErrorMessage="Enter 'Strategy Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--QueryPercent--%>
<div id="divQueryPercent_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtQueryPercent.ClientID %>" class="block">Queries Percent</label>
    <asp:TextBox ID="txtQueryPercent" runat="server" MaxLength="5" Width="250px" /> %
    <asp:CompareValidator ID="cvQueryPercent" runat="server" ControlToValidate="txtQueryPercent" Operator="DataTypeCheck" Type="Double" ErrorMessage="'Runs Percent' must be a number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<div class="float-left div-form-field" style="padding: 10px;">
    <asp:Label ID="lblQueryInfo" runat="server" />
</div>

<div class="clear-both"></div>

<asp:CustomValidator ID="cvTemplate" runat="server" OnServerValidate="cvTemplate_ServerValidate" />

<div style="margin: 10px;">
    <b>Strategy Templates</b>
    <asp:Repeater ID="repTemplate" runat="server" OnItemDataBound="repTemplate_ItemDataBound">
        <HeaderTemplate>
            <table class="table-sub">
                <tr>
                    <th>#</th>
                    <th>Source</th>
                    <th>Template</th>
                    <th>Date From</th>
                    <th>Date To</th>
                    <th><asp:Label ID="lblTerminalNumber" runat="server" /></th>
                    <th><asp:Label ID="lblSupplierNumber" runat="server" /></th>
                    <th><asp:Label ID="lblCreditBrand" runat="server" /></th>
                    <th><asp:Label ID="lblCardBrand" runat="server" /></th>
                    <th><asp:Label ID="lblTransactionCurrency" runat="server" /></th>
                    <th><asp:Label ID="lblCardNumber" runat="server" /></th>
                    <th><asp:Label ID="lblPaymentsCount" runat="server" /></th>
                    <th><asp:Label ID="lblDutyPaymentNumber" runat="server" /></th>
                    <th><asp:Label ID="lblTransmissionNumber" runat="server" /></th>
                    <th><asp:Label ID="lblTransactionGrossAmount" runat="server" /></th>
                    <th><asp:Label ID="lblDutyPaymentAmount" runat="server" /></th>
                    <th><asp:Label ID="lblTransactionDate" runat="server" /></th>
                    <th><asp:Label ID="lblPaymentDate" runat="server" /></th>
                    <th><asp:Label ID="lblVoucherNumber" runat="server" /></th>
                    <th><asp:Label ID="lblConfirmationNumber" runat="server" /></th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td>
                        <asp:HiddenField ID="hidTemplateID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                        <asp:CheckBox ID="chkIsSelected" runat="server" />
                    </td>
                    <td class="nowrap"><asp:Label ID="lblDataSource" AssociatedControlID="chkIsSelected" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DataSource") %>' /></td>
                    <td class="nowrap"><asp:Label ID="lblTemplateName" AssociatedControlID="chkIsSelected" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TemplateName") %>' /></td>
                    <td class="nowrap"><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateFromMin")) %></td>
                    <td class="nowrap"><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateToMax")) %></td>
                    <td class="text-center"><asp:CheckBox ID="chkTerminalNumber" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkSupplierNumber" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkCreditBrand" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkCardBrand" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkTransactionCurrency" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkCardNumber" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkPaymentsCount" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkDutyPaymentNumber" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkTransmissionNumber" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkTransactionGrossAmount" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkDutyPaymentAmount" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkTransactionDate" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkPaymentDate" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkVoucherNumber" runat="server" Enabled="false" /></td>
                    <td class="text-center"><asp:CheckBox ID="chkConfirmationNumber" runat="server" Enabled="false" /></td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>

<div id="divField" runat="server" visible="false" style="margin: 10px;">
    <br />
    <b>Strategy Fields</b> ( Priority: Up to <asp:Label ID="lblMaxPriority" runat="server" Font-Bold="true" /> fields / Tolerance: Up to <asp:Label ID="lblMaxTolerance" runat="server" Font-Bold="true" /> fields )
    <table id="tblField" runat="server" class="table-sub">
        <tr>
            <td class="bg-gray nowrap bold">Field Name:</td>
            <th><asp:Label ID="lblTerminalNumber" runat="server" /></th>
            <th><asp:Label ID="lblSupplierNumber" runat="server" /></th>
            <th><asp:Label ID="lblCreditBrand" runat="server" /></th>
            <th><asp:Label ID="lblCardBrand" runat="server" /></th>
            <th><asp:Label ID="lblTransactionCurrency" runat="server" /></th>
            <th><asp:Label ID="lblCardNumber" runat="server" /></th>
            <th><asp:Label ID="lblPaymentsCount" runat="server" /></th>
            <th><asp:Label ID="lblDutyPaymentNumber" runat="server" /></th>
            <th><asp:Label ID="lblTransmissionNumber" runat="server" /></th>
            <th><asp:Label ID="lblTransactionGrossAmount" runat="server" /></th>
            <th><asp:Label ID="lblDutyPaymentAmount" runat="server" /></th>
            <th><asp:Label ID="lblTransactionDate" runat="server" /></th>
            <th><asp:Label ID="lblPaymentDate" runat="server" /></th>
            <th><asp:Label ID="lblVoucherNumber" runat="server" /></th>
            <th><asp:Label ID="lblConfirmationNumber" runat="server" /></th>
        </tr>
        <tr>
            <td class="bg-gray nowrap bold">Included:</td>
            <td class="text-center"><asp:CheckBox ID="chkTerminalNumber" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkSupplierNumber" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkCreditBrand" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkCardBrand" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkTransactionCurrency" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkCardNumber" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkPaymentsCount" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkDutyPaymentNumber" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkTransmissionNumber" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkTransactionGrossAmount" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkDutyPaymentAmount" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkTransactionDate" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkPaymentDate" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkVoucherNumber" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
            <td class="text-center"><asp:CheckBox ID="chkConfirmationNumber" runat="server" onclick="javascript: enable_textbox(this.id, this.checked);" /></td>
        </tr>
        <tr>
            <td class="bg-gray nowrap bold">Priority:</td>
            <td class="text-center"><asp:TextBox ID="txtPriorityTerminalNumber" runat="server" Width="40px" MaxLength="2" /></td>
            <td class="text-center"><asp:TextBox ID="txtPrioritySupplierNumber" runat="server" Width="40px" MaxLength="2" /></td>
            <td class="text-center"><asp:TextBox ID="txtPriorityCreditBrand" runat="server" Width="40px" MaxLength="2" /></td>
            <td class="text-center"><asp:TextBox ID="txtPriorityCardBrand" runat="server" Width="40px" MaxLength="2" /></td>
            <td class="text-center"></td>
            <td class="text-center"></td>
            <td class="text-center"><asp:TextBox ID="txtPriorityPaymentsCount" runat="server" Width="40px" MaxLength="2" /></td>
            <td class="text-center"><asp:TextBox ID="txtPriorityDutyPaymentNumber" runat="server" Width="40px" MaxLength="2" /></td>
            <td class="text-center"><asp:TextBox ID="txtPriorityTransmissionNumber" runat="server" Width="40px" MaxLength="2" /></td>
            <td class="text-center"><asp:TextBox ID="txtPriorityTransactionGrossAmount" runat="server" Width="40px" MaxLength="2" /></td>
            <td class="text-center"></td>
            <td class="text-center"></td>
            <td class="text-center"></td>
            <td class="text-center"><asp:TextBox ID="txtPriorityVoucherNumber" runat="server" Width="40px" MaxLength="2" /></td>
            <td class="text-center"><asp:TextBox ID="txtPriorityConfirmationNumber" runat="server" Width="40px" MaxLength="2" /></td>
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
            <td class="text-center">
                <asp:TextBox ID="txtToleranceTransactionGrossAmount_1" runat="server" Width="40px" MaxLength="5" />
                <br />
                <asp:TextBox ID="txtToleranceTransactionGrossAmount_2" runat="server" Width="40px" MaxLength="5" style="margin: 5px 0px;" />
                <br />
                <asp:TextBox ID="txtToleranceTransactionGrossAmount_3" runat="server" Width="40px" MaxLength="5" />
            </td>
            <td class="text-center">
                <asp:TextBox ID="txtToleranceDutyPaymentAmount_1" runat="server" Width="40px" MaxLength="5" />
                <br />
                <asp:TextBox ID="txtToleranceDutyPaymentAmount_2" runat="server" Width="40px" MaxLength="5" style="margin: 5px 0px;" />
                <br />
                <asp:TextBox ID="txtToleranceDutyPaymentAmount_3" runat="server" Width="40px" MaxLength="5" />
            </td>
            <td class="text-center">
                <asp:TextBox ID="txtToleranceTransactionDate_1" runat="server" Width="40px" MaxLength="3" />
                <br />
                <asp:TextBox ID="txtToleranceTransactionDate_2" runat="server" Width="40px" MaxLength="3" style="margin: 5px 0px;" />
                <br />
                <asp:TextBox ID="txtToleranceTransactionDate_3" runat="server" Width="40px" MaxLength="3" />
            </td>
            <td class="text-center">
                <asp:TextBox ID="txtTolerancePaymentDate_1" runat="server" Width="40px" MaxLength="3" />
                <br />
                <asp:TextBox ID="txtTolerancePaymentDate_2" runat="server" Width="40px" MaxLength="3" style="margin: 5px 0px;" />
                <br />
                <asp:TextBox ID="txtTolerancePaymentDate_3" runat="server" Width="40px" MaxLength="3" />
            </td>
            <td class="text-center">
                <asp:TextBox ID="txtToleranceVoucherNumber_1" runat="server" Width="40px" MaxLength="1" />
                <br />
                <asp:TextBox ID="txtToleranceVoucherNumber_2" runat="server" Width="40px" MaxLength="1" style="margin: 5px 0px;" />
                <br />
                <asp:TextBox ID="txtToleranceVoucherNumber_3" runat="server" Width="40px" MaxLength="1" />
            </td>
            <td class="text-center">
                <asp:TextBox ID="txtToleranceConfirmationNumber_1" runat="server" Width="40px" MaxLength="1" />
                <br />
                <asp:TextBox ID="txtToleranceConfirmationNumber_2" runat="server" Width="40px" MaxLength="1" style="margin: 5px 0px;" />
                <br />
                <asp:TextBox ID="txtToleranceConfirmationNumber_3" runat="server" Width="40px" MaxLength="1" />
            </td>
        </tr>
    </table>

    <script>
        function enable_textbox(s_id, b_checked) {
            var s_priority_id = s_id.replace("chk", "txtPriority");
            var s_tolerance_id_1 = s_id.replace("chk", "txtTolerance") + "_1";
            var s_tolerance_id_2 = s_id.replace("chk", "txtTolerance") + "_2";
            var s_tolerance_id_3 = s_id.replace("chk", "txtTolerance") + "_3";

            var txt_priority = document.getElementById(s_priority_id);
            var txt_tolerance_1 = document.getElementById(s_tolerance_id_1);
            var txt_tolerance_2 = document.getElementById(s_tolerance_id_2);
            var txt_tolerance_3 = document.getElementById(s_tolerance_id_3);

            if (txt_priority == null && txt_tolerance_1 == null && txt_tolerance_2 == null && txt_tolerance_3 == null) { return; }

            if (txt_priority != null) {
                if (b_checked == true) {
                    txt_priority.disabled = false;
                }
                else {
                    txt_priority.disabled = true;
                    txt_priority.value = "";
                }
            }

            if (txt_tolerance_1 != null) {
                if (b_checked == true) {
                    txt_tolerance_1.disabled = false;
                }
                else {
                    txt_tolerance_1.disabled = true;
                    txt_tolerance_1.value = "";
                }
            }

            if (txt_tolerance_2 != null) {
                if (b_checked == true) {
                    txt_tolerance_2.disabled = false;
                }
                else {
                    txt_tolerance_2.disabled = true;
                    txt_tolerance_2.value = "";
                }
            }

            if (txt_tolerance_3 != null) {
                if (b_checked == true) {
                    txt_tolerance_3.disabled = false;
                }
                else {
                    txt_tolerance_3.disabled = true;
                    txt_tolerance_3.value = "";
                }
            }
        }
    </script>
</div>

<div class="div-form-command">
    <asp:Button ID="btnSaveBottom" runat="server" Text="Save" OnClick="Save_Click" />
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
    Status:
    <asp:Label ID="lblStatusBottom" runat="server" />
</div>

<hr />

<div id="divQuery" runat="server" visible="false" style="margin: 10px;">
    <br />
    <b>Strategy Queries</b>
    <asp:GridView ID="gvQuery" runat="server" AutoGenerateColumns="true" CssClass="table-center" />

    <script>
        document.getElementById("<%= gvQuery.ClientID %>").attributes.removeNamedItem("style");
    </script>
</div>
