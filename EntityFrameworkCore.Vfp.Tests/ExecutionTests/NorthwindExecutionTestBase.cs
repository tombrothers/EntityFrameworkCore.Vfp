using EntityFrameworkCore.Vfp.Tests.Data.Northwind;
using EntityFrameworkCore.Vfp.Tests.Fixtures;
using Xunit.Abstractions;

namespace EntityFrameworkCore.Vfp.Tests.ExecutionTests {
    public abstract class NorthwindExecutionTestBase : DbContextExecutionTestBase<NorthwindContextFixture, NorthwindContext> {

        protected NorthwindExecutionTestBase(NorthwindContextFixture fixture, ITestOutputHelper output) : base(fixture, output) {
        }
    }
}
