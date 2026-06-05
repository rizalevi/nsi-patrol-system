$('thead th').css({
    'background-color': '#3276b1',
    'color': 'white',
    'border': '2px solid #ccc'

});


let searchTimers = {};

$(document).on("keyup", ".dt-search", function () {
    debugger
    let keyword = $(this).val();

    let tableId = $(this).data("table");

    let table = $(tableId);

    if (!$.fn.DataTable.isDataTable(table)) {
        return;
    }

    let dt = table.DataTable();

    dt.search(keyword).draw();

});


/*RECALC DATATABLE WIDTH*/
$(window).on('resize', function () {
    var dt1 = $("#tableID").DataTable();
    dt1.columns.adjust().draw();

});
$(document).on('click', '[data-toggle="app-sidebar-minify"]', function () {
    setTimeout(function () {
        var dtTbl = $("#tableID").DataTable();
        dtTbl.columns.adjust();
    }, 100);
});
