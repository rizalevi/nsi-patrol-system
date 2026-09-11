let notificationConnection = null;
let notificationPolling = null;
let isSignalRConnected = false;


function updateNotificationBadge(count) {
    console.log('jumlah count notif');
    console.log(count);

    const badge = $('#notifCount');

    if (count <= 0) {
        badge.text('0').hide();
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
       
        updateNotificationBadge(result.data.Count || 0);
    });
}

function loadNotificationList() {
    $.get(buildServiceUrl('service/notification/list'), function (result) {
        
        let html = '';

        if (result && result.data.length > 0) {

            result.data.forEach(item => {
                
                const formattedDate = formatDateTime(item.CreatedDate); // custom format
                const ago = timeAgo(item.CreatedDate);

                html += `
                <div class="notification-item ${item.IsRead ? 'read' : 'unread'}" data-id="${item.RecipientId}">
                    
                    <div class="notif-title">
                        ${item.Title}
                    </div>

                    <div class="notif-row">
                        <span class="notif-date">
                            ${formattedDate}
                        </span>

                        <span class="notif-message">
                            ${item.Message} (${ago})
                        </span>
                    </div>

                </div>`;
            });

        } else {
            html = '<div class="text-center text-muted">No notifications</div>';
        }

        $('#notificationList').html(html);
    });
}

//function loadNotificationList() {
//    $.get(buildServiceUrl('service/notification/list'), function (result) {
//        let html = '';
        
//        if (result && result.data.length > 0) {
//            result.data.forEach(item => {
//                html += `
//                    <div class="notification-item ${item.IsRead ? 'read' : 'unread'}" data-id="${item.RecipientId}">
//                        <div class="title">${item.Title}</div>
//                        <div class="message">${item.Message}</div>
//                        <small class="text-muted">${timeAgo(item.CreatedDate)}</small>
//                    </div>`;
//            });
//        } else {
//            html = '<div class="text-center text-muted">No notifications</div>';
//        }

//        $('#notificationList').html(html);
//    });
//}
//function prependNotificationToList(notification) {
//    if (!$('#notificationList').length) return;

//    const emptyText = $('#notificationList .text-muted');
//    if (emptyText.length) {
//        emptyText.remove();
//    }

//    const html = `
//        <div class="notification-item unread" data-id="${notification.recipientId || 0}">
//            <div class="title">${notification.title}</div>
//            <div class="message">${notification.message}</div>
//            <small class="text-muted">Just now</small>
//        </div>`;

//    $('#notificationList').prepend(html);
//}

function prependNotificationToList(notification) {
 
    const emptyText = $('#notificationList .text-muted');
    if (emptyText.length) emptyText.remove();

    const html = `
        <div class="notification-item unread">
            <div class="title">${notification.Title}</div>
            <div class="message">${notification.Message}</div>
            <small class="text-muted">${timeAgo(notification.CreatedDate)}</small>
        </div>`;

    $('#notificationList').prepend(html);
}

function markNotificationListAsRead() {
    $('#notificationList .notification-item.unread')
        .removeClass('unread')
        .addClass('read');
}

function markAllNotificationsRead() {
    $.post(buildServiceUrl('service/notification/mark-all-read'), function () {
        updateNotificationBadge(0);
        markNotificationListAsRead();
    });
}

async function initializeNotificationHub() {

    if (typeof signalR === 'undefined') {
        console.warn('SignalR not loaded');
        return;
    }

    notificationConnection = new signalR.HubConnectionBuilder()
        .withUrl(ApiUrl+'notificationHub')
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

    // RECEIVE NOTIFICATION
    notificationConnection.on('ReceiveNotification', function (notification) {
        prependNotificationToList(notification);
        console.log(notification)
        $.smallBox({
            title: notification.title,
            content: `<i class="fa fa-bell"></i> ${notification.message}`,
            color: '#3276B1',
            timeout: 5000
        });
    });

    // UPDATE BADGE (INI SINGLE SOURCE OF TRUTH)
    notificationConnection.on('NotificationCountUpdated', function (count) {
        updateNotificationBadge(count);
    });

    // MARK READ
    notificationConnection.on('NotificationsMarkedRead', function () {
        updateNotificationBadge(0);
        markNotificationListAsRead();
    });

    //RECONECCTING
    notificationConnection.onreconnecting((err) => {
        console.log("Reconnecting...", err);
    });


    // RECONNECT
    notificationConnection.onreconnected(function () {
        console.log('SignalR reconnected');
        isSignalRConnected = true;

    });

    notificationConnection.onclose(function () {
        console.warn('SignalR disconnected');
        isSignalRConnected = false;
    });

    try {
        await notificationConnection.start();
        isSignalRConnected = true;
        console.log('SignalR connected');
    } catch (err) {
        console.error('SignalR failed:', err);
    }
}

//////ga perlu pake ini lagi
//function startNotificationPolling() {
//    if (notificationPolling) {
//        clearInterval(notificationPolling);
//    }

//    notificationPolling = setInterval(function () {
//        if (!notificationConnection ||
//            notificationConnection.state !== signalR.HubConnectionState.Connected) {
//            loadNotificationCount();
//        }
//    }, 30000);
//}

$(function () {

    //console.log("INIT NOTIFICATION");

    initializeNotificationHub();

    // OPTIONAL: initial load sekali saja

    loadNotificationCount();
    loadNotificationList();

    $('#btnNotification').on('click', function (e) {
        e.preventDefault();
      
        const btn = $(this);
        const offset = btn.offset();

        $('#notificationModal').modal('show');

        $('#notificationModal').one('shown.bs.modal', function () {
            const modal = $(this).find('.modal-dialog');

            modal.css({
                position: 'absolute',
                top: offset.top + btn.outerHeight() + 10,
                left: offset.left - modal.outerWidth() + btn.outerWidth(),
                margin: 0
            });
        });


        //$('#notificationModal').modal({
        //    backdrop: false
        //}).modal('show');

        loadNotificationList();
        markAllNotificationsRead();
    });
});

function timeAgo(dateString) {
    const date = new Date(dateString);
    const now = new Date();
    const diff = (now - date) / 1000;

    if (diff < 60) return "just now";
    if (diff < 3600) return Math.floor(diff / 60) + "m ago";
    if (diff < 86400) return Math.floor(diff / 3600) + "h ago";
    return Math.floor(diff / 86400) + "d ago";
}

//$(async function () {
//    debugger
//    console.log("INIT START");

//    await initializeNotificationHub();

//    loadNotificationCount();
//    startNotificationPolling();

//    $('#btnNotification').on('click', function (e) {
//        e.preventDefault();

//        $('#notificationModal').modal({
//            backdrop: false
//        }).modal('show');

//        loadNotificationList();
//        markAllNotificationsRead();
//    });
//});
