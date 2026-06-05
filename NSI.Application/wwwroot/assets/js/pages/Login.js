
$("#loginForm").on("submit", function (e) {

    e.preventDefault(); // 🔥 penting supaya tidak reload page

    var token = $('input[name="__RequestVerificationToken"]').val();

    var dataObject = {
        __RequestVerificationToken: token,
        UserID: $("#UserID").val(),
        Password: $("#Password").val()
    };

    $.ajax({
        url: "/Account/Login",
        type: "POST",
        data: dataObject,
        success: function (result) {
                
                   debugger
            if (result.Status) {
                window.location.href = "/Home/Index";
            } else {

                toastr.warning(result.message || "Login failed", "Warning", {
                    timeOut: 3000,
                    closeButton: true
                });

            }
        },
        error: function (xhr) {
                  
            if (xhr.status === 400) {
                toastr.warning(xhr.responseJSON?.Message ?? "Invalid login", "Warning");
            }
            else if (xhr.status === 401) {
                toastr.warning("User ID or password incorrect", "Warning");
            }
            else {
                toastr.error("Server error", "Error");
            }

        }
    });

});

//no back
history.pushState(null, null, $(location).attr('href'));
window.addEventListener('popstate', function () {
    history.pushState(null, null, $(location).attr('href'));
});

