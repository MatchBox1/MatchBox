<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgreementPayment.ascx.cs" Inherits="MatchBox.AgreementPayment" %>

<table>
    <tr>
        <td class="nowrap" style="padding: 0px 10px;"><b>Company</b> : <asp:Label ID="lblCompanyName" runat="server" /></td>
        <td class="nowrap" style="padding: 0px 10px;">&nbsp;</td>
        <td class="nowrap" style="padding: 0px 10px;"><b>Credit</b> : <asp:Label ID="lblCreditName" runat="server" /></td>
        <td class="nowrap" style="padding: 0px 10px; width: 100%;">&nbsp;</td>
        <td class="nowrap" style="padding: 0px 10px;"><asp:LinkButton ID="btnAddPayment" runat="server" OnCommand="Edit_Payment" CommandArgument="0" Text="Add Payment Settings" /></td>
    </tr>
</table>

<hr style="margin: 10px;" />

<div style="margin: 10px;">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<div id="divAgreementPaymentForm" runat="server" visible="false">
    <div class="div-form-command">
        <asp:Button ID="btnSaveTop" runat="server" ValidationGroup="AgreementPayment" Text="Save" OnClick="Save_Payment" />
        &nbsp;
        <a id="lnkCloseTop" runat="server">Close</a>
    </div>

    <%--Card--%>
    <div id="divCard_Change" runat="server" class="float-left div-form-field">
        <label for="<%= ddlCard.ClientID %>" class="block">Card</label>
        <asp:DropDownList ID="ddlCard" runat="server">
            <asp:ListItem Text="" Value="0" />
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvCard" runat="server" ValidationGroup="AgreementPayment" ControlToValidate="ddlCard" InitialValue="0" ErrorMessage="Select 'Card'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--DateStart--%>
    <div id="divDateStart_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtDateStart.ClientID %>" class="block">Start Date</label>
        <asp:TextBox ID="txtDateStart" runat="server" MaxLength="10" placeholder="dd/mm/yyyy" />
        <asp:RequiredFieldValidator ID="rfvDateStart" runat="server" ValidationGroup="AgreementPayment" ControlToValidate="txtDateStart" ErrorMessage="Enter 'Start Date'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--DateEnd--%>
    <div id="divDateEnd_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtDateEnd.ClientID %>" class="block">End Date</label>
        <asp:TextBox ID="txtDateEnd" runat="server" MaxLength="10" placeholder="dd/mm/yyyy" />
        <asp:RequiredFieldValidator ID="rfvDateEnd" runat="server" ValidationGroup="AgreementPayment" ControlToValidate="txtDateEnd" ErrorMessage="Enter 'End Date'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>
    
    <div class="clear-both"></div>

    <asp:CustomValidator ID="cvDateRange" runat="server" ValidationGroup="AgreementPayment" OnServerValidate="cvDateRange_ServerValidate" Display="Dynamic" SetFocusOnError="true" CssClass="error block div-form-message" />

    <div style="margin: 10px;">
        <b>Agreement Payment Settings</b>
        &nbsp;
        <asp:Label ID="lblPaymentSettingsError" runat="server" EnableViewState="false" CssClass="error" />
    </div>

    <asp:Repeater ID="repPaymentSettings" runat="server" OnItemDataBound="repPaymentSettings_ItemDataBound">
        <HeaderTemplate>
            <table style="margin: 5px;">
                <tr>
                    <td></td>
                    <td>Month Day From</td>
                    <td>Month Day To</td>
                    <td>Next Month Payment Day</td>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td style="padding: 0px 5px;"><asp:Label ID="lblRowNumber" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="txtDayFrom" runat="server" MaxLength="2" Text='<%# DataBinder.Eval(Container.DataItem, "DayFrom") %>' />
                        <asp:RangeValidator id="rvDayFrom" runat="server" ValidationGroup="AgreementPayment" ControlToValidate="txtDayFrom" MinimumValue="1" MaximumValue="31" Type="Integer" Text="<br />not valid day" Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtDayTo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DayTo") %>' />
                        <asp:RangeValidator id="rvDayTo" runat="server" ValidationGroup="AgreementPayment" ControlToValidate="txtDayTo" MinimumValue="1" MaximumValue="31" Type="Integer" Text="<br />not valid day" Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtDayPayment" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DayPayment") %>' />
                        <asp:RangeValidator id="rvDayPayment" runat="server" ValidationGroup="AgreementPayment" ControlToValidate="txtDayPayment" MinimumValue="1" MaximumValue="31" Type="Integer" Text="<br />not valid day" Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
                    </td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <div class="div-form-command">
        <br />
        <asp:Button ID="btnSaveBottom" runat="server" ValidationGroup="AgreementPayment" Text="Save" OnClick="Save_Payment" />
        &nbsp;
        <a id="lnkCloseBottom" runat="server">Close</a>
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

                <asp:Repeater ID="repAgreementPayment" runat="server" OnItemDataBound="repAgreementPayment_ItemDataBound">
                    <HeaderTemplate>
                        <ul>
                    </HeaderTemplate>
                    <ItemTemplate>
                            <li style="padding: 5px 0px;">
                                <b><%# DataBinder.Eval(Container.DataItem, "CardName") %></b>
                                &nbsp;
                                [ <%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateStart")) %> - <%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateEnd")) %> ]
                                <br />
                                <asp:LinkButton ID="btnEditPayment" runat="server" OnCommand="Edit_Payment" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="Edit" />
                                |
                                <asp:LinkButton ID="btnCopyPayment" runat="server" OnCommand="Edit_Payment" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="Copy" />
                                |
                                <asp:LinkButton ID="btnDeletePayment" runat="server" OnCommand="Delete_Payment" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="Delete" OnClientClick="javascript: return confirm('Delete Item?');" ForeColor="Red" />

                                <asp:Repeater ID="repAgreementPaymentSettings" runat="server">
                                    <HeaderTemplate>
                                        <table class="table-sub" style="margin: 5px 0px;">
                                            <tr>
                                                <td>Day From</td>
                                                <td>Day To</td>
                                                <td>Payment Day</td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                            <tr>
                                                <td><%# DataBinder.Eval(Container.DataItem, "DayFrom") %></td>
                                                <td><%# DataBinder.Eval(Container.DataItem, "DayTo") %></td>
                                                <td><%# DataBinder.Eval(Container.DataItem, "DayPayment") %></td>
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
