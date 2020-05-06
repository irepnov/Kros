using FluentAssertions;
using Kros.Presentations;
using Kros.Services;
using Microsoft.Reactive.Testing;
using ReactiveUI;
using ReactiveUI.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Xunit;

namespace Kros.Testing
{
    public class MainViewModelTest
    {
        private readonly IPhoneRepositoryAsync repos;
        private readonly MainViewModel model;

        /*
        public MainViewModelTest()
        {
            ApplicationContext context = new ApplicationContext();
            repos = new PhoneRepositoryAsync(context);
            model = new MainViewModel(repos, RxApp.MainThreadScheduler);            
        }

        [Fact]
        public void InitModel()
        {            
            Assert.NotNull(repos);
            Assert.NotNull(model);
        }

        [Fact]
        public void Test()
        {
            model.Phones.Should().BeNull();
            model.SelectedPhone.Should().BeNull();
            model.Activator.Activate();
            model.Phones.Should().NotBeEmpty();
            var y = model.Phones.First();
            model.SelectedPhone.Should().NotBeNull();
            model.CountPhones.Should().Equals(4);
            Assert.True(model.Phones.Count() > 0);
           // Assert.True(model.SelectedPhone.Equals(model.Phones.FirstOrDefault()));
        }*/

        private MainViewModel BuildMainViewModel(IScheduler _1)
        {
            return new MainViewModel();          
        }

        [Fact]
        public void CheckPhones() => new TestScheduler().With(scheduler =>
        {
            var model = BuildMainViewModel(scheduler);
            scheduler.AdvanceBy(2);
            model.Phones.Should().BeNull();
            model.RefreshCommand.Execute();
            scheduler.AdvanceBy(3);
            model.Phones.Should().NotBeEmpty();
        });

        [Fact]
        public void Activated() => new TestScheduler().With(scheduler =>
        {
            var model = BuildMainViewModel(scheduler);
            scheduler.AdvanceBy(2);
            model.Phones.Should().BeNull();
            model.Activator.Activate();
            scheduler.AdvanceBy(5);
            model.Phones.Should().NotBeEmpty();
            scheduler.AdvanceBy(10);
            model.SelectedPhone.Should().NotBeNull();
            model.CountPhones.Should().Equals(4);
        });

    }
}
