function GetCleanFileName(fileName) {
    // Ganti spasi dengan underscore
    fileName = fileName.replace(/\s+/g, '_');

    // Ganti karakter yang tidak diperbolehkan dengan kosong (hapus)
    fileName = fileName.replace(/[\\\/:*?"<>|]/g, '');

    // Hapus tanda titik berlebih dan memastikan tidak ada titik di awal atau akhir nama file
    fileName = fileName.replace(/^[\.]+|[\.]+$/g, ''); // Menghapus titik di awal/akhir
    fileName = fileName.replace(/\.\.+/g, '.'); // Mengganti dua atau lebih titik berturut-turut dengan satu titik

    // Pastikan nama file tidak kosong setelah pembersihan
    if (!fileName) {
        fileName = 'default_filename'; // Gunakan default jika nama file menjadi kosong
    }

    return fileName;
}

function GenerateGUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, // Generate random number between 0 and 15
            v = c === 'x' ? r : (r & 0x3 | 0x8); // Modify 'y' position for version 4 UUID
        return v.toString(16); // Convert the number to hexadecimal
    });
}

function GenerateGUIDNoDash() {
    return 'xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0,
            v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

/*
    let menuLink = "/UserGuide/Index";
    let menu = GetUserMenuByLink(menuLink);

*/
function GetUserMenuByLink(menuLink) { //old
    // Ambil userMenu dari localStorage dan parsing ke dalam bentuk array
    const userMenu = JSON.parse(localStorage.getItem('usermenu'));

    // Pastikan userMenu tidak null atau undefined dan merupakan array
    if (Array.isArray(userMenu)) {
        // Cari menu berdasarkan MenuLink
        return userMenu.find(menu => menu.MenuLink === menuLink) || null;
    }

    // Jika userMenu tidak ditemukan atau bukan array
    return null;
}


/*
    let menuId = "R-01";
    let menu = GetUserMenuByMenuID(menuId);

*/
function GetUserMenuByMenuID(menuID) { //old

    // Ambil userMenu dari localStorage dan parsing ke dalam bentuk array
    const userMenu = JSON.parse(localStorage.getItem('usermenu'));

    // Pastikan userMenu tidak null atau undefined dan merupakan array
    if (Array.isArray(userMenu)) {
        // Cari menu berdasarkan MenuLink
        return userMenu.find(menu => menu.MenuID === menuID) || null;
    }

    // Jika userMenu tidak ditemukan atau bukan array
    return null;
}

/*
    let menuId = "T-99";
    let userAccess = GetUserAccess(menuId);

*/
function GetUserAccess(menuID) {
    const userMenu = JSON.parse(sessionStorage.getItem('usermenu-cached'));

    // Pastikan userMenu tidak null atau undefined dan merupakan array
    if (Array.isArray(userMenu)) {
        // Cari menu berdasarkan MenuLink
        return userMenu.find(menu => menu.MenuID === menuID) || null;
    }

    // Jika userMenu tidak ditemukan atau bukan array
    return null;
}

function GetMenuPermission() {
    // Mendapatkan menu item yang aktif (yang memiliki kelas "show")
    var activeMenuItem = document.querySelector('.menu-item.show');

    if (activeMenuItem) {
        // Membaca atribut data dari elemen yang terpilih
        var permission = {
            AllowAccess: activeMenuItem.getAttribute('data-allow-access'),
            AllowUpdate: activeMenuItem.getAttribute('data-allow-update'),
            AllowSpecial: activeMenuItem.getAttribute('data-allow-special'),
            AllowDelete: activeMenuItem.getAttribute('data-allow-delete')
        };

        // Mengembalikan objek dengan hak akses
        return permission;
    }

    // Jika tidak ada menu item yang terpilih, kembalikan objek dengan nilai default
    return {
        AllowAccess: null,
        AllowUpdate: null,
        AllowSpecial: null,
        AllowDelete: null
    };
}

function ConvertToCSV(data) {
    const header = Object.keys(data[0]).join(","); // Mendapatkan header (a, b, c)
    const rows = data.map(item => Object.values(item).join(",")); // Mengonversi setiap objek menjadi baris CSV
    return [header, ...rows].join("\n"); // Gabungkan header dan data
}

/*
    const blob = createCSVBlob(data); // Membuat Blob dari data

    const formData = new FormData(); // Membuat FormData untuk mengirim data ke server
    formData.append("file", blob, "data.csv"); // Menambahkan file CSV ke FormData

    // Mengirimkan data melalui AJAX
    $.ajax({
        url: '/api/fileupload/upload-csv', // Ganti dengan URL endpoint Anda
        type: 'POST',
        data: formData,
        processData: false, // Agar jQuery tidak memproses data
        contentType: false, // Agar jQuery tidak mengatur Content-Type header
        ...
*/
function CreateCSVBlob(data) {
    const csvData = ConvertToCSV(data);  // Mengonversi objek ke CSV
    const blob = new Blob([csvData], { type: 'text/csv' });  // Membuat Blob CSV
    return blob;
}

function Debounce(func, delay) {
    let debounceTimer;
    return function () {
        const context = this;
        const args = arguments;
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(() => func.apply(context, args), delay);
    };
}

function IsStringIsNullOrEmpty(obj) {
    if (obj === null || obj === "" || obj === undefined || obj === '') {
        return true;
    }
    else {
        return false;
    }
}

async function AjaxRequest(_url = 'http://localhost:1234/api/dailyactivityasd123/downloadexcel/', _param) {
    if (_url === 'http://localhost:1234/api/dailyactivityasd123/downloadexcel/' || _url === '' || _url === undefined || _url === null) {
        toastr.warning('Please provide url!', 'Warning', { timeOut: 3000, "closeButton": true });
        return false;
    }

    try {
        // Mengirimkan request menggunakan fetch dengan kredensial
        const response = await fetch(_url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(_param),
            credentials: 'include', // Sertakan kredensial (seperti cookies)
        });

        // Memeriksa apakah respons sukses
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        return response.json();

    } catch (error) {
        console.error('Failed to ajax request, error: ', error);
        toastr.error('Failed to ajax request', 'Error', { timeOut: 3000, "closeButton": true });
    }
}

