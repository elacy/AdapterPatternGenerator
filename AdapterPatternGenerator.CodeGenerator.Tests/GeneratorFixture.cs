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
            var codeCompileUnitCreator = A.Fake<ICodeCompileUnitCreator>();
            var typeWriter = A.Fake<ITypeWriter>();
            var generator = new Generator(codeCompileUnitCreator, typeWriter);
            var types = new List<Type>();
            const string directoryName = "directoryName";
            var codeCompileUnit = new CodeCompileUnit();
            A.CallTo(() => codeCompileUnitCreator.CreateCodeCompileUnit(types)).Returns(codeCompileUnit);

            generator.GenerateCode(types, directoryName);

            A.CallTo(() => codeCompileUnitCreator.CreateCodeCompileUnit(types)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => typeWriter.WriteTypes(codeCompileUnit, directoryName)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
