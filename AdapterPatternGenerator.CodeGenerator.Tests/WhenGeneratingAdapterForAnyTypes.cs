using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.Example;
using NUnit.Framework;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public class WhenGeneratingAdapterForAnyTypes:BaseGeneratorTests
    {
        private const string DirectoryName = "DirectoryName";
        private const string BaseNameSpace = "BaseNameSpace";

        public WhenGeneratingAdapterForAnyTypes()
        {
            var generator = Ioc.Resolve<IGenerator>();
            generator.GenerateCode(new List<Type> { typeof(ExampleClass)}, DirectoryName, BaseNameSpace);
        }

        [Test]
        public void BaseInstanceAdapterConstructorParameterUsesTypeParam()
        {
            var type = AllCodeTypeDeclarations.First(x => x.Name == "BaseInstanceAdapter");
            var typeParam = type.TypeParameters[0];
            var constructor = type.Members.OfType<CodeConstructor>().Single();
            var constructorParam = constructor.Parameters.OfType<CodeParameterDeclarationExpression>().Single();
            Assert.AreEqual(typeParam.Name, constructorParam.Type.BaseType);
        }

        [Test]
        public void BaseInstanceAdapterHasAFieldThatMatchesTypeParam()
        {
            var type = AllCodeTypeDeclarations.First(x => x.Name == "BaseInstanceAdapter");
            var typeParam = type.TypeParameters[0];
            var field = type.Members.OfType<CodeMemberField>().Single();
            Assert.AreEqual(Constants.InternalAdapterFieldName, field.Name);
            Assert.AreEqual(typeParam.Name, field.Type.BaseType);
        }
        [Test]
        public void BaseInstanceAdapterInstantiatesFieldInConstructor()
        {
            var type = AllCodeTypeDeclarations.First(x => x.Name == "BaseInstanceAdapter");
            var constructor = type.Members.OfType<CodeConstructor>().Single();
            var constructorParam = constructor.Parameters.OfType<CodeParameterDeclarationExpression>().Single();
            var statement = constructor.Statements[0] as CodeAssignStatement;
            Assert.IsNotNull(statement);
            var left = statement.Left as CodeFieldReferenceExpression;
            Assert.IsNotNull(left);
            Assert.IsTrue(left.TargetObject is CodeThisReferenceExpression);
            Assert.AreEqual(Constants.InternalAdapterFieldName, left.FieldName);
            var right = statement.Right as CodeVariableReferenceExpression;
            Assert.IsNotNull(right);
            Assert.AreEqual(constructorParam.Name, right.VariableName);
        }
        [Test]
        public void BaseInstanceAdapterConstructorIsProtected()
        {
            var type = AllCodeTypeDeclarations.First(x => x.Name == "BaseInstanceAdapter");
            var constructor = type.Members.OfType<CodeConstructor>().Single();
            Assert.AreEqual(MemberAttributes.Family, constructor.Attributes);
        }

        [Test]
        public void BaseInstanceAdapterIsAbstractAndPublic()
        {
            var type = AllCodeTypeDeclarations.First(x => x.Name == "BaseInstanceAdapter");
            Assert.AreEqual(TypeAttributes.Public | TypeAttributes.Abstract, type.TypeAttributes);
        }
        [Test]
        public void BaseInstanceAdapterInstanceFieldIsInternal()
        {
            var type = AllCodeTypeDeclarations.First(x => x.Name == "BaseInstanceAdapter");
            var field = type.Members.OfType<CodeMemberField>().First(x => x.Name == Constants.InternalAdapterFieldName);
            Assert.AreEqual(MemberAttributes.FamilyAndAssembly, field.Attributes);
        }


    }
}
