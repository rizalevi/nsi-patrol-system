// ===============================
// GLOBAL VARIABLES
// ===============================
let dashboardData = null;
let statusChart = null;
let plantChart = null;
let equipmentChart = null;
var startRT = null;

// ===============================
// REALTIME CURRENT DATE AND TIME
// ===============================
function updateDateTime() {
    const now = new Date();

    const options = {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit',
        hour12: false
    };

    document.getElementById('currentDateTime').innerText =
        now.toLocaleString('en-US', options);
}

updateDateTime();
setInterval(updateDateTime, 1000); //counting time every second

//init 

initializeEvents();
onLoad();
/*startAutoRefresh();*/


function onLoad() {
    $("#globalLoading").stop(true, true).css("display", "flex").show().fadeIn(150);
    loadDashboard();
}

function startAutoRefresh() {
    if (startRT) clearInterval(startRT);

    startRT = setInterval(() => {
        console.log('refresh tiap 30000');
        loadDashboard();
    }, 30000);//0.5 menit interval
}

// ===============================
// LOAD DASHBOARD
// ===============================
function delay(ms = 0) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function loadDashboard() {
    $("#globalLoading").stop(true, true).css("display", "flex").show().fadeIn(150);
    try {
        const response = await $.ajax({
            url: buildServiceUrl("service/dashboard/overview"),
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({})
        });
        

        if (!response || !response.success || !response.data) {
            toastr.error("Failed to load dashboard data.");
            //$("#globalLoading").stop(true, true).fadeOut(200);
            //waitingDialog.hide();
            return;
        }

        dashboardData = response.data;

        await delay();
        renderSummaryProcess(dashboardData.SummaryProcess);

        await delay();
        renderProgressInspection(dashboardData.SummaryProgressInspection);
        //renderProgressInspectionDetails(dashboardData.ProgressInspectionDetails);
        await delay();
        renderRealtimeShift(dashboardData.RealtimeShifts || []);

        await delay();
        renderTopPlantTrouble(dashboardData.TopPlantAbnormal || []);

        await delay();
        renderTopEquipmentGroupTrouble(dashboardData.TopEquipmentGroupAbnormal || []);

    }
    catch (error) {
        console.log(error)
        toastr.error(error);
        // waitingDialog.hide();
        $("#globalLoading").stop(true, true).fadeOut(200);
    }
    finally {
        // waitingDialog.hide();
        $("#globalLoading").stop(true, true).fadeOut(200);
    }
}

// ===============================
// EVENT HANDLERS
// ===============================
function initializeEvents() {

    //$(document).on('click', '#viewAll, #box-remain', function () {
    //    $('#modalRemaining').modal('show');
    //    renderProgressInspectionDetails
    //    clearInterval(startRT);
    //});

    $(document).on('click', '#box-remain, #box-remain *', function () {
      
        $('#modalRemaining').modal('show');
        $("#ModalTitle2").html("Daily Remaining Inspection ");

        renderProgressInspectionDetails("remaining");
        clearInterval(startRT);
    });

    $(document).on('click', '#box-completed, #box-completed *', function () {
        $('#modalRemaining').modal('show');
        $("#ModalTitle2").html("Inspection Completed Today");

        renderProgressInspectionDetails("completed");
        clearInterval(startRT);
    });


    //$(document).on('click', '#viewAll2, #box-uncompleted', function () {
    //    $('#modaluncompleted').modal('show');
    //    clearInterval(startRT);

    //});

    $(document).on('click', '#viewAll-realtime', function () {
        $('#modalRealtime').modal('show');
        clearInterval(startRT);

    });


    $(document).on('click', '.btn-problem-detail', function (e) {
        e.preventDefault();
        if ($(this).data('problem') > 0) {
            const shift = $(this).data('shift');
            $('#modalRealtimeshiftDetailAbnormal').modal('show');
            $('#modalRealtime').modal('hide');
            $("#ModalTitle3").html("Realtime shift - detail problem");
            renderRealtimeShiftAbnormalDetails();
            clearInterval(startRT);

        }
    });

    $(document).on('click', '.modal-footer .btn[data-bs-dismiss="modal"]', function () {
        console.log('modal close → restart interval');
        startAutoRefresh();
    });

   

}

