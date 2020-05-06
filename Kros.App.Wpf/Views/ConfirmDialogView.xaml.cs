using Kros.Presentations;
using Kros.Services;
using Kros.Presentations.Dialogs;
using ReactiveUI;
using MahApps.Metro.Controls;
using System.Windows;
using System.ComponentModel;
using System;

namespace Kros.App.Wpf
{
    [DialogWindow(typeof(ConfirmDialogModel))]
    public partial class ConfirmDialogView : MetroWindow, IViewFor<ConfirmDialogModel>
    {
        private bool _resultWasSet;
        public ConfirmDialogView()
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
            DataContextChanged += (sender, args) => ViewModel = (ConfirmDialogModel)DataContext;
        }
        protected override void OnClosed(EventArgs e)
        {
            //если нажали кнопку Закрыть окно, то автоматиччески подставим результат Отмена
            if (ViewModel != null && !_resultWasSet)
            {
                // ViewModel.Close(Unit.Default);
                //_resultWasSet = true;
                ViewModel.CloseDialog(false);
            }
            base.OnClosed(e);
        }




        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ConfirmDialogModel), typeof(ConfirmDialogView), null);
        public ConfirmDialogModel ViewModel
        {
            get => (ConfirmDialogModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ConfirmDialogModel)value;
        }
    }
}