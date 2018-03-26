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

    <div id="divMessage" runat="server" enableviewstate="false" visible="false" class="div-message">
        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
        <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
    </div>

    <hr />

    <div class="clear-both"></div>

    <%--TransactionDateFrom Inside--%>
    <div id="divTransactionDateFrom" runat="server" class="float-left div-form-field">
        <b>Transaction Date</b>
        <br />
        <asp:TextBox ID="txtTransactionDate" runat="server" Width="98%" />
        <asp:Label ID="lblTransactionDateError" runat="server" EnableViewState="false" CssClass="error block" />
    </div>

    <div class="float-left div-form-field">
        <br />
        <asp:CheckBox ID="chkEmptyTransactionDate" runat="server" Text="Include Empty Transaction Date" />
    </div>

    <%--TransactionDateFrom Outside--%>
    <div id="divTransactionDateFromOutside" runat="server" class="float-right div-form-field">
        <b>Transaction Date Outside</b>
        <br />
        <asp:TextBox ID="txtTransactionDateOutside" runat="server" Width="98%" />
        <asp:Label ID="lblTransactionDateErrorOutside" runat="server" EnableViewState="false" CssClass="error block" />
    </div>

    <div class="float-left div-form-field">
        <br />
        <asp:CheckBox ID="chkEmptyTransactionDateOutside" runat="server" Text="Include Empty Transaction Date Outside" />
    </div>

    <div class="clear-both"></div>

    <%--PaymentDateFrom Inside--%>
    <div id="divPaymentDateFrom" runat="server" class="float-left div-form-field">
        <b>Payment Date</b>
        <br />
        <asp:TextBox ID="txtPaymentDate" runat="server" Width="98%" />
        <asp:Label ID="lblPaymentDateError" runat="server" EnableViewState="false" CssClass="error block" />
    </div>

    <div class="float-left div-form-field">
        <br />
        <asp:CheckBox ID="chkEmptyPaymentDate" runat="server" Text="Include Empty Payment Date" />
    </div>
    
    <%--PaymentDateFrom Outside--%>
    <div id="divPaymentDateFromOutside" runat="server" class="float-right div-form-field">
        <b>Payment Date Outside</b>
        <br />
        <asp:TextBox ID="txtPaymentDateOutside" runat="server" Width="98%" />
        <asp:Label ID="lblPaymentDateErrorOutside" runat="server" EnableViewState="false" CssClass="error block" />
    </div>

    <div class="float-right div-form-field">
        <br />
        <asp:CheckBox ID="chkEmptyPaymentDateOutside" runat="server" Text="Include Empty Payment Date Outside" />
    </div>


    <div class="clear-both"></div>

    <div class="div-form-command">
        <asp:Button ID="Button1" runat="server" OnClick="btnSearch_Click_New" OnClientClick="javascript: show_wait(this.id, 'spnSearch', false);" Text="Search" Visible="false" />
        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click_New" OnClientClick="javascript: show_wait(this.id, 'spnSearch', false);" Text="Search" />
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
                                <td>
                                    <asp:Label ID="lblTransactionCountTotal" runat="server" /></td>
                                <td>
                                    <asp:Label ID="lblAmountSumTotal" runat="server" /></td>
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
                                <td>
                                    <asp:Label ID="lblTransactionCountTotal" runat="server" /></td>
                                <td>
                                    <asp:Label ID="lblAmountSumTotal" runat="server" /></td>
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
            <asp:Label ID="lblErrorBelow" runat="server" EnableViewState="false" CssClass="error block" />
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
