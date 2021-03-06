﻿using System;
using System.Collections.Generic;
using FastMapper.NetCore.Adapters;
using FastMapper.NetCore.Utils;

namespace FastMapper.NetCore
{
    public static class TypeAdapter
    {
        #region Members

        private static readonly Dictionary<string, FastInvokeHandler> _cache =
            new Dictionary<string, FastInvokeHandler>();

        #endregion

        #region Public Static Methods

        public static TDestination Adapt<TDestination>(object source)
        {
            return (TDestination) GetAdapter(source.GetType(), typeof(TDestination))(null, new[] {source});
        }

        public static TDestination Adapt<TSource, TDestination>(TSource source)
        {
            return (TDestination) GetAdapter<TSource, TDestination>()(null, new object[] {source});
        }

        public static TDestination Adapt<TSource, TDestination>(TSource source, TDestination destination)
        {
            return (TDestination) GetAdapter<TSource, TDestination>(true)(null, new object[] {source, destination});
        }

        public static object Adapt(object source, Type sourceType, Type destinationType)
        {
            return GetAdapter(sourceType, destinationType)(null, new[] {source});
        }

        public static object Adapt(object source, object destination, Type sourceType, Type destinationType)
        {
            return GetAdapter(sourceType, destinationType, true)(null, new[] {source, destination});
        }

        #endregion

        #region Private Methods

        private static FastInvokeHandler GetAdapter(Type sourceType, Type destinationType, bool hasDestination = false)
        {
            var key = sourceType.FullName + destinationType.FullName;

            if (_cache.ContainsKey(key))
                return _cache[key];
            lock (_cache)
            {
                if (_cache.ContainsKey(key)) return _cache[key];

                var arguments = hasDestination ? new[] {sourceType, destinationType} : new[] {sourceType};

                FastInvokeHandler invoker = null;

                if (ReflectionUtils.IsPrimitive(sourceType) && ReflectionUtils.IsPrimitive(destinationType))
                    invoker = FastInvoker.GetMethodInvoker(typeof(PrimitiveAdapter<,>)
                        .MakeGenericType(sourceType, destinationType).GetMethod("Adapt", arguments));
                else if (ReflectionUtils.IsCollection(sourceType) && ReflectionUtils.IsCollection(destinationType))
                    invoker = FastInvoker.GetMethodInvoker(typeof(CollectionAdapter<,,>)
                        .MakeGenericType(sourceType, ReflectionUtils.ExtractElementType(destinationType),
                            destinationType).GetMethod("Adapt", arguments));
                else
                    invoker = FastInvoker.GetMethodInvoker(typeof(ClassAdapter<,>)
                        .MakeGenericType(sourceType, destinationType).GetMethod("Adapt", arguments));

                _cache.Add(key, invoker);
                return invoker;
            }
        }

        private static FastInvokeHandler GetAdapter<TSource, TDestination>(bool hasDestination = false)
        {
            return GetAdapter(typeof(TSource), typeof(TDestination), hasDestination);
        }

        #endregion
    }
}