// ===============================
// SUMMARY PROCESS
// ===============================
const ctxDoughnut = document.getElementById('statusChart').getContext('2d');

function renderSummaryProcess(data) {
    if (!data) return;

    $('#lblStatusUncompleted').text(data.Uncompleted ?? 0);
    $('#lblStatusSendback').text(data.SendBack ?? 0);
    $('#lblStatusUnchecked').text(data.Unchecked ?? 0);
    $('#lblUnapprove').text(data.Unapproved ?? 0);
    /*$('#lblStatusCompleted').text(data.Completed ?? 0);*/

    const chartData = [
        data.Uncompleted ?? 0,
        data.SendBack ?? 0,
        data.Unchecked ?? 0,
        data.Unapproved ?? 0,
       /* data.Completed ?? 0*/
    ];

    if (statusChart) {
        statusChart.data.datasets[0].data = chartData;
        statusChart.update();
        return;
    }

   
    statusChart = new Chart(ctxDoughnut, {
        type: 'doughnut',
        data: {
            labels: ['Uncompleted', 'Send Back', 'Unchecked', 'Unapproved'], //, 'Completed'
            datasets: [{
                data: chartData,
                backgroundColor: ['#ff6b6b', '#f6c23e', '#f7971e', '#36b9cc'], //, '#1cc88a'
                borderWidth: 0
            }]
        },
        responsive: true,
        maintainAspectRatio: false,
        options: {
            cutoutPercentage: 40,
            legend: {
                display: false,
                position: 'right',
                onClick: function (e, legendItem) {
                    var index = legendItem.index; // 🔥 BUKAN datasetIndex
                    var ci = this.chart;
                    var meta = ci.getDatasetMeta(0);
                    // toggle slice
                    var hidden = meta.data[index].hidden;
                    meta.data[index].hidden = !hidden;

                    ci.update();
                }
            },
            animation: {
                onComplete: function () {
                    var chart = this.chart;
                    var ctx = chart.ctx;

                    var chartArea = chart.chartArea;

                    // 🔥 ambil titik tengah chart (bukan canvas)
                    var centerX = (chartArea.left + chartArea.right) / 2;
                    var centerY = (chartArea.top + chartArea.bottom) / 2;

                    ctx.save();

                    // 🔥 HITUNG TOTAL (VISIBLE SAJA)
                    var total = 0;
                    var meta = chart.getDatasetMeta(0);

                    meta.data.forEach(function (el, index) {
                        if (!el.hidden) {
                            total += chart.data.datasets[0].data[index];
                        }

                    });

                    // 🔹 FONT RESPONSIVE
                    var fontSize = (chart.height / 120).toFixed(2);

                    // 🔹 TOTAL
                    ctx.font = "bold " + fontSize + "em sans-serif";
                    ctx.fillStyle = "#333";
                    ctx.textAlign = "center";
                    ctx.textBaseline = "middle";

                    ctx.fillText((total.toLocaleString() == "0") ? "" : total.toLocaleString(), centerX, centerY);

                    // 🔹 LABEL
                    // ctx.font = (fontSize * 0.6) + "em sans-serif";
                    // ctx.fillStyle = "#999";

                    // ctx.fillText( centerX, centerY - 20);

                    ctx.restore();
                }
            }
        }
    });
}

//CUSTOM LEGEND CLICK BASE ON CARD-PANEL
document.querySelectorAll(".legend-item").forEach(function (el) {
    el.addEventListener("click", function () {

        var index = this.getAttribute("data-index");
        var meta = statusChart.getDatasetMeta(0);
        var element = meta.data[index];
        // 🔥 toggle visibility (sama kayak legend)
        element.hidden = !element.hidden;
        statusChart.update();
        // 🔥 efek visual (biar user tau lagi off)
        this.classList.toggle("inactive");
    });
});


