﻿namespace AvroConvertTests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using AvroConvert.Attributes;

    [Equals]
    [DataContract(Name = "User", Namespace = "user")]
    public class User
    {
        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "favorite_number")]
        [NullableSchema]
        public int? favorite_number { get; set; }

        [DataMember(Name = "favorite_color")]
        public string favorite_color { get; set; }
    }

    [Equals]
    public class SomeTestClass
    {
        public NestedTestClass objectProperty { get; set; }

        public int simpleProperty { get; set; }
    }

    [Equals]
    public class NestedTestClass
    {
        public string justSomeProperty { get; set; }

        public long andLongProperty { get; set; }
    }

    [Equals]
    public class SmallerNestedTestClass
    {
        public string justSomeProperty { get; set; }
    }

    [Equals]
    public class ClassWithConstructorPopulatingProperty
    {
        public List<NestedTestClass> nestedList { get; set; }
        public List<ClassWithSimpleList> anotherList { get; set; }
        public string stringProperty { get; set; }

        public ClassWithConstructorPopulatingProperty()
        {
            nestedList = new List<NestedTestClass>();
            anotherList = new List<ClassWithSimpleList>();
        }

    }
    [Equals]
    public class ClassWithSimpleList
    {
        public List<int> someList { get; set; }

        public ClassWithSimpleList()
        {
            someList = new List<int>();
        }
    }

    [Equals]
    public class ClassWithArray
    {
        public int[] theArray { get; set; }
    }

    [Equals]
    public class ClassWithGuid
    {
        public Guid theGuid { get; set; }
    }

    [Equals]
    public class VeryComplexClass
    {
        public List<ClassWithArray> ClassesWithArray { get; set; }
        public ClassWithGuid[] ClassesWithGuid { get; set; }
        public ClassWithConstructorPopulatingProperty anotherClass { get; set; }
        public User simpleClass { get; set; }
        public int simpleObject { get; set; }
        public List<bool> bools { get; set; }
        public double doubleProperty { get; set; }
        public float floatProperty { get; set; }
    }
}
