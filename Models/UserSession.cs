using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    public class UserSession
    {
        public const long TicksPerSecond = 10000000;
        public string UserId { get; set; }
        public long LastConnectedTime { get; set; }

        public UserSession()
        {
            LastConnectedTime = 0;
        }

        public bool IsConnected()
        {
            long gap = DateTime.Now.Ticks - this.LastConnectedTime;
            return gap < TicksPerSecond * 5;
        }
    }
}
