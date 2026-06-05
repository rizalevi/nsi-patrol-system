var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;

loadData();

function loadData() {
    if (table) {
        table.ajax.reload(null, false);
        return;
    }

     table = $("#tableID").DataTable({
        destroy: true,
        ajax: {
          
            url: BaseUrl + "service/errorlog/list",
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
            { data: "Method", width: "100" },
            {
                data: "CreatedAt", width: "200",
                render: function (data) {
                    return formatDateTime(data);

                },
            },
            { data: "UserId", width: "200" },
            { data: "Path", width: "200" },
            { data: "Message", width: "450" },
            //{ data: "StackTrace", width:"150" },
            {
                data: null,
                width: "120" ,
                className: "text-center",
                render: function (data, type, row) {
                        return `
                                <button class="btn btn-xs btn-warning btn-edit" data-id="${row.StackTrace}">
                                   View Detail
                                </button>
                                `
                    
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


$(document).on("click", ".btn-edit", function () {

    var value = $(this).data("id");

    $("#StackTrace").val(value);
    $("#MyModal").modal("show");

});
