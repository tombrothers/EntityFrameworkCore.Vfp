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
        public void Query_WithTake_ShouldLimitResults() => Execute(context => {
            var items = context.Orders.OrderBy(x => x.Customer.CustomerId).Take(5).ToArray();

            Assert.Equal(5, items.Length);
        });

        [Fact]
        public void QueryCustomer_WithValidCustomerId_ShouldReturn_SingleResult() => Execute(context => {
            var customers = context.Customers.Where(x => x.CustomerId == CustomerId).ToArray();

            Assert.Single(customers);
            Assert.Equal(CustomerId, customers[0].CustomerId);
        });

        [Fact]
        public void QueryingAllCustomers_ShouldHave_91Results() => Execute(context => {
            Assert.Equal(91, context.Customers.ToArray().Length);
        });

        [Fact]
        public void QueryingTheCountOfAllCustomers_ShouldHave_91Results() => Execute(context => {
            Assert.Equal(91, context.Customers.Count());
        });
    }
}
