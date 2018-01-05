<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UserSettings.aspx.cs" Inherits="MatchBox.UserSettings" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" runat="server">
    <h1>Settings</h1>

    <p>
        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
        <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
    </p>

    <div class="div-form-command">
        <asp:Button ID="btnSaveTop" runat="server" Text="Save" OnClick="btnSave_Click" />
    </div>

    <div id="divBank" runat="server" class="float-left" style="padding: 10px; margin: 10px; border: 1px solid black; width: 275px;">
        <h3>Bank</h3>

        <asp:Repeater ID="repBank" runat="server" OnItemDataBound="repBank_ItemDataBound">
            <HeaderTemplate>
                <table>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <a id="lnkBankName" runat="server" href="javascript: void(0);"><%# DataBinder.Eval(Container.DataItem, "BankName") %></a>
                        <br />
                        <asp:CheckBoxList ID="cblBankBranch" runat="server" CssClass="no-display"></asp:CheckBoxList>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div id="divCurrency" runat="server" class="float-left" style="padding: 10px; margin: 10px; border: 1px solid black; width: 275px;">
        <h3>Currency</h3>

        <asp:Repeater ID="repCurrency" runat="server">
            <HeaderTemplate>
                <table>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkIsSelected" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsSelected")) %>' />
                            <asp:HiddenField ID="hidIsDefault" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "IsDefault") %>' />
                            <asp:HiddenField ID="hidCurrencyID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CurrencyID") %>' />
                        </td>
                        <td>
                            <asp:Label ID="lblCurrencyName" AssociatedControlID="chkIsSelected" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CurrencyName") + " - " + DataBinder.Eval(Container.DataItem, "CountryName") %>' />
                        </td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div id="divDataSource" runat="server" class="float-left" style="padding: 10px; margin: 10px; border: 1px solid black; width: 275px;">
        <h3>Data Source</h3>

        <a class="block" href="javascript: void(0);" onclick="javascript: display_by_class('divDataSourceEdit');">Add Data Source</a>

        <div id="divDataSourceEdit" class="no-display">
            <asp:TextBox ID="txtDataSourceName" runat="server" MaxLength="50" />
            <asp:HiddenField ID="hidDataSourceID" runat="server" Value="0" />
            <asp:Button ID="btnDataSource" runat="server" ValidationGroup="DataSource" Width="60px" Text="Save" OnCommand="btnDataSource_Save" />
            <asp:RequiredFieldValidator ID="rfvDataSourceName" runat="server" ValidationGroup="DataSource" ControlToValidate="txtDataSourceName" ErrorMessage="Enter 'Data Source Name'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
        </div>
        
        <asp:Repeater ID="repDataSource" runat="server">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                    <li><%# DataBinder.Eval(Container.DataItem, "DataSourceName") %></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div id="divCreditCompany" runat="server" class="float-left" style="padding: 10px; margin: 10px; border: 1px solid black; width: 275px;">
        <h3>Credit Company</h3>

        <asp:Repeater ID="repCredit" runat="server">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                    <li><%# DataBinder.Eval(Container.DataItem, "CreditName") %></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div id="divDiscountCompany" runat="server" class="float-left" style="padding: 10px; margin: 10px; border: 1px solid black; width: 275px;">
        <h3>Discount Company</h3>

        <asp:Repeater ID="repDiscount" runat="server">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                    <li><%# DataBinder.Eval(Container.DataItem, "DiscountName") %></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div class="clear-both"></div>

    <div class="div-form-command">
        <asp:Button ID="btnSaveBottom" runat="server" Text="Save" OnClick="btnSave_Click" />
    </div>
</asp:Content>
