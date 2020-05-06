using Kros.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace Kros.Presentations
{
    public interface _IMainViewModel : INotifyPropertyChanged
    {
        IEnumerable<RxPhone> Phones { get; }
        RxPhone SelectedPhone { get; set; }
        string Title { get; set; }
        string ErrorMessage { get; }
        bool HasErrors { get; }
        bool IsBusy { get; }
        int CountPhones { get; }
        ICommand Refresh { get; }
        ICommand Edit { get; }
    }
}
