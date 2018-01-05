<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CashBoxPeriod.ascx.cs" Inherits="MatchBox.CashBoxPeriod" %>

<table>
    <tr>
        <td class="nowrap" style="padding: 0px 10px;"><b>Number</b> : <asp:Label ID="lblCashBoxNumber" runat="server" /></td>
        <td class="nowrap" style="padding: 0px 10px;">&nbsp;</td>
        <td class="nowrap" style="padding: 0px 10px;"><b>Name</b> : <asp:Label ID="lblCashBoxName" runat="server" /></td>
        <td class="nowrap" style="padding: 0px 10px; width: 100%;">&nbsp;</td>
        <td class="nowrap" style="padding: 0px 10px;"><asp:LinkButton ID="btnAddPeriod" runat="server" OnCommand="Edit_Period" CommandArgument="0" Text="Add Period" /></td>
    </tr>
</table>

<hr style="margin: 10px;" />

<div style="margin: 10px;">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<asp:HiddenField ID="hidCashBoxPeriodID" runat="server" Value="0" />

<%--Period--%>
<div id="divPeriodForm" runat="server" visible="false" style="margin: 10px;">
    <div class="nowrap">
        <asp:HiddenField ID="hidTerminalID" runat="server" Value="0" />

        <label for="<%= txtDateStart.ClientID %>" class="bold">Start Date</label>
        <asp:TextBox ID="txtDateStart" runat="server" MaxLength="10" Width="100px" placeholder="dd/mm/yyyy" />
        &nbsp;
        <label for="<%= txtDateEnd.ClientID %>" class="bold">End Date</label>
        <asp:TextBox ID="txtDateEnd" runat="server" MaxLength="10" Width="100px" placeholder="dd/mm/yyyy" />
        &nbsp;
        <b>Terminal</b> <asp:TextBox ID="txtTerminal" runat="server" ReadOnly="true" Width="200px" />
        &nbsp;
        <asp:Button ID="btnSavePeriod" runat="server" ValidationGroup="Period" OnClick="btnSavePeriod_Click" Text="Save" />
        &nbsp;
        <asp:LinkButton ID="btnClosePeriod" runat="server" OnClick="Close_Form" Text="Close" />
        <br />
    </div>

    <div style="margin: 10px 0px;">
        <asp:CustomValidator ID="cvDateRange" runat="server" ValidationGroup="Period" OnServerValidate="cvDateRange_ServerValidate" Display="Dynamic" CssClass="error block" />
        <asp:RequiredFieldValidator ID="rfvDateStart" runat="server" ValidationGroup="Period" ControlToValidate="txtDateStart" ErrorMessage="Enter 'Start Date'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <hr />
</div>

<%--Terminal--%>
<div id="divTerminalForm" runat="server" visible="false" style="margin: 10px;">
    <div class="nowrap">
        <b>Start</b> <asp:Label ID="lblDateStart" runat="server" BorderStyle="Solid" BorderColor="#cccccc" BorderWidth="1px" style="padding: 5px;" />
        &nbsp;
        <b>End</b> <asp:Label ID="lblDateEnd" runat="server" BorderStyle="Solid" BorderColor="#cccccc" BorderWidth="1px" style="padding: 5px;" />
        &nbsp;
        <asp:DropDownList ID="ddlTerminal" runat="server" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlTerminal_SelectedIndexChanged" />
        &nbsp;
        <asp:Button ID="btnSaveCashBoxPeriodTerminal" runat="server" ValidationGroup="Terminal" OnClick="btnSaveCashBoxPeriodTerminal_Click" Text="Save" />
        &nbsp;
        <asp:LinkButton ID="btnCloseTerminal" runat="server" OnClick="Close_Form" Text="Close" />
    </div>

    <div style="margin: 10px 0px;">
        <asp:RequiredFieldValidator ID="rfvTerminal" runat="server" ValidationGroup="Terminal" ControlToValidate="ddlTerminal" InitialValue="0" ErrorMessage="Select 'Terminal'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <asp:UpdatePanel runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Repeater ID="repAgreementItemTerminal" runat="server">
                <HeaderTemplate>
                    <table class="table-sub">
                        <tr>
                            <th rowspan="2">#</th>
                            <th rowspan="2">Start Date</th>
                            <th rowspan="2">End Date</th>
                            <th colspan="2">Payments</th>
                            <th rowspan="2">Credit</th>
                            <th rowspan="2">Card</th>
                            <th rowspan="2">Operation Type</th>
                            <th colspan="2">Commission</th>
                            <th rowspan="2">Group</th>
                            <th rowspan="2">Supplier</th>
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
                            <td>
                                <asp:CheckBox ID="chkIsSelected" runat="server" Checked="true" />
                                <%# DataBinder.Eval(Container.DataItem, "ID") %>
                                <asp:HiddenField ID="hidID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                            </td>
                            <td><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateStart")) %></td>
                            <td><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateEnd")) %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "PaymentsFrom") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "PaymentsTo") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "CreditName") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "CardName") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "OperationTypeName") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "CommissionLocal") %>%</td>
                            <td><%# DataBinder.Eval(Container.DataItem, "CommissionAbroad") %>%</td>
                            <td><%# DataBinder.Eval(Container.DataItem, "SupplierGroupNumber") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "SupplierNumber") %></td>
                        </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlTerminal" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <hr />
