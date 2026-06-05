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
        width: "180px",
        callback: function () {
            if (courseCode == "" && ($("#PlantCode").val() != null && $("#PlantCode").val() != "")) {
                let ddl = $("#CourseCode");

                let code = "ALL";
                let desc = "ALL";

                ddl.prepend(
                    '<option value="' + code + '" ' +
                    'data-content="<div style=\'display:flex;width:300px\'>' +
                    '<span style=\'width:80px;text-align:left\'>' + code + '</span>' +
                    '<span style=\'padding-left:10px; text-align:left;\'>' + desc + '</span>' +
                    '</div>" ' +
                    'data-code="' + code + '" ' +
                    'data-desc="' + desc + '">' +       
                    code + ' - ' + desc +
                    '</option>'
                );
                ddl.trigger('change');

               
                //ddl.selectpicker('refresh');

            }
            if (statusAdmin == "1" && (courseCode != "" && courseCode != null) ) {
                $("#CourseCode").prop("disabled", true).trigger('change.select2');
            } else {
                $("#CourseCode").prop("disabled", false).trigger('change.select2');

            }

        }
    });

    debugger
    if (!isRedirect) {
        clearTable()
    }
});

//course
$(document).on("change", "#CourseCode", function () {
    let desc = $("#CourseCode option:selected").data("desc");
    $("#CourseName").val((desc || "").trim());
    
    if (!isRedirect) {
        clearTable()
    } else {
        setTimeout(function () {
            $(".btn-search").trigger("click");
        }, 200); // delay 0.2 detik
    }
});

$(document).on("change", "#Frequency,#InspectionDateFrom,#InspectionDateTo", function () {
    if (!isRedirect) {
        clearTable()
    }

});


//check
//$(document).on("change", ".chk-select", function () {

//    if ($(this).is(":checked")) {

//        // hanya 1 yang boleh dipilih
//        $(".chk-select").not(this).prop("checked", false);

//    }
//});
$(document).on('click', '.datepicker-icon', function () {
    $(this).siblings('.datepicker').datepicker('show').focus();
});

//search data
//$(document).on("click", ".btn-search", function () {
//   // waitingDialog.show("Please wait retrieving data...");

//    if (table) {
//        table.ajax.reload(null, false);
//        isRedirect = false;
//    } else {
//        loadData();
//    }

//});

$(document).on("click", ".btn-search", function () {
    // waitingDialog.show("Please wait retrieving data...");

    if (table) {
        table.ajax.reload(null, false);
        isRedirect = false;
    } else {
        loadData();
    }

});

$(document).on("click", ".btn-clear", function () {
    // reset 

    $("#PlantCode").val('').trigger('change');
    $("#PlantName").val("");
    $("#CourseCode").val('').trigger('change');
    $("#CourseName").val("");
    $("#Frequency").val('').trigger('change');
    let now = new Date();
    let firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
    let today = new Date();

    $("#InspectionDateFrom").datepicker('setDate', firstDay);
    $("#InspectionDateTo").datepicker('setDate', today);

    $("#tableID").dataTable().fnClearTable();
});

//SUBMIT APPROVE BATCH
$(document).on("click", ".btn-submit", function () {


    let tableIDList= $('#tableID').DataTable();
    let listApprove = [];

    $('#tableID').DataTable().rows().every(function () {
        let row = this.node();
        let data = this.data();

        let IsApprove = $(row).find('.chk-spv').is(':checked');

        // 🔥 hanya push kalau ada yg dicentang
        if (IsApprove) {
            listApprove.push({
                InspectionNo: data.InspectionNo,
                CourseCode: data.CourseCode,
                GroupCode: data.GroupCode,
                InspectionDate: data.InspectionDate,
                Frequency: data.Frequency
            });
        }
    });

    // 🔥 VALIDASI: tidak ada yg dicentang
    if (listApprove.length === 0) {
        toastr.info("Please select data to approve", 'Info', { timeOut: 3000, "closeButton": true });
        return;
    }

    // 🔥 final model
    let model = {
        ApproveBatch: listApprove
    };

    Swal.fire({
        title: 'Confirmation',
        text: "Are you sure you want to approve this inspection?",
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                type: "POST",
                //url: BaseUrl + "service/inspectiondataapproval/approve-batch",
                url: buildServiceUrl("service/inspectiondataapproval/approve-batch"),
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
                        table.ajax.reload(null, false);
                    }

                }
            })
        }
    });
    //}
});


