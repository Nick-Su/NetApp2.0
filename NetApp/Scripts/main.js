//function GetUserLeaderBoard(gid, ifAnimate = 1) {
    
//     $.ajax({
//        type: "GET",
//        url: '/UserRatings/GetLeaderBoard', 
//        contentType: "application/json; charset=utf-8",
//        data: { gid: gid},
//        dataType: "json",
//        success: handleLeaderBoard,
//        error: function (xhr, status, error) {
//            console.log('Неудачный запрос');
//            var err = eval("(" + xhr.responseText + ")");
//            console.log(err.Message);
//        }
//     });

//    function handleLeaderBoard(json) {
//        var leaderBoard = JSON.parse(json);
//        //console.log("Leaderboard");
//        //console.log(ifAnimate);
//        //console.log(leaderBoard);
//        buildLeaderBoard(leaderBoard);
//    }

//    function buildLeaderBoard(leaderBoard) {
//        let animation = "";
//        if (ifAnimate == 1) {
//            animation = "animated fadeInRight";
//        }

//        html = '<h3 class="' + "text-primary "+ animation + "" + '">Рейтинг участников</h3>'
//            + '<table class="' + "table table-responsive table-striped " + animation + "" + '">'
//            + '<tr>'
//            + '<td><b>#</b></td>'
//            + '<td><b>Участник</b></td>'
//            + '<td class="' + "text-center" + '"><b>Рейтинг</b></td>'
//            + '<td></td>';

//        var pos = 0;
//        var maxRating = 0;
//        let downshift = 3;
//        for (let i = 0; i < leaderBoard.length; i++) {
//            pos++;

//            let medal = "";

//            if (leaderBoard[i].rating >= maxRating) {
//                if (downshift == 3) {
//                    medal = '<i class="' + "fa fa-trophy first" + '">'
//                } else if (downshift == 2) {
//                    medal = '<i class="' + "fa fa-trophy second" + '">';
//                } else if (downshift == 1) {
//                    medal = '<i class="' + "fa fa-trophy third" + '">';
//                }

//                maxRating = leaderBoard[i].rating;
//            } else {
//                downshift--;
//                if (downshift == 3) {
//                    medal = '<i class="' + "fa fa-trophy first" + '">'
//                } else if (downshift == 2) {
//                    medal = '<i class="' + "fa fa-trophy second" + '">';
//                } else if (downshift == 1) {
//                    medal = '<i class="' + "fa fa-trophy third" + '">';
//                }
//            }

//            let highlightUser = 'user';
//            if (leaderBoard[i].isItMe == 1) {
//                highlightUser = 'highlightUser';
//            }

//            html += '<tr class="' + highlightUser + '">'
//                + '<td>' + pos + '</td>'
//                + '<td>' + leaderBoard[i].name + " " + leaderBoard[i].lastName + '</td>'
//                + '<td class="' + "text-center" + '">' + leaderBoard[i].rating + '</td>'
//                + '<td>' + medal + '</td>';
//        }

//        html += '</table>';
//        $(".leaderBoardBlock").html(html);
//    }
//}

//function errorFunc(xhr, status, error) {
//    //console.log('Неудачный запрос');
//    var err = eval("(" + xhr.responseText + ")");
//    console.log(err.Message);
//}



//// Tacts info 
//let searchParamsGid = new URLSearchParams(window.location.search);
//searchParamsGid.has('gid');
//let gid = searchParamsGid.get('gid');
