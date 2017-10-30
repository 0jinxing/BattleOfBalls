using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleOfBalls.Core
{
    public class Bomb
    {
        // 位置
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        // 半径和颜色
        public double Radius { get; set; }
        public string Color { get; set; }
    }
}
