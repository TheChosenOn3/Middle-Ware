using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ConnectionHandler
{
    public enum FilterCondition {
        equals =1,notEquals
    }

   public class DBFilterClass<T>
    {
       
        public Expression<Func<T, object>> Field { get; set; }
        public Func<T, object> FieldValues { get; set; }
        public FilterCondition condition { get; set; }

    }
}
