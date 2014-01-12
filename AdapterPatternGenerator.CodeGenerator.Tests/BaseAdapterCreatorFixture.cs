using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public class BaseAdapterCreatorFixture
    {
        private const string BaseNameSpace = "BaseNameSpace";
        [Test]
        public void UsesCorrectNameSpace()
        {
            var baseAdapterCreate = new BaseAdapterCreator();
            var unit = baseAdapterCreate.CreateBaseClass(BaseNameSpace);
            var nameSpace = unit.Namespaces[0];
            Assert.AreEqual(BaseNameSpace, nameSpace.Name);
        }
        [Test]
        public void UsesCorrectTypeName()
        {
            var baseAdapterCreate = new BaseAdapterCreator();
            var unit = baseAdapterCreate.CreateBaseClass(BaseNameSpace);
            var nameSpace = unit.Namespaces[0];
            var type = nameSpace.Types[0];
            Assert.AreEqual(Constants.BaseInstanceAdapterName,type.Name);
        }

        [Test]
        public void ConstructorParameterUsesTypeParam()
        {
            var baseAdapterCreate = new BaseAdapterCreator();
            var unit = baseAdapterCreate.CreateBaseClass(BaseNameSpace);
            var nameSpace = unit.Namespaces[0];
            var type = nameSpace.Types[0];
            var typeParam = type.TypeParameters[0];
            var constructor = type.Members.OfType<CodeConstructor>().Single();
            var constructorParam = constructor.Parameters.OfType<CodeParameterDeclarationExpression>().Single();
            Assert.AreEqual(typeParam.Name, constructorParam.Type.BaseType);
        }

    }
}
