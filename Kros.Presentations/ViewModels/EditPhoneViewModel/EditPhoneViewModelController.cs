using Kros.Presentations.Dialogs;
using Kros.Services;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;

namespace Kros.Presentations
{
    public class EditPhoneViewModelController
    {
        private readonly EditPhoneViewModel _model;
        private readonly IPhoneRepositoryAsync _repos;
        private readonly IScheduler _scheduler;
        private readonly IDialogManager _dialogManager;
        public EditPhoneViewModelController(IPhoneRepositoryAsync repos, IScheduler scheduler, IDialogManager dialogManager)
        {
            _repos = repos;
            _scheduler = scheduler;
            _dialogManager = dialogManager;
        }

        public EditPhoneViewModelController(EditPhoneViewModel model)
            :this(Locator.Current.GetService<IPhoneRepositoryAsync>(),
                  Locator.Current.GetService<IScheduler>(),
                  Locator.Current.GetService<IDialogManager>()
                 )
        {
            _model = model;
        }
        //обработаме нажатие и обернем в результат модального окна
        public IDisposable BindSaveCommand()
        {
            var canSaveEntity = this._model.SelectedPhone.WhenAnyValue(
                x => x.Id, x => x.Price, x => x.Company, x => x.Title,
                (id, price, company, title) => !string.IsNullOrWhiteSpace(company) && !string.IsNullOrWhiteSpace(title) && title.Length >= 1 && price >= 0
                )
                .DistinctUntilChanged();
            _model.SaveCommand = ReactiveCommand.CreateFromObservable(SaveCommand, canSaveEntity, _scheduler);
            return _model.SaveCommand.SubscribeWithLog(resultEdit =>
                {
                    if (resultEdit != null)
                    {
                        switch (resultEdit)
                        {
                            case RxPhone phone:
                                _model.CloseDialog(new DialogModelResult<RxPhone>.Succesfull(phone));
                                break;
                            case Exception ex:
                                _model.CloseDialog(new DialogModelResult<RxPhone>.Failed(ex));
                                break;
                            default:
                                _model.CloseDialog(new DialogModelResult<RxPhone>.Failed(new Exception("ошибка не из Репозитория БД")));
                                break;
                        }
                    }
                    else
                        _model.CloseDialog(new DialogModelResult<RxPhone>.Cancelled());
                });
        }
        //вернут результа из БД или ошибку из БД
        private IObservable<object> SaveCommand()
        {
            return _dialogManager.Open(new ConfirmDialogModel
                {
                    TitleText = "Изменить объект?",
                    MessageText = "Вы желаете изменить объект?",
                    ConfirmText = "Изменить",
                    CancelText = "Отмена"
                })
                .SelectMany(resultConfirmed =>
                {
                    if (!resultConfirmed)
                        return Observable.Return<object>(null);
                    else
                        return _repos.AddOrUpdate(_model.SelectedPhone)
                                    .ToObservable()
                                    .Select<Phone, object>(item => item != null ? new RxPhone(item) : null)
                                    .Catch<object, Exception>(except => Observable.Return(except));
                });
            //return _repos.AddOrUpdate(_model.SelectedPhone).ToObservable()
            //    .Select<Phone, object>(item => item != null ? new RxPhone(item) : null)
            //    .Catch<object, Exception>(except => Observable.Return(except));
        }
        public IDisposable BindCancelCommand()
        {
            _model.CancelCommand = ReactiveCommand.Create(() => { }, null, _scheduler);
            return _model.CancelCommand.SubscribeWithLog(_ =>
                 {
                     _model.CloseDialog(new DialogModelResult<RxPhone>.Cancelled());
                     //_model.Close(Unit.Default);
                 });
        }
        public IDisposable BindTitle()
        {
            return _model.WhenAnyValue(x => x.SelectedPhone)
                .SubscribeWithLog(phone =>
                {
                    if (phone?.Id > 0)
                        _model.Title = "Изменить";
                });
        }

    }
}
