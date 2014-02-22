using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdapterPatternGenerator.Example;
using NUnit.Framework;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public class WhenGeneratingCopyForEnum : BaseGeneratorTests
    {
        private const string BaseNameSpace = "BaseNameSpace";
        private const string DirectoryName = "DirectoryName";

        public WhenGeneratingCopyForEnum()
        {
            var generator = Ioc.Resolve<IGenerator>();
            generator.GenerateCode(new List<Type>{typeof(ExampleEnum)} , DirectoryName, BaseNameSpace);
        }

        private static CodeTypeDeclaration Enum()
        {
            var ctd = new CodeTypeDeclaration("ExampleEnumCopy")
            {
                IsEnum = true,
                IsPartial = true,
                CustomAttributes = { new CodeAttributeDeclaration(Constants.CodeGenerationAttribute)},
                Members = { 
                    new CodeMemberField("ExampleEnumCopy","Test"),
                    new CodeMemberField("ExampleEnumCopy","Ted")
                }
            };
            return ctd;
        }

        [Test]
        public void CorrectlyGeneratesEnum()
        {
            var ctd = Enum();
            Verify(ctd);
        }
    }
}
