using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public class TypeMapFixture
    {
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void AddingTwoDefaultsThrowsAnArgumentException()
        {
            var typemap = new TypeMap();
            typemap.Add(typeof(TypeMapFixture), new CodeTypeDeclaration(), true);
            typemap.Add(typeof(TypeMapFixture), new CodeTypeDeclaration(), true);
        }
        [Test]
        public void GetAllReturnsAllAddedTypes()
        {
            var typemap = new TypeMap();
            var array = new[] { new CodeTypeDeclaration(), new CodeTypeDeclaration() };
            var type = typeof (TypeMapFixture);
            typemap.Add(type, array[0], true);
            typemap.Add(type, array[1], false);
            var actual = typemap.GetAll(type);
            CollectionAssert.AreEquivalent(array, actual);
        }
        [Test]
        public void GetDefaultReturnsDefault()
        {
            var typemap = new TypeMap();
            var array = new[] { new CodeTypeDeclaration(), new CodeTypeDeclaration() };
            var type = typeof(TypeMapFixture);
            typemap.Add(type, array[0], false);
            typemap.Add(type, array[1], true);
            var actual = typemap.GetDefault(type);
            Assert.AreEqual(array[1], actual);
        }

        [Test]
        public void GetDefaultReturnsNullWhenEmpty()
        {
            var typemap = new TypeMap();
            var type = typeof(TypeMapFixture);
            var actual = typemap.GetDefault(type);
            Assert.IsNull( actual);
        }

        [Test]
        public void GetAllReturnsEmptyListWhenEmpty()
        {
            var typemap = new TypeMap();
            var type = typeof(TypeMapFixture);
            var actual = typemap.GetAll(type);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0,actual.Count);
        }
    }
}
