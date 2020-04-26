using SignalRServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRServer.Controllers
{
    public class UserSessionCache : IUserSessionCache
    {
        private Dictionary<string, UserSession> _userSession;
        private System.Timers.Timer _timer;
        private IHubContext<UserHub> _hub;


        public UserSessionCache(IHubContext<UserHub> hub)
        {
            _userSession = new Dictionary<string, UserSession>();
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.AutoReset = true;

            _hub = hub;
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            PingSubscribers();
        }

        public void UpdateCache(string userId)
        {
            var newSessions = _userSession.Count == 0;
            if (newSessions)
            {
                if (!_timer.Enabled)
                {
                    _timer.Enabled = true;
                    _timer.Start();
                }
            }
            this.AddOrUpdateUserSession(userId);
        }

        private void PingSubscribers()
        {
            if (_userSession.Count() > 0)
            {
                var validSessions = _userSession.ToList().Where(x => x.Value.IsConnected());
                if (validSessions != null)
                {
                    var allIds = string.Join(",", validSessions.Select(x => x.Value.UserId));
                    _hub.Clients.All.SendAsync("notifyAdmin", allIds);
                }
            }
        }

        private void AddOrUpdateUserSession(string userId)
        {
            var existSesssion = _userSession.ContainsKey(userId);
            if (existSesssion)
            {
                var currentSession = _userSession[userId];
                currentSession.LastConnectedTime = DateTime.Now.Ticks;
                currentSession.UserId = userId;
                return;
            }
            var session = new UserSession
            {
                UserId = userId,
                LastConnectedTime = DateTime.Now.Ticks
            };

            _userSession.Add(userId, session);
        }
    }
}
