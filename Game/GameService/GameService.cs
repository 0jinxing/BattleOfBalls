using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BattleOfBalls.Game.GameService
{
    public static class GameService
    {
        private static void Start()
        {
            Timer gameTimer = new Timer();
            gameTimer.Elapsed += new ElapsedEventHandler(OnGameTimerEvent);
            gameTimer.Interval = 1000 / 60;
            gameTimer.Enabled = true;
        }
        private static int _timer = 0;
        private static void OnGameTimerEvent(object source, ElapsedEventArgs e)
        {
            if (_timer >= 8)
            {
                Game.Maintain();
                Game.Collision();
                _timer = 0;
            }
            Game.Update();
            _timer++;
        }
        public static IServiceCollection AddGameService(this IServiceCollection services)
        {
            Start();
            return services;
        }
    }
}
