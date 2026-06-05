var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;

loadData();
loadDataSetting("ALL", "ALL");

populateDDL({
    url: "service/mscluster/list",
    dropdownId: "ClusterCode",
    codeField: "ClusterCode",
    descField: "Description",
    descTextbox: "ClusterName",
    width: "120px",
    function() {
        $("#ClusterCode").trigger('changed.bs.select');
    }
});

reset();

function loadData() {
    if (table) {
        table.ajax.reload(null, false);
        return;
    }
    table = $("#tableID").DataTable({
        destroy: true,
        ajax: {
            url: BaseUrl + "service/groupsetup/list",
            //url: buildServiceUrl("service/groupsetup/list"),
            type: "POST",
            xhrFields: {
                withCredentials: true
            },
            contentType: "application/json",
            dataSrc: function (json) {

                if (!json || json.message == "Unauthorized") {
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
                orderable: false,
                render: function (data, type, row) {
                    return ` <input type="checkbox" class="chk-select" name="groupSelect" value="${row.GroupID}" data-plantcode="${row.PlantCode || ''}">`;
                }
            },
            { data: "GroupID" },
            { data: "GroupName" }
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

    });

}

//SELCTED
$(document).on("change", ".chk-select", function () {

    if ($(this).is(":checked")) {

        // hanya 1 yang boleh dipilih
        $(".chk-select").not(this).prop("checked", false);

        let groupId = $(this).val();
        let plantCode = $(this).data("plantcode");

        loadGroupDetail(groupId);
        loadDataSetting(groupId, plantCode);

    } else {

        reset();
    }

});

//add new
$(document).on("click", ".btn-add", function () {
    reset();
});

//onchange cluster
$(document).on("change", "#ClusterCode", function () {
    let desc = $("#ClusterCode option:selected").data("desc");
    $("#ClusterName").val((desc || "").trim());

    populateDDL({
        url: "service/msplant/list",
        dropdownId: "PlantCode",
        model: {
            ClusterCode: $("#ClusterCode").val()
        },
        codeField: "PlantCode",
        descField: "Description",
        descTextbox: "PlantName",
        width: "150px",
        
    });

});

$(document).on("change", "#PlantCode", function () {
    let desc = $("#PlantCode option:selected").data("desc");
    $("#PlantName").val((desc || "").trim());
});

//SAVE 
$(document).on("click", ".btn-save", function () {
   
    
    let tablePrev = $('#tableID2').DataTable();
    let tableCourse = $('#tableID3').DataTable();

    // 🔥 build privilege
    let detailPrivilege = [];

    $('#tableID2').DataTable().rows().every(function () {
        let row = this.node();
        let data = this.data();

        let isAccess = $(row).find('.chk-access').is(':checked');
        let isUpdate = $(row).find('.chk-update').is(':checked');
        let isDelete = $(row).find('.chk-delete').is(':checked');

        // 🔥 hanya push kalau ada yg dicentang
        if (isAccess || isUpdate || isDelete) {
            detailPrivilege.push({
                GroupID: $("#GroupID").val(),
                MenuID: data.MenuID,
                AllowAccess: isAccess,
                AllowUpdate: isUpdate,
                AllowDelete: isDelete
            });
        }
    });

    //let detailPrivilege = [];
    //tablePrev.rows().every(function () {
    //    let d = this.data();

    //    detailPrivilege.push({
    //        GroupID: $("#GroupID").val(),
    //        MenuID: d.MenuID,
    //        AllowAccess: $(this.node()).find(".chk-access").is(":checked"),
    //        AllowUpdate: $(this.node()).find(".chk-update").is(":checked"),
    //        AllowDelete: $(this.node()).find(".chk-delete").is(":checked")
    //    });
    //});

    // 🔥 build course
    let detailCourse = [];

    $('#tableID3').DataTable().rows().every(function () {
        let row = this.node();
        let data = this.data();

        let isChecked = $(row).find('.chk-course').is(':checked');

        if (isChecked) {
            detailCourse.push({
                GroupID: $("#GroupID").val(),
                CourseCode: data.CourseCode,
                Show: true
            });
        }
    });

    //let detailCourse = [];
    //tableCourse.rows().every(function () {
    //    let d = this.data();

    //    detailCourse.push({
    //        GroupID: $("#GroupID").val(),
    //        CourseCode: d.CourseCode,
    //        Show: $(this.node()).find(".chk-course").is(":checked")
    //    });
    //});

    // 🔥 final model
    let model = {
        GroupID: $("#GroupID").val(),
        GroupName: $("#GroupName").val(),
        ClusterCode: $("#ClusterCode").val(),
        PlantCode: $("#PlantCode").val(),
        InspectorApproval: $("#Inspector").is(":checked") ? "1" : "0",
        CheckerApproval: $("#Checker").is(":checked") ? "1" : "0",
        LeaderApproval: $("#Leader").is(":checked") ? "1" : "0",
        SupervisorApproval: $("#Supervisor").is(":checked") ? "1" : "0",
        DetailPrivilege: detailPrivilege,
        DetailCourse: detailCourse
    };


    $.ajax({
        type: "POST",
        url: BaseUrl + "service/groupsetup/save",
        //url: buildServiceUrl("service/groupsetup/save"),
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify(model),
        success: function (result) {
          
            if (!result.success) {
                reset();
                toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });
            }
            else {
                reset();
                toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
            }

        }
    })
    //}
});

