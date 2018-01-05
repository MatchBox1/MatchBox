<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrameForm.aspx.cs" Inherits="MatchBox.FrameForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="content-type" content="text/html;charset=UTF-8" />
    <meta name="viewport" content="width=device-width" />
    <link href="App_CSS/Main.css" rel="stylesheet" />
    <script src="App_JS/Main.js"></script>
    <script type="text/javascript">
        check_host();

        function check_host() {
            var b_redirect = false;

            if (this == this.parent) { b_redirect = true; }
            if (location.host != parent.location.host) { b_redirect = true; }

            if (b_redirect) { document.location.href = "Error.aspx"; }
        }

        function check_result() {
            var hid_result = document.getElementById("<%= hidResult.ClientID %>");

            switch (hid_result.value) {
                case "updated":
                    after_update();
                    break;
                case "inserted":
                    after_insert();
                    break;
                default:
                    return;
            }

            hid_result.value = "";
        }

        function after_update() {
            var arr_frames = parent.document.getElementsByClassName("frame-form");
            var arr_query = location.search.replace("?", "").split("&");
            var i_param = "", q_param = "";

            for (var i = 0; i < arr_query.length; i++) {
                if (arr_query[i].startsWith("i=")) {
                    i_param = arr_query[i];
                }

                if (arr_query[i].startsWith("q=")) {
                    q_param = arr_query[i];
                }
            }

            for (var i = 0; i < arr_frames.length; i++) {
                var o_frame = arr_frames[i];

                if (o_frame.src.indexOf(i_param) >= 0 && o_frame.src.indexOf(q_param) >= 0) {
                    var s_id = o_frame.parentNode.parentNode.id.replace("trForm", "trRow");
                    var o_row = parent.document.getElementById(s_id);
                    var o_span = parent.document.getElementById("spanReload");

                    o_row.className = "updated";
                    o_span.innerHTML = "*";

                    break;
                }
            }
        }

        function after_insert() {
            var s_href = new String(parent.location.href);
            var s_result = "?result=inserted";

            if (s_href.indexOf(s_result) == -1) { s_href += s_result; }

            parent.location.href = s_href;
        }
    </script>
</head>
<body>
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />

    <form id="frmFrameForm" runat="server">
        <asp:ScriptManager ID="smFrameForm" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hidResult" runat="server" />
    </form>

    <script type="text/javascript">
        check_result();
    </script>
</body>
</html>
