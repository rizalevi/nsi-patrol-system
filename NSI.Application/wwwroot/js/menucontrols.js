$(function () {
    var url = window.location;
    
    // for hold to actived sidebar menu
    $('.menu-item a').filter(function () {
        return this.href == url;
    }).parentsUntil('.menu-link')
        .addClass('active');

    // for search menu in sidebar menu 
    $("#menuSearch").on("keyup", function () {
        var srchmenu = "";
        srchmenu = $(this).val().toLowerCase();

        if (srchmenu == "") {
            $(".menu-submenu").hide();
            $(".has-sub").removeClass("expand");
            $(".has-sub").addClass("closed");
        } else {
            $(".menu-submenu").show();
            $(".has-sub").addClass("expand");
            $(".has-sub").removeClass("closed");
        }

        $(".menu-submenu .menu-item").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(srchmenu) > -1)
        })

        //console.log(srchmenu)
    })

    // remove style width in select2
    $(".select2-container").removeAttr('style');
});