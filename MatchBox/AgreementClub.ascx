<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgreementClub.ascx.cs" Inherits="MatchBox.AgreementClub" %>

<table>
    <tr>
        <td class="nowrap" style="padding: 0px 10px;"><b>Company</b> : <asp:Label ID="lblCompanyName" runat="server" /></td>
        <td class="nowrap" style="padding: 0px 10px;">&nbsp;</td>
        <td class="nowrap" style="padding: 0px 10px;"><b>Credit</b> : <asp:Label ID="lblCreditName" runat="server" /></td>
        <td class="nowrap" style="padding: 0px 10px; width: 100%;">&nbsp;</td>
        <td class="nowrap" style="padding: 0px 10px;"><a href="javascript: void(0);" onclick="javascript: display_agreement_club_form();">Add Club Item</a></td>
    </tr>
</table>

<hr style="margin: 10px;" />

<div style="margin: 10px;">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<div id="divAgreementClubForm" class="no-display">
    <%--ManagementCompany--%>
    <div id="divManagementCompany_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtManagementCompany.ClientID %>" class="block">Management Company</label>
        <asp:TextBox ID="txtManagementCompany" runat="server" MaxLength="50" />
        <asp:RequiredFieldValidator ID="rfvManagementCompany" runat="server" ValidationGroup="AgreementClub" ControlToValidate="txtManagementCompany" ErrorMessage="Enter 'Management Company'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--ContactName--%>
    <div id="divContactName_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtContactName.ClientID %>" class="block">Contact Name</label>
        <asp:TextBox ID="txtContactName" runat="server" MaxLength="50" />
        <asp:RequiredFieldValidator ID="rfvContactName" runat="server" ValidationGroup="AgreementClub" ControlToValidate="txtContactName" ErrorMessage="Enter 'Contact Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <div class="clear-both"></div>

    <%--Phone--%>
    <div id="divPhone_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtPhone.ClientID %>" class="block">Phone</label>
        <asp:TextBox ID="txtPhone" runat="server" MaxLength="15" />
    </div>

    <%--Mail--%>
    <div id="divMail_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtMail.ClientID %>" class="block">Mail</label>
        <asp:TextBox ID="txtMail" runat="server" MaxLength="50" TextMode="Email" />
        <asp:RegularExpressionValidator ID="revMail" runat="server" ValidationGroup="AgreementClub" ControlToValidate="txtMail" Display="Dynamic" SetFocusOnError="true" CssClass="error block"
            ErrorMessage="Enter valid mail." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
    </div>

    <%--Address--%>
    <div id="divAddress_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtAddress.ClientID %>" class="block">Address</label>
        <asp:TextBox ID="txtAddress" runat="server" MaxLength="50" />
    </div>

    <div class="clear-both"></div>

    <%--ClubNumber--%>
    <div id="divClubNumber_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtClubNumber.ClientID %>" class="block">Club Number</label>
        <asp:TextBox ID="txtClubNumber" runat="server" MaxLength="15" />
        <asp:RequiredFieldValidator ID="rfvClubNumber" runat="server" ValidationGroup="AgreementClub" ControlToValidate="txtClubNumber" ErrorMessage="Enter 'Club Number'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:CompareValidator ID="cvClubNumber" runat="server" ValidationGroup="AgreementClub" ControlToValidate="txtClubNumber" Operator="DataTypeCheck" Type="Integer" ErrorMessage="Value must be a whole number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--ClubName--%>
    <div id="divClubName_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtClubName.ClientID %>" class="block">Club Name</label>
        <asp:TextBox ID="txtClubName" runat="server" MaxLength="50" />
        <asp:RequiredFieldValidator ID="rfvClubName" runat="server" ValidationGroup="AgreementClub" ControlToValidate="txtClubName" ErrorMessage="Enter 'Club Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <%--Commission--%>
    <div id="divCommission_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtCommission.ClientID %>" class="block">Commission</label>
        <asp:TextBox ID="txtCommission" runat="server" MaxLength="5" Width="250px" /> %
        <asp:RequiredFieldValidator ID="rfvCommission" runat="server" ValidationGroup="AgreementClub" ControlToValidate="txtCommission" ErrorMessage="Enter 'Commission'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:CompareValidator ID="cvCommission" runat="server" ValidationGroup="AgreementClub" ControlToValidate="txtCommission" Operator="DataTypeCheck" Type="Double" ErrorMessage="'Commission' must be a number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <div class="clear-both"></div>

    <%--Saving--%>
    <div id="divSaving_Change" runat="server" class="float-left div-form-field">
        <label for="<%= txtSaving.ClientID %>" class="block">Saving</label>
        <asp:TextBox ID="txtSaving" runat="server" MaxLength="5" Width="250px" /> %
        <asp:RequiredFieldValidator ID="rfvSaving" runat="server" ValidationGroup="AgreementClub" ControlToValidate="txtSaving" ErrorMessage="Enter 'Saving'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        <asp:CompareValidator ID="cvSaving" runat="server" ValidationGroup="AgreementClub" ControlToValidate="txtSaving" Operator="DataTypeCheck" Type="Double" ErrorMessage="'Saving' must be a number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    </div>

    <asp:CustomValidator ID="cvDateRange" runat="server" ValidationGroup="AgreementClub" OnServerValidate="cvDateRange_ServerValidate" Display="None" CssClass="error block" />

    <%--DateStart--%>
    <div id="divDateStart_Change" runat="server" class="float-left div-form-field">
        <label for="<%= ddlDayStart.ClientID %>" class="block">Start Date</label>
        <asp:DropDownList ID="ddlDayStart" runat="server" style="width: 32%;">
            <asp:ListItem Text="Day" Value="0" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlMonthStart" runat="server" style="width: 32%;">
            <asp:ListItem Text="Month" Value="0" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlYearStart" runat="server" style="width: 32%;">
            <asp:ListItem Text="Year" Value="0" />
        </asp:DropDownList>

        <asp:Label ID="lblDateStartError" runat="server" CssClass="error" />
    </div>

    <%--DateEnd--%>
    <div id="divDateEnd_Change" runat="server" class="float-left div-form-field">
        <label for="<%= ddlDayEnd.ClientID %>" class="block">End Date</label>
        <asp:DropDownList ID="ddlDayEnd" runat="server" style="width: 32%;">
            <asp:ListItem Text="Day" Value="0" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlMonthEnd" runat="server" style="width: 32%;">
            <asp:ListItem Text="Month" Value="0" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlYearEnd" runat="server" style="width: 32%;">
            <asp:ListItem Text="Year" Value="0" />
        </asp:DropDownList>
        
        <asp:Label ID="lblDateEndError" runat="server" CssClass="error" />
    </div>

    <div class="clear-both"></div>

    <div class="div-form-command">
        <br />
        <asp:Button ID="btnSave" runat="server" ValidationGroup="AgreementClub" Text="Save" OnClick="btnSave_Click" />
        &nbsp;
        <a href="javascript: void(0);" onclick="javascript: display_by_class('divAgreementClubForm');">Close</a>
    </div>

    <hr style="margin: 10px;" />
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

                <asp:Repeater ID="repAgreementClub" runat="server">
                    <HeaderTemplate>
                        <table class="table-sub" style="margin: 10px 0px;">
                            <tr>
                                <th>Start Date</th>
                                <th>End Date</th>
                                <th>Management Company</th>
                                <th>Phone</th>
                                <th>Club Number</th>
                                <th>Club Name</th>
                                <th>Commission</th>
                                <th>Saving</th>
                                <th>x</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                            <tr>
                                <td><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateStart")) %></td>
                                <td><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DateEnd")) %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "ManagementCompany") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "Phone") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "ClubNumber") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "ClubName") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "Commission") %>%</td>
                                <td><%# DataBinder.Eval(Container.DataItem, "Saving") %>%</td>
                                <td class="nowrap text-center"><asp:LinkButton ID="btnDeleteAgreementClub" runat="server" OnCommand="btnDeleteAgreementClub_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Text="x" OnClientClick="javascript: return confirm('Delete Item?');" ToolTip="Delete" ForeColor="Red" CssClass="no-line" /></td>
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

<script>
    var b_visible_item_form = (("<%= IsVisibleClubForm %>").toLowerCase() == "true");

    if (b_visible_item_form == true) {
        document.getElementById("divAgreementClubForm").className = "";
    }

    function display_agreement_club_form() {
        display_by_class('divAgreementClubForm');
        document.getElementById("<%= txtManagementCompany.ClientID %>").focus();
    }
</script>
