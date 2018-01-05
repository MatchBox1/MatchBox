<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MatchingNew.aspx.cs" Inherits="MatchBox.MatchingNew" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <div>
        <h1 id="hedMain" runat="server" class="float-left">New Matching</h1>
        <div id="divMenu" class="float-right">
            <a href="MatchingView.aspx">Matchings</a>
        </div>
        <div class="clear-both"></div>
    </div>

    <div id="divMessage" runat="server" EnableViewState="false" Visible="false" class="div-message">
        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
        <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
    </div>

    <hr />

    <div class="clear-both"></div>
    
    <%--TransactionDateFrom--%>
    <div id="divTransactionDateFrom" runat="server" class="float-left div-form-field">
        <label for="<%= ddlTransactionDayFrom.ClientID %>" class="block">Transaction Date From</label>
        <asp:DropDownList ID="ddlTransactionDayFrom" runat="server" style="width: 32%;">
            <asp:ListItem Text="Day" Value="0" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlTransactionMonthFrom" runat="server" style="width: 32%;">
            <asp:ListItem Text="Month" Value="0" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlTransactionYearFrom" runat="server" style="width: 32%;">
            <asp:ListItem Text="Year" Value="0" />
        </asp:DropDownList>
    </div>

    <%--TransactionDateTo--%>
    <div id="divDateToInside" runat="server" class="float-left div-form-field">
        <label for="<%= ddlTransactionDayTo.ClientID %>" class="block">Transaction Date To</label>
        <asp:DropDownList ID="ddlTransactionDayTo" runat="server" style="width: 32%;">
            <asp:ListItem Text="Day" Value="0" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlTransactionMonthTo" runat="server" style="width: 32%;">
            <asp:ListItem Text="Month" Value="0" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlTransactionYearTo" runat="server" style="width: 32%;">
            <asp:ListItem Text="Year" Value="0" />
        </asp:DropDownList>
    </div>
    
    <div class="clear-both"></div>

    <%--PaymentDateFrom--%>
    <div id="divPaymentDateFrom" runat="server" class="float-left div-form-field">
        <label for="<%= ddlPaymentDayFrom.ClientID %>" class="block">Payment Date From</label>
        <asp:DropDownList ID="ddlPaymentDayFrom" runat="server" style="width: 32%;">
            <asp:ListItem Text="Day" Value="0" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlPaymentMonthFrom" runat="server" style="width: 32%;">
            <asp:ListItem Text="Month" Value="0" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlPaymentYearFrom" runat="server" style="width: 32%;">
            <asp:ListItem Text="Year" Value="0" />
        </asp:DropDownList>
    </div>
    
    <%--PaymentDateTo--%>
    <div id="divPaymentDateTo" runat="server" class="float-left div-form-field">
        <label for="<%= ddlPaymentDayTo.ClientID %>" class="block">Payment Date To</label>
        <asp:DropDownList ID="ddlPaymentDayTo" runat="server" style="width: 32%;">
            <asp:ListItem Text="Day" Value="0" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlPaymentMonthTo" runat="server" style="width: 32%;">
            <asp:ListItem Text="Month" Value="0" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlPaymentYearTo" runat="server" style="width: 32%;">
            <asp:ListItem Text="Year" Value="0" />
        </asp:DropDownList>
    </div>

    <div class="float-left div-form-field">
        <br />
        <asp:CheckBox ID="chkEmptyPaymentDate" runat="server" Text="Include Empty Payment Date" />
    </div>

    <div class="clear-both"></div>

    <%--Company--%>
    <div id="divCompany" runat="server" class="float-left div-form-field">
        Company
        <br />
        <asp:UpdatePanel ID="upSelectBy" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Block">
            <ContentTemplate>
                Select by :
                <asp:RadioButtonList ID="rblSelectBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblSelectBy_SelectedIndexChanged" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Value="cashbox" Text="CashBox" Selected="True" />
                    <asp:ListItem Value="terminal" Text="Terminal" />
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
        <label for="<%= lstCredit.ClientID %>" class="block">Credit</label>
        <asp:CheckBoxList ID="lstCredit" runat="server" />
    </div>

    <%--Card--%>
    <div id="divCard" runat="server" class="float-left div-form-field">
        <label for="<%= lstCard.ClientID %>" class="block">Card</label>
        <asp:CheckBoxList ID="lstCard" runat="server" />
    </div>

    <div class="clear-both"></div>

    <div class="div-form-command">
        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="javascript: show_wait(this.id, 'spnSearch', false);" Text="Search" />
        <span id="spnSearch" class="message" style="display: none;">Please wait..</span>
        &nbsp;
        <a href="MatchingNew.aspx">Reset</a>
    </div>

    <asp:Panel ID="pnlSearchResult" runat="server" Visible="false">
        <hr />

        <table style="margin: 0px; padding: 0px;">
            <tr>
                <td style="vertical-align: top; width: 49%;">
                    <asp:Repeater ID="repInsideSum" runat="server" OnItemDataBound="repInsideSum_ItemDataBound">
                        <HeaderTemplate>
                            Inside
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
                                    <td><%# DataBinder.Eval(Container.DataItem, "CompanyID") %></td>
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
                </td>
                <td class="nowrap">&nbsp;</td>
                <td style="vertical-align: top; width: 49%;">
                    <asp:Repeater ID="repOutsideSum" runat="server" OnItemDataBound="repOutsideSum_ItemDataBound">
                        <HeaderTemplate>
                            Outside
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
                                    <td><%# DataBinder.Eval(Container.DataItem, "CompanyID") %></td>
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
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlAutoMatching" runat="server" Visible="false">
        <%--Strategy--%>
        <div id="divStrategy" runat="server" class="float-left div-form-field">
            <label for="<%= ddlStrategy.ClientID %>" class="block">Strategy</label>
            <asp:DropDownList ID="ddlStrategy" runat="server" />
            <asp:RequiredFieldValidator ID="rfvStrategy" runat="server" ValidationGroup="Matching" ControlToValidate="ddlStrategy" InitialValue="0" ErrorMessage="Select 'Strategy'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        </div>

        <div class="float-left div-form-field text-center">
            <br />
            <asp:Button ID="btnAutoMatching" runat="server" ValidationGroup="Matching" OnClick="btnAutoMatching_Click" OnClientClick="javascript: show_wait(this.id, 'spnAutoMatching', true);" Text="Auto Matching" />
            <span id="spnAutoMatching" class="message" style="display: none;">Please wait..</span>
        </div>

        <div class="float-left div-form-field">
            <asp:Label ID="lblQueryInfo" runat="server" EnableViewState="false" />
        </div>

        <div class="clear-both"></div>
    </asp:Panel>

    <asp:Panel ID="pnlQuery" runat="server" EnableViewState="false" Visible="false">
        <hr />
        Queries (<asp:Label ID="lblQueryCount" runat="server" />)
        <asp:GridView ID="gvQuery" runat="server" AutoGenerateColumns="true" CssClass="table-center" />

        Inside (<asp:Label ID="lblInsideCount" runat="server" />)
        <asp:GridView ID="gvInside" runat="server" AutoGenerateColumns="true" CssClass="table-center" />

        Outside (<asp:Label ID="lblOutsideCount" runat="server" />)
        <asp:GridView ID="gvOutside" runat="server" AutoGenerateColumns="true" CssClass="table-center" />

        Matching (<asp:Label ID="lblMatchingCount" runat="server" />)
        <asp:GridView ID="gvMatching" runat="server" AutoGenerateColumns="true" CssClass="table-center" />

        <script>
            try { document.getElementById("<%= gvQuery.ClientID %>").attributes.removeNamedItem("style"); } catch (ex) { }
            try { document.getElementById("<%= gvInside.ClientID %>").attributes.removeNamedItem("style"); } catch (ex) { }
            try { document.getElementById("<%= gvOutside.ClientID %>").attributes.removeNamedItem("style"); } catch (ex) { }
            try { document.getElementById("<%= gvMatching.ClientID %>").attributes.removeNamedItem("style"); } catch (ex) { }
        </script>
    </asp:Panel>

    <script>
        function show_wait(s_button_id, s_span_id, b_validate) {
            if (b_validate == true && Page_ClientValidate() == false) { return; }

            document.getElementById(s_button_id).style.display = "none";
            document.getElementById(s_span_id).style.display = "";
        }
    </script>
</asp:Content>
