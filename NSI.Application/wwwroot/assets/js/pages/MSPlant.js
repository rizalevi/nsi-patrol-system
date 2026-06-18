var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;

init();


if ($("#ClusterCodeFilter").val() != "") {
    table.ajax.reload();
}

/*
============================================
            onchange ddl
============================================
*/

//cluster
$(document).on("change", "#ClusterCodeFilter", function () {
    let desc = $("#ClusterCodeFilter option:selected").data("desc");
    $("#ClusterNameFilter").val((desc || "").trim());
    
    table.ajax.reload();
});


//add new
$(document).on("click", ".btn-addnew", function () {
    if ($("#ClusterCodeFilter").val() == "" || $("#ClusterCodeFilter").val() == null) {
        toastr.info('Please Select Cluster', 'Info', { timeOut: 3000 });
        return;
    }
   
    $("#form")[0].reset();
    $("#PlantCode").val('');
    $("#PlantName").val('');
    $("#ClusterCode").val($("#ClusterCodeFilter").val()).trigger('change.select2');
    $("#ClusterName").val($("#ClusterNameFilter").val());
    
    $("#ModalTitle").html("Add New Plant Master");
    $("#MyModal").modal("show");
    $('#InsertData').show();
    $('#UpdateData').hide();

    $("#PlantCode").prop('readonly', false);
    $("#ClusterCode").prop('readonly', true);
    $("#ClusterName").prop('readonly', true);
});

//search data
$(document).on("click", ".btn-search", function () {
    //loadData($('#ClusterCodeFilter').val());
    
    table.ajax.reload();
});

//clear button
$(document).on("click", ".btn-clear", function () {
    // reset dropdown
    $("#ClusterCodeFilter").selectpicker('val', '');


    $("#ClusterNameFilter").val("");
    $("#tableID").dataTable().fnClearTable();
});

//POPUP MODAL GET DATA
$(document).on("click", ".btn-edit", function () {

    var code = $(this).data("id").toString();

    $.ajax({
        //url: BaseUrl + "service/msplant/detail",
        url: buildServiceUrl("service/msplant/detail"),
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            PlantCode: code,
            ClusterCode: $("#ClusterCodeFilter").val()
        }),
        success: function (res) {
           
            if (res && res.data) {
                $("#PlantCode").val(res.data.PlantCode);
                $("#PlantName").val(res.data.Description);
                $("#PlantCode").prop("readonly", true);

                $("#ClusterCode").prop("readonly", true);
                $("#ClusterName").prop("readonly", true);

                $("#ClusterCode").val(res.data.ClusterCode).trigger('change.select2');
                $("#ClusterName").val(res.data.ClusterName);

                $("#ModalTitle").html("Plant Master");

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
    var model = new Object();
    model.PlantCode = $('#PlantCode').val();
    model.Description = $('#PlantName').val();
    model.ClusterCode = $('#ClusterCode').val();

    if (!validateForm(model)) return;

    $.ajax({
        type: "POST",
       // url: BaseUrl + "service/msplant/create",
        url: buildServiceUrl("service/msplant/create"),
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
    /*    if ($("#form").valid()) {*/
    var model = new Object();
    model.PlantCode   = $('#PlantCode').val();
    model.Description = $('#PlantName').val();
    model.ClusterCode = $('#ClusterCode').val();

    if (!validateForm(model)) return;

    $.ajax({
        type: "PATCH",
        //url: BaseUrl + "service/msplant/update",
        url: buildServiceUrl("service/msplant/update"),
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

    let data = $(this).data("id").split("|");
    let plantCode = data[0].toString();;
    let clusterCode = data[1].toString();;

    Swal.fire({
        title: 'Are you sure?',
        text: "Data will be deleted permanently!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {

        if (result.isConfirmed) {
            $.ajax({
                //url: BaseUrl + "service/msplant/delete",
                url: buildServiceUrl("service/msplant/delete"),
                type: "POST",
                contentType: "application/json",
                xhrFields: {
                    withCredentials: true
                },
                data: JSON.stringify({
                    ClusterCode: clusterCode,
                    PlantCode: plantCode
                }),
                success: function (result) {
                   
                    if (!result.success) {
                        toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });

                    } else {
                       
                        toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
                        $('#tableID').DataTable().ajax.reload(null, false);
                    }
                },
                error: function (xhr) {
                    debugger
                    const response = xhr.responseJSON;

                    toastr.warning(
                        response?.message || 'An unexpected error occurred.',
                        'Warning',
                        {
                            timeOut: 3000,
                            closeButton: true
                        }
                    );
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
        url: "service/mscluster/list",
        dropdownId: "ClusterCodeFilter",
        codeField: "ClusterCode",
        descField: "Description",
        descTextbox: "ClusterNameFilter",
        width: "120px",
       
    });

    populateDDL({
        url: "service/mscluster/list",
        dropdownId: "ClusterCode",
        codeField: "ClusterCode",
        descField: "Description",
        descTextbox: "ClusterName",
        width: "120px",

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
        ajax: {
            //url: BaseUrl + "service/msplant/list",
            url: buildServiceUrl("service/msplant/list"),
            type: "POST",
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: function (d) {
                return JSON.stringify({
                    ClusterCode: $("#ClusterCodeFilter").val()
                });
            },
            dataSrc: function (json) {
                return json.data; // atau json kalau array langsung
            },


        },

        columns: [
            {
                data: null,
                className: "text-center",
                render: function (data, type, row) {

                    if (allowDelete == '0') {

                        return `
                                    <button class="btn btn-xs theme-color btn-edit" data-id="${row.PlantCode}">
                                        <i class="fa fa-edit"></i>
                                    </button>
                                    <button class="btn btn-xs theme-color" disabled>
                                        <i class="fa fa-trash-o"></i>
                                    </button>
                                `;

                    } else {

                        return `
                                    <button class="btn btn-xs theme-color btn-edit" data-id="${row.PlantCode}">
                                        <i class="fa fa-edit"></i>
                                    </button>  
                                    <button class="btn btn-xs theme-color btn-delete" data-id="${row.PlantCode}|${$("#ClusterCodeFilter").val()}">
                                        <i class="fa fa-trash-o"></i>
                                    </button>
                                `;
                    }
                }
            },
            { data: "PlantCode" },
            { data: "Description" },
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

function validateForm(model) {
    if (!model.PlantCode || !model.Description || !model.ClusterCode) {
        toastr.warning("Please fill all required fields (*)", "Validation Error");
        return false;
    }
    return true;
}
