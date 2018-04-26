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

        <div id="divMessage" runat="server" enableviewstate="false" visible="true" class="div-message">
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
            <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
        </div>

        <hr />

        <div class="clear-both"></div>

        <div id="divCompany" runat="server" class="float-left div-form-field">
            <label for="<%= ddlCompany.ClientID %>" class="block">Company</label>
            <asp:DropDownList ID="ddlCompany" runat="server">
                <%--<asp:ListItem Text="" Value="0" />--%>
            </asp:DropDownList>
        </div>

        <div id="divCreditCompany" runat="server" class="float-none div-form-field">

            <label for="<%= ddlCreditCompany.ClientID %>" class="block">Credit Company</label>
            <asp:DropDownList ID="ddlCreditCompany" runat="server">
                <%--<asp:ListItem Text="" Value="0" />--%>
            </asp:DropDownList>
            <%-- <asp:RequiredFieldValidator ID="rfvCredit" runat="server" ControlToValidate="ddlCreditCompany" InitialValue="0" ErrorMessage="Select 'Credit'." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />--%>
        </div>

        <div id="divCommissionType" runat="server" class="float-none div-form-field">

            <label for="<%= ddlCommissionType.ClientID %>" class="block">Commission Type</label>
            <asp:DropDownList ID="ddlCommissionType" runat="server">
                <%--<asp:ListItem Text="" Value="0" />--%>
            </asp:DropDownList>
        </div>

        <div id="divPaymentDate" runat="server" class="float-left div-form-field-long">
            <b>Payment Date</b>
            <br />
            <asp:TextBox ID="txtPaymentDate" runat="server" Width="60%" />
            <%--<asp:Label ID="lblTransactionDateError" runat="server" EnableViewState="false" CssClass="error block" />--%>
        </div>


        <div class="clear-both"></div>

        <div class="div-form-command">
            <%--<asp:Button ID="Button1" runat="server" OnClick="btnSearch_Click_New" OnClientClick="javascript: show_wait(this.id, 'spnSearch', false);" Text="Search" Visible="false" />
        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click_New" OnClientClick="javascript: show_wait(this.id, 'spnSearch', false);" Text="Search" />--%>

            <asp:Button ID="btnSaveCommission" runat="server" OnClick="btnSaveCommission_Click" OnClientClick="javascript: show_wait(this.id, 'spnSearch', false);" Text="Save" />
            <span id="spnSearch" class="message" style="display: none;">Please wait..</span>
            &nbsp;
        <a href="Commissions.aspx">Reset</a>
        </div>
    </div>


    <script>
            //function show_wait(s_button_id, s_span_id, b_validate) {
            //    if (b_validate == true && Page_ClientValidate() == false) { return; }

            //    document.getElementById(s_button_id).style.display = "none";
            //    document.getElementById(s_span_id).style.display = "";
            //}
    </script>
</asp:Content>
