
var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;
let globalData = [];
let paretoChart = null;


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
            $("#CourseCode").trigger('change');

        }
    });

    clearTable();

});

//course
$(document).on("change", "#CourseCode", function () {
    debugger
    let desc = $("#CourseCode option:selected").data("desc");
    $("#CourseName").val((desc || "").trim());

    populateDDL({
        url: "service/inspectiondata/get-inspectionpoint-chart",
        dropdownId: "InspectionPointCode",
        model: {
            CourseCode: $("#CourseCode").val()
        },
        codeField: "Code",
        descField: "Description",
        descTextbox: "InspectionPointName",
        width: "180px",

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
});

//clear button
$(document).on("click", ".btn-clear", function () {
    // reset 
    init();
    $("#PlantCode").val('').trigger('change');
    $("#PlantName").val("");
    $("#CourseCode").val('').trigger('change');
    $("#CourseName").val("");
    $("#InspectionPointCode").val('').trigger('change');
    $("#InspectionPointName").val("");

    if (paretoChart) {
        paretoChart.destroy();
        paretoChart = null;
    }
    $('#chartTitle, #noDataMsg').empty();
    clearTable();
});

$('#tableID').on('xhr.dt', function (e, settings, json, xhr) {
    globalData = (json && json.data) ? json.data : [];

    setTimeout(() => {
        renderParetoChart(globalData);
        if (globalData.length > 0) {
            updateChartTitle();
        }
    }, 100);
});


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
            url: buildServiceUrl("service/topplanabnormality/getdatainspection"),
            type: "POST",
            contentType: "application/json",
            xhrFields: { withCredentials: true },
            data: function (d) {
                return JSON.stringify({
                    CourseCode: $("#CourseCode").val(),
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
                autoWidth: true,
                className: "text-center align-middle",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: "GroupName", autoWidth: true, className: "text-left" },
           /* { data: "InspectionCode", autoWidth: true, className: "text-left align-middle" },*/
            { data: "InspectionName", autoWidth: true, className: "text-left align-middle" },
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
    const plant = $("#PlantCode").val();
    const plantName = $("#PlantName").val();
    const dateFrom = $("#InspectionDateFrom").val();
    const dateTo = $("#InspectionDateTo").val();

    let subtitle = `
										${plant ? ` Inspection Point : ${plantName}` : ' Inspection Point : ALL'}
										${dateFrom && dateTo ? `<br>Period : ${dateFrom} ~ ${dateTo}` : ''}
									`;

    $("#chartTitle").html(`<div>${subtitle}</div>`);
//    $('#chartFooter').text('Plant Areas');
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

function renderParetoChart(rawData) {
    console.log(rawData);
    if (paretoChart) {
        paretoChart.destroy();
        paretoChart = null;
    }
    $('#noDataMsg').remove();

    if (!rawData || rawData.length < 2) {
        const ctx = document.getElementById('paretoChart');
        if (ctx) {
            const context = ctx.getContext('2d');
            context.clearRect(0, 0, ctx.width, ctx.height);
        }
        $('#chartTitle, #chartFooter').text('');
        $('#paretoChart').parent().append('<div id="noDataMsg" style="text-align:center;color:#999;margin-top:10px;">No sufficient data</div>');
        return;
    }

  
    //let data = rawData.map(x => ({
    //    label: x.InspectionName,
    //    value: parseInt(x.AbnormalFrequency || 0),
    //    color: x.ColorCode
    //}));

    //// hanya abnormal > 0
    //data = data.filter(x => x.value > 0);

    //// urutkan terbesar
    //data.sort((a, b) => b.value - a.value);

    //// ambil top 10
    //data = data.slice(0, 10);

    //if (data.length > 7) {
    //    const top6 = data.slice(0, 6);
    //    const rest = data.slice(6);
    //    const othersTotal = rest.reduce((sum, x) => sum + x.value, 0);
    //    top6.push({ label: 'Others', value: othersTotal });
    //    data = top6;
    //}
    
    //let cumulativePoints = [{ x: 0.1, y: 0 }];

    //const total = data.reduce((sum, x) => sum + x.value, 0);
    //let cumulative = 0;

    //const step = 100 / data.length;

    //////alternativ kalo mau line chart point diakhir tiap bar chart
    //data.forEach((item, index) => {
    //    cumulative += item.value || 0;

    //    cumulativePoints.push({
    //        x: (index + 1) * step,
    //        y: Math.round((cumulative / total) * 100)
    //    });
    //});


    const allData = rawData
        .map(x => ({
            label: x.InspectionName,
            value: parseInt(x.AbnormalFrequency || 0),
            color: x.ColorCode
        }))
        .filter(x => x.value > 0)
        .sort((a, b) => b.value - a.value);

    const totalAll = allData.reduce((sum, x) => sum + x.value, 0);

    // chart hanya top 10
    const data = allData.slice(0, 10);
    const labels = data.map(x => x.label);
    const values = data.map(x => x.value || 0);
    const colorCodes = data.map(x => x.color);


    let cumulativePoints = [{ x: 0.1, y: 0 }];

    const total = data.reduce((sum, x) => sum + x.value, 0);
    let cumulative = 0;

    const step = 100 / data.length;

    ////alternativ kalo mau line chart point diakhir tiap bar chart
    data.forEach((item, index) => {
        cumulative += item.value || 0;

        cumulativePoints.push({
            x: (index + 1) * step,
            y: Math.round((cumulative / total) * 100)
        });
    });

    //let cumulativePoints = [{ x: 0.1, y: 0 }];
    //let cumulative = 0;

    //data.forEach((item, index) => {
    //    cumulative += item.value;

    //    cumulativePoints.push({
    //        x: index + 1,
    //        y: ((cumulative / totalAll) * 100).toFixed(2)
    //    });
    //});

    if (allData.length > 10) {
        $('#chartFooter').text(
            `Showing Top 10 of ${allData.length} abnormal items`
        );
    }

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
                    usePointStyle: true,
                    pointStyle:'line',
                    datalabels: { display: false }
                },
                {
                    type: 'bar',
                    label: 'Frequency',
                    data: values,
                    yAxisID: 'y-axis-1',
                    order: 1,
                    usePointStyle: true,
                    pointStyle: 'rect',
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
                        font: { size: 10 },
                        formatter: function (value) {
                            return value > 0 ? value : '';
                        }
                    }
                }
                
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                xAxes: [
                    {
                        type: 'category',
                        barPercentage: 1.0,
                        categoryPercentage: 0.9,// 1.0,
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
                        gridLines: { display: false }
                    },
                    {
                        id: 'x-axis-2',
                        type: 'linear',
                        display: false,
                        ticks: { min: 0, max: 100 }
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
                        scaleLabel: { display: true, labelString: 'Frequency' }
                    },
                    {
                        id: 'y-axis-2',
                        position: 'right',
                        ticks: {
                            min: 0,
                            max: 100,
                            //stepSize: rightStep,
                            fontSize: 8,
                            callback: function (value) { return value + "%"; }
                        },
                        gridLines: { drawOnChartArea: false },
                        scaleLabel: { display: true, labelString: 'Cumulative %' }
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
            }
        }
    });
}

