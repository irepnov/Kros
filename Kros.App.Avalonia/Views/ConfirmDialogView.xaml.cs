using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Kros.Presentations;
using Kros.Services;
using Kros.Presentations.Dialogs;
using ReactiveUI;

namespace Kros.App.Avalonia.Views
{
    [DialogWindow(typeof(ConfirmDialogModel))]
    public class ConfirmDialogView : BaseWindow<ConfirmDialogModel>
    {
        private bool _resultWasSet;
        public ConfirmDialogView()
            : base(false)
        {
            AvaloniaXamlLoader.Load(this);
            this.AttachDevTools(KeyGesture.Parse("Ctrl+Shift+D"));
            this.WhenActivated(disposables =>
            {
                ViewModel.GetResult()
                    .SubscribeWithLog(_ =>
                    {
                        _resultWasSet = true;
                        Close();
                    });
            });
        }
        protected override bool HandleClosing()
        {
            //если нажали кнопку Закрыть окно, то автоматиччески подставим результат Отмена
            if (ViewModel != null && !_resultWasSet)
            {
                // ViewModel.Close(Unit.Default);
                //_resultWasSet = true;
                ViewModel.CloseDialog(false);
            }
            return base.HandleClosing();
        }
    }
}