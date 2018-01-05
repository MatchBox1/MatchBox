// GLOBAL

function trim_string(s_string) {
    try {
        s_string = s_string.trim();
    }
    catch (ex) {
        s_string = s_string.replace(/^\s+|\s+$/gm, '');
    }

    return s_string;
}

function display_by_class(s_element_id) {
    var o_element = document.getElementById(s_element_id);

    try {
        if (o_element.className == "no-display") { o_element.className = ""; } else { o_element.className = "no-display"; }
    }
    catch (ex) { }
}

function display_by_style(s_element_id) {
    var o_element = document.getElementById(s_element_id);

    try {
        if (o_element.style.display == "none") { o_element.style.display = ""; } else { o_element.style.display = "none"; }
    }
    catch (ex) { }
}

function delete_item(s_url, s_msg) {
    if (confirm(s_msg)) { document.location.href = s_url; }
}

function checkbox_check_all(arr_checkbox, b_checked, b_return_checked) {
    // CHECK / UNCHECK ALL CHECKBOXES IN arr_checkbox
    // RETURN COMMA SEPARATED VALUES OF CHECKED / UNCHECKED CHECKBOXES IN arr_checkbox
    // arr_checkbox     :   CHECKBOXES ARRAY
    // b_checked        :   IF true THEN CHECK ALL ELSE UNCHECK ALL
    // b_return_checked :   IF true THEN RETURN VALUES OF CHECKED CHECKBOXES ELSE RETURN VALUES OF UNCHECKED CHECKBOXES

    var s_value_array = "";

    for (var i = 1; i < arr_checkbox.length; i++) {
        arr_checkbox[i].checked = b_checked;

        if (arr_checkbox[i].checked == b_return_checked) {
            if (s_value_array != "") { s_value_array += ","; }

            s_value_array += arr_checkbox[i].value;
        }
    }

    return s_value_array;
}

function checkbox_check(arr_checkbox, b_return_checked) {
    // CHECK / UNCHECK MAIN (FIRST) CHECKBOX IN arr_checkbox
    // RETURN COMMA SEPARATED VALUES OF CHECKED / UNCHECKED CHECKBOXES IN arr_checkbox
    // arr_checkbox     :   CHECKBOXES ARRAY
    // b_return_checked :   IF true THEN RETURN VALUES OF CHECKED CHECKBOXES ELSE RETURN VALUES OF UNCHECKED CHECKBOXES

    var s_value_array = "";
    var b_check_first = true;

    for (var i = 1; i < arr_checkbox.length; i++) {
        if (arr_checkbox[i].checked == false) { b_check_first = false; }

        if (arr_checkbox[i].checked == b_return_checked) {
            if (s_value_array != "") { s_value_array += ","; }

            s_value_array += arr_checkbox[i].value;
        }
    }

    arr_checkbox[0].checked = b_check_first;

    return s_value_array;
}

// PERMISSION

function permission_check_change(s_id, b_change_child) {
    var s_permission = "";
    var o_checkbox = document.getElementById(s_id);

    s_id = new String(s_id);

    if (s_id.match("chkAllowView_")) {
        s_permission = "view";
    }
    else if (s_id.match("chkAllowChange_")) {
        s_permission = "change";
    }
    else if (s_id.match("chkAllowAdd_")) {
        s_permission = "add";
    }
    else if (s_id.match("chkAllowRemove_")) {
        s_permission = "remove";
    }
    else {
        return;
    }

    if (s_permission == "view" && o_checkbox.checked == false) {
        try { document.getElementById(s_id.replace("chkAllowView_", "chkAllowChange_")).checked = false; } catch (err) { }
        try { document.getElementById(s_id.replace("chkAllowView_", "chkAllowAdd_")).checked = false; } catch (err) { }
        try { document.getElementById(s_id.replace("chkAllowView_", "chkAllowRemove_")).checked = false; } catch (err) { }
    }

    if (s_permission == "change" && o_checkbox.checked == true) {
        try { document.getElementById(s_id.replace("chkAllowChange_", "chkAllowView_")).checked = true; } catch (err) { }
    }

    if (s_permission == "add" && o_checkbox.checked == true) {
        try { document.getElementById(s_id.replace("chkAllowAdd_", "chkAllowView_")).checked = true; } catch (err) { }
        permission_check_change_child(s_id, "view", true);
        return;
    }

    if (s_permission == "remove" && o_checkbox.checked == true) {
        try { document.getElementById(s_id.replace("chkAllowRemove_", "chkAllowView_")).checked = true; } catch (err) { }
        permission_check_change_child(s_id, "view", true);
        return;
    }

    if (b_change_child == 1) { permission_check_change_child(s_id, s_permission, o_checkbox.checked); }
}

