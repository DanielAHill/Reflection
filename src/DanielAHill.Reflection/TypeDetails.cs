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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DanielAHill.Reflection
{
    internal class TypeDetails : ITypeDetails
    {
        public Type Type { get; }
        public bool IsValue => ValueTypes.Contains(Type) || PropertyReaders.Count + PropertyWriters.Count == 0;
        public object DefaultValue { get; }
        public bool IsNumeric => NumericTypes.Contains(Type);
        public bool IsCollection { get; }
        public IReadOnlyList<IPropertyReader> PropertyReaders { get; }
        public IReadOnlyList<IPropertyWriter> PropertyWriters { get; }

        internal TypeDetails(Type type, object defaultValue, IReadOnlyList<IPropertyReader> propertyReaders, IReadOnlyList<IPropertyWriter> propertyWriters)
        {
            Type = type;
            DefaultValue = defaultValue;
            PropertyReaders = propertyReaders;
            PropertyWriters = propertyWriters;
            IsCollection = type != typeof (string) && typeof (IEnumerable).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
        }

        private static readonly HashSet<Type> ValueTypes = new HashSet<Type>
        {
            typeof (string),
            typeof (Guid),
            typeof (Guid?)
        };

        private static readonly HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(byte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(byte?),
            typeof(short?),
            typeof(int?),
            typeof(long?),
            typeof(float?),
            typeof(double?),
            typeof(decimal?),
        };
    }
}