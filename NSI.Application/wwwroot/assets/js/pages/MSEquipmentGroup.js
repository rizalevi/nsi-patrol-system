var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;

init();

/*
============================================
            onchange ddl
============================================
*/
//cluster
$(document).on("change", "#ClusterCodeFilter", function () {
    let desc = $("#ClusterCodeFilter option:selected").data("desc");
    $("#ClusterNameFilter").val((desc || "").trim());

    $("#tableID").dataTable().fnClearTable();
});


//plant
$(document).on("change", "#PlantCodeFilter", function () {
    
    let desc = $("#PlantCodeFilter option:selected").data("desc");
    $("#PlantNameFilter").val((desc || "").trim());
      

    populateDDL({
        url: "service/inspectiondata/get-course",
        dropdownId: "CourseCodeFilter",
        model: {
            ClusterCode: $("#ClusterCodeFilter").val(),
            PlantCode: $("#PlantCodeFilter").val()
        },
        codeField: "Code",
        descField: "Description",
        descTextbox: "CourseTitleFilter",
       
    });


    $("#tableID").dataTable().fnClearTable();
    
});


$(document).on("change", "#CourseCodeFilter", function () {

    populateDDL({
        url: "service/inspectiondata/get-course",
        dropdownId: "ModalCourseCode",
        model: {
            ClusterCode: $("#ClusterCodeFilter").val(),
            PlantCode: $("#PlantCodeFilter").val()
        },
        codeField: "Code",
        descField: "Description",
        descTextbox: "ModalCourseName",
        selectedValue:$("#CourseCodeFilter").val()
    });


    updateNavButton();
});
/*
============================================
            action button
============================================
*/
$(document).on("click", ".btn-search", function () {
    if ($("#ClusterCodeFilter").val() == "" || $("#ClusterCodeFilter").val() == null) {
        toastr.info('Please Select Cluster', 'Info', { timeOut: 3000 });
        return;
    }

    if ($("#PlantCodeFilter").val() == "" || $("#PlantCodeFilter").val() == null) {
        toastr.info('Please Select Plant', 'Info', { timeOut: 3000 });
        return;
    }

    if ($("#CourseCodeFilter").val() == "" || $("#CourseCodeFilter").val() == null) {
        toastr.info('Please Select Course', 'Info', { timeOut: 3000 });
        return;
    }

    if (table) {
        table.ajax.reload(null, false);

    } else {
        loadData();
    }
});

$(document).on("click", ".btn-clear", function () {
    $("#PlantCodeFilter").val(null).trigger('change');
    $("#PlantNameFilter").val("");
    $("#CourseCodeFilter").val(null).trigger('change');
    $("#CourseTitleFilter").val("");

    table.ajax.reload();
});

$(document).on("click", ".btn-next", function () {
    let ddl = $("#CourseCodeFilter")[0];

    if (ddl.selectedIndex < ddl.options.length - 1) {
        ddl.selectedIndex++;
        $("#CourseCodeFilter").trigger("change");
    }
});

$(document).on("click", ".btn-prev", function () {
    let ddl = $("#CourseCodeFilter")[0];

    if (ddl.selectedIndex > 1) {
        ddl.selectedIndex--;
        $("#CourseCodeFilter").trigger("change");
    }
});

// =========================================
// MODAL CRUD OPERATIONS
// =========================================

function AddNewData() {
    // Validasi VB: Harus pilih Course dulu sebelum Add? (Opsional, tapi di sini kita defaultkan)
    if ($("#ClusterCodeFilter").val() == "" || $("#ClusterCodeFilter").val() == null) {
        toastr.info('Please Select Cluster', 'Info', { timeOut: 3000 });
        return;
    }

    if ($("#PlantCodeFilter").val() == "" || $("#PlantCodeFilter").val() == null) {
        toastr.info('Please Select Plant', 'Info', { timeOut: 3000 });
        return;
    }

    if ($("#CourseCodeFilter").val() == "" || $("#CourseCodeFilter").val() == null) {
        toastr.info('Please Select Course', 'Info', { timeOut: 3000 });
        return;
    }
   
    $("#GroupCode").val(""); // Kosong = Backend akan auto-generate
    $("#Description").val("");

    $("#ModalCourseCode").val($("#CourseCodeFilter").val()).trigger('change');
    $("#ModalCourseCode").prop("disabled", false);

    $("#ModalTitle").html("Add Group Master");
    $('#InsertData').show();
    $('#UpdateData').hide();
    $("#MyModal").modal("show");
}

$(document).on("click", ".btn-edit", function () {
    var code = $(this).data("id");

    $.ajax({
        url: BaseUrl + "service/msEquipmentgroup/detail",
        type: "POST",
        contentType: "application/json",
        xhrFields: { withCredentials: true },
        data: JSON.stringify({ GroupCode: code }),
        success: function (res) {
            if (res && res.data) {
                $("#GroupCode").val(res.data.GroupCode);
                $("#GroupCode").prop("disabled", true); 

                $("#Description").val(res.data.Description);


                $("#ModalCourseCode").val(res.data.CourseCode).trigger('change.select2');
                let coursedesc = $("#ModalCourseCode option:selected").data("desc");
                $("#ModalCourseName").val(coursedesc);
                $("#ModalCourseCode").prop("disabled", true); 

                $("#ModalTitle").html("Edit Group Master");
                $("#InsertData").hide();
                $("#UpdateData").show();
                $("#MyModal").modal("show");
            }
        },
        error: function (xhr) {
            toastr.warning('Failed to load data.', 'Error', { timeOut: 3000 });
        }
    });
});