//async function AjaxRequestDownload(_url = 'http://localhost:1234/api/dailyactivityasd123/downloadexcel/', _param, _fileName = 'dailyactivityasd', _extension = 'xlsx') {
//    /* usage:
//         $('#btnExcel').click(async function () {
//            await AjaxRequestDownload(BaseUrl + 'service/dailyactivityreport/downloadexcel/', payload, 'DailyActivityReport', 'xlsx');
//        });
//    */

//    if (_url === 'http://localhost:1234/api/dailyactivityasd123/downloadexcel/' || _url === '' || _url === undefined || _url === null) {
//        toastr.warning('Please provide download url!', 'Warning', { timeOut: 3000, "closeButton": true });
//        return false;
//    }

//    try {
//        // Mengirimkan request menggunakan fetch dengan kredensial
//        const response = await fetch(_url, {
//            method: 'POST',
//            headers: { 'Content-Type': 'application/json' },
//            body: JSON.stringify(_param),
//            credentials: 'include', // Sertakan kredensial (seperti cookies)
//        });

//        // Memeriksa apakah respons sukses
//        if (!response.ok) {
//            throw new Error(`HTTP error! Status: ${response.status}`);
//        }

//        // Menentukan nama file yang akan diunduh
//        let fileName = _fileName + '_' + new Date().toISOString().slice(0, 10) + '.' + _extension; // Misalnya 'dailyactivityasd_2025-02-06.xlsx'

//        // Mendapatkan data file dalam bentuk array buffer
//        const fileData = await response.arrayBuffer();

//        // Menentukan tipe MIME berdasarkan ekstensi file
//        let _type = '';
//        switch (_extension.toLowerCase()) {
//            case "xls":
//            case "xlsx":
//                _type = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
//                break;
//            case "doc":
//            case "docx":
//                _type = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';
//                break;
//            case "pdf":
//                _type = 'application/pdf';
//                break;
//            case "jpeg":
//            case "jpg":
//                _type = 'image/jpeg';
//                break;
//            case "png":
//                _type = 'image/png';
//                break;
//            default:
//                break;
//        }

