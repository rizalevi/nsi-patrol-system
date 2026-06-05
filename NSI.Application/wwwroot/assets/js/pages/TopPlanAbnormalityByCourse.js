
var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;
let globalData = [];
//let paretoChart = null;
let equipmentChart = null;

initPage();


// ==========================================
// EVENT LISTENERS
// ==========================================
$(document).on("click", ".datepicker-icon", function () {
    $(this).closest(".datepicker-wrapper")
        .find(".datepicker")
        .datepicker("show");
});

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


//cluster
$(document).on("change", "#ClusterCode", function () {
    let desc = $("#ClusterCode option:selected").data("desc");
    $("#ClusterName").val((desc || "").trim());
    clearTable()
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
        width: "180px",
        callback: function () {
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

            // $("#PlantCode").trigger('change');
        }
    });

    clearTable();

});

//search data
$(document).on("click", ".btn-search", function () {

    if (!validateData()) return;
    // waitingDialog.show("Please wait retrieving data...");

    if (table) {
        table.ajax.reload(null, false);

    } else {
        loadData();
    }

    loadChart();
});

//clear button
$(document).on("click", ".btn-clear", function () {
    // reset 
    initPage();
    $("#PlantCode").val('').trigger('change');
    $("#PlantName").val("");

    if (lineChart) {
        lineChart.destroy();
        lineChart = null;
    }
    $('#chartTitle, #noDataMsg').empty();
    clearTable();
});

//$('#tableID').on('xhr.dt', function (e, settings, json, xhr) {
//    globalData = (json && json.data) ? json.data : [];

//    setTimeout(() => {
//        renderParetoChart(globalData);
//        if (globalData.length > 0) {
//            updateChartTitle();
//        }
//    }, 100);
//});


// ==========================================
// INITIALIZATION
// ==========================================


function initPage() {
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
        width: "180px",
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
                width: "180px",
                callback: function () {
                    let ddl = $("#PlantCode");

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

                   // $("#PlantCode").trigger('change');
                }
            });

        }
    });

    loadData();

}

