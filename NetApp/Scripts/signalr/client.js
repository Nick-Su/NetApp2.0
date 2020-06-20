
$(function () {
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


    var timerHub = $.connection.timerHub;

    $.connection.hub.disconnected(function () {
        setTimeout(function () {
            $.connection.hub.start();
        }, 5000);
    });

    timerHub.client.updateTime = function (time) {
        $('#timer').html('<em>' + htmlEncode(time) + '</em>');
    };
    
    timerHub.client.updateIntroductionGroupTime = function (time, username) {
  
        $(".currentSpeaker").html('<b>' + username + '</b>');
        $('#introTimer').html('<em>' + htmlEncode(time) + '</em>');     
    }

    timerHub.client.askForSpeakersList = function () {
        timerHub.server.getSpeakersList(tid);
    }

    timerHub.client.showIntroductionWarning = function (warning) {
        $('.now-speak-title').text(warning);
    }

    timerHub.client.showWaitingSpeakerList = function (json) {
        let parsed = JSON.parse(json);
        buildWaitingSpeakersTable(parsed);
    }

    function buildWaitingSpeakersTable(parsed) {

        let currentSpeaker = $('.currentSpeaker b').text();
        let junkarr = parsed.wplayers.slice(0);

        let index = 0;
        let arrLength = junkarr.length;
        for (let i = 0; i < arrLength; i++) {
            if (junkarr[i] == currentSpeaker) {
                index = i;
                break;
            }
        }

        index += 1;
        let finalResult = junkarr.splice(index, arrLength - index + 1);

        if (finalResult.length != 0) {
            let tblHtml = '<table class="' + "table table-striped" + '">';
            for (let i = 0; i < finalResult.length; i++) {      
                let nameRow = "<tr><td>";
                nameRow += finalResult[i] + "</td></tr>";
                tblHtml += nameRow;
            }
            tblHtml += "</table><hr />";
            $(".table-waiters-wrap").html(tblHtml);
        } else {
            let el = $('.waiters-wrap');
            el.remove();    
        }
    }

    timerHub.client.deleteCurrentSpeakerInfo = function () {
        let el = document.getElementById('speaker-info');
        el.remove();
       
        let warning = '<h5 class="'+"text-info"+'">Ждем другие группы</h5>';
        $('.wait-other-warning').html(warning);
    }

    timerHub.client.showMyGroupNumber = function (groupNumber, tableId) {
        $(".tid").val(tableId);
        let res = "#" + groupNumber;
        $('.idRoom').html(res);
        //let;
        $('.frameRooms').html('<iframe width="100%" height="100%" frameborder="yes" src="/online/Games/Rooms?idRoom=' + groupNumber+'"></iframe>');
        $('.group-num').val(groupNumber);
    }

    timerHub.client.showStageName = function (stageInfo) {
        $('.currentStage').html(htmlEncode(" | " + stageInfo));
    }

    timerHub.client.showNetworkingProgress = function (currTactNum, totalTactNum) {
        $(".round-label").html("Раунд ");
        $('.currentTact').html(currTactNum + " из " + totalTactNum);
        $(".divider-bar").append('<hr />');
        //$(".divider-bar").append('<hr class="'+"nopadding"+'"/>');
    }

    timerHub.client.returnPlayerToActualStage = function (currentStage) {
        let userPosition = $(".pageName").val();

        if (userPosition == "requestsList" && currentStage != 4) {
            showSyncBtn();
        } else if (userPosition == "trustedPage") {
            showSyncBtn();
        } else {
            checkWhereRedirect(currentStage, userPosition);
        }

        function showSyncBtn() {
            let url = '../Games/WhereAmI/?gid=' + gid;
            let text = "Вернуться " + '<i class="' + "glyphicon glyphicon-refresh" + '"></i>';
            let clname = "btn btn-xs btn-warning animate-flicker syncStage";
            let btnSync = '| <a href="' + url + '" class="' + clname + '">' + text + '</a>';
            $(".syncStageWrap").html(btnSync);
        }

        
    }

    function checkWhereRedirect(currentStage, userPosition) {
        if (currentStage == 0 && userPosition != "waitRoom" && userPosition != "trustedPage") {
            window.location.href = "../Games/WaitRoom/?gid=" + gid;
        }

        if (currentStage == 1 && userPosition != "groupTransition" && userPosition != "trustedPage") {
            window.location.href = "../Games/WaitUntilRedirectTable/?gid=" + gid;
        }

        if (currentStage == 2 && userPosition != "introduction" && userPosition != "trustedPage") {
            window.location.href = "../Games/Introduction/?gid=" + gid;
        }

        if (currentStage == 3 && userPosition != "networking" && userPosition != "trustedPage") {
            window.location.href = "../Games/ShowPlayRoom?gid=" + gid + "&tid=0&tnum=0";
        }

        if (currentStage == 4 && userPosition != "requestsList" && userPosition != "trustedPage") {
           
            window.location.href = "../ConnectionRequests/Index/?gid=" + gid + '&toLeaderBoard=1' + '&after=' + 10;;

        }

        if (currentStage == 5 && userPosition != "endGame" && userPosition != "trustedPage") {
            window.location.href = "../Games/EndGame/?gid=" + gid;
        }
    }

    timerHub.client.redirectToTransition = function (gid) {
        window.location.href = "../Games/WaitUntilRedirectTable/?gid=" + gid;
    }

    timerHub.client.redirectToIntroduction = function (gid) {
        window.location.href = "../Games/Introduction/?gid=" + gid;
    }

    timerHub.client.redirectToPlayRoom = function (gid) {
        window.location.href = "../Games/ShowPlayRoom?gid=" + gid + "&tid=0&tnum=0";
    }

    timerHub.client.redirectToAproveRequests = function (gid) {
        window.location.href = "../Games/TimeToAproveRequests/?gid="+gid;
    }

    timerHub.client.redirectUsersToEndGame = function (gid) {
        window.location.href = "../Games/EndGame/?gid=" + gid;
    }

    $.connection.hub.start().done(function () {

        $(document).ready(function () {

            var name = $("#txtUserName").val();
            var group = $("#txtGroupName").val();

            $("#group").val(group);

            let searchParams = new URLSearchParams(window.location.search);
            searchParams.has('gid');
            let gid = searchParams.get('gid');
           
            if (gid != null) {
                checkGameStatus();
            } else {
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

            function checkGameStatus() {
                $.ajax({
                    type: "GET",
                    url: '../Games/GetGameStatus',
                    contentType: "application/json; charset=utf-8",
                    data: { gameId: gid },
                    dataType: "json",
                    success: successFunc,
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.status);
                        console.log(thrownError);
                    }
                });

                function successFunc(data, status) {
                    //console.log("Game status " + data);
                    if (data == 2) {
                        return true;
                    }
                }
            }

            let groupNum = $(".group-num").val();

            checkGroupNum();
            function checkGroupNum() {
                // console.log(groupNum);
                if (groupNum == "") {
                    setTimeout(extractGroupNum(), 1000);
                } else {
                    if (typeof  tid != 'undefined') {
                        timerHub.server.getSpeakersList(tid);
                    }
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

            if (name.length > 0) {
                timerHub.server.connect(name, group);
                timerHub.server.joinGroup(group);
            }

            timerHub.server.showMyGroupNumber(gid);
            setInterval(getMyGroupNumber, 1000);
            function getMyGroupNumber() {
                if ($('.idRoom').is(':empty')) {
                    timerHub.server.showMyGroupNumber(gid);
                }
                else {
                    return true;
                }
            }

            timerHub.server.getNetworkingProgress(gid);
            setInterval(networkingProgress, 2500);
            function networkingProgress() {
                if ($('.currentTact').is(':empty')) {
                    timerHub.server.getNetworkingProgress(gid);
                }
                else {
                    return true;
                }
            }

            timerHub.server.getStageName(gid);
            setInterval(networkingProgrress, 2500);
            function networkingProgrress() {
                if ($('.currentStage').is(':empty')) {
                    timerHub.server.getStageName(gid);
                }
                else {
                    return true;
                }
            }

            setInterval(checkMyUrl, 2000);
            function checkMyUrl() {
                timerHub.server.getGameStage(gid);
            }
            
        });
    });
});

// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}