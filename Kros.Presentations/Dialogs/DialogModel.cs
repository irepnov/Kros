using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PropertyChanged;
using ReactiveUI;

namespace Kros.Presentations.Dialogs
{
    [AddINotifyPropertyChangedInterface]
    public abstract class DialogModel<TDialogModel> : BaseViewModel, IActivatableViewModel
    {
        private readonly ReplaySubject<TDialogModel> _result = new ReplaySubject<TDialogModel>();        
        public IObservable<TDialogModel> GetResult()
        {
            return _result.AsObservable().Take(1);
        }
        //public void CloseDialog(TDialogModel result)
        //{
        //    _result.OnNext(result);
        //}
        public bool CloseDialog(TDialogModel result)
        {
            _result.OnNext(result);
            return true;
        }
    }
}