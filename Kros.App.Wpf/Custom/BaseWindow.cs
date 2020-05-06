using System.Reactive.Disposables;
using System.Windows;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace Kros.App.Wpf
{
    public class BaseWindow<TViewModel> : ReactiveWindow<TViewModel> where TViewModel : class
    {
        public BaseWindow(bool activate = true)
        {
            if (activate)
            {
                this.WhenActivated(disposables =>
                {
                    Disposable.Create(() => { }).DisposeWith(disposables);
                });
            }
        }
    }

    //public class BaseWindow<TViewModel> : MetroWindow, IViewFor<TViewModel> where TViewModel : class
    //{
    //    //public BaseWindow(bool activate = true)
    //    //{
    //    //    if (activate)
    //    //    {
    //    //        this.WhenActivated(disposables =>
    //    //        {
    //    //            Disposable.Create(() => { }).DisposeWith(disposables);
    //    //        });
    //    //    }
    //    //}

    //    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(TViewModel), typeof(MainView), null);

    //    public TViewModel ViewModel {
    //        get => (TViewModel)GetValue(ViewModelProperty);
    //        set => SetValue(ViewModelProperty, value);
    //    }
    //    object IViewFor.ViewModel {
    //        get => ViewModel;
    //        set => ViewModel = (TViewModel)value;
    //    }
    //}
}