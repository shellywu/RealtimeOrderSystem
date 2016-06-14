/// <reference path="\Scripts/jquery-1.10.2.js" />
/// <reference path="\Scripts/jquery.signalR-2.2.0.js" />
$(function () {
    var con = $.connection("/OrderInfo");
    con.received(function (data) {
        $("#messages").append("<li>" + data + "</li>");
    });
    con.start().done(function () {
        $("#send").click(function () {
            console.log("ok");
            con.send($("#name").text() + ":" + $("#message").val());
        }).error(function (data) {
            alert(data);
        });
    })
});