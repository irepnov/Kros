using Kros.Presentations;
using MahApps.Metro.Controls;
using ReactiveUI;
using System.Windows;

namespace Kros.App.Wpf
{
    public partial class MainView : MetroWindow, IViewFor<MainViewModel>
    {
        public MainView()
        {            
            this.WhenActivated(disposable =>{

            });
            
            InitializeComponent();
            DataContextChanged += (sender, args) => ViewModel = (MainViewModel)DataContext;
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(MainViewModel), typeof(MainView), null);
        public MainViewModel ViewModel
        {
            get => (MainViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainViewModel)value;
        }
    }
}
