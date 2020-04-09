function isOnlyGivenConTypes(gid) {
    $.ajax({
        type: "GET",
        url: '/Games/isOnlyGivenConTypes',
        contentType: "application/json; charset=utf-8",
        data: { gid: gid },
        dataType: "json",
        success: successFunc,
        error: errorFunc
    });

    function successFunc(data, status) {
        //console.log(data);
        if (data) {
            $(".btnShowCustomConFieldWrap").hide();
            $("#userConType").hide();
            $(".typeInputOrSelect").hide();
            $("#btnShowCustomConField").hide();
            $("#givenConType").show();
        }
    }

    function errorFunc() {
        console.log('Не удалось получить список типов связей.');
    }
}

function isOnlyGivenNeedTypes(gid) {
    $.ajax({
        type: "GET",
        url: '/Games/isOnlyGivenNeedTypes',
        contentType: "application/json; charset=utf-8",
        data: { gid: gid },
        dataType: "json",
        success: successFunc,
        error: errorFunc
    });

    function successFunc(data, status) {
        console.log("Need types " + data);
        if (data) {
            $(".btnShowCustomNeedFieldWrap").hide();
        }
    }

    function errorFunc() {
        console.log('Не удалось получить список типов запросов.');
    }
}

function isOnlyGivenBenefitTypes(gid) {
    $.ajax({
        type: "GET",
        url: '/Games/isOnlyGivenBenefitTypes',
        contentType: "application/json; charset=utf-8",
        data: { gid: gid },
        dataType: "json",
        success: successFunc,
        error: errorFunc
    });

    function successFunc(data, status) {
        if (data) {
            $(".btnShowCustomBenefitFieldWrap").hide();
        }
    }

    function errorFunc() {
        console.log('Не удалось получить список типов польз.');
    }
}



