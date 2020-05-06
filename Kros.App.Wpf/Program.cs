using Kros.Services;
using Splat;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.App.Wpf
{
    public sealed class Program
    {
        [STAThread]
        public static void Main(string[] args) 
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

                    new App().Run();
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var error = e.ExceptionObject.ToString();
            Console.Error.WriteLine(error);
            Console.Error.WriteLine();

            var file = Path.Combine(ApplicationStorage.Instance.LogDirectory, "fatal.log");
            File.AppendAllText(file, error);
            File.AppendAllText(file, Environment.NewLine);
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var error = e.Exception.ToString();
            Console.WriteLine(error);
            Console.Error.WriteLine();

            var file = Path.Combine(ApplicationStorage.Instance.LogDirectory, "fatal.log");
            File.AppendAllText(file, error);
            File.AppendAllText(file, Environment.NewLine);
        }
    }
}
