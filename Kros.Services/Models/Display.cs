using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kros.Services
{
    public class Display: BaseEntity
    {
        [Reactive] public decimal Size { get; set; }
        [Reactive] public string Matrix { get; set; }
        public Display()
            : base() { }
        public Display(int id, decimal size, string matrix)
            : base(id)
        {
            Size = size;
            Matrix = matrix;
        }
    }
}
