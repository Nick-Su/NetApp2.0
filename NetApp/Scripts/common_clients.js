let searchParamsGidCommon = new URLSearchParams(window.location.search);
searchParamsGidCommon.has('gid');
let gid = searchParamsGidCommon.get('gid');


$("#txtGroupName").val(gid);

$(document).ready(function () {
    WhereIsMyGroup(gid);

    function WhereIsMyGroup(gid) {
        $.getJSON('@Url.Action("WhereIsMyGroup", "Games")',
            { gid: gid },
            function (json) {
                console.log(json);
                var groupNumber = "#" + JSON.parse(json);
                $('.groupNum').html(groupNumber);
            });
    }

    ShowCurrentTact(gid);
    function ShowCurrentTact(gid) {
        $.getJSON('@Url.Action("ShowCurrentTact", "Games")',
            { gid: gid },
            function (json) {
                $('.currentTact').html(json);
            });
    }

    ShowTotalTactNum(gid);
    function ShowTotalTactNum(gid) {
        $.getJSON('@Url.Action("ShowTotalTactNum", "Games")',
            { gid: gid },
            function (json) {
                $('.totalTactNum').html(json);
            });
    }
});