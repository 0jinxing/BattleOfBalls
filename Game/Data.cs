using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BattleOfBalls.Core;

namespace BattleOfBalls.Game
{
    public static class Data
    {
        // 用于存放游戏数据
        private static List<Player> _gamePlayers = new List<Player>();
        private static List<Dot> _gameDots = new List<Dot>();
        private static List<Bomb> _gameBombs = new List<Bomb>();

        private static object _lockPlayers = new object();
        private static object _lockDots = new object();
        private static object _lockBombs = new object();

        public static List<Player> GamePlayers
        {
            get
            {
                lock (_lockPlayers)
                {
                    return _gamePlayers;
                }
            }
        }
        public static List<Dot> GameDots
        {
            get
            {
                lock (_lockDots)
                {
                    return _gameDots;
                }
            }
        }
        public static List<Bomb> GameBombs
        {
            get
            {
                lock (_lockBombs)
                {
                    return _gameBombs;
                }
            }
        }

        public static long _autoIncrement = 0;
        public static object _lockAutoIncrement = new object();
        public static long AutoIncrement
        {
            get
            {
                lock (_lockAutoIncrement)
                {
                    return _autoIncrement++;
                }
            }
        }

        // 用于产生游戏所需要的随机性

        private readonly static Random _random = new Random();

        // 舞台的大小
        public readonly static int StageWidth = 1024*4;
        public readonly static int StageHeight = 1024*4;
        // 用于Dot与Ball的颜色
        public readonly static string[] Colors = new string[] { "#F17C67", "#495A80", "#BDB76A", "#FD5B78", "#ACE1AF" };

        public static Dot NewDot
        {
            get
            {
                int radius = 8;
                return new Dot()
                {
                    PositionX = _random.Next(StageWidth - radius),
                    PositionY = _random.Next(StageHeight - radius),
                    Radius = radius,
                    Color = Colors[_random.Next(Colors.Length)]
                };
            }
        }
        public static Bomb NewBomb
        {
            get
            {
                int radius = 24;
                string color = "#404040";
                return new Bomb()
                {
                    PositionX = _random.Next(StageWidth - radius),
                    PositionY = _random.Next(StageHeight - radius),
                    Radius = radius,
                    Color = color
                };
            }
        }
        public static Ball NewBall
        {
            get
            {
                int radius = 16;
                string color = Colors[_random.Next(Colors.Length)];
                return new Ball()
                {
                    PositionX = _random.Next(StageWidth - radius),
                    PositionY = _random.Next(StageHeight - radius),
                    Radius = radius,
                    Color = color
                };
            }
        }
        public static Player NewPlayer
        {
            get
            {
                Player player = new Player();
                player.Balls = new List<Ball>();
                player.Balls.Add(NewBall);
                player.TargetX = player.Balls[0].PositionX;
                player.TargetY = player.Balls[0].PositionY;
                return player;
            }
        }
    }
}
