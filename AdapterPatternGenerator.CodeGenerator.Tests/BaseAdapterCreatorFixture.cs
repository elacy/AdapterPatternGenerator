using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        [Test]
        public void HasAFieldThatMatchesTypeParam()
        {
            var baseAdapterCreate = new BaseAdapterCreator();
            var unit = baseAdapterCreate.CreateBaseClass(BaseNameSpace);
            var nameSpace = unit.Namespaces[0];
            var type = nameSpace.Types[0];
            var typeParam = type.TypeParameters[0];
            var field = type.Members.OfType<CodeMemberField>().Single();
            Assert.AreEqual(Constants.InternalAdapterFieldName,field.Name);
            Assert.AreEqual(typeParam.Name,field.Type.BaseType);
        }
        [Test]
        public void InstantiatesFieldInConstructor()
        {
            var baseAdapterCreate = new BaseAdapterCreator();
            var unit = baseAdapterCreate.CreateBaseClass(BaseNameSpace);
            var nameSpace = unit.Namespaces[0];
            var type = nameSpace.Types[0];
            var constructor = type.Members.OfType<CodeConstructor>().Single();
            var constructorParam = constructor.Parameters.OfType<CodeParameterDeclarationExpression>().Single();
            var statement = constructor.Statements[0] as CodeAssignStatement;
            Assert.IsNotNull(statement);
            var left = statement.Left as CodeFieldReferenceExpression;
            Assert.IsNotNull(left);
            Assert.IsTrue(left.TargetObject is CodeThisReferenceExpression);
            Assert.AreEqual(Constants.InternalAdapterFieldName,left.FieldName);
            var right = statement.Right as CodeVariableReferenceExpression;
            Assert.IsNotNull(right);
            Assert.AreEqual(constructorParam.Name, right.VariableName);
        }
        [Test]
        public void ConstructorIsProtected()
        {
            var baseAdapterCreate = new BaseAdapterCreator();
            var unit = baseAdapterCreate.CreateBaseClass(BaseNameSpace);
            var nameSpace = unit.Namespaces[0];
            var type = nameSpace.Types[0];
            var constructor = type.Members.OfType<CodeConstructor>().Single();
            Assert.AreEqual(MemberAttributes.Family, constructor.Attributes);
        }

        [Test]
        public void BaseInstanceAdapterIsAbstractAndPublic()
        {
            var baseAdapterCreate = new BaseAdapterCreator();
            var unit = baseAdapterCreate.CreateBaseClass(BaseNameSpace);
            var nameSpace = unit.Namespaces[0];
            var type = nameSpace.Types[0];
            Assert.AreEqual(TypeAttributes.Public | TypeAttributes.Abstract, type.TypeAttributes);
        }
        [Test]
        public void InstanceFieldIsInternal()
        {
            var baseAdapterCreate = new BaseAdapterCreator();
            var unit = baseAdapterCreate.CreateBaseClass(BaseNameSpace);
            var nameSpace = unit.Namespaces[0];
            var type = nameSpace.Types[0];
            var field = type.Members.OfType<CodeMemberField>().First(x => x.Name == Constants.InternalAdapterFieldName);
            Assert.AreEqual(MemberAttributes.FamilyAndAssembly, field.Attributes);
        }


    }
}
