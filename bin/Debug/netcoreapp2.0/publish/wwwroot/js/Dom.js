function _$(s) {
    return Array.prototype.slice.call(
        document.querySelectorAll(s)
    );
}

var _$start = _$("#start")[0];
var _$game = _$("#game")[0];

_$start.onclick = function () {
    controlMsg.userName = _$("#user_name")[0].value.trim() || "ÄäÃûÍæ¼Ò";
    controlMsg.inGame = true;
}
var controlTimer;
_$game.onmousemove = function (event) {
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
}
document.onkeyup = function (event) {
    event = event || window.event;
    if (event.keyCode == 65) {
        if (controlMsg.isBurst) return;
        controlMsg.isBurst = true;
    }
}