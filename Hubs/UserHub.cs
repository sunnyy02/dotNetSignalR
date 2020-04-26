using Microsoft.AspNetCore.SignalR;
using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SignalRServer.Controllers;

namespace SignalRServer.Models
{
    public class UserHub : Hub
    {
        private IUserSessionCache _sessionCache;
        public UserHub(IUserSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task UserRegister(string userId)
        {
            _sessionCache.UpdateCache(userId);
        }
    }
}
