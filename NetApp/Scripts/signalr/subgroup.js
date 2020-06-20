let tid;

$(function () {
  
    var subtimerHub = $.connection.timerHub;

    $.connection.hub.disconnected(function () {
        setTimeout(function () {
            $.connection.hub.start();
        }, 5000);
    });

    $.connection.hub.start().done(function () {

        $(document).ready(function () {

            let searchParams = new URLSearchParams(window.location.search);
            searchParams.has('gid');
            let gid = searchParams.get('gid');

            if (gid == null) {
                GetGidFromServer();
            } 

            function GetGidFromServer() {
                $.ajax({
                    type: "GET",
                    url: '../Games/BackToGame',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: successFunc,
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.status);
                        console.log(thrownError);
                    }
                });

                function successFunc(json) {
                    gid = JSON.parse(json);
                    console.log("Server gid " + gid);
                }
            }

            let groupNum = $(".group-num").val();

            checkGroupNum();
            function checkGroupNum() {
                if (groupNum == "") {

                    setTimeout(extractGroupNum(), 1000);
                } else {
                    connectToHub();
                }
            }

            function extractGroupNum() {
                groupNum = $(".group-num").val();
                setTimeout(checkGroupNum, 1000);
            }

            tid = $(".tid").val();
            checkTid();
            function checkTid() {
   
                if (tid == "") {
                    setTimeout(extractTid(), 1000);
                } 
            }

            function extractTid() {
                tid = $(".tid").val();
                setTimeout(checkTid, 1000);
            }

            function connectToHub() {
                var name = $("#txtUserName").val();
                var subgroup = gid + groupNum;
                console.log("subgroup " + subgroup);

                if (name.length > 0) {
                    console.log("Conn");
                    subtimerHub.server.connect(name, subgroup);
                    subtimerHub.server.joinGroup(subgroup);
                    subtimerHub.server.getSpeakersList(tid);
                }
            }
        });
    });
});

// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}