//view detail
$(document).on('click', '.btn-open-modal', function (e) {
    e.preventDefault();

    let table = $('#tableID').DataTable();
    let rowData = table.row($(this).closest('tr')).data();

    if (!rowData) return;

    openDetailModal(rowData);
});

$(document).on('dblclick', '#tableID tbody tr', function () {
    let table = $('#tableID').DataTable();
    let rowData = table.row(this).data();

    if (!rowData) return;

    openDetailModal(rowData);
});



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
            model.GroupCode = $("#GroupCodeModal").val();
            model.InspectionDate = formatDateGeneral($("#InspectionDate").val());
            model.Frequency = $("#FrequencyName").val();


            $.ajax({
                //url: BaseUrl + "service/inspectiondataapproval/approve-detail",
                url: buildServiceUrl("service/inspectiondataapproval/approve-detail"),
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


//$('#tableDetail').on('shown.bs.modal', function () {

//    var tableDet = $('#tableDetail').DataTable();
//    setTimeout(() => {
//        tableDet.columns.adjust().draw();
//    }, 200);



//});



//check all access
$(document).on("change", "#chk-all-spv", function () {
    let checked = $(this).is(":checked");

    $('#tableID').DataTable().rows({ search: 'applied' }).nodes().to$()
        .find('.chk-spv')
        .prop("checked", checked);
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

    //DDL Cluster
    populateDDL({
        url: "service/inspectiondata/get-cluster",
        dropdownId: "ClusterCode",
        codeField: "Code",
        descField: "Description",
        descTextbox: "ClusterName",
        selectedValue: clusterCode,
        width: "180px",
        callback: function () {

            if (statusAdmin == "1" && (clusterCode != "" && clusterCode != null)) {
                $("#ClusterCode").prop("disabled", true).trigger('change.select2');
            } 
            populateDDL({
                url: "service/inspectiondata/get-plant",
                dropdownId: "PlantCode",
                model: {
                    ClusterCode: $("#ClusterCode").val()
                },
                codeField: "Code",
                descField: "Description",
                descTextbox: "PlantName",
                width: "180px",
                selectedValue:plantCode,
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

    //DDL Frequency
    populateDDL({
        url: "service/inspectiondata/get-frequency",
        dropdownId: "Frequency",
        codeField: "Code",
        descField: "Description",
        width: "180px",
        selectedValue: frequency,
        callback: function () {
            if (frequency == "") {
                let ddl = $("#Frequency");

                let code = "ALL";
                let desc = "ALL";


                ddl.prepend(
                    '<option value="' + code + '" ' +
                    'data-content="<div style=\'display:flex;width:300px\'>' +
                    '<span style=\'width:100px;text-align:left\'>' + code + '</span>' +
                    '<span style=\'padding-left:30px; text-align:left;\'>' + desc + '</span>' +
                    '</div>" ' +
                    'data-code="' + code + '" ' +
                    'data-desc="' + desc + '">' +
                    code +
                    '</option>'
                );

                ddl.trigger('change');
            }


            if (statusAdmin == "1" && (frequency != "" && frequency != null) ) {
                $("#Frequency").prop("disabled", true).trigger('change.select2');
            } else {
                $("#Frequency").prop("disabled", false).trigger('change.select2');

            }
        }
    });


    if (inspectionDate != "" && frequency != "") {
        //setTimeout(function () {
        //    $(".btn-search").trigger("click");
        //}, 200); // delay 0.2 detik
        isRedirect = true;

        if (statusAdmin == "1") {

            $("#InspectionDateFrom").prop("disabled", true);
            $("#InspectionDateTo").prop("disabled", true);
            $(".btn-submit").prop("disabled", true);
            $(".btn-popupsendback").prop("disabled", true);
            $(".btn-check-approve").prop("disabled", true);
            $(".btn-check").prop("disabled", true);
            $(".btn-submit-sendback").prop("disabled", true);

        } else {
            $("#InspectionDateFrom").prop("disabled", false);
            $("#InspectionDateTo").prop("disabled", false);
            $(".btn-submit").prop("disabled", false);
            $(".btn-popupsendback").prop("disabled", false);
            $(".btn-check-approve").prop("disabled", false);
            $(".btn-check").prop("disabled", false);
            $(".btn-submit-sendback").prop("disabled", false);
        }
    } else {
        if (statusAdmin == "1") {

            $(".btn-submit").prop("disabled", true);
            $(".btn-popupsendback").prop("disabled", true);
            $(".btn-check-approve").prop("disabled", true);
            $(".btn-check").prop("disabled", true);
            $(".btn-submit-sendback").prop("disabled", true);
            $(".btn-approve").prop("disabled", true);

        } else {
            $(".btn-submit").prop("disabled", false);
            $(".btn-popupsendback").prop("disabled", false);
            $(".btn-check-approve").prop("disabled", false);
            $(".btn-check").prop("disabled", false);
            $(".btn-submit-sendback").prop("disabled", false);
            $(".btn-approve").prop("disabled", false);

        }
    }

        table = $("#tableID").DataTable({
            destroy: true,
            autoWidth: false,
            autoWidth: false,
            searching: true,
            ordering: true,
            paging: true,
            info: true,
            bSort: true,
            bLengthChange: true,
            pageLength: 25,
            scrollX: true,
            scrollY: "calc(42vh)",
            processing: false,
            serverSide: false,
            responsive: false,
            fixedColumns: {
                leftColumns: 5
            },
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
            //kalo warnanya dibackground data
            //rowCallback: function (row, data, index) {
            //    if (data.Status && data.Status.toLowerCase() === "abnormal") {
            //        $(row).addClass("row-abnormal");
            //    }
            //}
            rowCallback: function (row, data, index) {
                if (data.Status &&
                    data.Status.toLowerCase() === "abnormal") {

                    $(row).addClass("text-abnormal");
                }
            },
            ajax: {
                //url: BaseUrl + "service/inspectiondataapproval/list",
                url: buildServiceUrl("service/inspectiondataapproval/list"),
                type: "POST",
                contentType: "application/json",
                xhrFields: {
                    withCredentials: true
                },
                data: function (d) {

                    let cluster = isRedirect ? clusterCode : $("#ClusterCode").val();
                    let plant = isRedirect ? plantCode : $("#PlantCode").val();
                    let course = isRedirect ? courseCode : $("#CourseCode").val();
                    let freq = isRedirect ? frequency : $("#Frequency").val();
                    let inspDate = isRedirect ? inspectionDate : $('#InspectionDateFrom').val();
                    let inspDateTo = isRedirect ? inspectionDate : $('#InspectionDateTo').val();

                    return JSON.stringify({
                        ClusterCode: cluster,
                        PlantCode: plant,
                        CourseCode: course,
                        Frequency: freq,
                        InspectionDateFrom: formatDateGeneral(inspDate),
                        InspectionDateTo: formatDateGeneral(inspDateTo)
                    });
                },

                dataSrc: function (json) {

                    let data = json.data || [];

                    if (!isRedirect && data.length === 0 && ($("#PlantCode").val() != null && $("#PlantCode").val() != "")) {

                        toastr.info("No data available in table", 'Info', {
                            timeOut: 3000,
                            closeButton: true
                        });
                    }

                    return data;
                },
                complete: function () {
                    //setInterval(
                    //    waitingDialog.hide(), 500);
                }, error: function () {
                    // waitingDialog.hide();
                }
            },
            columns: [
                //jika mau tetep pake checkbox view detail
                //{
                //    data: null,
                //    className: "text-center", "autoWidth": true,
                //    orderable: false,
                //    render: function (data, type, row) {
                //        return ` <input type="checkbox" class="chk-select" name="groupSelect" value="${row.CourseCode}" data-inspectdate="${row.InspectionDate || ''}">`;
                //    }
                //},
                { data: "PlantCode", "autoWidth": false, className: "text-center", },
                {
                    data: "InspectionDate", "autoWidth": false,
                    render: function (data) {
                        return formatDate(data);

                    },
                },
                { data: "Frequency", "autoWidth": false },
                { data: "CourseCode", "autoWidth": false },
                {
                    data: "DocumentNo",
                    autoWidth: false,
                    render: function (data, type, row) {

                        if (type === "display") {
                            return `<a href="#" class="btn-open-modal"
                        data-doc="${data}"
                        data-course="${row.CourseCode}"
                        data-title="${row.Title}"
                        data-inspectdate="${formatDate(row.InspectionDate)}"
                        data-frequency="${row.Frequency}"
                        data-inspectionno="${row.InspectionNo}"
                        >
                        ${data}
                    </a>`;
                        }

                        return data;
                    }
                },
                {
                    data: "InspectorStatus",
                    className: "text-center",
                    orderable: false,
                    render: function (data, type, row) {
                        return ` <input type="checkbox" class="chk-insp" name="groupSelect" value="${row.InspectorStatus}" ${row.InspectorStatus == "1" ? "checked" : ""} disabled data-inspector="${row.InspectorStatus || ''}">`;
                    }
                },
                { data: "InspectorUser", "autoWidth": false },
                {
                    data: "InspectorDate", "autoWidth": false,
                    render: function (data) {
                        return formatDateTime(data);

                    },
                },
                {
                    data: "LeaderStatus",
                    className: "text-center",
                    orderable: false,
                    render: function (data, type, row) {
                        return ` <input type="checkbox" class="chk-lead" name="groupSelect" value="${row.LeaderStatus}" ${row.LeaderStatus == "1" ? "checked" : ""} disabled data-leader="${row.LeaderStatus || ''}">`;
                    }
                },
                { data: "LeaderUser", "autoWidth": false },
                {
                    data: "LeaderDate", "autoWidth": false,
                    render: function (data) {
                        return formatDateTime(data);

                    },
                },

                {
                    data: "SupervisorStatus",
                    className: "text-center",
                    title: `<input type="checkbox" id="chk-all-spv"> Approve`,
                    orderable: false,
                    render: function (data, type, row) {
                        return ` <input type="checkbox" class="chk-spv" name="groupSelect" value="${row.SupervisorStatus}" data-spv="${row.SupervisorStatus || ''}">`;
                    }
                },
                { data: "SupervisorUser", "autoWidth": false },
                {
                    data: "SupervisorDate", "autoWidth": false,
                    render: function (data) {
                        return formatDateTime(data);

                    },
                },



            ],
        });

}

//function loadData() {

//    isRedirect = false;
//    if (table) {
//        table.ajax.reload(null, false);
//        return;
//    }
//    table = $("#tableID").DataTable({
//        destroy: true,
//        autoWidth: false,
//        ajax: {
//            //url: BaseUrl + "service/inspectiondataapproval/list",
//            url: buildServiceUrl("service/inspectiondataapproval/list"),
//            type: "POST",
//            contentType: "application/json",
//            xhrFields: {
//                withCredentials: true
//            },
//            data: function (d) {
//                return JSON.stringify({
//                    ClusterCode: (clusterCode != "") ? clusterCode : $("#ClusterCode").val(),
//                    PlantCode: (plantCode != "") ? plantCode : $("#PlantCode").val(),
//                    CourseCode: (courseCode != "") ? courseCode : $("#CourseCode").val(),
//                    Frequency: (frequency != "") ? frequency : $("#Frequency").val(),
//                    InspectionDateFrom: (inspectionDate != "") ? inspectionDate : formatDateGeneral($('#InspectionDateFrom').val()),
//                    InspectionDateTo: (inspectionDate != "") ? inspectionDate : formatDateGeneral($('#InspectionDateTo').val())
//                });

//            },
//            dataSrc: function (json) {
//                return json.data; // atau json kalau array langsung
//            },
//            complete: function () {
//                //setInterval(
//                //    waitingDialog.hide(), 500);
//            }, error: function () {
//               // waitingDialog.hide();
//            }
//        },

//        columns: [
//            //jika mau tetep pake checkbox view detail
//            //{
//            //    data: null,
//            //    className: "text-center", "autoWidth": true,
//            //    orderable: false,
//            //    render: function (data, type, row) {
//            //        return ` <input type="checkbox" class="chk-select" name="groupSelect" value="${row.CourseCode}" data-inspectdate="${row.InspectionDate || ''}">`;
//            //    }
//            //},
//            { data: "PlantCode", "autoWidth": false, className: "text-center", },
//            {
//                data: "InspectionDate", "autoWidth": false,
//                render: function (data) {
//                    return formatDate(data);

//                },
//            },
//            { data: "Frequency", "autoWidth": false },
//            { data: "CourseCode", "autoWidth": false },
//            {
//                data: "DocumentNo",
//                autoWidth: false,
//                render: function (data, type, row) {

//                    if (type === "display") {
//                        return `<a href="#" class="btn-open-modal"
//                        data-doc="${data}"
//                        data-course="${row.CourseCode}"
//                        data-title="${row.Title}"
//                        data-inspectdate="${formatDate(row.InspectionDate)}"
//                        data-frequency="${row.Frequency}"
//                        data-inspectionno="${row.InspectionNo}"
//                        >
//                        ${data}
//                    </a>`;
//                    }

//                    return data;
//                }
//            },
//            {
//                data: "InspectorStatus",
//                className: "text-center",
//                orderable: false,
//                render: function (data, type, row) {
//                    return ` <input type="checkbox" class="chk-insp" name="groupSelect" value="${row.InspectorStatus}" ${row.InspectorStatus == "1" ? "checked" : ""} disabled data-inspector="${row.InspectorStatus || ''}">`;
//                }
//            },
//            { data: "InspectorUser", "autoWidth": false },
//            {
//                data: "InspectorDate", "autoWidth": false,
//                render: function (data) {
//                    return formatDateTime(data);

//                },
//            },
//            {
//                data: "LeaderStatus",
//                className: "text-center",
//                orderable: false,
//                render: function (data, type, row) {
//                    return ` <input type="checkbox" class="chk-lead" name="groupSelect" value="${row.LeaderStatus}" ${row.LeaderStatus == "1" ? "checked" : ""} disabled data-leader="${row.LeaderStatus || ''}">`;
//                }
//            },
//            { data: "LeaderUser", "autoWidth": false },
//            {
//                data: "LeaderDate", "autoWidth": false,
//                render: function (data) {
//                    return formatDateTime(data);

//                },
//            },

//            {
//                data: "SupervisorStatus",
//                className: "text-center",
//                title: `<input type="checkbox" id="chk-all-spv"> Approve`,
//                orderable: false,
//                render: function (data, type, row) {
//                    return ` <input type="checkbox" class="chk-spv" name="groupSelect" value="${row.SupervisorStatus}" data-spv="${row.SupervisorStatus || ''}">`;
//                }
//            },
//            { data: "SupervisorUser", "autoWidth": false },
//            {
//                data: "SupervisorDate", "autoWidth": false,
//                render: function (data) {
//                    return formatDateTime(data);

//                },
//            },



//        ],
//        autoWidth: false,
//        searching: true,
//        ordering: true,
//        paging: true,
//        info: true,
//        bSort: true,
//        bLengthChange: true,
//        pageLength: 25,
//        scrollX: true,
//        scrollY: "calc(42vh)",
//        processing: false,
//        serverSide: false,
//        responsive: false,
//        fixedColumns: {
//            leftColumns: 5
//        },
//        "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "ALL"]],
//        language: {
//            lengthMenu: "Show _MENU_"
//        },
//        dom:
//            "rt" +
//            "<'row mt-2 align-items-center'\
//            <'col-sm-6'i>\
//            <'col-sm-6 d-flex justify-content-end align-items-center gap-3'lp>\ >"
//        ,
//        //kalo warnanya dibackground data
//        //rowCallback: function (row, data, index) {
//        //    if (data.Status && data.Status.toLowerCase() === "abnormal") {
//        //        $(row).addClass("row-abnormal");
//        //    }
//        //}
//        rowCallback: function (row, data, index) {
//            if (data.Status &&
//                data.Status.toLowerCase() === "abnormal") {

//                $(row).addClass("text-abnormal");
//            }
//        },

//    });


//    //new $.fn.dataTable.FixedColumns(table, {
//    //    leftColumns: 5
//    //});

//}

function loadDetail() {
    if (tableDetail) {
        tableDetail.ajax.reload(null, false);
        return;
    }

    tableDetail = $("#tableID-Detail").DataTable({
        destroy: true,
        autoWidth:false,
        ajax: {
            //url: BaseUrl + "service/inspectiondataapproval/list-detail",
            url: buildServiceUrl("service/inspectiondataapproval/list-detail"),
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
                    Frequency: $("#FrequencyName").val()

                });
            },
            dataSrc: function (json) {
                return json.data; // atau json kalau array langsung
                

            }
        },
        columns: [
            {
                data: null, className: "v-align", "autoWidth": false,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: "Group", "autoWidth": false },
            { data: "InspectionPoint", "autoWidth": false },
            { data: "Unit", "autoWidth": false },
            { data: "Minimum", "autoWidth": false },
            { data: "Maximum", "autoWidth": false },
            { data: "InspectionResult", "autoWidth": false },
            { data: "Status", "autoWidth": false },
            { data: "Remarks", "autoWidth": false }


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
        scrollY: "calc(40vh)",
        processing: false,
        serverSide: false,
        responsive: false,
        fixedColumns: {
            leftColumns: 3
        },
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
        //        $(row).addClass("row-abnormal");
        //    }
        //},
        rowCallback: function (row, data, index) {
            if (data.Status &&
                data.Status.toLowerCase() === "abnormal") {

                $(row).addClass("text-abnormal");
            }
        },

    });
    //new $.fn.dataTable.FixedColumns(tableDetail, {
    //    leftColumns: 3
    //});
}

function openDetailModal(data) {
    let docNo = $(this).data("doc");
    let course = $(this).data("course");
    let title = $(this).data("title");
    let inspecdate = $(this).data("inspectdate");
    let frequency = $(this).data("frequency");
    let group = $(this).data("group");
    let inspectionno = $(this).data("inspectionno");

    $("#Course").val(data.CourseCode || '');
    $("#Title").val(data.Title || '');
    $("#DocumentNo").val(data.DocumentNo || '');
    $("#InspectionDate").val(formatDate(data.InspectionDate));
    $("#FrequencyName").val(data.Frequency || '');
    $("#GroupCodeModal").val(data.GroupCode || '');
    $("#InspectionNo").val(data.InspectionNo || '');

    loadDetail();
    setTimeout(() => {
        tableDetail.columns.adjust().draw();
    }, 200);
    $("#MyModal").modal("show");
}

//function isFilterComplete() {

//    return clusterCode &&
//        plantCode &&
//        courseCode &&
//        frequency &&
//        inspectionDate;
//}

function clearTable() {
//    $("#tableID").DataTable().rows().remove().draw(false);

    let dt = $("#tableID").DataTable();
    dt.clear().draw(false);
    dt.columns.adjust();

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

    if ($("#Frequency").val() == null || $("#Frequency").val() == "") {
        toastr.warning("Please select frequency", 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    const fromVal = $('#InspectionDateFrom').val();
    const toVal = $('#InspectionDateTo').val();


    // convert ke Date
    const fromDate = new Date(fromVal);
    const toDate = new Date(toVal);

    // 🔴 Validasi 1: From > To
    if (fromDate > toDate) {
        toastr.warning("Inspection Date From must less than date to", 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    return true;
}