var table;
let allowUpdate = window.userPermission?.allowUpdate;
let allowDelete = window.userPermission?.allowDelete;
let statusAdmin = window.userPermission?.statusAdmin;


loadData();

// Event Trigger untuk Dropdown Method Type (Persis seperti cboMethodType_TextChanged di VB.NET)
$('#MethodCode').on('change', function () {
    var val = $(this).val();
    if (val === "01") { // "01" = Option
        $('#Option1').prop('disabled', false);
        $('#Option2').prop('disabled', false);
        $('#Option3').prop('disabled', false);
    } else if (val === "02") { // "02" = Numeric
        $('#Option1').prop('disabled', true).val('');
        $('#Option2').prop('disabled', true).val('');
        $('#Option3').prop('disabled', true).val('');
    } else {
        $('#Option1').prop('disabled', false).val('');
        $('#Option2').prop('disabled', false).val('');
        $('#Option3').prop('disabled', false).val('');
    }
});

function loadData() {
    if (table) {
        table.ajax.reload(null, false);
        return;
    }


    table = $("#tableID").DataTable({
        destroy: true,
        ajax: {
            url: BaseUrl + "service/msunitclassification/list", // Sesuaikan Endpoint
            type: "POST",
            xhrFields: {
                withCredentials: true
            },
            contentType: "application/json",
            data: function () {
                // Return empty object since we don't have search parameters for now
                return JSON.stringify({});
            },
            dataSrc: function (json) {
               
                return json.data;
            }
        },
        columns: [
            {
                data: null,
                className: "text-center",
                render: function (data, type, row) {
                    if (allowDelete == '0') {
                        return `
                                <button class="btn btn-xs theme-color btn-edit" data-id="${row.UnitCode}">
                                    <i class="fa fa-edit"></i>
                                </button>
                                <button class="btn btn-xs theme-color" disabled>
                                    <i class="fa fa-trash-o"></i>
                                </button>
                            `;
                    } else {
                        return `
                                <button class="btn btn-xs theme-color btn-edit" data-id="${row.UnitCode}">
                                    <i class="fa fa-edit"></i>
                                </button>  
                                <button class="btn btn-xs theme-color btn-delete" data-id="${row.UnitCode}">
                                    <i class="fa fa-trash-o"></i>
                                </button>
                            `;
                    }
                }
            },
            { data: "UnitCode" },
            { data: "UnitDescription" },
            {
                data: "MethodDescription",
                render: function (data, type, row) {
                    return data ? data : row.MethodCode; // Fallback jika desc kosong
                }
            },
            { data: "Option1" },
            { data: "Option2" },
            { data: "Option3" },
            {
                data: "RegisterDate",
                render: function (data) {
                    return data ? formatDateTime(data) : ""; // Pastikan formatDateTime function ada
                },
            },
            { data: "RegisterUser" },
            {
                data: "LastUpdate",
                render: function (data) {
                    if (!data || data === "0001-01-01T00:00:00") return "";
                    return formatDateTime(data);
                },
            },
            { data: "LastUser" }
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
        scrollY: "calc(105vh - 400px)",
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

function AddNewData() {
    // Clear all fields
    $("#UnitCode").val('');
    $("#UnitDescription").val('');
    $("#MethodCode").val('');
    $("#Option1").val('').prop('disabled', false);
    $("#Option2").val('').prop('disabled', false);
    $("#Option3").val('').prop('disabled', false);

    $("#ModalTitle").html("Unit Classification Master");
    $("#MyModal").modal("show");
    $('#InsertData').show();
    $('#UpdateData').hide();
    $("#UnitCode").prop('readonly', false);
}

// POPUP MODAL GET DATA (EDIT)
$(document).on("click", ".btn-edit", function () {
    
    // 1. TAMBAHKAN .trim() DI SINI
    // Ini akan memotong "Acidity_pH     " menjadi "Acidity_pH" secara otomatis
    var code = String($(this).data("id")).trim(); 


    $.ajax({
        url: BaseUrl + "service/msunitclassification/detail",
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            UnitCode: code // Sekarang yang dikirim sudah bersih tanpa spasi
        }),
        success: function (res) {
           
            // Kadang-kadang struktur JSON balikan dari C# bisa berbeda huruf besar/kecil.
            // Kita akali dengan mengambil res.data atau res.Data
            var responseData = res.data || res.Data; 

            if (responseData) {
                // Gunakan || untuk mengatasi perbedaan camelCase dan PascalCase
                $("#UnitCode").val(responseData.UnitCode || responseData.unitCode || '');
                $("#UnitDescription").val(responseData.UnitDescription || responseData.unitDescription || '');
                
                var methodCode = responseData.MethodCode || responseData.methodCode || '';
                $("#MethodCode").val(methodCode).trigger('change'); 
                
                $("#Option1").val(responseData.Option1 || responseData.option1 || '');
                $("#Option2").val(responseData.Option2 || responseData.option2 || '');
                $("#Option3").val(responseData.Option3 || responseData.option3 || '');

                $("#UnitCode").prop("readonly", true); 
                $("#ModalTitle").html("Unit Classification Master");

                $("#InsertData").hide();
                $("#UpdateData").show();

                $("#MyModal").modal("show");
            } else {
                // Jika masih masuk ke sini, berarti database benar-benar me-return NULL
                toastr.warning('Data format from server is invalid or Data not found.', 'Warning');
            }
        },
        error: function () {
            toastr.error('Failed to load data.', 'Error', { timeOut: 3000, "closeButton": true });
        }
    });
});

// INSERT ATAU SAVE
$(document).on("click", "#InsertData", function () {
    if (!validateForm()) return;

    var model = getFormData();

    $.ajax({
        type: "POST",
        url: BaseUrl + "service/msunitclassification/save", // <-- UBAH DARI create MENJADI save
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        xhrFields: {
            withCredentials: true
        },
        success: function (result) {
          
            if (!result.success) {
                toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });
            } else {
                loadData();
                toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
                closeModal();
            }
        }
    });
});

