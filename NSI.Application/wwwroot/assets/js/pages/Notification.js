
$(function () {
    loadNotificationCount();

    setInterval(loadNotificationCount, 30000);

    $('#btnNotification').on('click', function (e) {
        e.preventDefault();

        $('#notificationModal').modal({
            backdrop: false
        }).modal('show');

        loadNotificationList();
        markAllNotificationsRead();
    });
});

function updateNotificationBadge(count) {
    const badge = $('#notifCount');

    if (count <= 0) {
        badge.hide();
        return;
    }

    badge
        .text(count > 99 ? '99+' : count)
        .show()
        .addClass('animate');

    setTimeout(() => {
        badge.removeClass('animate');
    }, 500);
}

function loadNotificationCount() {
    $.get(buildServiceUrl('service/notification/unread-count'), function (result) {
        updateNotificationBadge(result.count || 0);
    });
}

function loadNotificationList() {
    $.get(buildServiceUrl('service/notification/list'), function (result) {
        let html = '';

        if (result && result.length > 0) {
            result.forEach(item => {
                html += `
                    <div class="notification-item">
                        <div class="title">${item.title}</div>
                        <div class="message">${item.message}</div>
                        <small class="text-muted">${item.timeAgo}</small>
                    </div>`;
            });
        } else {
            html = '<div class="text-center text-muted">No notifications</div>';
        }

        $('#notificationList').html(html);
    });
}

function markAllNotificationsRead() {
    $.post(buildServiceUrl('service/notification/mark-all-read'), function () {
        updateNotificationBadge(0);
    });
}