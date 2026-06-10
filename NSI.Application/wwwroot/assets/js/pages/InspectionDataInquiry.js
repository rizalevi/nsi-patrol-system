var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;


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
//onchange cluster
$(document).on("change", "#ClusterCode", function () {
    let desc = $("#ClusterCode option:selected").data("desc");
    $("#ClusterName").val((desc || "").trim());
    clearTable()
});

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
      
        //callback: function () {
            
        //    if (($("#PlantCode").val() != null && $("#PlantCode").val() != "")) {
        //        let ddl = $("#CourseCode");
                
        //        let code = "ALL";
        //        let desc = "ALL";

        //        ddl.prepend(
        //            '<option value="' + code + '" ' +
        //            'data-content="<div style=\'display:flex;width:300px\'>' +
        //            '<span style=\'width:80px;text-align:left\'>' + code + '</span>' +
        //            '<span style=\'padding-left:10px; text-align:left;\'>' + desc + '</span>' +
        //            '</div>" ' +
        //            'data-code="' + code + '" ' +
        //            'data-desc="' + desc + '">' +       
        //            code + ' - ' + desc +
        //            '</option>'
        //        );
        //        ddl.trigger('change');
        //        //ddl.selectpicker('refresh');

        //    }

        //    $("#CourseCode").trigger('change');
        //}
    });

    clearTable();

});

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
                ddl.trigger('change');



            }
            //$("#GroupCode").trigger('changed.bs.select');
        }
    });

    clearTable();

});


//group
$(document).on("change", "#GroupCode", function () {

    let desc = $("#GroupCode option:selected").data("desc");
    $("#GroupName").val((desc || "").trim());


    let model = new Object();
    model.CourseCode = $("#CourseCode").val();
    model.GroupCode = $("#GroupCode").val();

    //inspection point ddl
    populateDDL({
        url: "service/inspectiondata/get-inspectionpoint",
        dropdownId: "InspectionPointCode", //ddl id
        model: {
            CourseCode: $("#CourseCode").val(),
            GroupCode: $("#GroupCode").val()
        },
        codeField: "Code",
        descField: "Description",
        descTextbox: "InspectionPointName",
        callback:function() {

            if ($("#GroupCode").val() != null) {
                let ddl = $("#InspectionPointCode");
                
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

            }

            $("#InspectionPointCode").trigger('change');
        }
    });
    clearTable();

});


//onchange inspection
$(document).on("change", "#InspectionPointCode", function () {
    let desc = $("#InspectionPointCode option:selected").data("desc");
    $("#InspectionPointName").val((desc || "").trim());
    updateNavButton();
});

$(document).on("change", "#Frequency", function () {
  
    let desc = $("#Frequency option:selected").data("desc");
    $("#FrequencyName").val((desc || "").trim());

});

$(document).on("change", ".chk-select", function () {

    if ($(this).is(":checked")) {

        // hanya 1 yang boleh dipilih
        $(".chk-select").not(this).prop("checked", false);

    }
});

//search data
$(document).on("click", ".btn-search", function () {
    //waitingDialog.show("Please wait retrieving data...");
    
    if (table) {
        table.ajax.reload(null, false);

    } else {
        loadData();
    }
});


//$('#tableID').on('xhr.dt', function (e, settings, json, xhr) {

//    if (!json || !json.data || json.data.length === 0) {
//        toastr.info("No data available in table", 'Info', {
//            timeOut: 3000,
//            closeButton: true
//        });
//    }

//});

//clear button
$(document).on("click", ".btn-clear", function () {
    // reset 
 

    $("#PlantCode").val('').trigger('change');
    $("#PlantName").val("");
    $("#CourseCode").val('').trigger('change');
    $("#CourseName").val("");
    $("#GroupCode").val('').trigger('change');
    $("#GroupName").val("");
    $("#Frequency").val('').trigger('change');
    $("#InspectionPointCode").val('').trigger('change');
    $("#InspectionPointName").val("");
    GlobalDatepicker.setDefault("#InspectionDateFrom", 0);
    GlobalDatepicker.setDefault("#InspectionDateTo", 2);

    clearTable();
});

//export excel
$(document).on("click", ".btn-excel", function () {

    let btn = $(".btn-excel");
    btn.prop("disabled", true).text("Exporting...");

    let model = {
        ClusterCode: $("#ClusterCode").val(),
        PlantCode: $("#PlantCode").val(),
        CourseCode: $("#CourseCode").val(),
        GroupCode: $("#GroupCode").val(),
        InspectionPointCode: $("#InspectionPointCode").val(),
        Frequency: $("#Frequency").val(),
        InspectionDateFrom: formatDateGeneral($('#InspectionDateFrom').val()),
        InspectionDateTo: formatDateGeneral($('#InspectionDateTo').val())
    };

    fetch('/InspectionDataInquiry/exportexcel', {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(model)
    })
    .then(response => {

            if (!response.ok) {
                throw new Error("Gagal export file");
            }

            // ===== AMBIL FILENAME DARI HEADER =====
            let fileName = "download.xlsx";
            let disposition = response.headers.get('Content-Disposition');

            if (disposition) {
                // UTF-8 filename (priority)
                let utf8Match = disposition.match(/filename\*\=UTF-8''([^;]+)/);
                if (utf8Match && utf8Match[1]) {
                    fileName = decodeURIComponent(utf8Match[1]);
                } else {
                    // fallback filename biasa
                    let normalMatch = disposition.match(/filename="?([^"]+)"?/);
                    if (normalMatch && normalMatch[1]) {
                        fileName = normalMatch[1];
                    }
                }
            }

            return response.blob().then(blob => ({ blob, fileName }));
    })
    .then(({ blob, fileName }) => {

            let url = window.URL.createObjectURL(blob);
            let a = document.createElement("a");

            a.href = url;
            a.download = fileName;

            document.body.appendChild(a);
            a.click();

            // cleanup
            a.remove();
            window.URL.revokeObjectURL(url);
    })
    .catch(err => {
            console.error(err);
    })
    .finally(() => {
            btn.prop("disabled", false).text("Excel");
    });
});

