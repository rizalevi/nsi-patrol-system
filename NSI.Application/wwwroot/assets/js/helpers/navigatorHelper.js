
/** ====== START PREV & NEXT BUTTON ======*/
function goNext(ddl) {

    //let ddl = $("#ClusterCodeFilter");
    let options = ddl.find("option");

    let currentVal = ddl.val();
    let index = options.index(options.filter("[value='" + currentVal + "']"));

    if (index < options.length - 1) {
        let nextVal = $(options[index + 1]).val();

        ddl.val(nextVal);
        ddl.selectpicker('refresh').trigger('changed.bs.select');
    }

}

function goPrev(ddl) {

   // let ddl = $("#ClusterCodeFilter");
    let options = ddl.find("option");

    let currentVal = ddl.val();
    let index = options.index(options.filter("[value='" + currentVal + "']"));

    if (index > 0) {
        let prevVal = $(options[index - 1]).val();

        ddl.val(prevVal);
        ddl.selectpicker('refresh').trigger('changed.bs.select');
    }

}


/** ====== END PREV & NEXT BUTTON ======*/