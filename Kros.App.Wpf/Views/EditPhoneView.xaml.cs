using Kros.Presentations;
using Kros.Presentations.Dialogs;
using Kros.Services;
using MahApps.Metro.Controls;
using ReactiveUI;
using System;
using System.Windows;

namespace Kros.App.Wpf
{
    [DialogWindow(typeof(EditPhoneViewModel))]
    public partial class EditPhoneView : MetroWindow, IViewFor<EditPhoneViewModel>
    {
        private bool _resultWasSet;
        public EditPhoneView()
        {
            this.WhenActivated(disposable => {
                ViewModel.GetResult()
                    .SubscribeWithLog(_ =>
                    {
                        _resultWasSet = true;
                        Close();
                    });
            });

            InitializeComponent();
            DataContextChanged += (sender, args) => ViewModel = (EditPhoneViewModel)DataContext;
        }
        protected override void OnClosed(EventArgs e)
        {
            //если нажали кнопку Закрыть окно, то автоматиччески подставим результат Отмена
            if (ViewModel != null && !_resultWasSet)
            {
                // ViewModel.Close(Unit.Default);
                //_resultWasSet = true;
                ViewModel.CloseDialog(new DialogModelResult<RxPhone>.Cancelled());
            }
            base.OnClosed(e);
        }


        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(EditPhoneViewModel), typeof(EditPhoneView), null);
        public EditPhoneViewModel ViewModel
        {
            get => (EditPhoneViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (EditPhoneViewModel)value;
        }
    }
}