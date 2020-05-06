using Kros.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Kros.Testing
{
    public class PhoneRepositoryAsyncTest
    {
        private readonly IPhoneRepositoryAsync repos;

        public PhoneRepositoryAsyncTest()
        {
            ApplicationContext context = new ApplicationContext();
            repos = new PhoneRepositoryAsync(context);
        }

        [Fact]
        public void InitRepos()
        {
            Assert.NotNull(repos);
        }

        [Fact]
        public async void Test()
        {

            /* ParameterExpression param = Expression.Parameter(typeof(Customer), "t");
             MemberExpression member = Expression.Property(param, columnName);
             ConstantExpression constant = Expression.Constant(searchText);
             MethodInfo containsMethod = typeof(string).GetMethod("Contains");
             Expression exp = Expression.Call(member, containsMethod, constant);           
             var deleg = Expression.Lambda<Func<Customer, bool>>(exp, param).Compile();
             db.Customers.Where(t => t.ContactName.Contains(SearchText));*/
            //https://stackoverflow.com/questions/44677281/entity-framework-6-dynamically-build-search-clause-against-db
            //https://stackoverflow.com/questions/20054742/dynamic-query-with-or-conditions-in-entity-framework
            //https://stackoverflow.com/questions/5595338/add-the-where-clause-dynamically-in-entity-framework


            IEnumerable<Phone> allEnumResult = await repos.GetAsEnumerable();
            Assert.NotNull(allEnumResult);

            var filter = new List<Expression<Func<Phone, bool>>>();
            filter.Add(a => a.Id > 2);
            filter.Add(a => a.Id > 2);
            filter.Add(b => b.DisplayId == 1);
            IEnumerable<Phone> allEnumResultInclude = await repos.GetAsEnumerable(
                filter, 
                p => p.Display
                //можно добавить еще всякие дополнительные сущьности
                );
            Assert.NotNull(allEnumResultInclude);

            var allQueryResult = repos.GetAsQueryable();
            Assert.NotNull(allQueryResult);

            var allQuery2Result = repos.GetAsQueryable().Where(p => p.Title == "тел 2");
            var tt = allQuery2Result.ToList();
            Assert.NotNull(tt);

            IEnumerable<Phone> whereEnumResult = await repos.GetWhereAsEnumerable(repos.GetAsQueryable().Where(p => p.Title == "тел 2"));
           // IEnumerable<Phone> whereEnumResult = whereEnum.Result;
            Assert.NotNull(whereEnumResult);

            Phone byIdResult = await repos.GetById(2);
          //  Phone byIdResult = byId.Result;
            Assert.NotNull(byIdResult);

            byIdResult = await repos.GetById(56);
           // byIdResult = byId.Result;
            Assert.Null(byIdResult);

            var byTitle = repos.GetByTitle("тел 2");
            Phone byTitleResult = byTitle.Result;
            Assert.NotNull(byTitleResult);

            int countResult = await repos.CountAll();
            //int countResult = count.Result;
            Assert.True(countResult > 0);

            countResult = await repos.CountWhere(p => p.Title == "тел 2");
            //countResult = count.Result;
            Assert.True(countResult == 1);

            Phone phad = new Phone() { Company = "add", Title = "add" };
            Phone addResult = await repos.Add(phad);
            // int addResult = add.Result;
            Assert.NotNull(addResult);

            Phone phup = new Phone() { Id = 3 }; ;
            phup.Company = "upd";
            Phone updResult = await repos.Update(phup);
            // int updResult = upd.Result;
            Assert.NotNull(updResult);

            Phone phAU = new Phone() { Company = "addUpd1", Title = "addUpd1" };
            Phone AUResult = await repos.AddOrUpdate(phAU);
            //int AUResult = AU.Result;
            Assert.NotNull(AUResult);

            Phone phAU2 = new Phone() { Id = 1 }; ;
            phAU2.Company = "upd";
            Phone AU2Result = await repos.AddOrUpdate(phAU2);
            //int AU2Result = AU2.Result;
            Assert.NotNull(AU2Result);

            /*var execSqlResult = await repos.ExecuteSqlCommand("UPDATE Phones SET title={0} WHERE id={1}", new object[] { "sql", "2" });
            //var execSqlResult = execSql.Result;
            Assert.True(execSqlResult > 0);

            int _id = 3;
            string _title = "sql3";
            var execSqlInResult = await repos.ExecuteSqlCommandInterpolated($"UPDATE Phones SET title={_title} WHERE id={_id}");
            //var execSqlInResult = execSqlIn.Result;
            Assert.True(execSqlInResult > 0);

            var fromSqlResult = await repos.GetFromSqlAsEnumerable("select * from phones where id >= {0}", new object[] { 3 });
            //var fromSqlResult = fromSql.Result;
            Assert.NotNull(fromSqlResult);

            int __id = 4;
            var fromSqlResult2 = await repos.GetFromSqlInterpolatedAsEnumerable($"select * from phones where id >= {__id}");
           // var fromSqlResult2 = fromSql2.Result;
            Assert.NotNull(fromSqlResult2);*/
        }
    }
}
