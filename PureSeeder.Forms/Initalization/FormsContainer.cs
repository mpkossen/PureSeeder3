﻿using PureSeeder.Core.Context;
using PureSeeder.Core.ProcessControl;
using PureSeeder.Core.ServerManagement;
using PureSeeder.Core.Settings;
using System.Collections.Generic;

namespace PureSeeder.Forms.Initalization
{
    public static class Bootstrapper
    {
        public static MainForm GetMainForm(IDataContext dataContext)
        {
            return new MainForm(dataContext,
                new ProcessController(),
                new SeederActionFactory());
        }

        public static IDataContext GetDataContext()
        {
            return new SeederContext(
                    new SessionData(),
                    new BindableSettings(
                        new SeederUserSettings()),
                    new List<IDataContextUpdater>
                        {
                            new CurrentBf4UserUpdater(),
                        }.ToArray(),
                    new ServerStatusUpdater(
                        new UpdateServerIds()),
                    new PlayerStatusGetter());
        }
    }
}