//CUSTOM LEGEND CLICK BASE ON CARD-PANEL


// ===============================
// PROGRESS INSPECTION
// ===============================
function renderProgressInspection(data) {
    if (!data) return;

    $('#progress-remaining').text(data.Remaining ?? 0);
    $('#progress-completed').text(data.Completed ?? 0);
}


// ===============================
// DETAIL PROGRESS INSPECTION DATATABLE
// ===============================
function renderProgressInspectionDetails(sType) {
    $("#tableRemainDet").DataTable({
            destroy: true,
            ajax: {
                url: BaseUrl + "service/dashboard/list-detail",
                type: "POST",
                contentType: "application/json",
                xhrFields: {
                    withCredentials: true
                },
                data: function (d) {
                    return JSON.stringify({
                        Type: sType
                    });
                },
                dataSrc: function (json) {
                  
                   return json.data;
                }
            },

            columns: [
                {
                    data: null, className: "v-align", "autoWidth": false, width: "50", className: "text-center",
                    render: function (data, type, row, meta) {
                        return meta.row + 1;
                    }
                },
                {

                    data: "DocumentNo", width: "150",
                    render: function (data, type, row) {
                        
                        if (sType == "remaining") {
                            if (type === "display") {

                                let actionUrl = "";

                                if (row.Status === "Unapproved") {
                                    actionUrl = "/InspectionDataApproval/Task";
                                }
                                else if (row.Status === "Unchecked") {
                                    actionUrl = "/InspectionDataCheck/Task";
                                }
                                else {
                                    actionUrl = "/InspectionDataUpload/Task";
                                }

                                return ` <a href="${actionUrl}?ID=${encodeURIComponent(row.RedirectKey)}">${data}</a>`;
                            }

                            return data;

                            //if (type === "display") {

                            //    let actionUrl = "";
                            //    let menuID = "";
                            //    // 🔥 KONDISI STATUS
                            //    if (row.Status.toUpperCase() === "APPROVED" || row.Status.toUpperCase() === "UNAPPROVED") {
                            //        actionUrl = "/InspectionDataApproval/Detail";
                            //        menuID = "B03"
                            //    }

                            //    else if (row.Status.toUpperCase() === "CHECKED" || row.Status.toUpperCase() === "UNCHECKED") {

                            //        actionUrl = "/InspectionDataCheck/Detail";
                            //        menuID = "B02"
                            //    }
                            //    else {
                            //        actionUrl = "/InspectionDataUpload/Detail";
                            //        menuID = "B01"
                            //    }

                            //    return `
                            //        <form method="post" action="${actionUrl}" style="display:inline;">
                            //            <input type="hidden" name="ID" value="${menuID}" />
                            //            <input type="hidden" name="ClusterCode" value="${row.ClusterCode}" />
                            //            <input type="hidden" name="PlantCode" value="${row.PlantCode}" />
                            //            <input type="hidden" name="CourseCode" value="${row.CourseCode}" />
                            //            <input type="hidden" name="GroupCode" value="${row.GroupCode}" />
                            //            <input type="hidden" name="InspectionDate" value="${row.InspectionDate}" />
                            //            <input type="hidden" name="Frequency" value="${row.Frequency}" />
                            //            <button type="submit" class="btn btn-link p-0 m-0 align-baseline">
                            //                ${data}
                            //            </button>
                            //        </form>
                            //    `;
                            //}

                        } else {
                            return `${data}`;
                        }
                        return data;
                    }
                },
                { data: "CourseCode", "autoWidth": false, width: "130", },
                { data: "Title", "autoWidth": false, width: "350", },
                { data: "GroupName", "autoWidth": false, width: "170", },
                {
                    data: "InspectionDate", class: "text-center", width: "120", "autoWidth": false, render: function (data) {

                        if (!data || data === "0001-01-01T00:00:00") return "";
                        return formatDate(data);

                    },
                },
                { data: "Frequency", "autoWidth": false, width: "120", },
                { data: "InspectorUser", "autoWidth": false, width: "200", },
                { data: "Status", "autoWidth": false, width: "120" },
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
            scrollY: "calc(25vh)",
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
    
    //let rows = '';
   
    //details.forEach(s => {
    //    rows += `
    //        <tr>
    //            <td style="text-align:center;">${s.Course ?? ''}</td>
    //            <td style="text-align:center;">${s.Title ?? ''}</td>
    //            <td style="text-align:center;">${s.Group ?? ''}</td>
    //            <td style="text-align:center;">${formatDate(s.Date) ?? ''}</td >
    //        </tr>`;

    //});

    //$('#allRemainInspection').html(rows);
    //$('#AllRemainUncompleted').html(rows);
}


// ===============================
// REALTIME SHIFT
// ===============================
function renderRealtimeShift(shifts) {
    let rows = '';
    let detailRows = '';
  
    shifts.forEach(s => {
        rows += `
            <tr>
                <td style="text-align:center;">${s.ShiftCode ?? '-'}</td>
                <td style="text-align:center;">${s.Time ?? '-'}</td>
                <td style="text-align:center;">${s.Progress ?? '0%'}</td>
            </tr>`;

        detailRows += `
            <tr>
                <td style="text-align:center;">${s.ShiftCode ?? '-'}</td>
                <td style="text-align:center;">${s.Time ?? '-'}</td>
                <td style="text-align:center;">${s.Progress ?? '0%'}</td>
                <td style="text-align:center;">
                    <a href="#" class="btn-problem-detail" data-problem="${s.Problem}" data-shift="${s.ShiftCode}">
                        ${s.Problem ?? 0}
                    </a>
                </td>
                <td style="text-align:center;">${s.Overdue ?? ''}</td>
                <td style="text-align:center;">${s.Status ?? '-'}</td>
            </tr>`;
    });
    //console.log(shifts[0].PlantName);
    $("#lblrealtimeshiftPlant").text('🔔 Realtime Shift ' + shifts[0].PlantName);
    $("#ModalTitle").html("Shift for " + shifts[0].PlantName);
    $('#realtimeShiftBody').html(rows);
    $('#realtimeShiftDetailBody').html(detailRows);
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
            //console.log('plantName');

            const startX = xAxis.getPixelForTick(start);
            const endX = xAxis.getPixelForTick(end);
            const centerX = (startX + endX) / 2;

            ctx.fillText(
                plantName,
                centerX,
                xAxis.bottom + 5
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

function renderTopPlantTrouble(items) {

    // destroy chart lama kalau ada
    if (plantChart) {
        plantChart.destroy();
        plantChart = null;
    }

    const period = items.length > 0 ? items[0].Period : "";

    const labels = items.map(x => x.CourseName);
    const values = items.map(x => x.Count || 0);
    const colorCodes = items.map(x => x.ColorCode );
    //let cumulative = values; //calculateCumulativePercentage(values);

    $("#chartTitle").html(`
        <div class="chart-title">${period}</div>
    `);


    //START dari 0
    let cumulativePoints = [{ x: 0.1, y: 0 }];

    const total = values.reduce((sum, val) => sum + val, 0);
    let cumulative = 0;
   
    const step = 100 / items.length;
       
    ////point line tiap tengah2 bar
    //items.forEach((item, index) => {
    //    cumulative += item.Count || 0;

    //    cumulativePoints.push({
    //        x: (index * step) + (step / 2),
    //        y: Math.round((cumulative / total) * 100)
    //    });
    //});

    ////alternativ kalo mau line chart point diakhir tiap bar chart
    items.forEach((item, index) => {
        cumulative += item.Count || 0;

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
                        gradient.addColorStop(0.80, lightenColor(baseColor,45));

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

// ===============================
// TOP EQUIPMENT GROUP ABNORMAL
// ===============================
function renderTopEquipmentGroupTrouble(items) {

    // destroy chart lama kalau ada
    if (equipmentChart) {
        equipmentChart.destroy();
        equipmentChart = null;
    }
    const period = items.length > 0 ? items[0].Period : "";

    const labels = items.map(x => x.GroupCode+ ' - ' +x.GroupName || x.GroupCode);
    const values = items.map(x => x.Count || 0);

    $("#chartTitle2").html(`
        <div class="chart-title">${period}</div>
    `);

    //const cumulative = calculateCumulativePercentage(values);
    //START dari 0
    let cumulativePoints = [{ x: 0.1, y: 0 }];
    const total = values.reduce((sum, val) => sum + val, 0);
    let cumulative = 0;

    const plantGroups = items.map(x =>  x.PlantName);
   

    const step = 100 / items.length;
    items.forEach((item, index) => {
        cumulative += item.Count || 0;

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

    const ctx = document.getElementById('lineChart2').getContext('2d');
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
                            fontSize: 6,
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
                //yAxes: [
                //    {
                //        id: 'y-axis-1',
                //        position: 'left',
                //        ticks: {
                //            beginAtZero: true,
                //            //min: 0,
                //            //max: ,
                //            //stepSize: leftStep,
                //            fontSize: 8

                //        },
                //        scaleLabel: {
                //            display: true,
                //            labelString: 'Count Trouble'
                //        }
                //    },
                //    {
                //        id: 'y-axis-2',
                //        position: 'right',
                //        ticks: {
                //            min: 0,
                //            max: 100,
                //            // stepSize: rightStep,
                //            fontSize: 8,
                //            callback: function (value) {
                //                return value + "%";
                //            }
                //        },
                //        gridLines: {
                //            drawOnChartArea: false
                //        },
                //        scaleLabel: {
                //            display: true,
                //            labelString: 'Cumulative %'
                //        }
                //    }
                //]
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
                        return "Top Equipment Group Abnormal"; // data.labels[index]; // CourseCode
                    },
                    label: function (tooltipItem, data) {
                        const index = tooltipItem.index;

                        const plantGroups = plantChart.config.options.plantGroups || [];
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



// ===============================
// DETAIL ABNORMAL REALTIME SHIFT DATATABLE
// ===============================
$('#modalRealtimeshiftDetailAbnormal').on('shown.bs.modal', function () {

    let tableDet = $('#tableAbnormalDet').DataTable();

    tableDet.columns.adjust().draw();

});

function renderRealtimeShiftAbnormalDetails() {
    $("#tableAbnormalDet").DataTable({
        destroy: true,
        ajax: {
            url: BaseUrl + "service/dashboard/realtimeshift-detail-abnormal",
            type: "POST",
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            dataSrc: function (json) {
                return json.data;
            }
        },

        columns: [
            {
                data: null, className: "v-align", "autoWidth": false,  className: "text-center",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: "InspectorUser", "autoWidth": false },
            {
                data: "InspectionDate", class: "text-center",  "autoWidth": false, render: function (data) {

                    if (!data || data === "0001-01-01T00:00:00") return "";
                    return formatDate(data);

                },
            },
            /*{ data: "CourseName", "autoWidth": false, width: "350", },*/
            { data: "GroupName", "autoWidth": false, },
           /* { data: "Frequency", "autoWidth": false, width: "120", },*/
            { data: "InspectionPoint", "autoWidth": false,  },
            { data: "InspectionNo", "autoWidth": false, },

            { data: "Min", "autoWidth": false },
            { data: "Max", "autoWidth": false },
            { data: "InspectionResult", "autoWidth": false},

           
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
        scrollY: "calc(25vh)",
        processing: false,
        serverSide: false,
        responsive: false,
        //fixedColumns: {
        //    leftColumns: 5
        //},
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
