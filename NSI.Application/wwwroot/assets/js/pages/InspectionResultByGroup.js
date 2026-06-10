var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;

let globalData = [];
let paretoChart = null;

init();

if (clusterCode != "" && plantCode != "") {
    setTimeout(function () {
        $(".btn-search").trigger("click");
    }, 500); // delay 0.5 detik
} else {
    $('#tableID').DataTable().clear().draw();
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
});


//export excel
$(document).on("click", ".btn-excel", function () {

    let btn = $(".btn-excel");
    btn.prop("disabled", true).text("Exporting...");

    let model = {
        ClusterCode: $("#ClusterCode").val(),
        PlantCode: $("#PlantCode").val(),
        InspectionDateFrom: formatDateGeneral($('#InspectionDateFrom').val()),
        InspectionDateTo: formatDateGeneral($('#InspectionDateTo').val()),
        ClusterName: $("#ClusterName").val(),
        PlantName: $("#PlantName")
    };

    fetch('/InspectionResultSummaryByGroup/ExportExcel', {
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

//clear button
$(document).on("click", ".btn-clear", function () {
    // reset 
    init();
    $("#PlantCode").val('').trigger('change');
    $("#PlantName").val("");

    clearTable();
    if (paretoChart) {
        paretoChart.destroy();
        paretoChart = null;
    }
    $('#chartTitle, #noDataMsg').empty();
});

//biar ga berat saat load chart data tetp 1 request 1 api
$('#tableID').on('xhr.dt', function (e, settings, json, xhr) {

    globalData = json.data;
    //if (!json || !json.data || json.data.length === 0) {
    //    toastr.info("No data available in table", 'Info', {
    //        timeOut: 3000,
    //        closeButton: true
    //    });
    //}
    setTimeout(() => {
        if (globalData.length >= 4) {
            renderParetoChart(globalData);
            updateChartTitle();
        } else {
            renderParetoChart(globalData); // biar clear
        }
    }, 100);
});
function loadData() {
    if (table) {
        table.ajax.reload(null, false);
        return;
    }
    table = $("#tableID").DataTable({
        destroy: true,
        ajax: {
            //url: BaseUrl + "service/inspectiondatainquiry/list",
            url: BaseUrl + "service/inspectionresultbygroup/list" ,
            type: "POST",
            contentType: "application/json",
            xhrFields: {
                withCredentials: true
            },
            data: function (d) {
                return JSON.stringify({
                    ClusterCode: $("#ClusterCode").val(),
                    PlantCode: $("#PlantCode").val(),
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
            //}, error: function () {
            //    waitingDialog.hide();
            //}
        },

        columns: [
            {
                data: null, className: "v-align", "autoWidth": false, className: "text-center",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                data: "CourseName", "autoWidth": false,
                render: function (data, type, row) {

                    if (type === "display") {
                        let actionUrl = "/InspectionResultSummaryByItem/Detail";

                        let datefrom = formatDateGeneral($("#InspectionDateFrom").val());
                        let dateto = formatDateGeneral($("#InspectionDateTo").val());
                        let clusterCode = $("#ClusterCode").val();
                        let plantCode = $("#PlantCode").val();
                        let menuID = "C05";

                        return `
                                <form method="post" action="${actionUrl}" style="display:inline;">
                                    <input type="hidden" name="ID" value="${menuID}" />
                                    <input type="hidden" name="ClusterCode" value="${clusterCode}" />
                                    <input type="hidden" name="PlantCode" value="${plantCode}" />
                                    <input type="hidden" name="CourseCode" value="${row.CourseCode}" />
                                    <input type="hidden" name="GroupCode" value="${row.GroupCode}" />
                                    <input type="hidden" name="InspectionDateFrom" value="${datefrom}" />
                                    <input type="hidden" name="InspectionDateTo" value="${dateto}" />
                                    <button type="submit" class="btn btn-link p-0 m-0 align-baseline" title="Go to detail">
                                         ${data}
                                    </button>
                                </form>
                            `;
                    }

                    return data;
                }
            },
            {
                data: "GroupName", "autoWidth": false,
                render: function (data, type, row) {

                    if (type === "display") {
                        let actionUrl = "/InspectionResultSummaryByItem/Detail";

                        let datefrom = formatDateGeneral($("#InspectionDateFrom").val());
                        let dateto = formatDateGeneral($("#InspectionDateTo").val());
                        let clusterCode = $("#ClusterCode").val();
                        let plantCode = $("#PlantCode").val();
                        let menuID = "C05";

                        return `
                                <form method="post" action="${actionUrl}" style="display:inline;">
                                    <input type="hidden" name="ID" value="${menuID}" />
                                    <input type="hidden" name="ClusterCode" value="${clusterCode}" />
                                    <input type="hidden" name="PlantCode" value="${plantCode}" />
                                    <input type="hidden" name="CourseCode" value="${row.CourseCode}" />
                                    <input type="hidden" name="GroupCode" value="${row.GroupCode}" />
                                    <input type="hidden" name="InspectionDateFrom" value="${datefrom}" />
                                    <input type="hidden" name="InspectionDateTo" value="${dateto}" />
                                    <button type="submit" class="btn btn-link p-0 m-0 align-baseline" title="Go to inspection by item">
                                        ${data}
                                    </button>
                                </form>
                            `;
                    }

                    return data;
                }
            },
            { data: "AbnormalFrequency", "autoWidth": false, className:"text-right" },
            //{

            //    data: null,
            //    orderable: false,
            //    className:"text-center",
            //    render: function (data, type, row) {

            //        if (type === "display") {
            //            let actionUrl = "/InspectionResultSummaryByItem/Detail";

            //            let datefrom = formatDateGeneral($("#InspectionDateFrom").val());
            //            let dateto = formatDateGeneral($("#InspectionDateTo").val());
            //            let clusterCode = $("#ClusterCode").val();
            //            let plantCode = $("#PlantCode").val();
            //            let menuID = "C05";

            //            return `
            //                    <form method="post" action="${actionUrl}" style="display:inline;">
            //                        <input type="hidden" name="ID" value="${menuID}" />
            //                        <input type="hidden" name="ClusterCode" value="${clusterCode}" />
            //                        <input type="hidden" name="PlantCode" value="${plantCode}" />
            //                        <input type="hidden" name="CourseCode" value="${row.CourseCode}" />
            //                        <input type="hidden" name="GroupCode" value="${row.GroupCode}" />
            //                        <input type="hidden" name="InspectionDateFrom" value="${datefrom}" />
            //                        <input type="hidden" name="InspectionDateTo" value="${dateto}" />
            //                        <button type="submit" class="btn btn-link p-0 m-0 align-baseline">
            //                            View Detail
            //                        </button>
            //                    </form>
            //                `;
            //        }

            //        return data;
            //    }
            //},

        ],

        autoWidth: false,
        searching: true,
        ordering: true,
        paging: false,
        info: true,
        bSort: true,
        bLengthChange: false,
        pageLength: 25,
        scrollX: false,
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
            <'col-sm-5'i>\
            <'col-sm-7 d-flex justify-content-end align-items-center gap-3'lp>\ >"
        ,

    });
}

function init() {
    GlobalDatepicker.init(".datepicker");
    if (clusterCode != "" && plantCode != "") {

        const parsedFrom = new Date(inspectionDateFrom);
        const parsedTo = new Date(inspectionDateTo);


        $("#InspectionDateFrom").datepicker('setDate', formatDate(parsedFrom));
        $("#InspectionDateTo").datepicker('setDate', formatDate(parsedTo));
        //$("#InspectionDateFrom").datepicker('setDate', formatDate(inspectionDateFrom)).val(formatDate(inspectionDateFrom)).prop("disabled",true);
        //$("#InspectionDateTo").datepicker('setDate', formatDate(inspectionDateTo)).val(formatDate(inspectionDateTo)).prop("disabled", true);

        $("#ClusterCode").prop("disabled", true);
        $("#PlantCode").prop("disabled", true);

      
        $(".btn-back").removeClass("hidden");
        $(".btn-back").addClass("show");
    } else {
       
        let now = new Date();
        let firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
        let today = new Date();

        $("#InspectionDateFrom").datepicker('setDate', firstDay);
        $("#InspectionDateTo").datepicker('setDate', today);

        $("#ClusterCode").prop("disabled", false);
        $("#PlantCode").prop("disabled", false);

        $("#InspectionDateFrom").prop("disabled", false);
        $("#InspectionDateTo").prop("disabled", false);

        
    }
    //DDL Cluster
    populateDDL({
        url: "service/inspectiondata/get-cluster",
        dropdownId: "ClusterCode",
        codeField: "Code",
        descField: "Description",
        descTextbox: "ClusterName",
        selectedValue: clusterCode,
        /*width: "180px",*/
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
                selectedValue:plantCode,
                callback: function () {
                    $("#PlantCode").trigger('change');
                }
            });

        }
    });

    loadData();
}

function updateChartTitle() {

    const plant = $("#PlantCode").val();
    const plantName = $("#PlantName").val();

    //const course = $("#CourseCode").val();
    //const courseName = $("#CourseName").val();

    const dateFrom = $("#InspectionDateFrom").val();
    const dateTo = $("#InspectionDateTo").val();

    //let title = `Inspection Result Analysis`;

    let subtitle = `
        ${plant ? ` Plant : ${plantName}` : ''}
        ${dateFrom && dateTo ? `<br>Period : ${dateFrom} ~ ${dateTo}` : ''}
    `;

    $("#chartTitle").html(`
        <div class="chart-title">${subtitle}</div>
    `);
    $('#chartFooter').text('Equipment Group');

}
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

//pareto chart sama seperti versi sebelumnya
function renderParetoChart(rawData) {
    
    if (!rawData || rawData.length < 4) {

        // destroy chart lama kalau ada
        if (paretoChart) {
            paretoChart.destroy();
            paretoChart = null;
        }

        // optional: clear canvas
        const ctx = document.getElementById('paretoChart');
        if (ctx) {
            const context = ctx.getContext('2d');
            context.clearRect(0, 0, ctx.width, ctx.height);
        }

        $('#chartTitle').text('');
        $('#chartFooter').text('');

        $('#noDataMsg').remove();

        $('#paretoChart').parent().append(
            '<div id="noDataMsg" style="text-align:center;color:#999;margin-top:10px;">No sufficient data</div>'
        );
        
        return; 
    }

    //hapus pesan kalau sebelumnya ada
    $('#noDataMsg').remove();

    if (paretoChart) {
        paretoChart.destroy();
    }

    let data = rawData.map(x => ({
        label: x.GroupName,
        value: parseInt(x.AbnormalFrequency),
        color: x.ColorCode
    }));

    data.sort((a, b) => b.value - a.value);



    // kalo mau fix chart bar hanya 7 bar kaya pareto pada umumnya
    if (data.length > 7) {

        const top6 = data.slice(0, 6);
        const rest = data.slice(6);

        const othersTotal = rest.reduce((sum, x) => sum + x.value, 0);

        top6.push({
            label: 'Others',
            value: othersTotal
        });

        data = top6;
    }

    const labels = data.map(x => x.label);
    const values = data.map(x => x.value || 0);
    const colorCodes = data.map(x => x.color);

    let cumulativePoints = [{ x: 0.1, y: 0 }];

    const total = data.reduce((sum, x) => sum + x.value, 0);
    let cumulative = 0;

    const step = 100 / data.length;

    ////point line tiap tengah2 bar
    //items.forEach((item, index) => {
    //    cumulative += item.Count || 0;

    //    cumulativePoints.push({
    //        x: (index * step) + (step / 2),
    //        y: Math.round((cumulative / total) * 100)
    //    });
    //});

    ////alternativ kalo mau line chart point diakhir tiap bar chart
    data.forEach((item, index) => {
        cumulative += item.value || 0;

        cumulativePoints.push({
            x: (index + 1) * step,
            y: Math.round((cumulative / total) * 100)
        });
    });

    //const total = data.reduce((sum, x) => sum + x.value, 0);

    //let cumulative = 0;
    //let xdata = 0;

    //const labels = [];
    //const values = [];
    //const cumulativePoints = [];

    ////START dari 0 
    //cumulativePoints.push({ x: 0, y: 0 });

    //data.forEach(x => {
    //    cumulative += x.value;

    //    labels.push(x.label);
    //    values.push(x.value);

    //    //bagi rata 0–100
    //    xdata += (100 / data.length);

    //    cumulativePoints.push({
    //        x: Math.ceil(xdata),
    //        y: Math.ceil(cumulative / total * 100)
    //    });
    //});

    /*max tick axes left right*/
    //const maxValue = Math.max(...values);
    //const MaxtickX = getDynamicScale(maxValue);
    //const gridCount = getAutoGridCount(MaxtickX);
    //// kiri
    //const leftStep = Math.ceil(MaxtickX / gridCount);

    //// kanan (fix 100%)
    //const rightMax = 100;
    //const rightStep = Math.ceil(rightMax / gridCount);

    const ctx = document.getElementById('paretoChart');

    paretoChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    type: 'line',
                    label: 'Cumulative %',
                    data: cumulativePoints,
                    xAxisID: 'x-axis-2',
                    yAxisID: 'y-axis-2',
                    borderColor: '#E57373',
                    backgroundColor: '#E57373',
                    fill: false,
                    borderWidth: 1,
                    pointRadius: 2,
                    order: 99,
                    datalabels: { display: false }
                },
                {
                    type: 'bar',
                    label: 'Frequency',
                    data: values,
                    yAxisID: 'y-axis-1',
                    xAxisID: 'x-axis-1',
                    order: 1,
                    //backgroundColor: 'rgba(255, 99, 132, 0.5)',
                    //borderColor: 'rgba(255, 99, 132, 1)',
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
                    borderWidth: 1,
                    datalabels: {
                        anchor: 'end',
                        align: 'end',
                        color: '#000',
                        font: {
                            size: 10
                        },
                        formatter: function (value) {
                            return value > 0 ? value : '';
                        }
                    }
                },
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                xAxes: [
                    {
                        id: 'x-axis-1',
                        type: 'category',
                        barPercentage: 1.0,
                        categoryPercentage: 0.9,
                        //offset:false,
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
                        gridLines: {
                            display: false
                        }
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
                            beginAtZero: true,
                            min: 0,
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
                           // stepSize: rightStep,
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
            tooltips: {
                callbacks: {
                    title: function (tooltipItems, data) {
                        const index = tooltipItems[0].index;
                        return "Inspection Result By Group"; // data.labels[index]; // CourseCode
                    },
                    label: function (tooltipItem, data) {
                        const index = tooltipItem.index;

                        const group = data.labels[index];
                        const count = data.datasets[1].data[index];

                        return [
                            `Group : ${group}`,
                            `Count : ${count}`
                        ];
                    }
                }
            }
        }
    });
}

//kalo mau point style line chart ditengah bar
function renderParetoChart2(rawData) {

    if (paretoChart) {
        paretoChart.destroy();
    }

    let data = rawData.map(x => ({
        label: x.InspectionPoint,
        value: parseInt(x.AbnormalFrequency)
    }));

    // sort desc
    data.sort((a, b) => b.value - a.value);

    const total = data.reduce((sum, x) => sum + x.value, 0);

    let cumulative = 0;
    const labels = [];
    const values = [];
    const cumulativePercent = [];

    data.forEach(x => {
        cumulative += x.value;

        labels.push(x.label);
        values.push(x.value);
        cumulativePercent.push((cumulative / total * 100).toFixed(2));
    });

    const ctx = document.getElementById('paretoChart');

    paretoChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    type: 'bar',
                    label: 'Frequency',
                    data: values,
                    yAxisID: 'y-axis-1',
                    backgroundColor: 'rgba(255, 99, 132, 0.5)', // 🔥 pink
                    borderColor: 'rgba(255, 99, 132, 1)',

                },
                {
                    type: 'line',
                    label: 'Cumulative %',
                    data: cumulativePercent,
                    yAxisID: 'y-axis-2',
                    borderColor: 'blue',
                    fill: false,
                    borderWidth: 1,
                    pointRadius: 2,
                    datalabels: { display: false }
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false, // 🔥 WAJIB biar ikut container
            scales: {
                xAxes: [{
                    
                    ticks: {
                        fontSize:8,
                        autoSkip: false,
                        maxRotation: 30,
                        minRotation: 30,
                        callback: function (value) {

                            const maxLength = 20; // panjang per baris
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

                            return lines; // 🔥 ini bikin jadi multi-line
                        }
                    },
                    gridLines: {
                        offsetGridLines: false,
                        display: false
                    }
                }],
                yAxes: [
                    {
                        id: 'y-axis-1',
                        position: 'left',
                        ticks: {
                            fontSize: 8,
                            beginAtZero: true,
                            min: 0,
                            max:100,
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
                            fontSize: 8,
                            beginAtZero: true,
                            min:0,
                            max: 100,
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
            }
        }
    });
}

function validateData() {


    if ($("#ClusterCode").val() == null || $("#ClusterCode").val() == "") {
        toastr.warning("Please select cluster code", 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    if ($("#PlantCode").val() == null || $("#PlantCode").val() == "") {
        toastr.warning("Please select plant code", 'Warning', { timeOut: 3000, "closeButton": true });
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

function clearTable() {
    let dt = $("#tableID").DataTable();
    dt.clear().draw(false);
    dt.columns.adjust();

}
