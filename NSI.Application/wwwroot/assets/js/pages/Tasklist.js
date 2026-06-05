var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;


// pertama kali jalan
updateDateTime();
// update tiap 1 detik
setInterval(updateDateTime, 1000);
//waitingDialog.show("Please wait retrieving data...");
loadSummaryTask();
loadData();

function loadSummaryTask() {

    $.ajax({
        url: BaseUrl + "service/tasklist/get-summary",
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        success: function (res) {
            
            if (res && res.data) {
           
                $("#lblUncomplete").html(res.data[0].Uncompleted);
                $("#Sendback").html(res.data[0].SendBack);
                $("#lblUnchecked").html(res.data[0].Unchecked);
                $("#lblUnapproved").html(res.data[0].Unapproved);
                $("#lblCompleted").html(res.data[0].Completed);
            } else {
                $("#lblUncomplete").html(0);
                $("#lblSendBack").html(0);
                $("#lblUnchecked").html(0);
                $("#lblUnapproved").html(0);
                $("#lblCompleted").html(0);
            }

        }
    });
}

function loadData(filter) {
    
    let userGroup = 1;
    let groupName="";

    $.ajax({
        //url: BaseUrl + "service/tasklist/get-usergroup",
        url: buildServiceUrl("service/tasklist/get-usergroup"),
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        async:false,
        success: function (res) {
            userGroup = res.data[0].UserGroupRole;
            $("#userGroupSetup").text("  -  User Group (" + res.data[0].GroupName + ")");

        },
       
    });
    
    let isSupervisor = userGroup == 3;
    // 🔥 reset table
    $("#tableID").empty();

    // 🔥 THEAD DINAMIS
    let thead = `<thead>
                    <tr role="row">
                        <th style="text-align:center;color:white;">No</th>
                        <th style="text-align:center;color:white;">Document No</th>
                        <th style="text-align:center;color:white;">Course Code</th>
                        <th style="text-align:center;color:white;">Title</th>
                        <th style="text-align:center;color:white;">Group Name</th>
                        <th style="text-align:center;color:white;">Date</th>
                        <th style="text-align:center;color:white;">Frequency</th>
                        <th style="text-align:center;color:white;">Status</th>
                        <th style="text-align:center;color:white;">Inspector User</th>
    `;

    if (isSupervisor) {
        thead += `
                        <th style="text-align:center;color:white;">Inspector Date</th>
                        <th style="text-align:center;color:white;">Leader User</th>
                        <th style="text-align:center;color:white;">Leader Date</th>
        `;
    }

    thead += `          
                    </tr>
                 </thead>`;

    $("#tableID").append(thead);
   
    // 🔥 COLUMN CONFIG DINAMIS
    let columns = [
        {
            data: null, className: "v-align", width: "80",
            render: function (data, type, row, meta) {
                return meta.row + 1;
            }
        },
        {

            data: "DocumentNo",width:"180",
            render: function (data, type, row) {

                if (type === "display") {
                    let actionUrl = "";
                    let menuID = "";
                    // 🔥 KONDISI STATUS
                    if (row.Status=== "Unapproved") {
                        actionUrl = "/InspectionDataApproval/Detail";
                        menuID = "B03"
                    }
                    else if (row.Status === "Unchecked") {
                        actionUrl = "/InspectionDataCheck/Detail";
                        menuID = "B02"
                    }
                    else {
                        actionUrl = "/InspectionDataUpload/Detail";
                        menuID = "B01"

                    }

                    return `
                                <form method="post" action="${actionUrl}" style="display:inline;">
                                    <input type="hidden" name="ID" value="${menuID}" />
                                    <input type="hidden" name="ClusterCode" value="${row.ClusterCode}" />
                                    <input type="hidden" name="PlantCode" value="${row.PlantCode}" />
                                    <input type="hidden" name="CourseCode" value="${row.CourseCode}" />
                                    <input type="hidden" name="GroupCode" value="${row.GroupCode}" />
                                    <input type="hidden" name="InspectionDate" value="${row.InspectionDate}" />
                                    <input type="hidden" name="Frequency" value="${row.Frequency}" />
                                    <button type="submit" class="btn btn-link p-0 m-0 align-baseline">
                                        ${data}
                                    </button>
                                </form>
                            `;
                }

                return data;
            }
        },

        { data: "CourseCode", orderable: true, width: "150", },
        { data: "Title", width: "300px", },
        { data: "GroupName", width: "200", },
        {
            data: "InspectionDate", class: "text-center", width: "120", render: function (data) {

                if (!data || data === "0001-01-01T00:00:00") return "";
                return formatDate(data);

            },
        },
        { data: "Frequency", width: "120", },
        { data: "Status", className: "text-center", width: "150", },
        { data: "InspectorUser", width: "200", }
    ];

    if (isSupervisor) {
        columns.push(
            {
                data: "InspectorDate", width: "180px", render: function (data) {

                    if (!data || data === "0001-01-01T00:00:00") return "";
                    return formatDateTime(data);

                },
            },
            { data: "LeaderUser", width: "200", defaultContent: "" },
            {
                data: "LeaderDate", width: "180px", render: function (data) {

                    if (!data || data === "0001-01-01T00:00:00") return "";
                    return formatDateTime(data);

                },
            },
        );
    }

    //columns.push({ data: "Status", className: "text-center", width: "150", });

    table = $("#tableID").DataTable({
        destroy: true,
        ajax: {
            //url: BaseUrl + "service/tasklist/list",
            url: buildServiceUrl("service/tasklist/list"),
            type: "POST",
            xhrFields: {
                withCredentials: true
            },
            contentType: "application/json",
            dataSrc: function (json) {
              
                return json.data; // atau json kalau array langsung
            },
            complete: function () {
                //setInterval(
                //    waitingDialog.hide(), 500);
            }, error: function () {
               // waitingDialog.hide();
            }
            
        },
        columns: columns,
        autoWidth: false,
        searching: true,
        ordering: true,
        paging: true,
        info: true,
        bSort: true,
        bLengthChange: true,
        pageLength: 25,
        scrollX: true,
        scrollY: "calc(105vh - 300px)",
        processing: false,
        serverSide: false,
        responsive: false,
        fixedColumns: {
            leftColumns: 4
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
        
    });
  
}

//realtime current date and time
function updateDateTime() {
    const now = new Date();

    const options = {
        weekday: 'long',   // Senin
        year: 'numeric',   // 2026
        month: 'long',     // Maret
        day: 'numeric',    // 30
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit',
        hour12: false
    };

    const formatted = now.toLocaleString('en-US', options);

    document.getElementById("currentDateTime").innerText = formatted;
}


//$(document).on("click", ".legend-item", function () {

//    const status = $(this).data("status");

//    // Highlight card aktif
//    $(".legend-item").removeClass("active");
//    $(this).addClass("active");

//    // Ambil kolom terakhir (Status)
//    const statusColumnIndex = table.columns().indexes().length - 1;
//    // Filter exact match
//    table
//        .column(statusColumnIndex)
//        .search("^" + $.fn.dataTable.util.escapeRegex(status) + "$", true, false)
//        .draw();
//});

$(document).on("click", ".legend-item", function () {
    const $this = $(this);
    const status = $this.data("status");
    const statusColumnIndex = table.column(7).index();
    let today = new Date();

    // Jika card yang sama diklik lagi → hapus filter
    if ($this.hasClass("active")) {
        $this.removeClass("active");

        table
            .column(statusColumnIndex)
            .search("")
            .draw();

        return;
    }

    // Aktifkan card baru
    $(".legend-item").removeClass("active");
    $this.addClass("active");

    // Khusus Uncompleted =>>> tampilkan semua data
    if (status === "Uncompleted") {
  
        table
            .column(statusColumnIndex)
            .search(
                "",
                true,
                false
            )
            .draw();
        return;
    }


    table
        .column(statusColumnIndex)
        .search(
            "^" + $.fn.dataTable.util.escapeRegex(status) + "$",
            true,
            false
        )
        .draw();
});



/*
////manual binding data
function loadData() {

    table = $("#tableID").DataTable({
        destroy: true,
        ajax: {
            url: BaseUrl + "service/tasklist/get-list",
            type: "POST",
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            dataSrc: function (json) {
                return json.data; // atau json kalau array langsung
            }
        },

        columns: [
            {
                data: null,
                className: "text-center",
                render: function (data, type, row) {
                    return `
                            <button class="btn btn-xs btn-warning btn-link" data-id="${row.DocumentNo}">
                              
                            </button>
                           
                        `;
                }
            },
            { data: "CourseCode", "autoWidth": false },
            { data: "Title", "autoWidth": false },
            { data: "Group", "autoWidth": false },
            { data: "Date", "autoWidth": false },
            { data: "Frequency", "autoWidth": false },
            { data: "InspectorUser", "autoWidth": false },
            { data: "InspectorDate", "autoWidth": false }
            { data: "LeaderUser", "autoWidth": false }
            { data: "LeaderDate", "autoWidth": false }
            { data: "Status", "autoWidth": false }


        ],
        autoWidth: false,
        searching: true,
        ordering: true,
        paging: true,
        info: true,
        bSort: true,
        bLengthChange: false,
        pageLength: 15,
        scrollX: true,
        scrollCollapse: true,
        processing: false,
        serverSide: false,
        response: true
       

    });
}
*/

//$(document).on("click", ".btn-search", function () {
//    if (table) {
//        table.ajax.reload(null, false);

//    } else {
//        loadData();
//    }
//});