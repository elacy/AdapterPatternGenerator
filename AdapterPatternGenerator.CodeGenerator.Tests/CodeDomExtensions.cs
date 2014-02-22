using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;

namespace AdapterPatternGenerator.CodeGenerator.Tests
{
    public static class CodeDomAssert
    {
        
        public static void AreEqual( IDictionary expected, IDictionary actual)
        {
            CollectionAssert.AreEquivalent(expected,actual);
        }


        public static void PerformAction<T>(IEnumerable<T> expected, IEnumerable<T> actual, Action<T, T> action)
        {
            PerformAction<T,object>(expected,actual,action);
        }
        public static void PerformAction<T,TY>(IEnumerable<T> expected, IEnumerable<T> actual, Action<T, T> action, Func<T,TY> joinFunc = null )
        {
            Assert.AreEqual(expected.Count(), actual.Count());

            var items = expected.Select((value, i) => new { i, value }).Join(actual.Select((value, i) => new { i, value }), x => x.i, x => x.i, (x, y) => new { Expected = x.value, Actual = y.value });
            if (joinFunc != null)
            {
                items = expected.Join(actual, joinFunc, joinFunc, (x, y) => new { Expected = x, Actual = y });
            }
            foreach (var item in items)
            {
                action(item.Expected, item.Actual);
            }
        }
        public static void AreEqual(CodeTypeReference expected, CodeTypeReference actual)
        {
            if (expected == actual)
            {
                return;
            }
            Assert.AreEqual(expected.BaseType,actual.BaseType);
            PerformAction(expected.TypeArguments.AsEnumerable(), actual.TypeArguments.AsEnumerable(), AreEqual);
            AreEqual(expected.ArrayElementType, actual.ArrayElementType);
            AreEqual(expected.UserData,actual.UserData);
            Assert.AreEqual(expected.Options,actual.Options);
        }
        
        

        public static void AreEqual(CodeComment expected, CodeComment actual)
        {
            Assert.AreEqual(expected.DocComment,actual.DocComment);
            Assert.AreEqual(expected.Text,actual.Text);
            AreEqual(expected.UserData,actual.UserData);
        }
        public static void AreEqual(CodeDirective expected, CodeDirective actual)
        {
            AreEqual(expected.UserData,actual.UserData);
        }
        public static void AreEqual(CodeLinePragma expected, CodeLinePragma actual)
        {
            if (expected == actual)
                return;
            Assert.AreEqual(expected.FileName, actual.FileName);
            Assert.AreEqual(expected.LineNumber, actual.LineNumber);
        }
        public static void AreEqual(CodeCommentStatement expected, CodeCommentStatement actual)
        {
            AreEqual(expected.Comment,actual.Comment);
            PerformAction(expected.EndDirectives.AsEnumerable(), actual.EndDirectives.AsEnumerable(), AreEqual);
            AreEqual(expected.LinePragma,actual.LinePragma);
            AreEqual(expected.UserData, actual.UserData);
            PerformAction(expected.StartDirectives.AsEnumerable(), actual.StartDirectives.AsEnumerable(), AreEqual);
        }
        public static void AreEqual(CodeAttributeDeclaration expected, CodeAttributeDeclaration actual)
        {
            Assert.AreEqual(expected.Name,actual.Name);
            PerformAction(expected.Arguments.AsEnumerable(), actual.Arguments.AsEnumerable(), AreEqual);
        }

        public static void AreEqual(CodeAttributeArgument expected, CodeAttributeArgument actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            AreEqual(expected.Value, actual.Value);
        }

