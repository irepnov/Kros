using System;
using System.Reactive;
using Kros.Services;
using ReactiveUI;

namespace Kros.Presentations
{
    public class ConfirmDialogController
    {
        private readonly ConfirmDialogModel _model;
        public ConfirmDialogController(ConfirmDialogModel model)
        {
            _model = model;
        }
        public IDisposable BindConfirmCommand()
        {
            _model.ConfirmCommand = ReactiveCommand.Create(() => { _model.CloseDialog(true); }, null, RxApp.MainThreadScheduler);
            return _model.ConfirmCommand.SubscribeWithLog();
        }
        public IDisposable BindCancelCommand()
        {
            _model.CancelCommand = ReactiveCommand.Create(() => { _model.CloseDialog(false); }, null, RxApp.MainThreadScheduler);
            return _model.CancelCommand.SubscribeWithLog();
        }
    }
}