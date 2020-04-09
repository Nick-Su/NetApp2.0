function getName(handleData, pid) {
    $.ajax({
        type: "GET",
        url: '/Projects/FindProjectNameById',
        contentType: "application/json; charset=utf-8",
        data: { pid: pid },
        dataType: "json",
        success: function (data) {
            handleData(data);
        }
    });
}