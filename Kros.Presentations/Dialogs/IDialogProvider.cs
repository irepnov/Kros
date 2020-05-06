using System;

namespace Kros.Presentations.Dialogs
{
    public interface IDialogProvider
    {
        Type GetWindowType(Type contextType);
    }
}