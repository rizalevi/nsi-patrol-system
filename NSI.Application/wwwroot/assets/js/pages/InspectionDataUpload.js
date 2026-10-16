var table;
var tableDetail;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;

let isRedirect = false;


init();


// ==========================================
// EVENT LISTENERS
// ==========================================

//date picker
$(document).on("click", ".datepicker-icon", function () {
    $(this).closest(".datepicker-wrapper")
        .find(".datepicker")
        .datepicker("show");
});

//cluster
$(document).on("change", "#ClusterCode", function () {
    let desc = $("#ClusterCode option:selected").data("desc");
    $("#ClusterName").val((desc || "").trim());
    if (!isRedirect) {
        clearTable()
    }
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
        selectedValue: courseCode,
        callback: function () {


            populateDDL({
                url: "service/inspectiondata/get-group",
                dropdownId: "GroupCode", //ddl id
                model: {
                    CourseCode: $("#CourseCode").val(),
                    PlantCode: $("#PlantCode").val()
                },
                codeField: "Code",
                descField: "Description",
                descTextbox: "GroupName",
                selectedValue: groupCode,
                callback: function () {

                    //if ($("#CourseCode").val() != null) {
                    //    let ddl = $("#GroupCode");

                    //    let code = "ALL";
                    //    let desc = "ALL";

                    //    ddl.prepend(
                    //        '<option value="' + code + '" ' +
                    //        'data-content="<div style=\'display:flex;width:300px\'>' +
                    //        '<span style=\'width:80px;text-align:left\'>' + code + '</span>' +
                    //        '<span style=\'padding-left:10px; text-align:left;\'>' + desc + '</span>' +
                    //        '</div>" ' +
                    //        'data-code="' + code + '" ' +
                    //        'data-desc="' + desc + '">' +
                    //        code + ' - ' + desc +
                    //        '</option>'
                    //    );
                    //    ddl.trigger('change');

                    //}
                    //$("#GroupCode").trigger('change');
                    if (statusAdmin == "1" && (groupCode != "" && groupCode != null)) {
                        $("#GroupCode").prop("disabled", true).trigger('change.select2');
                    } else {
                        $("#GroupCode").prop("disabled", false).trigger('change.select2');

                    }

                }
            });

            //if (courseCode == "" && ($("#PlantCode").val() != null && $("#PlantCode").val() != "")) {
            //    let ddl = $("#CourseCode");

            //    let code = "ALL";
            //    let desc = "ALL";

            //    ddl.prepend(
            //        '<option value="' + code + '" ' +
            //        'data-content="<div style=\'display:flex;width:300px\'>' +
            //        '<span style=\'width:80px;text-align:left\'>' + code + '</span>' +
            //        '<span style=\'padding-left:10px; text-align:left;\'>' + desc + '</span>' +
            //        '</div>" ' +
            //        'data-code="' + code + '" ' +
            //        'data-desc="' + desc + '">' +
            //        code + ' - ' + desc +
            //        '</option>'
            //    );
            //    ddl.trigger('change');
            //    //ddl.selectpicker('refresh');

            //}
            //$("#CourseCode").trigger('change');

            if (statusAdmin == "1" && (courseCode != "" && courseCode != null) ) {
                $("#CourseCode").prop("disabled", true).trigger('change.select2');
            } else {
                $("#CourseCode").prop("disabled", false).trigger('change.select2');

            }

        }
    });

    if (!isRedirect) {
        clearTable()
    }
});

//course
$(document).on("change", "#CourseCode", function () {
    let desc = $("#CourseCode option:selected").data("desc");
    $("#CourseName").val((desc || "").trim());

    populateDDL({
        url: "service/inspectiondata/get-group",
        dropdownId: "GroupCode", //ddl id
        model: {
            CourseCode: $("#CourseCode").val(),
            PlantCode: $("#PlantCode").val()
        },
        codeField: "Code",
        descField: "Description",
        descTextbox: "GroupName",
        selectedValue: groupCode,
        callback: function () {

            //if ($("#CourseCode").val() != null) {
            //    let ddl = $("#GroupCode");

            //    let code = "ALL";
            //    let desc = "ALL";

            //    ddl.prepend(
            //        '<option value="' + code + '" ' +
            //        'data-content="<div style=\'display:flex;width:300px\'>' +
            //        '<span style=\'width:80px;text-align:left\'>' + code + '</span>' +
            //        '<span style=\'padding-left:10px; text-align:left;\'>' + desc + '</span>' +
            //        '</div>" ' +
            //        'data-code="' + code + '" ' +
            //        'data-desc="' + desc + '">' +
            //        code + ' - ' + desc +
            //        '</option>'
            //    );
            //    ddl.trigger('change');

            //}
            //$("#GroupCode").trigger('change');
            if (statusAdmin == "1" && (groupCode != "" && groupCode != null) ) {
                $("#GroupCode").prop("disabled", true).trigger('change.select2');
            } else {
                $("#GroupCode").prop("disabled", false).trigger('change.select2');

            }

        }
    });

    if (!isRedirect) {
        clearTable()
    }
});

