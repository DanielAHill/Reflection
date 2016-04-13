# DanielAHill.Reflection
Lightweight reflection helper library with emphasis on speed by reducing reflection calls through caching and inline compilation.

## Build Status
| Master | Beta | Alpha |
|--------|------|-------|
| ![Master Build Status](https://danielahill.visualstudio.com/DefaultCollection/_apis/public/build/definitions/01357b42-fd89-449b-86ba-c3ce8ee41bbb/4/badge) | ![Beta Build Status](https://danielahill.visualstudio.com/DefaultCollection/_apis/public/build/definitions/01357b42-fd89-449b-86ba-c3ce8ee41bbb/3/badge) | ![Alpha Build Status](https://danielahill.visualstudio.com/DefaultCollection/_apis/public/build/definitions/01357b42-fd89-449b-86ba-c3ce8ee41bbb/5/badge)|

## Feature Overview
 - Cached Information on Object Properties
 - Fast (Compiled) Property Readers/Writers
 - Supports both .NET 4.5+ and DNX/ASP.NET Core 1.0
 
## Installation
Install via NuGet (hosted on [NuGet.org](https://www.nuget.org/)):
```
nuget install DanielAHill.Reflection
```

## Usage
###Get Cached Information on a Type

Returns an `ITypeDetails` with the following properties:
- `System.Type` Type - *The type associated with `ITypeDetails`*
- `bool` IsValue - *True when the type is a value type, a string, or has no readible/writable properties, false otherwise*
- `bool` IsNumeric - *True when the type is associated with a number*
- `bool` IsCollection - *True when the type is associated with multiple of the same items, such as an array, list, or ICollection, false otherwise*
- `object` DefaultValue - *Gets the default value of the type, null, in most cases*
- `IReadOnlyList<IPropertyReader>` PropertyReaders - *A list of `PropertyReaders` that can quickly read property values from an instance, as well as, describe the property name and type*
- `IReadOnlyList<IPropertyWriter>` PropertyWriters - *A list of `PropertyWriters` that can quickly write property values to an instance, describe the property name and type*

**From a Type Object:**
```
typeof(MyClass).GetTypeDetails();
```

**From an Object Instance:**
```
new MyClass().GetTypeDetails();
```

###Reading a Property Value
```
    var obj = new MyClass();
    
    // Get Property Reader
    var myPropertyReader = obj.GetTypeDetails().PropertyReaders().First(r => r.Name.Equals("MyPropertyName"));
    
    // Use Property Reader to Retrieve Value
    var propertyValue = myPropertyReader.Read(obj);
```

###Writing a Property Value
```
    var obj = new MyClass();
    
    // Get Property Reader
    var myPropertyWriter = obj.GetTypeDetails().PropertyWriters().First(r => r.Name.Equals("MyPropertyName"));
    
    // Use Property Reader to Retrieve Value
    myPropertyWriter.Write(obj, "MyValue");
    
    // obj.MyPropertyName will have string value of "MyValue"
```

## License
Copyright (c) 2016 Daniel Alan Hill. All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

[http://www.apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.