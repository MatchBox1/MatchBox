<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DataFieldPriority.aspx.cs" Inherits="MatchBox.DataFieldPriority" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <table style="width: 100%;">
        <tr class="nowrap">
            <td><h1 id="hedMain" runat="server" class="float-left">Columns Settings</h1></td>
            <td class="nowrap">&nbsp; &nbsp; - &nbsp; &nbsp;</td>
            <td><a href="MatchingView.aspx">Matchings</a></td>
            <td>&nbsp;|&nbsp;</td>
            <td><a href="DataInspector.aspx">Data Inspector</a></td>
            <td style="width: 100%;"">&nbsp;</td>
        </tr>
    </table>

    <div id="divMessage" runat="server" EnableViewState="false" Visible="false" class="div-message">
        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
        <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
    </div>

    <div style="margin: 20px 0px;">
        <asp:Button ID="btnSaveTop" runat="server" Text="Save" OnClick="Save_Settings" Width="150px" />
        &nbsp;
        <asp:LinkButton ID="btnDefaultTop" runat="server" Text="Default Settings" OnClick="Default_Settings" />
    </div>

    <div class="float-left">
        <b>Inside Sort Order</b>
        <table class="table-sub">
            <tr>
                <th>Priority</th>
                <th>Field</th>
                <th>Sort Order</th>
            </tr>
            <tr>
                <td>1</td>
                <td><asp:DropDownList ID="ddlInsideSort_1" runat="server" /></td>
                <td>
                    <asp:DropDownList ID="ddlInsideOrder_1" runat="server">
                        <asp:ListItem Value="ASC" Text="Ascending" />
                        <asp:ListItem Value="DESC" Text="Descending" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>2</td>
                <td><asp:DropDownList ID="ddlInsideSort_2" runat="server" /></td>
                <td>
                    <asp:DropDownList ID="ddlInsideOrder_2" runat="server">
                        <asp:ListItem Value="ASC" Text="Ascending" />
                        <asp:ListItem Value="DESC" Text="Descending" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>3</td>
                <td><asp:DropDownList ID="ddlInsideSort_3" runat="server" /></td>
                <td>
                    <asp:DropDownList ID="ddlInsideOrder_3" runat="server">
                        <asp:ListItem Value="ASC" Text="Ascending" />
                        <asp:ListItem Value="DESC" Text="Descending" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />

        <b>Inside Field Priority</b>
        <asp:Repeater ID="repInsideFieldPriority" runat="server">
            <HeaderTemplate>
                <table class="table-sub">
                    <tr>
                        <th>Field</th>
                        <th>Priority</th>
                        <th>Top</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td><%# DataBinder.Eval(Container.DataItem, "FieldDescription") %><asp:HiddenField ID="hidFieldName" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "FieldName") %>' /></td>
                        <td><asp:TextBox ID="txtPriority" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Priority") %>' MaxLength="2" Width="50px" /></td>
                        <td><asp:CheckBox ID="chkDisplay" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Checked")) %>' /></td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div class="float-left" style="width: 100px;">&nbsp;</div>

    <div class="float-left">
        <b>Outside Sort Order</b>
        <table class="table-sub">
            <tr>
                <th>Priority</th>
                <th>Field</th>
                <th>Sort Order</th>
            </tr>
            <tr>
                <td>1</td>
                <td><asp:DropDownList ID="ddlOutsideSort_1" runat="server" /></td>
                <td>
                    <asp:DropDownList ID="ddlOutsideOrder_1" runat="server">
                        <asp:ListItem Value="ASC" Text="Ascending" />
                        <asp:ListItem Value="DESC" Text="Descending" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>2</td>
                <td><asp:DropDownList ID="ddlOutsideSort_2" runat="server" /></td>
                <td>
                    <asp:DropDownList ID="ddlOutsideOrder_2" runat="server">
                        <asp:ListItem Value="ASC" Text="Ascending" />
                        <asp:ListItem Value="DESC" Text="Descending" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>3</td>
                <td><asp:DropDownList ID="ddlOutsideSort_3" runat="server" /></td>
                <td>
                    <asp:DropDownList ID="ddlOutsideOrder_3" runat="server">
                        <asp:ListItem Value="ASC" Text="Ascending" />
                        <asp:ListItem Value="DESC" Text="Descending" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />

        <b>Outside Field Priority</b>
        <asp:Repeater ID="repOutsideFieldPriority" runat="server">
            <HeaderTemplate>
                <table class="table-sub">
                    <tr>
                        <th>Field</th>
                        <th>Priority</th>
                        <th>Top</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td><%# DataBinder.Eval(Container.DataItem, "FieldDescription") %><asp:HiddenField ID="hidFieldName" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "FieldName") %>' /></td>
                        <td><asp:TextBox ID="txtPriority" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Priority") %>' MaxLength="2" Width="50px" /></td>
                        <td><asp:CheckBox ID="chkDisplay" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "Checked")) %>' /></td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div class="clear-both"></div>

    <div style="margin: 20px 0px;">
        <asp:Button ID="btnSaveBottom" runat="server" Text="Save" OnClick="Save_Settings" Width="150px" />
        &nbsp;
        <asp:LinkButton ID="btnDefaultBottom" runat="server" Text="Default Settings" OnClick="Default_Settings" />
    </div>
</asp:Content>
