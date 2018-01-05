<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DataSummary.aspx.cs" Inherits="MatchBox.DataSummary" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <table style="width: 100%;">
        <tr class="nowrap">
            <td><h1 id="hedMain" runat="server" class="float-left">Data Summary</h1></td>
            <td class="nowrap">&nbsp; &nbsp; - &nbsp; &nbsp;</td>
            <td><a href="javascript: void(0);" onclick="javascript: display_section('section-search', 'block');">Show search</a></td>
            <td class="nowrap">&nbsp; &nbsp; - &nbsp; &nbsp;</td>
            <td><a href="DataInspector.aspx">Data Inspector</a></td>
            <td style="width: 100%;"">&nbsp;</td>
        </tr>
    </table>

    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />

    <section id="secSearch" runat="server" class="section-search">
        <h3 class="hed-search">Search</h3>

        <%--Company--%>
        <div id="divCompany_Search" runat="server" class="float-left div-form-field">
            <label for="<%= ddlCompanySearch.ClientID %>" class="block">Company</label>
            <asp:DropDownList ID="ddlCompanySearch" runat="server">
                <asp:ListItem Text="" Value="0" />
            </asp:DropDownList>
        </div>

        <%--DateFrom--%>
        <div id="divDateFrom_Search" runat="server" class="float-left div-form-field">
            <label for="<%= ddlYearFrom.ClientID %>" class="block">From</label>
            <asp:DropDownList ID="ddlYearFrom" runat="server" style="width: 49%;">
                <asp:ListItem Text="Year" Value="0" />
            </asp:DropDownList>
            <asp:DropDownList ID="ddlMonthFrom" runat="server" style="width: 49%;">
                <asp:ListItem Text="Month" Value="0" />
            </asp:DropDownList>
        </div>

        <%--DateTo--%>
        <div id="divDateTo_Search" runat="server" class="float-left div-form-field">
            <label for="<%= ddlYearTo.ClientID %>" class="block">To</label>
            <asp:DropDownList ID="ddlYearTo" runat="server" style="width: 49%;">
                <asp:ListItem Text="Year" Value="0" />
            </asp:DropDownList>
            <asp:DropDownList ID="ddlMonthTo" runat="server" style="width: 49%;">
                <asp:ListItem Text="Month" Value="0" />
            </asp:DropDownList>
        </div>

        <div class="clear-both"></div>

        <div class="div-search-command">
            <asp:LinkButton ID="btnSearch" runat="server" Text="Search" OnCommand="On_Command" CommandName="Search" />
            &nbsp;
            <asp:LinkButton ID="btnReset" runat="server" Text="Reset" OnCommand="On_Command" CommandName="Reset" />
            &nbsp;
            <a href="javascript: void(0);" onclick="javascript: display_section('section-search', 'none');">Close</a>
        </div>
    </section>

    <table>
        <tr>
            <td>
                <b>Inside Data</b>
                <asp:Repeater ID="repInside" runat="server" OnItemDataBound="repInside_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table">
                            <tr>
                                <th>Template</th>
                                <th>Source</th>
                                <th>Month</th>
                                <th>Amount</th>
                                <th>Rows</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                            <tr>
                                <td><%# DataBinder.Eval(Container.DataItem, "TemplateName") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "DataSourceName") %></td>
                                <td class="nowrap"><%# DataBinder.Eval(Container.DataItem, "TransactionDateYear") %>-<%# DataBinder.Eval(Container.DataItem, "TransactionDateMonth") %></td>
                                <td class="nowrap"><%# String.Format("{0:n2}", DataBinder.Eval(Container.DataItem, "TransactionGrossAmountSum")) %></td>
                                <td class="nowrap"><%# String.Format("{0:n0}", DataBinder.Eval(Container.DataItem, "RowsCount")) %> <asp:Label ID="lblRowsCountSplitted" runat="server" /></td>
                            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <div style="margin: 10px 0px;">
                    <b>Total Rows</b>: <asp:Label ID="lblInsideRows" runat="server" />
                    <br />
                    <b>Total Amount</b>: <asp:Label ID="lblInsideAmount" runat="server" />
                </div>
            </td>
            <td>&nbsp;</td>
            <td>
                <b>Outside Data</b>
                <asp:Repeater ID="repOutside" runat="server">
                    <HeaderTemplate>
                        <table class="table">
                            <tr>
                                <th>Template</th>
                                <th>Credit</th>
                                <th>Discount</th>
                                <th>Month</th>
                                <th>Amount</th>
                                <th>Rows</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                            <tr>
                                <td><%# DataBinder.Eval(Container.DataItem, "TemplateName") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "CreditName") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "DiscountName") %></td>
                                <td class="nowrap"><%# DataBinder.Eval(Container.DataItem, "PaymentDateYear") %>-<%# DataBinder.Eval(Container.DataItem, "PaymentDateMonth") %></td>
                                <td class="nowrap"><%# String.Format("{0:n2}", DataBinder.Eval(Container.DataItem, "DutyPaymentAmountSum")) %></td>
                                <td class="nowrap"><%# String.Format("{0:n0}", DataBinder.Eval(Container.DataItem, "RowsCount")) %></td>
                            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <div style="margin: 10px 0px;">
                    <b>Total Rows</b>: <asp:Label ID="lblOutsideRows" runat="server" />
                    <br />
                    <b>Total Amount</b>: <asp:Label ID="lblOutsideAmount" runat="server" />
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
