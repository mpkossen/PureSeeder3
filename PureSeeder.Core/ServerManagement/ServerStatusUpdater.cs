using Newtonsoft.Json;
using PureSeeder.Core.Annotations;
using PureSeeder.Core.Configuration;
using PureSeeder.Core.Context;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PureSeeder.Core.ServerManagement
{
    public class ServerStatusUpdater : IServerStatusUpdater
    {
        private readonly IUpdateServerIds _serverIdUpdater;

        public ServerStatusUpdater([NotNull] IUpdateServerIds serverIdUpdater)
        {
            if (serverIdUpdater == null) throw new ArgumentNullException("serverIdUpdater");
            _serverIdUpdater = serverIdUpdater;
        }

        private async Task UpdateServerStatus(ServerStatus serverStatus)
        {
            if (String.IsNullOrEmpty(serverStatus.Id))
                return;

            using (var httpClient = new HttpClient())
            {
                //var address = String.Format(Constants.BattlelogUrlTemplates.ServerStatus, serverStatus.Id);
                var address = String.Format("https://keeper.battlelog.com/snapshot/{0}/", serverStatus.Id);
                var response = await httpClient.GetStringAsync(address).ConfigureAwait(true);

                if (String.IsNullOrEmpty(response))
                    return;

                int count = 0;

                var newServerInfo = System.Json.JsonArray.Parse(response);

                if (newServerInfo["snapshot"]["teamInfo"] != null)
                {
                    count += newServerInfo["snapshot"]["teamInfo"]["1"]["players"].Count;
                    count += newServerInfo["snapshot"]["teamInfo"]["2"]["players"].Count;
                    if (newServerInfo["snapshot"]["teamInfo"].ContainsKey("3"))
                    {
                        count += newServerInfo["snapshot"]["teamInfo"]["3"]["players"].Count;
                    }
                    if (newServerInfo["snapshot"]["teamInfo"].ContainsKey("4"))
                    {
                        count += newServerInfo["snapshot"]["teamInfo"]["4"]["players"].Count;
                    }
                }

                serverStatus.CurPlayers = count;
                serverStatus.ServerMax = newServerInfo["snapshot"]["maxPlayers"];
            }
        }

        public async Task UpdateServerStatuses(IDataContext context)
        {
            await _serverIdUpdater.Update(context);

            var allTasks = context.Session.ServerStatuses.Select(UpdateServerStatus);

            await Task.WhenAll(allTasks);
        }
    }

    public class ServerInfo
    {
        public Snapshot Snapshot { get; set; }
        public ServerSlots Slots { get; set; }
    }

    public class Snapshot
    {
        public TeamInfo TeamInfo { get; set; }
    }

    public class TeamInfo
    {
        [JsonProperty(PropertyName = "2")]
        public TeamFaction TeamFaction { get; set; }
    }

    public class TeamFaction
    {
        public int Faction { get; set; }
    }

    public class ServerSlots
    {
        [JsonProperty(PropertyName = "1")]
        public ServerSlot Queue { get; set; }
        [JsonProperty(PropertyName = "2")]
        public ServerSlot Players { get; set; }
        [JsonProperty(PropertyName = "4")]
        public ServerSlot Commanders { get; set; }
        [JsonProperty(PropertyName = "8")]
        public ServerSlot Spectators { get; set; }

    }

    public class ServerSlot
    {
        public int Current { get; set; }
        public int Max { get; set; }
    }
}