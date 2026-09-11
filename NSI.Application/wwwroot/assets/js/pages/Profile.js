
GlobalDatepicker.init(".datepicker");
const emailRegex = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;

loadData();

$("#Email").on("input blur", function () {
    validateEmail();
});

function validateEmail() {
    const emailInput = $("#Email");
    const email = $.trim(emailInput.val());

    if (email === "") {
        emailInput.removeClass("is-valid is-invalid");
        $(".div-email").removeClass("show");
        $(".div-email").addClass("hide");
        $("#emailError").hide();
        return false;
    }

    if (emailRegex.test(email)) {
        emailInput.removeClass("is-invalid").addClass("is-valid");
        $(".div-email").removeClass("show");
        $(".div-email").addClass("hide");

        $("#emailError").hide();
        return true;
    } else {
        emailInput.removeClass("is-valid").addClass("is-invalid");
        $(".div-email").removeClass("hidden");
        $(".div-email").addClass("show");

        $("#emailError").show();
        return false;
    }
}


function loadData() {
    $("#targetImg").removeAttr("src");
    $.ajax({
        url: buildServiceUrl("service/usersetup/get-profile"),
        type: "POST",
        contentType: "application/json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify({
            UserID: $("#UserID").val()
        }),
        success: function (res) {
          
            if (res && res.data) {

                $("#UserID").val(res.data.UserID);
                $("#UserID").prop("readonly", true);
                $("#UserNameProfile").val(res.data.UserName);
                $("#Password").val(res.data.Password).attr("type", "password");
                $("#Password").prop("readonly", true);

                let dateObj = new Date(res.data.PasswordExpired);
                $("#PasswordExpired").datepicker('setDate', dateObj) .val(formatDate(dateObj));     
                $("#PasswordExpired").prop("readonly", true);
                $("#PictureName").val(res.data.PicturePath);
                $("#Email").val(res.data.Email);

                if (res.data.PicturePath &&
                    res.data.PicturePath.trim() !== '') {
                        
                    $("#targetImg")
                        .attr(
                            "src",res.data.PicturePath
                        );

                    $("#imgPreview").show();

                }
                else {

                    $("#targetImg")
                        .attr(
                            "src",
                            "/content/img/avatar-icon.jpg"
                        );

                }


                //setTimeout(function () {
                //    ViewPicture(res.data.UserID);
                //}, 1000);

            }
        },
        error: function () {

            toastr.warning('Failed to load data.', 'Error', { timeOut: 3000, "closeButton": true });

        }
    });

}


//prevent default submit form
$("#Profile").on("submit", function (e) {
    e.preventDefault(); // 🔥 WAJIB
});
$(document).on("click", ".save-profile", function () {
 
    /*Maximum allowed size in bytes
                  1 MB Example
    */
    const maxAllowedSize = 1* 1024 * 1024;
    var fileimg = $("#imageBrowse").get(0).files;
    if (fileimg.length > 0) {
        if ($('#imageBrowse')[0].files[0].size > maxAllowedSize) {
            toastr.warning('Maximum upload picture 1 MB', 'Warning', { timeOut: 3000, "closeButton": true });
            return;
        }
    }

    //var model = new Object();
    //model.UserID = $('#UserID').val();
    //model.UserName = $('#UserNameProfile').val();
    //model.Password = $('#Password').val();
    //model.PasswordExpired = formatDateGeneral($('#PasswordExpired').val());
    //model.Email = $("#Email").val();
    //model.PictureName = (fileimg[0] === undefined ? $("#PictureName").val() : fileimg[0].name);
    //model.PicturePath = (fileimg[0] === undefined ? $("#PictureName").val() : fileimg[0]);

    var formData = new FormData();

    formData.append("UserID", $('#UserID').val());
    formData.append("UserName", $('#UserNameProfile').val());
    formData.append("Password", $('#Password').val());
    formData.append("Email", $("#Email").val());

    if (fileimg.length > 0) {
        formData.append("PictureFile", fileimg[0]);
       
    }
    formData.append("PictureName", (fileimg[0] === undefined ? $("#PictureName").val() : fileimg[0].name));


    $.ajax({
        type: "PATCH",
        //url: BaseUrl + "service/usersetup/create",
        url: buildServiceUrl("service/usersetup/save-profile"),
        data: formData,
        contentType: false,
        processData: false,
        //contentType: "application/json; charset=utf-8",
        //dataType: "json",
        //xhrFields: {
        //    withCredentials: true
        //},
        success: function (result) {
          
            if (!result.success) {
                toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });
            }
            else {
                toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });
            }

        }
    })
    
});

//===============================================
//PICTURE FUNC
//===============================================
$('#imageBrowse').change(function () {

    var File = this.files;

    if (File && File[0]) {
        ReadImage(File[0]);
    }
});

$('#imgPreview').click(function () {
    document.getElementById('imageBrowse').click();
});

//read image when add
var ReadImage = function (file) {
    //alert(file);
    var reader = new FileReader;
    var image = new Image;

    reader.readAsDataURL(file);
    reader.onload = function (_file) {

        image.src = _file.target.result;
        image.onload = function () {
            $("#targetImg").attr('src', _file.target.result);
            $("#imgPreview").show();

        }

    }

}

var ClearPreview = function () {
    $("#imageBrowse").val('');
    $("#targetImg").attr('src', '~/content/img/avatar-icon.jpg');
    //$("#imgPreview").hide();
}




