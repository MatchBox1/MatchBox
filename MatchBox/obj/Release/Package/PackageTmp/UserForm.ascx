<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserForm.ascx.cs" Inherits="MatchBox.UserForm" %>

<h2 id="hedTitle" runat="server" class="hed-form"></h2>

<div class="div-form-item">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<%--Status--%>
<div id="divStatus_Change" runat="server" class="float-left div-form-field">
    <label for="<%= ddlStatus.ClientID %>" class="block">Status</label>
    <asp:DropDownList ID="ddlStatus" runat="server">
        <asp:ListItem Text="" Value="" />
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus" InitialValue="" ErrorMessage="Select 'Status'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--FullName--%>
<div id="divFullName_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtFullName.ClientID %>" class="block">Full Name</label>
    <asp:TextBox ID="txtFullName" runat="server" MaxLength="25" />
    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Enter 'Full Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--UserName--%>
<div id="divUserName_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtUserName.ClientID %>" class="block">UserName</label>
    <asp:TextBox ID="txtUserName" runat="server" MaxLength="25" />
    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName" ErrorMessage="Enter 'User Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--Password--%>
<div id="divPassword_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtPassword.ClientID %>" class="block">Password</label>
    <asp:TextBox ID="txtPassword" runat="server" MaxLength="25" />
    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Enter 'Password'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<%--Mail--%>
<div id="divMail_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtMail.ClientID %>" class="block">Mail</label>
    <asp:TextBox ID="txtMail" runat="server" MaxLength="50" TextMode="Email" />
    <asp:RequiredFieldValidator ID="rfvMail" runat="server" ControlToValidate="txtMail" ErrorMessage="Enter 'Mail'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    <asp:RegularExpressionValidator ID="revMail" runat="server" ControlToValidate="txtMail" Display="Dynamic" SetFocusOnError="true" CssClass="error block"
        ErrorMessage="Enter valid mail." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
</div>

<%--Phone--%>
<div id="divPhone_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtPhone.ClientID %>" class="block">Home Phone</label>
    <asp:TextBox ID="txtPhone" runat="server" MaxLength="10" />
</div>

<%--Mobile--%>
<div id="divMobile_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtMobile.ClientID %>" class="block">Mobile Phone</label>
    <asp:TextBox ID="txtMobile" runat="server" MaxLength="10" />
</div>

<%--City--%>
<div id="divCity_Change" runat="server" class="float-left div-form-field">
    <label for="<%= ddlCity.ClientID %>" class="block">City</label>
    <asp:DropDownList ID="ddlCity" runat="server">
        <asp:ListItem Text="" Value="0" />
    </asp:DropDownList>
</div>

<%--Address--%>
<div id="divAddress_Change" runat="server" class="float-left div-form-field">
    <label for="<%= txtAddress.ClientID %>" class="block">Address</label>
    <asp:TextBox ID="txtAddress" runat="server" MaxLength="50" />
</div>

<div class="clear-both"></div>

<div id="divService_Change" runat="server" visible="false" class="div-form-field">
    <b>Service</b>

    <asp:Label ID="lblServiceError" runat="server" Visible="false" EnableViewState="false" CssClass="error block" />

    <asp:Repeater ID="repService" runat="server">
        <HeaderTemplate>
            <table style="border: 1px solid #cccccc; width: 100%;">
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label ID="lblServiceID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ServiceID") %>' />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsSelected" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsSelected")) %>' />
                        <asp:HiddenField ID="hidServiceID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ServiceID") %>' />
                        <asp:HiddenField ID="hidDependencyID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "DependencyID") %>' />
                    </td>
                    <td style="width: 100%;">
                        <asp:Label ID="lblServiceName" AssociatedControlID="chkIsSelected" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ServiceName") %>' />
                    </td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
<div id="divService_View" runat="server" visible="false">
    <div id="divService_View_Selected" runat="server" visible="false" class="float-left div-form-field">
        <b>Selected Services</b>

        <asp:Repeater ID="repServiceSelected" runat="server">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                    <li><%# DataBinder.Eval(Container.DataItem, "ServiceName") %></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div id="divService_View_Available" runat="server" visible="false" class="float-left div-form-field">
        <b>Available Services</b>

        <asp:Repeater ID="repServiceAvailable" runat="server">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                    <li><%# DataBinder.Eval(Container.DataItem, "ServiceName") %></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="clear-both"></div>
</div>

<div class="div-form-command">
    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
</div>
