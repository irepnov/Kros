using Kros.Presentations;
using Kros.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Windows;
using System.Linq;

namespace Kros.App.Wpf
{
    public partial class App : Application
    {
        public App() => InitializeComponent();

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            var current = CurrentThreadScheduler.Instance;
            var main = RxApp.MainThreadScheduler;
            MainViewModel mainViewModel = new MainViewModel();
            MainView window = new MainView { DataContext = mainViewModel };
            window.Closed += delegate { Shutdown(); };
            window.Show();
        }
    }
}
