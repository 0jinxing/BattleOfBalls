using BattleOfBalls.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleOfBalls.Game
{
    public static class Game
    {
        // 游戏的碰撞检测及处理
        public static void Collision()
        {
            for (int i = Data.GamePlayers.Count - 1; i >= 0; i--)
            {
                Player player0 = Data.GamePlayers[i];
                for (int j = player0.Balls.Count - 1; j >= 0; j--)
                {
                    for (int k = Data.GameDots.Count - 1; k >= 0; k--)
                    {
                        // Ball与Dot的碰撞检测
                        Dot dot = Data.GameDots[k];
                        Ball ball = player0.Balls[j];
                        if (ball.Radius <= dot.Radius) continue;

                        double distance = Math.Sqrt(Math.Pow(dot.PositionX - ball.PositionX, 2) +
                             Math.Pow(dot.PositionY - ball.PositionY, 2));
                        // 判断是否碰撞
                        if (distance > Math.Max(dot.Radius, ball.Radius)) continue;
                        // 发生碰撞
                        double volumn = Math.PI * (Math.Pow(dot.Radius, 2) + Math.Pow(ball.Radius, 2));
                        ball.Radius = Math.Sqrt(volumn / Math.PI);
                        Data.GameDots.Remove(dot);
                    }
                    for (int k = Data.GameBombs.Count - 1; k >= 0; k--)
                    {
                        // Ball与Bomb的碰撞检测
                        Bomb bomb = Data.GameBombs[k];
                        Ball ball = player0.Balls[j];
                        if (ball.Radius <= bomb.Radius) continue;

                        double distance = Math.Sqrt(Math.Pow(bomb.PositionX - ball.PositionX, 2) +
                             Math.Pow(bomb.PositionY - ball.PositionY, 2));
                        // 判断是否碰撞
                        if (distance > Math.Max(bomb.Radius, ball.Radius)) continue;
                        // 发生碰撞
                        double volumn = Math.PI * (Math.Pow(bomb.Radius, 2) + Math.Pow(ball.Radius, 2));
                        ball.Radius = Math.Sqrt(volumn / Math.PI);
                        // TODO 炸裂效果
                        Ball bitBall = ball.Burst(bomb.PositionX - ball.PositionX, bomb.PositionY - ball.PositionY);
                        if (bitBall != null)
                        {
                            player0.Balls.Add(bitBall);
                        }
                        Data.GameBombs.Remove(bomb);
                    }
                    // 玩家与玩家之间的碰撞检测
                    for (int k = i - 1; k >= 0; k--)
                    {
                        Player player1 = Data.GamePlayers[k];
                        for (int l = player1.Balls.Count - 1; l >= 0; l--)
                        {
                            // Ball与Ball之间的碰撞检测
                            Ball ball1 = player1.Balls[l];
                            Ball ball0 = player0.Balls[j];
                            if (ball0.Radius == ball1.Radius) continue;

                            double distance = Math.Sqrt(Math.Pow(ball0.PositionX - ball1.PositionX, 2) +
                             Math.Pow(ball0.PositionY - ball1.PositionY, 2));
                            // 判断是否碰撞
                            if (distance > Math.Max(ball0.Radius, ball1.Radius)) continue;
                            // 发生碰撞
                            double volumn = Math.PI * (Math.Pow(ball0.Radius, 2) + Math.Pow(ball1.Radius, 2));
                            if (ball0.Radius > ball1.Radius)
                            {
                                ball0.Radius = Math.Sqrt(volumn / Math.PI);
                                player1.Balls.Remove(ball1);
                            }
                            else if (ball0.Radius < ball1.Radius)
                            {
                                ball1.Radius = Math.Sqrt(volumn / Math.PI);
                                player0.Balls.Remove(ball0);
                            }
                        }
                    }
                }
            }
        }
        // 游戏道具数量的维持
        public static void Maintain()
        {
            int GameDotSum = 256;
            int GameBombSum = 32;
            while (Data.GameDots.Count < GameDotSum)
            {
                Data.GameDots.Add(Data.NewDot);
            }
            while (Data.GameBombs.Count < GameBombSum)
            {
                Data.GameBombs.Add(Data.NewBomb);
            }
        }
        // 玩家状态的更新
        public static void Update()
        {
            long now = DateTime.Now.Ticks / 10000000;
            for (int i = 0; i < Data.GamePlayers.Count; i++)
            {
                Player player = Data.GamePlayers[i];
                // 长时间未连接,删除玩家
                if (now - player.LastUpdateTimeStame > 10)
                {
                    Data.GamePlayers.Remove(player);
                    continue;
                }
                for (int j = 0; j < player.Balls.Count; j++)
                {
                    Ball ball = player.Balls[j];
                    ball.Move(Data.GamePlayers[i].TargetX, Data.GamePlayers[i].TargetY);
                }
                if (player.IsBurst)
                {
                    Ball bitBall = player.MaxBall.Burst(32 * (player.TargetX - player.PositionX) / Math.Abs(player.TargetX - player.PositionX),
                        32 * (player.TargetY - player.PositionY) / Math.Abs(player.TargetY - player.PositionY));
                    if (bitBall != null)
                    {
                        player.Balls.Add(bitBall);
                        player.IsBurst = false;
                    }
                }
                player.Fuse();
            }
        }
    }
}
