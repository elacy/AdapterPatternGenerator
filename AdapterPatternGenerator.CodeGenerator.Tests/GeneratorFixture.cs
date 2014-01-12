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
            var typeWriter = A.Fake<ICodeWriter>();
            var generator = new Generator(codeCompileUnitCreator, typeWriter);
            var types = new List<Type>();
            const string directoryName = "directoryName";
            const string baseNameSpace = "baseNameSpace";
            var codeCompileUnits = new[] {new CodeCompileUnit()};
            A.CallTo(() => codeCompileUnitCreator.CreateCodeCompileUnit(types, baseNameSpace)).Returns(codeCompileUnits);

            generator.GenerateCode(types, directoryName, baseNameSpace);

            A.CallTo(() => codeCompileUnitCreator.CreateCodeCompileUnit(types,baseNameSpace)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => typeWriter.WriteCompileUnits(codeCompileUnits, directoryName)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
