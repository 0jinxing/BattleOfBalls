var tmpPlayers = null;
// 双缓冲,离屏的canvas
var tmp_background_canvas = document.createElement("canvas");
tmp_background_canvas.width = _stageWidth;
tmp_background_canvas.height = _stageHeight;
var tmp_background_context = tmp_background_canvas.getContext("2d");

var tmp_prop_canvas = document.createElement("canvas");
tmp_prop_canvas.width = _stageWidth;
tmp_prop_canvas.height = _stageHeight;
var tmp_prop_context = tmp_prop_canvas.getContext("2d");

var tmp_player_canvas = document.createElement("canvas")
tmp_player_canvas.width = _stageWidth;
tmp_player_canvas.height = _stageHeight;
var tmp_player_context = tmp_player_canvas.getContext("2d");

// 显示到浏览器canvas
var $background_canvas = document.getElementById("background");
var $prop_canvas = document.getElementById("prop");
var $player_canvas = document.getElementById("player");

var background_context = $player_canvas.getContext("2d");
var prop_context = $prop_canvas.getContext("2d");
var player_context = $background_canvas.getContext("2d");


// 背景绘制
(function drawBackground() {
    // configure
    tmp_background_context.strokeStyle = "#efefef";
    tmp_background_context.lineWidth = 2;

    tmp_background_context.beginPath();
    for (_i = 0; _i < _stageWidth; _i += 32) {
        tmp_background_context.moveTo(_i, 0);
        tmp_background_context.lineTo(_i, _stageHeight);
    }
    for (_i = 0; _i < _stageHeight; _i += 32) {
        tmp_background_context.moveTo(0, _i);
        tmp_background_context.lineTo(_stageWidth, _i);
    }
    tmp_background_context.closePath();
    tmp_background_context.stroke();
})();
var animationCount = 0
function drawFillArc(context, x, y, radius, color) {
    context.fillStyle = color;
    context.beginPath();
    context.arc(x, y, radius, 0, Math.PI * 2);
    context.closePath();
    context.fill();
    // 画边框
    context.beginPath();
    context.arc(x, y, radius * (1 - 1 / 16), 0, Math.PI * 2);
    context.strokeStyle = "rgba(255,255,255,0.2)"
    if (animationCount % 4000 < 2000) {
        context.lineWidth = radius / 32
    }
    else {
        context.lineWidth = radius / 8
    }
    animationCount++;
    animationCount %= 4000;
    context.closePath();
    context.stroke();
}
function flush(x, y, scale) {
    // 清空
    background_context.clearRect(0, 0, _width, _height);
    prop_context.clearRect(0, 0, _width, _height);
    player_context.clearRect(0, 0, _width, _height);
    // 绘制
    background_context.drawImage(tmp_background_canvas, x / 1.4, y / 1.4, scale * _width, scale * _height, 0, 0, _width, _height);
    prop_context.drawImage(tmp_prop_canvas, x, y, scale * _width, scale * _height, 0, 0, _width, _height);
    player_context.drawImage(tmp_player_canvas, x, y, scale * _width, scale * _height, 0, 0, _width, _height);
}

