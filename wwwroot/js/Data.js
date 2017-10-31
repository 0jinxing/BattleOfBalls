var _width = 900;
var _height = 600;

var _stageWidth = 1024 * 4;
var _stageHeight = 1024 * 4;

var _visionCenterX = _width / 2;
var _visionCenterY = _height / 2;
var _visionScale = 1;
var _visionWidthX = _width * 0.3;
var _visionWidthY = _height * 0.3;
var _leftX = 0;
var _leftY = 0;

var controlMsg = {
    targetX: 0,
    targetY: 0,
    isBurst: false,
    inGame: false,
    userName: "",
    id: -1,
    key: -1
};

var gameData = {
    GameDots: [],
    GameBombs: [],
    GamePlayers: [],
    Index: -1,
    ID: -1,
    Key: -1
};

var sortPlaters = [];