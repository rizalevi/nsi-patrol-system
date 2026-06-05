

/**
 * GLOBAL SELECT PICKER
 * ==========================================
 * options:
 * 
 * url             : API URL
 * method          : POST / GET
 * dropdownId      : select id
 * model           : request body (optional)
 * codeField       : code field
 * descField       : description field
 * extraField      : extra column field (optional)
 * descTextbox     : textbox target desc (optional)
 * selectedValue   : selected value (optional)
 * placeholder     : placeholder text
 * callback        : callback function
 * autoDisable     : disable if only 1 data
 * width           : bootstrap width
 * 
 */


function escapeHtml(text) {
    return text
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
} 


function initSelectPicker(options) {

    let config = {
        url: '',
        method: 'POST',
        dropdownId: '',
        model: null,
        codeField: 'Code',
        descField: 'Description',
        extraField: null,
        descTextbox: null,
        selectedValue: '',
        placeholder: 'Please Select Data',
        callback: null,
        autoDisable: true,
        width: '100%'
    };

    config = $.extend({}, config, options);
    let ddl = $("#" + config.dropdownId);

    if (ddl.length === 0) {
        console.warn("Dropdown not found:", config.dropdownId);
        return;
    }

    // =====================================
    // FULL CLEANUP
    // =====================================


    // destroy selectpicker lama total
    if (ddl.data('selectpicker')) {
        ddl.selectpicker('destroy');
        ddl.off('.initSelectPicker');
        ddl.empty();
    }
    // =====================================
    // AJAX
    // =====================================

    $.ajax({
        url: buildServiceUrl(config.url),
        type: config.method,
        xhrFields: {
            withCredentials: true
        },
        contentType: "application/json",
        data: config.model
            ? JSON.stringify(config.model)
            : null,
        success: function (res) {
            
            if (config.descTextbox) {
                $("#" + config.descTextbox).val('');
            }

            // normalize
            if (typeof res === "string") {
                res = JSON.parse(res);
            }

            let data = res.data || res.value || res;

            if (!Array.isArray(data)) {
                data = Object.values(data);
            }


            // =====================================
            // WIDTH CALCULATION
            // =====================================

            let maxCode = 0;
            let maxDesc = 0;
            let maxExtra = 0;

            data.forEach(x => {

                maxCode = Math.max(
                    maxCode,
                    (x[config.codeField] || '').toString().length
                );

                maxDesc = Math.max(
                    maxDesc,
                    (x[config.descField] || '').toString().length
                );

                if (config.extraField) {

                    maxExtra = Math.max(
                        maxExtra,
                        (x[config.extraField] || '').toString().length
                    );
                }

            });

            let charWidth = 8;

            let codeWidth =
                Math.min(Math.max(maxCode * charWidth, 80), 180);

            let descWidth =
                Math.min(Math.max(maxDesc * charWidth, 120), 350);

            let extraWidth =
                Math.min(Math.max(maxExtra * charWidth, 80), 180);

            // =====================================
            // APPEND OPTIONS
            // =====================================
            
            let htmlOptions = '';

            data.forEach(function (item) {

                let code =
                    (item[config.codeField] || '')
                        .toString()
                        .trim();

                let desc =
                    (item[config.descField] || '')
                        .toString();

                let extra =
                    config.extraField
                        ? (item[config.extraField] || '').toString()
                        : '';

                let safeCode = escapeHtml(code);
                let safeDesc = escapeHtml(desc);
                let safeExtra = escapeHtml(extra);

                let html = `
                    <div style="
                        display:flex;
                        align-items:center;
                        width:100%;
                    ">

                        <span style="
                            flex:0 0 ${codeWidth}px;
                        ">
                            ${safeCode}
                        </span>

                        <span style="
                            flex:1;
                            padding-left:10px;
                        ">
                            ${safeDesc}
                        </span>
                `;

                if (config.extraField) {

                    html += `
                        <span style="
                            flex:0 0 ${extraWidth}px;
                            padding-left:10px;
                        ">
                            ${safeExtra}
                        </span>
                    `;
                }

                html += `</div>`;

                htmlOptions += `
                    <option
                        value="${safeCode}"
                        data-content='${html}'
                        data-tokens="${safeCode} ${safeDesc} ${safeExtra}">
                        ${safeCode} - ${safeDesc}
                    </option>
                `;
            });

            ddl.html(htmlOptions);
//            ddl.removeClass('bs-select-hidden');
            // =====================================
            // INIT SELECTPICKER
            // =====================================

            ddl.selectpicker({
                liveSearch: true,
                liveSearchPlaceholder: 'Search...',
                noneSelectedText: config.placeholder,
                width: config.width,
                size: 8
                
            });

           


            // =====================================
            // HEADER
            // =====================================

            setTimeout(() => {

                let picker = ddl.data('selectpicker');

                if (!picker) return;

                let dropdown = picker.$menu;

                // remove old header
                dropdown.find("> .bs-header-custom").remove();

                let header = `
        <div class="bs-header-custom"
             style="
                display:flex;
                align-items:center;
                padding:8px 12px;
                font-weight:bold;
                border-bottom:1px solid #ddd;
                background:#6c757d;
                color:white;
                position:sticky;
                top:0;
                z-index:999;
             ">

            <span style="flex:0 0 ${codeWidth}px;">
                Code
            </span>

            <span style="flex:1;padding-left:10px;">
                Description
            </span>
    `;

                if (config.extraField) {

                    header += `
            <span style="
                flex:0 0 ${extraWidth}px;
                padding-left:10px;
            ">
                ${config.extraField}
            </span>
        `;
                }

                header += `</div>`;

                // inject once
                dropdown.prepend(header);

            }, 50);

            // =====================================
            // SELECTED VALUE
            // =====================================

            if (config.selectedValue) {

                ddl.selectpicker(
                    'val',
                    config.selectedValue
                );

                let selectedItem = data.find(
                    x => x[config.codeField]
                        == config.selectedValue
                );

                if (
                    selectedItem &&
                    config.descTextbox
                ) {

                    $("#" + config.descTextbox)
                        .val(selectedItem[config.descField]);
                }

            } else {

                ddl.selectpicker('val', '');
            }

            //// =====================================
            //// AUTO SELECT SINGLE
            //// =====================================

            if (data.length === 1) {

                let item = data[0];

                ddl.selectpicker(
                    'val',
                    item[config.codeField]
                );

                if (config.descTextbox) {

                    $("#" + config.descTextbox)
                        .val(item[config.descField]);
                }

                if (config.autoDisable) {

                    ddl.prop('disabled', true);
                   
                }
            }

            //// =====================================
            //// CHANGE EVENT
            //// =====================================

            ddl.off('changed.bs.select.initSelectPicker');

            ddl.on(
                'changed.bs.select.initSelectPicker',
                function () {
                    
                    let val = $(this).val();

                    let selectedItem = data.find(
                        x => x[config.codeField] == val
                    );

                    if (
                        selectedItem &&
                        config.descTextbox
                    ) {

                        $("#" + config.descTextbox)
                            .val(selectedItem[config.descField]);
                    }

                    
                }
            );

            // callback
            if (config.callback) {
                config.callback(data);
                
            }

        },

        error: function (err) {

            console.log(
                "initSelectPicker error:",
                err
            );
        }
    });
}
function setupSelectPicker(selector, placeholder = 'Please Select Data') {

    let ddl = $(selector);

    if (!ddl.data('selectpicker')) {

        ddl.selectpicker({
            liveSearch: true,
            liveSearchPlaceholder: 'Search...',
            noneSelectedText: placeholder,
            width: '100%',
            size: 8
        });

    }

}