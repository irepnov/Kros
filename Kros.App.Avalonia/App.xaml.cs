using ReactiveUI;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Kros.App.Avalonia.Views;
using System.Reactive.Concurrency;
using Kros.Presentations;
using Kros.Services;
using Splat;

namespace Kros.App.Avalonia
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                /* var main = RxApp.MainThreadScheduler;
                 MainViewModel mainViewModel = new MainViewModel(
                     new PhoneRepositoryAsync(new ApplicationContext()),
                     main
                 );
                 MainView window = new MainView { DataContext = mainViewModel };*/
                // MainView window = new MainView { DataContext = Locator.Current.GetService<IMainViewModel>() };
                MainView window = new MainView { DataContext = new MainViewModel() };
                window.Show();
            }

            base.OnFrameworkInitializationCompleted();
        }


    }
}
