using Avalonia;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Kros.Presentations;
using Kros.Presentations.Dialogs;
using Kros.Services;
using ReactiveUI;

namespace Kros.App.Avalonia.Views
{
    [DialogWindow(typeof(EditPhoneViewModel))]
    public class EditPhoneView : BaseWindow<EditPhoneViewModel>
    {
        private bool _resultWasSet;
        public EditPhoneView()
            : base(false)
        {
            AvaloniaXamlLoader.Load(this);
            this.AttachDevTools(KeyGesture.Parse("Ctrl+Shift+D"));
            this.WhenActivated(disposables =>
            {
                ViewModel.GetResult()
                    .SubscribeWithLog(result =>
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
                ViewModel.CloseDialog(new DialogModelResult<RxPhone>.Cancelled());
            }
            return base.HandleClosing();
        }
    }
}