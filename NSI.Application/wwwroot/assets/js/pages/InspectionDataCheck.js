var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;

var tableDetail;
let isRedirect = false;


init();

setTimeout(checkStatusAdmin(), 100);

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
                // Default pilih ALL
                ddl.val("ALL");
                ddl.trigger('change');
                //ddl.selectpicker('refresh');

               

            }
            $("#CourseCode").trigger('change');

            if (statusAdmin == "1" && (courseCode != "" && courseCode != null)) {
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

            if ($("#CourseCode").val() != null) {
                let ddl = $("#GroupCode");

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
                ddl.val("ALL");
                ddl.trigger('change');

            }
            $("#GroupCode").trigger('change');
            if (statusAdmin == "1" && (groupCode != "" && groupCode != null)) {
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
        setTimeout(function () {
            $(".btn-search").trigger("click");
        }, 200); // delay 0.2 detik
    }


});

//
$(document).on("change", "#Frequency,#InspectionDateFrom", function () {
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


//search data
$(document).on("click", ".btn-search", function () {
    if (!validateData()) return;


    if (table) {
        table.ajax.reload(null, false);
        isRedirect = false;
    } else {
        loadData();
    }


    //table.ajax.reload(null, false);

    //isRedirect = false;

    //if (!validateData()) return;

    ////loadData();
    //if (isRedirect) {
    //    loadData(clusterCode, plantCode, courseCode, groupCode, frequency, inspectionDate)
    //} else {
    //    loadData($("#ClusterCode").val(), $("#PlantCode").val(), $("#CourseCode").val(), $("#GroupCode").val(), $("#Frequency").val(), $("#InspectionDateFrom").val());
    //}
    //isRedirect = false;
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
    let today = new Date();

    $("#InspectionDateFrom").datepicker('setDate', today);
    //GlobalDatepicker.setDefault("#InspectionDateTo", 2);

    clearTable();

});

//SUBMIT APPROVE LEADER BATCH
$(document).on("click", ".btn-submit", function () {


    let tableIDList = $('#tableID').DataTable();
    let listApprove = [];

    $('#tableID').DataTable().rows().every(function () {
        let row = this.node();
        let data = this.data();

        let IsApprove = $(row).find('.chk-leader').is(':checked');
        let IsNormal = data.Status && data.Status.toLowerCase() === "normal";

        if (IsApprove && IsNormal) {
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
        toastr.info("Please select to submit data", 'Info', { timeOut: 3000, "closeButton": true });
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
                url: buildServiceUrl("service/inspectiondatacheck/approve-batch"),
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
                        // Uncheck Check All Leader
                        $('#chk-all-leader').prop('checked', false);
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

$(document).on("click", ".btn-check-approve", function () {

    Swal.fire({
        title: 'Are you sure check this inspection?',
        text: "Data will be disapear after submit!",
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
            model.Remarks = $("#RemarksLeader").val(); //inputan notes dari leader kalo approve bukan send back
            model.RemarksSendBack = ""; //ini untuk sendback to inspector
            model.Type = "1"; //approve 


            $.ajax({
                url: buildServiceUrl("service/inspectiondatacheck/approve-detail"),
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

$(document).on("click", ".btn-submit-sendback", function () {

    Swal.fire({
        title: 'Are you sure check this inspection?',
        text: "Data will be disapear after submit!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, sendback it!'
    }).then((result) => {

        if (result.isConfirmed) {
            var model = new Object();

            model.InspectionNo = $("#InspectionNo").val();
            model.CourseCode = $("#Course").val();
            model.GroupCode = $("#GroupCodeModal").val();
            model.InspectionDate = formatDateGeneral($("#InspectionDate").val());
            model.Frequency = $("#FrequencyName").val();
            model.Remarks = "";
            model.RemarksSendBack = $("#RemarksSendBack").val(); //ini untuk sendback to inspector
            model.Type = "0"; //sendback

            $.ajax({
                url: buildServiceUrl("service/inspectiondatacheck/approve-detail"),
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
                        $('body').removeClass('modal-open');
                        $('.modal-backdrop').remove();
                        $('#MyModal').modal('hide');
                        $('#modalSendBack').modal('hide');
                        table.ajax.reload();
                    }
                },

            });

        }
    });

});

$(document).on("click", ".btn-popupsendback", function (e) {
    e.preventDefault();
    $("#RemarksSendBack").val('');

    //$("#MyModal").modal("hide");
    $("#modalSendBack").modal("show");

});

//check all access
$(document).on("change", "#chk-all-leader", function () {
    //let checked = $(this).is(":checked");

    //$('#tableID').DataTable().rows({ search: 'applied' }).nodes().to$()
    //    .find('.chk-leader')
    //    .prop("checked", checked);

    let isChecked = $(this).is(":checked");
    let table = $('#tableID').DataTable();

    table.rows().every(function () {

        let row = this.node();
        let data = this.data();

        let isNormal = data.Status &&
            data.Status.toLowerCase() === "normal";

        let checkbox = $(row).find('.chk-leader');

        if (isNormal) {
            checkbox.prop("checked", isChecked);
        }
        else {
            // Abnormal selalu tidak boleh dicentang
            checkbox.prop("checked", false);
        }
    });
});

function init() {

    GlobalDatepicker.init(".datepicker");
    if (inspectionDate != "") {
        const parsed = new Date(inspectionDate);
        $("#InspectionDateFrom").datepicker('setDate', formatDate(parsed));
        //$("#InspectionDateTo").datepicker('setDate', parsed);

        $(".btn-back").removeClass("hidden");
        $(".btn-back").addClass("show");
    } else {
        let now = new Date();
        let firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
        let today = new Date();
        $("#InspectionDateFrom").datepicker('setDate', firstDay);
        // $("#InspectionDateTo").datepicker('setDate', today);
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
        /*width: "180px",*/
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
                selectedValue: plantCode,
                callback: function () {
                    $("#PlantCode").trigger('change');

                    if (statusAdmin == "1" && (plantCode != "" && plantCode != null)) {
                        $("#PlantCode").prop("disabled", true).trigger('change.select2');
                    } else {
                        $("#PlantCode").prop("disabled", false).trigger('change.select2');

                    }
                }
            });


        }
    });

    // $("#ClusterCode").val(clusterCode).trigger('change.select2');

    //DDL Frequency
    populateDDL({
        url: "service/inspectiondata/get-frequency",
        dropdownId: "Frequency",
        codeField: "Code",
        descField: "Description",
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
                ddl.val("ALL");
                ddl.trigger('change');
            }

            if (statusAdmin == "1" && (frequency != "" && frequency != null)) {
                $("#Frequency").prop("disabled", true).trigger('change.select2');
            } else {
                $("#Frequency").prop("disabled", false).trigger('change.select2');

            }
        }
    })


    //table
    table = $("#tableID").DataTable({
        autoWidth: false,
        //processing: true,
        //deferRender: true,
        searching: true,
        ordering: true,
        paging: true,
        info: true,
        bLengthChange: true,
        pageLength: 25,
        scrollX: true,
        scrollY: "calc(42vh)",
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
        //}
        //drawCallback: function (settings) {
        //    debugger
        //    let api = this.api();
        //    let data = api.rows().data();

        //    if (isFirstLoad) return;

        //    if (data.length === 0) {

        //        toastr.info("No data available in table", 'Info', {
        //            timeOut: 3000,
        //            closeButton: true
        //        });
        //    }
        //    isFirstLoad = false;
        //},
        rowCallback: function (row, data, index) {
            //if (data.Status &&
            //    data.Status.toLowerCase() === "abnormal") {

            //    $(row).addClass("text-abnormal");
            //}

            let isAbnormal = data.Status &&
                data.Status.toLowerCase() === "abnormal";

            if (isAbnormal) {
                $(row).addClass("text-abnormal");

                // Abnormal tidak boleh approve
                $(row).find('.chk-leader')
                    .prop('checked', false)
                    .prop('disabled', true);
            }
            else {
                $(row).find('.chk-leader')
                    .prop('disabled', false);
            }
        },
        ajax: {
            url: buildServiceUrl("service/inspectiondatacheck/list"),
            type: "POST",
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },

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
                    Frequency: freq,
                    InspectionDateFrom: formatDateGeneral(inspDate),
                    InspectionDateTo: formatDateGeneral(inspDate)
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
            //{
            //    data: "InspectionDate", "autoWidth": false,
            //    render: function (data) {
            //        return formatDate(data);

            //    },
            //},
            { data: "Frequency", "autoWidth": false },
            { data: "CourseCode", "autoWidth": false },
            {
                data: "GroupName",
                autoWidth: false,
                render: function (data, type, row) {

                    if (type === "display") {
                        return `<a href="#" class="btn-open-modal"
                        data-groupcode="${row.GroupCode}"
                        data-groupname="${data}"
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
                title: `<input type="checkbox" id="chk-all-leader"> Check`,
                orderable: false,
                render: function (data, type, row) {
                    return ` <input type="checkbox" class="chk-leader" name="groupSelect" value="${row.LeaderStatus}" ${row.LeaderStatus == "1" ? "checked" : ""}  data-leader="${row.LeaderStatus || ''}">`;
                }
            },
            { data: "LeaderUser", "autoWidth": false },
            {
                data: "LeaderDate", "autoWidth": false,
                render: function (data) {
                    return formatDateTime(data);

                },
            },

        ],

    });

}

//function loadData(pClustr, pPlant, pCourse, pGroup, pShift, pDateFrom) {

//    table = $("#tableID").DataTable({
//        destroy: true,
//        autoWidth: false,
//        ajax: {
//            url: buildServiceUrl("service/inspectiondatacheck/list"),
//            type: "POST",
//            contentType: "application/json",
//            xhrFields: {
//                withCredentials: true
//            },
//            data: function (d) {

//                return JSON.stringify({
//                    ClusterCode: pClustr,
//                    PlantCode: pPlant,
//                    CourseCode: pCourse,
//                    GroupCode: pGroup,
//                    Frequency: pShift,
//                    InspectionDateFrom: formatDateGeneral(pDateFrom),// (inspectionDate != "") ? inspectionDate : formatDateGeneral($('#InspectionDateFrom').val()),
//                    InspectionDateTo: formatDateGeneral(pDateFrom) //(inspectionDate != "") ? inspectionDate : formatDateGeneral($('#InspectionDateFrom').val()), //parse hanya 1 insp date aja 

//                });
//            },
//            dataSrc: function (json) {

//                return json.data; // atau json kalau array langsung

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
//            //{
//            //    data: "InspectionDate", "autoWidth": false,
//            //    render: function (data) {
//            //        return formatDate(data);

//            //    },
//            //},
//            { data: "Frequency", "autoWidth": false },
//            { data: "CourseCode", "autoWidth": false },
//            {
//                data: "GroupName",
//                autoWidth: false,
//                render: function (data, type, row) {

//                    if (type === "display") {
//                        return `<a href="#" class="btn-open-modal"
//                        data-groupcode="${row.GroupCode}"
//                        data-groupname="${data}"
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
//                title: `<input type="checkbox" id="chk-all-leader"> Check`,
//                orderable: false,
//                render: function (data, type, row) {
//                    return ` <input type="checkbox" class="chk-leader" name="groupSelect" value="${row.LeaderStatus}" ${row.LeaderStatus == "1" ? "checked" : ""}  data-leader="${row.LeaderStatus || ''}">`;
//                }
//            },
//            { data: "LeaderUser", "autoWidth": false },
//            {
//                data: "LeaderDate", "autoWidth": false,
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
//        processing: true,
//        deferRender: true,
//        serverSide: false,
//        responsive: false,
//        fixedColumns: {
//            leftColumns: 3
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
//    //    leftColumns: 4
//    //});


//    console.log(isRedirect);

//}

function loadDetail() {
    if (tableDetail) {
        tableDetail.ajax.reload(null, false);
        return;
    }
    tableDetail = $("#tableID-Detail").DataTable({
        destroy: true,
        autoWidth: false,
        ajax: {
            //url: BaseUrl + "service/inspectiondataapproval/list-detail",
            url: buildServiceUrl("service/inspectiondatacheck/list-detail"),
            type: "POST",
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: function (d) {
                return JSON.stringify({
                    CourseCode: $("#Course").val(),
                    GroupCode: $("#GroupCodeModal").val(),
                    InspectionDate: formatDateGeneral($('#InspectionDate').val()),
                    Frequency: $("#FrequencyName").val()

                });
            },
            dataSrc: function (json) {

                let data = json.data || [];

                if (data.length > 0) {
                    $("#Remarks").val(data[0].Remarks || "");
                    $("#RemarksLeader").val(data[0].RemarksLeader || "");
                    
                }
                else {
                    $("#Remarks").val("");
                    $("#RemarksLeader").val("");
                }

                return data;
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
                data: "InspectionResult",
                autoWidth: false,
                render: function (data, type, row) {
                    if (type !== "display") return data ?? "";

                    return FormatInspectionResult(data);
                }
            },
            //{
            //    data: "Minimum",
            //    autoWidth: false,
            //    render: function (data, type, row) {
            //        if (data == null || data === "") return "";

            //        // Jika numeric
            //        if (!isNaN(data) && data.toString().trim() !== "") {
            //            return parseFloat(data)
            //                .toFixed(3)
            //                .replace(/\.?0+$/, "");
            //        }

            //        // Jika string seperti YES / NO
            //        return data;
            //    }
            //},
            //{
            //    data: "Maximum",
            //    autoWidth: false,
            //    render: function (data, type, row) {
            //        if (data == null || data === "") return "";

            //        // Jika numeric
            //        if (!isNaN(data) && data.toString().trim() !== "") {
            //            return parseFloat(data)
            //                .toFixed(3)
            //                .replace(/\.?0+$/, "");
            //        }

            //        // Jika string seperti YES / NO
            //        return data;
            //    }
            //},
            //{
            //    data: "InspectionResult",
            //    autoWidth: false,
            //    render: function (data, type, row) {
            //        if (data == null || data === "") return "";

            //        // Jika numeric
            //        if (!isNaN(data) && data.toString().trim() !== "") {
            //            return parseFloat(data)
            //                .toFixed(3)
            //                .replace(/\.?0+$/, "");
            //        }

            //        // Jika string seperti YES / NO
            //        return data;
            //    }
            //},
            { data: "Status", "autoWidth": false }

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

    $("#MyModal").modal("show");

    $("#Course").val(data.CourseCode || '');
    $("#Title").val(data.Title || '');
    $("#Group").val(data.GroupName || '');
    $("#InspectionDate").val(formatDate(data.InspectionDate));
    $("#FrequencyName").val(data.Frequency || '');
    $("#GroupCodeModal").val(data.GroupCode || '');
    $("#InspectionNo").val(data.InspectionNo || '');
    $("#Remarks").val("");
    loadDetail();
    //setTimeout(() => {
    //    tableDetail.columns.adjust().draw();
    //}, 200);
}
$("#MyModal").on('shown.bs.modal', function () {

    if (tableDetail) {
        tableDetail.columns.adjust();
    }

});

function clearTable() {
    let dt = $("#tableID").DataTable();
    dt.clear().draw(false);
    dt.columns.adjust();
    //$("#tableID").dataTable().fnClearTable();

}

function checkStatusAdmin() {


    if (inspectionDate != "" && frequency != "") {
        isRedirect = true;
        //setTimeout(
        //    loadData(clusterCode, plantCode, courseCode, groupCode, frequency, inspectionDate, inspectionDate)
        //    , 200);


        if (statusAdmin == "1") {
            $("#InspectionDateFrom").prop("disabled", true);
            $(".btn-submit").prop("disabled", true);
            $(".btn-popupsendback").prop("disabled", true);
            $(".btn-check-approve").prop("disabled", true);
            $(".btn-check").prop("disabled", true);
            $(".btn-submit-sendback").prop("disabled", true);

        } else {
            $("#InspectionDateFrom").prop("disabled", false);
            $(".btn-submit").prop("disabled", false);
            $(".btn-popupsendback").prop("disabled", false);
            $(".btn-check-approve").prop("disabled", false);
            $(".btn-check").prop("disabled", false);
            $(".btn-submit-sendback").prop("disabled", false);
        }
    } else {

        isRedirect = false;
        // loadData($("#ClusterCode").val(), $("#PlantCode").val(), $("#CourseCode").val(), $("#GroupCode").val(), $("#Frequency").val(), $("#InspectionDateFrom").val());

        if (statusAdmin == "1") {
            $(".btn-submit").prop("disabled", true);
            $(".btn-popupsendback").prop("disabled", true);
            $(".btn-check-approve").prop("disabled", true);
            $(".btn-check").prop("disabled", true);
            $(".btn-submit-sendback").prop("disabled", true);

        } else {
            $(".btn-submit").prop("disabled", false);
            $(".btn-popupsendback").prop("disabled", false);
            $(".btn-check-approve").prop("disabled", false);
            $(".btn-check").prop("disabled", false);
            $(".btn-submit-sendback").prop("disabled", false);
        }
    }


}

//function clearDefvalue() {

//    clusterCode = "";
//    plantCode = "";
//    groupCode = "";
//    frequency = "";
//    inspectionDate = "";
//}

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


