using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Kros.Presentations.Dialogs;

namespace Kros.App.Wpf
{
    public class DialogManager : IDialogManager
    {
        private readonly IDialogProvider _dialogProvider;
        public DialogManager(IDialogProvider dialogProvider) => _dialogProvider = dialogProvider;
        public IObservable<string> OpenFolder(string startDir = null)
        {
            Task<string> task = Dispatcher.CurrentDispatcher.InvokeAsync<string>(() =>
                {
                    // var topWindow = GetTopWindow();            
                    var dialog = new FolderBrowserDialog
                    {
                        RootFolder = Environment.SpecialFolder.MyComputer,
                        SelectedPath = startDir
                    };
                    string ret = null;
                    if (dialog.ShowDialog() == DialogResult.OK)
                        ret = dialog.SelectedPath;
                    return ret;
                })
                .Task;

            return task
                .ToObservable();
        }

        public IObservable<string> OpenFile(string startDir = null)
        {
            Task<string[]> task = Dispatcher.CurrentDispatcher.InvokeAsync<string[]>(() =>
                {
                    // var topWindow = GetTopWindow();            
                    var dialog = new OpenFileDialog
                    {
                        InitialDirectory = startDir,
                        Multiselect = false
                    };
                    string[] ret = Array.Empty<string>();
                    if (dialog.ShowDialog() == DialogResult.OK)
                        ret = dialog.FileNames;
                    return ret;
                })
                .Task;

            return task
                .ToObservable()
                .Select(files => files?.FirstOrDefault());
        }

        public IObservable<string[]> OpenFiles(string startDir = null)
        {
            Task<string[]> task = Dispatcher.CurrentDispatcher.InvokeAsync<string[]>(() =>
                {
                    // var topWindow = GetTopWindow();            
                    var dialog = new OpenFileDialog
                    {
                        InitialDirectory = startDir,
                        Multiselect = true
                    };
                    string[] ret = Array.Empty<string>();
                    if (dialog.ShowDialog() == DialogResult.OK)
                        ret = dialog.FileNames;
                    //Task.FromResult<string[]>(Array.Empty<string>());
                    return ret;
                })
                .Task;

            return task
                .ToObservable()
                .Select((files) => files ?? new string[0]);
        }
        public IObservable<T> Open<T>(DialogModel<T> dialogModel)
        {
            var contextType = dialogModel.GetType();
            var windowType = _dialogProvider.GetWindowType(contextType);
            /*  return Dispatcher.InvokeAsync(() =>
                  {
                      var topWindow = GetTopWindow();
                      var dialogWindow = (Window)Activator.CreateInstance(windowType);
                      dialogWindow.Owner = topWindow;
                      dialogWindow.DataContext = dialogModel;
                      // HACK: https://github.com/AvaloniaUI/Avalonia/issues/2774
                      if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                      {
                          CenterDialog(dialogWindow, topWindow);
                      }
                      return dialogWindow.ShowDialog();
                  })
                  .ToObservable()
                  .SelectMany(_ => dialogModel.GetResult());*/
            Task task = Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                {
                    var topWindow = GetTopWindow();
                    var dialogWindow = (Window)Activator.CreateInstance(windowType);
                  //  dialogWindow.Owner = topWindow;
                    dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    dialogWindow.DataContext = dialogModel;
                    dialogWindow.ShowDialog();
                })
                .Task;

            return task
                .ToObservable()
                .SelectMany(_ => dialogModel.GetResult());
        }

        //private void CenterDialog(Window dialogWindow, Window parentWindow)
        //{
        //    var toScreen = new Func<Point, PixelPoint>(parentWindow.PlatformImpl.PointToScreen);

        //    var topLeftParent = parentWindow.Position;
        //    var bottomRightParent = parentWindow.Position + toScreen(parentWindow.Bounds.TopRight);
            
        //    var topChild = toScreen(new Point(dialogWindow.MinWidth, dialogWindow.MinHeight));

        //    dialogWindow.WindowStartupLocation = WindowStartupLocation.Manual;
        //    dialogWindow.Position = new PixelPoint(
        //        parentWindow.Position.X + (bottomRightParent.X - topLeftParent.X - topChild.X) / 2,
        //        parentWindow.Position.Y + 100);
        //}

        private Window GetTopWindow()
        {
            var window = System.Windows.Application.Current.Windows.Cast<Window>().Last();
            return window;
        }
    }
}