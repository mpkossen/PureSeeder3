﻿using PureSeeder.Core.Configuration;
using PureSeeder.Core.Monitoring;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PureSeeder.Core.ProcessControl
{
    public interface IProcessController
    {
        Task StopGame(bool bfIsRunning);
        Task MinimizeAfterLaunch();
        //Task<bool> BfIsRunning();

        ProcessMonitor GetProcessMonitor();
        IdleKickAvoider GetIdleKickAvoider();
        ReadyUpper GetReadyUpper();
    }

    class ProcessController : IProcessController
    {
        public async Task StopGame(bool bfIsRunning)
        {
            if (!bfIsRunning)
                return;

            var process = Process.GetProcessesByName(Constants.Games.Bf4.ProcessName).FirstOrDefault();

            if (process != null)
                process.Kill();
        }

        public async Task MinimizeAfterLaunch()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5 * 60 * 1000);  // Cancel after 5 mins

            await new GameMinimizer().MinimizeGameOnce(cts.Token, () => Constants.Games.Bf4);
        }

        //        public Task<bool> BfIsRunning()
        //        {
        //            throw new System.NotImplementedException();
        //        }

        public ProcessMonitor GetProcessMonitor()
        {
            return new ProcessMonitor(
                new ICrashDetector[]
                {
                    new CrashDetector(new CrashHandler()), new DetectCrashByFaultWindow(),
                });
        }

        public IdleKickAvoider GetIdleKickAvoider()
        {
            return new IdleKickAvoider();
        }

        public ReadyUpper GetReadyUpper()
        {
            return new ReadyUpper();
        }
    }
}