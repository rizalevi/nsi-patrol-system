var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;

init();

//cluster
$(document).on("change", "#ClusterCode", function () {
    let desc = $("#ClusterCode option:selected").data("desc");
    $("#ClusterName").val((desc || "").trim());

    $("#tableID").dataTable().fnClearTable();
});



//plant
$(document).on("change", "#PlantCode", function () {

    let desc = $("#PlantCode option:selected").data("desc");
    $("#PlantName").val((desc || "").trim());


    populateDDL({
        url: "service/inspectiondata/get-course",
        dropdownId: "CourseCode",
        model: {
            ClusterCode: $("#ClusterCode").val(),
            PlantCode: $("#PlantCode").val()
        },
        codeField: "Code",
        descField: "Description",
        descTextbox: "CourseName",

    });


    $("#tableID").dataTable().fnClearTable();

});

//course
$(document).on("change", "#CourseCode", function () {

    let desc = $("#CourseCode option:selected").data("desc");
    $("#CourseName").val((desc || "").trim());

    populateDDL({
        url: "service/inspectiondata/get-group",
        dropdownId : "GroupCode", //ddl id
        model: {
            CourseCode: $("#CourseCode").val(),
            PlantCode: $("#PlantCode").val()
        }, 
        codeField:"Code",
        descField:"Description",
        descTextbox:"GroupName",
    });
    $("#tableID").dataTable().fnClearTable();

    //updateNavButton();
});

//group
$(document).on("change", "#GroupCode", function () {

    let desc = $("#GroupCode option:selected").data("desc");
    $("#GroupName").val((desc || "").trim());

    updateNavButton();
    $("#tableID").dataTable().fnClearTable();
});

//unit
$(document).on("change", "#UnitCode", function () {
  
    let selected = $(this).find("option:selected");
    let methode = selected.data("extra"); // ✅ ambil dari attribute
    let code = selected.val();
    handleUnitMethode(methode);
});

//search data
$(document).on("click", ".btn-search", function () {
    if ($("#ClusterCode").val() == "" || $("#ClusterCode").val() == null) {
        toastr.info('Please Select Cluster', 'Info', { timeOut: 3000 });
        return;
    }

    if ($("#PlantCode").val() == "" || $("#PlantCode").val() == null) {
        toastr.info('Please Select Plant', 'Info', { timeOut: 3000 });
        return;
    }

    if ($("#CourseCode").val() == "" || $("#CourseCode").val() == null) {
        toastr.info('Please Select Course', 'Info', { timeOut: 3000 });
        return;
    }

    if ($("#GroupCode").val() == "" || $("#GroupCode").val() == null) {
        toastr.info('Please Select Group', 'Info', { timeOut: 3000 });
        return;
    }

    if (table) {
        table.ajax.reload(null, false);

    } else {
        loadData();
    }
});

$(document).on("click", ".btn-clear", function () {
    $("#PlantCode").val('').trigger('change');
    $("#PlantName").val("");
    $("#CourseCode").val().trigger('change');
    $("#CourseName").val("");
    $("#GroupCode").val().trigger('change');
    $("#GroupName").val("");

    $("#tableID").dataTable().fnClearTable();
});