// =====================================================
// DOWNLOAD EXCEL - TOP PLAN ABNORMALITY BY INSPECTION
// =====================================================
function DownloadExcel() {
    var payload = {
        ClusterCode: $("#ClusterCode").val() || "ALL", // Tambahan "ALL" jika input kosong
        PlantCode: $("#PlantCode").val() || "ALL",     // Tambahan "ALL" jika input kosong
        CourseCode: $("#CourseCode").val() || "ALL",     // Tambahan "ALL" jika input kosong
        InspectionPointCode: $("#InspectionPointCode").val() || "ALL",
        InspectionDateFrom: formatDateGeneral($('#InspectionDateFrom').val()),
        InspectionDateTo: formatDateGeneral($('#InspectionDateTo').val())
    };

    var btn = $('#btnExcel');
    var originalText = btn.html();

    btn.html('<i class="fa fa-spinner fa-spin"></i> Loading...');
    btn.prop('disabled', true);

    fetch('/service/topplanabnormality/exportinspection', {
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
            a.download = "Top_Plan_Abnormality_Inspection.xlsx";

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
        toastr.warning("Please select plant code", 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    if ($("#CourseCode").val() == null || $("#CourseCode").val() == "") {
        toastr.warning("Please select course code", 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    //if ($("#InspectionPointCode").val() == null || $("#InspectionPointCode").val() == "") {
    //    toastr.warning("Please select inspection code", 'Warning', { timeOut: 3000, "closeButton": true });
    //    return false;
    //}


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