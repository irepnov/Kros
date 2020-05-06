using Kros.Services;
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;

namespace Kros.Presentations 
{
#pragma warning disable CS0659 // Тип переопределяет Object.Equals(object o), но не переопределяет Object.GetHashCode()
    public class RxPhone : Phone
#pragma warning restore CS0659 // Тип переопределяет Object.Equals(object o), но не переопределяет Object.GetHashCode()
    {
        private readonly ObservableAsPropertyHelper<string> _TitleWithCompany;
        public string TitleWithCompany => _TitleWithCompany.Value;
        /*public RxPhone(): base()
        {
            _TitleWithCompany = this.WhenAnyValue(m => m.Company, m => m.Title)
                                        .Select(t => $"{t.Item1} {t.Item2}")
                                        .ToProperty(this, m => m.TitleWithCompany);
        }*/
        public RxPhone(Phone phone)
            : base(phone.Id, phone.Title, phone.Company, phone.Price, phone.Display)
        {
            _TitleWithCompany = this.WhenAnyValue(m => m.Company, m => m.Title)
                                         .Select(t => $"{t.Item1} {t.Item2}")
                                         .ToProperty(this, m => m.TitleWithCompany);
        }
        /*public RxPhone(int id = 0, string company = null, string title = null, int price = 0) : base(id, company, title, price)
        {
            _TitleWithCompany = this.WhenAnyValue(m => m.Company, m => m.Title)
                                         .Select(t => $"{t.Item1} {t.Item2}")
                                         .ToProperty(this, m => m.TitleWithCompany);
        }*/
        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                RxPhone p = (RxPhone)obj;
                return (this.Id == p.Id);
            }
        }
    }
}
