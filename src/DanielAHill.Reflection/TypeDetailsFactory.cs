#region Copyright
// Copyright (c) 2016 Daniel Alan Hill. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DanielAHill.Reflection
{
    internal static class TypeDetailsFactory
    {
        private static readonly ConcurrentDictionary<Type, ITypeDetails> Cache = new ConcurrentDictionary<Type, ITypeDetails>();

        internal static ITypeDetails Get(Type type)
        {
#if DEBUG
            if (type == null) throw new ArgumentNullException(nameof(type));
#endif
            return Cache.GetOrAdd(type, CreateNew);
        }

        private static ITypeDetails CreateNew(Type type)
        {
            var readibles = new List<IPropertyReader>();
            var writables = new List<IPropertyWriter>();

            foreach (var pi in type.GetTypeInfo().GetProperties())
            {
                var propertyInfo = CreatePropertyAccessor(type, pi);

                if (propertyInfo == null)
                {
                    continue;
                }

                var propertyReader = propertyInfo as IPropertyReader;
                if (propertyReader != null)
                {
                    readibles.Add(propertyReader);
                }

                var propertyWriter = propertyInfo as IPropertyWriter;
                if (propertyWriter != null)
                {
                    writables.Add(propertyWriter);
                }
            }

            var defaultValue = type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;
            return new TypeDetails(type, defaultValue, readibles.ToArray(), writables.ToArray());
        }


        private static IPropertyDetails CreatePropertyAccessor(Type type, PropertyInfo pi)
        {
#if DEBUG
            if (pi == null) throw new ArgumentNullException(nameof(pi));
#endif

            Func<object, object> reader = null;
            if (pi.CanRead)
            {
                reader = CreateReader(type, pi);
            }

            Action<object, object> writer = null;
            if (pi.CanWrite)
            {
                writer = CreateWriter(type, pi);
            }

            if (reader != null && writer != null)
            {
                return new PropertyAccessor(pi.Name, pi.PropertyType, writer, reader);
            }
            else if (reader != null)
            {
                return new PropertyReader(pi.Name, pi.PropertyType, reader);
            }
            else if (writer != null)
            {
                return new PropertyWriter(pi.Name, pi.PropertyType, writer);
            }

            return null;
        }

        private static Func<object, object> CreateReader(Type type, PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetIndexParameters().Length > 0)
            {
                // No Indexors
                return null;
            }

            var itemParameter = Expression.Parameter(typeof(object), "item");

            try
            {
                var expression = Expression.TypeAs(
                                            Expression.Call(
                                                Expression.Convert(itemParameter, type), propertyInfo.GetGetMethod())
                                            , typeof(object));

                return Expression.Lambda<Func<object, object>>(expression, itemParameter).Compile();
            }
            catch (Exception)
            {   // Fallback to reflection
#if DEBUG
                throw;
#else
                return t => propertyInfo.GetMethod.Invoke(t, new object[0]);
#endif
            }
        }

        private static Action<object, object> CreateWriter(Type type, PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetIndexParameters().Length > 0)
            {
                // No Indexors
                return null;
            }

            var itemParameter = Expression.Parameter(typeof(object), "item");
            var valueParamter = Expression.Parameter(typeof(object), "value");

            try
            {
                var expression = Expression.Assign(
                                        Expression.Property(
                                            Expression.Convert(itemParameter, type), 
                                            propertyInfo.Name),
                                        Expression.Convert(valueParamter, propertyInfo.PropertyType));

                return Expression.Lambda<Action<object, object>>(expression, itemParameter, valueParamter).Compile();
            }
            catch (Exception)
            {   // Fallback to reflection
#if DEBUG
                throw;
#else
                return (t, v) => propertyInfo.SetMethod.Invoke(t, new object[] { v });
#endif
            }
        }
    }
}