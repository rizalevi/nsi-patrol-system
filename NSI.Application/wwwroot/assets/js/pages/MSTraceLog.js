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

$(document).on("click", "#btnSearch", function () {
	if (table) {
		table.ajax.reload(null, false);

	} else {
		loadData();
	}
});

$(document).on("change", "#ModuleFilter", function () {
	let desc = $("#ModuleFilter option:selected").data("desc");
	$("#ModuleNameFilter").val((desc || "").trim());


	//var moduleDesc = $(this).find("option:selected").data("desc");
	//if (moduleDesc) {
	//	$('#ModuleNameFilter').val(moduleDesc);
	//} else {
	//	$('#ModuleNameFilter').val($(this).find("option:selected").text());
	//}
});

// Helper for Smart Search Box to filter Datatables globally
// $('#tableSearch').on('keyup', function () {
// 	if (table) {
// 		table.search(this.value).draw();
// 	}
// });

// setPanelHeight();
// setTimeout(adjustTableHeight, 100);

// $(window).resize(function () {
// 	setPanelHeight();
// 	adjustTableHeight();
// });


// =========================================
// NOTIFICATION FUNCTION (GANTI ALERT)
// =========================================
//function showNotif(message, type = 'info') {
//	var icon = '';

//	if (type === 'success') icon = 'fa-check-circle';
//	else if (type === 'danger') icon = 'fa-times-circle';
//	else if (type === 'warning') icon = 'fa-exclamation-triangle';
//	else icon = 'fa-info-circle';

//	var html = `
//							<div class="alert alert-${type} alert-dismissible fade in" role="alert" style="border-radius: 6px;">
//								<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
//								<i class="fa ${icon}"></i> ${message}
//							</div>
//						`;

//	$('#notifArea').html(html);

//	setTimeout(function () {
//		$('.alert').fadeOut('slow', function () {
//			$(this).remove();
//		});
//	}, 3000);
//}

// =========================================
// LOAD COMBO MODULE
// =========================================
function LoadFilterModule() {

	populateDDL({
		url: "service/mstracelog/getcombo",
		dropdownId: "ModuleFilter",
		codeField: "ModuleID",
		descField: "Description",
		descTextbox: "ModuleNameFilter",
		selectedValue:"ALL",
		width: "150px"
	});

	$("#ModuleFilter").val("ALL").trigger('change.select2');


	// $.ajax({
	// 	url: '/service/mstracelog/getcombo',
	// 	type: 'POST',
	// 	contentType: 'application/json',
	// 	data: JSON.stringify({}),
	// 	success: function (res) {
	// 		var $dropdown = $('#ModuleFilter');
	// 		// $dropdown.empty().append('<option value="ALL">ALL</option>');

	// 		if (res.data) {
	// 			$.each(res.data, function (i, item) {
	// 				$dropdown.append($('<option>', {
	// 					value: item.ModuleID,
	// 					text: item.ModuleID,
	// 					"data-desc": item.Description,
	// 					title: item.ModuleID,
	// 					"data-content": "<span>" + item.ModuleID + " - " + (item.Description || '') + "</span>"
	// 				}));
	// 			});
	// 		}

	// 		$dropdown.selectpicker('refresh');
	// 	},
	// 	error: function () {
	// 		showNotif("Failed load module list.", "danger");
	// 	}
	// });
}

// =========================================
// INIT 
// =========================================
function init() {

	GlobalDatepicker.init(".datepicker");
	// Set default date ke hari ini
	let now = new Date();
	let firstDay = new Date(now.getFullYear(), now.getMonth(), 1);
	let today = new Date();

	$("#StartDateFilter").datepicker('setDate', firstDay);
	$("#EndDateFilter").datepicker('setDate', today);

	LoadFilterModule();
	//InitDataTable();
	loadData();
}

// =========================================
// LOAD GRID
// =========================================
function loadData() {
	
	if (table) {
		table.ajax.reload(null, false);
		return;
	}

	table = $('#tableID').DataTable({
		destroy: true,
		ajax: {
			url: buildServiceUrl("service/mstracelog/list"),
			type: "POST",
			contentType: "application/json",
			xhrFields: {
				withCredentials: true
			},
			data: function (d) {
				return JSON.stringify({
					Module: $('#ModuleFilter').val() || "ALL",
					LogDateFrom: formatDateGeneral($('#StartDateFilter').val()),
					LogDateTo: formatDateGeneral($('#EndDateFilter').val())
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
				data: "LogDate",
				className: "text-center",
				render: function (data) {
					if (!data || data === "0001-01-01T00:00:00") return "";
					return formatDateTime(data);
				}
			},
			{ data: "UserID", className: "text-center" },
			{ data: "IPAddress", className: "text-center" },
			{
				data: "Module",
				render: function (data, type, row) {
					return row.ModuleID + (data ? ' - ' + data : '');
				}
			},
			{ data: "Action", className: "text-center" },
			{ data: "EventLog" }
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
		scrollY: "calc(105vh - 500px)",
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

// =========================================
// DOWNLOAD EXCEL
// =========================================
function DownloadExcel() {
	var payload = {
		Module: $('#ModuleFilter').val() || "ALL",
		ModuleName: $('#ModuleNameFilter').val() || "ALL",
		LogDateFrom: formatDateGeneral($('#StartDateFilter').val()),
		LogDateTo: formatDateGeneral($('#EndDateFilter').val())
	};

	var btn = $('#btnExcel');
	var originalText = btn.html();

	btn.html('<i class="fa fa-spinner fa-spin"></i> Loading...');
	btn.prop('disabled', true);

	fetch('/service/mstracelog/export', {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify(payload)
	})
		.then(response => {
			if (!response.ok) throw new Error("Gagal mengunduh file.");
			return response.blob();
		})
		.then(blob => {
			var a = document.createElement("a");
			a.href = window.URL.createObjectURL(blob);
			a.download = "Trace Log Master.xlsx";
			document.body.appendChild(a);
			a.click();
			a.remove();
			toastr.success("Download excel success!", 'success', { timeOut: 3000, "closeButton": true });
		})
		.catch(error => {
			toastr.warning(error.message, 'warning', { timeOut: 3000, "closeButton": true });

			//showNotif(error.message, "danger");
		})
		.finally(() => {
			btn.html(originalText);
			btn.prop('disabled', false);
		});
}


function setPanelHeight() {
	var windowHeight = window.innerHeight;

	// Ambil jarak dari atas halaman ke panel
	var panelOffsetTop = $('.panel').offset().top;

	// Kasih sedikit margin bawah (biar tidak mentok)
	var marginBottom = 20;

	var finalHeight = windowHeight - panelOffsetTop - marginBottom;

	$('.panel').css({
		'height': finalHeight + 'px',
		'display': 'flex',
		'flex-direction': 'column',
		'overflow': 'hidden'
	});
}

function adjustTableHeight() {
	var panel = $('.panel');
	var panelHeight = panel.height();

	var headerHeight = 0;

	panel.find('.panel-heading, .btn-common-panel').each(function () {
		headerHeight += $(this).outerHeight(true);
	});

	var notifHeight = $('#notifArea').outerHeight(true) || 0;

	// Ambil tinggi footer DataTables (IMPORTANT)
	var footerHeight = $('.dataTables_info').outerHeight(true) || 0;
	footerHeight += $('.dataTables_paginate').outerHeight(true) || 0;

	var tableHeight = panelHeight - headerHeight - notifHeight - footerHeight - 30;

	// Set hanya body scroll
	$('.dataTables_scrollBody').css({
		'max-height': tableHeight + 'px',
		'overflow-y': 'auto'
	});
}
