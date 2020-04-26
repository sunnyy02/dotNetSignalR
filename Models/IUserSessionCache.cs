using System.Threading;
using System.Threading.Tasks;

namespace SignalRServer.Controllers
{
    public interface IUserSessionCache
    {
        void UpdateCache(string userId);
    }
}