var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;

var tableDetail;

GlobalDatepicker.init(".datepicker");
//$("#PasswordExpired").datepicker({
//    format: "dd M yyyy",
//        prevText: "<",
//        nextText: ">",
//        autoclose: true,
//        forceParse: false
//    });
loadData();




const emailRegex = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;

// ==========================================
// EVENT LISTENERS
// ==========================================

$("#Email").on("input blur", function () {
    validateEmail();
});

//date picker
$(document).on("click", ".datepicker-icon", function () {
    $(this).closest(".datepicker-wrapper")
        .find(".datepicker")
        .datepicker("show");
});

function validateEmail() {
    const emailInput = $("#Email");
    const email = $.trim(emailInput.val());

    if (email === "") {
        emailInput.removeClass("is-valid is-invalid");
        $(".div-email").removeClass("show");
        $(".div-email").addClass("hide");
        $("#emailError").hide();
        return false;
    }

    if (emailRegex.test(email)) {
        emailInput.removeClass("is-invalid").addClass("is-valid");
        $(".div-email").removeClass("show");
        $(".div-email").addClass("hide");

        $("#emailError").hide();
        return true;
    } else {
        emailInput.removeClass("is-valid").addClass("is-invalid");
        $(".div-email").removeClass("hidden");
        $(".div-email").addClass("show");

        $("#emailError").show();
        return false;
    }
}

function loadData() {
    if (table) {
        table.ajax.reload(null, false);
        return;
    }

     table = $("#tableID").DataTable({
        destroy: true,
        autoWidth:false,
        ajax: {
            url: buildServiceUrl("service/usersetup/list"),
            type: "POST",
            xhrFields: {
                withCredentials: true
            },
            contentType: "application/json",
            dataSrc: function (json) {
                
                if (!json || json.message =="Unauthorized") {
                    if (json.message && json.message.toLowerCase().includes("unauthorized")) {
                        window.location.href = "/Account/Login";
                        return [];
                    }
                }

                return json.data || [];
            },
            
        },
         
        columns: [
            {
                data: null,
                className: "text-center",
                render: function (data, type, row) {

                    if (allowDelete == '0') {

                        return `
                                <button class="btn btn-xs theme-color btn-edit" data-id="${row.UserID}" title="Edit" disabled>
                                    <i class="fa fa-edit"></i>
                                </button>
                                <button class="btn btn-xs theme-color" title="Delete" disabled>
                                    <i class="fa fa-trash-o"></i>
                                </button>
                               
                            `;

                    } else {

                        return `
                                <button class="btn btn-xs theme-color btn-edit" data-id="${row.UserID}" title="Edit">
                                    <i class="fa fa-edit"></i>
                                </button>  
                                <button class="btn btn-xs theme-color btn-delete" data-id="${row.UserID}" title="Delete">
                                    <i class="fa fa-trash-o"></i>
                                </button>
                              
                            `;
                    }  //<button class="btn btn-xs theme-color btn-setting" data-id="${row.UserID}" data-name="${row.UserName}" title="User Settings">
                    //    <i class="fa fa-cog"></i>
                    //</button >
                }
            },
            { data: "UserID" },
            { data: "UserName" },
            { data: "UserGroupName" },
            { data: "EmailStatus" },
            { data: "Email" },
            { data: "ClusterName" },
            { data: "AdminStatus" },
            { data: "LockedStatus" },
            {
                data: "PasswordExpired",
                render: function (data) {
                    return formatDate(data);

                },
            },
            {
                data: "RegisterDate", 
                    render: function (data) {
                        return formatDateTime(data);
                
                },
            },
            { data: "RegisterUser" },
            {
                data: "LastUpdate", render: function (data) {
                   
                    if (!data || data === "0001-01-01T00:00:00") return "";
                    return formatDateTime(data);
                
                },
            },
            { data: "LastUser" }

        ],
         autoWidth: false,
         searching: true,
         ordering: true,
         paging: true,
         info: true,
         bSort: true,
         bLengthChange: true,
         pageLength: 25,
         scrollX: true,
         scrollY: "calc(105vh - 390px)",
         processing: false,
         serverSide: false,
         responsive: true,
         "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "ALL"]],
         language: {
             lengthMenu: "Show _MENU_"
         },
         dom:
             "rt" +
             "<'row mt-2 align-items-center'\
            <'col-sm-6'i>\
            <'col-sm-6 d-flex justify-content-end align-items-center gap-3'lp>\ >"


     });

}

