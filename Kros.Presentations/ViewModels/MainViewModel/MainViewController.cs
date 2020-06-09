using Kros.Presentations.Dialogs;
using Kros.Presentations;
using Kros.Services;
using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Linq.Expressions;
using System.Reactive.Concurrency;

namespace Kros.Presentations
{
    public sealed class MainViewController
    {
        private readonly IPhoneRepositoryAsync _repository;
        private readonly MainViewModel _model;
        private readonly IDialogManager _dialogManager;
        public MainViewController(IPhoneRepositoryAsync repository, IDialogManager dialogManager)
        {
            _repository = repository;
            _dialogManager = dialogManager;
        }
        public MainViewController(MainViewModel model)
            : this(Locator.Current.GetService<IPhoneRepositoryAsync>(),
                   Locator.Current.GetService<IDialogManager>())
        {
            _model = model;
        }
        public MainViewController(IPhoneRepositoryAsync repository, IDialogManager dialogManager, MainViewModel model)
            : this(repository,
                   dialogManager)
        {
            _model = model;
        }
        //привяжем отслеживание заголовка и автоматическую настройку фильтров для поиска
        public IDisposable BindTitle()
        {
            return _model.WhenAnyValue(m => m.Title)
                .SubscribeWithLog(title =>
                {
                   /* if (string.IsNullOrWhiteSpace(title) || title == "…")
                        _model.Title = "Radish";
                    else
                        _model.Title = "Radish - " + title;*/
                });
        }
        public IDisposable BindTestCommand()
        {
            _model.TestCommand = ReactiveCommand.Create(
                () => {
                    if (_model.SelectedPhone.Id > 3)
                        _model.Title = "1";
                    else
                        _model.Title = "dfgdfg";
                }
                , null, RxApp.MainThreadScheduler);
            return _model.TestCommand.SubscribeWithLog();
        }

        public IDisposable BindRefreshFilter() /// перенесено в BindTitle
        {
            return _model.Filter.WhenAnyValue(m => m.Id, m => m.Title)
                .SubscribeWithLog(filter =>
                {
                    _model.RefreshFilter.Clear();
                    if (filter.Item1 > 0)
                        _model.RefreshFilter.Add(phone => phone.Id >= filter.Item1);
                    if (!String.IsNullOrEmpty(filter.Item2))
                        _model.RefreshFilter.Add(phone => phone.Title.Contains(filter.Item2));
                });
        }
        public IDisposable BindRefreshCommand()
        {
            _model.RefreshCommand = ReactiveCommand.CreateFromTask(
                () => _repository.GetAsEnumerable(_model.RefreshFilter, p => p.Display)
                , null, RxApp.MainThreadScheduler);
            return _model.RefreshCommand.SubscribeWithLog(phones => 
            {
                _model.Phones = phones?.Select(phone => new RxPhone(phone));
                _model.CountPhones = _model.Phones.Count();
                _model.SelectedPhone = _model.Phones.FirstOrDefault(p => p.Id == 3);
            });
        }
        public IDisposable BindEditCommand()
        {
            IObservable<bool> canEdit = _model.WhenAnyValue(
                x => x.SelectedPhone)
                .IsEmpty()
                .Select(x => !x);
            //.Select(x => true);
            _model.EditCommand = ReactiveCommand.CreateFromObservable(EditCommand, canEdit, RxApp.MainThreadScheduler);
            return _model.EditCommand.SubscribeWithLog(result => {
                _model.Title = result.ToString(); //получаем результат, когда модальное окно закрывается
                _model.RefreshCommand.Execute().Subscribe();
            });
        }
        private IObservable<DialogModelResult<RxPhone>> EditCommand()
        {
            //return _dialogManager.Open(new ConfirmDialogModel
            //{
            //    TitleText = "Изменить объект?",
            //    MessageText = "Вы желаете изменить объект?",
            //    ConfirmText = "Изменить",
            //    CancelText = "Отмена"
            //})
            //    .SelectMany(confirmed =>
            //    {
            //        if (!confirmed)
            //            return Observable.Return<DialogModelResult<RxPhone>>(new DialogModelResult<RxPhone>.Cancelled());
            //        else
            //            //откроем диалог Редактирования объекта
            //            return _dialogManager.Open(new EditPhoneViewModel(_model.SelectedPhone))
            //                .Select(result =>
            //                {
            //                    var t = confirmed;
            //                    // можно варьировать предварительным результатом, например отслеживать ошибки
            //                    // if (result is EditPhoneViewModelResult.Edited edit)
            //                    //     return edit;
            //                    // else return null;
            //                    return result;
            //                });
            //    });

            return _dialogManager.Open(new EditPhoneViewModel(_model.SelectedPhone))
                .Select(resultEdit =>
                {
                    // можно варьировать предварительным результатом, например отслеживать ошибки
                    // if (result is EditPhoneViewModelResult.Edited edit)
                    //     return edit;
                    // else return null;
                    return resultEdit;
                });
        }

        public IDisposable BindConfirmCommand()
        {
            _model.ConfirmCommand = ReactiveCommand.CreateFromObservable(() => _dialogManager.Open(new ConfirmDialogModel
                {
                    TitleText = "Delete connection?",
                    MessageText = "Do you want to delete selected connection?",
                    ConfirmText = "Delete",
                    CancelText = "Cancel"
                }), null, RxApp.MainThreadScheduler);
            return _model.ConfirmCommand.SubscribeWithLog(res => {
                var t = res;
                _model.Title = res.ToString();
            });
        }

        public IDisposable BindOpenFilesCommand()
        {
            _model.OpenFilesCommand = ReactiveCommand.CreateFromObservable(() => {
                var d = 0;
                return _dialogManager.OpenFiles();
            }, null, RxApp.MainThreadScheduler); 
            return _model.OpenFilesCommand.SubscribeWithLog(files => {
                _model.Title = "selectd files " + files.Length.ToString();
            });
        }

        public IDisposable BindOpenFolderCommand()
        {
            _model.OpenFolderCommand = ReactiveCommand.CreateFromObservable(() => {
                var d = 0;
                return _dialogManager.OpenFolder("c:\\Temp");
            }, null, RxApp.MainThreadScheduler);
            return _model.OpenFolderCommand.SubscribeWithLog(folder => {
                _model.Title = "selectd folder " + folder;
            });
        }

    }
}
