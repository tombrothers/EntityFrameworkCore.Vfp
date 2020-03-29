using System;
using EntityFrameworkCore.Vfp.Tests.Data.Northwind;
using EntityFrameworkCore.Vfp.Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace EntityFrameworkCore.Vfp.Tests.ExecutionTests {
    public abstract class NorthwindExecutionTestBase : IClassFixture<NorthwindContextFixture> {
        private readonly NorthwindContextFixture _northwindContextFixture;
        private readonly ITestOutputHelper _output;

        protected NorthwindExecutionTestBase(NorthwindContextFixture northwindContextFixture, ITestOutputHelper output) {
            _northwindContextFixture = northwindContextFixture;
            _output = output;
        }

        protected void Execute(Action<NorthwindContext> action) => _northwindContextFixture.Execute(_output, action);
    }
}
