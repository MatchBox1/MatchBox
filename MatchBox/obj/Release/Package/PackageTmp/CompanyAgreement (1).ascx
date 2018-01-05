<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyAgreement.ascx.cs" Inherits="MatchBox.CompanyAgreement" %>

<div style="margin: 10px;">
    <b>Number</b> : <asp:Label ID="lblCompanyNumber" runat="server" />
    &nbsp; &nbsp; &nbsp;
    <b>Name</b> : <asp:Label ID="lblCompanyName" runat="server" />
</div>

<hr style="margin: 10px;" />

<div style="margin: 10px;">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<asp:Repeater ID="repAgreement" runat="server" OnItemDataBound="repAgreement_ItemDataBound">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
            <li>
                <b><%# DataBinder.Eval(Container.DataItem, "CreditName") %></b>

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
                                                <td><%# DataBinder.Eval(Container.DataItem, "CardName") %></td>
                                                <td><%# DataBinder.Eval(Container.DataItem, "OperationTypeName") %></td>
                                                <td><%# DataBinder.Eval(Container.DataItem, "CommissionLocal") %>%</td>
                                                <td><%# DataBinder.Eval(Container.DataItem, "CommissionAbroad") %>%</td>
                                                <td>
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
                                                </td>
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
            </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>