function permission_check_change_child(s_id, s_permission, b_checked) {
    var s_interface_id = s_id.substr(s_id.lastIndexOf("_") + 1);
    var s_class = "nonexistent-class-" + s_interface_id + "-" + s_permission;
    var o_checkbox_array = document.getElementsByClassName(s_class);

    for (var i = 0; i < o_checkbox_array.length; i++) {
        o_checkbox_array[i].checked = b_checked;
        o_checkbox_array[i].onchange();
    }
}

function permission_check_selected(s_hid_interface_id, s_hid_field_id) {
    if (typeof(Page_ClientValidate) == "function") { Page_ClientValidate(); }

    if (!Page_IsValid) { return false; }

    var hid_interface_list = document.getElementById(s_hid_interface_id);
    var hid_field_list = document.getElementById(s_hid_field_id);

    var array_interface_id = hid_interface_list.value.split(",");
    var array_field_id = hid_field_list.value.split(",");

    hid_interface_list.value = "";
    hid_field_list.value = "";

    for (var i = 0; i < array_interface_id.length; i++) {
        var s_id = array_interface_id[i];
        var b_view = false, b_change = false, b_add = false, b_remove = false;

        try { b_view = document.getElementById(s_hid_interface_id.replace("hidInterfaceList", "chkAllowView_") + s_id).checked; } catch (err) { }

        b_view = permission_check_child(s_id, "view", b_view);

        if (b_view) {
            try { b_change = document.getElementById(s_hid_interface_id.replace("hidInterfaceList", "chkAllowChange_") + s_id).checked; } catch (err) { }

            b_change = permission_check_child(s_id, "change", b_change);

            try { b_add = document.getElementById(s_hid_interface_id.replace("hidInterfaceList", "chkAllowAdd_") + s_id).checked; } catch (err) { }
            try { b_remove = document.getElementById(s_hid_interface_id.replace("hidInterfaceList", "chkAllowRemove_") + s_id).checked; } catch (err) { }

            if (hid_interface_list.value != "") { hid_interface_list.value += ";"; }

            hid_interface_list.value += "[" + s_id + "," + b_view + "," + b_change + "," + b_add + "," + b_remove + "]";
        }
    }

    for (var i = 0; i < array_field_id.length; i++) {
        var s_id = array_field_id[i];

        var o_checkbox_view = null, o_checkbox_change = null;
        var b_view = false, b_change = false;
        var s_interface_id = "";

        try { o_checkbox_view = document.getElementById(s_hid_field_id.replace("hidFieldList", "chkAllowView_Field_") + s_id); } catch (err) { }
        try { o_checkbox_change = document.getElementById(s_hid_field_id.replace("hidFieldList", "chkAllowChange_Field_") + s_id); } catch (err) { }

        if (o_checkbox_view == null) { continue; }

        b_view = o_checkbox_view.checked;
        s_interface_id = o_checkbox_view.className.replace("nonexistent-class-", "").replace("-view", "");

        if (o_checkbox_change) { b_change = o_checkbox_change.checked; }

        if (b_view) {
            if (hid_field_list.value != "") { hid_field_list.value += ";"; }

            hid_field_list.value += "[" + s_id + "," + b_view + "," + b_change + "]";

            if (hid_interface_list.value.indexOf("[" + s_interface_id + ",") == -1) {
                if (hid_interface_list.value != "") { hid_interface_list.value += ";"; }
                hid_interface_list.value += "[" + s_interface_id + ",true,false,false,false]";
            }
        }

        if (b_change) {
            if (hid_interface_list.value.indexOf("[" + s_interface_id + ",") == -1) {
                if (hid_interface_list.value != "") { hid_interface_list.value += ";"; }
                hid_interface_list.value += "[" + s_interface_id + ",true,true,false,false]";
            }
            else {
                hid_interface_list.value = hid_interface_list.value.replace("[" + s_interface_id + ",true,false,", "[" + s_interface_id + ",true,true,");
            }
        }
    }
}

