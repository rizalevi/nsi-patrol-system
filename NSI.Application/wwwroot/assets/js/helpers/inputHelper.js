$("input[maxlength]").each(function () {

    let max = parseInt($(this).attr("maxlength")) || 10;

    // estimasi lebar karakter
    let width = max * 0.65;

    // batas minimum & maksimum
    width = Math.max(width, 8);
    width = Math.min(width, 30);

    $(this).css({
        width: width + "rem",
        minWidth: "100px"
    });

});

$(document).on("click", ".toggle-password", function () {
    let input = $($(this).attr("toggle"));

    if (input.attr("type") === "password") {
        input.attr("type", "text");
        $(this).removeClass("fa-eye").addClass("fa-eye-slash");
    } else {
        input.attr("type", "password");
        $(this).removeClass("fa-eye-slash").addClass("fa-eye");
    }
});

//$("input.auto-width[maxlength]").each(function () {
//    let max = parseInt($(this).attr("maxlength")) || 10;

//    $(this).css({
//        width: `calc(${max}ch + 6rem)`,
//        minWidth: "80px"
//    });
//});
//function autoResizeInput(input) {
//    let $input = $(input);

//    let $span = $("#hidden-span-measure");
//    if ($span.length === 0) {
//        $span = $("<span id='hidden-span-measure'></span>").appendTo("body");
//    }

//    $span.css({
//        "font-family": $input.css("font-family"),
//        "font-size": $input.css("font-size"),
//        "font-weight": $input.css("font-weight"),
//        "letter-spacing": $input.css("letter-spacing"),
//        "white-space": "pre",
//        "visibility": "hidden"
//    });

//    let value = $input.val() || $input.attr("placeholder") || "";
//    $span.text(value);

//    // 🔥 Tambahkan minimum width 100px
//    let calculatedWidth = $span.width() + 30;
//    let finalWidth = Math.max(100, calculatedWidth);

//    $input.css("width", finalWidth + "px");
//}


//// on input
//$(document).on("input", "input.auto-width", function () {
//    //autoResizeInput(this);
//});

/* =========================
   GLOBAL DATEPICKER HELPER
   ========================= */
var GlobalDatepicker = (function () {

    function init(selector) {
        $(selector).each(function () {

            // 🔥 destroy dulu biar tidak pakai config lama
            if ($(this).data('datepicker')) {
                $(this).datepicker('destroy');
            }

            $(this).datepicker({
                format: "dd M yyyy",
                autoclose: true,
                todayHighlight: true,
                forceParse: false, // 🔥 penting
                language: "en"
            });

            // OPTIONAL: kalau mau cegah input manual
            // $(this).attr("readonly", true);
        });
    }

    function setDefault(selector, addMonth = 0) {
        let date = new Date();
        date.setMonth(date.getMonth() + addMonth);

        $(selector).each(function () {
            $(this).datepicker('setDate', date);
        });
    }

    return {
        init: init,
        setDefault: setDefault
    };

})();