//add new
$(document).on("click", ".btn-add", function () {

    $("#spanSendEmail").text("NO");
    $("#spanStatusAdmin").text("NO");
    $("#spanLocked").text("NO");

    populateDDL({
        url: "service/groupsetup/group-ddl",
        dropdownId: "UserGroup",
        codeField: "Code",
        descField: "Description",
        descTextbox: "UserGroupName",
        width: "150px",
        callback: function () {
            $("#UserGroup").val('').trigger('change');

        }
    });
    $("#UserID").val('');
    $("#UserName").val('');
    $("#Password").val('');
    $("#ConfirmPassword").val('');
    $("#UserGroup").val('').trigger('change');
    $("#UserGroupName").val('');
    $("#Email").val('');
    $("#IsSendEmail").prop("checked", false);

    
    $("#IsAdmin").prop("checked", false);
    $("#IsLocked").prop("checked", false);
    let defDate = new Date("2050-12-31");
    $("#PasswordExpired").datepicker('setDate', defDate);
    //GlobalDatepicker.setDefault("#PasswordExpired", 3);

    //let today = new Date();
    //today.setMonth(today.getMonth() + 3);
    //$("#PasswordExpired").datepicker('setDate', today);

    $("#ModalTitle").html("Add New User");
    $("#MyModal").modal("show");
    $('#InsertData').show();
    $('#UpdateData').hide();
    $("#UserID").prop('readonly', false);
});


//POPUP MODAL GET DATA
$(document).on("click", ".btn-edit", function () {

    var userID = $(this).data("id").toString();

    $.ajax({
        //url: BaseUrl + "service/usersetup/detail",
        url: buildServiceUrl("service/usersetup/detail"),
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            UserID: userID
        }),
        success: function (res) {
          debugger
            if (res && res.data) {
                $("#ModalTitle").html("User Setup");

                $("#UserID").val(res.data.UserID);
                $("#UserID").prop("readonly", true);

                $("#UserName").val(res.data.UserName);
                $("#Password").val(res.data.Password).attr("type", "password");
                $("#ConfirmPassword").val(res.data.Password).attr("type", "password");

                $("#Email").val(res.data.Email);

                $("#IsSendEmail").prop("checked", res.data.StatusEmail == true || res.data.StatusEmail == "1");
                $("#IsAdmin").prop("checked", res.data.StatusAdmin == true || res.data.StatusAdmin == "1");
                $("#IsLocked").prop("checked", res.data.Locked == true || res.data.Locked == "1");
                
                if (res.data.StatusEmail == true || res.data.StatusEmail == "1") {
                        $("#spanSendEmail").text("YES")
                            .removeClass("no")
                            .addClass("yes");
                } else {
                        $("#spanSendEmail")
                            .text("NO")
                            .removeClass("yes")
                            .addClass("no");
                }

                if (res.data.StatusAdmin == true || res.data.StatusAdmin == "1") {
                    $("#spanStatusAdmin").text("YES")
                        .removeClass("no")
                        .addClass("yes");
                } else {
                    $("#spanStatusAdmin")
                        .text("NO")
                        .removeClass("yes")
                        .addClass("no");
                }

                if (res.data.Locked == true || res.data.Locked == "1") {
                    $("#spanLocked").text("Locked")
                        .removeClass("no")
                        .addClass("yes");
                } else {
                    $("#spanLocked")
                        .text("NO")
                        .removeClass("yes")
                        .addClass("no");
                }

                //$("#PasswordExpired").val(formatDate(res.data.PasswordExpired));
                let dateObj = new Date(res.data.PasswordExpired);

                $("#PasswordExpired")
                    .datepicker('setDate', dateObj)  // set internal
                    .val(formatDate(dateObj));       // set tampilan (optional kalau format beda)

             
                populateDDL({
                    url: "service/groupsetup/group-ddl",
                    dropdownId: "UserGroup",
                    codeField: "Code",
                    descField: "Description",
                    descTextbox: "UserGroupName",
                    width: "150px",
                    selectedValue: res.data.UserGroupCode
                });

                $("#ClusterCode").val(res.data.ClusterCode);
                $("#ClusterName").val(res.data.ClusterName);

                $("#InsertData").hide();
                $("#UpdateData").show();

                $("#MyModal").modal("show");

            }
        },
        error: function () {

            toastr.warning('Failed to load data.', 'Error', { timeOut: 3000, "closeButton": true });

        }
    });

});