// INSERT
$(document).on("click", "#InsertData", function () {
    var model = {
        CourseCode: $('#ModalCourseCode').val(),
        GroupCode: $('#GroupCode').val(), // Dikirim kosong biar backend yang set AutoNumber
        Description: $('#Description').val()
    };

    if (!model.CourseCode || !model.Description) {
        toastr.warning("Course and Description are required!", 'Warning');
        return;
    }

    $.ajax({
        type: "POST",
        url: BaseUrl + "service/MSEquipmentGroup/create",
        data: JSON.stringify(model),
        contentType: "application/json",
        xhrFields: { withCredentials: true },
        success: function (result) {
            if (!result.success) {
                toastr.warning(result.message, 'Warning', { timeOut: 3000 });
            } else {
                table.ajax.reload(null, false);
                toastr.success(result.message, 'Success');
                $('#MyModal').modal('hide');
            }
        }
    });
});

// UPDATE
$(document).on("click", "#UpdateData", function () {
    var model = {
        GroupCode: $('#GroupCode').val(),
        Description: $('#Description').val(),
        CourseCode: $('#ModalCourseCode').val()
    };

    if (!model.Description) {
        toastr.warning("Description is required!", 'Warning'); return;
    }

    $.ajax({
        type: "PATCH",
        url: BaseUrl + "service/MSEquipmentGroup/update",
        data: JSON.stringify(model),
        contentType: "application/json",
        xhrFields: { withCredentials: true },
        success: function (result) {
            if (!result.success) { toastr.warning(result.message, 'Warning', { timeOut: 3000 }); } else { table.ajax.reload(null, false); toastr.success(result.message, 'Success'); $('#MyModal').modal('hide'); }
        }
    });
});

// DELETE
$(document).on("click", ".btn-delete", function () {
    let groupCode = $(this).data("group");
    let courseCode = $(this).data("course");

    Swal.fire({
        title: 'Are you sure?',
        text: "Data will be deleted permanently!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: BaseUrl + "service/MSEquipmentGroup/delete",
                type: "POST",
                contentType: "application/json",
                xhrFields: { withCredentials: true },
                data: JSON.stringify({
                    GroupCode: groupCode,
                    CourseCode: courseCode
                }),
                success: function (result) {
                    if (!result.success) { toastr.warning(result.message, 'Warning'); } else { toastr.success(result.message, 'Success'); table.ajax.reload(null, false); }
                }
            });
        }
    });
});


/*===========================================
                   FUNCTION
============================================*/

function init() {
   
    populateDDL({
        url: "service/inspectiondata/get-cluster",
        dropdownId: "ClusterCodeFilter",
        codeField: "Code",
        descField: "Description",
        descTextbox: "ClusterNameFilter",
        callback: function () {
            populateDDL({
                url: "service/inspectiondata/get-plant",
                dropdownId: "PlantCodeFilter",
                model: {
                    ClusterCode: $("#ClusterCodeFilter").val()
                },
                codeField: "Code",
                descField: "Description",
                descTextbox: "PlantNameFilter",
                function() {
                    $("#PlantCodeFilter").trigger('changed.bs.select');
                }
            });
                        
        }
    });

    $("#CourseCodeFilter").select2({
        theme: 'bootstrap-5',
        placeholder: "Fill a data"
    });

    loadData();
}


// =========================================
// DATATABLES
// =========================================
function loadData() {
    if (table) {
        table.ajax.reload(null, false);
        return;
    }

    table = $("#tableID").DataTable({
        destroy: true,
        ajax: {
            url: BaseUrl + "service/MSEquipmentGroup/list",
            type: "POST",
            contentType: "application/json",
            xhrFields: { withCredentials: true },
            data: function (d) {
                return JSON.stringify({
                    CourseCode: $("#CourseCodeFilter").val(),
                    PlantCode: $("#PlantCodeFilter").val()
                });
            },
            dataSrc: function (json) {
                return json.data;
            }
        },
        columns: [
            {
                data: null,
                className: "text-center",
                render: function (data, type, row) {
                    let delBtn = allowDelete == '0'
                        ? `<button class="btn btn-xs theme-color btn-delete" disabled><i class="fa fa-trash-o"></i></button>`
                        : `<button class="btn btn-xs theme-color btn-delete" data-group="${row.GroupCode}" data-course="${row.CourseCode}"><i class="fa fa-trash-o"></i></button>`;

                    return `
                        <button class="btn btn-xs theme-color btn-edit" data-id="${row.GroupCode}">
                            <i class="fa fa-edit"></i>
                        </button>  
                        ${delBtn}
                    `;
                }
            },
            { data: "GroupCode" },
            { data: "BarcodeNo" },
            { data: "Description" },
            { data: "CourseCode" },
            {
                data: "RegisterDate",
                render: function (data) { return formatDateTime(data); }
            },
            { data: "RegisterUser" },
            {
                data: "LastUpdate",
                render: function (data) {
                    if (!data || data === "0001-01-01T00:00:00") return "";
                    return formatDateTime(data);
                }
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
        scrollY: "calc(105vh - 545px)",
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

function updateNavButton() {
    if (table) {
        table.ajax.reload(null, false);

    } else {
        loadData();
    }
    let ddl = $("#CourseCodeFilter")[0];

    $(".btn-prev").prop("disabled", ddl.selectedIndex === 0);

    $(".btn-next").prop("disabled", ddl.selectedIndex === ddl.options.length - 1);

}
