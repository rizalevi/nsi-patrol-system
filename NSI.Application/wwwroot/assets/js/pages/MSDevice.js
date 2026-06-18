var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;

var tableDetail;

// simpan pilihan user
let originalData = {};
let changedData = {};

//setInterval(
//    waitingDialog.show("Please wait retrieving data...")
//    , 400);
init();
loadData();


$(document).on("click", ".btn-addnew", function () {
    AddNewData();
});

function AddNewData() {
    $("#ModalTitle").html("Device Master");
    $("#MyModal").modal("show");
    $('#InsertData').show();
    $('#UpdateData').hide();
    $("#DeviceCode").val('');
    $("#DeviceCode").prop('readonly', false);
    $("#DeviceName").val('');
    $("#PlantCode").val('');
    $("#Description").val('');
    $("#Description").prop('readonly', true);
    $("#SerialNo").val('');
}

//ONCHANGE PLANT
$(document).on("change", "#PlantCode", function () {
  
    let desc = $("#PlantCode option:selected").data("desc");

    $("#Description").val((desc || "").trim());

});


//ONCHANGE DEVICE
$(document).on("change", "#CodeSettings", function () {
    let desc = $("#CodeSettings option:selected").data("desc");
    $("#CodeSettingsName").val((desc || "").trim());

    let code = $("#CodeSettings").val();
    var plantCode = $("#PlantCodeSettings").val();  
   /* if (code && plantCode) {*/
        loadDetailTable(code, plantCode);
    
});

//ONCHANGE PLANT SETTINGS
$(document).on("change", "#PlantCodeSettings", function () {
    
    let desc = $("#PlantCodeSettings option:selected").data("desc");
    $("#PlantNameSettings").val((desc || "").trim());
    let code = $("#PlantCodeSettings").val();
    var htCode = $("#CodeSettings").val();

    if (htCode != "" && code !="") {
        loadDetailTable(htCode, code);
    }
});



//POPUP MODAL GET DATA
$(document).on("click", ".btn-edit", function () {

    $("#MyModal").modal("show");
    // var code = $(this).data("id");
    var code = $(this).attr("data-id").toString();
    $.ajax({
        url: BaseUrl + "service/msdevice/detail",
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            DeviceCode: code
        }),
        success: function (res) {
            if (res && res.data) {
              
                $("#ModalTitle").html("Device Master");
                $("#DeviceCode").val(res.data.DeviceCode.trim());
                $("#DeviceName").val(res.data.DeviceName);
                $("#PlantCode").val(res.data.PlantCode.trim());
                $("#PlantCode").trigger('change');
                var selectedOption = $("#PlantCode").find("option:selected");
                $("#Description").val(res.data.PlantName);
                $("#Description").prop('readonly', true);
                $("#DeviceCode").prop('readonly', true);
                $("#SerialNo").val(res.data.SerialNo);


                $("#InsertData").hide();
                $("#UpdateData").show();


            }
        },
        error: function (xhr) {
            console.log(xhr);
            toastr.warning('Failed to load data.', 'Error', { timeOut: 3000, "closeButton": true });

        }
    });

});

//POPUP MODAL Detail Settings
$(document).on("click", ".btn-settings", function () {
    $("#MyModal").modal("hide");
    setTimeout(() => {
        $("#ModalDeviceDetail").modal("show");
    }, 300);

    // var ht    = $(this).data("ht");
    // var plant = $(this).data("plant");
    var ht = $(this).attr("data-ht").toString();
    var plant = $(this).attr("data-plant").toString();
   
    //$("#ModalTitle").html("Detail Settings");
  
    populateDDL({
        url: "service/msdevice/get-listdevice",
        dropdownId: "CodeSettings",
        codeField: "DeviceCode",
        descField: "DeviceName",
        descTextbox: "CodeSettingsName",
        width: "150px",
        selectedValue: ht,
        
    });

    populateDDL({
        url: "service/msdevice/get-plant",
        dropdownId: "PlantCodeSettings",
        codeField: "PlantCode",
        descField: "Description",
        descTextbox: "PlantNameSettings",
        width: "150px",
        selectedValue: plant,
        callback: function () {
            $("#PlantCodeSettings").trigger('change');
            loadDetailTable(ht, plant);
        }
    });



});
$('#ModalDeviceDetail').on('shown.bs.modal', function () {

    let tableDet = $('#tableDetail').DataTable();

    tableDet.columns.adjust().draw();

});

