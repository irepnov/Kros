using Kros.Presentations.Dialogs;
using Kros.Services;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Text;
using System.Windows.Input;

namespace Kros.Presentations
{
    public class EditPhoneViewModel : DialogModel<DialogModelResult<RxPhone>>
    {
        public ReactiveCommand<Unit, object> SaveCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; set; }
        public RxPhone SelectedPhone { get; set; }
        public string Title { get; set; } = "Создать";
        public EditPhoneViewModel(RxPhone phone)
        {
            SelectedPhone = phone;
            var controller = new EditPhoneViewModelController(this);
            this.WhenActivated(disposables =>
              {
                    controller.BindSaveCommand().DisposeWith(disposables);
                    controller.BindCancelCommand().DisposeWith(disposables);
                    controller.BindTitle().DisposeWith(disposables);
              });
        }

    }
}
