var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;


//DDL init
//setupSelectPicker('#ClusterCodeFilter');
//setupSelectPicker('#PlantCodeFilter');
//setupSelectPicker('#PlantCode');


init();
loadData();
/*
============================================
            onchange ddl
============================================
*/

//cluster
$(document).on("change", "#ClusterCodeFilter", function () {
   
    let text = $("#ClusterCodeFilter option:selected").text();
    let desc = text.split(" - ")[1];

    $("#ClusterNameFilter").val((desc || "").trim());
    $("#tableID").dataTable().fnClearTable();

});


$(document).on("changed.bs.select", "#PlantCodeFilter", function () {
    
    let text = $("#PlantCodeFilter option:selected").text();
    let desc = text.split(" - ")[1];

    $("#PlantNameFilter").val((desc || "").trim());

    $("#tableID").dataTable().fnClearTable();
});

//$(document).on("changed.bs.select", "#PlantCode", function () {
//    console.log('changed');
//    let text = $("#PlantCode option:selected").text();
//    let desc = text.split(" - ")[1];
    

//    $("#PlantName").val((desc || "").trim());
//});


//$(document).on("changed.bs.select", "#FrequencyModal", function () {

//    let text = $("#FrequencyModal option:selected").text();
//    let desc = text.split(" - ")[1];
//    console.log(desc);

//    $("#Description").val((desc || "").trim());
//});


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

    if (table) {
        table.ajax.reload(null, false);

    } else {
        loadData();
    }
});


$(document).on("click", ".btn-clear", function () {
    $("#PlantCodeFilter").val('');
    $("#PlantNameFilter").val("");

    table.ajax.reload();
});

// =========================================
// 3. MODAL CRUD OPERATIONS
// =========================================

function AddNewData() {

    if ($("#ClusterCodeFilter").val() == "" || $("#ClusterCodeFilter").val() == null) {
        toastr.info('Please Select Cluster', 'Info', { timeOut: 3000 });
        return;
    }

    if ($("#PlantCodeFilter").val() == "" || $("#PlantCodeFilter").val() == null) {
        toastr.info('Please Select Plant', 'Info', { timeOut: 3000 });
        return;
    }


    $("#form")[0].reset();
    //$(".selectpicker").selectpicker('refresh');
    $("#ClusterCode").val('');
   
    $("#ModalTitle").html("Add New Course");
    $('#InsertData').show();
    $('#UpdateData').hide();
    $("#MyModal").modal("show");

    // Enable primary keys untuk input baru
    $("#PlantCode").val(null).trigger('change.select2');
    $("#FrequencyModal").val(null).trigger('change.select2');

    $("#PlantCode").prop('disabled', false);
    $("#CourseCode").prop('readonly', false);
}

// GET DATA DETAIL (EDIT)
$(document).on("click", ".btn-edit", function () {
    var cluster = $("#ClusterCodeFilter").val();
    var plant = $("#PlantCodeFilter").val();
    var course = $(this).data("course");

    $.ajax({
        url: BaseUrl + `service/mscourse/detail`,
        type: "POST",
        xhrFields: { withCredentials: true },
        contentType: "application/json",
        data: JSON.stringify({ ClusterCode: cluster, PlantCode: plant, CourseCode: course }),
        success: function (res) {
            if (res.data) {
                
                $("#PlantCode").val(res.data.PlantCode).trigger('change.select2');
                let Plantdesc = $("#PlantCode option:selected").data("desc");

                $("#PlantName").val(Plantdesc);

                $("#CourseCode").val(res.data.CourseCode).prop("readonly", true);
                $("#Title").val(res.data.Title);
                $("#DocNo").val(res.data.DocNo);
                $("#FrequencyModal").val(res.data.CheckType).trigger('change.select2');

                let Freqdesc = $("#FrequencyModal option:selected").data("desc");
                $("#Description").val(Freqdesc);

                $("#ModalTitle").html("Edit Course");
                $("#InsertData").hide();
                $("#UpdateData").show();
                $("#MyModal").modal("show");
            }
        },
        error: function () {
            toastr.error('Failed to load detail data.', 'Error', { timeOut: 3000 });
        }
    });
});

// INSERT
$(document).on("click", "#InsertData", function () {
    var model = getFormData();
    if (!validateForm(model)) return;

    $.ajax({
        type: "POST",
        url: BaseUrl + "service/mscourse/create",
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        xhrFields: { withCredentials: true },
        success: handleResponse
    });
});

// UPDATE
$(document).on("click", "#UpdateData", function () {
    var model = getFormData();
    if (!validateForm(model)) return;

    $.ajax({
        type: "POST",
        url: BaseUrl + "service/mscourse/update",
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        xhrFields: { withCredentials: true },
        success: handleResponse
    });
});

