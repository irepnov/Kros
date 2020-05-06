using DynamicData;
using Kros.Presentations.Dialogs;
using Kros.Presentations;
using Kros.Services;
using PropertyChanged;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq.Expressions;

namespace Kros.Presentations
{
   // [PropertyChanged.AddINotifyPropertyChangedInterface]
    public sealed class MainViewModel : BaseViewModel
    {
        public IEnumerable<RxPhone> Phones { get; set; }
        public RxPhone SelectedPhone { get; set; }
        [Reactive] public string Title { get; set; } = "start";
        [Reactive] public RxPhone Filter { get; set; } = new RxPhone(new Phone(id: 1, null, null, 0));
        public int CountPhones { get; set; } = 0;
        public List<Expression<Func<Phone, bool>>> RefreshFilter { get; set; } = new List<Expression<Func<Phone, bool>>>();
        public ReactiveCommand<Unit, IEnumerable<Phone>> RefreshCommand { get; set; }
        public ReactiveCommand<Unit, DialogModelResult<RxPhone>> EditCommand { get; set; }
        public ReactiveCommand<Unit, Unit> TestCommand { get; set; }
        public ReactiveCommand<Unit, bool> ConfirmCommand { get; set; }
        public ReactiveCommand<Unit, string[]> OpenFilesCommand { get; set; }
        public ReactiveCommand<Unit, string> OpenFolderCommand { get; set; }
        public MainViewModel()
        {
            MainViewController _controller = new MainViewController(this);


           /* this.WhenAnyValue(m => m.Title)
                .Subscribe(title =>
                {
                    if (string.IsNullOrWhiteSpace(title) || title == "…")
                        this.Title = "Radish";
                    else
                        this.Title = "Radish - " + title;

                    this.RefreshFilter.Clear();
                    if (int.TryParse(title, out int id))
                        this.RefreshFilter.Add(a => a.Id > id);
                    else
                        this.RefreshFilter.Add(a => a.Id > 2);
                    this.RefreshFilter.Add(b => b.DisplayId == 1);
                });*/


            this.WhenActivated(disposables =>
            {
                _controller.BindTitle().DisposeWith(disposables);
                _controller.BindRefreshFilter().DisposeWith(disposables);
                _controller.BindRefreshCommand().DisposeWith(disposables);
                _controller.BindTestCommand().DisposeWith(disposables);
                _controller.BindEditCommand().DisposeWith(disposables);
                _controller.BindConfirmCommand().DisposeWith(disposables);
                _controller.BindOpenFilesCommand().DisposeWith(disposables);
                _controller.BindOpenFolderCommand().DisposeWith(disposables);
                RefreshCommand.Execute().Subscribe(phones => {
                    var m = this;
                    //то что сделать после обработки по умолчанию
                }).DisposeWith(disposables);
            });
        }


    }
}