//group
$(document).on("change", "#GroupCode", function () {

   
    let desc = $("#GroupCode option:selected").data("desc");
    $("#GroupName").val((desc || "").trim());
    if (!isRedirect) {
        clearTable()
    } else {
        $('.btn-search').first().trigger('click');
    }

});

$(document).on("change", "#Frequency", function () {
   
    $('.btn-search').first().trigger('click');
    clearTable()
});



////group
//$(document).on("changed.bs.select", "#GroupCode", function () {

//    let text = $("#GroupCode option:selected").text();
//    let desc = text.split(" - ")[1];   // ambil setelah "-"

//    $("#GroupName").val((desc || "").trim());

//});



//search data
//$(document).on("click", ".btn-search", function () {
//    if (table) {
//        table.ajax.reload(null, false);
//        isRedirect = false;
//    } else {
//        loadData();
//    }
//});
$(document).on("click", ".btn-search", function () {
    
    if (!validateData()) return;

    table.ajax.reload(null, false);
    
    isRedirect = false;

});
$(document).on("click", ".btn-clear", function () {
    // reset 
    $("#PlantCode").val('').trigger('change');
    $("#PlantName").val("");
    $("#CourseCode").val('').trigger('change');
    $("#CourseName").val("");
    $("#GroupCode").val('').trigger('change');
    $("#GroupName").val("");
    $("#Frequency").val('').trigger('change');
    let now = new Date();
    let firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
    let today = new Date();

    $("#InspectionDateFrom").datepicker('setDate', firstDay);
    $("#InspectionDateTo").datepicker('setDate', today);
    $("#modalRemarksContent").val('');
    clearTable();

});

//SUBMIT
$(document).on("click", ".btn-submit", function () {
    let tableDataTable = $('#tableID').DataTable();
    let allData = tableDataTable.rows().data();

    if (allData.length === 0) {
        toastr.warning("No data available to approve.",'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    let firstRow = allData[0];

    Swal.fire({
        title: 'Confirmation',
        text: "Are you sure you want to approve this inspection?",
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            let model = {
                PlantCode: $("#PlantCode").val(),
                CourseCode: $("#CourseCode").val(),
                GroupCode: firstRow.Group,  
                InspectionDate: formatDateGeneral($('#InspectionDateFrom').val()),
                Shift: $("#Frequency").val(),
                InspectionNo: firstRow.InspectionNo,
                UserID: ""  
            };

            $.ajax({
                type: "POST",
                url: buildServiceUrl("service/inspectiondataupload/approve-inspector"),
                data: JSON.stringify(model),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                xhrFields: { withCredentials: true },
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message || "Data Approved",'Success', { timeOut: 3000, "closeButton": true }); 
                        tableDataTable.ajax.reload();
                    } else {
                        toastr.warning(response.message, 'Warning', { timeOut: 3000, "closeButton": true });
                    }
                }
            });
        }
    });
});


//view detail
//$(document).on("click", ".btn-open-modal", function (e) {
//    e.preventDefault();
    
//    let docNo = $(this).data("doc");
//    let course = $(this).data("course");
//    let title = $(this).data("title");
//    let inspecdate = $(this).data("inspectdate");
//    let frequency = $(this).data("frequency");
//    let group = $(this).data("group");
//    let inspectionno = $(this).data("inspectionno");
   
//    $("#Course").val(course);
//    $("#Title").val(title);
//    $("#DocumentNo").val(docNo);
//    $("#InspectionDate").val(inspecdate);
//    $("#FrequencyName-Detail").val(frequency);
//    $("#GroupCodeModal").val(group);
//    $("#InspectionNo").val(inspectionno); 

//    loadDetail();
//    setTimeout(() => {
//        tableDetail.columns.adjust();
//    }, 100);
//    // 🔥 show modal
//    $("#MyModal").modal("show");
//});