$(document).on("click", ".btn-print", function () {
    
    let courseCodes = [];
    let dates = [];
    let freqs = [];

    $(".chk-select:checked").each(function () {
        courseCodes.push($(this).val());
        dates.push($(this).data("inspectdate"));
        freqs.push($(this).data("frequency"));
    });

    //let frequency = $("#Frequency").val();

    // VALIDASI
    if (courseCodes.length === 0) {
        toastr.warning('Please select data to print preview', 'Info', { timeOut: 3000, "closeButton": true });

        return;
    }
    
    // gabungkan jadi string
    let courseCode = courseCodes.join(",");
    let date = dates.join(",");
    let frequency = freqs.join(",");

    let url = `/Report/InspectionReport?courseCode=${courseCode}&inspectionDate=${date}&frequency=${frequency}`;

    $("#reportFrame").attr("src", url);
    $("#modalReport").modal("show");
});

function init() {
   
    GlobalDatepicker.init(".datepicker");
   
    let now = new Date();
    let firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
    let today = new Date();

    $("#InspectionDateFrom").datepicker('setDate', firstDay);
    $("#InspectionDateTo").datepicker('setDate', today);


    //DDL Cluster
    populateDDL({
        url: "service/inspectiondata/get-cluster",
        dropdownId: "ClusterCode",
        codeField: "Code",
        descField: "Description",
        descTextbox: "ClusterName",
        //width: "180px",
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
                callback: function () {

                    $("#PlantCode").trigger('change');
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
        callback: function () {
           
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
                    code + ' - ' + desc +
                    '</option>'
                );

                ddl.trigger('change');
           
        }
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
        autoWidth:false,
        ajax: {
            //url: BaseUrl + "service/inspectiondatainquiry/list",
            url: buildServiceUrl("service/inspectiondatainquiry/list"),
            type: "POST",
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: function (d) {
                return JSON.stringify({
                    ClusterCode: $("#ClusterCode").val(),
                    PlantCode: $("#PlantCode").val(),
                    CourseCode: $("#CourseCode").val(),
                    GroupCode: $("#GroupCode").val(),
                    InspectionPointCode: $("#InspectionPointCode").val(),
                    Frequency: $("#Frequency").val(),
                    InspectionDateFrom: formatDateGeneral($('#InspectionDateFrom').val()),
                    InspectionDateTo: formatDateGeneral($('#InspectionDateTo').val()),

                });
            },
            dataSrc: function (json) {
                return json.data; // atau json kalau array langsung
            },
            complete: function () {
                //setInterval(
                //    waitingDialog.hide(), 500);
            }, error: function () {
                //waitingDialog.hide();
            }
        },

        columns: [
            {
                data: null,
                className: "text-center", "autoWidth": false,
                orderable: false,
                render: function (data, type, row) {
                    return ` <input type="checkbox" class="chk-select" name="groupSelect" value="${row.CourseCode}" data-frequency="${row.Frequency}" data-inspectdate="${row.InspectionDate || ''}">`;
                }
            },
            {
                data: "InspectionDate", "autoWidth": false,
                render: function (data) {
                    return formatDate(data);

                },
            },
            { data: "Frequency", "autoWidth": false },
            { data: "PlantCode", "autoWidth": false },
            { data: "CourseCode", "autoWidth": false },
            { data: "Group", "autoWidth": false },
            { data: "InspectionPoint", "autoWidth": false },
            { data: "Unit", "autoWidth": false },
            { data: "Minimum", "autoWidth": false },
            { data: "Maximum", "autoWidth": false },
            { data: "InspectionResult", "autoWidth": false },
            { data: "Status", "autoWidth": false },
            { data: "GroupSelection", "autoWidth": false },
            { data: "InspectorUser", "autoWidth": false },
            { data: "Notes", "autoWidth": false }


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
        scrollY: "calc(45vh)",
        processing: false,
        serverSide: false,
        responsive: false,
        fixedColumns: {
            leftColumns: 6
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
        //klo mw backgroud data
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


    //new $.fn.dataTable.FixedColumns(table, {
    //    leftColumns: 5
    //});
}

function clearTable() {
    let dt = $("#tableID").DataTable();
    dt.clear().draw(false);
    dt.columns.adjust();

}
/** ====== PREV & NEXT BUTTON ====== **/

function updateNavButton() {
    table.ajax.reload();
    /*    loadData();*/
    let ddl = $("#InspectionPointCode")[0];

    $(".btn-prev").prop("disabled", ddl.selectedIndex === 0);

    $(".btn-next").prop("disabled", ddl.selectedIndex === ddl.options.length - 1);

}

$(document).on("click", ".btn-next", function () {
    let ddl = $("#InspectionPointCode")[0];

    if (ddl.selectedIndex < ddl.options.length - 1) {
        ddl.selectedIndex++;
        $("#InspectionPointCode").trigger("change");
    }
});

$(document).on("click", ".btn-prev", function () {
    let ddl = $("#InspectionPointCode")[0];

    if (ddl.selectedIndex >= 1) {
        ddl.selectedIndex--;
        $("#InspectionPointCode").trigger("change");
    }
});

/** ====== END PREV & NEXT BUTTON ======*/