/* Без контроллера
 
//    public sealed class MainViewModel : ReactiveObject, IMainViewModel, IActivatableViewModel
//    {
//        private readonly ObservableAsPropertyHelper<IEnumerable<RxPhone>> _phones;        
//        private readonly ObservableAsPropertyHelper<string> _errorMessage;
//        private readonly ObservableAsPropertyHelper<bool> _hasErrors;
//        private readonly ObservableAsPropertyHelper<bool> _isBusy;
//        private readonly ObservableAsPropertyHelper<int> _countPhones;
//        private readonly ReactiveCommand<Unit, IEnumerable<Phone>> _refresh;
//        private readonly ReactiveCommand<Unit, EditPhoneViewModelResult> _edit;
//        public MainViewModel(IPhoneRepositoryAsync _repos, IScheduler _sheduler)
//        {
//            _sheduler = _sheduler ?? RxApp.MainThreadScheduler;
//            //this.WhenAnyValue(x => x.SearchTerm).InvokeCommand(this, x => x.Search);  если есть данные в троке, то выполняю ReactiveCommand "Search", можно отложить на 1 сек Throttle
//            _refresh = ReactiveCommand.CreateFromTask(() => _repos.GetAllAsEnumerable());
//            var canEdit = this
//                .WhenAnyValue(x => x.SelectedPhone)
//                .IsEmpty()
//                .Select(x => !x);
//                //.Select(x => true);
//            _edit = ReactiveCommand.CreateFromObservable<Unit, EditPhoneViewModelResult>(
//                _ => {
//                    return Locator.Current.GetService<IDialogManager>().Open(new EditPhoneViewModel(SelectedPhone));
//                }, canEdit, _sheduler);
//            _phones = _refresh
//                 .Select(phones => phones.Select(phone => new RxPhone(phone.Id, phone.Company, phone.Title, phone.Price)))
//                 .ObserveOn(_sheduler)
//                 .ToProperty(this, x => x.Phones);
//            _errorMessage = _refresh
//                .ThrownExceptions
//                .Select(exception => exception.Message)
//                .Log(this, $"Error refresh data")
//                .ObserveOn(_sheduler)
//                .ToProperty(this, x => x.ErrorMessage);
//            _hasErrors = _refresh
//                .ThrownExceptions
//                .Select(exception => true)
//                .Merge(_refresh.Select(unit => false))
//                .ObserveOn(_sheduler)
//                .ToProperty(this, x => x.HasErrors);
//_isBusy = _refresh
//    .IsExecuting
//    .ObserveOn(_sheduler)
//                .ToProperty(this, x => x.IsBusy);
//            //_refresh.Subscribe(list =>
//            //{
//            //    if (list?.Count() > 0)
//            //    {
//            //        var item = list.FirstOrDefault(p => p.Id == 3);
//            //        SelectedPhone = new RxPhone(item.Id, item.Company, item.Title, item.Price);
//            //    }        
//            //});
//            this.WhenAnyValue(vm => vm.Phones)
//                .Where(x => x != null)
//                .Select(list => list.FirstOrDefault(p => p.Id == 3))
//                .Select(item => new RxPhone(item.Id, item.Company, item.Title, item.Price))
//                .ObserveOn(_sheduler)
//                .Subscribe(phone => SelectedPhone = phone);
//            this.WhenAnyValue(vm => vm.Phones)
//                .Where(x => x != null)
//                .Select(list => list.Count())
//                .ObserveOn(_sheduler)
//                .ToProperty(source: this, property: vm => vm.CountPhones, result: out _countPhones, initialValue: 0);
////_countPhones = this.WhenAnyValue(vm => vm.Phones).Where(x => x != null).Select(list => list.Count()).ToProperty(this, vm => vm.CountPhones);
//MainViewController _controller = new MainViewController(this);
//Activator = new ViewModelActivator();
//            this.WhenActivated(disposables =>
//            {
//                _controller.BindTitle().DisposeWith(disposables);
//                _refresh.Execute().Subscribe().DisposeWith(disposables);
//            });
//        }

//        public ViewModelActivator Activator { get; }
//public IEnumerable<RxPhone> Phones => _phones.Value;

//[Reactive] public RxPhone SelectedPhone { get; set; }
////public RxPhone SelectedPhone
//// {
////     get => _selectedPhone;
////     set => this.RaiseAndSetIfChanged(ref _selectedPhone, value);
//// }

//[Reactive] public string Title { get; set; } = "123";
//public string ErrorMessage => _errorMessage.Value;
//public bool HasErrors => _hasErrors.Value;
//public bool IsBusy => _isBusy.Value;
//public int CountPhones => _countPhones.Value;
//public ICommand Refresh => _refresh;
//public ICommand Edit => _edit;
//    }


     */


//https://itnan.ru/post.php?c=1&p=418007
//https://github.com/jamilgeor/FormsTutor
//https://jamilgeor.com/refreshing-a-listview-with-reactivecommand/
//https://github.com/reactiveui/ReactiveUI#throttling-network-requests-and-automatic-search-execution-behaviour
//https://dynamic-data.org/category/reactiveui/
//https://github.com/reactiveui/DynamicData
//https://oz-code.com/blog/mvvm-gone-reactive-creating-wpf-twitter-client-reactiveui/
//this.WhenAnyValue(x => x.SearchTerm).InvokeCommand(this, x => x.Search);  если есть данные в троке, то выполняю ReactiveCommand "Search", можно отложить на 1 сек Throttle
/*
 * 
 * public ExplorerPadController(
            ExplorerPadModel model)
            : this(
                Locator.Current.GetService<IDialogManager>())
        {
            _model = model;
        }

        public ExplorerPadController(
            IDialogManager dialogManager)
        {
            _dialogManager = dialogManager;
        }

        public IDisposable BindOpenSettingsCommand()
        {
            _model.OpenSettingsCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(
                _ => OpenSettings(), null, RxApp.MainThreadScheduler);

            return _model.OpenSettingsCommand
                .SubscribeWithLog();
        }

        private IObservable<Unit> OpenSettings()
        {
            return _dialogManager.Open(new SettingsDialogModel());
        }

 * https://html.developreference.com/article/20219213/How+to+bind+a+ReactiveCommand+to+a+control+in+a+Xamarin.Forms+ListView%3F
 
        ReactiveList<string> _articles;
		readonly IArticleService _articleService;
		public ReactiveCommand<Unit, IEnumerable<Article>> LoadArticles { get; private set; }

        public ReactiveList<string> Articles
		{
			get => _articles;
			set => this.RaiseAndSetIfChanged(ref _articles, value);
		}

		public ArticlesViewModel()
		{
            _articleService = new ArticleService();
            Articles = new ReactiveList<string>();
			LoadArticles = ReactiveCommand.CreateFromTask(LoadArticlesImpl);
			LoadArticles.ObserveOn(RxApp.MainThreadScheduler).Subscribe(MapArticlesImpl);
			LoadArticles.Execute().Subscribe();
		}

		async Task<IEnumerable<Article>> LoadArticlesImpl()
		{
            return await _articleService.Get();
		}

		void MapArticlesImpl(IEnumerable<Article> articles)
		{
            using (Articles.SuppressChangeNotifications())
            {
                Articles.Clear();
                articles.ToObservable().Subscribe(x => Articles.Add(x.Title));
            }
		}

     */
