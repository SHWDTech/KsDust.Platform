using System.Linq;
using System.Linq.Expressions;

namespace Dust.Platform.Web.Process
{
    public static class QueryableExtensionClass
    {
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string sortField, bool @ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, sortField);
            var exp = Expression.Lambda(prop, param);
            var method = @ascending ? "OrderBy" : "OrderByDescending";
            var types = new[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
    }
}