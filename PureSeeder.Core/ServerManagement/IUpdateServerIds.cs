using PureSeeder.Core.Context;
using System.Threading.Tasks;

namespace PureSeeder.Core.ServerManagement
{
    public interface IUpdateServerIds
    {
        Task Update(IDataContext context, bool updateAll = false);
    }
}