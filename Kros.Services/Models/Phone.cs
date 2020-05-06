using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kros.Services
{
    public class Phone : BaseEntity
    {
        [Reactive] public string Title { get; set; }
        [Reactive] public string Company { get; set; }
        [Reactive] public int Price { get; set; }
        [Reactive] public int? DisplayId { get; set; }
        [Reactive] public Display Display { get; set; }
        public Phone():base() { }
        public Phone(int id = 0, string title = null, string company = null, int price = 0)
            : base(id)
        {
            Title = title;
            Company = company;
            Price = price;
        }
        public Phone(int id = 0, string title = null, string company = null, int price = 0, Display display = null)
            : this(id, title, company, price)
        {
            Display = display;
        }
    }
}