//onchange
$(document).on("change", "#UserGroup", function () {
    let desc = $("#UserGroup option:selected").data("desc");
    $("#UserGroupName").val((desc || "").trim());
});

//onchange checkbox email status
$("#IsSendEmail").on("change", function () {
    const isChecked = $(this).is(":checked");
    const $label = $("#spanSendEmail");

    if (isChecked) {
        $label
            .text("YES")
            .removeClass("no")
            .addClass("yes");
    } else {
        $label
            .text("NO")
            .removeClass("yes")
            .addClass("no");
    }
});

$("#IsAdmin").on("change", function () {
    const isChecked = $(this).is(":checked");
    const $label = $("#spanStatusAdmin");

    if (isChecked) {
        $label
            .text("YES")
            .removeClass("no")
            .addClass("yes");
    } else {
        $label
            .text("NO")
            .removeClass("yes")
            .addClass("no");
    }
});

$("#IsLocked").on("change", function () {
    const isChecked = $(this).is(":checked");
    const $label = $("#spanLocked");

    if (isChecked) {
        $label
            .text("Locked")
            .removeClass("no")
            .addClass("yes");
    } else {
        $label
            .text("NO")
            .removeClass("yes")
            .addClass("no");
    }
});


//INSERT
$(document).on("click", "#InsertData", function () {
    var model = new Object();
    model.UserID = $('#UserID').val();
    model.UserName = $('#UserName').val();
    model.Password = $('#Password').val();
    model.ConfirmPassword = $('#ConfirmPassword').val();
    model.UserGroup=  $('#UserGroup').val();
    model.StatusAdmin = ($('#IsAdmin').prop('checked')==true?"1":"0");
    model.Locked = ($('#IsLocked').prop('checked') == true ? "1" : "0");
    model.PasswordExpired = formatDateGeneral($('#PasswordExpired').val());
    model.Email = $("#Email").val();
    model.StatusEmail = ($('#IsSendEmail').prop('checked') == true ? "1" : "0"); 


    if ($('#IsSendEmail').prop('checked') == true) {

        if ($("#Email").val() == "" || $("#Email").val() == null) {
            toastr.warning("Please input email address!", 'Warning', { timeOut: 3000, "closeButton": true });
            return;
        }

        //validasi email
        if (!validateEmail()) {
            $("#Email").focus();
            return;
        }
    }


    $.ajax({
        type: "POST",
        //url: BaseUrl + "service/usersetup/create",
        url: buildServiceUrl("service/usersetup/create"),
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        xhrFields: {
            withCredentials: true
        },
        success: function (result) {
          
            if (!result.success) {
                table.ajax.reload();
                toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });
                $('#MyModal').modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            }
            else {
                table.ajax.reload();
                toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
                $('#MyModal').modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            }

        }
    })
    //}
});

//UPDATE
$(document).on("click", "#UpdateData", function () {
    var model = new Object();
    model.UserID = $('#UserID').val();
    model.UserName = $('#UserName').val();
    model.Password = $('#Password').val();
    model.ConfirmPassword = $('#ConfirmPassword').val();
    model.UserGroup = $('#UserGroup').val();
    model.StatusAdmin = ($('#IsAdmin').prop('checked') == true ? "1" : "0");
    model.Locked = ($('#IsLocked').prop('checked') == true ? "1" : "0");
    model.PasswordExpired = formatDateGeneral($('#PasswordExpired').val());
    model.Email = $("#Email").val();
    model.StatusEmail = ($('#IsSendEmail').prop('checked') == true ? "1" : "0"); 


    if ($('#IsSendEmail').prop('checked') == true) {

        if ($("#Email").val() == "" || $("#Email").val() == null) {
            toastr.warning("Please input email address!", 'Warning', { timeOut: 3000, "closeButton": true });
            return;
        }

        //validasi email
        if (!validateEmail()) {
            $("#Email").focus();
            return;
        }
    }

  


    $.ajax({
        type: "PATCH",
        //url: BaseUrl + "service/usersetup/update",
        url: buildServiceUrl("service/usersetup/update"),
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        xhrFields: {
            withCredentials: true
        },
        success: function (result) {
        
            if (!result.success) {
                table.ajax.reload();
                toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });
                $('#MyModal').modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            }
            else {
                table.ajax.reload();
                toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
                $('#MyModal').modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            }

        }
    });
});

