<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CashBoxView.aspx.cs" Inherits="MatchBox.CashBoxView" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <table style="width: 100%;">
        <tr class="nowrap">
            <td><h1 id="hedMain" runat="server" class="float-left">Cashboxes</h1></td>
            <td class="nowrap">&nbsp; &nbsp; - &nbsp; &nbsp;</td>
            <td><a href="javascript: void(0);" onclick="javascript: display_section('section-excel', 'block'); display_section('section-search', 'none');">Upload table</a></td>
            <td style="width: 100%;"">&nbsp;</td>
            <td><a href="javascript: void(0);" onclick="javascript: display_section('section-search', 'block'); display_section('section-excel', 'none');">Show search</a></td>
            <td>&nbsp;|&nbsp;</td>
            <td><a id="lnkAddNew" runat="server" href="javascript: void(0);">Add new</a></td>
            <td>&nbsp;|&nbsp;</td>
            <td><asp:LinkButton ID="btnDownloadExcel" runat="server" Text="Download excel" OnClick="btnDownloadExcel_Click" /></td>
            <td>&nbsp;|&nbsp;</td>
            <td>
                <asp:LinkButton ID="btnReload" runat="server" Text="Reload page" OnCommand="On_Command" CommandName="Reload" />
                <span id="spanReload" class="attention"></span>
            </td>
        </tr>
    </table>

    <br />

    <section id="secForm" class="no-display">
        <div class="close"><a href="javascript: void(0);" onclick="javascript: cancel_add_new();">Close</a></div>
        <iframe id="fraForm" runat="server" height="600" class="frame-form"></iframe>
    </section>

    <section id="secUpload" class="section-excel">
        <asp:Label ID="lblCashBoxUpload" runat="server" AssociatedControlID="fuCashBoxUpload" Text="Cashboxes File" Font-Bold="true" />
        <asp:FileUpload ID="fuCashBoxUpload" runat="server" />
        &nbsp;
        <asp:Button ID="btnUploadCashBox" runat="server" ValidationGroup="CashBoxUpload" OnClick="btnUploadCashBox_Click" Text="Upload" Font-Bold="true" />
        &nbsp;
        <a href="Download/CashBox.xlsx">Download Template</a>
        &nbsp;
        <a href="javascript: void(0);" onclick="javascript: display_section('section-excel', 'none');">Close</a>
        <br />
        <asp:RequiredFieldValidator ID="rfvCashBoxUpload" runat="server" ValidationGroup="CashBoxUpload" ControlToValidate="fuCashBoxUpload" ErrorMessage="Select 'Cashboxes File'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </section>
    
    <section id="secSearch" runat="server" class="section-search">
        <h3 class="hed-search">Search</h3>
        
        <%--Company--%>
        <div id="divCompany_Search" runat="server" class="float-left div-form-field">
            <label for="<%= ddlCompanySearch.ClientID %>" class="block">Company</label>
            <asp:DropDownList ID="ddlCompanySearch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCompanySearch_SelectedIndexChanged">
                <asp:ListItem Text="" Value="0" />
            </asp:DropDownList>
        </div>
        <%--Network--%>
        <asp:UpdatePanel ID="upNetwork" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divNetwork_Search" runat="server" class="float-left div-form-field">
                    <label for="<%= ddlNetworkSearch.ClientID %>" class="block">Network</label>
                    <asp:DropDownList ID="ddlNetworkSearch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNetworkSearch_SelectedIndexChanged">
                        <asp:ListItem Text="" Value="0" />
                    </asp:DropDownList>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlCompanySearch" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <%--Branch--%>
        <asp:UpdatePanel ID="upBranch" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divBranch_Search" runat="server" class="float-left div-form-field">
                    <label for="<%= ddlBranchSearch.ClientID %>" class="block">Branch</label>
                    <asp:DropDownList ID="ddlBranchSearch" runat="server">
                        <asp:ListItem Text="" Value="0" />
                    </asp:DropDownList>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlCompanySearch" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlNetworkSearch" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <%--CashBoxName--%>
        <div id="divCashBoxName_Search" runat="server" class="float-left div-form-field">
            <label for="<%= txtCashBoxNameSearch.ClientID %>" class="block">CashBox Name</label>
            <asp:TextBox ID="txtCashBoxNameSearch" runat="server" MaxLength="50" />
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

    <div id="divMessage" runat="server" EnableViewState="false" Visible="false" class="div-message">
        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
        <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
    </div>

    <section id="secTable">
        <asp:HiddenField ID="hidOrderBy" runat="server" />

        <asp:Repeater ID="repTable" runat="server" OnItemDataBound="repTable_ItemDataBound">
            <HeaderTemplate>
                <table class="table">
                    <tr class="nowrap">
                        <th id="thCashBoxNumber" runat="server" rowspan="2">
                            CashBox Number
                            <asp:LinkButton ID="btnSortCashBoxNumberAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="CashBoxNumber ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortCashBoxNumberDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="CashBoxNumber DESC" CssClass="no-line" />
                        </th>
                        <th id="thCashBoxName" runat="server" rowspan="2">
                            CashBox Name
                            <asp:LinkButton ID="btnSortCashBoxNameAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="CashBoxName ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortCashBoxNameDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="CashBoxName DESC" CssClass="no-line" />
                        </th>
                        <th id="thBranchName" runat="server" rowspan="2">
                            Branch
                            <asp:LinkButton ID="btnSortBranchNameAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="BranchName ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortBranchNameDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="BranchName DESC" CssClass="no-line" />
                        </th>
                        <th id="thNetworkName" runat="server" rowspan="2">
                            Network
                            <asp:LinkButton ID="btnSortNetworkNameAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="NetworkName ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortNetworkNameDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="NetworkName DESC" CssClass="no-line" />
                        </th>
                        <th id="thCompanyName" runat="server" rowspan="2">
                            Company
                            <asp:LinkButton ID="btnSortCompanyNameAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="CompanyName ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortCompanyNameDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="CompanyName DESC" CssClass="no-line" />
                        </th>
                        <th id="thLastPeriod" runat="server" colspan="3">Last Period</th>
                        <th id="thOpen" runat="server" rowspan="2">&#8597;</th>
                        <th id="thDelete" runat="server" rowspan="2">x</th>
                    </tr>
                    <tr id="trLastPeriod" runat="server">
                        <th>Start Date</th>
                        <th>End Date</th>
                        <th>Terminal</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr id="trRow" runat="server">
                        <td id="tdCashBoxNumber" runat="server"><%# DataBinder.Eval(Container.DataItem, "CashBoxNumber") %></td>
                        <td id="tdCashBoxName" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "CashBoxName") %></td>
                        <td id="tdBranchName" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "BranchName") %></td>
                        <td id="tdNetworkName" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "NetworkName") %></td>
                        <td id="tdCompanyName" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "CompanyName") %></td>
                        <td id="tdLastStartDate" runat="server" class="nowrap"></td>
                        <td id="tdLastEndDate" runat="server" class="nowrap"></td>
                        <td id="tdLastTerminal" runat="server" class="nowrap"></td>
                        <td id="tdOpen" runat="server" class="nowrap text-center">
                            <a id="lnkOpen" runat="server" href="javascript: void(0);" onclick="javascript: link_open_onmouseup(this);" title="Open" class="no-line">+</a>
                        </td>
                        <td id="tdDelete" runat="server" class="nowrap text-center">
                            <asp:LinkButton ID="btnDelete" runat="server" Text="x" OnClientClick="javascript: return confirm('Delete Item?');" OnCommand="On_Command" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' ToolTip="Delete" ForeColor="Red" CssClass="no-line" />
                        </td>
                    </tr>
                    <tr id="trForm" runat="server" class="no-display">
                        <td colspan="14" class="nowrap">
                            <iframe id="fraForm" runat="server" height="600" class="frame-form" />
                        </td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <p id="pPaging" runat="server" visible="false">
            <asp:LinkButton ID="btnPreviousPage" runat="server" OnCommand="On_Command" CommandName="Previous" Text="&larr;" ToolTip="Previous" CssClass="no-line" />
            <asp:TextBox ID="txtCurrentPage" runat="server" AutoPostBack="true" OnTextChanged="txtCurrentPage_TextChanged" Text="1" MaxLength="5" Width="50" CssClass="text-center" />
            <asp:LinkButton ID="btnNextPage" runat="server" OnCommand="On_Command" CommandName="Next" Text="&rarr;" ToolTip="Next" CssClass="no-line" />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            Page size
            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem Value="5" Text="5" />
                <asp:ListItem Value="10" Text="10" />
                <asp:ListItem Value="25" Text="25" Selected="True" />
                <asp:ListItem Value="50" Text="50" />
                <asp:ListItem Value="100" Text="100" />
            </asp:DropDownList>
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            Pages count:
            <asp:Label ID="lblPagesCount" runat="server" />
        </p>
    </section>

    <script>
        var s_lnk_open_id = "<%= s_lnk_open_id %>";

        if (s_lnk_open_id != "") {
            document.getElementById(s_lnk_open_id).click();
        }
    </script>
</asp:Content>
