using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace XMRestAPIClient
{
    public static class ExpressionsExtensions
    {
        /// <summary>
        /// Converts a predicate to a string in lamda format.
        /// </summary>
        /// <param name="filterPredicate">The filter predicate.</param>
        /// <returns></returns>
        public static string ToStringLamda(this Func<dynamic, bool> filterPredicate)
        {
            Expression<Func<dynamic, bool>> exp = filterPredicate.FuncToExpression();
            string expBody = exp.ToString()
                                   .Replace("AndAlso", "&&")
                                   .Replace("OrElse", "||");
            return expBody;
        }

        /// <summary>
        /// Functions to expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> FuncToExpression<T>(this Func<T, bool> f)
        {
            return x => f(x);
        }
    }
}
