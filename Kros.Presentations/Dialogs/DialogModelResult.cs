using System;
using System.Collections.Generic;
using System.Text;

namespace Kros.Presentations.Dialogs
{
    public abstract class DialogModelResult<TEntity> where TEntity : class
    {
        public class Succesfull : DialogModelResult<TEntity>
        {
            public TEntity Entity { get; }
            public Succesfull(TEntity entity) => Entity = entity;
        }
        public class Failed : DialogModelResult<TEntity>
        {
            public Exception Exception { get; }
            public Failed(Exception exception) => Exception = exception;
        }
        public class Cancelled : DialogModelResult<TEntity>
        {
        }
    }
}
