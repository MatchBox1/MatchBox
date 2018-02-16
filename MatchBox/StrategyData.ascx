<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StrategyData.ascx.cs" Inherits="MatchBox.StrategyData" %>

<table>
    <tr>
        <td class="nowrap" style="padding: 0px 10px;"><b>Strategy Name</b> : <asp:Label ID="lblStrategyName" runat="server" /></td>
        <td class="nowrap" style="padding: 0px 10px;">&nbsp;</td>
        <td class="nowrap" style="padding: 0px 10px;"><b>Queries Percent</b> : <asp:Label ID="lblQueryPercent" runat="server" />%</td>
        <td class="nowrap" style="padding: 0px 10px;">&nbsp;</td>
        <td class="nowrap" style="padding: 0px 10px;"><b>Status</b> : <asp:Label ID="lblStatusName" runat="server" /></td>
        <td class="nowrap" style="padding: 0px 10px; width: 100%;">&nbsp;</td>
        <td id="tdDataFile" runat="server" class="nowrap" style="padding: 0px 10px;"><a href="javascript: void(0);" onclick="javascript: display_by_class('divDataFileAdd');">Add Data Files</a></td>
        <td id="tdPending" runat="server" class="nowrap" style="padding: 0px 10px;"><asp:Button ID="btnPending" runat="server" OnClick="Strategy_Pending" Text="Execute Strategy" /></td>
        <td id="tdstopexecution" runat="server" class="nowrap" style="padding: 0px 10px;" visible="false"><asp:Button ID="btnstopexecution" runat="server" OnClick="Strategy_StopExecution" Text="Stop Strategy" /></td>
    </tr>
</table>

<asp:HiddenField ID="hidStatusID" runat="server" />

<hr style="margin: 10px;" />

<div style="margin: 10px;">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<div id="divDataFileAdd" class="no-display">
    <h3 style="margin: 10px;">Add Data Files</h3>

    <div class="div-form-item font-mono nowrap">
        <asp:CheckBoxList ID="cblDataFile" runat="server" />
    </div>

    <div class="div-form-command">
        <asp:Button ID="btnSave" runat="server" ValidationGroup="Data" Text="Save" OnClick="DataFile_Add" />
        &nbsp;
        <a href="javascript: void(0);" onclick="javascript: display_by_class('divDataFileAdd');">Close</a>
        &nbsp; &nbsp; &nbsp;
        <asp:CustomValidator ID="cvDataFile" runat="server" ValidationGroup="Data" OnServerValidate="DataFile_Validate" ClientValidationFunction="data_file_validate" ErrorMessage="Select Data File/s." Display="Dynamic" CssClass="error" />
    </div>

    <hr style="margin: 10px;" />
</div>

<div>
    <div class="float-left div-form-item nowrap" style="width: 48%;">
        <asp:Repeater ID="repInsideSum" runat="server" OnItemDataBound="repInsideSum_ItemDataBound">
            <HeaderTemplate>
                Inside Data
                <table class="table-sub">
                    <tr>
                        <th>Company</th>
                        <th>Transaction Date</th>
                        <th>Payment Date</th>
                        <th>Transaction Count</th>
                        <th>Amount Sum</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td><%# DataBinder.Eval(Container.DataItem, "CompanyName") %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "TransactionYear") %>-<%# String.Format("{0:00}", DataBinder.Eval(Container.DataItem, "TransactionMonth")) %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "PaymentYear") %>-<%# String.Format("{0:00}", DataBinder.Eval(Container.DataItem, "PaymentMonth")) %></td>
                        <td><%# String.Format("{0:n0}", DataBinder.Eval(Container.DataItem, "TransactionCount")) %></td>
                        <td><%# String.Format("{0:n2}", DataBinder.Eval(Container.DataItem, "AmountSum")) %></td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                    <tr class="bold">
                        <td colspan="3">Total:</td>
                        <td><asp:Label ID="lblTransactionCountTotal" runat="server" /></td>
                        <td><asp:Label ID="lblAmountSumTotal" runat="server" /></td>
                    </tr>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <br />
        Inside Files
        <asp:Repeater ID="repInsideDataFile" runat="server" OnItemDataBound="DataFile_ItemDataBound">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                    <li>
                        [ <asp:LinkButton ID="btnDelete" runat="server" OnCommand="DataFile_Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="x" OnClientClick="javascript: return confirm('Delete Item?');" ToolTip="Delete" ForeColor="Red" CssClass="no-line" /> ]
                        &nbsp;
                        <%# DataBinder.Eval(Container.DataItem, "DataFileName") %>
                    </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div class="float-left div-form-item nowrap" style="width: 48%;">
        <asp:Repeater ID="repOutsideSum" runat="server" OnItemDataBound="repOutsideSum_ItemDataBound">
            <HeaderTemplate>
                Outside Data
                <table class="table-sub">
                    <tr>
                        <th>Company</th>
                        <th>Transaction Date</th>
                        <th>Payment Date</th>
                        <th>Transaction Count</th>
                        <th>Amount Sum</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td><%# DataBinder.Eval(Container.DataItem, "CompanyName") %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "TransactionYear") %>-<%# String.Format("{0:00}", DataBinder.Eval(Container.DataItem, "TransactionMonth")) %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "PaymentYear") %>-<%# String.Format("{0:00}", DataBinder.Eval(Container.DataItem, "PaymentMonth")) %></td>
                        <td><%# String.Format("{0:n0}", DataBinder.Eval(Container.DataItem, "TransactionCount")) %></td>
                        <td><%# String.Format("{0:n2}", DataBinder.Eval(Container.DataItem, "AmountSum")) %></td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                    <tr class="bold">
                        <td colspan="3">Total:</td>
                        <td><asp:Label ID="lblTransactionCountTotal" runat="server" /></td>
                        <td><asp:Label ID="lblAmountSumTotal" runat="server" /></td>
                    </tr>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <br />
        Outside Files
        <asp:Repeater ID="repOutsideDataFile" runat="server" OnItemDataBound="DataFile_ItemDataBound">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                    <li>
                        [ <asp:LinkButton ID="btnDelete" runat="server" OnCommand="DataFile_Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="x" OnClientClick="javascript: return confirm('Delete Item?');" ToolTip="Delete" ForeColor="Red" CssClass="no-line" /> ]
                        &nbsp;
                        <%# DataBinder.Eval(Container.DataItem, "DataFileName") %>
                    </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div class="clear-both"></div>
</div>

<script>
    function data_file_validate(source, args) {
        var b_valid = false;
        var cblDataFile = document.getElementById("<%= cblDataFile.ClientID %>").getElementsByTagName("input");

        for (var i = 0; i < cblDataFile.length; i++) {
            if (cblDataFile[i].checked == true) {
                b_valid = true;
                break;
            }
        }

        args.IsValid = b_valid;
    }
</script>