function permission_check_child(s_id, s_permission, b_parent_checked) {
    var b_checked = false;
    var o_checkbox_array = document.getElementsByClassName("nonexistent-class-" + s_id + "-" + s_permission);

    if (o_checkbox_array.length == 0) {
        b_checked = b_parent_checked;
    }
    else {
        for (var i = 0; i < o_checkbox_array.length; i++) {
            b_checked = o_checkbox_array[i].checked;
            if (b_checked) { break; }
        }
    }

    return b_checked;
}

// DATE PICKER

function copy_datepicker_value(s_source_id, s_destination_id) {
    document.getElementById(s_destination_id).value = document.getElementById(s_source_id).value;
}

function clear_datepicker_value(s_id_array) {
    s_id_array = s_id_array.split(",");

    for (var i = 0; i < s_id_array.length; i++) {
        document.getElementById(s_id_array[i]).value = "";
    }
}

// SEARCH // EXCEL

function display_section(s_class_name, s_display) {
    var o_section = document.getElementsByClassName(s_class_name);

    if (!o_section) { return; }

    o_section[0].style.display = s_display;

    if (s_display == "block") {
        try { cancel_add_new(); } catch (err) { }
    }
}

// FORM

function display_frame_form(s_container_id, s_frame_id, s_frame_url) {
    var o_frame = document.getElementById(s_frame_id);
    var o_container = document.getElementById(s_container_id);

    if (o_container.className == "no-display") {
        o_frame.src = s_frame_url;
        o_container.className = "";
    }
    else {
        if (s_container_id != "secForm") {
            o_frame.src = "";
            o_container.className = "no-display";
        }
    }

    if (s_container_id == "secForm" && o_container.className != "no-display") {
        try { display_section("section-excel", "none"); } catch (err) { }
        try { display_section("section-search", "none"); } catch (err) { }
    }
}

function cancel_add_new() {
    var o_section_form = document.getElementById("secForm");

    for (var i = 0; i < o_section_form.childNodes.length; i++) {
        var o_element = o_section_form.childNodes[i];

        if (o_element.id && o_element.id.indexOf("fraForm") >= 0) {
            o_element.src = "";
            break;
        }
    }

    o_section_form.className = "no-display";
}

// TABLE

var s_open = "Open", s_close = "Close";

function link_open_onmouseup(o_link) {
    var o_row = o_link.parentNode.parentNode;

    if (o_link.innerHTML == "+") {
        o_link.innerHTML = "-";
        o_link.title = s_close;
        if (o_row.className == "") { o_row.className = "selected"; }
    }
    else {
        o_link.innerHTML = "+";
        o_link.title = s_open;
        if (o_row.className == "selected") { o_row.className = ""; }
    }
}

function display_sort(b_display) {
    var o_span_array = document.getElementsByClassName("span-sort-display");

    for (var i = 0; i < o_span_array.length; i++) {
        if (b_display) { o_span_array[i].style.display = "inline"; } else { o_span_array[i].style.removeAttribute("display"); }
    }
}

//