function gameUpdate() {
    try {
        if (!gameData.GamePlayers) return;
        if (!tmpPlayers || tmpPlayers.length != gameData.GamePlayers.length) {
            tmpPlayers = gameData.GamePlayers;
            return;
        }
        for (var _i = 0; _i < tmpPlayers.length; _i++) {
            var player0 = gameData.GamePlayers[_i];
            var player1 = tmpPlayers[_i];
            player1.Score = player0.Score;
            if (player0.Balls.length != player1.Balls.length) {
                tmpPlayers[_i] = player0;
                continue;
            }
            var tmpMaxBall = null;
            for (var _j = 0; _j < player1.Balls.length; _j++) {
                var ball0 = player0.Balls[_j];
                var ball1 = player1.Balls[_j];
                ball1.Speed = ball0.Speed;
                ball1.Radius = ball0.Radius;
                player1.TargetX = player0.TargetX;
                player1.TargetY = player0.TargetY;
                var targetX = player1.TargetX;
                var targetY = player1.TargetY;
                var offsetX = (ball1.Speed * (targetX - ball1.PositionX)) / Math.sqrt(Math.pow((targetX - ball1.PositionX), 2)
                    + Math.pow((targetY - ball1.PositionY), 2));
                var offsetY = (ball1.Speed * (targetY - ball1.PositionY)) / Math.sqrt(Math.pow((targetX - ball1.PositionX), 2)
                    + Math.pow((targetY - ball1.PositionY), 2));

                if (Math.sqrt(Math.pow(ball0.PositionX - ball1.PositionX, 2)
                    + Math.pow(ball0.PositionY - ball1.PositionY, 2)) > 8) {
                    ball1.PositionX += (offsetX + (ball0.PositionX - ball1.PositionX) / 16);
                    ball1.PositionY += (offsetY + (ball0.PositionY - ball1.PositionY) / 16);
                }
                else {
                    ball1.PositionX += offsetX;
                    ball1.PositionY += offsetY;
                }
                if (Math.abs(ball0.Radius- ball1.Radius) > 0.1) {
                    ball1.PositionX = (ball0.PositionX + ball1.PositionX) / 2;
                    ball1.PositionY = (ball0.PositionY + ball1.PositionY) / 2;
                }

                ball1.PositionX = Math.min(Math.max(ball1.Radius, ball1.PositionX), _stageWidth - ball1.Radius);
                ball1.PositionY = Math.min(Math.max(ball1.Radius, ball1.PositionY), _stageHeight - ball1.Radius);


                if (!tmpMaxBall) tmpMaxBall = ball1;
                if (tmpMaxBall.Radius < ball1.Radius) tmpMaxBall = ball1
            }
            player1.PositionX = tmpMaxBall.PositionX;
            player1.PositionY = tmpMaxBall.PositionY;
        }
    } catch (e) {
        tmpPlayers = gameData.GamePlayers;
    }
}

function gameDisplay() {
    tmp_prop_context.clearRect(0, 0, _stageWidth, _stageHeight);
    tmp_player_context.clearRect(0, 0, _stageWidth, _stageHeight);
    for (var _i = 0; _i < gameData.GameDots.length; _i++) {
        var dot = gameData.GameDots[_i];
        drawFillArc(tmp_prop_context, dot.PositionX, dot.PositionY, dot.Radius, dot.Color);
    }
    for (var _i = 0; _i < gameData.GameBombs.length; _i++) {
        var bomb = gameData.GameBombs[_i];
        drawFillArc(tmp_prop_context, bomb.PositionX, bomb.PositionY, bomb.Radius, bomb.Color);
    }
    if (!tmpPlayers) return;
    for (var _i = 0; _i < tmpPlayers.length; _i++) {
        var player = tmpPlayers[_i];
        for (var _j = 0; _j < player.Balls.length; _j++) {
            var ball = player.Balls[_j];
            drawFillArc(tmp_player_context, ball.PositionX, ball.PositionY, ball.Radius, ball.Color);
        }
    }
    var player = null;
    try {
        player = tmpPlayers[gameData.Index];
    } catch (e) { player = null; }
    if (player != null) {
        _visionScale = Math.pow(player.Score / 24, 1 / 8);
        if (!_visionCenterX || !_visionCenterY) {
            _visionCenterX = player.PositionX;
            _visionCenterY = player.PositionY;
        }
        _visionCenterX = (player.PositionX + _visionCenterX) / 2;
        _visionCenterY = (player.PositionY + _visionCenterY) / 2;
        _leftX = Math.min(Math.max(0, _visionCenterX - (_width / 2) * _visionScale), _stageWidth - _width * _visionScale);
        _leftY = Math.min(Math.max(0, _visionCenterY - (_height / 2) * _visionScale), _stageHeight - _height * _visionScale);
        flush(_leftX, _leftY, _visionScale);
    }
    else flush(_stageWidth / 2 - _width / 2, _stageHeight / 2 - _height / 2, 2);
}
var gameTimer = setInterval(function () {
    gameUpdate();
    gameDisplay();
}, 1000 / 60);