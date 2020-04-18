using EntityFrameworkCore.Vfp.Tests.Data.Northwind.Models;
using EntityFrameworkCore.Vfp.Tests.Fixtures;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EntityFrameworkCore.Vfp.Tests.ExecutionTests {
    public class ExecutionTests2 : NorthwindExecutionTestBase {

        public ExecutionTests2(NorthwindContextFixture northwindContextFixture, ITestOutputHelper output) : base(northwindContextFixture, output) {
        }

        [Fact]
        public void TestInsert() => Execute(context => {
            var product = new Product {
                ProductName = "Test" + Environment.NewLine + "1",
            };

            context.Products.Add(product);
            context.SaveChanges();

            Assert.NotEqual(0, product.ProductId);

            product.ProductName = "Test" + Environment.NewLine + "2";
            context.SaveChanges();
            product = context.Products.OrderBy(x => x.ProductId).SingleOrDefault(x => x.ProductId == product.ProductId);

            Assert.Equal("Test" + Environment.NewLine + "2", product.ProductName);

            context.Products.Remove(product);
            context.SaveChanges();
            product = context.Products.OrderBy(x => x.ProductId).SingleOrDefault(x => x.ProductId == product.ProductId);

            Assert.Null(product);
        });

        [Fact]
        public void TestWhereTrue() => Execute(context => {
            Assert.Equal(91, context.Customers.Where(c => true).Count());
        });

        [Fact]
        public void SelectTakeTest() => Execute(context => {
            var list = context.Customers.OrderBy(x => x.Address.Address).Take(1).ToList();

            Assert.Single(list);
            Assert.Equal("1 rue Alsace-Lorraine", list[0].Address.Address);
        });

        [Fact]
        public void SelectDistinctTest() => Execute(context => {
            var list = context.Customers.Select(x => x.Address.Country).Distinct().ToList();

            Assert.Equal(21, list.Count);
        });

        [Fact]
        public void SelectOrderByTest() => Execute(context => {
            var list = context.Customers.OrderBy(x => x.Address.Address).ToList();

            Assert.Equal(91, list.Count);
            Assert.Equal("1 rue Alsace-Lorraine", list[0].Address.Address);
        });

        [Fact]
        public void SelectWhereCustomerIdTest() => Execute(context => {
            var list = context.Customers.Where(item => item.CustomerId == "ALFKI").ToList();

            Assert.Single(list);
        });

        [Fact]
        public void SelectTest() => Execute(context => {
            var list = context.Customers.ToList();

            Assert.Equal(91, list.Count);
        });
    }
}