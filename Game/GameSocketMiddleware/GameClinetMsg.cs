using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleOfBalls.Game.GameSocketMiddleware
{
    public class GameClinetMsg
    {
        public double targetX;
        public double targetY;
        public long id;
        public int key;
        public string userName;
        public bool inGame;
        public bool isBurst;
    }
}
