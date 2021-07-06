using PureSeeder.Core.Configuration;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PureSeeder.Core.Monitoring
{
    public class IdleKickAvoider
    {
        public Task AvoidIdleKick(CancellationToken token, int numSeconds, Func<GameInfo> getCurrentGame)
        {
            return Task.Factory.StartNew(() =>
            {
                var currentGame = getCurrentGame.Invoke();
                while (!token.IsCancellationRequested)
                {
                    var process = Process.GetProcessesByName(currentGame.ProcessName).FirstOrDefault();

                    if (process != null)
                    {
                        if (currentGame != null)
                        {
                            PInvoke.ClickInWindow(currentGame.WindowTitle, 20, 20);
                            // Click in the Window to avoid the idle kick
                        }
                    }

                    Thread.Sleep(1 * numSeconds * 1000);  // Sleep
                }
            });
        }
    }
}
