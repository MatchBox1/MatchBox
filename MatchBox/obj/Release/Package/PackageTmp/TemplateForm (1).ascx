<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplateForm.ascx.cs" Inherits="MatchBox.TemplateForm" %>

<h2 id="hedTitle" runat="server" class="hed-form"></h2>

<div class="div-form-item">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="message block" />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="error block" />
</div>

<div class="div-form-command">
    <asp:Button ID="btnSaveTop" runat="server" Text="Save" OnClick="Save_Click" />
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
    Status:
    <asp:Label ID="lblStatusTop" runat="server" Text="New" />
</div>

<%--TemplateFor--%>
<div id="divTemplateFor" runat="server" class="float-left div-form-field">
    <b>Template For:</b>
    <br />
    <asp:RadioButton ID="rdoDataSource" runat="server" GroupName="TemplateFor" Text="Inside Data Source" AutoPostBack="true" OnCheckedChanged="Check_Template_For" />
    <br />
    <asp:DropDownList ID="ddlDataSource" runat="server" Enabled="false">
        <asp:ListItem Text="" Value="0" />
    </asp:DropDownList>
    <br />
    <asp:RadioButton ID="rdoCredit" runat="server" GroupName="TemplateFor" Text="Credit" AutoPostBack="true" OnCheckedChanged="Check_Template_For" />
    <br />
    <asp:DropDownList ID="ddlCredit" runat="server" Enabled="false">
        <asp:ListItem Text="" Value="0" />
    </asp:DropDownList>
    <br />
    <asp:RadioButton ID="rdoDiscount" runat="server" GroupName="TemplateFor" Text="Discount" AutoPostBack="true" OnCheckedChanged="Check_Template_For" />
    <br />
    <asp:DropDownList ID="ddlDiscount" runat="server" Enabled="false">
        <asp:ListItem Text="" Value="0" />
    </asp:DropDownList>
    <asp:CustomValidator ID="cvTemplateFor" runat="server" OnServerValidate="cvTemplateFor_ServerValidate" CssClass="error block" />
</div>

<%--UploadExcel--%>
<div id="divUploadExcel_Change" runat="server" class="float-left div-form-field">
    <label for="<%= fuUploadExcel.ClientID %>" class="block">Upload Excel - Formatted as text</label>
    <asp:FileUpload ID="fuUploadExcel" runat="server" EnableViewState="false" style="width: 50%;" />
    <asp:TextBox ID="txtHeaderRowsCount" runat="server" MaxLength="2" style="width: 16%;" placeholder="Rows..." />
    <asp:Button ID="btnUploadExcel" runat="server" ValidationGroup="UploadExcel" OnClick="btnUploadExcel_Click" OnClientClick="javascript: upload_excel_click();" Text="Upload" style="width: 22%;" />
    <div id="divWait" class="no-display"><span class="message">Please wait...</span></div>
    <asp:RequiredFieldValidator ID="rfvUploadExcel" runat="server" ValidationGroup="UploadExcel" ControlToValidate="fuUploadExcel" ErrorMessage="Select excel file." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    <asp:RequiredFieldValidator ID="rfvHeaderRowsCount" runat="server" ValidationGroup="UploadExcel" ControlToValidate="txtHeaderRowsCount" ErrorMessage="Enter header rows count." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    <asp:CompareValidator ID="cvHeaderRowsCount" runat="server" ValidationGroup="UploadExcel" ControlToValidate="txtHeaderRowsCount" Operator="DataTypeCheck" Type="Integer" ErrorMessage="Rows count must be a whole number." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
    <asp:RangeValidator id="rvHeaderRowsCount" runat="server" ValidationGroup="UploadExcel" ControlToValidate="txtHeaderRowsCount" MinimumValue="1" MaximumValue="99" Type="Integer" Text="Rows count must be greater than 0." Display="Dynamic" SetFocusOnError="true" CssClass="error block" />
</div>

<div class="clear-both"></div>

<div ID="divTemplateMessage" runat="server" class="div-form-message">New Template</div>

