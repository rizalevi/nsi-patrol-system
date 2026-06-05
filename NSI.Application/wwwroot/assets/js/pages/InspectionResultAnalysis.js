var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;

let globalData = [];
let lineChart = null;


init();

if (groupCode != "" && inspectionCode != "") {
    setTimeout(function () {
        $(".btn-search").trigger("click");
    }, 500); // delay 0.5 detik

} else {
    loadData();
    loadChart();
}



document.querySelectorAll(".tab").forEach(tab => {
    tab.addEventListener("click", function () {

        // remove active semua tab
        document.querySelectorAll(".tab").forEach(t => t.classList.remove("active"));
        document.querySelectorAll(".tab-content").forEach(c => c.classList.remove("active"));

        // aktifkan tab yg diklik
        this.classList.add("active");
        document.getElementById(this.dataset.tab).classList.add("active");

        setTimeout(function () {
            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        }, 100);


    });
});

// ==========================================
// EVENT LISTENERS
// ==========================================

//date picker
$(document).on("click", ".datepicker-icon", function () {
    $(this).closest(".datepicker-wrapper")
        .find(".datepicker")
        .datepicker("show");
});

$(document).on("change", "#CourseCode", function () {
    let desc = $("#CourseCode option:selected").data("desc");
    $("#CourseName").val((desc || "").trim());

    populateDDL({
        url: "service/inspectiondata/get-inspectionpoint-chart",
        dropdownId: "InspectionPointCode", //ddl id
        model: {
            CourseCode: $("#CourseCode").val()
        },
        codeField: "Code",
        descField: "Description",
        descTextbox: "InspectionPointName",
        width: "180px",
        selectedValue: inspectionCode,
        //callback: function () {

        //    if ($("#GroupCode").val() != null) {
        //        let ddl = $("#InspectionPointCode");

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

        //    }

        //    $("#InspectionPointCode").trigger('change');
        //}
    });

    

    $("#tableID").dataTable().fnClearTable();
});

$(document).on("change", "#InspectionPointCode", function () {
    let desc = $("#InspectionPointCode option:selected").data("desc");
    $("#InspectionPointName").val((desc || "").trim());
  
    updateNavButton();
  
    $("#tableID").dataTable().fnClearTable();
});


//search data
$(document).on("click", ".btn-search", function () {
       
    if (!validateData()) return;

    //waitingDialog.show("Please wait retrieving data...");

    if (table) {
        table.ajax.reload(null, false);

    } else {
        loadData();
    }
    loadChart();
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
    $("#CourseCode").val('').trigger('change');
    $("#CourseName").val("");
    $("#InspectionPointCode").val('').trigger('change');
    $("#InspectionPointName").val("");
    GlobalDatepicker.setDefault("#InspectionDateFrom", 0);
    GlobalDatepicker.setDefault("#InspectionDateTo", 2);

    $("#tableID").dataTable().fnClearTable();
});

$(document).on("submit", "#formBack", function () {
    $('#CourseCodeHdn').val(courseCode);
    $('#GroupCodeHdn').val(groupCode);
    $('#InspectionDateFromHdn').val($('#InspectionDateFrom').val());
    $('#InspectionDateToHdn').val($('#InspectionDateTo').val());

    $("#InspectionDateFromHdn").datepicker('setDate', $('#InspectionDateFrom').val()).val(formatDate($('#InspectionDateFrom').val()));
    $("#InspectionDateToHdn").datepicker('setDate', $('#InspectionDateTo').val()).val(formatDate($('#InspectionDateTo').val()));

});

