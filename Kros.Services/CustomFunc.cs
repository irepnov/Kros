using DynamicData.Binding;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using ILogger = Serilog.ILogger;

namespace Kros.Services
{
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that evaluates to true.
        /// </summary>
        public static Expression<Func<T, bool>> True<T>() 
            => param => true;
        /// <summary>
        /// Creates a predicate that evaluates to false.
        /// </summary>
        public static Expression<Func<T, bool>> False<T>() 
            => param => false;
        /// <summary>
        /// Creates a predicate expression from the specified lambda expression.
        /// </summary>
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) 
            => predicate;
        /// <summary>
        /// Combines the first predicate with the second using the logical "and".
        /// </summary>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second) 
            => first.Compose(second, Expression.AndAlso);
        /// <summary>
        /// Combines the first predicate with the second using the logical "or".
        /// </summary>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second) 
            => first.Compose(second, Expression.OrElse);
        /// <summary>
        /// Negates the predicate.
        /// </summary>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }
        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);
            // replace parameters in the second lambda expression with the parameters in the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            // create a merged lambda expression with parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        class ParameterRebinder : ExpressionVisitor
        {
            readonly Dictionary<ParameterExpression, ParameterExpression> map;
            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map) => this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp) => new ParameterRebinder(map).Visit(exp);
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;
                if (map.TryGetValue(p, out replacement))
                    p = replacement;              
                return base.VisitParameter(p);
            }
        }
    }
    /* public struct ExpItem
     {
         public ExpItem(MemberExpression member, string method, ConstantExpression value)
         {
             Member = member; //Expression.Property(param, columnName);
             Method = typeof(string).GetMethod(method);
             Value = value; //Expression.Constant(searchText);
         }
         public MemberExpression Member { get; }
         public ConstantExpression Value { get; }
         public MethodInfo Method { get; }
     }*/

    public static class CustomFunc
    {
        public static T ElementAt<T>(IEnumerable<T> items, int index)
        {
            using (IEnumerator<T> iter = items.GetEnumerator())
            {
                for (int i = 0; i <= index; i++, iter.MoveNext()) ;
                return iter.Current;
            }
        }
       /* public static Expression<Func<T, bool>> GenExpression<T>(params ExpItem[] ExpressionItems)
        {
            Expression exp = Expression.Call(member, containsMethod, constant);

            foreach (ExpItem item in ExpressionItems)
            {
                Expression exp = Expression.Call(item.Member, item.Method, item.Value);
            }


        }*/
    }

    public static class ObservableCollectionExtensions
    {
        public static IObservable<T> ObserveAdditions<T>(this IObservableCollection<T> collection)
        {
            return Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    h => collection.CollectionChanged += h,
                    h => collection.CollectionChanged -= h)
                .Where(args => args.EventArgs.Action == NotifyCollectionChangedAction.Add)
                .SelectMany(args => args.EventArgs.NewItems.Cast<T>());
        }

        public static IObservable<T> ObserveRemovals<T>(this IObservableCollection<T> collection)
        {
            return Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    h => collection.CollectionChanged += h,
                    h => collection.CollectionChanged -= h)
                .Where(args => args.EventArgs.Action == NotifyCollectionChangedAction.Remove)
                .SelectMany(args => args.EventArgs.OldItems.Cast<T>());
        }
    }

    public static class ObservableExtensions
    {
        /// <summary>
        /// Like SelectMany but ordered
        /// </summary>
        public static IObservable<TResult> SelectSeq<T, TResult>(this IObservable<T> observable, Func<T, IObservable<TResult>> selector)
        {
            return observable.Select(selector).Concat();
        }

        /// <summary>
        /// Send errors to stderr
        /// </summary>
        public static IDisposable SubscribeWithLog<T>(this IObservable<T> observable, Action<T> onNext, Action onCompleted)
        {
            var logger = Locator.Current.GetService<ILogger>();

            return observable.Subscribe(
                onNext,
                e => logger.Error(e, "Unhandled exception occured on observable"),
                onCompleted);
        }

        /// <summary>
        /// Send errors to stderr
        /// </summary>
        public static IDisposable SubscribeWithLog<T>(this IObservable<T> observable, Action onCompleted)
        {
            var logger = Locator.Current.GetService<ILogger>();

            return observable.Subscribe(
                _ => { },
                e => logger.Error(e, "Unhandled exception occured on observable"),
                onCompleted);
        }

        /// <summary>
        /// Send errors to stderr
        /// </summary>
        public static IDisposable SubscribeWithLog<T>(this IObservable<T> observable, Action<T> onNext)
        {
            var logger = Locator.Current.GetService<ILogger>();

            return observable.Subscribe(
                onNext,
                e => logger.Error(e, "Unhandled exception occured on observable"));
        }

        /// <summary>
        /// Send errors to stderr
        /// </summary>
        public static IDisposable SubscribeWithLog<T>(this IObservable<T> observable)
        {
            var logger = Locator.Current.GetService<ILogger>();

            return observable.Subscribe(
                _ => { },
                e => logger.Error(e, "Unhandled exception occured on observable"));
        }
    }

}
