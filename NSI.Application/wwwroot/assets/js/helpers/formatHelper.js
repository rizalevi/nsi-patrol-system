// ================================
// DATE FORMATTER
// ================================

function formatDate(dateString) {
    if (!dateString) return "";

    const d = new Date(dateString);

    const day = String(d.getDate()).padStart(2, "0");

    const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    const month = months[d.getMonth()];
    const year = d.getFullYear();

    return `${day} ${month} ${year}`;
}
// ================================
// DATE TIME FORMATTER
// ================================

function formatDateTime(dateString) {
    if (!dateString) return "";

    const d = new Date(dateString);

    const day = String(d.getDate()).padStart(2, "0");

    const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    const month = months[d.getMonth()];
    const year = d.getFullYear();

    const hours = String(d.getHours()).padStart(2, "0");
    const minutes = String(d.getMinutes()).padStart(2, "0");
    const seconds = String(d.getSeconds()).padStart(2, "0");

    return `${day} ${month} ${year} ${hours}:${minutes}:${seconds}`;
}

// ================================
// TIME FORMATTER
// ================================

function formatTime(dateString) {
    if (!dateString) return "";

    const d = new Date(dateString);

    const hours = String(d.getHours()).padStart(2, "0");
    const minutes = String(d.getMinutes()).padStart(2, "0");
    const seconds = String(d.getSeconds()).padStart(2, "0");

    return `${hours}:${minutes}:${seconds}`;
}

function formatDateGeneral(dateStr) {
    let d = new Date(dateStr);

    let yyyy = d.getFullYear();
    let mm = String(d.getMonth() + 1).padStart(2, '0');
    let dd = String(d.getDate()).padStart(2, '0');

    return `${yyyy}-${mm}-${dd}`;
}

// ================================
// NUMBER FORMATTER
// ================================

function formatNumber(number, decimal = 2) {
    if (number === null || number === undefined || number === "") return "0";

    return parseFloat(number).toLocaleString("en-US", {
        minimumFractionDigits: decimal,
        maximumFractionDigits: decimal
    });
}


// ================================
// CURRENCY FORMATTER
// ================================

function formatCurrency(number, symbol = "Rp") {
    if (number === null || number === undefined || number === "") return `${symbol} 0`;

    const formatted = parseFloat(number).toLocaleString("en-US", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });

    return `${symbol} ${formatted}`;
}

//new requirement from user (15 oct 2026) max total digit 4 if value decimal

function FormatInspectionResult(value) {
    if (value === null || value === undefined || value === "") {
        return "";
    }

    var num = Number(value);

    if (!Number.isFinite(num)) {
        return value;
    }

    // Bilangan bulat ribuan atau lebih
    if (Number.isInteger(num) && Math.abs(num) >= 1000) {
        return num.toFixed(0);
    }

    var integerDigits = Math.abs(Math.trunc(num)).toString().length;
    var decimalDigits = Math.max(0, 4 - integerDigits);

    return num.toFixed(decimalDigits);
}


