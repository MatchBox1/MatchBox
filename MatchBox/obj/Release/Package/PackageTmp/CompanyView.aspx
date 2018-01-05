<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CompanyView.aspx.cs" Inherits="MatchBox.CompanyView" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <table style="width: 100%;">
        <tr class="nowrap">
            <td><h1 id="hedMain" runat="server">Companies</h1></td>
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
        
        <%--CompanyNumber--%>
        <div id="divCompanyNumber_Search" runat="server" class="float-left div-form-field">
            <label for="<%= txtCompanyNumberSearch.ClientID %>" class="block">Company Number</label>
            <asp:TextBox ID="txtCompanyNumberSearch" runat="server" MaxLength="15" />
        </div>
        <%--CompanyName--%>
        <div id="divCompanyName_Search" runat="server" class="float-left div-form-field">
            <label for="<%= txtCompanyNameSearch.ClientID %>" class="block">Company Name</label>
            <asp:TextBox ID="txtCompanyNameSearch" runat="server" MaxLength="50" />
        </div>
        <%--Phone / Fax--%>
        <div id="divPhone_Search" runat="server" class="float-left div-form-field">
            <label for="<%= txtPhoneSearch.ClientID %>" class="block">Phone / Fax</label>
            <asp:TextBox ID="txtPhoneSearch" runat="server" MaxLength="15" />
        </div>
        <%--Mail--%>
        <div id="divMail_Search" runat="server" class="float-left div-form-field">
            <label for="<%= txtMailSearch.ClientID %>" class="block">Mail</label>
            <asp:TextBox ID="txtMailSearch" runat="server" MaxLength="50" />
        </div>
        <%--City--%>
        <div id="divCity_Search" runat="server" class="float-left div-form-field">
            <label for="<%= ddlCitySearch.ClientID %>" class="block">City</label>
            <asp:DropDownList ID="ddlCitySearch" runat="server">
                <asp:ListItem Text="" Value="0" />
            </asp:DropDownList>
        </div>
        <%--Address--%>
        <div id="divAddress_Search" runat="server" class="float-left div-form-field">
            <label for="<%= txtAddressSearch.ClientID %>" class="block">Address</label>
            <asp:TextBox ID="txtAddressSearch" runat="server" MaxLength="50" />
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
                        <th id="thCompanyNumber" runat="server">
                            Company Number
                            <asp:LinkButton ID="btnSortNameAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="CompanyNumber ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortNameDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="CompanyNumber DESC" CssClass="no-line" />
                        </th>
                        <th id="thCompanyName" runat="server">
                            Company Name
                            <asp:LinkButton ID="btnSortCompanyNameAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="CompanyName ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortCompanyNameDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="CompanyName DESC" CssClass="no-line" />
                        </th>
                        <th id="thPhone" runat="server">
                            Phone
                            <asp:LinkButton ID="btnSortPhoneAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="Phone ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortPhoneDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="Phone DESC" CssClass="no-line" />
                        </th>
                        <th id="thFax" runat="server">
                            Fax
                            <asp:LinkButton ID="btnSortFaxAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="Fax ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortFaxDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="Fax DESC" CssClass="no-line" />
                        </th>
                        <th id="thMail" runat="server">
                            Mail
                            <asp:LinkButton ID="btnSortMailAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="Mail ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortMailDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="Mail DESC" CssClass="no-line" />
                        </th>
                        <th id="thCity" runat="server">
                            City
                            <asp:LinkButton ID="btnSortCityAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="City ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortCityDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="City DESC" CssClass="no-line" />
                        </th>
                        <th id="thAddress" runat="server">
                            Address
                            <asp:LinkButton ID="btnSortAddressAsc" runat="server" Text="&darr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="Address ASC" CssClass="no-line" />
                            <asp:LinkButton ID="btnSortAddressDesc" runat="server" Text="&uarr;" OnCommand="On_Command" CommandName="Sort" CommandArgument="Address DESC" CssClass="no-line" />
                        </th>
                        <th id="thOpen" runat="server">&#8597;</th>
                        <th id="thDelete" runat="server">x</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr id="trRow" runat="server">
                        <td id="tdCompanyNumber" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "CompanyNumber") %></td>
                        <td id="tdCompanyName" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "CompanyName") %></td>
                        <td id="tdPhone" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "Phone") %></td>
                        <td id="tdFax" runat="server" class="nowrap"><%# DataBinder.Eval(Container.DataItem, "Fax") %></td>
                        <td id="tdMail" runat="server" class="nowrap"><a id="lnkMail" runat="server" visible="false">Send</a></td>
                        <td id="tdCity" runat="server"><%# DataBinder.Eval(Container.DataItem, "City") %></td>
                        <td id="tdAddress" runat="server"><%# DataBinder.Eval(Container.DataItem, "Address") %></td>
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
