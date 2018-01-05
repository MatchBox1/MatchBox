<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MatchingDetails.ascx.cs" Inherits="MatchBox.MatchingDetails" %>

<table>
    <tr>
        <td class="nowrap"><h2 id="hedTitle" runat="server" class="hed-form"></h2></td>
        <td class="nowrap">&nbsp; | &nbsp;</td>
        <td class="nowrap"><a id="lnkHome" runat="server">Matching Queries</a></td>
        <td class="nowrap">&nbsp; | &nbsp;</td>
        <td class="nowrap"><a id="lnkReturn" runat="server">Return</a></td>
        <td class="nowrap">&nbsp; | &nbsp;</td>
        <td class="nowrap" id="tdRecalculate" runat="server"><asp:LinkButton ID="btnRecalculate" runat="server" OnClick="btnRecalculate_Click" OnClientClick="javascript: return recalculate_click();" Text="Recalculate" /></td>
        <td class="nowrap" id="tdSaveChanges" runat="server" visible="false">
            <asp:Button ID="btnSaveChanges" runat="server" OnClick="btnSaveChanges_Click" Text="Save Changes" />
            &nbsp;
            <a id="lnkCancelChanges" runat="server">Cancel</a>
        </td>
        <td class="nowrap">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
        <td>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
            <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
        </td>
    </tr>
</table>

<asp:HiddenField ID="hidSelectInside" runat="server" />
<asp:HiddenField ID="hidSelectOutside" runat="server" />

<div>
    <div class="float-left div-form-item nowrap" style="width: 48%;">
        <b>Inside</b>
        <div style="width: 100%; height: 450px; overflow: auto;">
            <asp:GridView ID="gvMatchingInside" runat="server" AutoGenerateColumns="true" OnRowDataBound="gvMatchingInside_RowDataBound" CssClass="table-sub" />
        </div>
        <div class="div-form-message" style="margin: 5px 0px;">
            <div id="divMatchingInsideControl" runat="server">
                <b>Rows</b> : <asp:Label ID="lblMatchingInsideRows" runat="server" />
                &nbsp;&nbsp;&nbsp;
                <b>Amount</b> : <asp:Label ID="lblMatchingInsideAmount" runat="server" />
                &nbsp;&nbsp;&nbsp;
                <b>Page</b> : <asp:DropDownList ID="ddlMatchingInsidePage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Page_Changed" Width="100px" />
            </div>
            <div id="divMatchingInsideMessage" runat="server" visible="false"></div>
        </div>
    </div>

    <div class="float-right div-form-item nowrap" style="width: 48%;">
        <b>Outside</b>
        <div style="width: 100%; height: 450px; overflow: auto;">
            <asp:GridView ID="gvMatchingOutside" runat="server" AutoGenerateColumns="true" OnRowDataBound="gvMatchingOutside_RowDataBound" CssClass="table-sub" />
        </div>
        <div class="div-form-message" style="margin: 5px 0px;">
            <div id="divMatchingOutsideControl" runat="server">
                <b>Rows</b> : <asp:Label ID="lblMatchingOutsideRows" runat="server" />
                &nbsp;&nbsp;&nbsp;
                <b>Amount</b> : <asp:Label ID="lblMatchingOutsideAmount" runat="server" />
                &nbsp;&nbsp;&nbsp;
                <b>Page</b> : <asp:DropDownList ID="ddlMatchingOutsidePage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Page_Changed" Width="100px" />
            </div>
            <div id="divMatchingOutsideMessage" runat="server" visible="false"></div>
        </div>
    </div>

    <div class="clear-both"></div>
</div>

<script>
    try { document.getElementById("<%= gvMatchingInside.ClientID %>").attributes.removeNamedItem("style"); } catch (ex) { }
    try { document.getElementById("<%= gvMatchingOutside.ClientID %>").attributes.removeNamedItem("style"); } catch (ex) { }

    function recalculate_click() {
        if (document.getElementById("<%= hidSelectInside.ClientID %>").value == "" && document.getElementById("<%= hidSelectOutside.ClientID %>").value == "") {
            alert("Unselect item/s to recalculate.");
            return false;
        }
        else {
            return true;
        }
    }

    function select_data_item_all(s_class, b_checked) {
        var chkSelectItemArray = document.getElementsByClassName(s_class);

        var s_array = checkbox_check_all(chkSelectItemArray, b_checked, false);

        update_hidden_field(s_class, s_array);
    }

    function select_data_item(s_class, o_checkbox) {
        var chkSelectItemArray = document.getElementsByClassName(s_class);

        var s_array = checkbox_check(chkSelectItemArray, false);

        update_hidden_field(s_class, s_array);
    }

    function update_hidden_field(s_class, s_id_array) {
        switch (s_class) {
            case "checkbox-inside":
                document.getElementById("<%= hidSelectInside.ClientID %>").value = s_id_array;
                break;
            case "checkbox-outside":
                document.getElementById("<%= hidSelectOutside.ClientID %>").value = s_id_array;
                break;
        }
    }
</script>
