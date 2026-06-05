// ==============================
// GLOBAL AJAX SETUP
// ==============================
window.Auth = {
    isRedirecting: false,

    logout(message = "Your session has expired. Please login again.") {
        if (this.isRedirecting) return;

        this.isRedirecting = true;

        if (window.Swal) {
            Swal.fire({
                icon: 'warning',
                title: 'Session Expired',
                text: message,
                showConfirmButton: false,
                allowOutsideClick: false,
                allowEscapeKey: false,
                timer: 2000,
                timerProgressBar: true
            });

            setTimeout(() => {
                window.location.replace('/Account/Login');
            }, 2000);
        } else {
            alert(message);
            setTimeout(() => {
                window.location.replace('/Account/Login');
            }, 2000);
        }
    },

    handleHttpError(xhr) {
        switch (xhr.status) {
            case 401:
                this.logout();
                break;
            case 403:
                this.logout('Access denied. You do not have permission to access this resource.');
                break;
            case 419:
            case 440:
                this.logout('Your session has expired. Please login again.');
                break;
        }
    },

    initialize() {
        $.ajaxSetup({
            cache: false,
            xhrFields: {
                withCredentials: true
            }
        });

        $(document).ajaxError((event, xhr) => {
            this.handleHttpError(xhr);
        });
    }
};

$(function () {
    Auth.initialize();
});

$(function () {
    Auth.initialize();
});

function buildServiceUrl(endpoint) {
    const base = BaseUrl.replace(/\/$/, '');
    const path = endpoint.replace(/^\//, '');
 
    return `${base}/${path}`;
}