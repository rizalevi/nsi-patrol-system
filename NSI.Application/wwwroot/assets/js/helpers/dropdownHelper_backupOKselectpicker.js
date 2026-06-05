/**
 * ==========================================
 * GLOBAL SELECT PICKER
 * Bootstrap 5 + Color Admin
 * ==========================================
 */

function escapeHtml(text) {

    if (text == null) return '';

    return text
        .toString()
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}


/**
 * ==========================================
 * INIT SELECT PICKER
 * CALL SEKALI SAJA
 * ==========================================
 */


function setupSelectPicker(selector, placeholder = 'Please Select Data') {

    let ddl = $(selector);

    // init hanya sekali
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

/**
 * ==========================================
 * LOAD DATA
 * ==========================================
 */

function loadSelectPicker(options) {

    let config = $.extend({
        url: '',
        method: 'POST',
        dropdownId: '',
        model: null,
        codeField: 'Code',
        descField: 'Description',
        selectedValue: '',
        descTextbox: null
    }, options);

    let ddl = $("#" + config.dropdownId);

    if (ddl.length === 0) {
        console.warn("Dropdown not found:", config.dropdownId);
        return;
    }

    // ==========================================
    // AJAX
    // ==========================================

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
        
            // ==========================================
            // NORMALIZE DATA
            // ==========================================
            if (typeof res === "string") {
                res = JSON.parse(res);
            }
            let data = res.data || res.value || res;
            if (!Array.isArray(data)) {
                data = Object.values(data);
            }
            let newDataKey = JSON.stringify(data);
            let oldDataKey = ddl.data('source-key');
           
            // ==========================================
            // RESET
            // ==========================================

            ddl.empty();
            ddl.prop('disabled', false);

            if (config.descTextbox) {
                $("#" + config.descTextbox).val('');
            }

            // ==========================================
            // CALCULATE WIDTH
            // ==========================================

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

            // ==========================================
            // BUILD OPTION
            // ==========================================
           // if (newDataKey !== oldDataKey) {
                let htmlOptions = '';

                data.forEach(item => {

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

                    let optionHtml = `
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

                        optionHtml += `
                            <span style="
                                flex:0 0 ${extraWidth}px;
                                padding-left:10px;
                            ">
                                ${safeExtra}
                            </span>
                        `;
                    }

                    optionHtml += `</div>`;

                    htmlOptions += `
                        <option
                            value="${safeCode}"

                            data-content='${optionHtml}'

                            data-tokens="
                                ${safeCode}
                                ${safeDesc}
                                ${safeExtra}
                            ">
                        
                            ${safeCode} - ${safeDesc}

                        </option>
                    `;
                });
               
                ddl.html(htmlOptions);

                ddl.data('source-key', newDataKey);
           // }
            // ==========================================
            // REFRESH SAJA
            // ==========================================
            //if (!ddl.data('selectpicker')) {

            //    ddl.selectpicker({
            //        liveSearch: true,
            //        liveSearchPlaceholder: 'Search...',
            //        noneSelectedText: "Fill a  " + " " +config.dropdownId,
            //        width: '100%',
            //        size: 8
            //    });

            //}          
            ddl.selectpicker('refresh');
            

            // ==========================================
            // CUSTOM HEADER
            // ==========================================

            let picker = ddl.data('selectpicker');

            if (picker) {

                let dropdown = picker.$menu;

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

                        <span style="
                            flex:0 0 ${codeWidth}px;
                        ">
                            Code
                        </span>

                        <span style="
                            flex:1;
                            padding-left:10px;
                        ">
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

                dropdown.prepend(header);
            }

            // ==========================================
            // SELECTED VALUE
            // ==========================================

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

            }
            else {

                ddl.selectpicker('val', '');

            }

            // ==========================================
            // AUTO SELECT SINGLE
            // ==========================================

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
                    ddl.selectpicker('refresh');

                }

            }

            //// ==========================================
            //// CHANGE EVENT
            //// ==========================================

            ddl.off('changed.bs.select.global');

            ddl.on(
                'changed.bs.select.global',
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

            // ==========================================
            // CALLBACK
            // ==========================================

            if (config.callback) {
                config.callback(data);
            }

        },

        error: function (err) {

            console.log(
                "loadSelectPicker error:",
                err
            );

        }

    });

}