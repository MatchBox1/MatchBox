<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DataFileView.aspx.cs" Inherits="MatchBox.DataFileView" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <table style="width: 100%;">
        <tr class="nowrap">
            <td><h1 id="hedMain" runat="server" class="float-left">Data Files</h1></td>
            <td style="width: 100%;"">&nbsp;</td>
            <td><a href="javascript: void(0);" onclick="javascript: display_section('section-search', 'block');">Show search</a></td>
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
    
    <section id="secSearch" runat="server" class="section-search">
        <h3 class="hed-search">Search</h3>

        <%--FileName--%>
        <div id="divFileName_Search" runat="server" class="float-left div-form-field">
            <label for="<%= txtFileNameSearch.ClientID %>" class="block">File Name</label>
            <asp:TextBox ID="txtFileNameSearch" runat="server" MaxLength="50" />
        </div>
        <%--Template--%>
        <div id="divTemplate_Search" runat="server" class="float-left div-form-field">
            <label for="<%= ddlTemplateSearch.ClientID %>" class="block">Template</label>
            <asp:DropDownList ID="ddlTemplateSearch" runat="server">
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
                        <th id="thCompanyName" runat="server">
                            Company
                            <asp:LinkButton ID="btnSortCompanyNameAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="CompanyName ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortCompanyNameDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="CompanyName DESC" CssClass="no-line" />
                        </th>
                        <th id="thTemplateName" runat="server">
                            Template
                            <asp:LinkButton ID="btnSortTemplateNameAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="TemplateName ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortTemplateNameDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="TemplateName DESC" CssClass="no-line" />
                        </th>
                        <th id="thDateFrom" runat="server">
                            Date From
                            <asp:LinkButton ID="btnSortDateFromAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="DateFrom ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortDateFromDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="DateFrom DESC" CssClass="no-line" />
                        </th>
                        <th id="thDateTo" runat="server">
                            Date To
                            <asp:LinkButton ID="btnSortDateToAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="DateTo ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortDateToDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="DateTo DESC" CssClass="no-line" />
                        </th>
                        <th id="thDataRowsCount" runat="server">
                            Data Rows
                            <asp:LinkButton ID="btnSortDataRowsCountAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="DataRowsCount ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortDataRowsCountDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="DataRowsCount DESC" CssClass="no-line" />
                        </th>
                        <%--
                        <th id="thExcelRowsCount" runat="server">
                            Excel Rows
                            <asp:LinkButton ID="btnSortExcelRowsCountAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="ExcelRowsCount ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortExcelRowsCountDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="ExcelRowsCount DESC" CssClass="no-line" />
                        </th>
                        --%>
                        <th id="thDateUpload" runat="server">
                            Upload Date
                            <asp:LinkButton ID="btnSortDateUploadAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="DateUpload ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortDateUploadDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="DateUpload DESC" CssClass="no-line" />
                        </th>
                        <th id="thIsValid" runat="server">
                            Valid
                            <asp:LinkButton ID="btnSortIsValidAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="IsValid ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortIsValidDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="IsValid DESC" CssClass="no-line" />
                        </th>
                        <th id="thOpen" runat="server">&#8597;</th>
                        <th id="thDelete" runat="server">x</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr id="trRow" runat="server">
                        <td id="tdID" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "ID") %></td>
                        <td id="tdCompanyName" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "CompanyName") %></td>
                        <td id="tdTemplateName" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "TemplateName") %></td>
                        <td id="tdDateFrom" runat="server" class="nowrap"><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateFrom")) %></td>
                        <td id="tdDateTo" runat="server" class="nowrap"><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateTo")) %></td>
                        <td id="tdDataRowsCount" runat="server" class="nowrap"><%# String.Format("{0:n0}", DataBinder.Eval(Container.DataItem, "DataRowsCount")) %></td>
                        <%--
                        <td id="tdExcelRowsCount" runat="server" class="nowrap"><%# String.Format("{0:n0}", DataBinder.Eval(Container.DataItem, "ExcelRowsCount")) %></td>
                        --%>
                        <td id="tdDateUpload" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "DateUpload") %></td>
                        <td id="tdIsValid" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "IsValid") %></td>
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