//export excel
$(document).on("click", ".btn-excel", function () {

    let btn = $(".btn-excel");
    btn.prop("disabled", true).text("Exporting...");

    let model = {
        CourseCode: $("#CourseCode").val(),
        InspectionPointCode: $("#InspectionPointCode").val(),
        InspectionDateFrom: formatDateGeneral($('#InspectionDateFrom').val()),
        InspectionDateTo: formatDateGeneral($('#InspectionDateTo').val()),
        CourseName : $("#CourseName").val(),
        InspectionPointName: $("#InspectionPointName").val()
    };

    fetch('/InspectionResultAnalysis/ExportExcel', {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(model)
    })
    .then(response => {
       
            if (!response.ok) {
                toastr.warning("Failed retrieve data", 'Warning', { timeOut: 3000, "closeButton": true });
                return;
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
function validateData() {


    if ($("#CourseCode").val() == null || $("#CourseCode").val() == "") {
        toastr.warning("Please select course code", 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    if ($("#InspectionPointCode").val() == null || $("#InspectionPointCode").val() == "") {
        toastr.warning("Please select inspection code", 'Warning', { timeOut: 3000, "closeButton": true });
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

    // 🔴 Validasi 2: Harus dalam bulan & tahun yang sama
    if (
        fromDate.getMonth() !== toDate.getMonth() ||
        fromDate.getFullYear() !== toDate.getFullYear()
    ) {
        toastr.warning("Please set date cannot exceed 30 days", 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    return true;
}
function loadData() {
    if (table) {
        table.ajax.reload(null, false);
        return;
    }

    table = $("#tableID").DataTable({
        destroy: true,
        deferLoading: 0, // 🔥 penting
        ajax: {
            //url: BaseUrl + "service/inspectiondatainquiry/list",
            url: buildServiceUrl("service/inspectionresultanalysis/list"),
            type: "POST",
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: function (d) {
                return JSON.stringify({
                    CourseCode: $("#CourseCode").val(),
                    InspectionPointCode: $("#InspectionPointCode").val(),
                    InspectionDateFrom: formatDateGeneral($('#InspectionDateFrom').val()),
                    InspectionDateTo: formatDateGeneral($('#InspectionDateTo').val())
                });
            },
            dataSrc: function (json) {
                globalData = json.data; 
                

                return json.data; 
            },
            //complete: function () {
            //    setInterval(
            //        waitingDialog.hide(), 500);
            //},error: function () {
            //    waitingDialog.hide();
            //}
        },

        columns: [
            {
                data: null, className: "v-align", "autoWidth": true, className: "text-center",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: "InspectionPoint", "autoWidth": true },
            {
                data: "Date", "autoWidth": true, className: "text-center", render: function (data) {
                    return formatDate(data);

                },
            },
            { data: "Frequency", "autoWidth": true, className: "text-center"},
            { data: "Value", "autoWidth": true, className: "text-right" },

        ],
        autoWidth: false,
        searching: true,
        ordering: true,
        paging: false,
        info: true,
        bSort: true,
        bLengthChange: true,
        pageLength: 25,
        scrollX: true,
        scrollY: "calc(40vh)",
        processing: false,
        serverSide: false,
        responsive: false,
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
      

    });
}
function init() {
  
    GlobalDatepicker.init(".datepicker");

    if (groupCode != "" && inspectionCode != "") {
        //let parsedFrom = $.fn.datepicker.DPGlobal.parseDate(
        //    inspectionDateFrom,
        //    "dd M yyyy",
        //    "en"
        //);
        //let parsedTo = $.fn.datepicker.DPGlobal.parseDate(
        //    inspectionDateTo,
        //    "dd M yyyy",
        //    "en"
        //);

        const parsedFrom = new Date(inspectionDateFrom);
        const parsedTo = new Date(inspectionDateTo);
       


        $("#InspectionDateFrom").datepicker('setDate', formatDate(parsedFrom));
        $("#InspectionDateTo").datepicker('setDate', formatDate(parsedTo));

        //$("#InspectionDateFrom").datepicker('setDate', inspectionDateFrom).val(formatDate(inspectionDateFrom)).prop("disabled", true);
        //$("#InspectionDateTo").datepicker('setDate', inspectionDateTo).val(formatDate(inspectionDateTo)).prop("disabled", true);


        $("#CourseCode").prop("disabled", true);
        $("#InspectionPointCode").prop("disabled", true);


        $(".btn-back").removeClass("hidden");
        $(".btn-back").addClass("show");


    } else {
       
        let now = new Date();
        let firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
        let today = new Date();

        $("#InspectionDateFrom").datepicker('setDate', firstDay);
        $("#InspectionDateTo").datepicker('setDate', today);


        $("#InspectionDateFrom").prop("disabled", false);
        $("#InspectionDateTo").prop("disabled", false);


        $("#CourseCode").prop("disabled", false);
        $("#InspectionPointCode").prop("disabled", false);

    }

    $.ajax({
        //url: BaseUrl + "service/tasklist/get-usergroup",
        url: buildServiceUrl("service/inspectionresultanalysis/get-userplant"),
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        async: false,
        success: function (res) {
            $("#PlantCode").val(res.data[0].PlantCode);
        }
    });
  
    //course
    populateDDL({
        url: "service/inspectiondata/get-course",
        dropdownId: "CourseCode",
        model: {
            PlantCode: $("#PlantCode").val()
        },
        codeField: "Code",
        descField: "Description",
        descTextbox: "CourseName",
        width: "180px",
        selectedValue:courseCode,
        callback: function () {

            if (courseCode != "" && courseCode != null) {
                $("#CourseCode").trigger('change');
            }
        }
    });


   
}
function updateChartTitle() {

    const course = $("#CourseCode").val();
    const courseName = $("#CourseName").val();

    const inspectionPointName = $("#InspectionPointName").val();

    const dateFrom = $("#InspectionDateFrom").val();
    const dateTo = $("#InspectionDateTo").val();


    let subtitle = `
        ${inspectionPointName}
        ${dateFrom && dateTo ? `<br>Period : ${dateFrom} ~ ${dateTo}` : ''}
    `;

    $("#chartTitle").html(`
        <div class="chart-title">${subtitle}</div>
    `);
    $('#chartFooter').text('Inspection Point');

}
function loadChart() {
    updateChartTitle();
    $.ajax({
        url: buildServiceUrl("service/inspectionresultanalysis/chart"),
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            CourseCode: $("#CourseCode").val(),
            InspectionPointCode: $("#InspectionPointCode").val(),
            InspectionDateFrom: formatDateGeneral($('#InspectionDateFrom').val()),
            InspectionDateTo: formatDateGeneral($('#InspectionDateTo').val())
        }),
        success: function (res) {
            const data = res.data || res;
            renderChart(data);
        },
        //complete: function () {
        //    setInterval(
        //        waitingDialog.hide(), 500);
        //}, error: function () {
        //    console.error("Chart load error:", err);
        //    waitingDialog.hide();
        //}
    });

}

function getUnitName() {
    var UnitName ="";
    $.ajax({
        url: buildServiceUrl("service/inspectionresultanalysis/get-unitname"),
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            CourseCode: $("#CourseCode").val(),
            InspectionPointCode: $("#InspectionPointCode").val(),
            InspectionDateFrom: formatDateGeneral($('#InspectionDateFrom').val()),
            InspectionDateTo: formatDateGeneral($('#InspectionDateTo').val())
        }),
        async: false,
        success: function (res) {
            UnitName = (res.data[0].UnitName);
        }
    });

    return UnitName;
}

Chart.plugins.register({
    beforeDraw: function (chart) {

        const ctx = chart.ctx;

        let bgColor = getComputedStyle(document.documentElement)
            .getPropertyValue('--bs-component-bg')
            .trim();

        if (!bgColor) {
            bgColor = '#fff';
        }

        ctx.save();

        ctx.fillStyle = bgColor;

        ctx.fillRect(
            0,
            0,
            chart.width,
            chart.height
        );

        ctx.restore();
    }
});

function renderChart(data) {

    /**
     * ============================
     * NO DATA
     * ============================
     */
    const validData = data.filter(x =>
        x.InspectionResult !== null &&
        x.InspectionResult !== undefined &&
        x.InspectionResult !== ''
    );


    if(!validData || validData.length === 0) {
        if (window.myChart) {
            window.myChart.destroy();
            window.myChart = null;
        }

        const canvas = document.getElementById('lineChart');

        if (canvas) {
            const context = canvas.getContext('2d');

            context.clearRect(
                0,
                0,
                canvas.width,
                canvas.height
            );
        }
       

        $('#noDataMsg').remove();
       
        $('#lineChart').parent().append(`
            <div id="noDataMsg"
                 style="
                    text-align:center;
                    color:#999;
                    margin-top:-200px;
                 ">
                 No sufficient data
            </div>
        `);

        return;
    }

    $('#noDataMsg').remove();

    /**
     * ============================
     * ORIGINAL DECIMAL DATA
     * ============================
     */

    const labels = data.map(x => x.Label);
    const inspectionData = data.map(x => Number(x.InspectionResult));
    const minLine = data.map(x => Number(x.MinValue) );
    const maxLine = data.map(x => Number(x.MaxValue) );

    /**
     * ============================
     * ABNORMAL POINTS
     * ============================
     */
    const abnormalArea = [];
    inspectionData.forEach((v, i) => {

        if (
            v > maxLine[i] ||
            v < minLine[i]
        ) {

            abnormalArea.push(v);

        } else {

            abnormalArea.push(null);
        }

    });

    //inspectionData.forEach((v, i) => {

    //    if (v < minLine[i] || v > maxLine[i])
    //    {
    //        abnormalPoints.push({
    //            x: labels[i],
    //            y: v
    //        });

    //    }

    //});

    /**
     * ============================
     * Y AXIS RANGE
     * ============================
     */

    const allValues = [

        ...inspectionData,
        ...minLine,
        ...maxLine

    ];

    const minY =Math.min(...allValues);
    const maxY =Math.max(...allValues);

    /**
     * tambah padding kecil
     */

    const padding = (maxY - minY) * 0.15;

    /**
     * IMPORTANT:
     * supaya decimal asli tetap bagus
     */

    const suggestedMin =Number( (minY - padding).toFixed(5) );
    const suggestedMax =Number( (maxY + padding).toFixed(5) );

    /**
     * ============================
     * DESTROY OLD CHART
     * ============================
     */

    if (window.myChart) {
        window.myChart.destroy();
    }

    /**
     * ============================
     * SAFE RANGE PLUGIN
     * ============================
     */

    Chart.plugins.unregister(
        window.safeRangePlugin
    );

    window.safeRangePlugin = {

        beforeDraw: function (chart) {

            var ctx = chart.chart.ctx;
            var yAxis = chart.scales['y-axis-0'];
            var xAxis = chart.scales['x-axis-0'];

            if (!yAxis || !xAxis)
                return;

            ctx.save();

            for (let i = 0; i < minLine.length; i++) {

                let min = yAxis.getPixelForValue(minLine[i]);
                let max = yAxis.getPixelForValue(maxLine[i]);
                let left =
                    i === 0
                        ? xAxis.left
                        : xAxis.getPixelForTick(i - 0.5);

                let right =
                    i === labels.length - 1
                        ? xAxis.right
                        : xAxis.getPixelForTick(i + 0.5);

                /**
                 * SAFE RANGE AREA
                 */

                ctx.fillStyle = 'rgba(40,167,69,0.15)';

                ctx.fillRect(
                    left,
                    max,
                    right - left,
                    min - max
                );
            }

            ctx.restore();
        }
    };

    Chart.plugins.register(
        window.safeRangePlugin
    );

    /**
     * ============================
     * CREATE CHART
     * ============================
     */

    const ctx = document.getElementById('lineChart').getContext('2d');

    window.myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [
                /**
                 * INSPECTION RESULT
                 */
                {
                    label: 'Inspection Result',
                    data: inspectionData,
                    borderColor: '#007bff',
                    backgroundColor: '#007bff',
                    borderWidth: 2,
                    lineTension: 0.30,
                    pointRadius: 1,
                    pointHoverRadius: 3,
                    pointBackgroundColor: '#fff',
                    pointBorderWidth: 3,
                    fill: false,
                    datalabels: {
                        display: false
                    },
                    usePointStyle: true,
                    pointStyle: 'line',

                },

                /**
                 * MAX LINE
                 */
                {
                    label: 'Max',
                    data: maxLine,
                    borderColor: '#dc3545',
                    borderWidth: 1.5,
                    borderDash: [2, 2],
                    pointRadius: 0,
                   
                    fill: false,
                    datalabels: {
                        display: false
                    }
                },

                /**
                 * MIN LINE
                 */
                {
                    label: 'Min',
                    data: minLine,
                    borderColor: '#28a745',
                    borderWidth: 1.5,
                    borderDash: [2, 2],
                    pointRadius: 0,
                    fill: false,
                    datalabels: {
                        display: false
                    }
                },
                               
                /**
                * ABNORMAL POINTS VERSI BLOCK LINECHART AREA
                */
                //{
                //    label: 'Abnormal',
                //    type: 'scatter',
                //    data: abnormalPoints,
                //    backgroundColor: 'rgba(255, 99, 132, 0.5)',
                //    borderColor: 'rgba(255, 99, 132, 1)',
                //    pointRadius: 2,
                //    pointHoverRadius: 3,
                //    datalabels: {
                //        display: false
                //    }
                //},
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            legend: {
                display: true,
                position: 'top',
                labels: {
                    usePointStyle: true,
                    generateLabels: function (chart) {
                        var datasets = chart.data.datasets;

                        return datasets.map(function (dataset, i) {
                            var isInspectionResult =
                                dataset.label === 'Inspection Result';

                            return {
                                text: dataset.label,
                                fillStyle: dataset.backgroundColor ,
                                strokeStyle: dataset.borderColor,
                                lineWidth: isInspectionResult
                                    ? (dataset.borderWidth || 2)
                                    : 1,

                                pointStyle: isInspectionResult
                                    ? 'line'
                                    : 'rect',
                                hidden: !chart.isDatasetVisible(i),
                                datasetIndex: i
                            };
                        });
                    }
                }
            },
            
            title: {
                display: true,
                text: 'Inspection Chart'
            },
            tooltips: {
                mode: 'index',
                intersect: false,
                callbacks: {
                    label: function (tooltipItem, chartData) {
                        const datasetLabel =
                            chartData.datasets[
                                tooltipItem.datasetIndex
                            ].label || '';

                        const value =
                            Number(
                                tooltipItem.yLabel
                            ).toFixed(5);

                        /**
                         * =========================
                         * INSPECTION RESULT STATUS
                         * =========================
                         */

                        if (
                            datasetLabel ===
                            'Inspection Result'
                        ) {
                            const index =
                                tooltipItem.index;
                            const min =
                                Number(minLine[index]);
                            const max =
                                Number(maxLine[index]);

                            let status =
                                'Normal';

                            if (
                                tooltipItem.yLabel < min ||
                                tooltipItem.yLabel > max
                            ) {

                                status =
                                    'Abnormal';
                            }

                            return [
                                datasetLabel +
                                ': ' +
                                value,

                                'Status : ' +
                                status
                            ];
                        }

                        return (
                            datasetLabel +
                            ': ' +
                            value
                        );
                    }
                }
            },

            //tooltips: {
            //    mode: 'index',
            //    intersect: false,
            //    callbacks: {
            //        label: function (tooltipItem, chartData) {
            //            let label =
            //                chartData.datasets[
            //                    tooltipItem.datasetIndex
            //                ].label || '';

            //            let value =
            //                Number(
            //                    tooltipItem.yLabel
            //                ).toFixed(5);

            //            return label + ': ' + value;
            //        }
            //    }
            //},
            hover: {
                mode: 'nearest',
                intersect: true
            },
            scales: {
                yAxes: [{
                    ticks: {
                        suggestedMin: suggestedMin,
                        suggestedMax: suggestedMax,
                        fontSize: 10,
                        callback: function (value) {
                            return Number(value)
                                .toFixed(5);
                        }
                    },
                    gridLines: {
                        display: true,
                        color:'rgba(0,0,0,0.05)',
                        drawBorder: true
                    },
                    scaleLabel: {
                        display: true,
                        labelString: getUnitName(),
                        fontSize: 12,
                        fontStyle: 'bold'
                    }

                }],

                xAxes: [{
                    ticks: {
                        maxRotation: 45,
                        minRotation: 45,
                        fontSize: 10
                    },
                    gridLines: {
                        display: false,
                        drawBorder: true
                    }
                }]
            },
            elements: {
                line: {
                    tension: 0.35
                }
            }
        }
    });
}
//versi awal
//function renderChart(data) {
  
//    if (!data || data.length === 0) {

//        // destroy chart lama kalau ada
//        if (lineChart) {
//            lineChart.destroy();
//            lineChart = null;
//        }

//        // optional: clear canvas
//        const ctx = document.getElementById('lineChart');
//        if (ctx) {
//            const context = ctx.getContext('2d');
//            context.clearRect(0, 0, ctx.width, ctx.height);
//        }

//        $('#chartTitle').text('');
//        $('#chartFooter').text('');

//        $('#noDataMsg').remove();

//        $('#lineChart').parent().append(
//            '<div id="noDataMsg" style="text-align:center;color:#999;margin-top:10px;">No sufficient data</div>'
//        );

//        return;
//    }

//    //hapus pesan kalau sebelumnya ada
//    $('#noDataMsg').remove();
//    // 🔹 mapping dari SP
//    const labels = data.map(x => x.Label);
//    const inspectionData = data.map(x => x.InspectionResult);
//    const minLine = data.map(x => x.MinValue);
//    const maxLine = data.map(x => x.MaxValue);

//    const ctx = document.getElementById('lineChart').getContext('2d');
        
//    // ambil nilai tertinggi dari semua dataset
//    var maxData = Math.max(
//        ...inspectionData,
//        ...maxLine
//    );

//    // 🔥 tambahin spare 10% (bisa kamu ubah)
//    var sparePercent = 0.2;
//    var paddedMax = maxData * (1 + sparePercent);

//    // bulatkan pakai fungsi kamu
//    var MaxtickX = getDynamicScale(maxData);
//    if (MaxtickX == maxData) {
//        MaxtickX = MaxtickX + (MaxtickX*0.2)
//    }

//    // optional: hitung grid
//    var gridCount = getAutoGridCount(MaxtickX);
//    const leftStep = Math.ceil(MaxtickX / gridCount);

//    // destroy chart lama biar ga numpuk
//    if (window.myChart) {
//        window.myChart.destroy();
//    }

//    window.myChart = new Chart(ctx, {
//        type: 'line',
//        data: {
//            labels: labels,
//            datasets: [
//                {
//                    label: 'Inspection Result',
//                    data: inspectionData,
//                    borderColor: '#007bff',
//                    borderWidth: 1,
//                    lineTension: 0,
//                    pointRadius: 2,
//                    fill: false,
//                    datalabels: { display: false }
//                },
//                {
//                    label: 'Min',
//                    data: minLine,
//                    borderColor: '#28a745',
//                    borderWidth: 1,
//                    pointRadius: 1,
//                    lineTension: 0,
//                    //borderDash: [5, 5],
//                    fill: false,
//                    datalabels: { display: false }
//                },
//                {
//                    label: 'Max',
//                    data: maxLine,
//                    borderColor: '#dc3545',
//                    borderWidth: 1,
//                    pointRadius: 1,
//                    lineTension: 0,
//                    //borderDash: [5, 5],
//                    fill: false,
//                    datalabels: { display: false }
//                }
//            ]
//        },
//        options: {
//            responsive: true,
//            maintainAspectRatio: false,
//            legend: {
//                display: true,
//                position: 'top'
//            },

//            title: {
//                display: true,
//                text: 'Inspection Chart'
//            },

//            scales: {
//                yAxes: [{
//                    ticks: {
//                        min: 0,//Math.floor(Math.min(...inspectionData) / 10) * 10,
//                        max: MaxtickX,//Math.ceil(Math.max(...inspectionData) / 10) * 10,
//                        stepSize: leftStep,
//                        fontSize: 10
//                    },
//                    gridLines: {
//                        display: true,
//                        drawBorder: true
//                    },
//                    scaleLabel: {
//                        display: true,
//                        labelString: getUnitName(), 
//                        fontSize: 12,
//                        fontStyle: 'bold'
//                    }
//                }],
//                xAxes: [{
//                    ticks: {
//                        maxRotation: 30,
//                        minRotation: 45,
//                        fontSize: 10
//                    },
//                    gridLines: {
//                        display: false,
//                        drawBorder: true
//                    },
                   
//                }]
//            },

//            elements: {
//                line: {
//                    tension: 0
//                }
//            }
//        }
//    });
//}

/**update color*/
const observer = new MutationObserver(() => {

    Object.values(Chart.instances).forEach(chart => {

        const textColor = getChartTextColor();

        // X AXIS
        if (chart.options.scales?.x?.ticks) {
            chart.options.scales.x.ticks.color = textColor;
        }

        // Y AXIS
        if (chart.options.scales?.y?.ticks) {
            chart.options.scales.y.ticks.color = textColor;
        }

        // LEGEND
        if (chart.options.plugins?.legend?.labels) {
            chart.options.plugins.legend.labels.color = textColor;
        }

        // TITLE
        if (chart.options.plugins?.title) {
            chart.options.plugins.title.color = textColor;
        }

        chart.update();

    });

});

observer.observe(document.documentElement, {
    attributes: true,
    attributeFilter: ['data-bs-theme']
});

observer.observe(document.documentElement, {
    attributes: true,
    attributeFilter: ['data-bs-theme']
});

function getChartTextColor() {

    return getComputedStyle(document.documentElement)
        .getPropertyValue('--bs-body-color')
        .trim();

}



/** ====== PREV & NEXT BUTTON ====== **/

function updateNavButton() {
    loadData();
    loadChart();

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

