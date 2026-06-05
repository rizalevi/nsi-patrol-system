
var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;
let globalData = [];
let paretoChart = null;
let plantChart = null;

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
    init();
    $("#PlantCode").val('').trigger('change');
    $("#PlantName").val("");

    if (barChart) {
        barChart.destroy();
        barChart = null;
    }
    $('#chartTitle, #noDataMsg').empty();
    clearTable();
});

//$('#tableID').on('xhr.dt', function (e, settings, json, xhr) {
//    setTimeout(() => {
//        loadChart();
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

                    $("#PlantCode").trigger('change');
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
            url: buildServiceUrl("service/topplanabnormality/getdataplant"),
            type: "POST",
            contentType: "application/json",
            xhrFields: { withCredentials: true },
            data: function (d) {
                return JSON.stringify({
                    ClusterCode: $("#ClusterCode").val(),
                    PlantCode: $("#PlantCode").val(),
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
                className: "text-center align-middle",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
           /* { data: "PlantCode", className: "align-middle" },*/
            { data: "PlantName", className: "text-left" },
            { data: "CourseCode",  className: "text-left align-middle" },
            { data: "CourseName", className: "text-left align-middle" },
            { data: "AbnormalFrequency",  className: "text-right align-middle" },
            
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

// ==========================================
// CHARTING 
// ==========================================
function loadChart() {
    updateChartTitle();
    $.ajax({
        url: buildServiceUrl("service/topplanabnormality/getchart-byplant"),
        type: "POST",
        contentType: "application/json",
        xhrFields: { withCredentials: true },
        data: JSON.stringify({
                ClusterCode: $("#ClusterCode").val(),
                PlantCode: $("#PlantCode").val(),
                InspectionDateFrom: formatDateGeneral($('#InspectionDateFrom').val()),
                InspectionDateTo: formatDateGeneral($('#InspectionDateTo').val())
                })
        ,success: function (res) {
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

function updateChartTitle() {
    const plant = $("#PlantCode").val();
    const plantName = $("#PlantName").val();
    const dateFrom = $("#InspectionDateFrom").val();
    const dateTo = $("#InspectionDateTo").val();

    let subtitle = `
								
								${dateFrom && dateTo ? `<br>Period : ${dateFrom} ~ ${dateTo}` : ''}
							`;

    $("#chartTitle").html(`<div>${subtitle}</div>`);
    $('#chartFooter').text('Plant Areas');
}



// ===============================
// TOP PLANT ABNORMAL
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

/*
    FUNGSI UNTUK REPLACE COLOR BAR JADI GRADIENT
 */
function lightenColor(hex, percent) {
    hex = hex.replace('#', '');

    let r = parseInt(hex.substring(0, 2), 16);
    let g = parseInt(hex.substring(2, 4), 16);
    let b = parseInt(hex.substring(4, 6), 16);

    r = Math.min(255, Math.round(r + (255 - r) * percent / 100));
    g = Math.min(255, Math.round(g + (255 - g) * percent / 100));
    b = Math.min(255, Math.round(b + (255 - b) * percent / 100));

    return `rgb(${r}, ${g}, ${b})`;
}

function renderChart(items) {

    // destroy chart lama kalau ada
    if (plantChart) {
        plantChart.destroy();
        plantChart = null;
    }

    const labels = items.map(x => x.CourseName);
    const values = items.map(x => x.AbnormalFrequency || 0);
    const colorCodes = items.map(x => x.ColorCode || '#4e73df');
    //let cumulative = values; //calculateCumulativePercentage(values);
   
    //START dari 0
    let cumulativePoints = [{ x: 0.1, y: 0 }];

    const total = values.reduce((sum, val) => sum + val, 0);
    let cumulative = 0;

    const step = 100 / items.length;

    ////alternativ kalo mau line chart point diakhir tiap bar chart
    items.forEach((item, index) => {
        cumulative += item.AbnormalFrequency || 0;

        cumulativePoints.push({
            x: (index + 1) * step,
            y: Math.round((cumulative / total) * 100)
        });
    });



    const plantGroups = items.map(x => 'Plant : ' + x.PlantName);
    //console.log(plantGroups);   
    if (plantChart) {
        plantChart.data.labels = labels;
        plantChart.data.datasets[0].data = values;
        plantChart.data.datasets[1].data = cumulativePoints;
        //plantChart.data.datasets[0].backgroundColor = colorCodes; //pake custom bgcolor biar bisa gradient tapi base color tetep dari DB
        plantChart.options.plantGroups = plantGroups;
        plantChart.config.options.plantGroups = plantGroups;
        plantChart.update();
        return;
    }

    /*max tick axes left right*/
    const maxValue = Math.max(...values);
    const MaxtickX = getDynamicScale(maxValue);
    const gridCount = getAutoGridCount(MaxtickX);
    // kiri
    const leftStep = Math.ceil(MaxtickX / gridCount);

    // kanan (fix 100%)
    const rightMax = 100;
    const rightStep = Math.ceil(rightMax / gridCount);

    const ctx = document.getElementById('barChart').getContext('2d');

    plantChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    type: 'line',
                    label: 'Cumulative (%)',
                    data: cumulativePoints, //cumulative,
                    borderColor: '#E57373',
                    backgroundColor: '#E57373',
                    xAxisID: 'x-axis-2',
                    yAxisID: 'y-axis-2',
                    borderWidth: 1,
                    pointRadius: 2,
                    fill: false,
                    usePointStyle: true,
                    datalabels: { display: false },
                    order: 99,
                    //klo mau point diakhir barchart
                    pointRadius: function (context) {
                        return context.dataIndex === 0 ? 2 : 2;
                    },
                    pointHoverRadius: function (context) {
                        return context.dataIndex === 0 ? 2 : 2;
                    },

                },
                {
                    type: 'bar',
                    label: 'Abnormal',
                    data: values,
                    yAxisID: 'y-axis-1',
                    xAxisID: 'x-axis-1',
                    order: 1,
                    borderColor: 'blue',
                    usePointStyle: true,
                    //backgroundColor: colorCodes
                    backgroundColor: function (context) {
                        const chart = context.chart;
                        const { ctx, chartArea } = chart;

                        if (!chartArea) return '#4e73df';

                        const index = context.dataIndex;
                        const baseColor = colorCodes[index] || '#4e73df';

                        const gradient = ctx.createLinearGradient(
                            0,
                            chartArea.top,
                            0,
                            chartArea.bottom
                        );

                        gradient.addColorStop(0, lightenColor(baseColor, 0));
                        gradient.addColorStop(0.45, lightenColor(baseColor, 5));
                        gradient.addColorStop(0.80, lightenColor(baseColor, 45));

                        return gradient;
                    },

                },
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
                display: true,
                labels: {
                    usePointStyle: true,
                    generateLabels: function (chart) {

                        let labels = chart.data.datasets.map((dataset, i) => {

                            const isLine = dataset.type === 'line';

                            const barColor = '#e9e9e9';

                            return {
                                text: dataset.label,
                                fillStyle: isLine ? 'rgba(0,0,0,0)' : barColor,
                                strokeStyle: isLine ? dataset.borderColor : barColor,
                                lineWidth: isLine ? 3 : 1,
                                pointStyle: isLine ? 'line' : 'rect',
                                hidden: !chart.isDatasetVisible(i),
                                datasetIndex: i,
                                order: isLine ? 2 : 1
                            };
                        });

                        // Bar dulu, baru Line
                        labels.sort((a, b) => a.order - b.order);

                        return labels;
                    }
                }
            },

            //legend: {
            //    position: 'top',
            //    labels: {
            //        generateLabels: function (chart) {
            //            var original = Chart.defaults.global.legend.labels.generateLabels;
            //            var labels = original.call(this, chart);

            //            // Custom warna legend
            //            labels.forEach(function (label) {
            //                var dataset = chart.data.datasets[label.datasetIndex];

            //                if ((dataset.type || 'bar') === 'line') {
            //                    label.fillStyle = '#E57373';
            //                    label.strokeStyle = '#E57373';
            //                    label.lineWidth = 2;
            //                } else {
            //                    label.fillStyle = '#5DADE2';
            //                    label.strokeStyle = '#5DADE2';
            //                    label.lineWidth = 1;
            //                }
            //            });

            //            // Urutkan: bar dulu, lalu line
            //            labels.sort(function (a, b) {
            //                var typeA = chart.data.datasets[a.datasetIndex].type || 'bar';
            //                var typeB = chart.data.datasets[b.datasetIndex].type || 'bar';

            //                if (typeA === 'bar' && typeB === 'line') return -1;
            //                if (typeA === 'line' && typeB === 'bar') return 1;
            //                return 0;
            //            });

            //            return labels;
            //        }
            //    }
            //},
            scales: {
                xAxes: [
                    {
                        id: 'x-axis-1',
                        stacked: false,
                        barPercentage: 1.0,
                        categoryPercentage: 0.9, //kalo mau tidak ada space antar bar pake nilai 1.0
                        gridLines: {
                            offsetGridLines: false,
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
                        barPercentage: 1.0,
                        categoryPercentage: 1,
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
                            beginAtZero: true,
                            //max: MaxtickX,
                            //stepSize: leftStep,
                            fontSize: 8

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
                            min: 0,
                            max: 100,
                            /*stepSize: rightStep,*/
                            fontSize: 8,
                            callback: function (value) {
                                return value + "%";
                            }
                        },
                        gridLines: {
                            drawOnChartArea: false
                        },
                        scaleLabel: {
                            display: true,
                            labelString: 'Cumulative %'
                        }
                    }
                ]

            },
            tooltips: {
                callbacks: {
                    title: function (tooltipItems, data) {
                        const index = tooltipItems[0].index;
                        return "Abnormal By Plant"; // data.labels[index]; // CourseCode
                    },
                    label: function (tooltipItem, data) {
                        const index = tooltipItem.index;

                        const plantGroups = plantChart.config.options.plantGroups || [];
                        const plantName = (plantGroups[index] || '').replace('Plant : ', '');

                        const courseCode = data.labels[index];
                        const count = data.datasets[1].data[index];

                        return [
                            `Plant  : ${plantName}`,
                            `Course : ${courseCode}`,
                            `Count  : ${count}`
                        ];
                    }
                }
            }
        }
    });
}


//function renderChart(rawData) {
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

// =========================================
// DOWNLOAD EXCEL - TOP PLAN ABNORMALITY
// =========================================
function DownloadExcel() {
    var payload = {
        ClusterCode: $("#ClusterCode").val() || "ALL", // Tambahan "ALL" jika input kosong
        PlantCode: $("#PlantCode").val() || "ALL",     // Tambahan "ALL" jika input kosong
        InspectionDateFrom: formatDateGeneral($('#InspectionDateFrom').val()),
        InspectionDateTo: formatDateGeneral($('#InspectionDateTo').val())
    };

    var btn = $('#btnExcel');
    var originalText = btn.html();

    btn.html('<i class="fa fa-spinner fa-spin"></i> Loading...');
    btn.prop('disabled', true);

    fetch('/service/topplanabnormality/exportplant', {
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
            a.download = "Top_Plan_Abnormality_Plant.xlsx";

            document.body.appendChild(a);
            a.click();
            a.remove();
        })
        .catch(error => {
            // [PERBAIKAN]: Tampilkan alert agar user tahu jika gagal (bisa diganti dengan notifikasi library jika ada)
            
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