<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgreementPeriod.ascx.cs" Inherits="MatchBox.AgreementPeriod" %>

<table>
    <tr>
        <td class="nowrap" style="padding: 0px 10px;"><b>Company</b> : <asp:Label ID="lblCompanyName" runat="server" /></td>
        <td class="nowrap" style="padding: 0px 10px;">&nbsp;</td>
        <td class="nowrap" style="padding: 0px 10px;"><b>Credit</b> : <asp:Label ID="lblCreditName" runat="server" /></td>
        <td class="nowrap" style="padding: 0px 10px; width: 100%;">&nbsp;</td>
        <td class="nowrap" style="padding: 0px 10px;"><asp:LinkButton ID="btnAddPeriod" runat="server" OnCommand="Edit_Period" CommandArgument="0" Text="Add Service Period" /></td>
    </tr>
</table>

<hr style="margin: 10px;" />

<div style="margin: 10px;">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<%--Period--%>
<div id="divPeriodForm" runat="server" visible="false" style="margin: 10px;">
    <div class="nowrap">
        <label for="<%= txtDateStart.ClientID %>" class="bold">Start Date</label>
        <asp:TextBox ID="txtDateStart" runat="server" MaxLength="10" Width="100px" placeholder="dd/mm/yyyy" />
        &nbsp;
        <label for="<%= txtDateEnd.ClientID %>" class="bold">End Date</label>
        <asp:TextBox ID="txtDateEnd" runat="server" MaxLength="10" Width="100px" placeholder="dd/mm/yyyy" />
        &nbsp;
        <asp:Button ID="btnSavePeriod" runat="server" ValidationGroup="Period" OnClick="btnSavePeriod_Click" Text="Save" />
        &nbsp;
        <asp:LinkButton ID="btnClosePeriod" runat="server" OnClick="Close_Form" Text="Close" />
        <br />
    </div>

    <div style="margin: 10px 0px;">
        <asp:CustomValidator ID="cvDateRange" runat="server" ValidationGroup="Period" OnServerValidate="cvDateRange_ServerValidate" Display="Dynamic" CssClass="error block" />
        <asp:RequiredFieldValidator ID="rfvDayStart" runat="server" ValidationGroup="Period" ControlToValidate="txtDateStart" ErrorMessage="Enter 'Start Date'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <hr />
</div>

<%--SupplierTerminal--%>
<div id="divSupplierTerminalForm" runat="server" visible="false" style="margin: 10px;">
    <div class="nowrap" style="margin: 10px 0px;">
        <asp:Button ID="btnSaveSupplierTerminalTop" runat="server" ValidationGroup="SupplierTerminal" OnClick="Update_Supplier_Terminal" Text="Save" />
        &nbsp;
        <asp:LinkButton ID="btnCloseSupplierTerminalTop" runat="server" OnClick="Close_Form" Text="Close" />
    </div>

    <asp:Label ID="lblDate_Form" runat="server" Font-Bold="true" />

    <asp:Repeater ID="repSupplierTerminalForm" runat="server" OnItemDataBound="repSupplierTerminalForm_ItemDataBound">
        <HeaderTemplate>
            <table class="table-sub">
                <tr>
                    <th>Group</th>
                    <th>Supplier</th>
                    <th>Terminal</th>
                    <th>Message</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr class="nowrap">
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "SupplierGroup") %>
                        <asp:HiddenField ID="hidSupplierGroupID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "SupplierGroupID") %>' />
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Supplier") %>
                        <asp:HiddenField ID="hidSupplierID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTerminal" runat="server" Visible="false" />
                        <div style="overflow-y: auto; overflow-x: hidden; max-height: 100px;">
                            <asp:CheckBoxList ID="cblTerminal" runat="server" CssClass="table-list-sub" />
                        </div>
                    </td>
                    <td id="tdMessage" runat="server"></td>
                    <%--<td><asp:Label ID="lblSupplierTerminalMessage" runat="server" EnableViewState="false" /></td>--%>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <div class="nowrap" style="margin: 10px 0px;">
        <asp:Button ID="btnSaveSupplierTerminalBottom" runat="server" ValidationGroup="SupplierTerminal" OnClick="Update_Supplier_Terminal" Text="Save" />
        &nbsp;
        <asp:LinkButton ID="btnCloseSupplierTerminalBottom" runat="server" OnClick="Close_Form" Text="Close" />
    </div>

    <hr />
</div>

<%--SupplierTerminalUpload--%>
<div id="divSupplierTerminalUpload" runat="server" visible="false" style="margin: 10px;">
    <asp:Label ID="lblDate_Upload" runat="server" Font-Bold="true" />
    &nbsp; &nbsp;
    Supplier-Terminal File :
    <asp:FileUpload ID="fuSupplierTerminalUpload" runat="server" />
    &nbsp;
    <asp:Button ID="btnSaveSupplierTerminalUpload" runat="server" ValidationGroup="SupplierTerminalUpload" OnClick="btnSaveSupplierTerminalUpload_Click" Text="Upload" />
    &nbsp;
    <asp:LinkButton ID="btnCloseSupplierTerminalUpload" runat="server" OnClick="Close_Form" Text="Close" />
    <hr />
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
                <br />
                <asp:LinkButton ID="btnEditPeriod" runat="server" OnCommand="Edit_Period" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="Edit" />
                |
                <asp:LinkButton ID="btnAddSupplierTerminal" runat="server" OnCommand="Add_Supplier_Terminal" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="Add Supplier-Terminal" />
                |
                <asp:LinkButton ID="btnUploadSupplierTerminal" runat="server" OnCommand="Upload_Supplier_Terminal" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="Upload Table" />
                |
                <a href="Download/SupplierTerminal.xlsx">Download Template</a>
                |
                <asp:LinkButton ID="btnDeletePeriod" runat="server" OnCommand="btnDeletePeriod_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="Delete" OnClientClick="javascript: return confirm('Delete Item?');" ForeColor="Red" />

                <asp:Repeater ID="repSupplierTerminal" runat="server">
                    <HeaderTemplate>
                        <table class="table-sub" style="margin: 10px 0px;">
                            <tr>
                                <th>Group</th>
                                <th>Supplier</th>
                                <th>Terminal</th>
                                <th>x</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                            <tr>
                                <td><%# DataBinder.Eval(Container.DataItem, "SupplierGroup") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "Supplier") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "Terminal") %></td>
                                <td><asp:LinkButton ID="btnDeleteSupplierTerminal" runat="server" OnCommand="btnDeleteSupplierTerminal_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="x" OnClientClick="javascript: return confirm('Delete Item?');" ToolTip="Delete" ForeColor="Red" CssClass="no-line" /></td>
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
