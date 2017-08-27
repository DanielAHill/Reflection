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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace DanielAHill.Reflection.UnitTests
{
    [TestClass]
    public class TypeDetailsTest
    {
        [TestMethod]
        public void ValueTypes()
        {
            var details = typeof(string).GetTypeDetails();
            Assert.IsTrue(details.IsValue);
            Assert.IsFalse(details.IsNumeric);
            Assert.IsFalse(details.IsCollection);
            Assert.AreEqual(0, details.PropertyWriters.Count);
            Assert.AreEqual(1, details.PropertyReaders.Count);
            Assert.IsNull(details.DefaultValue);

            details = typeof (int).GetTypeDetails();
            Assert.IsTrue(details.IsValue);
            Assert.IsTrue(details.IsNumeric);
            Assert.IsFalse(details.IsCollection);
            Assert.AreEqual(0, details.PropertyWriters.Count);
            Assert.AreEqual(0, details.PropertyReaders.Count);
            Assert.AreEqual(0, details.DefaultValue);
        }

        public void ByteArray()
        {
            // TODO: Figure out hot byte array should act and make it so
            throw new NotImplementedException();
        }

        [TestMethod]
        public void CanSetAllPropertyTypes()
        {
            var item = new ClassContainingAllBasicPropertyTypes();

            var guid = Guid.NewGuid();
            var details = item.GetTypeDetails();

            details.PropertyWriters.First(p => p.Name.Equals("Byte")).Write(item, (byte) 5);
            details.PropertyWriters.First(p => p.Name.Equals("Short")).Write(item, (short)55);
            details.PropertyWriters.First(p => p.Name.Equals("Int")).Write(item, 5433);
            details.PropertyWriters.First(p => p.Name.Equals("Long")).Write(item, (long)52334343);
            details.PropertyWriters.First(p => p.Name.Equals("Float")).Write(item, (float)643.44);
            details.PropertyWriters.First(p => p.Name.Equals("Double")).Write(item, 35.323);
            details.PropertyWriters.First(p => p.Name.Equals("Decimal")).Write(item, (decimal)234.2343);
            details.PropertyWriters.First(p => p.Name.Equals("Guid")).Write(item, guid);

            details.PropertyWriters.First(p => p.Name.Equals("NullableByte")).Write(item, (byte)5);
            details.PropertyWriters.First(p => p.Name.Equals("NullableShort")).Write(item, (short)55);
            details.PropertyWriters.First(p => p.Name.Equals("NullableInt")).Write(item, 5433);
            details.PropertyWriters.First(p => p.Name.Equals("NullableLong")).Write(item, (long)52334343);
            details.PropertyWriters.First(p => p.Name.Equals("NullableFloat")).Write(item, (float)643.44);
            details.PropertyWriters.First(p => p.Name.Equals("NullableDouble")).Write(item, (double)35.323);
            details.PropertyWriters.First(p => p.Name.Equals("NullableDecimal")).Write(item, (decimal)234.2343);
            details.PropertyWriters.First(p => p.Name.Equals("NullableGuid")).Write(item, guid);

            details.PropertyWriters.First(p => p.Name.Equals("Char")).Write(item, 'r');
            details.PropertyWriters.First(p => p.Name.Equals("String")).Write(item, "my string");
            details.PropertyWriters.First(p => p.Name.Equals("Object")).Write(item, new ClassContainingAllBasicPropertyTypes() { String = "My Written Object" });            

            Assert.IsNotNull(item);
            Assert.AreEqual((byte)5, item.Byte);
            Assert.AreEqual((short)55, item.Short);
            Assert.AreEqual(5433, item.Int);
            Assert.AreEqual((long)52334343, item.Long);
            Assert.AreEqual((float)643.44, item.Float);
            Assert.AreEqual(35.323, item.Double);
            Assert.AreEqual((decimal)234.2343, item.Decimal);
            Assert.AreEqual(guid, item.Guid);

            Assert.AreEqual((byte)5, item.NullableByte);
            Assert.AreEqual((short)55, item.NullableShort);
            Assert.AreEqual(5433, item.NullableInt);
            Assert.AreEqual((long)52334343, item.NullableLong);
            Assert.AreEqual((float)643.44, item.NullableFloat);
            Assert.AreEqual(35.323, item.NullableDouble);
            Assert.AreEqual((decimal)234.2343, item.NullableDecimal);
            Assert.AreEqual(guid, item.NullableGuid);

            Assert.AreEqual('r', item.Char);
            Assert.AreEqual("my string", item.String);
            Assert.IsNotNull(item.Object);
            Assert.AreEqual("My Written Object", item.Object.String);
            Assert.IsNull(item.Object.Object);
        }

        [TestMethod]
        public void CanReadAllPropertyTypes()
        {
            var item = new ClassContainingAllBasicPropertyTypes()
            {
                Byte = 4,
                Short = 42,
                Int = 11,
                Long = 11101101011011011,
                Float = (float) 0.23,
                Double = 0.444,
                Decimal = (decimal) 1.22,
                Guid = Guid.NewGuid(),
                NullableByte = null,
                NullableShort = 42,
                NullableInt = 11,
                NullableLong = 11101101011011011,
                NullableFloat = (float) 0.23,
                NullableDouble = 0.444,
                NullableDecimal = (decimal) 1.22,
                NullableGuid = Guid.NewGuid()
            };

            item.Object = item;
            var details = item.GetTypeDetails();

            Assert.AreEqual(item.Byte, details.PropertyReaders.First(r => r.Name.Equals("Byte")).Read(item));
            Assert.AreEqual(item.Short, details.PropertyReaders.First(r => r.Name.Equals("Short")).Read(item));
            Assert.AreEqual(item.Int, details.PropertyReaders.First(r => r.Name.Equals("Int")).Read(item));
            Assert.AreEqual(item.Long, details.PropertyReaders.First(r => r.Name.Equals("Long")).Read(item));
            Assert.AreEqual(item.Float, details.PropertyReaders.First(r => r.Name.Equals("Float")).Read(item));
            Assert.AreEqual(item.Double, details.PropertyReaders.First(r => r.Name.Equals("Double")).Read(item));
            Assert.AreEqual(item.Decimal, details.PropertyReaders.First(r => r.Name.Equals("Decimal")).Read(item));
            Assert.AreEqual(item.Guid, details.PropertyReaders.First(r => r.Name.Equals("Guid")).Read(item));

            Assert.AreEqual(item.NullableByte, details.PropertyReaders.First(r => r.Name.Equals("NullableByte")).Read(item));
            Assert.AreEqual(item.NullableShort, details.PropertyReaders.First(r => r.Name.Equals("NullableShort")).Read(item));
            Assert.AreEqual(item.NullableInt, details.PropertyReaders.First(r => r.Name.Equals("NullableInt")).Read(item));
            Assert.AreEqual(item.NullableLong, details.PropertyReaders.First(r => r.Name.Equals("NullableLong")).Read(item));
            Assert.AreEqual(item.NullableFloat, details.PropertyReaders.First(r => r.Name.Equals("NullableFloat")).Read(item));
            Assert.AreEqual(item.NullableDouble, details.PropertyReaders.First(r => r.Name.Equals("NullableDouble")).Read(item));
            Assert.AreEqual(item.NullableDecimal, details.PropertyReaders.First(r => r.Name.Equals("NullableDecimal")).Read(item));
            Assert.AreEqual(item.NullableGuid, details.PropertyReaders.First(r => r.Name.Equals("NullableGuid")).Read(item));

            Assert.AreEqual(item.Object, details.PropertyReaders.First(r => r.Name.Equals("Object")).Read(item));
        }

        [Fact]
        public void TypeDetailsOfDateTime()
        {
            Assert.NotNull(typeof (DateTime).GetTypeDetails());
        }
    }

    public class ClassContainingAllBasicPropertyTypes
    {
        public byte Byte { get; set; }
        public short Short { get; set; }
        public int Int { get; set; }
        public long Long { get; set; }
        public float Float { get; set; }
        public double Double { get; set; }
        public decimal Decimal { get; set; }
        public Guid Guid { get; set; }

        public byte? NullableByte { get; set; }
        public short? NullableShort { get; set; }
        public int? NullableInt { get; set; }
        public long? NullableLong { get; set; }
        public float? NullableFloat { get; set; }
        public double? NullableDouble { get; set; }
        public decimal? NullableDecimal { get; set; }
        public Guid? NullableGuid { get; set; }

        public char Char { get; set; }
        public string String { get; set; }

        public ClassContainingAllBasicPropertyTypes Object { get; set; }
    }
}