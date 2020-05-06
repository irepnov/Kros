using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kros.Services
{
    public class PhoneRepositoryAsync : BaseRepositoryAsync<Phone>, IPhoneRepositoryAsync
    {
        public PhoneRepositoryAsync(ApplicationContext context) : base(context) { }
        
        //add specific metod
        public Task<Phone> GetByTitle(string title)
        {
            return FirstOrDefault(w => w.Title == title);
        }
        public new async Task<Phone> AddOrUpdate(Phone entity)
        {
            //специфичная реализация метода для Телефонов
            var t = 0;
            if (entity == null) throw new NullReferenceException("entity not initialized");
            Phone ph = GetById(entity.Id).Result;
            if (ph != null)
            {
                Context.Entry(ph).State = EntityState.Detached;
                Context.Attach(entity).State = EntityState.Modified;
            }
            else
            {
                await Context.Set<Phone>().AddAsync(entity);
            }
            if (await Context.SaveChangesAsync() > 0)
                return entity;
            else
                return null;
        }

    }
}
