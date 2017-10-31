var scheme = document.location.protocol == "https:" ? "wss" : "ws";
var port = document.location.port ? (":" + document.location.port) : "";

var socket;
var syncTimer;
connect();

var lastGetMessage = (new Date()).valueOf();
setInterval(function () {
    if (socket.readyState != 1) return;
    else if ((new Date()).valueOf() - lastGetMessage > 480) {
        console.log("连接中");
        connect();
    }
}, 480);

function connect() {
    socket = new WebSocket(scheme + "://" + document.location.hostname + port);

    socket.onopen = function () {
        syncTimer = setInterval(function () {
            if (socket.readyState == 1) {
                var msg = JSON.stringify(controlMsg);
                try {
                    socket.send(msg);
                } catch (e){ }
                controlMsg.isBurst = false;
            }
        }, 160);
    };

    socket.onmessage = function (event) {
        lastGetMessage = (new Date()).valueOf();
        event = event || window.event;
        try {
            gameData = JSON.parse(event.data);
            if (gameData.ID != -1) {
                controlMsg.inGame = true;
                controlMsg.id = gameData.ID;
                controlMsg.key = gameData.Key;
            }
            else {
                controlMsg.inGame = false;
                controlMsg.id = -1;
                controlMsg.key = -1;
            }
        } catch (e) {
            console.log(e);
        }
    };
    socket.onclose = function () {
        delete socket;
        clearInterval(syncTimer);
        console.log("webSocket断开");
        console.log("重新连接");
        connect();
    };
    socket.onerror = function () {
        delete socket;
        clearInterval(syncTimer);
        console.log("webSocket错误");
        console.log("重新连接");
        connect();
    };
}