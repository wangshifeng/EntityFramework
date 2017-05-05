// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Expressions.Internal;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class ConditionalOptimizingExpressionVisitor : ExpressionVisitorBase
    {
        private readonly QueryCompilationContext _queryCompilationContext;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public ConditionalOptimizingExpressionVisitor([NotNull] QueryCompilationContext queryCompilationContext)
        {
            _queryCompilationContext = queryCompilationContext;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override Expression VisitConditional(ConditionalExpression conditionalExpression)
        {
            if (conditionalExpression.Test is BinaryExpression binaryExpression)
            {
                // Converts '[q] != null ? [q] : [s]' into '[q] ?? [s]'

                if (binaryExpression.NodeType == ExpressionType.NotEqual
                    && binaryExpression.Left is QuerySourceReferenceExpression querySourceReferenceExpression1
                    && binaryExpression.Right.IsNullConstantExpression()
                    && ReferenceEquals(conditionalExpression.IfTrue, querySourceReferenceExpression1))
                {
                    return Expression.Coalesce(conditionalExpression.IfTrue, conditionalExpression.IfFalse);
                }

                // Converts 'null != [q] ? [q] : [s]' into '[q] ?? [s]'

                if (binaryExpression.NodeType == ExpressionType.NotEqual
                    && binaryExpression.Right is QuerySourceReferenceExpression querySourceReferenceExpression2
                    && binaryExpression.Left.IsNullConstantExpression()
                    && ReferenceEquals(conditionalExpression.IfTrue, querySourceReferenceExpression2))
                {
                    return Expression.Coalesce(conditionalExpression.IfTrue, conditionalExpression.IfFalse);
                }

                // Converts '[q] == null ? [s] : [q]' into '[s] ?? [q]'

                if (binaryExpression.NodeType == ExpressionType.Equal
                    && binaryExpression.Left is QuerySourceReferenceExpression querySourceReferenceExpression3
                    && binaryExpression.Right.IsNullConstantExpression()
                    && ReferenceEquals(conditionalExpression.IfFalse, querySourceReferenceExpression3))
                {
                    return Expression.Coalesce(conditionalExpression.IfTrue, conditionalExpression.IfFalse);
                }

                // Converts 'null == [q] ? [s] : [q]' into '[s] ?? [q]'

                if (binaryExpression.NodeType == ExpressionType.Equal
                    && binaryExpression.Right is QuerySourceReferenceExpression querySourceReferenceExpression4
                    && binaryExpression.Left.IsNullConstantExpression()
                    && ReferenceEquals(conditionalExpression.IfFalse, querySourceReferenceExpression4))
                {
                    return Expression.Coalesce(conditionalExpression.IfTrue, conditionalExpression.IfFalse);
                }
            }

            if (conditionalExpression.IsNullPropagationCandidate(out var testExpression, out var resultExpression))
            {
                var nullCheckRemovalTestingVisitor = new NullCheckRemovalTestingVisitor();// _queryCompilationContext);

                if (nullCheckRemovalTestingVisitor.CanRemoveNullCheck(testExpression, resultExpression))
                {
                    return new NullConditionalExpression(testExpression, testExpression, resultExpression);
                }

                //return nullCheckRemovalTestingVisitor.CanRemoveNullCheck(testExpression, resultExpression)
                //    ? new NullConditionalExpression(testExpression, testExpression, resultExpression)
                //    : null;
            }

            return base.VisitConditional(conditionalExpression);
            //var nullConditional = TryConvertToNullConditional(conditionalExpression);

            //return nullConditional ?? base.VisitConditional(conditionalExpression);
        }

        private Expression TryConvertToNullConditional(ConditionalExpression node)
        {
            if (node.IsNullPropagationCandidate(out var testExpression, out var resultExpression))
            {
                var nullCheckRemovalTestingVisitor = new NullCheckRemovalTestingVisitor();// _queryCompilationContext);

                if (nullCheckRemovalTestingVisitor.CanRemoveNullCheck(testExpression, resultExpression))
                {
                    return new NullConditionalExpression(testExpression, testExpression, resultExpression);
                }

                //return nullCheckRemovalTestingVisitor.CanRemoveNullCheck(testExpression, resultExpression)
                //    ? new NullConditionalExpression(testExpression, testExpression, resultExpression)
                //    : null;
            }

            return null;


            //var binaryTest = node.Test as BinaryExpression;

            //if (binaryTest == null
            //    || !(binaryTest.NodeType == ExpressionType.Equal
            //         || binaryTest.NodeType == ExpressionType.NotEqual))
            //{
            //    return null;
            //}

            //var isLeftNullConstant = binaryTest.Left.IsNullConstantExpression();
            //var isRightNullConstant = binaryTest.Right.IsNullConstantExpression();

            //if (isLeftNullConstant == isRightNullConstant)
            //{
            //    return null;
            //}

            //if (binaryTest.NodeType == ExpressionType.Equal)
            //{
            //    var ifTrueConstant = node.IfTrue as ConstantExpression;
            //    if (ifTrueConstant == null
            //        || ifTrueConstant.Value != null)
            //    {
            //        return null;
            //    }
            //}
            //else
            //{
            //    var ifFalseConstant = node.IfFalse as ConstantExpression;
            //    if (ifFalseConstant == null
            //        || ifFalseConstant.Value != null)
            //    {
            //        return null;
            //    }
            //}

            //var testExpression = isLeftNullConstant ? binaryTest.Right : binaryTest.Left;
            //var resultExpression = binaryTest.NodeType == ExpressionType.Equal ? node.IfFalse : node.IfTrue;

            //var nullCheckRemovalTestingVisitor = new NullCheckRemovalTestingVisitor();// _queryCompilationContext);

            //return nullCheckRemovalTestingVisitor.CanRemoveNullCheck(testExpression, resultExpression)
            //    ? new NullConditionalExpression(testExpression, testExpression, resultExpression)
            //    : null;
        }

        private class NullCheckRemovalTestingVisitor : ExpressionVisitorBase
        {
            //private readonly QueryCompilationContext _queryCompilationContext;
            private IQuerySource _querySource;
            private bool? _canRemoveNullCheck;
            private string _propertyName;


            //public NullCheckRemovalTestingVisitor(QueryCompilationContext queryCompilationContext)
            //{
            //    _queryCompilationContext = queryCompilationContext;
            //}

            public bool CanRemoveNullCheck(Expression testExpression, Expression resultExpression)
            {
                AnalyzeTestExpression(testExpression);
                if (_querySource == null)
                {
                    return false;
                }

                Visit(resultExpression);

                return _canRemoveNullCheck ?? false;
            }

            public override Expression Visit(Expression node)
                => _canRemoveNullCheck == false
                || !(node is MemberExpression 
                    || node is QuerySourceReferenceExpression 
                    || node is MethodCallExpression
                    || node is TypeBinaryExpression
                    || node is UnaryExpression)
                ? node 
                : base.Visit(node);


            private void AnalyzeTestExpression(Expression expression)
            {
                if (expression.RemoveConvert() is QuerySourceReferenceExpression querySourceReferenceExpression)
                {
                    _querySource = querySourceReferenceExpression.ReferencedQuerySource;
                    _propertyName = null;

                    return;
                }

                if (expression.RemoveConvert() is MemberExpression memberExpression
                    && memberExpression.Expression.RemoveConvert() is QuerySourceReferenceExpression querySourceInstance)
                {
                    _querySource = querySourceInstance.ReferencedQuerySource;
                    _propertyName = memberExpression.Member.Name;

                    return;
                }

                if (expression.RemoveConvert() is MethodCallExpression methodCallExpression
                    && methodCallExpression.Method.IsEFPropertyMethod())
                {
                    if (methodCallExpression.Arguments[0].RemoveConvert() is QuerySourceReferenceExpression querySourceCaller)
                    {
                        if (methodCallExpression.Arguments[1] is ConstantExpression propertyNameExpression)
                        {
                            _querySource = querySourceCaller.ReferencedQuerySource;
                            _propertyName = (string)propertyNameExpression.Value;

                            // TODO: what is this for?
                            //if ((_queryCompilationContext.FindEntityType(_querySource)
                            //     ?? _queryCompilationContext.Model.FindEntityType(_querySource.ItemType))
                            //    ?.FindProperty(_propertyName)?.IsPrimaryKey()
                            //    ?? false)
                            //{
                            //    _propertyName = null;
                            //}
                        }
                    }
                }
            }

            //private void AnalyzeTestExpression(Expression expression)
            //{


            //    var currentExpression = expression;
            //    while (currentExpression is QuerySourceReferenceExpression
            //        || currentExpression is MemberExpression
            //        || currentExpression is MethodCallExpression
            //        || currentExpression.NodeType == ExpressionType.Convert)
            //    {
            //        currentExpression = currentExpression.RemoveConvert();

            //        if (expression is QuerySourceReferenceExpression querySourceReferenceExpression)
            //        {
            //            _querySource = querySourceReferenceExpression.ReferencedQuerySource;

            //            return;
            //        }

            //        if (expression is MemberExpression memberExpression)
            //        {
            //            _propertyNames.Insert(0, memberExpression.Member.Name);
            //            currentExpression = memberExpression.Expression;
            //        }

            //        if (expression is MethodCallExpression methodCallExpression
            //            && methodCallExpression.Method.IsEFPropertyMethod()
            //            && methodCallExpression.Arguments[1] is ConstantExpression propertyNameExpression)
            //        {
            //            _propertyNames.Insert(0, (string)propertyNameExpression.Value);
            //            currentExpression = methodCallExpression.Arguments[0];
            //        }
            //    }




            //        //{
            //        //    //if (methodCallExpression.Arguments[0] is QuerySourceReferenceExpression querySourceCaller)
            //        //    //{
            //        //        if (methodCallExpression.Arguments[1] is ConstantExpression propertyNameExpression)
            //        //        {
            //        //            _querySource = querySourceCaller.ReferencedQuerySource;
            //        //            _propertyName = (string)propertyNameExpression.Value;
            //        //            if ((_queryCompilationContext.FindEntityType(_querySource)
            //        //                 ?? _queryCompilationContext.Model.FindEntityType(_querySource.ItemType))
            //        //                ?.FindProperty(_propertyName)?.IsPrimaryKey()
            //        //                ?? false)
            //        //            {
            //        //                _propertyName = null;
            //        //            }
            //        //        }
            //        //    }
            //        //}






            //    //if ( expression is MemberExpression memberExpression
            //    //    && memberExpression.Expression is QuerySourceReferenceExpression querySourceInstance)
            //    //{
            //    //    _querySource = querySourceInstance.ReferencedQuerySource;
            //    //    _propertyName = memberExpression.Member.Name;

            //    //    return;
            //    //}

            //    //if (expression is MethodCallExpression methodCallExpression
            //    //    && methodCallExpression.Method.IsEFPropertyMethod())
            //    //{
            //    //    if (methodCallExpression.Arguments[0] is QuerySourceReferenceExpression querySourceCaller)
            //    //    {
            //    //        if (methodCallExpression.Arguments[1] is ConstantExpression propertyNameExpression)
            //    //        {
            //    //            _querySource = querySourceCaller.ReferencedQuerySource;
            //    //            _propertyName = (string)propertyNameExpression.Value;
            //    //            if ((_queryCompilationContext.FindEntityType(_querySource)
            //    //                 ?? _queryCompilationContext.Model.FindEntityType(_querySource.ItemType))
            //    //                ?.FindProperty(_propertyName)?.IsPrimaryKey()
            //    //                ?? false)
            //    //            {
            //    //                _propertyName = null;
            //    //            }
            //    //        }
            //    //    }
            //    //}
            //}


            protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression)
            {
                _canRemoveNullCheck
                    = expression.ReferencedQuerySource == _querySource
                      && _propertyName == null;

                return expression;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Member.Name == _propertyName)
                {
                    if (node.Expression is QuerySourceReferenceExpression querySource)
                    {
                        _canRemoveNullCheck = querySource.ReferencedQuerySource == _querySource;

                        return node;
                    }
                }

                return base.VisitMember(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.IsEFPropertyMethod())
                {
                    if (node.Arguments[1] is ConstantExpression propertyNameExpression
                        && (string)propertyNameExpression.Value == _propertyName)
                    {
                        if (node.Arguments[0] is QuerySourceReferenceExpression querySource)
                        {
                            _canRemoveNullCheck = querySource.ReferencedQuerySource == _querySource;

                            return node;
                        }
                    }
                }

                _canRemoveNullCheck = false;

                return node;
            }

            protected override Expression VisitUnary(UnaryExpression node)
            {
                if (node.NodeType == ExpressionType.Convert)
                {
                    return Visit(node.Operand);
                }

                _canRemoveNullCheck = false;

                return node;
            }

            protected override Expression VisitTypeBinary(TypeBinaryExpression node)
            {
                if (node.NodeType == ExpressionType.TypeAs)
                {
                    return Visit(node.Expression);
                }

                _canRemoveNullCheck = false;

                return node;
            }
        }
    }
}