// UPDATE
$(document).on("click", "#UpdateData", function () {
    if (!validateForm()) return;

    var model = getFormData();

    $.ajax({
        type: "POST", // <-- UBAH DARI PATCH MENJADI POST
        url: BaseUrl + "service/msunitclassification/save", // <-- UBAH DARI update MENJADI save
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        xhrFields: {
            withCredentials: true
        },
        success: function (result) {
            if (!result.success) {
                toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });
            } else {
                loadData();
                toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
                closeModal();
            }
        }
    });
});

// DELETE
$(document).on("click", ".btn-delete", function () {
    var id = $(this).data("id");

    Swal.fire({
        title: 'Are you sure?',
        text: "Data will be deleted permanently!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'Cancel',
        // Tiga baris di bawah ini adalah kunci untuk merapikan layout-nya:
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-danger mx-2',  // Tambahkan margin kiri-kanan (mx-2)
            cancelButton: 'btn btn-default mx-2'   // Gunakan btn-default atau btn-secondary
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: BaseUrl + "service/msunitclassification/delete",
                type: "POST",
                contentType: "application/json",
                xhrFields: {
                    withCredentials: true
                },
                data: JSON.stringify({
                    UnitCode: id
                }),
                success: function (result) {
                    if (!result.success) {
                        toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });
                    } else {
                        toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
                        $('#tableID').DataTable().ajax.reload(null, false);
                    }
                }
            });
        }
    });
});

// SEARCH BOX DATATABLE
var timer;
$(document).on("keyup", "#tableSearch", function () {
    let keyword = $(this).val();
    clearTimeout(timer);
    timer = setTimeout(function () {
        table.search(keyword).draw();
    }, 300);
});

// --- HELPER FUNCTIONS ---

function getFormData() {
    var model = new Object();
    model.UnitCode = $('#UnitCode').val();
    model.UnitDescription = $('#UnitDescription').val();
    model.MethodCode = $('#MethodCode').val();
    model.MethodDescription = $("#MethodCode option:selected").text(); // Ambil text dari dropdown
    model.Option1 = $('#Option1').val();
    model.Option2 = $('#Option2').val();
    model.Option3 = $('#Option3').val();
    return model;
}

function validateForm() {
    var method = $('#MethodCode').val();

    if (!$('#UnitCode').val()) {
        toastr.warning("Unit Code is required.");
        return false;
    }
    if (!$('#UnitDescription').val()) {
        toastr.warning("Unit Description is required.");
        return false;
    }
    if (!method) {
        toastr.warning("Please select a Method Type.");
        return false;
    }

    // Validasi logic dari VB.NET: Jika Option dipilih, Option 1 dan 2 wajib diisi
    if (method === "01") {
        if (!$('#Option1').val()) {
            toastr.warning("Option 1 is required for Option method.");
            return false;
        }
        if (!$('#Option2').val()) {
            toastr.warning("Option 2 is required for Option method.");
            return false;
        }
    }
    return true;
}

function closeModal() {
    $('#MyModal').modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();
}