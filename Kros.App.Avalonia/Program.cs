using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using Kros.Services;
using Splat;

namespace Kros.App.Avalonia
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.

        //public static int Main(string[] args) => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

        public static int Main(string[] args)
        {
            Thread.CurrentThread.TrySetApartmentState(ApartmentState.STA);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            var mutex = new Mutex(false, typeof(Program).FullName);

            try
            {
                if (mutex.WaitOne(TimeSpan.FromSeconds(5), true))
                {
                    var registry = new Dependences();
                    registry.Register(Locator.CurrentMutable, Locator.Current);

                    return BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return 0;
        }

        private static void CurrentDomainOnUnhandledException(
            object sender, UnhandledExceptionEventArgs e)
        {
            var error = e.ExceptionObject.ToString();
            Console.Error.WriteLine(error);
            Console.Error.WriteLine();

            var file = Path.Combine(ApplicationStorage.Instance.LogDirectory, "fatal.log");
            File.AppendAllText(file, error);
            File.AppendAllText(file, Environment.NewLine);
        }

        private static void TaskSchedulerOnUnobservedTaskException(
            object sender, UnobservedTaskExceptionEventArgs e)
        {
            var error = e.Exception.ToString();
            Console.WriteLine(error);
            Console.Error.WriteLine();

            var file = Path.Combine(ApplicationStorage.Instance.LogDirectory, "fatal.log");
            File.AppendAllText(file, error);
            File.AppendAllText(file, Environment.NewLine);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();

        /*.UsePlatformDetect()
                .With(new X11PlatformOptions { EnableMultiTouch = true })
                .With(new Win32PlatformOptions
                {
                    EnableMultitouch = true,
                    AllowEglInitialization = true
                })
                .UseSkia()
                .UseReactiveUI();*/


    }
}
