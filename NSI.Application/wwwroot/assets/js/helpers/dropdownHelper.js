/**
 * ==========================================
 * GLOBAL SELECT2 HELPER
 * Bootstrap 5 + Color Admin
 * ==========================================
 */

/**
 * ==========================================
 * ESCAPE HTML
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
 * /
 * @param {any} dropdownId
 */
function getDropdownLabel(dropdownId) {
    return dropdownId
        .replace(/^ddl/i, '')          // hapus prefix "ddl"
        .replace(/(Code|Filter|ID|Id)$/i, '') // hapus suffix umum
        .trim();
}


/**
 * ==========================================
 * GLOBAL LOAD SELECT2
 * ==========================================
 */

function populateDDL(options) {

    let config = $.extend({
        url: '',
        method: 'POST',
        dropdownId: '',
        model: null,
        codeField: 'Code',
        descField: 'Description',
        extraField: null,
        selectedValue: '',
        descTextbox: null,
        autoDisable: true,
        placeholder: 'Fill a ',
        /*width:"180px",*/
        callback: null

    }, options);


    let ddl = $("#" + config.dropdownId);

    if (ddl.length === 0) {

        console.warn(
            "Dropdown not found:",
            config.dropdownId
        );

        return;
    }


    /**
     * ==========================================
     * AJAX
     * ==========================================
     */

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
            /**
             * ==========================================
             * NORMALIZE RESPONSE
             * ==========================================
             */

            if (typeof res === "string") {

                res = JSON.parse(res);

            }

            let data =
                res.data ||
                res.value ||
                res;

            if (!Array.isArray(data)) {

                data = Object.values(data);

            }



            /**
             * ==========================================
             * RESET
             * ==========================================
             */

            if (ddl.hasClass('select2-hidden-accessible'))
            {
                ddl.select2('destroy');

            }

            ddl.empty();

            ddl.prop('disabled', false);

            if (config.descTextbox) {

                $("#" + config.descTextbox).val('');

            }



            /**
             * ==========================================
             * CALCULATE WIDTH
             * ==========================================
             */

            let maxCode = 0;
            let maxDesc = 0;
            let maxExtra = 0;

            data.forEach(x => {

                maxCode = Math.max(
                    maxCode,
                    (x[config.codeField] || '')
                        .toString()
                        .length
                );

                maxDesc = Math.max(
                    maxDesc,
                    (x[config.descField] || '')
                        .toString()
                        .length
                );

                if (config.extraField) {

                    maxExtra = Math.max(
                        maxExtra,
                        (x[config.extraField] || '')
                            .toString()
                            .length
                    );

                }

            });

            let charWidth = 8;

            let codeWidth =
                Math.min(
                    Math.max(maxCode * charWidth, 80),
                    180
                );

            let descWidth =
                Math.min(
                    Math.max(maxDesc * charWidth, 120),
                    350
                );

            let extraWidth =
                Math.min(
                    Math.max(maxExtra * charWidth, 80),
                    180
                );



            /**
             * ==========================================
             * DEFAULT OPTION
             * ==========================================
             */
          
            let defOps = config.placeholder + getDropdownLabel(config.dropdownId)


            ddl.append(`
                <option value="" disabled selected>
                    ${defOps}
                </option>
            `);



            /**
             * ==========================================
             * APPEND OPTIONS
             * ==========================================
             */

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
                        ? (item[config.extraField] || '')
                            .toString()
                        : '';

                let option =
                    new Option(
                        code,
                        code,
                        false,
                        false
                    );

                $(option).attr({

                    'data-code': code,

                    'data-desc': desc,

                    'data-extra': extra

                });

                ddl.append(option);

            });



            /**
             * ==========================================
             * INIT SELECT2
             * ==========================================
             */
            let parentModal = ddl.closest('.modal');

            ddl.select2({
                theme: 'bootstrap-5',
               /* width: config.width,*/
                placeholder: defOps,
                allowClear: true,
                dropdownParent: parentModal.length
                    ? parentModal
                    : $(document.body),
                dropdownAutoWidth: true,
                matcher: function (params, data) {
                    
                    if ($.trim(params.term) === '') {
                        return data;
                    }

                    let term = params.term.toLowerCase();

                    let el = $(data.element);

                    let code = (el.data('code') || '').toLowerCase();
                    let desc = (el.data('desc') || '').toLowerCase();
                    let extra = (el.data('extra') || '').toLowerCase();

                    if (
                        code.includes(term) ||
                        desc.includes(term) ||
                        extra.includes(term)
                    ) {
                        return data;
                    }

                    return null;
                },
                /**
                 * ==========================================
                 * DROPDOWN TEMPLATE
                 * ==========================================
                 */

                templateResult: function (state) {
                    if (!state.id) {
                        return state.text;

                    }

                    let el =
                        $(state.element);

                    let code =
                        escapeHtml(
                            el.data('code')
                        );

                    let desc =
                        escapeHtml(
                            el.data('desc')
                        );

                    let extra =
                        escapeHtml(
                            el.data('extra')
                        );

                    let html = `
                        <div style="
                            display:flex;
                            align-items:center;
                            width:100%;
                        ">

                            <span style="
                                flex:0 0 ${codeWidth}px;
                                font-weight:500;
                            ">
                                ${code}
                            </span>

                            <span style="
                                flex:1;
                                padding-left:10px;
                            ">
                                ${desc}
                            </span>
                    `;

                    if (config.extraField) {

                        html += `
                            <span style="
                                flex:0 0 ${extraWidth}px;
                                padding-left:10px;
                            ">
                                ${extra}
                            </span>
                        `;

                    }

                    html += `</div>`;

                    return $(html);

                },



                /**
                 * ==========================================
                 * SELECTED TEMPLATE
                 * ==========================================
                 */

                templateSelection: function (state) {

                    if (!state.id) {

                        return state.text;

                    }

                    let el =
                        $(state.element);

                    let code =
                        el.data('code');

                    let desc =
                        el.data('desc');

                    return `
                        ${code} 
                    `;

                },



                escapeMarkup: function (markup) {

                    return markup;

                }

            });



            /**
             * ==========================================
             * CUSTOM HEADER
             * ==========================================
             */

            ddl.off('select2:open.global');

            ddl.on(
                'select2:open.global',
                function () {

                    setTimeout(() => {

                        let dropdown =
                            $('.select2-results');

                        dropdown
                            .find(
                                '.custom-select2-header'
                            )
                            .remove();

                        let header = `
                            <div class="custom-select2-header"
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
                                    z-index:9999;
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

                    }, 0);

                }
            );



            /**
             * ==========================================
             * SELECTED VALUE
             * ==========================================
             */

            if (config.selectedValue) {
                
                ddl.val(config.selectedValue)
                    .trigger('change.select2');

                let selectedItem = data.find(
                    x =>
                        x[config.codeField]
                        == config.selectedValue
                );

                if (
                    selectedItem &&
                    config.descTextbox
                ) {

                    $("#" + config.descTextbox)
                        .val(
                            selectedItem[
                            config.descField
                            ]
                        );

                }

            }
            else {

                ddl.val(null)
                    .trigger('change.select2');

            }



            /**
             * ==========================================
             * AUTO SELECT SINGLE
             * ==========================================
             */

            if (data.length === 1) {

                let item = data[0];

                ddl.val(
                    item[config.codeField]
                ).trigger('change');

                if (config.descTextbox) {

                    $("#" + config.descTextbox)
                        .val(
                            item[
                            config.descField
                            ]
                        );

                }

                if (config.autoDisable) {

                    
                    ddl.prop("disabled", true).trigger('change.select2');
                }

            }



            /**
             * ==========================================
             * CHANGE EVENT
             * ==========================================
             */

            ddl.off('change.globalSelect2');

            ddl.on(
                'change.globalSelect2',
                function () {

                    let val =
                        $(this).val();

                    let selectedItem =
                        data.find(
                            x =>
                                x[
                                config.codeField
                                ] == val
                        );

                    if (
                        selectedItem &&
                        config.descTextbox
                    ) {

                        $("#" + config.descTextbox)
                            .val(
                                selectedItem[
                                config.descField
                                ]
                            );

                    }

                }
            );



            /**
             * ==========================================
             * CALLBACK
             * ==========================================
             */

            if (config.callback) {

                config.callback(data);

            }

        },



        error: function (err) {

            console.log(
                "loadSelect2 error:",
                err
            );

        }

    });

}