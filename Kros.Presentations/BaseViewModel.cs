using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Kros.Presentations
{
    public class BaseViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
        /*
        public ViewModelActivator Activator { get; }
        public BaseViewModel()
        {
            Activator = new ViewModelActivator();
        }
        */
    }
}
