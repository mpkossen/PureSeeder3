using PureSeeder.Core.Context;
using System.Threading.Tasks;

namespace PureSeeder.Core.ServerManagement
{
    public interface IServerStatusUpdater
    {
        Task UpdateServerStatuses(IDataContext context);
    }
}