//        // Membuat Blob dari data yang diterima
//        const blob = new Blob([fileData], { type: _type });

//        // Membuat URL untuk file yang baru dibuat
//        const link = document.createElement('a');
//        link.href = URL.createObjectURL(blob);
//        link.download = fileName; // Menetapkan nama file untuk unduhan

//        // Memicu klik untuk memulai unduhan
//        document.body.appendChild(link);
//        link.click();
//        document.body.removeChild(link); // Menghapus elemen setelah unduhan

//    } catch (error) {
//        console.error('Error:', error);
//        toastr.error('Failed to Download', 'Error', { timeOut: 3000, "closeButton": true });
//    }
//}

async function AjaxRequestDownload(_url = 'http://localhost:1234/api/dailyactivityasd123/downloadexcel/', _param, _fileName = 'dailyactivityasd', _extension = 'xlsx') {
    try {
        // Mengirim permintaan ke API untuk mengunduh file Excel
        const response = await fetch(_url, {
            method: 'POST', // Menggunakan POST karena ada parameter (_param)
            headers: {
                'Content-Type': 'application/json',
                // Menambahkan header Authorization jika diperlukan
                // 'Authorization': 'Bearer YOUR_ACCESS_TOKEN' // Jika menggunakan Bearer Token
            },
            credentials: 'include', // Menyertakan kredensial sesi (cookies)
            body: JSON.stringify(_param), // Mengirimkan parameter dalam bentuk JSON
        });

        // Memeriksa apakah permintaan berhasil
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        // Mengambil response body sebagai Blob
        const blob = await response.blob();

        // Membuat URL object dari Blob untuk mendownload
        const url = window.URL.createObjectURL(blob);

        // Membuat elemen <a> untuk trigger download
        const a = document.createElement('a');
        a.href = url;

        _fileName = _fileName + '_' + new Date().toISOString().slice(0, 10) + '.' + _extension; // Misalnya 'dailyactivityasd_2025-02-06.xlsx'

        a.download = _fileName; // Nama file yang akan diunduh dengan ekstensi yang diberikan
        document.body.appendChild(a);
        a.click(); // Memicu klik untuk memulai download
        document.body.removeChild(a);

        // Bebaskan URL object setelah selesai
        window.URL.revokeObjectURL(url);
    } catch (error) {
        console.error('Error during download:', error);
    }
}

function RemoveLastComma(str) {
    // Hapus spasi, tabulasi, dan whitespace lainnya di awal dan akhir
    str = str.replace(/[\s\t]+$/, '');

    // Cek apakah string berakhir dengan koma
    if (str.endsWith(',')) {
        return str.slice(0, str.lastIndexOf(',')); // Hapus koma terakhir
    }

    return str; // Jika tidak ada koma, kembalikan string asli
}

const CopyToClipboard = (id) => {
    var r = document.createRange();
    r.selectNode(document.getElementById(id));
    window.getSelection().removeAllRanges();
    window.getSelection().addRange(r);
    document.execCommand("copy");
    window.getSelection().removeAllRanges();
    toastr.success('Copy Success!', 'Success', { timeOut: 3000, "closeButton": true });
};

const isIOS = /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;

const copyIOS = (id) => {
    const text = document.getElementById(id).innerHTML;

    if (!navigator.clipboard) {
        const textarea = document.createElement("textarea");

        textarea.value = text;
        textarea.style.fontSize = "20px";
        document.body.appendChild(textarea);

        const range = document.createRange();
        range.selectNodeContents(textarea);

        const selection = window.getSelection();
        selection.removeAllRanges();
        selection.addRange(range);
        textarea.setSelectionRange(0, 999999);

        document.execCommand("copy");

        document.body.removeChild(textarea);
    }

    navigator.clipboard.writeText(text);
};

const copyTextById = (id) => {
    if (isIOS) {
        return copyIOS(id);
    }
    CopyToClipboard(id);
};

window.copyTextById = copyTextById