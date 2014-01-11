using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.Example;
using FakeItEasy;
using NUnit.Framework;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public class GeneratorFixture
    {
        [Test]
        public void GenerateCodeCreatesTypesAndWritesThemToDirectory()
        {
            var typeCreator = A.Fake<ITypeDeclarationCreator>();
            var typeWriter = A.Fake<ITypeWriter>();
            var generator = new Generator(typeCreator, typeWriter);
            var types = new List<Type>();
            const string directoryName = "directoryName";
            var declaredTypes = new List<CodeTypeDeclaration>();
            A.CallTo(() => typeCreator.CreateTypes(types)).Returns(declaredTypes);

            generator.GenerateCode(types, directoryName);

            A.CallTo(() => typeCreator.CreateTypes(types)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => typeWriter.WriteTypes(declaredTypes, directoryName)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
