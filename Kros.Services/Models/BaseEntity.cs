using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kros.Services
{
    public class BaseEntity : ReactiveObject
    {
        [Reactive] public int Id { get; set; }
        public BaseEntity() { }
        public BaseEntity(int id = 0) => Id = id;
    }
}
