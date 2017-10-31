using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BattleOfBalls.Game;

namespace BattleOfBalls.Core
{
    public class Ball
    {
        // 当前所在位置
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        // 半径和颜色
        public double Radius { get; set; }
        public string Color { get; set; }
        // 根据Radius计算
        public double Speed
        {
            get
            {
                return Math.Max(1.2, 2.4 - Math.Sqrt(Radius) * 0.01);
            }
        }
        public double Volume
        {
            get
            {
                return Math.PI * Math.Pow(Radius, 2);
            }
        }
        // 位置移动
        public void Move(double targetX, double targetY)
        {
            double offsetX = 0;
            double offsetY = 0;
            try
            {
                offsetX = (Speed * (targetX - PositionX)) / Math.Sqrt(Math.Pow((targetX - PositionX), 2)
                    + Math.Pow((targetY - PositionY), 2));
                offsetY = (Speed * (targetY - PositionY)) / Math.Sqrt(Math.Pow((targetX - PositionX), 2)
                    + Math.Pow((targetY - PositionY), 2));
            }
            catch (DivideByZeroException)
            {
                offsetX = 0;
                offsetY = 0;
            }

            PositionX += offsetX;
            PositionY += offsetY;

            PositionX = Math.Min(Math.Max(Radius, PositionX), Data.StageWidth - Radius);
            PositionY = Math.Min(Math.Max(Radius, PositionY), Data.StageHeight - Radius);
        }
        // 炸裂
        public Ball Burst(double offsetX, double offsetY)
        {
            if (Volume < 2 * Math.PI * Math.Pow(16, 2)) return null;
            if (offsetX == 0 || offsetY == 0) return null;
            Random random = new Random();
            double radiusRange = Math.Sqrt((Volume - Math.PI * Math.Pow(16, 2)) / Math.PI);
            double _radius = Math.Max(16, random.NextDouble() * radiusRange);
            Radius = Math.Sqrt((Volume - Math.PI * Math.Pow(_radius, 2)) / Math.PI);
            return new Ball()
            {
                Radius = _radius,
                PositionX = this.PositionX + (offsetX / Math.Abs(offsetX)) * this.Radius + offsetX,
                PositionY = this.PositionY + (offsetY / Math.Abs(offsetY)) * this.Radius + offsetY,
                Color = this.Color
            };
        }
    }
}
