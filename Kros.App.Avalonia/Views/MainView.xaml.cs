using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Kros.Presentations;
using ReactiveUI;

//chown user:user -R /home/user/netcoreapp3.0
//dotnet /home/user/netcoreapp3.0/Kros.App.Avalonia.dll

namespace Kros.App.Avalonia.Views
{
    public class MainView : BaseWindow<MainViewModel>
    {
        public MainView()
        {
            this.WhenActivated(disposables => {
            
            });
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif
        }
    }
}
