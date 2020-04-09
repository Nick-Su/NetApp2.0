$(function () {

    let searchParams = new URLSearchParams(window.location.search);
    searchParams.has('gid');
    let gid = searchParams.get('gid');

    var timerHub = $.connection.timerHub;

    timerHub.client.warnAdminFewPlayers = function () {
        $(".activateButton").show();
        $(".activateWarn").hide();
        alert("Недостаточно игроков! Дождитесь подключения всех игроков.");

    }
    timerHub.client.updateTime = function (time) {
        $('.timer').html('<em>' + htmlEncode(time) + '</em>');
    };

    timerHub.client.showCurrentStage = function (stageInfo) {
        $('.currentStage').html(htmlEncode(stageInfo));
    }

    timerHub.client.showCurrentTact = function (tactNum) {
        $('.currentTact').html(htmlEncode(tactNum));
    }

    timerHub.client.hideDisclaimer = function () {
        $(".disclaimer").hide();
    }

    timerHub.client.showAdminLeaderBoard = function () {
        $(".gameActive").hide();
        $(".gameInfo").hide();
        $(".activateButton").hide();
        $(".gameFinished").hide();

        $(".code-block").hide();
        $(".tuneGameLists").hide();
        $(".game-data").hide();
        setTimeout(function () { GetUserLeaderBoard(gid); }, 3000);
        setInterval(function () { GetUserLeaderBoard(gid, 0); }, 20000);
        $(".mainTitle").html("Игра завершена.");
        $(".disclaimer").show();
        $(".leaderBoardBlock").show();
    }

    timerHub.client.showGameInfo = function () {
        $(".activateWarn").hide();
        $(".gameActive").show();
        $(".gameInfo").show();
    }

    $.connection.hub.start().done(function () {
     
        $('#runTimer').click(function () {

            $(".activateButton").hide();
            $(".activateWarn").show();

            var group = $('#txtGroupName').val();

            let searchParams = new URLSearchParams(window.location.search);
            searchParams.has('gid');
            let gid = searchParams.get('gid');

            timerHub.server.main(group, gid);
        });

        $(document).ready(function () {

            var name = $("#txtUserName").val();
            var group = $("#txtGroupName").val();

            $("#group").val(group);

            if (name.length > 0) {
                timerHub.server.connect(name, group);
                timerHub.server.joinGroup(group);
            }

        });
    });
});

// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}