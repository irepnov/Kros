using System;

namespace Kros.Presentations.Dialogs
{
    public interface IDialogManager
    {
        IObservable<string> OpenFolder(string startDir = null);
        IObservable<string> OpenFile(string startDir = null);
        IObservable<string[]> OpenFiles(string startDir = null);
        IObservable<T> Open<T>(DialogModel<T> dialogModel);
    }
}