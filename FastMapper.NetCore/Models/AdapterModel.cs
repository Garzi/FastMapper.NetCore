using System;
using System.Linq.Expressions;
using FastMapper.NetCore.Utils;

namespace FastMapper.NetCore.Models
{
    /// <summary>
    ///     Getter : FieldInfo.GetValue
    ///     Setter : FieldInfo.SetValue
    /// </summary>
    public class FieldModel
    {
        public FastInvokeHandler Getter;
        public FastInvokeHandler Setter;
    }

    public class PropertyModel<TSource, TDestination>
    {
        public FastInvokeHandler AdaptInvoker;

        // Use byte, because byte performance better than enum
        public byte
            ConvertType; //Primitive = 1, FlatteningGetMethod = 2, FlatteningDeep = 3, Adapter = 4, CustomResolve = 5;

        public Func<TSource, object> CustomResolver;
        public object DefaultDestinationValue;
        public GenericGetter[] FlatteningInvokers;
        public PropertyCaller<TSource>.GenGetter Getter;
        public PropertyCaller<TDestination>.GenSetter Setter;
    }

    public class AdapterModel<TSource, TDestination>
    {
        public FieldModel[] Fields;
        public PropertyModel<TSource, TDestination>[] Properties;
    }

    public class CollectionAdapterModel
    {
        public FastInvokeHandler AdaptInvoker;
        public bool IsPrimitive;
    }

    public class InvokerModel<TSource>
    {
        public Func<TSource, object> Invoker;
        public string MemberName;
    }

    public class ExpressionModel
    {
        public string DestinationMemberName;
        public Expression SourceExpression;
    }
}