//INSERT MASTER
$(document).on("click", "#InsertData", function () {
    var DeviceCode = $('#DeviceCode').val();
    var DeviceName = $('#DeviceName').val();
    var PlantCode = $('#PlantCode').val();
    var SerialNo = $('#SerialNo').val();

    $("#DeviceCode").prop('readonly', false);

    if (allowUpdate === "0") {
        toastr.warning('You do not have permission to write.', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    if (!DeviceCode) { toastr.warning('Device code is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!DeviceName) { toastr.warning('Device name is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!PlantCode) { toastr.warning('Plant code is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!SerialNo) { toastr.warning('Serial number is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }

    var model = new Object();
    model.DeviceCode = $('#DeviceCode').val();
    model.DeviceName = $('#DeviceName').val();
    model.PlantCode = $('#PlantCode').val();
    model.SerialNo = $('#SerialNo').val();

    $.ajax({
        type: "POST",
        url: BaseUrl + "service/msdevice/create",
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

//UPDATE MASTER
$(document).on("click", "#UpdateData", function () {
    var DeviceCode = $('#DeviceCode').val();
    var DeviceName = $('#DeviceName').val();
    var PlantCode = $('#PlantCode').val();
    var SerialNo = $('#SerialNo').val();

    if (allowUpdate === "0") {
        toastr.warning('You do not have permission to update.', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }


    if (!DeviceCode) { toastr.warning('Device code is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!DeviceName) { toastr.warning('Device name is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!PlantCode) { toastr.warning('Plant code is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!SerialNo) { toastr.warning('Serial number is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }

   
    Swal.fire({
        title: 'Are you sure?',
        text: "Data will be updated!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, update it!'
    }).then((confirm) => {

        if (!confirm.isConfirmed) return;

        var model = new Object();
        model.DeviceCode = $('#DeviceCode').val();
        model.DeviceName = $('#DeviceName').val();
        model.PlantCode = $('#PlantCode').val();
        model.SerialNo = $('#SerialNo').val();

        $.ajax({
            type: "PATCH",
            url: BaseUrl + "service/msdevice/update",
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

//DELETE MASTER
$(document).on("click", ".btn-delete", function () {

    // var HTCode = $(this).data("id"); 
    var Code = $(this).attr("data-id").toString();

    if (!Code) {
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
                url: BaseUrl + "service/msdevice/delete",
                type: "POST",
                contentType: "application/json",
                xhrFields: {
                    withCredentials: true
                },
                data: JSON.stringify({
                    DeviceCode: Code
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


$(document).on("change", ".chk-detail", function () {
    let row = $(this).closest("tr");
    let data = $('#tableDetail')
        .DataTable()
        .row(row)
        .data();

    let courseCode = data.CourseCode;
    let currentValue = $(this).is(":checked");
    let originalValue = originalData[courseCode] ?? false;

    if (currentValue === originalValue) {
        delete changedData[courseCode];
    } else {
        changedData[courseCode] = {
            DeviceCode: $("#CodeSettings").val(),
            PlantCode: $("#PlantCodeSettings").val(),
            CourseCode: courseCode,
            IsChecked: currentValue
        };
    }

});

//SUBMIT DETAIL SETTING BATCH
$(document).on("click", ".btn-submitdetail", function () {
    let listData = Object.values(changedData);

    if (listData.length === 0) {
        toastr.info("No change data detected!", "Info", { timeOut: 3000, "closeButton": true });
        return;
    }

    let model = {
        SubmitBatch: listData
    };

    $.ajax({
        type: "POST",
        url: buildServiceUrl("service/msdevice/submit-batch"),
        data: JSON.stringify(model),
        contentType: "application/json",
        dataType: "json",
        xhrFields: {
            withCredentials: true
        },
        success: function (result) {
            if (!result.success) {
                toastr.warning(result.message);
                return;
            }

            toastr.success(result.message, 'success', { timeOut: 3000, "closeButton": true });
            $('#ModalDeviceDetail').modal('hide');

            originalData = {};
            changedData = {};

            table.ajax.reload(null, false);
        }
    });
});

//SUBMIT DETAIL SETTING BATCH OLD
//$(document).on("click", ".btn-submitdetail", function () {
//    //let tableIDList = $('#tableDetail').DataTable();
//    let listData = [];

//    $('#tableDetail').DataTable().rows().every(function () {
//        let row = this.node();
//        let data = this.data();

//        let IsCheck = $(row).find('.chk-detail').is(':checked');

//        // 🔥 hanya push kalau ada yg dicentang
//        if (IsCheck) {
//            listData.push({
//                DeviceCode: $("#CodeSettings").val(),
//                PlantCode: $("#PlantCodeSettings").val(),
//                CourseCode: data.CourseCode
//            });
//        }
//    });

//    // 🔥 VALIDASI: tidak ada yg dicentang
//    if (listData.length === 0) {
//        toastr.info("Please select data to submit data");
//        return;
//    }

//    // 🔥 final model
//    let model = {
//        SubmitBatch: listData
//    };


//    $.ajax({
//        type: "POST",
//        url: buildServiceUrl("service/msdevice/submit-batch"),
//        data: JSON.stringify(model),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        xhrFields: {
//            withCredentials: true
//        },
//        data: JSON.stringify(model),
//        success: function (result) {

//            if (!result.success) {
//                toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });
//            }
//            else {
//                toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
//                $('#ModalDeviceDetail').modal('hide');
//                table.ajax.reload(null, false); // Reload tanpa mereset halaman (stay di page current datatable)
//            }

//        }
//    });
//});

function init() {
    populateDDL({
        url: "service/msdevice/get-plant",
        dropdownId: "PlantCode",
        codeField: "PlantCode",
        descField: "Description",
        descTextbox: "PlantName",
        width: "150px",

    });

    loadData();

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
            url: BaseUrl + "service/msdevice/list",
            type: "POST",
            xhrFields: {
                withCredentials: true
            },
            contentType: "application/json",
            //complete: function () {
            //    setTimeout(waitingDialog.hide(), 200);
            //},
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
                                    <button class="btn btn-xs theme-color btn-edit" data-id="${row.DeviceCode}">
                                        <i class="fa fa-edit"></i>
                                    </button>
                                    <button class="btn btn-xs theme-color" disabled>
                                        <i class="fa fa-trash-o"></i>
                                    </button>
                                    <button class="btn btn-xs btn-default btn-settings" disabled>
                                        <i class="fa fa-cog"></i>
                                    </button >
                                `;

                    } else {

                        return `
                                    <button class="btn btn-xs theme-color btn-edit" data-id="${row.DeviceCode}" >
                                        <i class="fa fa-edit"></i>
                                    </button >
                                    <button class="btn btn-xs theme-color btn-delete" data-id="${row.DeviceCode}">
                                        <i class="fa fa-trash-o"></i>
                                    </button>
                                    <button class="btn btn-xs theme-color btn-settings" data-ht="${row.DeviceCode}" data-plant="${row.PlantCode}" title="Settings">
                                        <i class="fa fa-cog"></i>
                                    </button>
                                `;
                    }
                }
            },
            { data: "DeviceCode" },
            { data: "DeviceName" },
            { data: "PlantCode" },
            { data: "PlantName" },
            { data: "SerialNo" },
            {
                data: "RegisterDate", render: function (data) {

                    if (!data || data === "0001-01-01T00:00:00") return "";
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
//new $.fn.dataTable.FixedColumns(table, {
//    leftColumns: 3
//});

function loadDetailTable(Code, plantCode) {
    //if (tableDetail) {
    //    table.ajax.reload(null, false);
    //    return;
    //}

    tableDetail = $("#tableDetail").DataTable({
        destroy: true,
        autoWidth:false,
        ajax: {
            url: BaseUrl + "service/msdevice/list-detail",
            data: function () {
                return JSON.stringify({
                    DeviceCode: Code,
                    PlantCode: plantCode
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
                data: "Check",
                className: "text-center",
                render: function (data, type, row) {
                    // untuk sorting
                    if (type === 'sort' || type === 'type') {
                        return data == "1" ? 1 : 0;
                    }

                    //var isChecked = row.Check == 0 ? 'checked' : '';
                    return `<input  type="checkbox"
                                    class="chk-detail"
                                    data-ht="${row.DeviceCode}"
                                    data-plant="${row.PlantCode}"
                                    data-course="${row.CourseCode}"
                            value="${row.CourseCode}" ${row.Check =="1" ? "checked" : ""} />`;
                }
            },
            { data: "PlantCode" },
            { data: "CourseCode" },
            { data: "Title" }
        ],
        order: [[0, "desc"]]  , // Check=1 di atas
        drawCallback: function () {

            this.api().rows().every(function () {

                let data = this.data();

                originalData[data.CourseCode] = data.Check;

            });
        },
        autoWidth: false,
        searching: true,
        ordering: true,
        paging: true,
        info: true,
        bSort: true,
        bLengthChange: true,
        pageLength: 15,
        scrollX: true,
        scrollY: "35vh",
        processing: false,
        serverSide: false,
        responsive: true,
        "lengthMenu": [[15, 25, 50, 75, 100, -1], [15, 25, 50, 75, 100, "ALL"]],
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

