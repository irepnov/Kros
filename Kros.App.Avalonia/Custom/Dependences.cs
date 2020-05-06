using Kros.App.Avalonia.Views;
using Kros.Presentations;
using Kros.Presentations.Dialogs;
using Kros.Services;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Concurrency;
using System.Text;

namespace Kros.App.Avalonia
{
    public class Dependences
    {
        public void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.RegisterLazySingleton<IApplicationStorage>(() => ApplicationStorage.Instance);
            services.Register<Serilog.ILogger>(
                () =>
                {
                    var storage = resolver.GetService<IApplicationStorage>();
                    return new LoggerConfiguration()
                        .WriteTo.Console()
                        .WriteTo.File(Path.Combine(storage.LogDirectory, "application.log"))
                        .CreateLogger();
                });

            services.RegisterLazySingleton<IScheduler>(() => RxApp.MainThreadScheduler);
            services.RegisterLazySingleton<ApplicationContext>(() => new ApplicationContext());
            services.RegisterLazySingleton<IPhoneRepositoryAsync>(() => new PhoneRepositoryAsync(resolver.GetService<ApplicationContext>()));
            
            services.RegisterLazySingleton<IDialogProvider>(() => new DialogProvider(typeof(App).Assembly));           
            services.RegisterLazySingleton<IDialogManager>(() => new DialogManager(resolver.GetService<IDialogProvider>()));

          //  services.RegisterLazySingleton<IMainViewModel>(() => new MainViewModel(resolver.GetService<IPhoneRepositoryAsync>(), resolver.GetService<IScheduler>()));
        }
    }
}
