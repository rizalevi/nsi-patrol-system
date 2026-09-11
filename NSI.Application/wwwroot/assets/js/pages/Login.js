function showLoading() {
    $("#globalLoading").stop(true, true).css("display", "flex").show().fadeIn(150);
}

function hideLoading() {
    $("#globalLoading").stop(true, true).fadeOut(200);
}
function toggle(id) {
    
    const input = document.getElementById(id);
    input.type = input.type === 'password' ? 'text' : 'password';
}

$(document).on('click', '.toggle-password', function () {
  
    var target = $(this).data('target');
    var input = $(target);

    if (input.attr('type') === 'password') {
        input.attr('type', 'text');
        $(this).find('i')
            .removeClass('fa-eye')
            .addClass('fa-eye-slash');
    } else {
        input.attr('type', 'password');
        $(this).find('i')
            .removeClass('fa-eye-slash')
            .addClass('fa-eye');
    }

});

$("#loginForm").on("submit", function (e) {

    e.preventDefault(); // 🔥 penting supaya tidak reload page

    var token = $('input[name="__RequestVerificationToken"]').val();

    var dataObject = {
        __RequestVerificationToken: token,
        UserID: $("#UserID").val(),
        Password: $("#Password").val()
    };
    // SHOW loading
    showLoading();

    $.ajax({
        url: "/Account/Login",
        type: "POST",
        data: dataObject,
        success: function (result) {
              
            // Login gagal → hide loading
            hideLoading();
            if (result.Status) {
                window.location.href = "/Home/Index";
            } else {
                debugger
                toastr.warning(result.message || "Login failed. please try again in a few minutes", "Warning", {
                    timeOut: 3000,
                    closeButton: true
                });

            }

        },
        error: function (xhr) {
            hideLoading();

            //console.log("Status:", xhr.status);
            //console.log("Status Text:", xhr.statusText);
            //console.log("Response Text:", xhr.responseText);
            //console.log("Response JSON:", xhr.responseJSON);
            //console.log("Full XHR:", xhr);
            console.log(xhr.responseJSON.Message);
            if (xhr.responseJSON.Message != "Invalid User ID or Password") {
                toastr.warning("Login failed. please try again in a few minutes", "Warning");
            } else {
                toastr.warning(xhr.responseJSON.Message, "Warning");
            }

            //if (xhr.status === 400) {
            //toastr.warning("Login failed. please try again in a few minutes", "Warning");
            //}
            //else if (xhr.status === 401) {
            //    toastr.warning("User ID or password incorrect", "Warning");
            //}
            //else {
            //    toastr.error("Server error", "Error");
            //}

        }
    });

});

//no back
history.pushState(null, null, $(location).attr('href'));
window.addEventListener('popstate', function () {
    history.pushState(null, null, $(location).attr('href'));
});

