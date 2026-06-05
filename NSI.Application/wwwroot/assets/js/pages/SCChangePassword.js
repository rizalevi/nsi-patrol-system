GlobalDatepicker.init(".datepicker");
GlobalDatepicker.setDefault("#PasswordExpired", 3);

function toggle(id) {
    const input = document.getElementById(id);
    input.type = input.type === 'password' ? 'text' : 'password';
}

const newPassword = document.getElementById('newPassword');
const strengthBar = document.getElementById('strengthBar');

newPassword.addEventListener('input', () => {
    const val = newPassword.value;
    let strength = 0;

    if (val.length >= 8) strength++;
    if (/[A-Z]/.test(val)) strength++;
    if (/[0-9]/.test(val)) strength++;
    if (/[^A-Za-z0-9]/.test(val)) strength++;

    const width = strength * 25;
    strengthBar.style.width = width + '%';

    if (width <= 25) strengthBar.style.background = 'red';
    else if (width <= 50) strengthBar.style.background = 'orange';
    else if (width <= 75) strengthBar.style.background = 'yellow';
    else strengthBar.style.background = 'green';
});

function validateForm() {
    const oldPass = document.getElementById('oldPassword').value;
    const newPass = document.getElementById('newPassword').value;
    const confirmPass = document.getElementById('confirmPassword').value;
    const message = document.getElementById('message');

    message.innerHTML = '';

    if (!oldPass || !newPass || !confirmPass) {
        message.innerHTML = '<div class="error">All fields are required</div>';
        return;
    }

    if (newPass.length < 8 || !/[A-Za-z]/.test(newPass) || !/[0-9]/.test(newPass)) {
        message.innerHTML = '<div class="error">Password must meet policy</div>';
        return;
    }

    if (newPass === oldPass) {
        message.innerHTML = '<div class="error">New password must differ from old</div>';
        return;
    }

    if (newPass !== confirmPass) {
        message.innerHTML = '<div class="error">Passwords do not match</div>';
        return;
    }

    message.innerHTML = '<div class="success">Password updated successfully 🎉</div>';
}
// ==========================================
// EVENT LISTENERS
// ==========================================

//date picker
$(document).on("click", ".datepicker-icon", function () {
    $(this).closest(".datepicker-wrapper")
        .find(".datepicker")
        .datepicker("show");
});
//prevent default submit form
$("#ChangePassword").on("submit", function (e) {
    e.preventDefault(); // 🔥 WAJIB
});

//$("#ChangePassword").on("submit", function (e) {
$(document).on("click", ".btn-save", function () {
    
    let form = document.querySelector("form");

    if (!form.checkValidity()) {
        form.reportValidity(); // 🔥 tampilkan pesan required
        return;
    }
    
    let oldPass = $("#oldPassword").val();
    let newPass = $("#newPassword").val();
    let confirmPass = $("#confirmPassword").val();

    
    if (newPass.length < 8 || !/[A-Za-z]/.test(newPass) || !/[0-9]/.test(newPass)) {
        toastr.warning('Password must meet policy', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    if (newPass === oldPass) {
        toastr.warning('New password must different from old password', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    if (newPass !== confirmPass) {
        toastr.warning('Passwords do not match', 'Warning', { timeOut: 3000, "closeButton": true });
        return;
    }

    let model = {
        UserID: $("#UserID").val(),
        OldPassword: $("#oldPassword").val(),
        NewPassword: $("#newPassword").val(),
        ConfirmPassword: $("#confirmPassword").val(),
        PasswordExpired: formatDateGeneral($('#PasswordExpired').val())
    };


    $.ajax({
        type: "POST",
        //url: BaseUrl + "service/changepassword/update-password",
        url: buildServiceUrl("service/changepassword/update-password"),
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify(model),
        success: function (result) {

            if (!result.success) {
                //reset();
                toastr.warning(result.message, 'Warning', { timeOut: 3000, "closeButton": true });
            }
            else {
                //reset();
                toastr.success(result.message, 'Success', { timeOut: 3000, "closeButton": true });

                setTimeout(
                    window.location.href = "/Account/Login"
                    , 2000);
            }

        }
    })
});