<div id="divTemplateField" runat="server" visible="false" class="float-left" style="margin: 10px;">
    Template Fields ( <asp:Label ID="lblTemplateFieldCount" runat="server" /> )
    <asp:Repeater ID="repTemplateField" runat="server" OnItemDataBound="repTemplateField_ItemDataBound">
        <HeaderTemplate>
            <table class="table-sub">
                <tr>
                    <th>Excel Field</th>
                    <th>DB Field</th>
                    <th>Exclude</th>
                    <th>Format</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td class="nowrap"><%# DataBinder.Eval(Container.DataItem, "FieldFromExcel") %></td>
                    <td>
                        <asp:DropDownList ID="ddlFieldFromDB" runat="server" onchange="javascript: show_format(this);"></asp:DropDownList>
                        <asp:HiddenField ID="hidFieldFromExcel" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "FieldFromExcel") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtFieldExclude" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FieldExclude") %>' />
                    </td>
                    <td class="nowrap">
                        <div id="divFieldFormatDate" runat="server" class="no-display">
                            <%--  --%>
                            <asp:DropDownList ID="ddlFormatDate" runat="server" Font-Names="'Courier New'" onchange="javascript: show_format_date(this);">
                                <asp:ListItem Value="" Text="Format" style="color: lightgray;" />
                                <asp:ListItem Value="yyyy MM dd" Text="YYYY MM DD" />
                                <asp:ListItem Value="dd MM yyyy" Text="DD MM YYYY" />
                                <asp:ListItem Value="MM dd yyyy" Text="MM DD YYYY" />
                                <asp:ListItem Value="" Text="---" />
                                <asp:ListItem Value="yyyy M d" Text="YYYY M D" />
                                <asp:ListItem Value="d M yyyy" Text="D M YYYY" />
                                <asp:ListItem Value="M d yyyy" Text="M D YYYY" />
                                <asp:ListItem Value="" Text="---" />
                                <asp:ListItem Value="yy MM dd" Text="YY MM DD" />
                                <asp:ListItem Value="dd MM yy" Text="DD MM YY" />
                                <asp:ListItem Value="MM dd yy" Text="MM DD YY" />
                                <asp:ListItem Value="" Text="---" />
                                <asp:ListItem Value="[yy M d]" Text="YY M D" />
                                <asp:ListItem Value="[d M yy]" Text="D M YY" />
                                <asp:ListItem Value="[M d yy]" Text="M D YY" />
                                <asp:ListItem Value="" Text="---" />
                                <asp:ListItem Value="number" Text="Number" />
                            </asp:DropDownList>
                            <span id="spanFormatDateDelimiter" runat="server" class="no-display">
                                <asp:DropDownList ID="ddlFormatDateDelimiter" runat="server" Font-Names="'Courier New'" onchange="javascript: check_format_date_delimiter(this);">
                                    <asp:ListItem Value="" Text="Delimiter" style="color: lightgray;" />
                                    <asp:ListItem Value="none" Text="None" />
                                    <asp:ListItem Value="space" Text="Space" />
                                    <asp:ListItem Value="-" Text="-" />
                                    <asp:ListItem Value="/" Text="/" />
                                    <asp:ListItem Value="." Text="." />
                                </asp:DropDownList>
                            </span>
                            <span id="spanFormatDateFromNumber" runat="server" class="no-display">
                                <asp:DropDownList ID="ddlFormatDatePart" runat="server" Font-Names="'Courier New'">
                                    <asp:ListItem Value="" Text="Part" style="color: lightgray;" />
                                    <asp:ListItem Value="year" Text="Year" />
                                    <asp:ListItem Value="month" Text="Month" />
                                    <asp:ListItem Value="day" Text="Day" />
                                    <asp:ListItem Value="hour" Text="Hour" />
                                    <asp:ListItem Value="minute" Text="Minute" />
                                    <asp:ListItem Value="second" Text="Second" />
                                </asp:DropDownList>
                                <asp:TextBox ID="txtFormatDateYear" runat="server" MaxLength="4" Width="45px" placeholder="YYYY" />
                                <asp:TextBox ID="txtFormatDateMonth" runat="server" MaxLength="2" Width="30px" placeholder="MM" />
                                <asp:TextBox ID="txtFormatDateDay" runat="server" MaxLength="2" Width="30px" placeholder="DD" />
                            </span>
                            <div id="divFormatDelimiterError" class="no-display">In this case, the delimiter can't be set to 'None'.</div>
                        </div>

                        <div id="divFieldFormatBit" runat="server" class="no-display">
                            <asp:TextBox ID="txtFormatBit" runat="server" Width="250px" placeholder="Value/s separated by commas" />
                        </div>
                    </td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>

<div id="divTemplateCurrency" runat="server" visible="false" class="float-left" style="margin: 10px;">
    Currency in Template
    <asp:Repeater ID="repTemplateCurrency" runat="server">
        <HeaderTemplate>
            <table class="table-sub">
                <tr>
                    <th>Currency in XL</th>
                    <th>Currency in DB</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td><%# DataBinder.Eval(Container.DataItem, "CurrencyFromUser") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "CurrencyName") %></td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>

<div id="divTemplateCredit" runat="server" visible="false" class="float-left" style="margin: 10px;">
    Credit in Template
    <asp:Repeater ID="repTemplateCredit" runat="server">
        <HeaderTemplate>
            <table class="table-sub">
                <tr>
                    <th>Credit in XL</th>
                    <th>Credit in DB</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td><%# DataBinder.Eval(Container.DataItem, "CreditFromUser") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "CreditName") %></td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>

<div id="divTemplateCard" runat="server" visible="false" class="float-left" style="margin: 10px;">
    Card in Template
    <asp:Repeater ID="repTemplateCard" runat="server">
        <HeaderTemplate>
            <table class="table-sub">
                <tr>
                    <th>Card in XL</th>
                    <th>Card in DB</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td><%# DataBinder.Eval(Container.DataItem, "CardFromUser") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "CardName") %></td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>