$(document).on("click", ".btn-approve", function () {


    Swal.fire({
        title: 'Are you sure approve this inspection?',
        text: "Data will be disapear after approve!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, approve it!'
    }).then((result) => {

        if (result.isConfirmed) {
            var model = new Object();

            model.InspectionNo = $("#InspectionNo").val();
            model.CourseCode = $("#Course").val();
            model.GroupCode = $("#Group").val();

            $.ajax({ 
                url: buildServiceUrl("service/inspectiondataupload/approve-detail"),
                type: "POST",
                contentType: "application/json",
                xhrFields: {
                    withCredentials: true
                },
                data: JSON.stringify(model),
                success: function (result) {

                    if (!result.success) {
                        toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });

                    } else {

                        toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
                        $('#MyModal').modal('hide');
                        $('body').removeClass('modal-open');
                        $('.modal-backdrop').remove();

                        table.ajax.reload();
                    }
                },

            });

        }
    });

});

//unused
// View Leader Remarks Modal
//$(document).on("click", "#btnViewRemarks", function () {
//    let remarksText = $("#txtRemarks").val();

//    // Cek apakah ada remarks atau tidak
//    if (!remarksText || remarksText.trim() === "") {
//        toastr.info("No remarks available from the leader.", "Info", { timeOut: 3000, "closeButton": true });
//        return;
//    }
     
//    $("#modalRemarksContent").val(remarksText);
     
//    $("#RemarksModal").modal("show");
//});

$(document).on('dblclick', '#tableID tbody tr', function () {

    let table = $('#tableID').DataTable();
    let rowData = table.row(this).data();

    if (!rowData) return;

    openDetailModal(rowData);
});