        public static void AreEqual(CodeExpression expected, CodeExpression actual)
        {
            if (expected == null && actual == null)
                return;
            AreEqual(expected.UserData,actual.UserData);
            Assert.AreEqual(expected.GetType(),actual.GetType());
            PerformActionIfTypeMatch<CodeParameterDeclarationExpression>(expected, actual, AreEqualSpec);
            PerformActionIfTypeMatch<CodeFieldReferenceExpression>(expected, actual, AreEqualSpec);
            PerformActionIfTypeMatch<CodeVariableReferenceExpression>(expected, actual, AreEqualSpec);

            //This reference have no extra values
            //PerformActionIfTypeMatch<CodeThisReferenceExpression>(first, second, AreEqualSpec);
        }
        public static void AreEqualSpec(CodeFieldReferenceExpression expected, CodeFieldReferenceExpression actual)
        {
            Assert.AreEqual(expected.FieldName, actual.FieldName);
            AreEqual(expected.TargetObject, actual.TargetObject);
        }
        public static void AreEqualSpec(CodeVariableReferenceExpression expected, CodeVariableReferenceExpression actual)
        {
            Assert.AreEqual(expected.VariableName,actual.VariableName);
        }
        
        public static void AreEqual()
        {
            
        }

        private static void AreEqualSpec(CodeParameterDeclarationExpression expected, CodeParameterDeclarationExpression actual)
        {
            Assert.AreEqual(expected.Direction,expected.Direction);
            AreEqual(expected.Type,actual.Type);
            Assert.AreEqual(expected.Name,actual.Name);
        }
        public static void AreEqual(CodeStatement expected, CodeStatement actual)
        {
            AreEqual(expected.UserData, actual.UserData);
            PerformAction(expected.EndDirectives.AsEnumerable(), actual.EndDirectives.AsEnumerable(), AreEqual);
            PerformAction(expected.StartDirectives.AsEnumerable(), actual.StartDirectives.AsEnumerable(), AreEqual);
            AreEqual(expected.LinePragma,actual.LinePragma);
            Assert.AreEqual(expected.GetType(),actual.GetType());
            PerformActionIfTypeMatch < CodeAssignStatement>(expected,actual,AreEqualSpec);
        }

        private static void AreEqualSpec(CodeAssignStatement expected, CodeAssignStatement actual)
        {
            AreEqual(expected.Left,actual.Left);
            AreEqual(expected.Right,actual.Right);
        }

        public static void AreEqual(CodeTypeMember expected, CodeTypeMember actual)
        {
            Assert.AreEqual(expected.Attributes,actual.Attributes);
            PerformAction(expected.Comments.AsEnumerable(), actual.Comments.AsEnumerable(), AreEqual);
            PerformAction(expected.CustomAttributes.AsEnumerable(),actual.CustomAttributes.AsEnumerable(),AreEqual);
            Assert.AreEqual(expected.GetType(),actual.GetType());
            AreEqual(expected.LinePragma,actual.LinePragma);
            Assert.AreEqual(expected.Name, actual.Name);
            AreEqual(expected.UserData, actual.UserData);
            PerformActionIfTypeMatch<CodeMemberProperty>(expected, actual, AreEqualSpec);
            PerformActionIfTypeMatch<CodeMemberField>(expected, actual, AreEqualSpec);
            PerformActionIfTypeMatch<CodeMemberMethod>(expected, actual, AreEqualSpec);
            PerformActionIfTypeMatch<CodeMemberEvent>(expected, actual, AreEqualSpec);
            PerformActionIfTypeMatch<CodeTypeDeclaration>(expected, actual, AreEqualSpec);
        }

        public static void AreEqual(CodeTypeParameter expected, CodeTypeParameter actual)
        {
            PerformAction(expected.Constraints.AsEnumerable(),actual.Constraints.AsEnumerable(),AreEqual);
            PerformAction(expected.CustomAttributes.AsEnumerable(), actual.CustomAttributes.AsEnumerable(), AreEqual);
            Assert.AreEqual(expected.HasConstructorConstraint,actual.HasConstructorConstraint);
            Assert.AreEqual(expected.Name,actual.Name);
            AreEqual(expected.UserData,actual.UserData);
        }

