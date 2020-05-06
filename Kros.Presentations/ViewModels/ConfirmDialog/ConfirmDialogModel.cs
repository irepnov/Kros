using System.Reactive;
using System.Reactive.Disposables;
using Kros.Presentations.Dialogs;
using ReactiveUI;

namespace Kros.Presentations
{
    public class ConfirmDialogModel : DialogModel<bool>
    {
        public ReactiveCommand<Unit, Unit> ConfirmCommand { get; set; }        
        public ReactiveCommand<Unit, Unit> CancelCommand { get; set; }        
        public string TitleText { get; set; }        
        public string MessageText { get; set; }
        public string ConfirmText { get; set; } = "ОК";
        public string CancelText { get; set; } = "Отмена";        
        public ConfirmDialogModel()
        {
            var _controller = new ConfirmDialogController(this);            
            this.WhenActivated(disposabels =>
            {
                _controller.BindConfirmCommand().DisposeWith(disposabels);
                _controller.BindCancelCommand().DisposeWith(disposabels);
            });
        }
    }
}