function init() {

    GlobalDatepicker.init(".datepicker");
    if (inspectionDate != "") {
        const parsed = new Date(inspectionDate);

        $("#InspectionDateFrom").datepicker('setDate', formatDate(parsed));
        $("#InspectionDateTo").datepicker('setDate', formatDate(parsed));

        $(".btn-back").removeClass("hidden");
        $(".btn-back").addClass("show");
    } else {
        let now = new Date();
        let firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
        let today = new Date();

        $("#InspectionDateFrom").datepicker('setDate', firstDay);
        $("#InspectionDateTo").datepicker('setDate', today);
        //GlobalDatepicker.setDefault("#InspectionDateFrom", 0);
        //GlobalDatepicker.setDefault("#InspectionDateTo", 2);
        $(".btn-back").removeClass("show");
        $(".btn-back").addClass("hidden");
    }


    populateDDL({
        url: "service/inspectiondata/get-cluster",
        dropdownId: "ClusterCode",
        codeField: "Code",
        descField: "Description",
        descTextbox: "ClusterName",
        selectedValue: clusterCode,
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
                selectedValue: plantCode,
                callback: function () {
                    $("#PlantCode").trigger('change');
                    if (statusAdmin == "1" && (plantCode != "" && plantCode != null) ) {
                        $("#PlantCode").prop("disabled", true).trigger('change.select2');
                    } else {
                        $("#PlantCode").prop("disabled", false).trigger('change.select2');

                    }
                }
            });

        }

    });
    //$("#ClusterCode").val(clusterCode).trigger('change.select2');


    //DDL Frequency
    populateDDL({
        url: "service/inspectiondata/get-frequency",
        dropdownId: "Frequency",
        codeField: "Code",
        descField: "Description",
        selectedValue: frequency,
        callback: function () {

            //if (frequency == "") {
            //    let ddl = $("#Frequency");

            //    let code = "ALL";
            //    let desc = "ALL";


            //    ddl.prepend(
            //        '<option value="' + code + '" ' +
            //        'data-content="<div style=\'display:flex;width:300px\'>' +
            //        '<span style=\'width:100px;text-align:left\'>' + code + '</span>' +
            //        '<span style=\'padding-left:30px; text-align:left;\'>' + desc + '</span>' +
            //        '</div>" ' +
            //        'data-code="' + code + '" ' +
            //        'data-desc="' + desc + '">' +
            //        code +
            //        '</option>'
            //    );

            //    ddl.trigger('change');
            //}
            if (statusAdmin == "1" && (frequency != "" && frequency != null)) {
                $("#Frequency").prop("disabled", true).trigger('change.select2');
            } else {
                $("#Frequency").prop("disabled", false).trigger('change.select2');

            }
            

        }
    });


    if (inspectionDate != "" && frequency != "") {
        isRedirect = true;


        if (statusAdmin == "1") {
            $("#InspectionDateFrom").prop("disabled", true);
            $(".btn-submit").prop("disabled", true);
            $(".btn-approve").prop("disabled", true);
            $(".btn-save-detail").prop("disabled", true);

        } else {
            $("#InspectionDateFrom").prop("disabled", false);
            $(".btn-submit").prop("disabled", false);
            $(".btn-approve").prop("disabled", false);
            $(".btn-save-detail").prop("disabled", false);
        }

    } else {
        isRedirect = false;

        if (statusAdmin == "1") {
            $(".btn-submit").prop("disabled", true);
            $(".btn-approve").prop("disabled", true);
            $(".btn-save-detail").prop("disabled", true);


        } else {
            $(".btn-submit").prop("disabled", false);
            $(".btn-approve").prop("disabled", false);
            $(".btn-save-detail").prop("disabled", false);
        }
    }

    table = $("#tableID").DataTable({
        destroy: true,
        autoWidth: false,
        searching: true,
        ordering: true,
        paging: true,
        info: true,
        bSort: true,
        bLengthChange: true,
        pageLength: 25,
        scrollX: true,
        scrollY: "calc(35vh)",
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
        ,
        //rowCallback: function (row, data, index) {
        //    if (data.Status && data.Status.toLowerCase() === "abnormal") {
        //        $(row).css("background-color", "yellow");
        //    }
        //}
        //drawCallback: function (settings) {
        //    let api = this.api();
        //    let data = api.rows().data();

        //    if (data.length > 0) {
        //        let firstRow = data[0];
        //        checkRemarks(firstRow);
        //        checkLastStatus(firstRow, "");
        //    } else {
        //        $("#txtRemarks").val("");
        //        $("#lblStatus").hide();
        //    }
        //},
        rowCallback: function (row, data, index) {
            if (data.Status && data.Status.toLowerCase() === "abnormal") {
                $(row).addClass("text-abnormal");
            }
           
            //checkRemarks(data);
            //$(row).off('click').on('click', function () {
            //    checkRemarks(data);
            //});
        },
        columns: [
            //{
            //    data: null, 
            //    className: "text-center",
            //    render: function (data, type, row) {
            //        return `
            //        <button class="btn btn-xs theme-color btn-edit" data-inspectioncode="${row.Inspection_Code} data-inspectionno="${row.Inspectionno} data-group="${row.Group}">
            //            <i class="fa fa-edit"></i>
            //        </button>
            //        `; 
            //    }
            //},
            {
                data: "Group", className: "text-left", autoWidth: false,
                render: function (data, type, row) {

                    if (type === "display") {
                        return `<a href="#" class="btn-open-modal"
                        data-inspectioncode="${row.Inspection_Code}"
                        data-inspectionno="${row.Inspectionno}"
                        data-group="${row.Group}">
                        ${data}
                    </a>`;
                    }

                    return data;
                }
            },
            {
                data: "Inspection_Point", className: "text-left", autoWidth: false,
                render: function (data, type, row) {

                    if (type === "display") {
                        return `<a href="#" class="btn-open-modal"
                        data-inspectioncode="${row.Inspection_Code}"
                        data-inspectionno="${row.Inspectionno}"
                        data-group="${row.Group}">
                        ${data}
                    </a>`;
                    }

                    return data;
                }
            },
            { data: "Unit", className: "text-center" },
            //{ data: "Minimum", className: "text-center" },
            //{ data: "Maximum", className: "text-center" },
            {
                data: "Minimum",
                autoWidth: false,
                render: function (data, type, row) {
                    if (type !== "display") return data ?? "";

                    return FormatInspectionResult(data);
                }
            },
            {
                data: "Maximum",
                autoWidth: false,
                render: function (data, type, row) {
                    if (type !== "display") return data ?? "";

                    return FormatInspectionResult(data);
                }
            },
            {
                data: "Inspection_Result",
                autoWidth: false,
                render: function (data, type, row) {
                    if (type !== "display") return data ?? "";

                    return FormatInspectionResult(data);
                }
            },
            { data: "Status", className: "text-center" },
            { data: "InspectionNo", className: "text-center" },
        ],
        ajax: {
            url: buildServiceUrl("service/inspectiondataupload/list"),
            type: "POST",
            contentType: "application/json",
            xhrFields: { withCredentials: true },
            //data: function (d) {
            //    return JSON.stringify({
            //        PlantCode: (plantCode != "") ? plantCode : $("#PlantCode").val(),
            //        CourseCode: (courseCode != "") ? courseCode : $("#CourseCode").val(),
            //        GroupCode: (groupCode != "") ? groupCode : $("#GroupCode").val(),
            //        Shift: (frequency != "") ? frequency : $("#Frequency").val(),
            //        InspectionDate: (inspectionDate != "") ? inspectionDate : formatDateGeneral($('#InspectionDateFrom').val())

            //    });
            //},
            data: function (d) {

                let cluster = isRedirect ? clusterCode : $("#ClusterCode").val();
                let plant = isRedirect ? plantCode : $("#PlantCode").val();
                let course = isRedirect ? courseCode : $("#CourseCode").val();
                let group = isRedirect ? groupCode : $("#GroupCode").val();
                let freq = isRedirect ? frequency : $("#Frequency").val();
                let inspDate = isRedirect ? inspectionDate : $("#InspectionDateFrom").val();

                return JSON.stringify({
                    ClusterCode: cluster,
                    PlantCode: plant,
                    CourseCode: course,
                    GroupCode: group,
                    Shift: freq,
                    InspectionDate: formatDateGeneral(inspDate)
                });
            },

            dataSrc: function (json) {
                
                //if (json.data.length === 0) {
                //    toastr.info("No data available in table", 'Info', { timeOut: 3000, "closeButton": true });
                //    return [];
                //}
                let data = json.data || [];
               
                if (!isRedirect && data.length === 0 && ($("#PlantCode").val() != null && $("#PlantCode").val() != "")) {

                    toastr.info("No data available in table", 'Info', {
                        timeOut: 3000,
                        closeButton: true
                    });
                }
                
                if (data.length > 0) {
                    $("#modalRemarksContent").val(data[0].Remarks_SendBack);
                    checkLastStatus(data[0], "");
                } else {
                    $("#modalRemarksContent").val('');
                    $("#divstatus").hide();
                }


                return data;

            }
        },
       
    });
}

