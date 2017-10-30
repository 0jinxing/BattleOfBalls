using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleOfBalls.Game.GameSocketMiddleware
{
    public static class GameSocketMiddlewareExtensions
    {
        public static IApplicationBuilder UseGameSocket(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GameSocketMiddleware>();
        }
    }
}
