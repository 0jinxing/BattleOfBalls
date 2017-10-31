using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BattleOfBalls.Core;
using System.Net.WebSockets;

namespace BattleOfBalls.Game
{
    public class Player
    {
        public long ID { get; set; }
        public int Key { get; set; }
        public string UserName { get; set; }
        public long LastUpdateTimeStame { get; set; }
        public List<Ball> Balls { get; set; }
        // 目的地位置
        public double TargetX { get; set; }
        public double TargetY { get; set; }
        public bool IsBurst { get; set; }
        // 根据Balls计算
        public double PositionX
        {
            get
            {
                if (MaxBall != null)
                    return MaxBall.PositionX;
                else
                    return 0;
            }
        }
        public double PositionY
        {
            get
            {
                if (MaxBall != null)
                    return MaxBall.PositionY;
                else
                    return 0;
            }
        }
        public double Score
        {
            get
            {
                double volume = 0;
                foreach (Ball ball in Balls)
                {
                    volume += Math.PI * Math.Pow(ball.Radius, 2);
                }
                return Math.Sqrt(volume / Math.PI);
            }
        }
        public Ball MaxBall
        {
            get
            {
                if (Balls.Count == 0) return null;
                Ball maxBall = Balls[0];
                foreach (Ball ball in Balls)
                {
                    if (ball.Radius > maxBall.Radius)
                    {
                        maxBall = ball;
                    }
                }
                return maxBall;
            }
        }
        public void Fuse()
        {
            Random random = new Random();
            if (Balls.Count < 2) return;
            for (int i = Balls.Count - 1; i >= 0; i--)
            {
                Ball ball = Balls[i];
                if (ball == MaxBall) continue;
                if (random.Next(40*Balls.Count) == 1)
                {
                    double distance = Math.Sqrt(Math.Pow(MaxBall.PositionX - ball.PositionX, 2) +
                             Math.Pow(MaxBall.PositionY - ball.PositionY, 2));
                    if (distance > MaxBall.Radius+ball.Radius) continue;
                    double fuseVolume = ball.Volume + MaxBall.Volume;
                    MaxBall.Radius = Math.Sqrt(fuseVolume / Math.PI);
                    Balls.Remove(ball);
                }
                break;
            }
        }
    }
}
