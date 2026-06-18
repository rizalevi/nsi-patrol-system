var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;

loadData();
function AddNewData() {
    $("#ModalTitle").html("Add New Shift Master");
    $("#MyModal").modal("show");
    $('#InsertData').show();
    $('#UpdateData').hide();
    $("#ShiftCls").val('');
    $("#ShiftCls").prop("readonly", false );
    $("#InspectionTime").val(''); 
}

function loadData() {
    if (table) {
        table.ajax.reload(null, false);
        return;
    }

    table = $("#tableID").DataTable({
        destroy: true,
        ajax: {
            url: BaseUrl + "service/msShift/list",
            type: "POST",
            xhrFields: {
                withCredentials: true
            },
            contentType: "application/json",
            dataSrc: function (json) {
               
                return json.data; // atau json kalau array langsung
            }
        },
        columns: [
            {
                data: null,
                className: "text-center",
                render: function (data, type, row) {

                    if (allowDelete == '0') {
                        return `
                                <button class="btn btn-xs theme-color btn-edit" data-id="${row.Shift_Cls}">
                                    <i class="fa fa-edit"></i>
                                </button>
                                <button class="btn btn-xs theme-color" disabled>
                                    <i class="fa fa-trash-o"></i>
                                </button>
                               `;

                    } else {
                        return `
                                <button class="btn btn-xs theme-color btn-edit" data-id="${row.Shift_Cls}">
                                    <i class="fa fa-edit"></i>
                                </button>
                                <button class="btn btn-xs theme-color btn-delete" data-id="${row.Shift_Cls}">
                                    <i class="fa fa-trash-o"></i>
                                </button>
                               `;
                    }
                }
            },
            { data: "Shift_Cls" },
            { data: "Inspection_Time" },
            
            {
                data: "Register_Date", render: function (data) {

                    if (!data || data === "0001-01-01T00:00:00") return "";
                    return formatDateTime(data);

                },
            },
            { data: "Register_User" },
            {
                data: "Last_Update", render: function (data) {

                    if (!data || data === "0001-01-01T00:00:00") return "";
                    return formatDateTime(data);

                },
            },
            { data: "Last_User" }

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
        scrollY: "calc(105vh - 400px)",
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


//POPUP MODAL GET DATA
$(document).on("click", ".btn-edit", function () {

    var code = $(this).data("id").toString();

    $.ajax({
        url: BaseUrl + "service/msShift/detail",
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            Shift_Cls: code.trim()
        }),
        success: function (res) {
            if (res && res.data) {
                $("#ShiftCls").val(res.data.Shift_Cls.trim());
                $("#ShiftCls").prop("readonly", true);
                $("#InspectionTime").val(res.data.Inspection_Time);

                $("#ModalTitle").html("Shift Master");

                $("#InsertData").hide();
                $("#UpdateData").show();

                $("#MyModal").modal("show");

            }
        },
        error: function (xhr) {
            console.log(xhr);
            toastr.warning('Failed to load data.', 'Error', { timeOut: 3000, "closeButton": true });

        }
    });

});

//INSERT
$(document).on("click", "#InsertData", function () {
    var shiftCls = $('#ShiftCls').val();

    if (allowUpdate === "0") {
        toastr.warning('You do not have permission to write.', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    if (!shiftCls) { toastr.warning('Shift code is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }

    var model = new Object();
    model.Shift_Cls = $('#ShiftCls').val();
    model.Inspection_Time = $('#InspectionTime').val();

    $.ajax({
        type: "POST",
        url: BaseUrl + "service/msShift/create",
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        xhrFields: {
            withCredentials: true
        },
        success: function (result) {
            if (!result.success) {
                if (table) table.ajax.reload();
                toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });
                $('#MyModal').modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            } else {
                if (table) table.ajax.reload();
                toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
                $('#MyModal').modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            }
        },
        error: function (xhr) {
            console.log(xhr);
            toastr.warning('Failed to save data.', 'Error', { timeOut: 3000, "closeButton": true });
        }
    });
});

//UPDATE
$(document).on("click", "#UpdateData", function () {
    var shiftCls = $('#ShiftCls').val();
    var inspectionTime = $('#InspectionTime').val();

    if (allowUpdate === "0") {
        toastr.warning('You do not have permission to update.', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    if (!shiftCls) { toastr.warning('Shift code is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!inspectionTime) { toastr.warning('Inspection Time is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }

    Swal.fire({
        title: 'Are you sure?',
        text: "Data will be updated!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, update it!'
    }).then((confirm) => {

        if (!confirm.isConfirmed) return;

        var model = new Object();
        model.Shift_Cls = shiftCls;
        model.Inspection_Time = inspectionTime;

        $.ajax({
            type: "PATCH",
            url: BaseUrl + "service/msShift/update",
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            xhrFields: { withCredentials: true },
            success: function (result) {
                if (!result.success) {
                    if (table) table.ajax.reload();
                    toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });
                } else {
                    if (table) table.ajax.reload();
                    toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
                }
                $('#MyModal').modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            },
            error: function (xhr) {
                console.log(xhr);
                toastr.warning('Failed to update data.', 'Error', { timeOut: 3000, "closeButton": true });
            }
        });

    });
});

// DELETE
$(document).on("click", ".btn-delete", function () {

    var ShiftCls = $(this).data("id").toString();
    var courseCode = $('#CourseCodeFilter').val();
    var groupCode = $('#GroupCodeFilter').val();

    if (!ShiftCls) {
        toastr.warning('No data to delete.', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    if (allowDelete === "0") {
        toastr.warning('You do not have permission to delete.', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    Swal.fire({
        title: 'Are you sure?',
        text: "Data will be deleted permanently!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {

        if (result.isConfirmed) {
            $.ajax({
                url: BaseUrl + "service/msShift/delete",
                type: "POST",
                contentType: "application/json",
                xhrFields: {
                    withCredentials: true
                },
                data: JSON.stringify({
                    Shift_Cls: ShiftCls
                }),
                success: function (result) {
                    if (!result.success) {
                        toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });
                    } else {
                        toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
                        if (table) table.ajax.reload(null, false);
                    }
                },
                error: function (xhr) {
                    console.log(xhr);
                    toastr.warning('Failed to delete data.', 'Error', { timeOut: 3000, "closeButton": true });
                }
            });
        }
    });

});