// DELETE
$(document).on("click", ".btn-delete", function () {
    var cluster = $("#ClusterCodeFilter").val();
    var plant = $(this).data("plant");
    var course = $(this).data("course");

    Swal.fire({
        title: 'Are you sure?',
        text: "Data will be deleted permanently!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: BaseUrl + `service/mscourse/remove`,
                type: "POST",
                xhrFields: { withCredentials: true },
                contentType: "application/json",
                data: JSON.stringify({ ClusterCode: cluster, PlantCode: plant, CourseCode: course }),
                success: function (result) {
                    if (result.status === false || result.status === "false") {
                        toastr.warning(result.message, 'Warning');
                    } else {
                        toastr.success(result.message, 'Success');
                        table.ajax.reload(null, false); // Reload tanpa reset halaman
                    }
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
        url: "service/mscourse/filtercluster",
        dropdownId: "ClusterCodeFilter",
        codeField: "ClusterCode",
        descField: "ClusterDesc",
        descTextbox: "ClusterNameFilter",
        width: "130px",
        callback: function () {
            populateDDL({
                url: "service/inspectiondata/get-plant",
                dropdownId: "PlantCodeFilter",
                model: {
                    ClusterCode:$("#ClusterCodeFilter").val()
                },
                codeField: "Code",
                descField: "Description",
                descTextbox: "PlantNameFilter",
                width: "130px",
                function() {
                    $("#PlantCodeFilter").trigger('changed.bs.select');
                }
            });

            populateDDL({
                url: "service/inspectiondata/get-plant",
                dropdownId: "PlantCode",
                model: {
                    ClusterCode: $("#ClusterCodeFilter").val()
                },
                codeField: "Code",
                descField: "Description",
                descTextbox: "PlantName",
                width: "130px",
                function() {
                    $("#PlantCode").trigger('changed.bs.select');
                }
            });

           
        }
    });

    populateDDL({
        url: "service/mscourse/filterfrequency",
        dropdownId: "FrequencyModal",
        codeField: "FrequencyCode",
        descField: "FrequencyDesc",
        descTextbox: "Description",
        width: "130px"

    });

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
            url: BaseUrl + "service/mscourse/list",
            type: "POST", // Sesuai API di file aslimu
            xhrFields: { withCredentials: true },
            contentType: "application/json",

            data: function (d) {
                return JSON.stringify({
                    ClusterCode: $("#ClusterCodeFilter").val(),
                    PlantCode: $("#PlantCodeFilter").val()
                });
            },

            dataSrc: function (json) {
                return json.Data || json.data || [];
            }
        },
        columns: [
            {
                data: null,
                className: "text-center",
                render: function (data, type, row) {
                    let delBtn = allowDelete == '0'
                        ? `<button class="btn btn-xs theme-color" disabled><i class="fa fa-trash-o"></i></button>`
                        : `<button class="btn btn-xs theme-color btn-delete" data-cluster="${row.ClusterCode}" data-plant="${row.PlantCode}" data-course="${row.CourseCode}"><i class="fa fa-trash-o"></i></button>`;

                    return `
                        <button class="btn btn-xs theme-color btn-edit" data-cluster="${row.ClusterCode}" data-plant="${row.PlantCode}" data-course="${row.CourseCode}">
                            <i class="fa fa-edit"></i>
                        </button>  
                        ${delBtn}
                    `;
                }
            },
            { data: "CourseCode" },
            { data: "Title" },
            { data: "DocNo" },
            { data: "StatusShift" },
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
            }
        ],
        autoWidth:false,
        searching: true,
        ordering: true,
        paging: true,
        info: true,
        bSort: true,
        bLengthChange: true,
        pageLength: 25,
        scrollX: true,
        scrollY: "calc(105vh - 505px)",
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


// =========================================
// 4. UTILITY FUNCTIONS
// =========================================
function getFormData() {
    return {
        ClusterCode: $('#ClusterCodeFilter').val(),
        PlantCode: $('#PlantCode').val(),
        CourseCode: $('#CourseCode').val(),
        Title: $('#Title').val(),
        DocNo: $('#DocNo').val(),
        CheckType: $('#FrequencyModal').val(),
        StatusShift: $('#FrequencyModal option:selected').text()
    };
}

function validateForm(model) {
    if (!model.ClusterCode || !model.PlantCode || !model.CourseCode || !model.Title || !model.DocNo) {
        toastr.warning("Please fill all required fields (*)", "Validation Error");
        return false;
    }
    return true;
}

function handleResponse(result) {
    if (result.status === false || result.status === "false") {
        toastr.warning(result.message, 'Warning', { timeOut: 3000 });
    } else {
        toastr.success(result.message, 'Success');
        $('#MyModal').modal('hide');
        table.ajax.reload(null, false); // Reload tanpa mereset halaman (stay di page current datatable)
    }
}