// DELETE
$(document).on("click", ".btn-delete", function () {
        
    var id = $("#GroupID").val();

    Swal.fire({
        title: 'Are you sure?',
        text: "Data will be deleted permanently!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {

        if (result.isConfirmed) {
            $.ajax({
                url: BaseUrl + "service/groupsetup/delete",
                //url: buildServiceUrl("service/groupsetup/delete"),
                type: "POST",
                contentType: "application/json",
                xhrFields: {
                    withCredentials: true
                },
                data: JSON.stringify({
                    GroupID: id
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

$(document).on("click", ".btn-clear", function () {
    // reset
    reset();
});

document.querySelectorAll(".tab").forEach(tab => {
    tab.addEventListener("click", function () {

        // remove active semua tab
        document.querySelectorAll(".tab").forEach(t => t.classList.remove("active"));
        document.querySelectorAll(".tab-content").forEach(c => c.classList.remove("active"));

        // aktifkan tab yg diklik
        this.classList.add("active");
        document.getElementById(this.dataset.tab).classList.add("active");


        let table2 = $('#tableID3').DataTable();

        table2.columns.adjust().draw();
        let table3 = $('#tableID3').DataTable();
2                                                               
        table3.columns.adjust().draw();
        //setTimeout(function () {
        //    $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        //}, 100);
       

    });
});

function loadGroupDetail(pGroupId) {

    $.ajax({
        url: BaseUrl + "service/groupsetup/detail",
        //url: buildServiceUrl("service/groupsetup/detail"),
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            GroupID: pGroupId
        }),
        success: function (res) {

            if (res && res.data) {

                // TEXT
                $("#GroupID").val(res.data.GroupID).prop("readonly", true);
                $("#GroupName").val(res.data.GroupName);

                $("#ClusterCode").val(res.data.ClusterCode).trigger('change');
                $("#ClusterName").val(res.data.ClusterName);

                populateDDL({
                    url: "service/msplant/list",
                    dropdownId: "PlantCode",
                    model: {
                        ClusterCode: $("#ClusterCode").val()
                    },
                    codeField: "PlantCode",
                    descField: "Description",
                    descTextbox: "PlantName",
                    width: "150px",
                    selectedValue: res.data.PlantCode
                });

                //$("#PlantCode").val(res.data.PlantCode).trigger('change');
                $("#PlantName").val(res.data.PlantName);

            
                // 🔥 SET CHECKBOX (sesuai data API)
                $("#Inspector").prop("checked", res.data.InspectorApproval == "1");
                $("#Leader").prop("checked", res.data.LeaderApproval =="1");
                $("#Checker").prop("checked", res.data.CheckerApproval == "1");
                $("#Supervisor").prop("checked", res.data.SupervisorApproval == "1");

            }
        },
        error: function (xhr) {
            
            toastr.warning('Failed to load data.', 'Error');
        }
    });

}

//PRIVILEGE SETTING & COURSE SETTING
var tablePrivSetting;
var tableCourseSetting;
function loadDataSetting(pGroupId,pPlantCode) {

    tablePrivSetting = $("#tableID2").DataTable({
        destroy: true,
        autoWidth: true,
        ajax: {
            url: BaseUrl + "service/groupsetup/privilege",
            //url: buildServiceUrl("service/groupsetup/privilege"),
            type: "POST",
            xhrFields: {
                withCredentials: true
            },
            data: function (d) {
                return JSON.stringify({
                    GroupID: pGroupId
                });
            },
            contentType: "application/json",
            dataSrc: function (json) {

                if (!json || json.message == "Unauthorized") {
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
                orderable: true,
                width: "100",
                //render no urut otomatis
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: "GroupMenu", orderable: true,width:"150" },
            { data: "MenuID", orderable: true, width: "150" },
            { data: "MenuName", orderable: true, width: "400" },
            {
                data: "AllowAccess",
                className: "text-center",
                title: `<input type="checkbox" id="chk-all-access"> &nbspAccess`,
                orderable: false, width: "100",
                render: function (data, type, row) {
                    return `<input type="checkbox" class="chk-access" name="accessSelect" value="${row.MenuID}" ${data == true ? "checked" : ""}>`;
                }
               
            },
            {
                data: "AllowUpdate",
                className: "text-center",
                title: `<input type="checkbox" id="chk-all-update"> &nbspUpdate`,
                orderable: false, width: "100",
                render: function (data, type, row) {
                    return `<input type="checkbox" class="chk-update" name="updateSelect" value="${row.MenuID}" ${data == true ? "checked" : ""}>`;
                }

            },
            {
                data: "AllowDelete",
                className: "text-center",
                title: `<input type="checkbox" id="chk-all-delete"> &nbspDelete`,
                orderable: false, width: "100",
                render: function (data, type, row) {
                    return `<input type="checkbox" class="chk-delete" name="deleteSelect" value="${row.MenuID}" ${data == true ? "checked" : ""}>`;
                }

            },
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

       
    });

    
    tableCourseSetting = $("#tableID3").DataTable({
        destroy: true,
        autoWidth:true,
        ajax: {
            //url: BaseUrl + "service/groupsetup/coursesetting",
            url: buildServiceUrl("service/groupsetup/coursesetting"),
            type: "POST",
            xhrFields: {
                withCredentials: true
            },
            data: function (d) {
                return JSON.stringify({
                    GroupID: pGroupId,
                    PlantCode: pPlantCode
                });
            },
            contentType: "application/json",
            dataSrc: function (json) {

                if (!json || json.message == "Unauthorized") {
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
                orderable: false,
                width: "100",
                //render no urut otomatis
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: "CourseCode", orderable: true, width: "180", },
            { data: "Title", orderable: false, width: "400", },
            {
                data: "Show",
                className: "text-center",
                title: `<input type="checkbox" id="chk-all-course"> &nbspShow`,
                orderable: false,
                width: "100",
                render: function (data, type, row) {
                    return `<input type="checkbox" class="chk-course" name="courseSelect" value="${row.CourseCode}" ${data == true ? "checked" : ""}>`;
                }

            }
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


//check all access
$(document).on("change", "#chk-all-access", function () {
    let checked = $(this).is(":checked");

    $('#tableID2').DataTable().rows({ search: 'applied' }).nodes().to$()
        .find('.chk-access')
        .prop("checked", checked);
});

$(document).on("change", ".chk-access", function () {
    let total = $('.chk-access').length;
    let checked = $('.chk-access:checked').length;

    $("#chk-all-access").prop("checked", total === checked);
});

//check all update
$(document).on("change", "#chk-all-update", function () {
    let checked = $(this).is(":checked");

    $('#tableID2').DataTable().rows({ search: 'applied' }).nodes().to$()
        .find('.chk-update')
        .prop("checked", checked);
});

//check all delete
$(document).on("change", "#chk-all-delete", function () {
    let checked = $(this).is(":checked");

    $('#tableID2').DataTable().rows({ search: 'applied' }).nodes().to$()
        .find('.chk-delete')
        .prop("checked", checked);
});

//course
$(document).on("change", "#chk-all-course", function () {
    let checked = $(this).is(":checked");

    $('#tableID3').DataTable().rows({ search: 'applied' }).nodes().to$()
        .find('.chk-course')
        .prop("checked", checked);
});

function reset() {
    $("#GroupID").val('');
    $("#GroupName").val('');
    $("#ClusterCode").val('').trigger('change');
    $("#ClusterName").val('');
    $("#PlantCode").val('').trigger('change');
    $("#PlantName").val('');

    $("#Inspector, #Leader, #Checker, #Supervisor").prop("checked", false);
    $("#GroupID").prop('readonly', false);


    // 🔥 uncheck semua di grid
    $(".chk-select").prop("checked", false);

    let tablePrev = $('#tableID2').DataTable();

    // reset semua checkbox di semua row
    tablePrev.rows().nodes().to$().find(
        '.chk-access, .chk-update, .chk-delete, .chk-course'
    ).prop("checked", false);

    let tableCourse = $('#tableID3').DataTable();
    tableCourse.rows().nodes().to$().find(
        '.chk-course'
    ).prop("checked", false);
}