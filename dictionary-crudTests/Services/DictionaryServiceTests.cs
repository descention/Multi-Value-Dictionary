using Microsoft.VisualStudio.TestTools.UnitTesting;
using dictionary_crud.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace dictionary_crud.Services.Tests
{
    [TestClass()]
    public class DictionaryServiceTests
    {
        DictionaryService Service { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Service = new DictionaryService();
            Service.Add("foo", "bar");
            Service.Add("foo", "baz");
        }

        [TestMethod()]
        public void KeysTest()
        {
            Service.Add("baz", "bang");

            var expected = new string[] { "foo", "baz" };

            CollectionAssert.AreEquivalent(expected, Service.Keys().ToList());
        }

        [TestMethod()]
        public void MembersTest()
        {
            var values = new string[] { "bar", "baz" };
            CollectionAssert.AreEquivalent(values, Service.Members("foo").ToArray());
        }


        [TestMethod()]
        public void AddTest()
        {
            Assert.IsTrue(Service.Keys().Contains("foo"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddTestException()
        {
            // adding an existing record should trigger an exception
            Service.Add("foo", "bar");
        }

        [TestMethod()]
        public void RemoveTest()
        {
            Service.Remove("foo", "bar");
        }

        [TestMethod()]
        
        public void RemoveTestException()
        {
            Service.Remove("foo", "bar");
            // second removal should trigger an exception
            Assert.ThrowsException<InvalidOperationException>(() => Service.Remove("foo", "bar"));

            // should still have one foo record
            var expected = new string[] { "foo" };
            CollectionAssert.AreEquivalent(expected, Service.Keys().ToArray());

            Service.Remove("foo", "baz");

            Assert.AreEqual(0, Service.Keys().Count());

            Assert.ThrowsException<InvalidOperationException>(() => Service.Remove("boom", "pow"));
        }

        [TestMethod]
        public void RemoveAllTest()
        {
            Service.RemoveAll("foo");
            Assert.AreEqual(0, Service.Keys().Count());

            Assert.ThrowsException<InvalidOperationException>(() => Service.RemoveAll("foo"));
        }

        [TestMethod()]
        public void ClearTest()
        {
            Service.Add("bang", "zip");
            Assert.AreEqual(2, Service.Keys().Count());
            Service.Clear();
            Assert.AreEqual(0, Service.Keys().Count());
            Service.Clear();
            Assert.AreEqual(0, Service.Keys().Count());
        }

        [TestMethod()]
        public void KeyExistsTest()
        {
            Service.Clear();

            Assert.IsFalse(Service.KeyExists("foo"));

            Service.Add("foo", "bar");

            Assert.IsTrue(Service.KeyExists("foo"));
        }

        [TestMethod()]
        public void MemberExistsTest()
        {
            Service.Clear();
            Assert.IsFalse(Service.MemberExists("foo", "bar"));
            Service.Add("foo", "bar");
            Assert.IsTrue(Service.MemberExists("foo", "bar"));
            Assert.IsFalse(Service.MemberExists("foo", "baz"));
        }

        [TestMethod()]
        public void AllMembersTest()
        {
            Service.Clear();
            Assert.IsFalse(Service.AllMembers().Any());
            Service.Add("foo", "bar");
            Service.Add("foo", "baz");

            var expectedFirst = new string[] { "bar", "baz" };
            CollectionAssert.AreEquivalent(expectedFirst, Service.AllMembers().ToArray());
            Assert.AreEqual(2, Service.AllMembers().Count());

            Service.Add("bang", "bar");
            Service.Add("bang", "baz");

            var expectedSecond = new string[] { "bar", "baz", "bar", "baz" };
            CollectionAssert.AreEquivalent(expectedSecond, Service.AllMembers().ToArray());
            Assert.AreEqual(4, Service.AllMembers().Count());
        }

        [TestMethod()]
        public void ItemsTest()
        {
            Service.Clear();
            IList<(string key, string member)> expected = new List<(string key, string member)>();
            expected.Add(("foo", "bar"));
            expected.Add(("foo", "baz"));

            Service.Add("foo", "bar");
            Service.Add("foo", "baz");

            CollectionAssert.AreEquivalent(expected.ToArray(), Service.Items().ToArray());

            expected.Add(("bang", "bar"));
            expected.Add(("bang", "baz"));

            Service.Add("bang", "bar");
            Service.Add("bang", "baz");

            CollectionAssert.AreEquivalent(expected.ToArray(), Service.Items().ToArray());

        }
    }
}
