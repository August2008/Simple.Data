﻿using Simple.Data;
using NUnit.Framework;
using System;

namespace Simple.Data.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for DynamicTableOrColumnTest and is intended
    ///to contain all DynamicTableOrColumnTest Unit Tests
    ///</summary>
    [TestFixture()]
    public class DynamicReferenceTest
    {
        [Test]
        public void GetDynamicPropertyReturnsNewDynamicReferenceWithTableAndColumn()
        {
            // Arrange
            dynamic table = new DynamicReference("Table");

            // Act
            DynamicReference column = table.Column;

            // Assert
            Assert.AreEqual("Column", column.GetName());
            Assert.AreEqual("Table", column.GetOwner().GetName());
        }

        [Test]
        public void GetDynamicPropertyDotPropertyReturnsNewDynamicReferenceWithTwoOwners()
        {
            // Arrange
            dynamic table = new DynamicReference("Table1");

            // Act
            DynamicReference column = table.Table2.Column;

            // Assert
            Assert.AreEqual("Column", column.GetName());
            Assert.AreEqual("Table2", column.GetOwner().GetName());
            Assert.AreEqual("Table1", column.GetOwner().GetOwner().GetName());
        }

        /// <summary>
        ///A test for GetAllObjectNames
        ///</summary>
        [Test()]
        public void GetAllObjectNamesTest()
        {
            // Arrange
            dynamic table = new DynamicTable("One", null);
            var column = table.Two.Column;

            // Act
            string[] names = column.GetAllObjectNames();

            // Assert
            Assert.AreEqual("One", names[0]);
            Assert.AreEqual("Two", names[1]);
            Assert.AreEqual("Column", names[2]);
        }

        [Test]
        public void FromStringTest()
        {
            // Act
            var actual = DynamicReference.FromString("One.Two.Three");

            // Assert
            Assert.AreEqual("Three", actual.GetName());
            Assert.AreEqual("Two", actual.GetOwner().GetName());
            Assert.AreEqual("One", actual.GetOwner().GetOwner().GetName());
            Assert.IsNull(actual.GetOwner().GetOwner().GetOwner());
        }

        private static void DoAsserts<T>(SimpleExpression expression, DynamicReference column, T rightOperand, SimpleExpressionType expressionType)
        {
            Assert.AreEqual(column, expression.LeftOperand);
            Assert.AreEqual(rightOperand, expression.RightOperand);
            Assert.AreEqual(expressionType, expression.Type);
        }

        [Test]
        public void FunctionCallReturnsSimpleExpressionWithFunctionType()
        {
            dynamic column = new DynamicReference("bar", new DynamicReference("foo", new Database(null)));

            SimpleExpression exp = column.BeAwesome(21, "funny");

            Assert.AreSame(column, exp.LeftOperand);
            Assert.AreEqual(SimpleExpressionType.Function, exp.Type);
            Assert.IsInstanceOf<SimpleFunction>(exp.RightOperand);

            SimpleFunction func = (SimpleFunction)exp.RightOperand;
            Assert.AreEqual("BeAwesome", func.Name);
            Assert.AreEqual(21, func.Args[0]);
            Assert.AreEqual("funny", func.Args[1]);
        }

        [Test]
        public void EqualOperatorReturnsSimpleExpressionWithEqualType()
        {
            // Arrange
            var column = DynamicReference.FromStrings("foo", "bar");

            // Act
            var expression = column == 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.Equal);
        }

        [Test]
        public void EqualOperatorReturnsSimpleExpressionWithEqualTypeWhenUsedAsDynamic()
        {
            // Arrange
            dynamic column = DynamicReference.FromStrings("foo", "bar");

            // Act
            var expression = column == 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.Equal);
        }

        [Test]
        public void NotEqualOperatorReturnsSimpleExpressionWithNotEqualType()
        {
            // Arrange
            var column = DynamicReference.FromStrings("foo", "bar");

            // Act
            var expression = column != 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.NotEqual);
        }

        [Test]
        public void LessThanOperatorReturnsSimpleExpressionWithLessThanType()
        {
            // Arrange
            var column = DynamicReference.FromStrings("foo", "bar");

            // Act
            var expression = column < 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.LessThan);
        }

        [Test]
        public void LessThanOrEqualOperatorReturnsSimpleExpressionWithLessThanOrEqualType()
        {
            // Arrange
            var column = DynamicReference.FromStrings("foo", "bar");

            // Act
            var expression = column <= 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.LessThanOrEqual);
        }

        [Test]
        public void GreaterThanOperatorReturnsSimpleExpressionWithGreaterThanType()
        {
            // Arrange
            var column = DynamicReference.FromStrings("foo", "bar");

            // Act
            var expression = column > 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.GreaterThan);
        }

        [Test]
        public void GreaterThanOrEqualOperatorReturnsSimpleExpressionWithGreaterThanOrEqualType()
        {
            // Arrange
            var column = DynamicReference.FromStrings("foo", "bar");

            // Act
            var expression = column >= 1;

            // Assert
            DoAsserts(expression, column, 1, SimpleExpressionType.GreaterThanOrEqual);
        }
    }
}
