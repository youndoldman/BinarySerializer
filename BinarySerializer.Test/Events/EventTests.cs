﻿using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySerialization.Test.Events
{
    [TestClass]
    public class EventTests
    {
        [TestMethod]
        public void TestSerializeEvents()
        {
            var serializer = new BinarySerializer();

            var events = new List<MemberSerializingEventArgs>();

            serializer.MemberSerializing += (sender, args) => events.Add(args);
            serializer.MemberSerialized += (sender, args) => events.Add(args);

            var stream = new MemoryStream();
            serializer.Serialize(stream, new EventTestClass {InnerClass = new EventTestInnerClass()});

            Assert.AreEqual(typeof(MemberSerializingEventArgs), events[0].GetType());
            Assert.AreEqual(typeof(MemberSerializingEventArgs), events[1].GetType());
            Assert.AreEqual(typeof(MemberSerializedEventArgs), events[2].GetType());
            Assert.AreEqual(typeof(MemberSerializedEventArgs), events[3].GetType());

            Assert.AreEqual("InnerClass", events[0].MemberName);
            Assert.AreEqual("Value", events[1].MemberName);
            Assert.AreEqual("Value", events[2].MemberName);
            Assert.AreEqual("InnerClass", events[3].MemberName);
        }
    }
}