</div>

<asp:Repeater ID="repCashBoxPeriod" runat="server" OnItemDataBound="repCashBoxPeriod_ItemDataBound">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
            <li>
                <b>Start Date</b> <asp:Label ID="lblDateStart" runat="server" BorderStyle="Solid" BorderColor="#cccccc" BorderWidth="1px" style="padding: 5px;" />
                &nbsp;
                <b>End Date</b> <asp:Label ID="lblDateEnd" runat="server" BorderStyle="Solid" BorderColor="#cccccc" BorderWidth="1px" style="padding: 5px;" />
                &nbsp;
                <b>Terminal</b> <asp:Label ID="lblTerminal" runat="server" BorderStyle="Solid" BorderColor="#cccccc" BorderWidth="1px" style="padding: 5px;" />
                <br />
                <asp:LinkButton ID="btnEditPeriod" runat="server" OnCommand="Edit_Period" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="Edit" />
                |
                <asp:LinkButton ID="btnAddTerminal" runat="server" OnCommand="Add_Terminal" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="Add Terminal" />
                |
                <asp:LinkButton ID="btnDeletePeriod" runat="server" OnCommand="btnDeletePeriod_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="Delete" OnClientClick="javascript: return confirm('Delete Item?');" ForeColor="Red" />

                <div style="margin: 10px 0px;">
                    <asp:Repeater ID="repCashBoxPeriodTerminal" runat="server">
                        <HeaderTemplate>
                            <table class="table-sub">
                                <tr>
                                    <th rowspan="2">Start Date</th>
                                    <th rowspan="2">End Date</th>
                                    <th colspan="2">Payments</th>
                                    <th rowspan="2">Credit</th>
                                    <th rowspan="2">Card</th>
                                    <th rowspan="2">Operation Type</th>
                                    <th colspan="2">Commission</th>
                                    <th rowspan="2">Group</th>
                                    <th rowspan="2">Supplier</th>
                                    <th rowspan="2">#</th>
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
                                    <td><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateStart")) %></td>
                                    <td><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateEnd")) %></td>
                                    <td><%# DataBinder.Eval(Container.DataItem, "PaymentsFrom") %></td>
                                    <td><%# DataBinder.Eval(Container.DataItem, "PaymentsTo") %></td>
                                    <td><%# DataBinder.Eval(Container.DataItem, "CreditName") %></td>
                                    <td><%# DataBinder.Eval(Container.DataItem, "CardName") %></td>
                                    <td><%# DataBinder.Eval(Container.DataItem, "OperationTypeName") %></td>
                                    <td><%# DataBinder.Eval(Container.DataItem, "CommissionLocal") %>%</td>
                                    <td><%# DataBinder.Eval(Container.DataItem, "CommissionAbroad") %>%</td>
                                    <td><%# DataBinder.Eval(Container.DataItem, "SupplierGroupNumber") %></td>
                                    <td><%# DataBinder.Eval(Container.DataItem, "SupplierNumber") %></td>
                                    <td><asp:LinkButton ID="btnDeleteCashBoxPeriodTerminal" runat="server" OnCommand="btnDeleteCashBoxPeriodTerminal_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") + "," + DataBinder.Eval(Container.DataItem, "CashBoxPeriodID") %>' Text="x" OnClientClick="javascript: return confirm('Delete Item?');" ToolTip="Delete" ForeColor="Red" CssClass="no-line" /></td>
                                </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