<div id="divOperationType" runat="server" visible="false" class="float-left" style="margin: 10px;">
    Operation Type in Template
    <asp:Repeater ID="repOperationType" runat="server">
        <HeaderTemplate>
            <table class="table-sub">
                <tr>
                    <th>Operation Type in XL</th>
                    <th>Operation Type in DB</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td><%# DataBinder.Eval(Container.DataItem, "OperationTypeFromUser") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "OperationTypeID") %></td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>

<div class="clear-both"></div>

<div class="div-form-command">
    <asp:Button ID="btnSaveBottom" runat="server" Text="Save" OnClick="Save_Click" />
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
    Status:
    <asp:Label ID="lblStatusBottom" runat="server" Text="New" />
</div>

<script>
    function show_format(ddl_field_from_db) {
        var s_id = new String(ddl_field_from_db.id);
        var s_type = "";

        if (ddl_field_from_db.selectedIndex > 0) {
            s_type = ddl_field_from_db.options[ddl_field_from_db.selectedIndex].attributes["data-type"].value;
        }

        // divFieldFormatDate

        var div_field_format_date = document.getElementById(s_id.replace("ddlFieldFromDB", "divFieldFormatDate"));
        var ddl_format_date = document.getElementById(s_id.replace("ddlFieldFromDB", "ddlFormatDate"));

        div_field_format_date.className = "no-display";
        ddl_format_date.value = "";

        show_format_date(ddl_format_date);

        // divFieldFormatBit

        var div_field_format_bit = document.getElementById(s_id.replace("ddlFieldFromDB", "divFieldFormatBit"));
        var txt_format_bit = document.getElementById(s_id.replace("ddlFieldFromDB", "txtFormatBit"));

        div_field_format_bit.className = "no-display";
        txt_format_bit.value = "";

        switch (s_type) {
            case "date":
                div_field_format_date.className = "";
                break;
            case "bit":
                div_field_format_bit.className = "";
                break;
        }
    }

    function show_format_date(ddl_format_date) {
        var s_value = ddl_format_date.value;

        var span_format_date_delimiter = document.getElementById(ddl_format_date.id.replace("ddlFormatDate", "spanFormatDateDelimiter"));
        var span_format_date_from_number = document.getElementById(ddl_format_date.id.replace("ddlFormatDate", "spanFormatDateFromNumber"));

        var ddl_format_date_part = document.getElementById(ddl_format_date.id.replace("ddlFormatDate", "ddlFormatDatePart"));
        var txt_format_date_year = document.getElementById(ddl_format_date.id.replace("ddlFormatDate", "txtFormatDateYear"));
        var txt_format_date_month = document.getElementById(ddl_format_date.id.replace("ddlFormatDate", "txtFormatDateMonth"));
        var txt_format_date_day = document.getElementById(ddl_format_date.id.replace("ddlFormatDate", "txtFormatDateDay"));

        ddl_format_date_part.selectedIndex = 0;

        txt_format_date_year.value = "";
        txt_format_date_month.value = "";
        txt_format_date_day.value = "";

        if (s_value == "") {
            span_format_date_delimiter.className = "no-display";
            span_format_date_from_number.className = "no-display";
            return;
        }
        else if (s_value == "number") {
            span_format_date_delimiter.className = "no-display";
            span_format_date_from_number.className = "";

            ddl_format_date_part.value = "day";

            txt_format_date_year.value = "1899";
            txt_format_date_month.value = "12";
            txt_format_date_day.value = "30";
        }
        else {
            span_format_date_delimiter.className = "";
            span_format_date_from_number.className = "no-display";

            var ddl_format_date_delimiter = document.getElementById(ddl_format_date.id.replace("ddlFormatDate", "ddlFormatDateDelimiter"));

            check_format_date_delimiter(ddl_format_date_delimiter);
        }
    }

    function check_format_date_delimiter(ddl_format_date_delimiter) {
        var ddl_format_date = document.getElementById(ddl_format_date_delimiter.id.replace("ddlFormatDateDelimiter", "ddlFormatDate"));
        var div_format_delimiter_error = document.getElementById("divFormatDelimiterError");

        var s_delimiter = ddl_format_date_delimiter.value;
        var s_format = ddl_format_date.value;

        if (s_delimiter == "none" && s_format.indexOf("[") >= 0) {
            div_format_delimiter_error.className = "error";
        }
        else {
            div_format_delimiter_error.className = "no-display";
        }
    }

    // ValidationGroup = UploadExcel
    function upload_excel_click() {
        if (Page_ClientValidate("UploadExcel") == true) {   
            display_by_class("divWait");
            document.getElementById("<%= btnUploadExcel.ClientID %>").style.visibility = "hidden";
        }
    }
</script>
