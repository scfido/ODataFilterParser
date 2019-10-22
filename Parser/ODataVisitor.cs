﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace ODataFilterParser
{
    internal class ODataVisitor : ODataBaseVisitor<object>
    {
        Dictionary<string, object> memory = new Dictionary<string, object>();

        public override object VisitParenthesis(ODataParser.ParenthesisContext context)
        {
            object obj = "(" + Visit(context.expression()).ToString() + ")";
            return obj;
        }

        //public override object VisitMultiplyDivide(ODataParser.MultiplyDivideContext context)
        //{
        //    double left = Convert.ToDouble(Visit(context.expression(0)));
        //    double right = Convert.ToDouble(Visit(context.expression(1)));

        //    object obj = new object(); 
        //    if (context.operate.Type == ODataParser.MUL) {
        //        obj = left * right;
        //    } else if (context.operate.Type == ODataParser.DIV) {
        //        if (right == 0) {
        //            throw new Exception("Cannot divide by zero.");
        //        }
        //        obj = left / right;
        //    }

        //    return obj;
        //}

        protected override object AggregateResult(object aggregate, object nextResult)
        {
            if (aggregate == null && nextResult != null)
                return nextResult;
            else if (aggregate != null && nextResult == null)
                return aggregate;
            else if (aggregate != null && nextResult != null)
                return aggregate + " " + nextResult;

            return base.AggregateResult(aggregate, nextResult);
        }

        public override object VisitLogic([NotNull] ODataParser.LogicContext context)
        {
            var left = Visit(context.expression(0)).ToString();
            var right = Visit(context.expression(1)).ToString();

            if (context.logic.Type == ODataParser.K_AND)
            {
                return $"{left} AND {right}";
            }
            else if (context.logic.Type == ODataParser.K_OR)
            {
                return $"{left} OR {right}";
            }

            return string.Empty;
        }


        public override object VisitCompare(ODataParser.CompareContext context)
        {
            var left = context.column.Text;
            var right = context.value.Text;
            var compare = context.compare.Type;

            switch (compare)
            {
                case ODataParser.Equal:
                    return $"{left} = {right}";

                case ODataParser.NotEqual:
                    return $"{left} != {right}";

                case ODataParser.GreaterThan:
                    return $"{left} > {right}";

                case ODataParser.GreaterThanOrEqual:
                    return $"{left} >= {right}";

                case ODataParser.LessThan:
                    return $"{left} < {right}";

                case ODataParser.LessThanOrEqual:
                    return $"{left} <= {right}";

                default:
                    return string.Empty;
            }
        }

        public override object VisitStartsWith([NotNull] ODataParser.StartsWithContext context)
        {
            var column = context.column.Text;
            var val = context.value.Text;

            return $"{column} like {val}%";
        }

        public override object VisitEndsWith([NotNull] ODataParser.EndsWithContext context)
        {
            var column = context.column.Text;
            var val = context.value.Text;

            return $"{column} like %{val}";

        }

        public override object VisitContains([NotNull] ODataParser.ContainsContext context)
        {
            var column = context.column.Text;
            var val = context.value.Text;

            return $"{column} like %{val}%";
        }

        //public override object VisitText([NotNull] ODataParser.TextContext context)
        //{
        //    object obj = context.GetText();
        //    return obj;
        //}
    }
}