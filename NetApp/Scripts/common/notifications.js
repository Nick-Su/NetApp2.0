$(document).ready(function () {

    function load_unseen_notification(view = '') {
        $.ajax({
            url: '/Notifications/GetNotifications',
            method: "POST",
            data: { isseen: view },
            dataType: "json",
            success: buildNotifications
        });
    }

    var width = $(window).width() * 0.95;
    $('.dropNotification').css('min-width', width);


    function buildNotifications(data) {

        if (data.length == 2) {

            return false;
        }

        var notifications = JSON.parse(data);
        let output = "";

        for (let i = 0; i < notifications.length; i++) {
            //console.log(notifications[i].type);

            output += '<li>'
                //+ '<a href="' + '#' + '">'
                + '<div class="' + "col-xs-12 notifWrap" + '">'
                + '<a href="' + '/ConnectionRequests/Index' + '">'
                + '<strong  class="' + "col-xs text-primary" + '" >' + notifications[i].title + '</strong><br />'
                + '<small class="' + "col-xs" + '">' + "Между проектом:" + '</small><br />'
                + '<small class="' + "col-xs" + '"><em>' + notifications[i].senderProject + '</em></small><br />'
                + '<small class="' + "col-xs" + '">' + "И проектом:" + '</small><br />'
                + '<small class="' + "col-xs" + '"><em>' + notifications[i].recieverProject + '</em></small><br />'
                + '</a>'
                + '<div class="' + "col-xs-12 divider" + '"></div>'
                + '</div>'
                //+'</a>'

                // + '</a>'
                + '</li>'
            // + '<li class="' + 'divider' +'"></li>';

        }

        if (notifications.length == 0) {
            output = '<li><a href="#" class="text-bold text-italic">Нет уведомлений для показа</a></li>';
        }

        let status = document.getElementById('drop-toggle').getAttribute('aria-expanded');

        if (status == 'false') {
            $('.dropdown-menu').html(output);
        }
 
        if (notifications.length > 0) {
            $('.count').html(notifications.length);
        }
    }

    load_unseen_notification();



    $(document).on('click', '.dropdown-toggle', function () {
        $('.count').html("");
        load_unseen_notification("seen");

    });

    setInterval(function () {
        load_unseen_notification();;
    }, 5000);

});