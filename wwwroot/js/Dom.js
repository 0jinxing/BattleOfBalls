var $start = $("#start");
var $stage= $("#stage");

$start.click(function () {
    if (!$("#user_name").val()) {
        $("#input_tab").addClass("has-error");
        $start.addClass("btn-danger");
        $("#error").removeClass("hidden");
        return;
    }
    $("#input_tab").removeClass("has-error");
    $start.removeClass("btn-danger");
    $("#error").addClass("hidden");

    controlMsg.userName = !$("#user_name").val() ? "ÄäÃûÍæ¼Ò" : $("#user_name").val();
    controlMsg.inGame = true;
    socket.send(JSON.stringify(controlMsg));
});
var controlTimer;
$stage.mousemove(function (event) {
    event = event || window.event;
    var offsetX = event.offsetX;
    var offsetY = event.offsetY;
    controlMsg.targetX = _leftX + offsetX * _visionScale;
    controlMsg.targetY = _leftY + offsetY * _visionScale;
    if (controlTimer) clearInterval(controlTimer);
    controlTimer = setInterval(function () {
        controlMsg.targetX = _leftX + offsetX * _visionScale;
        controlMsg.targetY = _leftY + offsetY * _visionScale;
    }, 140);
})
$(document).keyup(function (event) {
    event = event || window.event;
    if (event.keyCode == 65) {
        if (controlMsg.isBurst) return;
        controlMsg.isBurst = true;
    }
})

var domTimer = setInterval(function () {
    if (controlMsg.inGame) {
        $("#input_tab").hide();
        $("#game_msg").show();
    } else {
        $("#input_tab").show();
        $("#game_msg").hide();
    }
    var sortPlaters = Array.prototype.slice.call(gameData.GamePlayers);
    sortPlaters.sort(function (p0, p1) {
        return -(p0.Score - p1.Score);
    });
    var listHtmlStr = "";
    for (var _i = 0; _i < sortPlaters.length; _i++) {
        listHtmlStr += "<li>" + sortPlaters[_i].UserName + "</li>";
    }
    $("#range_list").html(listHtmlStr);

}, 120);