function loadDetail() {
    if (tableDetail) {
        tableDetail.ajax.reload(null, false);
        return;
    }


    tableDetail = $("#tableID-Detail").DataTable({
        destroy: true,
        ajax: { 
            url: buildServiceUrl("service/inspectiondataupload/list-detail"),
            type: "POST",
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: function (d) {
                return JSON.stringify({
                    CourseCode: $("#Course").val(),
                    GroupCode: $("#Group").val(),
                    InspectionDate: formatDateGeneral($('#InspectionDate').val()),
                    Frequency: $("#FrequencyName-Detail").val()

                });
            },
            dataSrc: function (json) {
                return json.data;
            }
        },
        columns: [
            {
                data: null, className: "v-align",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: "Group", "autoWidth": true },
            { data: "InspectionPoint", "autoWidth": true },
            { data: "Unit", "autoWidth": true },
            //{ data: "Minimum", "autoWidth": true },
            //{ data: "Maximum", "autoWidth": true },
            {
                data: "Minimum",
                autoWidth: false,
                render: function (data, type, row) {
                    if (data == null || data === "") return "";

                    // Jika numeric
                    if (!isNaN(data) && data.toString().trim() !== "") {
                        return parseFloat(data)
                            .toFixed(3)
                            .replace(/\.?0+$/, "");
                    }

                    // Jika string seperti YES / NO
                    return data;
                }
            },
            {
                data: "Maximum",
                autoWidth: false,
                render: function (data, type, row) {
                    if (data == null || data === "") return "";

                    // Jika numeric
                    if (!isNaN(data) && data.toString().trim() !== "") {
                        return parseFloat(data)
                            .toFixed(3)
                            .replace(/\.?0+$/, "");
                    }

                    // Jika string seperti YES / NO
                    return data;
                }
            },
            //{ data: "InspectionResult", "autoWidth": true },
            {
                data: "InspectionResult",
                autoWidth: false,
                render: function (data, type, row) {
                    if (data == null || data === "") return "";

                    // Jika numeric
                    if (!isNaN(data) && data.toString().trim() !== "") {
                        return parseFloat(data)
                            .toFixed(3)
                            .replace(/\.?0+$/, "");
                    }

                    // Jika string seperti YES / NO
                    return data;
                }
            },
            { data: "Status", "autoWidth": true },
            { data: "Remarks", "autoWidth": true } 
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
        scrollY: "calc(30vh)",
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
        ,
        rowCallback: function (row, data, index) {
            if (data.Status &&
                data.Status.toLowerCase() === "abnormal") {

                $(row).addClass("text-abnormal");
            }

        }

    });

}

function checkRemarks(rowData) {
    let model = {
        PlantCode: $("#PlantCode").val(),
        CourseCode: $("#CourseCode").val(),
        GroupCode: rowData.Group,
        InspectionDate: formatDateGeneral($('#InspectionDateFrom').val()),
        Shift: $("#Frequency").val(),
        InspectionNo: rowData.InspectionNo,
        Type: "1"  
    };
     
    $.ajax({
        type: "POST",
        url: buildServiceUrl("service/inspectiondataupload/getremarksbyleader"),
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        xhrFields: { withCredentials: true },
        success: function (result) {
            
            let note = "";
            if (result.status && result.data && result.data.length > 0) {
                note = result.data[0].Leader_SendBack || "";
            }
            //$("#txtRemarks").val(note);
            $("#modalRemarksContent").val(note);

            //if (note !== "") {
            //    $("#btnViewRemarks").removeClass("btn-info").addClass("btn-danger"); // Tombol jadi merah jika ada remarks/send back
            //} else {
            //    $("#btnViewRemarks").removeClass("btn-danger").addClass("btn-info");
            //}
        }
    });
}

function checkLastStatus(rowData) {
    let model = {
        PlantCode: rowData.PlantCode, /*$("#PlantCode").val(),*/
        CourseCode: rowData.Course_Code,// $("#CourseCode").val(),
        GroupCode: rowData.Group,
        InspectionDate: formatDateGeneral($('#InspectionDateFrom').val()),
        Shift: $("#Frequency").val(),
        InspectionNo: rowData.InspectionNo
    };

    $.ajax({
        type: "POST",
        url: buildServiceUrl("service/inspectiondataupload/getlaststatus"),
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        xhrFields: { withCredentials: true },
        success: function (result) {
           
            let currentStatus = "";
            if (result.status && result.data && result.data.length > 0) {
                currentStatus = result.data[0].Status;
            } 
            if (currentStatus !== "") {
                //$("#lblStatus").show();
                $("#divstatus").show(); 
                $("#lblStatus").text(currentStatus);

                if (currentStatus.toUpperCase() === "SEND BACK") {
                    /*$("#lblStatus").css("color", "red");*/
                    $("#divstatus").removeClass("label-upload");
                    $("#divstatus").removeClass("label-check");
                    $("#divstatus").removeClass("label-approve");

                    $("#divstatus").addClass("label-sendback");
                    $(".btn-submit").prop("disabled", false);  
                } else { 
                    if (currentStatus === "APPROVED" || currentStatus === "1") {
                        $("#divstatus").removeClass("label-upload");
                        $("#divstatus").removeClass("label-check");
                        $("#divstatus").removeClass("label-sendback");

                        $("#divstatus").addClass("label-approve");

                        //                       $("#lblStatus").css("color", "green");
                    } else if (currentStatus === "CHECKED" || currentStatus === "1") {
                        $("#divstatus").removeClass("label-upload");
                        $("#divstatus").removeClass("label-sendback");
                        $("#divstatus").removeClass("label-approve");

                        $("#divstatus").addClass("label-check");
                        // $("#lblStatus").css("color", "black");
                    } else {
                        $("#divstatus").removeClass("label-sendback");
                        $("#divstatus").removeClass("label-check");
                        $("#divstatus").removeClass("label-approve");

                        $("#divstatus").addClass("label-upload");
                    }
                    $(".btn-submit").prop("disabled", true); 
                }
            } else {
                $("#divstatus").hide();
                $(".btn-submit").prop("disabled", true);  
            }
        }
    });
}
 
$(document).on("click", ".btn-open-modal", function (e) {
    e.preventDefault();
  
    let table = $('#tableID').DataTable();
    let rowData = table.row($(this).closest('tr')).data();

    if (!rowData) return;

    openDetailModal(rowData);
     
});
 
$(document).on("change", "#fileUpload-Detail", function () {
    const file = this.files[0];
    if (file) {
        $("#imagePath-Detail").val(file.name);
        const reader = new FileReader();
        reader.onload = function (e) {
            $("#imgPreview-Detail").attr('src', e.target.result).show();
        }
        reader.readAsDataURL(file);
    }
});
 
$(document).on("click", ".btn-save-detail", function () { 
    if ($("#InspectionResult-Detail").is(":not([readonly])") && $("#InspectionResult-Detail").val() === "") {
        toastr.warning("Please Fill Inspection Result Correctly", 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    let formData = new FormData();
     
    formData.append("InspectionCode", $("#InspectionCode-Detail-Hidden").val());
    formData.append("PlantCode", $("#PlantCode").val());
    formData.append("CourseCode", $("#CourseCode").val());
    formData.append("GroupCode", $("#GroupCode-Detail-Hidden").val());
    formData.append("InspectionNo", $("#InspectionNo-Detail-Hidden").val());
    formData.append("InspectionDate", formatDateGeneral($('#InspectionDateFrom').val()));
    formData.append("LastUser", 'admin'); // Dari global var

    // Tentukan nilai result berdasarkan metode
    if ($("#divOptionResult").is(":visible")) { 
        formData.append("InspectionResult", "0"); 
        formData.append("InspectionResultOption", $("#cboInspResultOption-Detail").val());
        formData.append("Type", "0");
    } else {
        formData.append("InspectionResult", $("#InspectionResult-Detail").val());
        formData.append("InspectionResultOption", "");
        formData.append("Type", "1");
    }

    // Append File Gambar
    let fileInput = $('#fileUpload-Detail')[0].files[0];
    if (fileInput) {
        formData.append("FileImage", fileInput);
    }

    $.ajax({
        url: buildServiceUrl("service/inspectiondataupload/update-detail"),
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.success) {
                toastr.success("Data successfully saved",'Success', { timeOut: 3000, "closeButton": true });
                $("#MyModal").modal("hide");
                table.ajax.reload();
            } else {
                toastr.warning(res.message, 'Warning', { timeOut: 3000, "closeButton": true });
            }
        }
    });
});
 
function fillResultOptions(unitCode, currentVal) {
  
    populateDDL({
        url: "service/inspectiondata/get-unitoption",
        dropdownId: "cboInspResultOption-Detail",
        model: {UnitCode:unitCode},
        codeField: "OptionValue",
        descField: "OptionName",
/*        width: "180px",*/
        selectedValue: currentVal,
        callback: function () {

            $("#cboInspResultOption-Detail").trigger('change');
            if (currentVal) {
                $("#cboInspResultOption-Detail").val(currentVal).trigger('change');
            }
        }
    });
}
 
function loadImageDetail(rowData) {


    let code = encodeURIComponent(rowData.Inspection_Code || "");
    let group = encodeURIComponent(rowData.Group || "");
    let no = encodeURIComponent(rowData.InspectionNo || "");
    let resultVal = encodeURIComponent(rowData.Inspection_Result || "");
     
    $.ajax({
        url: buildServiceUrl(`service/inspectiondataupload/get-image?code=${code}&group=${group}&no=${no}&result=${resultVal}`),
        type: "GET",
        xhrFields: { withCredentials: true },
        success: function (response) {
            
            if (response.status && response.data.FileImagePath != "") {
                
                let timestamp = new Date().getTime();
                 
                let finalImageUrl = response.data.FileImagePath + "?t=" + timestamp;
                $("#imagePath-Detail").val(response.data.FileImageName);
                $("#imgPreview-Detail")
                    .attr("src", finalImageUrl)
                    .show();
            } else {
             
                $("#imgPreview-Detail").show();
                //console.warn("Gambar tidak ada di DB.");
                $("#imgPreview-Detail")
                    .attr(
                        "src",
                        "/content/img/NotFound.jpg"
                );

                //$("#imgPreview-Detail").hide();
            }
        },
        error: function (err) {
            //console.error("Gagal memanggil API GetImage", err);
            $("#imgPreview-Detail").hide();
        }
    });
}
 
function checkModalSecurity(inspectionNo, rowData) {
    let model = {
        InspectionNo: inspectionNo,
        InspectionDate: formatDateGeneral($('#InspectionDateFrom').val()),
        CourseCode: $("#CourseCode").val(),
        GroupCode: rowData.Group,  
        Shift: $("#Frequency").val()
    };

    $.ajax({
        type: "POST",
        url: buildServiceUrl("service/inspectiondataupload/checkstatusleader"), 
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        xhrFields: { withCredentials: true },
        success: function (result) {   
            if (result.data.RetVal === "1") { 
                $(".btn-block[onclick*='fileUpload']").prop("disabled", true); 
                $(".btn-save-detail").prop("disabled", true); 
                $("#InspectionResult-Detail").prop("readonly", true).addClass("bg-light");
                $("#cboInspResultOption-Detail").prop("disabled", true); 
                toastr.info("This data is already checked / approved and cannot be modified.",'Info', { timeOut: 3000, "closeButton": true });
            } else { 
                $(".btn-block[onclick*='fileUpload']").prop("disabled", false);
                if (statusAdmin == "1") {
                    $(".btn-save-detail").prop("disabled", true);

                } else {
                    $(".btn-save-detail").prop("disabled", false); 

                }

                $("#cboInspResultOption-Detail").prop("disabled", false);
            }
        },
        error: function (err) {
            console.error("Security check failed", err); 
            $(".btn-save-detail").prop("disabled", true);
        }
    });
}

function clearTable() {
    let dt = $("#tableID").DataTable();
    dt.clear().draw(false);
    dt.columns.adjust();

}


function openDetailModal(rowData) {

    //let tr = $(this).closest('tr');
    //if (tr.hasClass('child')) {
    //    tr = tr.prev('.parent');
    //}
    //let rowData = table.row(tr).data();

    if (!rowData) return;

    $("#imagePath-Detail").val("");
    $("#fileUpload-Detail").val("");
    $("#InspectionResult-Detail").val("").prop("readonly", false).removeClass("bg-light");
    $("#cboInspResultOption-Detail").val("").prop("disabled", false);
    $(".btn-block[onclick*='fileUpload']").prop("disabled", false);
    //$(".btn-save-detail").prop("disabled", false);
    // ----------------------------------------------------------

    $("#InspectionCode-Detail-Hidden").val(rowData.Inspection_Code);
    $("#GroupCode-Detail-Hidden").val(rowData.Group);
    $("#InspectionNo-Detail-Hidden").val(rowData.InspectionNo);
    $("#UnitMethode-Detail-Hidden").val(rowData.Methode);

    $("#Course-Detail").val($("#CourseCode").val());
    $("#InspectionDate-Detail").val($('#InspectionDateFrom').val());
    //$("#FrequencyName-Detail").val($("#Frequency option:selected").text());
    $("#FrequencyName-Detail").val($("#Frequency").val());
    $("#Time-Detail").val(rowData.Time || "16:00");

    $("#Group-Detail").val(rowData.Description || rowData.Group);

    $("#InspectionPoint-Detail").val(rowData.Inspection_Point);
    //$("#MinRange-Detail").val(formatNumeric3Digit(rowData.Minimum));
    //$("#MaxRange-Detail").val(formatNumeric3Digit(rowData.Maximum));

    $("#MinRange-Detail").val(FormatInspectionResult(rowData.Minimum));
    $("#MaxRange-Detail").val(FormatInspectionResult(rowData.Maximum));

    $("#Unit-Detail").val(rowData.Unit);

    if (rowData.Methode === "01") {
        //option 
        $("#divOptionResult").show();
        $("#divlblOptionResult").show();

        $("#InspectionResult-Detail").prop("readonly", true).addClass("bg-light");
        fillResultOptions(rowData.unitCode, rowData.Inspection_Result);
    }
    else if (rowData.Methode === "02") {
        //numeric
        $("#divOptionResult").hide();
        $("#divlblOptionResult").hide();
        //$("#InspectionResult-Detail").val(formatNumeric3Digit(rowData.Inspection_Result));
        $("#InspectionResult-Detail").val(FormatInspectionResult(rowData.Inspection_Result));
    }

    loadImageDetail(rowData);
    checkModalSecurity(rowData.InspectionNo, rowData);
    $("#MyModal").modal("show");
}

function validateData() {

    if ($("#ClusterCode").val() == null || $("#ClusterCode").val() == "") {
        toastr.warning("Please select cluster code", 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    if ($("#PlantCode").val() == null || $("#PlantCode").val() == "") {
        toastr.warning("Please select plant code", 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    if ($("#CourseCode").val() == null || $("#CourseCode").val() == "") {
        toastr.warning("Please select course code", 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    if ($("#GroupCode").val() == null || $("#GroupCode").val() == "") {
        toastr.warning("Please select group code", 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    if ($("#Frequency").val() == null || $("#Frequency").val() == "") {
        toastr.warning("Please select frequency", 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    //const fromVal = $('#InspectionDateFrom').val();
    //const toVal = $('#InspectionDateTo').val();


    //// convert ke Date
    //const fromDate = new Date(fromVal);
    //const toDate = new Date(toVal);

    //// 🔴 Validasi 1: From > To
    //if (fromDate > toDate) {
    //    toastr.warning("Inspection Date From must less than date to", 'Warning', { timeOut: 3000, "closeButton": true });
    //    return false;
    //}

    return true;
}

//show format numeric only 3 digit after comma
function formatNumeric3Digit(data) {
    if (data == null || data === "") return "";

    if (!isNaN(data) && data.toString().trim() !== "") {
        return parseFloat(data)
            .toFixed(3)
            .replace(/\.?0+$/, "");
    }

    return data;
}



////3 digit decimal place
//$("#InspectionResult-Detail").on("input", function () {
//    let value = this.value;
//    console.log(value);

//    // Hanya angka dan titik
//    value = value.replace(/[^0-9.]/g, "");

//    // Hanya boleh ada 1 titik
//    value = value.replace(/(\..*)\./g, "$1");

//    // Maksimal 3 digit setelah titik
//    value = value.replace(/(\.\d{3})\d+/g, "$1");

//    this.value = value;
//});


$("#InspectionResult-Detail").on("input", function () {
    let value = this.value;

    // Hanya angka dan titik
    value = value.replace(/[^0-9.]/g, "");

    // Hanya boleh ada 1 titik
    value = value.replace(/(\..*)\./g, "$1");

    let parts = value.split(".");
    let integerPart = parts[0];
    let decimalPart = parts.length > 1 ? parts[1] : "";

    // Bilangan bulat 4 digit atau lebih: tidak dibatasi desimal
    if (integerPart.length >= 4) {
        this.value = integerPart;
        return;
    }

    // Maksimal digit desimal = 4 dikurangi digit angka utama
    let maxDecimal = Math.max(0, 4 - integerPart.length);

    decimalPart = decimalPart.substring(0, maxDecimal);

    this.value = parts.length > 1
        ? integerPart + "." + decimalPart
        : integerPart;
});