        private static void AreEqualSpec(CodeTypeDeclaration expected, CodeTypeDeclaration actual)
        {
            PerformAction(expected.BaseTypes.AsEnumerable(),actual.BaseTypes.AsEnumerable(),AreEqual);
            Assert.AreEqual(expected.IsClass, actual.IsClass);
            Assert.AreEqual(expected.IsEnum, actual.IsEnum);
            Assert.AreEqual(expected.IsInterface, actual.IsInterface);
            Assert.AreEqual(expected.IsPartial, actual.IsPartial);
            Assert.AreEqual(expected.IsStruct,actual.IsStruct);
            PerformAction(expected.Members.AsEnumerable(),actual.Members.AsEnumerable(),AreEqual,x=>x.Name);
            Assert.AreEqual(expected.TypeAttributes,actual.TypeAttributes);
            PerformAction(expected.TypeParameters.AsEnumerable(),actual.TypeParameters.AsEnumerable(),AreEqual);
        }

        private static void PerformActionIfTypeMatch<T>(object expected, object actual,Action<T,T> action) where T : class
        {
            var item = expected as T;
            if (item != null)
            {
                action(item, (T)actual);
            }
        }

        private static void AreEqualSpec(CodeMemberProperty expected, CodeMemberProperty actual)
        {
            Assert.AreEqual(expected.HasGet,actual.HasGet);
            Assert.AreEqual(expected.HasSet,actual.HasSet);
            PerformAction(expected.ImplementationTypes.AsEnumerable(),actual.ImplementationTypes.AsEnumerable(),AreEqual);
            AreEqual(expected.Type,actual.Type);
            PerformAction(expected.GetStatements.AsEnumerable(), actual.GetStatements.AsEnumerable(), AreEqual);
            PerformAction(expected.Parameters.AsEnumerable(),actual.Parameters.AsEnumerable(),AreEqual);
            PerformAction(expected.SetStatements.AsEnumerable(),actual.SetStatements.AsEnumerable(),AreEqual);
            AreEqual(expected.PrivateImplementationType,actual.PrivateImplementationType);
            
        }
        private static void AreEqualSpec( CodeMemberField expected, CodeMemberField actual)
        {
            AreEqual(expected.InitExpression,actual.InitExpression);
            AreEqual(expected.Type,actual.Type);
        }
        private static void AreEqualSpec(CodeMemberEvent expected, CodeMemberEvent actual)
        {
            AreEqual(expected.PrivateImplementationType, actual.PrivateImplementationType);
            PerformAction(expected.ImplementationTypes.AsEnumerable(), actual.ImplementationTypes.AsEnumerable(), AreEqual);
            AreEqual(expected.Type,actual.Type);
        }
        private static void AreEqualSpec(CodeMemberMethod expected, CodeMemberMethod actual)
        {
            AreEqual(expected.PrivateImplementationType, actual.PrivateImplementationType);
            PerformAction(expected.ImplementationTypes.AsEnumerable(), actual.ImplementationTypes.AsEnumerable(), AreEqual);
            AreEqual(expected.ReturnType,actual.ReturnType);
            PerformAction(expected.Parameters.AsEnumerable(), actual.Parameters.AsEnumerable(), AreEqual,x=>x.Name);
            PerformAction(expected.ReturnTypeCustomAttributes.AsEnumerable(),  actual.ReturnTypeCustomAttributes.AsEnumerable(), AreEqual);
            PerformAction(expected.TypeParameters.AsEnumerable(),actual.TypeParameters.AsEnumerable(),AreEqual);
            PerformAction(expected.Statements.AsEnumerable(), actual.Statements.AsEnumerable(), AreEqual);
            PerformActionIfTypeMatch<CodeConstructor>(expected, actual, AreEqualSpec);
        }
        private static void AreEqualSpec(CodeConstructor expected, CodeConstructor actual)
        {
            PerformAction(expected.BaseConstructorArgs.AsEnumerable(),actual.BaseConstructorArgs.AsEnumerable(),AreEqual);
            PerformAction(expected.ChainedConstructorArgs.AsEnumerable(),actual.ChainedConstructorArgs.AsEnumerable(),AreEqual);
        }
    }
}
