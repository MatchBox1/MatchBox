<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommissionsNew.aspx.cs" Inherits="MatchBox.CommissionsNew" MasterPageFile="~/Main.Master" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <div style="background-color: #eeeeee">
        <div>
            <h1 id="hedMain" runat="server" class="float-left">New Commission</h1>
            <div id="divMenu" class="float-right">
                <a href="Commissions.aspx">Commissions</a>
            </div>
            <div class="clear-both"></div>
        </div>

        <section id="secForm" class="">
            <div class="close"><a href="Commissions.aspx">Close</a></div>
            <%--<iframe id="fraForm" runat="server" height="600" class="frame-form"></iframe>--%>
        </section>

        <div id="divMessage" runat="server" enableviewstate="false" visible="false" class="div-message">
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
            <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
        </div>

        <hr />

        <div class="clear-both"></div>

        <div id="divCompany" runat="server" class="float-left div-form-field">
            <label for="<%= ddlCompany.ClientID %>" class="block">Company</label>
            <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true" Onchange="OnCompanyChanged(this)" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"> <%--OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"--%>
                <asp:ListItem Text="" Value="0" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="ddlCompany" InitialValue="0" ErrorMessage="Select 'Company'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        </div>

        <asp:UpdatePanel ID="upNetwork" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divCreditCompany" runat="server" class="float-none div-form-field">
                    <asp:RadioButton ID="rdoCredit" runat="server" GroupName="TemplateFor" Text="Credit Company" AutoPostBack="true" OnCheckedChanged="Check_Template_For" />
                    <asp:DropDownList ID="ddlCreditCompany" runat="server" Onchange="OnCreditCompanyChanged(this)">
                        <asp:ListItem Text="" Value="0" />
                    </asp:DropDownList>
                    <%--<asp:RequiredFieldValidator ID="rfvCredit" runat="server" ControlToValidate="ddlCreditCompany" InitialValue="0" ErrorMessage="Select 'Credit'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />--%>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="rdoCredit" /> <%--EventName="SelectedIndexChanged"--%>
            </Triggers>
        </asp:UpdatePanel>


        <div id="divDiscount" runat="server" class="float-left div-form-field">
            <asp:RadioButton ID="rdoDiscount" runat="server" GroupName="TemplateFor" Text="Discount" AutoPostBack="true" OnCheckedChanged="Check_Template_For" />
            <asp:DropDownList ID="ddlDiscount" runat="server" AutoPostBack="true">
                <%--Onchange="OnDiscountChanged(this)" OnSelectedIndexChanged="ddlDiscount_SelectedIndexChanged"--%>
                <asp:ListItem Text="" Value="0" />
            </asp:DropDownList>
            <%--<asp:RequiredFieldValidator ID="rfvDiscount" runat="server" ControlToValidate="ddlDiscount" InitialValue="0" ErrorMessage="Select 'Discount'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />--%>
        </div>

        <asp:CustomValidator ID="cvTemplateFor" runat="server" OnServerValidate="cvTemplateFor_ServerValidate" CssClass="error block" />

        <div class="clear-both"></div>

        <div id="divPaymentDate" runat="server" class="float-left div-form-field-long">
            <b>Transaction Date</b>
            <br />
            <asp:TextBox ID="txtPaymentDate" runat="server" Width="60%" AutoPostBack="true" OnTextChanged="txtPaymentDate_TextChanged" />
            <%--Onchange="return OnPaymentDateChanged(this)" --%>
            <asp:RequiredFieldValidator ID="rfvPayment" runat="server" ControlToValidate="txtPaymentDate" ErrorMessage="Enter 'Payment Date'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
            <%--<asp:Label ID="lblTransactionDateError" runat="server" EnableViewState="false" CssClass="error block" />--%>
        </div>

        <div id="divCommissionType" runat="server" class="float-none div-form-field">

            <label for="<%= ddlCommissionType.ClientID %>" class="block">Commission Type</label>
            <asp:DropDownList ID="ddlCommissionType" runat="server">
                <asp:ListItem Text="" Value="0" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvCommission" runat="server" ControlToValidate="ddlCommissionType" InitialValue="0" ErrorMessage="Select 'Commission Type'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />

        </div>

        <div class="clear-both"></div>

        <div class="div-form-command">
            <%--<asp:Button ID="Button1" runat="server" OnClick="btnSearch_Click_New" OnClientClick="javascript: show_wait(this.id, 'spnSearch', false);" Text="Search" Visible="false" />
        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click_New" OnClientClick="javascript: show_wait(this.id, 'spnSearch', false);" Text="Search" />--%>

            <asp:Button ID="btnSearchCommission" runat="server" OnClick="btnSearchCommission_Click" OnClientClick="javascript: show_wait(this.id, 'spnSearch', false);" Text="Search" />
            <span id="spnSearch" class="message" style="display: none;">Please wait..</span>
            &nbsp;
        <a href="CommissionsNew.aspx">Reset</a>
        </div>
    </div>



    <asp:Panel ID="pnlSearchResult" runat="server" Visible="false">
        <hr />

        <table style="margin: 0px; padding: 0px;">
            <tr>
                <%--<td style="vertical-align: top; width: 49%;">
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
                <td class="nowrap">&nbsp;</td>--%>
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


        <asp:Panel ID="pnlAutoCommission" runat="server" Visible="false">

        <%--<div id="divStrategy" runat="server" class="float-left div-form-field">
            <label for="<%= ddlStrategy.ClientID %>" class="block">Strategy</label>
            <asp:DropDownList ID="ddlStrategy" runat="server" />
            <asp:RequiredFieldValidator ID="rfvStrategy" runat="server" ValidationGroup="Matching" ControlToValidate="ddlStrategy" InitialValue="0" ErrorMessage="Select 'Strategy'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        </div>--%>

        <div class="float-left div-form-field text-center">
            <br />
            <asp:Button ID="btnAutoCommission" runat="server" ValidationGroup="Matching" OnClick="btnAutoCommission_Click" OnClientClick="javascript: show_wait(this.id, 'spnAutoCommission', true);" Text="Calculate Commission" />
            <span id="spnAutoCommission" class="message" style="display: none;">Please wait..</span>
            <asp:Label ID="lblErrorBelow" runat="server" EnableViewState="false" CssClass="error block" />
        </div>

        <div class="float-left div-form-field">
            <asp:Label ID="lblQueryInfo" runat="server" EnableViewState="false" />
        </div>

        <div class="clear-both"></div>
    </asp:Panel>


    <script src="/App_JS/jquery.min.js"></script>
    <script>
        //function show_wait(s_button_id, s_span_id, b_validate) {
        //    if (b_validate == true && Page_ClientValidate() == false) { return; }

        //    document.getElementById(s_button_id).style.display = "none";
        //    document.getElementById(s_span_id).style.display = "";
        //}

        function OnCreditCompanyChanged(_control) {
            $('#ctl00_cphMain_txtPaymentDate').val('');
        }
        function OnCompanyChanged(_control) {
            $('#ctl00_cphMain_txtPaymentDate').val('');
        }
    </script>
</asp:Content>
