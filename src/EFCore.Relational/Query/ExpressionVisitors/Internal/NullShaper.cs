// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Clauses;

namespace Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal
{
    public sealed class NullShaper : IShaper<ValueBuffer>
    {
        public static readonly NullShaper Instance = new NullShaper();

        private NullShaper()
        {
        }

        public ValueBuffer Shape(QueryContext queryContext, ValueBuffer valueBuffer) => valueBuffer;

        public bool IsShaperForQuerySource(IQuerySource querySource) => false;

        public void SaveAccessorExpression(QuerySourceMapping querySourceMapping)
        {
        }

        public Expression GetAccessorExpression(IQuerySource querySource) => null;
    }
}