// ==========================================
// DATATABLES & API CALL
// ==========================================
function loadData() {
    if (table) {
        table.ajax.reload(null, false);
        return;
    }

    table = $("#tableID").DataTable({
        destroy: true,
        ajax: {
            url: buildServiceUrl("service/topplanabnormality/getdatacourse"),
            type: "POST",
            contentType: "application/json",
            xhrFields: { withCredentials: true },
            data: function (d) {
                return JSON.stringify({
                    ClusterCode: $("#ClusterCode").val(),
                    PlantCode: $("#PlantCode").val(),
                    CourseCode: $("#CourseCode").val(),
                    InspectionDateFrom: formatDateGeneral($('#InspectionDateFrom').val()),
                    InspectionDateTo: formatDateGeneral($('#InspectionDateTo').val())
                });
            },
            dataSrc: function (json) {
                return json.data || [];
            }
        },
        columns: [
            {
                data: null,
                autoWidth: true,
                className: "text-center align-middle",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: "CourseName", autoWidth: true, className: "align-middle" },
            { data: "GroupCode", autoWidth: true, className: "text-left align-middle" },
            { data: "GroupName", autoWidth: true, className: "text-left align-middle" },
            { data: "AbnormalFrequency", autoWidth: true, className: "text-right align-middle" }
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
        scrollY: "calc(45vh)",
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

// ==========================================
// CHARTING (Pareto)
// ==========================================
function updateChartTitle() {
    const course = $("#CourseCode").val();
    const courseName = $("#CourseName").val();
    const dateFrom = $("#InspectionDateFrom").val();
    const dateTo = $("#InspectionDateTo").val();

    let subtitle = `
								${course ? ` Course : ${courseName}` : ' Course : ALL'}
								${dateFrom && dateTo ? `<br>Period : ${dateFrom} ~ ${dateTo}` : ''}
							`;

    $("#chartTitle").html(`<div>${subtitle}</div>`);
    $('#chartFooter').text('Plant Areas');
}

function loadChart() {
    updateChartTitle();
    $.ajax({
        url: buildServiceUrl("service/topplanabnormality/getchart-bycourse"),
        type: "POST",
        contentType: "application/json",
        xhrFields: { withCredentials: true },
        data: JSON.stringify({
            ClusterCode: $("#ClusterCode").val(),
            PlantCode: $("#PlantCode").val(),
            CourseCode: $("#CourseCode").val(),
            InspectionDateFrom: formatDateGeneral($('#InspectionDateFrom').val()),
            InspectionDateTo: formatDateGeneral($('#InspectionDateTo').val())
        })
        , success: function (res) {
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

// ===============================
// TOP EQUIPMENT GROUP ABNORMAL
// ===============================

/*CUSTOM LABELS PLANT NAME DIBAWAH BAR CHART JIKA PLANT LEBIH DARI 1 (SEKARANG HARDCODE HANYA AA & SAP*/
const groupedXAxisPlugin = {
    afterDraw: function (chart) {
        const plantGroups = chart.config.options.plantGroups;
        if (!plantGroups || !plantGroups.length) return;

        const ctx = chart.ctx;
        const xAxis = chart.scales['x-axis-1'];
        if (!xAxis) return;

        ctx.save();
        ctx.textAlign = 'center';
        ctx.textBaseline = 'top';
        ctx.font = 'bold 11px Arial';
        ctx.fillStyle = '#444';

        let start = 0;

        while (start < plantGroups.length) {
            const plantName = plantGroups[start];
            let end = start;

            while (
                end + 1 < plantGroups.length &&
                plantGroups[end + 1] === plantName
            ) {
                end++;
            }
           
            const startX = xAxis.getPixelForTick(start);
            const endX = xAxis.getPixelForTick(end);
            const centerX = (startX + endX) / 2;

            ctx.fillText(
                plantName,
                centerX,
                xAxis.bottom + 25
            );

            start = end + 1;
        }

        ctx.restore();
    }
};
Chart.plugins.register(groupedXAxisPlugin);
/* END CUSTOM LABEL */
function renderChart(items) {

    // destroy chart lama kalau ada
    if (equipmentChart) {
        equipmentChart.destroy();
        equipmentChart = null;
    }
    const labels = items.map(x => x.GroupCode + ' - ' + x.GroupName || x.GroupCode);
    const values = items.map(x => x.AbnormalFrequency || 0);
    //const cumulative = calculateCumulativePercentage(values);
    //START dari 0
    let cumulativePoints = [{ x: 0.1, y: 0 }];
    const total = values.reduce((sum, val) => sum + val, 0);
    let cumulative = 0;

    const plantGroups = items.map(x => x.PlantName);


    const step = 100 / items.length;
    items.forEach((item, index) => {
        cumulative += item.AbnormalFrequency || 0;

        cumulativePoints.push({
            x: (index + 1) * step,
            y: Math.round((cumulative / total) * 100)
        });
    });


    if (equipmentChart) {
        equipmentChart.data.labels = labels;
        equipmentChart.data.datasets[0].data = values;
        equipmentChart.data.datasets[1].data = cumulative;
        equipmentChart.options.plantGroups = plantGroups;
        equipmentChart.config.options.plantGroups = plantGroups;
        equipmentChart.update();
        return;
    }

    const ctx = document.getElementById('lineChart').getContext('2d');
    //    var ctxLine = document.getElementById("lineChart2").getContext("2d");
    var gradientBlue = ctx.createLinearGradient(0, 0, 0, 420);
    // lebih solid di atas
    gradientBlue.addColorStop(0, "rgba(78, 115, 223, 0.9)");
    // mid biar tetap tebal
    gradientBlue.addColorStop(0.5, "rgba(78, 115, 223, 0.6)");
    // bawah baru fade
    gradientBlue.addColorStop(1, "rgba(78, 115, 223, 0.1)");

    equipmentChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Frequency',
                    data: values,
                    borderColor: '#4e73df',
                    backgroundColor: 'rgba(78,115,223,0.2)',
                    fill: true,
                    tension: 0.3,
                    pointRadius: 0,
                    borderWidth: 2,
                    yAxisID: 'y-axis-1',
                    xAxisID: 'x-axis-1',
                    datalabels: { display: false }
                },
                {
                    label: 'Cumulative (%)',
                    data: cumulativePoints,
                    borderColor: 'red',
                    backgroundColor: 'transparent',
                    fill: false,
                    borderWidth: 1,
                    pointRadius: 2,
                    yAxisID: 'y-axis-2',
                    xAxisID: 'x-axis-2',
                    order: 0,
                    datalabels: { display: false }
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plantGroups: plantGroups,
            layout: {
                padding: {
                    bottom: 40
                }
            },
            legend: {
                position: 'top',
                labels: {
                    generateLabels: function (chart) {
                        var original = Chart.defaults.global.legend.labels.generateLabels;
                        var labels = original.call(this, chart);

                        // 🔥 tetap urut: count dulu baru cumulative
                        labels.sort(function (a, b) {
                            var typeA = chart.data.datasets[a.datasetIndex].label;
                            var typeB = chart.data.datasets[b.datasetIndex].label;

                            if (typeA === 'Frequency') return -1;
                            if (typeB === 'Frequency') return 1;
                            return 0;
                        });

                        return labels;
                    }
                }
            },

            scales: {
                xAxes: [
                    {
                        id: 'x-axis-1',
                        gridLines: {
                            display: false
                        },
                        ticks: {
                            fontSize: 8,
                            autoSkip: false,
                            maxRotation: 30,
                            minRotation: 30,
                            callback: function (value) {

                                const maxLength = 20;
                                let words = value.split(" ");
                                let lines = [];
                                let currentLine = "";

                                words.forEach(word => {
                                    if ((currentLine + word).length > maxLength) {
                                        lines.push(currentLine.trim());
                                        currentLine = word + " ";
                                    } else {
                                        currentLine += word + " ";
                                    }
                                });

                                lines.push(currentLine.trim());
                                return lines;
                            }
                        },
                    },
                    {
                        id: 'x-axis-2',
                        type: 'linear',   // X LINEAR 0–100
                        display: false,
                        ticks: {
                            min: 0,
                            max: 100
                        }
                    }
                ],
             
                yAxes: [
                    {
                        id: 'y-axis-1',
                        position: 'left',
                        ticks: {
                            beginAtZero: true
                        },
                        scaleLabel: {
                            display: true,
                            labelString: 'Frequency'
                        }
                    },
                    {
                        id: 'y-axis-2',
                        position: 'right',
                        ticks: {
                            beginAtZero: true,
                            max: 100,
                            callback: function (value) {
                                return value + "%";
                            }
                        },
                        scaleLabel: {
                            display: true,
                            labelString: 'Cumulative (%)'
                        },
                        gridLines: {
                            drawOnChartArea: false
                        }
                    }
                ]
            },
            //tooltips: {
            //    mode: 'index',
            //    intersect: false
            //},
            tooltips: {
                callbacks: {
                    title: function (tooltipItems, data) {
                        const index = tooltipItems[0].index;
                        return "Top Plan Abnormal By Course"; // data.labels[index]; // CourseCode
                    },
                    label: function (tooltipItem, data) {
                        const index = tooltipItem.index;
                       
                        const plantGroups = equipmentChart.config.options.plantGroups || [];
                        const plantName = (plantGroups[index] || '').replace('Plant : ', '');

                        const groupName = data.labels[index];
                        const count = data.datasets[1].data[index].y;

                        return [
                            `Plant  : ${plantName}`,
                            `Group  : ${groupName}`,
                            `Count  : ${count}`
                        ];
                    }
                }
            },
            hover: {
                mode: 'index',
                intersect: false
            }
        }
    });
}


//function renderParetoChart(rawData) {
//    if (paretoChart) {
//        paretoChart.destroy();
//        paretoChart = null;
//    }
//    $('#noDataMsg').remove();

//    if (!rawData || rawData.length < 2) {
//        const ctx = document.getElementById('paretoChart');
//        if (ctx) {
//            const context = ctx.getContext('2d');
//            context.clearRect(0, 0, ctx.width, ctx.height);
//        }
//        $('#chartTitle, #chartFooter').text('');
//        $('#paretoChart').parent().append('<div id="noDataMsg" style="text-align:center;color:#999;margin-top:10px;">No sufficient data</div>');
//        return;
//    }

//    let data = rawData.map(x => ({
//        label: x.PlantName,
//        value: parseInt(x.AbnormalFrequency)
//    }));

//    data.sort((a, b) => b.value - a.value);

//    if (data.length > 7) {
//        const top6 = data.slice(0, 6);
//        const rest = data.slice(6);
//        const othersTotal = rest.reduce((sum, x) => sum + x.value, 0);
//        top6.push({ label: 'Others', value: othersTotal });
//        data = top6;
//    }

//    const total = data.reduce((sum, x) => sum + x.value, 0);
//    let cumulative = 0;
//    let xdata = 0;

//    const labels = [];
//    const values = [];
//    const cumulativePoints = [];

//    cumulativePoints.push({ x: 0, y: 0 });

//    data.forEach(x => {
//        cumulative += x.value;
//        labels.push(x.label);
//        values.push(x.value);
//        xdata += (100 / data.length);
//        cumulativePoints.push({
//            x: Math.ceil(xdata),
//            y: Math.ceil(cumulative / total * 100)
//        });
//    });

//    let maxValue = Math.max(...values);
//    let MaxtickX = typeof getDynamicScale === "function" ? getDynamicScale(maxValue) : maxValue + Math.ceil(maxValue * 0.1);
//    let gridCount = typeof getAutoGridCount === "function" ? getAutoGridCount(MaxtickX) : 5;

//    const leftStep = Math.ceil(MaxtickX / gridCount);
//    const rightStep = Math.ceil(100 / gridCount);

//    const ctx = document.getElementById('paretoChart');

//    paretoChart = new Chart(ctx, {
//        type: 'bar',
//        data: {
//            labels: labels,
//            datasets: [
//                {
//                    type: 'bar',
//                    label: 'Frequency',
//                    data: values,
//                    yAxisID: 'y-axis-1',
//                    backgroundColor: 'rgba(255, 99, 132, 0.5)',
//                    borderColor: 'rgba(255, 99, 132, 1)',
//                    borderWidth: 1,
//                    datalabels: {
//                        anchor: 'end',
//                        align: 'end',
//                        color: '#000',
//                        font: { size: 10 },
//                        formatter: function (value) {
//                            return value > 0 ? value : '';
//                        }
//                    }
//                },
//                {
//                    type: 'line',
//                    label: 'Cumulative %',
//                    data: cumulativePoints,
//                    xAxisID: 'x-axis-2',
//                    yAxisID: 'y-axis-2',
//                    borderColor: 'blue',
//                    fill: false,
//                    borderWidth: 1,
//                    pointRadius: 2,
//                    datalabels: { display: false }
//                }
//            ]
//        },
//        options: {
//            responsive: true,
//            maintainAspectRatio: false,
//            scales: {
//                xAxes: [
//                    {
//                        type: 'category',
//                        barPercentage: 1.0,
//                        categoryPercentage: 1.0,
//                        ticks: {
//                            fontSize: 8,
//                            autoSkip: false,
//                            maxRotation: 30,
//                            minRotation: 30,
//                            callback: function (value) {
//                                const maxLength = 20;
//                                let words = value.split(" ");
//                                let lines = [];
//                                let currentLine = "";

//                                words.forEach(word => {
//                                    if ((currentLine + word).length > maxLength) {
//                                        lines.push(currentLine.trim());
//                                        currentLine = word + " ";
//                                    } else {
//                                        currentLine += word + " ";
//                                    }
//                                });
//                                lines.push(currentLine.trim());
//                                return lines;
//                            }
//                        },
//                        gridLines: { display: false }
//                    },
//                    {
//                        id: 'x-axis-2',
//                        type: 'linear',
//                        display: false,
//                        ticks: { min: 0, max: 100 }
//                    }
//                ],
//                yAxes: [
//                    {
//                        id: 'y-axis-1',
//                        position: 'left',
//                        ticks: {
//                            beginAtZero: true,
//                            min: 0,
//                            max: MaxtickX,
//                            stepSize: leftStep,
//                            fontSize: 8
//                        },
//                        scaleLabel: { display: true, labelString: 'Frequency' }
//                    },
//                    {
//                        id: 'y-axis-2',
//                        position: 'right',
//                        ticks: {
//                            min: 0,
//                            max: 100,
//                            stepSize: rightStep,
//                            fontSize: 8,
//                            callback: function (value) { return value + "%"; }
//                        },
//                        gridLines: { drawOnChartArea: false },
//                        scaleLabel: { display: true, labelString: 'Cumulative %' }
//                    }
//                ]
//            }
//        }
//    });
//}

// ================================================
// DOWNLOAD EXCEL - TOP PLAN ABNORMALITY BY COURSE 
// ================================================
function DownloadExcel() {
    var payload = {
        ClusterCode: $("#ClusterCode").val() || "ALL", // Tambahan "ALL" jika input kosong
        PlantCode: $("#PlantCode").val() || "ALL",     // Tambahan "ALL" jika input kosong
        CourseCode: $("#CourseCode").val() || "ALL",
        InspectionDateFrom: formatDateGeneral($('#InspectionDateFrom').val()),
        InspectionDateTo: formatDateGeneral($('#InspectionDateTo').val())
    };

    var btn = $('#btnExcel');
    var originalText = btn.html();

    btn.html('<i class="fa fa-spinner fa-spin"></i> Loading...');
    btn.prop('disabled', true);

    fetch('/service/topplanabnormality/exportcourse', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    })
        .then(response => {
            if (!response.ok) throw new Error("Gagal mengunduh file dari server.");
            return response.blob();
        })
        .then(blob => {
            var a = document.createElement("a");
            a.href = window.URL.createObjectURL(blob);

            // [PERBAIKAN]: Ubah nama file yang akan tersimpan di komputer user
            a.download = "Top_Plan_Abnormality_Course.xlsx";

            document.body.appendChild(a);
            a.click();
            a.remove();
        })
        .catch(error => {
            // [PERBAIKAN]: Tampilkan alert agar user tahu jika gagal (bisa diganti dengan notifikasi library jika ada)
            //alert(error.message);
            console.log(error.message);
        })
        .finally(() => {
            btn.html(originalText);
            btn.prop('disabled', false);
        });
}
function clearTable() {
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
        toastr.warning("Please select inspection code", 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    if ($("#CourseCode").val() == null || $("#CourseCode").val() == "") {
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

    return true;
}