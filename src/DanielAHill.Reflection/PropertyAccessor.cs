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

namespace DanielAHill.Reflection
{
    internal class PropertyAccessor : IPropertyAccessor
    {
        private readonly Action<object, object> _writeDelegate;
        private readonly Func<object, object> _readDelegate;
        public string Name { get; }
        public Type PropertyType { get; }

        internal PropertyAccessor(string name, Type type, Action<object, object> writeDelegate, Func<object, object> readDelegate)
        {
            _writeDelegate = writeDelegate;
            _readDelegate = readDelegate;
            Name = name;
            PropertyType = type;
        }

        public void Write(object item, object value)
        {
            _writeDelegate(item, value);
        }

        public object Read(object item)
        {
            return _readDelegate(item);
        }
    }
}