// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Specification.Tests;
using Microsoft.EntityFrameworkCore.Specification.Tests.TestUtilities.Xunit;
using Xunit.Abstractions;

namespace Microsoft.EntityFrameworkCore.InMemory.FunctionalTests
{
    public class GearsOfWarQueryInMemoryTest : GearsOfWarQueryTestBase<InMemoryTestStore, GearsOfWarQueryInMemoryFixture>
    {
        public GearsOfWarQueryInMemoryTest(GearsOfWarQueryInMemoryFixture fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            //TestLoggerFactory.TestOutputHelper = testOutputHelper;
        }

        [ConditionalFact(Skip = "issue #7787")]
        public override void Select_null_propagation_optimization9()
        {
            base.Select_null_propagation_optimization9();
        }

        [ConditionalFact(Skip = "issue #7787")]
        public override void Select_null_propagation_negative3()
        {
            base.Select_null_propagation_negative3();
        }
    }
}
