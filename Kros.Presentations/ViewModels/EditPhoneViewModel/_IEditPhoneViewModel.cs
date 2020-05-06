using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Text;
using System.Windows.Input;

namespace Kros.Presentations
{
    public interface _IEditPhoneViewModel //: INotifyPropertyChanged
    {
       /* int Id { get; set; }
        string Title { get; set; }
        string Company { get; set; }
        int Price { get; set; }*/

        RxPhone SelectedPhone { get; set; }
        ReactiveCommand<Unit, int> SaveCommand { get; set; }
        ReactiveCommand<Unit, Unit> CancelCommand { get; set; }
    }
}
