var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;

loadData();

function loadData() {

     table = $("#tableID").DataTable({
        destroy: true,
        ajax: {
            /*url: buildServiceUrl("service/mscluster/list"),*/
            url : BaseUrl + "service/mscluster/list",
            type: "POST",
            xhrFields: {
                withCredentials: true
            },
            contentType: "application/json",
            dataSrc: function (json) {
              
                return json.data; // atau json kalau array langsung
            },
            
        },
        columns: [
            {
                data: null,
                className: "text-center",
                render: function (data, type, row) {

                    if (allowDelete == "1") {
                        return `
                                <button class="btn btn-xs theme-color btn-edit" data-id="${row.ClusterCode}">
                                    <i class="fa fa-edit"></i>
                                </button>
                                <button class="btn btn-xs theme-color btn-delete" data-id="${row.ClusterCode}">
                                    <i class="fa fa-trash-o"></i>
                                </button>
                            `;

                    } else {
                        return `
                                <button class="btn btn-xs theme-color btn-edit" data-id="${row.ClusterCode}">
                                    <i class="fa fa-edit"></i>
                                </button>  
                                <button class="btn btn-xs theme-color" disabled>
                                    <i class="fa fa-trash-o"></i>
                                </button>
                            `;
                    }
                }
            },
            { data: "ClusterCode" },
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
         searching: true,
         ordering: true,
         paging: true,
         info: true,
         bSort: true,
         bLengthChange: true,
         pageLength: 25,
         scrollX: true,
         scrollY: "calc(100vh - 380px)",
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

//add new
$(document).on("click", ".btn-addnew", function () {
    $("#ClusterCode").val('');
    $("#ClusterName").val('');

    $("#ModalTitle").html("Add Cluster Master");

    $("#MyModal").modal("show");

    $('#InsertData').show();
    $('#UpdateData').hide();
    $("#ClusterCode").prop('readonly', false);
});


//POPUP MODAL GET DATA
$(document).on("click", ".btn-edit", function () {

    var code = $(this).data("id");

    $.ajax({
        //url: BaseUrl + "service/mscluster/detail",
        url: buildServiceUrl("service/mscluster/detail"),
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            ClusterCode: code
        }),
        success: function (res) {
          
            if (res && res.data) {

                $("#ClusterCode").val(res.data.ClusterCode);
                $("#ClusterName").val(res.data.Description);

                $("#ClusterCode").prop("readonly", true);

                $("#ModalTitle").html("Cluster Master");

                $("#InsertData").hide();
                $("#UpdateData").show();

                $("#MyModal").modal("show");

            }
        },
        error: function () {

            toastr.warning('Failed to load data.', 'Error', { timeOut: 3000, "closeButton": true });

        }
    });

});

//INSERT
$(document).on("click", "#InsertData", function () {
    var model = new Object();
    model.ClusterCode = $('#ClusterCode').val();
    model.Description = $('#ClusterName').val();

  
    $.ajax({
        type: "POST",
        //url: BaseUrl + "service/mscluster/create",
        url: buildServiceUrl("service/mscluster/create"),
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
    model.ClusterCode = $('#ClusterCode').val();
    model.Description = $('#ClusterName').val();

    $.ajax({
        type: "PATCH",
        //url: BaseUrl + "service/mscluster/update",
        url: buildServiceUrl("service/mscluster/update"),
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

    var id = $(this).data("id");

    Swal.fire({
        title: 'Are you sure?',
        text: "Data will be deleted permanently!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {

        if (result.isConfirmed) {
            $.ajax({
                //url: BaseUrl + "service/mscluster/delete",
                url: buildServiceUrl("service/mscluster/delete"),
                type: "POST",
                contentType: "application/json",
                xhrFields: {
                    withCredentials: true
                },
                data: JSON.stringify({
                    ClusterCode: id
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

//wajib ada 
$(document).on("keyup", "#tableSearch", function () {
    let keyword = $(this).val();
   
    timer = setTimeout(function () {
        table.search(keyword).draw();
    }, 300);
   
});

