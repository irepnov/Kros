using System.Reactive;
using System.Reactive.Disposables;
using Kros.Presentations.Models.Dialogs;
using ReactiveUI;

namespace Kros.Presentations.Models.ConfirmDialog
{
    public class ConfirmDialogModel : DialogModel<bool>
    {
        public ReactiveCommand<Unit, Unit> ConfirmCommand { get; set; }
        
        public ReactiveCommand<Unit, Unit> CancelCommand { get; set; }
        
        public string TitleText { get; set; }
        
        public string MessageText { get; set; }

        public string ConfirmText { get; set; } = "OK";

        public string CancelText { get; set; } = "Cancel";
        
        public ConfirmDialogModel()
        {
            var controller = new ConfirmDialogController(this);
            
            this.WhenActivated(disposabels =>
            {
                controller.BindConfirmCommand()
                    .DisposeWith(disposabels);

                controller.BindCancelCommand()
                    .DisposeWith(disposabels);
            });
        }
    }
}