// DELETE
$(document).on("click", ".btn-delete", function () {
    
    var id = $(this).data("id").toString();

    Swal.fire({
        title: 'Are you sure?',
        text: "Data will be deleted permanently!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {

        if (result.isConfirmed) {
            $.ajax({
                //url: BaseUrl + "service/usersetup/delete",
                url: buildServiceUrl("service/usersetup/delete"),
                type: "POST",
                contentType: "application/json",
                xhrFields: {
                    withCredentials: true
                },
                data: JSON.stringify({
                    UserID: id
                }),
                success: function (result) {
                  

                    if (!result.success) {
                        toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });

                    } else {
                       
                        toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
                        table.ajax.reload();
                    }
                },
               
            });

        }
    });

});


/*setting course per user id */

//POPUP MODAL Detail Settings
$(document).on("click", ".btn-setting", function () {

    $("#ModalSettings").modal("show");
    var userID = $(this).attr("data-id").toString();
    var userName = $(this).attr("data-name");

    $("#ModalTitle2").html("Settings Course Per User");

    $("#UserIDDetail").val(userID);
    $("#UserNameDetail").val(userName);

    loadDetailTable(userID);

    setTimeout(() => {
        tableDetail.columns.adjust().draw();
    }, 200);
});

$(document).on("click", ".btn-submitdetail", function () {
    //let tableIDList = $('#tableDetail').DataTable();
    let listData = [];

    $('#tableDetail').DataTable().rows().every(function () {
        let row = this.node();
        let data = this.data();

        let IsCheck = $(row).find('.chk-detail').is(':checked');

        if (IsCheck) {
            listData.push({
                UserID: $("#UserIDDetail").val(),
                PlantCode: data.PlantCode,
                CourseCode: data.CourseCode,
                AllowAccess: IsCheck
            });
        }
    });

    // 🔥 VALIDASI: tidak ada yg dicentang
    if (listData.length === 0) {
        toastr.info("Please select data to submit data");
        return;
    }

    let model = listData ;


    $.ajax({
        type: "POST",
        url: buildServiceUrl("service/usersetup/submit-batch"),
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify(model),
        success: function (result) {

            if (!result.success) {
                toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });

            }
            else {
                toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
                $('#ModalSettings').modal('hide');
                table.ajax.reload(null, false); // Reload tanpa mereset halaman (stay di page current datatable)
            }

        }
    });
});


function loadDetailTable(pUserID) {

    tableDetail = $("#tableDetail").DataTable({
        destroy: true,
        autoWidth: false,
        ajax: {
            url: BaseUrl + "service/usersetup/list-setting",
            data: function () {
                return JSON.stringify({
                    UserID: pUserID
                });
            },
            type: "POST",
            xhrFields: { withCredentials: true },
            contentType: "application/json",
            dataSrc: function (json) {
                return json.data ?? json;
            }
        },
        columns: [
            {
                data: "AllowAccess",
                width:"60",
                className: "text-center",
                orderable: false,
                render: function (data, type, row) {
                    //var isChecked = row.Check == 0 ? 'checked' : '';
                    return `<input  type="checkbox"
                                    class="chk-detail"
                                    data-user="${row.UserID}"
                                    data-plant="${row.PlantCode}"
                            value="${row.UserID}" ${row.AllowAccess == "1" || row.AllowAccess ==true ? "checked" : ""} />`;
                }
            },
            { data: "PlantName", width: "250", },
            { data: "CourseCode", width: "130", },
            { data: "Title", width: "350", }
        ],
        order: [[0, 'desc']],
        searching: false,
        ordering: true,
        paging: true,
        info: true,
        bSort: true,
        bLengthChange: true,
        pageLength: 15,
        scrollX: true,
        processing: false,
        serverSide: false,
        responsive: true
    });
}