//POPUP MODAL GET DATA
$(document).on("click", ".btn-edit", function () {

    var code = $(this).data("id");

    $.ajax({
        url: BaseUrl + "service/msinspectionpoint/detail",
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            CourseCode: $("#CourseCode").val(),
            GroupCode: $("#GroupCode").val(),
            InspectionCode: code
        }),
        success: function (res) {
            
            if (res && res.data) {
                $("#InspectionCode").val(res.data.InspectionCode);
                $("#InspectionPoint").val(res.data.InspectionPoint);
                $("#UnitCode").val(res.data.UnitCode.trim()).trigger('change');
                $("#MinimumRange").val(res.data.Minimum);
                $("#MaximumRange").val(res.data.Maximum);
                $("#ChartType").val(res.data.Chart.trim()).trigger('change');

                // $("#StatusNonActive").val(res.data.Status);
                $("#StatusNonActive").prop("checked", res.data.Status === "0" || res.data.Status === 0);
                // model.Status = $("#StatusNonActive").is(":checked") ? "0" : "1";

                $("#InspectionCode").prop("readonly", true);

                $("#ClusterCode").prop("readonly", true);
                $("#ClusterName").prop("readonly", true);

                $("#ClusterCode").val(res.data.ClusterCode);
                $("#ClusterName").val(res.data.ClusterName);

                $("#ModalTitle").html("Inspection Point Master");

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

    var courseCode = $('#CourseCode').val();
    var groupCode = $('#GroupCode').val();
    var unitCode = $('#UnitCode').val();
    var unitSelected = $('#UnitCode').find('option:selected');
    var chart = $('#ChartType').val();

    if (allowUpdate === "0") {
        toastr.warning('You do not have permission to write.', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    if (!courseCode) { toastr.warning('Course is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!groupCode) { toastr.warning('Group is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!$('#InspectionCode').val()) { toastr.warning('Inspection Code is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!$('#InspectionPoint').val()) { toastr.warning('Inspection Point is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!unitCode) { toastr.warning('Unit is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }

    GetMethode(unitCode).then(function (unitMethod) {
        // Matches VB: If UnitMethod <> "01" And UnitMethod <> "" Then
        if (unitMethod !== "01" && unitMethod !== "") {
            var minRaw = $('#MinimumRange').val().replace(',', '.');
            var maxRaw = $('#MaximumRange').val().replace(',', '.');
            var minVal = parseFloat(minRaw);
            var maxVal = parseFloat(maxRaw);

            if (isNaN(minVal) || isNaN(maxVal)) {
                toastr.warning('Minimum and Maximum range must be numeric.', 'Warning', { timeOut: 3000, "closeButton": true });
                return;
            }

            if (maxVal < minVal) {
                toastr.warning('Maximum value must be greater than or equal to Minimum value.', 'Warning', { timeOut: 3000, "closeButton": true });
                return;
            }

            if (!chart || chart === "-") {
                toastr.warning('Chart Type is required.', 'Warning', { timeOut: 3000, "closeButton": true });
                return;
            }
        }

        var model = new Object();
        model.CourseCode = courseCode;
        model.GroupCode = groupCode;
        model.InspectionCode = $('#InspectionCode').val();
        model.InspectionPoint = $('#InspectionPoint').val();
        model.UnitCode = unitCode;
        model.Minimum = $('#MinimumRange').val();
        model.Maximum = $('#MaximumRange').val();
        model.Chart = chart;
        model.Option = unitMethod;
        model.Status = $("#StatusNonActive").is(":checked") ? "0" : "1";

        $.ajax({
            type: "POST",
            url: BaseUrl + "service/msinspectionpoint/create",
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
    }).catch(function (err) {
        toastr.warning('Failed to get Unit Methode.', 'Error', { timeOut: 3000, "closeButton": true });
    });
});

//UPDATE  
$(document).on("click", "#UpdateData", function () {
    var courseCode = $('#CourseCode').val();
    var groupCode = $('#GroupCode').val();
    var unitCode = $('#UnitCode').val();
    var chart = $('#ChartType').val();

    if (allowUpdate === "0") {
        toastr.warning('You do not have permission to update.', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    if (!courseCode) { toastr.warning('Course is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!groupCode) { toastr.warning('Group is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!$('#InspectionCode').val()) { toastr.warning('Inspection Code is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!$('#InspectionPoint').val()) { toastr.warning('Inspection Point is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }
    if (!unitCode) { toastr.warning('Unit is required.', 'Warning', { timeOut: 3000, "closeButton": true }); return; }

    GetMethode(unitCode).then(function (unitMethod) {

        if (unitMethod !== "01" && unitMethod !== "") {
            var minVal = parseFloat($('#MinimumRange').val());
            var maxVal = parseFloat($('#MaximumRange').val());

            if (isNaN(minVal) || isNaN(maxVal)) {
                toastr.warning('Minimum and Maximum range must be numeric.', 'Warning', { timeOut: 3000, "closeButton": true });
                return;
            }

            if (maxVal < minVal) {
                toastr.warning('Maximum value must be greater than or equal to Minimum value.', 'Warning', { timeOut: 3000, "closeButton": true });
                return;
            }

            if (!chart || chart === "-") {
                toastr.warning('Chart Type is required.', 'Warning', { timeOut: 3000, "closeButton": true });
                return;
            }
        }

        Swal.fire({
            title: 'Are you sure?',
            text: "Data will be updated!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, update it!'
        }).then((confirm) => {

            if (!confirm.isConfirmed) return;

            var model = new Object();
            model.CourseCode = courseCode;
            model.GroupCode = groupCode;
            model.InspectionCode = $('#InspectionCode').val();
            model.InspectionPoint = $('#InspectionPoint').val();
            model.UnitCode = unitCode;
            model.Minimum = $('#MinimumRange').val();
            model.Maximum = $('#MaximumRange').val();
            model.Chart = chart;
            model.Status = $("#StatusNonActive").is(":checked") ? "0" : "1";
            model.Option = unitMethod;
            model.Last_User = 'test';

            $.ajax({
                type: "PATCH",
                url: BaseUrl + "service/msinspectionpoint/update",
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

    }).catch(function (err) {
        toastr.warning('Failed to get Unit Methode.', 'Error', { timeOut: 3000, "closeButton": true });
    });
});

// DELETE
$(document).on("click", ".btn-delete", function () {

    var inspectionCode = $(this).data("id");
    var courseCode = $('#CourseCode').val();
    var groupCode = $('#GroupCode').val();

    if (!inspectionCode) {
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
                url: BaseUrl + "service/msinspectionpoint/delete",
                type: "POST",
                contentType: "application/json",
                xhrFields: {
                    withCredentials: true
                },
                data: JSON.stringify({
                    CourseCode: courseCode,
                    GroupCode: groupCode,
                    InspectionCode: inspectionCode
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


/*
=====================FUNCTION =========================
*/

function init() {
    //DDL Cluster
    populateDDL({
        url: "service/inspectiondata/get-cluster",
        dropdownId: "ClusterCode",
        codeField: "Code",
        descField: "Description",
        descTextbox: "ClusterName",
        callback: function () {
            populateDDL({
                url: "service/inspectiondata/get-plant",
                dropdownId: "PlantCode",
                model: {
                    ClusterCode: $("#ClusterCode").val()
                },
                codeField: "Code",
                descField: "Description",
                descTextbox: "PlantName",
                function() {
                    $("#PlantCode").trigger('change');
                }
            });

        }
    });

    populateDDL({
        url: "service/msinspectionpoint/units",
        dropdownId: "UnitCode",
        codeField: "UnitCode",
        descField: "UnitDescription",
        extraField: "MethodeDescription",
        
    });

    populateDDL({
        url: "service/msinspectionpoint/charts",
        dropdownId: "ChartType",
        codeField: "ChartType",
        descField: "ChartType",
    });

    //init table
    loadData();

}

function handleUnitMethode(methode) {
    
    if (methode === "Option") {
        $("#MinimumRange").prop("disabled", true).val("");
        $("#MaximumRange").prop("disabled", true).val("");
        //$("#MinimumRange").addClass('numeric-disabled');
        //$("#MaximumRange").addClass('numeric-disabled');

        $("#ChartType").val("-").trigger('change');
    } else if (methode === "Numeric") {
        $("#MinimumRange").prop("disabled", false);
        $("#MaximumRange").prop("disabled", false);
        //$("#MinimumRange").addClass('numeric-enabled');
        //$("#MaximumRange").addClass('numeric-enabled');

        if (isNaN($("#MinimumRange").val()) || $("#MinimumRange").val() === "") {
            $("#MinimumRange").val("");
        }
        if (isNaN($("#MaximumRange").val()) || $("#MaximumRange").val() === "") {
            $("#MaximumRange").val("");
        }

        $("#ChartType").val("Line Charts").trigger('change');
    }
}

function GetMethode(unitCode) {
    return new Promise(function (resolve, reject) {
        var model = new Object();
        model.UnitCode = unitCode;

        $.ajax({
            url: BaseUrl + "service/msinspectionpoint/getmethode",
            data: JSON.stringify(model),
            type: "POST",
            xhrFields: { withCredentials: true },
            contentType: "application/json",

            success: function (res) {
                let data = res.data ?? res;
                if (data && data.length > 0) {
                    resolve(data[0].Methode || "");
                } else {
                    resolve("");
                }
            },
            error: function (xhr) {
                console.log(xhr);
                toastr.warning('Failed to get Methode.', 'Error', { timeOut: 3000, "closeButton": true });
                reject("");
            }
        });
    });
}

//function AutoGenerateInspectionCode(courseCode, groupCode) {
//    return new Promise(function (resolve, reject) {
//        var model = new Object();
//        model.Course_Code = courseCode;
//        model.Group_Code = groupCode;

//        $.ajax({
//            url: BaseUrl + "service/msinspectionpoint/generateinspectioncode",
//            data: JSON.stringify(model),
//            type: "POST",
//            xhrFields: { withCredentials: true },
//            contentType: "application/json",
//            success: function (res) {
//                let data = res.data ?? res;
//                if (data && data.Inspection_Code) {
//                    resolve(data.Inspection_Code);
//                } else if (typeof data === "string" && data !== "") {
//                    resolve(data);
//                } else {
//                    resolve("");
//                }
//            },
//            error: function (xhr) {
//                console.log(xhr);
//                toastr.warning('Failed to generate Inspection Code.', 'Error', { timeOut: 3000, "closeButton": true });
//                reject("");
//            }
//        });
//    });
//}

function AddNewData(FactoryCode) {
    var clusterCode = $('#ClusterCode').val();
    var plantCode = $('#PlantCode').val();

    var courseCode = $('#CourseCode').val();
    var groupCode = $('#GroupCode').val();

    if (!clusterCode) {
        toastr.warning('Please select a Cluster.', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    if (!plantCode) {
        toastr.warning('Please select a Plant.', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    if (!courseCode) {
        toastr.warning('Please select a Course.', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }
    if (!groupCode) {
        toastr.warning('Please select a Group.', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    $("#ModalTitle").html("Inspection Point Master");
    $("#MyModal").modal("show");
    $('#InsertData').show();
    $('#UpdateData').hide();
    $("#InspectionCode").prop('readonly', true);
    $("#InspectionCode").val('(Auto Generate)');
    $("#InspectionPoint").val('');
    $("#UnitCode").val('').trigger('chande');
    $("#MinimumRange").val('').prop('disabled', false);
    $("#MaximumRange").val('').prop('disabled', false);
    $("#ChartType").val('').trigger('change');
    $("#StatusNonActive").prop("checked", true);

    //AutoGenerateInspectionCode(courseCode, groupCode).then(function (generatedCode) {
    //    $("#ModalTitle").html("Inspection Point Master");
    //    $("#MyModal").modal();
    //    $('#InsertData').show();
    //    $('#UpdateData').hide();
    //    $("#InspectionCode").prop('readonly', true);
    //    $("#InspectionCode").val(generatedCode);
    //    $("#InspectionPoint").val('');
    //    $("#Unit").val('').selectpicker('refresh');
    //    $("#MinimumRange").val('').prop('disabled', false);
    //    $("#MaximumRange").val('').prop('disabled', false);
    //    $("#ChartType").val('').selectpicker('refresh');
    //    $("#StatusNonActive").prop("checked", true);
    //}).catch(function () {
    //    toastr.warning('Failed to generate Inspection Code.', 'Error', { timeOut: 3000, "closeButton": true });
    //});
}

function loadData() {
    if (table) {
        table.ajax.reload(null, false);
        return;
    }

    table = $("#tableID").DataTable({
        destroy: true,
        autoWidth: false,
        ajax: {
            url: BaseUrl + "service/msinspectionpoint/list",
            data: function (d) {
                return JSON.stringify({
                    CourseCode: $('#CourseCode').val(),
                    GroupCode: $('#GroupCode').val()
                });
            },
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
                autoWidth: false ,
                render: function (data, type, row) {

                    if (allowDelete == '0') {
                        return `
                                    <button class="btn btn-xs theme-color btn-edit" data-id="${row.InspectionCode}">
                                        <i class="fa fa-edit"></i>
                                    </button>
                                    <button class="btn btn-xs theme-color" disabled>
                                        <i class="fa fa-trash-o"></i>
                                    </button>
                                `;

                    } else {
                        return `
                                    <button class="btn btn-xs theme-color btn-edit" data-id="${row.InspectionCode}">
                                        <i class="fa fa-edit"></i>
                                    </button>
                                    <button class="btn btn-xs theme-color btn-delete" data-id="${row.InspectionCode}">
                                        <i class="fa fa-trash-o"></i>
                                    </button>
                                `;
                    }
                }
            },
            { data: "InspectionCode", autoWidth: false },
            { data: "InspectionPoint", autoWidth: false },
            { data: "UnitDescription", autoWidth: false },
            { data: "Minimum", autoWidth: false },
            { data: "Maximum", autoWidth: false },
            { data: "Chart", autoWidth: false },
            {
                data: "Status", autoWidth: false, render: function (data) {

                    if (data === "0") return "Active";
                    return "Non Active";

                },
            },
            {
                data: "RegisterDate", autoWidth: false, render: function (data) {

                    if (!data || data === "0001-01-01T00:00:00") return "";
                    return formatDateTime(data);

                },
            },
            { data: "RegisterUser", autoWidth: false },
            {
                data: "LastUpdate", autoWidth: false, render: function (data) {

                    if (!data || data === "0001-01-01T00:00:00") return "";
                    return formatDateTime(data);

                },
            },
            { data: "LastUser", autoWidth: false },
           
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
//new $.fn.dataTable.FixedColumns(table, {
//    leftColumns: 3
//});


/* =================END FUNCTION ======================== */


/** ====== PREV & NEXT BUTTON ====== **/

function updateNavButton() {
    table.ajax.reload();
/*    loadData();*/
    let ddl = $("#GroupCode")[0];

    $(".btn-prev").prop("disabled", ddl.selectedIndex === 0);

    $(".btn-next").prop("disabled", ddl.selectedIndex === ddl.options.length - 1);

}

$(document).on("click", ".btn-next", function () {
    let ddl = $("#GroupCode")[0];

    if (ddl.selectedIndex < ddl.options.length - 1) {
        ddl.selectedIndex++;
        $("#GroupCode").trigger("change");
    }
});

$(document).on("click", ".btn-prev", function () {
    let ddl = $("#GroupCode")[0];

    if (ddl.selectedIndex >= 1) {
        ddl.selectedIndex--;
        $("#GroupCode").trigger("change");
    }
});

