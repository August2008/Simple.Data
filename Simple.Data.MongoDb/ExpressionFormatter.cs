using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Collections;
using MongoDB.Bson.DefaultSerializer;
using System.Text.RegularExpressions;

namespace Simple.Data.MongoDb
{
    class ExpressionFormatter : IExpressionFormatter
    {
        public ExpressionFormatter()
        { }

        public QueryComplete Format(SimpleExpression expression)
        {
            switch (expression.Type)
            {
                case SimpleExpressionType.And:
                    return LogicalExpression(expression, (l, r) => Query.And(l,r));
                case SimpleExpressionType.Equal:
                    return EqualExpression(expression);
                case SimpleExpressionType.GreaterThan:
                    return BinaryExpression(expression, Query.GT);
                case SimpleExpressionType.GreaterThanOrEqual:
                    return BinaryExpression(expression, Query.GTE);
                case SimpleExpressionType.LessThan:
                    return BinaryExpression(expression, Query.LT);
                case SimpleExpressionType.LessThanOrEqual:
                    return BinaryExpression(expression, Query.LTE);
                case SimpleExpressionType.Like:
                    return LikeExpression(expression);
                case SimpleExpressionType.NotEqual:
                    return NotEqualExpression(expression);
                case SimpleExpressionType.NotLike:
                    return NotLikeExpression(expression);
                case SimpleExpressionType.Or:
                    return LogicalExpression(expression, (l, r) => Query.Or(l,r));
            }

            throw new NotSupportedException();
        }

        private QueryComplete BinaryExpression(SimpleExpression expression, Func<string, BsonValue, QueryComplete> builder)
        {
            var fieldName = (string)FormatObject(expression.LeftOperand);
            var value = BsonValue.Create(FormatObject(expression.RightOperand));
            return builder(fieldName, value);
        }

        private QueryComplete EqualExpression(SimpleExpression expression)
        {
            var fieldName = (string)FormatObject(expression.LeftOperand);
            var range = expression.RightOperand as IRange;
            if (range != null)
            {
                return Query.And(
                    Query.GTE(fieldName, BsonValue.Create(range.Start)),
                    Query.LTE(fieldName, BsonValue.Create(range.End)));
            }

            var list = expression.RightOperand as IEnumerable;
            if (list != null & expression.RightOperand.GetType() != typeof(string))
                return Query.In(fieldName, new BsonArray(list.OfType<object>()));

            return Query.EQ(fieldName, BsonValue.Create(FormatObject(expression.RightOperand)));
        }

        private QueryComplete LikeExpression(SimpleExpression expression)
        {
            if (!(expression.RightOperand is string)) throw new InvalidOperationException("Cannot use Like on non-string type.");
            return Query.Matches((string)FormatObject(expression.LeftOperand), new BsonRegularExpression((string)FormatObject(expression.RightOperand)));
        }

        private QueryComplete LogicalExpression(SimpleExpression expression, Func<QueryComplete, QueryComplete, QueryComplete> builder)
        {
            return builder(
                Format((SimpleExpression)expression.LeftOperand),
                Format((SimpleExpression)expression.RightOperand));
        }

        private QueryComplete NotEqualExpression(SimpleExpression expression)
        {
            var fieldName = (string)FormatObject(expression.LeftOperand);
            var range = expression.RightOperand as IRange;
            if (range != null)
            {
                return Query.Or(
                    Query.LTE(fieldName, BsonValue.Create(range.Start)),
                    Query.GTE(fieldName, BsonValue.Create(range.End)));
            }

            var list = expression.RightOperand as IEnumerable;
            if (list != null & expression.RightOperand.GetType() != typeof(string))
                return Query.NotIn(fieldName, new BsonArray(list.OfType<object>()));

            return Query.NE(fieldName, BsonValue.Create(FormatObject(expression.RightOperand)));
        }

        private QueryComplete NotLikeExpression(SimpleExpression expression)
        {
            throw new NotSupportedException("Not Like is not a supported operation.");
        }

        private object FormatObject(object operand)
        {
            var reference = operand as DynamicReference;
            if (!ReferenceEquals(reference, null))
            {
                var name = reference.GetName();
                return name == "Id" ? "_id" : name;
            }
            return operand;
        }
    }
}