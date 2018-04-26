<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Commissions.aspx.cs" Inherits="MatchBox.Commissions" MasterPageFile="~/Main.Master"%>


<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <table style="width: 100%;">
        <tr class="nowrap">
            <td><h1 id="hedMain" runat="server">Matchings</h1></td>
            <td class="nowrap">&nbsp; &nbsp; - &nbsp; &nbsp;</td>
            <td><a href="DataFieldPriority.aspx">Columns Settings</a></td>
            <td style="width: 100%;"">&nbsp;</td>
            <td><a href="javascript: void(0);" onclick="javascript: display_section('section-search', 'block');">Show search</a></td>
            <td>&nbsp;|&nbsp;</td>
            <td><a href="CommissionsNew.aspx">Add new</a></td>
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
    
    <section id="secSearch" runat="server" class="section-search">
        <h3 class="hed-search">Search</h3>

        <%--Strategy--%>
        <div id="divStrategy_Search" runat="server" class="float-left div-form-field">
            <label for="<%= ddlStrategySearch.ClientID %>" class="block">Strategy</label>
            <asp:DropDownList ID="ddlStrategySearch" runat="server">
                <asp:ListItem Text="" Value="0" />
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
                        <th id="thID" runat="server">
                            ID
                            <asp:LinkButton ID="btnSortIDAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="ID ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortIDDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="ID DESC" CssClass="no-line" />
                        </th>
                        <th id="thMatchingActionName" runat="server">
                            Matching Action
                            <asp:LinkButton ID="btnSortMatchingActionNameAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="MatchingActionName ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortMatchingActionNameDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="MatchingActionName DESC" CssClass="no-line" />
                        </th>
                        <th id="thStrategyName" runat="server">
                            Strategy Name
                            <asp:LinkButton ID="btnSortStrategyNameAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="StrategyName ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortStrategyNameDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="StrategyName DESC" CssClass="no-line" />
                        </th>
                        <th id="thStatus" runat="server">Status</th>
                        <th id="thDateStart" runat="server">
                            Start Date
                            <asp:LinkButton ID="btnSortDateStartAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="DateStart ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortDateStartDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="DateStart DESC" CssClass="no-line" />
                        </th>
                        <th id="thDateEnd" runat="server">
                            End Date
                            <asp:LinkButton ID="btnSortDateEndAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="DateEnd ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortDateEndDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="DateEnd DESC" CssClass="no-line" />
                        </th>
                        <th id="thDuration" runat="server">
                            Duration
                            <asp:LinkButton ID="btnSortDurationAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="Duration ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortDurationDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="Duration DESC" CssClass="no-line" />
                        </th>
                        <th id="thOpen" runat="server">&#8597;</th>
                        <th id="thDelete" runat="server">x</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr id="trRow" runat="server">
                        <td id="tdID" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "ID") %></td>
                        <td id="tdMatchingActionName" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "MatchingActionName") %></td>
                        <td id="tdStrategyName" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "StrategyName") %></td>
                        <td id="tdStatus" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "Status") %></td>
                        <td id="tdDateStart" runat="server" class="nowrap"><%# String.Format("{0:dd/MM/yyyy HH:mm:ss}", DataBinder.Eval(Container.DataItem, "DateStart")) %></td>
                        <td id="tdDateEnd" runat="server" class="nowrap"><%# String.Format("{0:dd/MM/yyyy HH:mm:ss}", DataBinder.Eval(Container.DataItem, "DateEnd")) %></td>
                        <td id="tdDuration" runat="server" class="nowrap"><%# String.Format("{0:hh\\:mm\\:ss}", DataBinder.Eval(Container.DataItem, "Duration")) %></td>
                        <td id="tdOpen" runat="server" class="nowrap text-center">
                            <a id="lnkOpen" runat="server" href="javascript: void(0);" onclick="javascript: link_open_onmouseup(this);" title="Open" class="no-line">+</a>
                        </td>
                        <td id="tdDelete" runat="server" class="nowrap text-center">
                            <asp:LinkButton ID="btnDelete" runat="server" Text="x" OnClientClick="javascript: return confirm('Delete Item?');" OnCommand="On_Command" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' ToolTip="Delete" ForeColor="Red" CssClass="no-line" />
                        </td>
                        <td id="tdBlank" runat="server" class="nowrap text-center" visible="false">
                            <asp:LinkButton ID="btnBlank" runat="server" Text="" OnCommand="On_Command" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' ToolTip="Delete" ForeColor="Red" CssClass="no-line" />
                        </td>
                    </tr>
                    <tr id="trForm" runat="server" class="no-display">
                        <td colspan="14" class="nowrap">
                            <iframe id="Iframe1" runat="server" height="600" class="frame-form" />
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
