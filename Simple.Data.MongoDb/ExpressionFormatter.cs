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
                case SimpleExpressionType.Equal:
                    return EqualExpression(expression);
                case SimpleExpressionType.Like:
                    return LikeExpression(expression);
            }

            throw new NotSupportedException();
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