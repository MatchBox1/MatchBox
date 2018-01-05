<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MatchingForm.ascx.cs" Inherits="MatchBox.MatchingForm" %>

<table>
    <tr>
        <td class="nowrap"><h2 id="hedTitle" runat="server" class="hed-form"></h2></td>
        <td class="nowrap">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
        <td>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
            <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
        </td>
    </tr>
</table>

<div class="div-form-message">
    <table class="table-list">
        <tr>
            <td class="bold" colspan="2">Matching Transactions</td>
            <td>&nbsp;&nbsp;&nbsp;</td>
            <td class="bold" colspan="2">All Transactions</td>
            <td>&nbsp;&nbsp;&nbsp;</td>
            <td class="bold" colspan="2">Not Matching Transactions</td>
        </tr>
        <tr>
            <td>Inside Count</td>
            <td><asp:Label ID="lblMatchingInsideCount" runat="server" /></td>
            <td>&nbsp;&nbsp;&nbsp;</td>
            <td>Inside Count</td>
            <td><asp:Label ID="lblAllInsideCount" runat="server" /></td>
            <td>&nbsp;&nbsp;&nbsp;</td>
            <td>Inside Count</td>
            <td><asp:Label ID="lblNotMatchingInsideCount" runat="server" /></td>
        </tr>
        <tr>
            <td>Outside Count</td>
            <td><asp:Label ID="lblMatchingOutsideCount" runat="server" /></td>
            <td>&nbsp;&nbsp;&nbsp;</td>
            <td>Outside Count</td>
            <td><asp:Label ID="lblAllOutsideCount" runat="server" /></td>
            <td>&nbsp;&nbsp;&nbsp;</td>
            <td>Outside Count</td>
            <td><asp:Label ID="lblNotMatchingOutsideCount" runat="server" /></td>
        </tr>
        <tr>
            <td>Inside Amount</td>
            <td><asp:Label ID="lblMatchingInsideAmount" runat="server" /></td>
            <td>&nbsp;&nbsp;&nbsp;</td>
            <td>Inside Amount</td>
            <td><asp:Label ID="lblAllInsideAmount" runat="server" /></td>
            <td>&nbsp;&nbsp;&nbsp;</td>
            <td>Inside Amount</td>
            <td><asp:Label ID="lblNotMatchingInsideAmount" runat="server" /></td>
        </tr>
        <tr>
            <td>Outside Amount</td>
            <td><asp:Label ID="lblMatchingOutsideAmount" runat="server" /></td>
            <td>&nbsp;&nbsp;&nbsp;</td>
            <td>Outside Amount</td>
            <td><asp:Label ID="lblAllOutsideAmount" runat="server" /></td>
            <td>&nbsp;&nbsp;&nbsp;</td>
            <td>Outside Amount</td>
            <td><asp:Label ID="lblNotMatchingOutsideAmount" runat="server" /></td>
        </tr>
    </table>
</div>

<div class="div-form-item">
    <div style="margin: 10px 0px;">
        <b>Matching Queries</b>
        &nbsp; / &nbsp;
        <asp:LinkButton ID="btnMatchingQueryView" runat="server" OnClick="btnMatchingQueryView_Click" OnClientClick="javascript: return matching_query_view_click();" Text="View Selected Item/s Details" />
        &nbsp; / &nbsp;
        <asp:LinkButton ID="btnMatchingQueryDelete" runat="server" OnClick="btnMatchingQueryDelete_Click" OnClientClick="javascript: return matching_query_delete_click();" Text="Delete Selected Item/s" />
        <asp:HiddenField ID="hidMatchingQueryNumber" runat="server" />
    </div>

    <asp:GridView ID="gvMatchingSummary" runat="server" AutoGenerateColumns="true" OnRowDataBound="gvMatchingSummary_RowDataBound" CssClass="table-center" />
</div>

<script>
    try { document.getElementById("<%= gvMatchingSummary.ClientID %>").attributes.removeNamedItem("style"); } catch (ex) { }

    function select_matching_query_all(b_checked) {
        var chkSelectQueryArray = document.getElementsByClassName("SelectQuery");

        var s_query_number_array = checkbox_check_all(chkSelectQueryArray, b_checked, true);

        document.getElementById("<%= hidMatchingQueryNumber.ClientID %>").value = s_query_number_array;
    }

    function select_matching_query() {
        var chkSelectQueryArray = document.getElementsByClassName("SelectQuery");

        var s_query_number_array = checkbox_check(chkSelectQueryArray, true);

        document.getElementById("<%= hidMatchingQueryNumber.ClientID %>").value = s_query_number_array;
    }

    function matching_query_view_click() {
        if (document.getElementById("<%= hidMatchingQueryNumber.ClientID %>").value == "") {
            alert("Select item/s to view details.");
            return false;
        }
        else {
            return true;
        }
    }

    function matching_query_delete_click() {
        if (document.getElementById("<%= hidMatchingQueryNumber.ClientID %>").value == "") {
            alert("Select item/s to delete.");
            return false;
        }
        else {
            return confirm('Delete Item/s?');
        }
    }
</script>
