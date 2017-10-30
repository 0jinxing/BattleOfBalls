using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BattleOfBalls.Game.GameSocketMiddleware
{
    public class GameSocketMiddleware
    {
        private readonly RequestDelegate _next;


        public GameSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next?.Invoke(context);
            }
            else
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await Connect(context, webSocket);
            }
        }
        // 客户端数据交换及更新
        public async Task Connect(HttpContext context, WebSocket webSocket)
        {
            byte[] receiveBuffer = new byte[512];

            GameClinetMsg msg = null;
            Player _player = null;
            Random _random = new Random();

            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None); ;

            do
            {
                try
                {
                    string s = System.Text.Encoding.Default.GetString(receiveBuffer, 0, result.Count);
                    while (!result.EndOfMessage)
                    {
                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                        s += System.Text.Encoding.Default.GetString(receiveBuffer, 0, result.Count);
                    }
                    msg = JsonConvert.DeserializeObject<GameClinetMsg>(System.Text.Encoding.Default.GetString(receiveBuffer, 0, result.Count));
                }
                catch (Exception)
                {
                    //continue;
                }
                if (msg.inGame && _player == null)
                {
                    if (msg.id != -1)
                    {
                        _player = Data.GamePlayers.Find(p => p.ID == msg.id && p.Key == msg.key);
                    }
                    else if (_player == null)
                    {
                        _player = Data.NewPlayer;
                        _player.ID = Data.AutoIncrement;
                        _player.Key = (int)(_random.NextDouble() * int.MaxValue);
                        _player.UserName = msg.userName;
                        _player.LastUpdateTimeStame = DateTime.Now.Ticks / 10000000;
                        Data.GamePlayers.Add(_player);
                    }
                }
                if (_player != null)
                {
                    _player.TargetX = msg.targetX;
                    _player.TargetY = msg.targetY;
                    _player.IsBurst = msg.isBurst;
                    _player.LastUpdateTimeStame = DateTime.Now.Ticks / 10000000;
                    if (_player.Balls.Count == 0)
                    {
                        Data.GamePlayers.Remove(_player);
                        _player = null;
                        msg.inGame = false;
                    }
                }
                byte[] sendBuffer = System.Text.Encoding.Default.GetBytes("{"
                    + "\"GameDots\":" + JsonConvert.SerializeObject(Data.GameDots) + ","
                    + "\"GameBombs\":" + JsonConvert.SerializeObject(Data.GameBombs) + ","
                    + "\"GamePlayers\":" + JsonConvert.SerializeObject(Data.GamePlayers) + ","
                    + "\"Index\":" + Data.GamePlayers.IndexOf(_player) + ","
                    + "\"ID\":" + (_player == null ? -1 : _player.ID) + ","
                    + "\"Key\":" + (_player == null ? -1 : _player.Key)
                    + "}");
                try
                {
                    await webSocket.SendAsync(new ArraySegment<byte>(sendBuffer, 0, sendBuffer.Length),
                                        WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (Exception)
                {
                    break;
                }
                // 接收数据
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
            } while (!result.CloseStatus.HasValue);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
