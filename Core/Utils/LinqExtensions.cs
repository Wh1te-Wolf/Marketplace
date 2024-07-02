using System.Linq.Expressions;
using System.Reflection;

namespace Core.Utils;

public static class LinqExtensions
{
    private const string _orderByFunctionName = "OrderBy";
    private const string _orderByDescendingFunctionName = "OrderByDescending";

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering)
    {
        MethodCallExpression resultExp = GetOrderMethodExpression(source, _orderByFunctionName, ordering);
        return source.Provider.CreateQuery<T>(resultExp);
    }

    public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string ordering)
    {
        MethodCallExpression resultExp = GetOrderMethodExpression(source, _orderByDescendingFunctionName, ordering);
        return source.Provider.CreateQuery<T>(resultExp);
    }

    private static MethodCallExpression GetOrderMethodExpression<T>(IQueryable<T> source, string orderName, string ordering)
    {
        Type type = typeof(T);
        PropertyInfo property = type.GetProperty(ordering);
        ParameterExpression parameter = Expression.Parameter(type, "p");
        MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
        LambdaExpression orderByExp = Expression.Lambda(propertyAccess, parameter);
        MethodCallExpression resultExp = Expression.Call(typeof(Queryable), orderName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));

        return resultExp;
    }
}
