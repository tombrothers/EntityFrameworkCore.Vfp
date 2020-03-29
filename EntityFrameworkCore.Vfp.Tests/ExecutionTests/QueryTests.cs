using System.Linq;
using EntityFrameworkCore.Vfp.Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace EntityFrameworkCore.Vfp.Tests.ExecutionTests {
    public class QueryTests : NorthwindExecutionTestBase {
        private const string CustomerId = "ALFKI";

        public QueryTests(NorthwindContextFixture northwindContextFixture, ITestOutputHelper output) : base(northwindContextFixture, output) {
        }

        [Fact]
        public void QueryCustomer_WithValidCustomerId_ShouldReturn1Record() => Execute(context => {
            var customers = context.Customers.Where(x => x.CustomerId == CustomerId).ToArray();

            Assert.Single(customers);
            Assert.Equal(CustomerId, customers[0].CustomerId);
        });

        [Fact]
        public void QueryingAllCustomers_ShouldHave91Results() => Execute(context => {
            Assert.Equal(91, context.Customers.ToArray().Length);
        });

        [Fact]
        public void QueryingTheCountOfAllCustomers_ShouldHave91Results() => Execute(context => {
            Assert.Equal(91, context.Customers.Count());
        });
    }
}
