using EntityFrameworkCore.Vfp.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EntityFrameworkCore.Vfp.Tests.Tests {
    // Derived from https://github.com/tombrothers/VfpEntityFrameworkProvider2/blob/master/Source/VfpEntityFrameworkProvider.Tests/ExecutionTests.cs
    public class ExecutionTests : NorthwindExecutionTestBase {

        public ExecutionTests(NorthwindContextFixture northwindContextFixture, ITestOutputHelper output) : base(northwindContextFixture, output) {
        }

        [Fact]
        public void TestNotInList() => Execute(context => {
            var ids = context.Orders.OrderBy(o => o.OrderId).Take(10).Select(o => o.OrderId).ToArray();
            var list = context.Orders.Where(o => !ids.Contains(o.OrderId)).ToList();

            Assert.Equal(820, list.Count());
        });

        [Fact]
        public void TestInList() => Execute(context => {
            var ids = context.Orders.OrderBy(o => o.OrderId).Take(10).Select(o => o.OrderId).ToArray();
            var list = context.Orders.Where(o => ids.Contains(o.OrderId)).Where(x => x.EmployeeId == 5).ToList();

            Assert.Equal(2, list.Count());
        });

        [Fact]
        public void TestWhere() => Execute(context => {
            Assert.Equal(6, context.Customers.Where(c => c.Address.City == "London").Count());
        });

        [Fact]
        public void TestWhereTrue() => Execute(context => {
            Assert.Equal(91, context.Customers.Where(c => true).Count());
        });

        [Fact(Skip = "Not valid syntax for EF Core?")]
        public void TestCompareConstructedEqual() => Execute(context => {
            Assert.Equal(6, context.Customers.Where(c => new { x = c.Address.City } == new { x = "London" }).Count());
        });

        [Fact(Skip = "Not valid syntax for EF Core?")]
        public void TestCompareConstructedMultiValueEqual() => Execute(context => {
            Assert.Equal(6, context.Customers.Where(c => new { x = c.Address.City, y = c.Address.Country } == new { x = "London", y = "UK" }).Count());
        });

        [Fact(Skip = "Not valid syntax for EF Core?")]
        public void TestCompareConstructedMultiValueNotEqual() => Execute(context => {
            Assert.Equal(85, context.Customers.Where(c => new { x = c.Address.City, y = c.Address.Country } != new { x = "London", y = "UK" }).Count());
        });

        [Fact]
        public void TestSelectScalar() => Execute(context => {
            var list = context.Customers.Where(c => c.Address.City == "London").Select(c => c.Address.City).ToList();

            Assert.Equal(6, list.Count());
            Assert.Equal("London", list[0]);
            Assert.True(list.All(x => x == "London"));
        });

        [Fact]
        public void TestSelectAnonymousOne() => Execute(context => {
            var list = context.Customers.Where(c => c.Address.City == "London").Select(c => new { c.Address.City }).ToList();

            Assert.Equal(6, list.Count());
            Assert.Equal("London", list[0].City);
            Assert.True(list.All(x => x.City == "London"));
        });

        [Fact]
        public void TestSelectAnonymousTwo() => Execute(context => {
            var list = context.Customers.Where(c => c.Address.City == "London").Select(c => new { c.Address.City, c.Phone }).ToList();

            Assert.Equal(6, list.Count());
            Assert.Equal("London", list[0].City);
            Assert.True(list.All(x => x.City == "London"));
            Assert.True(list.All(x => x.Phone != null));
        });

        [Fact]
        public void TestSelectCustomerTable() => Execute(context => {
            var list = context.Customers.ToList();
            Assert.Equal(91, list.Count());
        });

        [Fact]
        public void TestSelectAnonymousWithObject() => Execute(context => {
            var list = context.Customers.Where(c => c.Address.City == "London").Select(c => new { c.Address.City, c }).ToList();

            Assert.Equal(6, list.Count());
            Assert.Equal("London", list[0].City);
            Assert.True(list.All(x => x.City == "London"));
            Assert.True(list.All(x => x.c.Address.City == x.City));
        });

        [Fact]
        public void TestSelectAnonymousLiteral() => Execute(context => {
            var list = context.Customers.Where(c => c.Address.City == "London").Select(c => new { X = 10 }).ToList();

            Assert.Equal(6, list.Count());
            Assert.True(list.All(x => x.X == 10));
        });

        [Fact]
        public void TestSelectConstantInt() => Execute(context => {
            var list = context.Customers.Select(c => 10).ToList();

            Assert.Equal(91, list.Count());
            Assert.True(list.All(x => x == 10));
        });

        [Fact]
        public void TestSelectLocal() => Execute(context => {
            var x = 10;
            var list = context.Customers.Select(c => x).ToList();

            Assert.Equal(91, list.Count());
            Assert.True(list.All(y => y == 10));
        });

        [Fact]
        public void TestSelectNestedCollectionInAnonymousType() => Execute(context => {
            var list = (
                from c in context.Customers
                where c.CustomerId == "ALFKI"
                select new { Foos = context.Orders.Where(o => o.Customer.CustomerId == c.CustomerId).Select(o => o.OrderId).ToList() }
                ).ToList();

            Assert.Single(list);
            Assert.Equal(6, list[0].Foos.Count());
        });

        [Fact]
        public void TestJoinCustomerOrders() => Execute(context => {
            var list = (
                from c in context.Customers
                where c.CustomerId == "ALFKI"
                join o in context.Orders on c.CustomerId equals o.Customer.CustomerId
                select new { c.ContactName, o.OrderId }
                ).ToList();

            Assert.Equal(6, list.Count());
        });

        [Fact]
        public void TestJoinMultiKey() => Execute(context => {
            var list = (
                from c in context.Customers
                where c.CustomerId == "ALFKI"
                join o in context.Orders on new { a = c.CustomerId, b = c.CustomerId } equals new { a = o.Customer.CustomerId, b = o.Customer.CustomerId }
                select new { c, o }
                ).ToList();

            Assert.Equal(6, list.Count());
        });

        [Fact]
        public void TestJoinIntoDefaultIfEmpty() => Execute(context => {
            var list = (
                from c in context.Customers
                where c.CustomerId == "PARIS"
                join o in context.Orders on c.CustomerId equals o.Customer.CustomerId into ords
                from o in ords.DefaultIfEmpty()
                select new { c, o }
                ).ToList();

            Assert.Single(list);
            Assert.Null(list[0].o);
        });

        [Fact]
        public void TestOrderBy() => Execute(context => {
            var list = context.Customers.OrderBy(c => c.CustomerId).Select(c => c.CustomerId).ToList();
            var sorted = list.OrderBy(c => c).ToList();

            Assert.Equal(91, list.Count());
            Assert.True(Enumerable.SequenceEqual(list, sorted));
        });

        [Fact]
        public void TestOrderByOrderBy() => Execute(context => {
            var list = context.Customers.OrderBy(c => c.Phone).OrderBy(c => c.CustomerId).ToList();
            var sorted = list.OrderBy(c => c.CustomerId).ToList();

            Assert.Equal(91, list.Count());
            Assert.True(Enumerable.SequenceEqual(list, sorted));
        });

        [Fact]
        public void TestOrderByThenBy() => Execute(context => {
            var list = context.Customers.OrderBy(c => c.CustomerId).ThenBy(c => c.Phone).ToList();
            var sorted = list.OrderBy(c => c.CustomerId).ThenBy(c => c.Phone).ToList();

            Assert.Equal(91, list.Count());
            Assert.True(Enumerable.SequenceEqual(list, sorted));
        });

        [Fact]
        public void TestOrderByDescending() => Execute(context => {
            var list = context.Customers.OrderByDescending(c => c.CustomerId).ToList();
            var sorted = list.OrderByDescending(c => c.CustomerId).ToList();

            Assert.Equal(91, list.Count());
            Assert.True(Enumerable.SequenceEqual(list, sorted));
        });

        [Fact]
        public void TestOrderByDescendingThenBy() => Execute(context => {
            var list = context.Customers.OrderByDescending(c => c.CustomerId).ThenBy(c => c.Address.Country).ToList();
            var sorted = list.OrderByDescending(c => c.CustomerId).ThenBy(c => c.Address.Country).ToList();

            Assert.Equal(91, list.Count());
            Assert.True(Enumerable.SequenceEqual(list, sorted));
        });

        [Fact]
        public void TestOrderByDescendingThenByDescending() => Execute(context => {
            var list = context.Customers.OrderByDescending(c => c.CustomerId).ThenByDescending(c => c.Address.Country).ToList();
            var sorted = list.OrderByDescending(c => c.CustomerId).ThenByDescending(c => c.Address.Country).ToList();

            Assert.Equal(91, list.Count());
            Assert.True(Enumerable.SequenceEqual(list, sorted));
        });

        [Fact]
        public void TestGroupBy() => Execute(context => {
            Assert.Equal(69, context.Customers.GroupBy(c => c.Address.City).Select(x => x.Count()).ToList().Count());
        });

        [Fact]
        public void TestGroupByOne() => Execute(context => {
            var list = context.Customers.Where(c => c.Address.City == "London").GroupBy(c => c.Address.City).Select(x => x.Count()).ToList();

            Assert.Single(list);
            Assert.Equal(6, list[0]);
        });

        [Fact]
        public void TestGroupByCount() => Execute(context => {
            var list = context.Orders.Where(o => o.Customer.CustomerId == "ALFKI").GroupBy(o => o.Customer.CustomerId).Select(g => g.Count()).ToList();

            Assert.Single(list);
            Assert.Equal(6, list[0]);
        });

        [Fact]
        public void TestGroupByLongCount() => Execute(context => {
            var list = context.Orders.Where(o => o.Customer.CustomerId == "ALFKI").GroupBy(o => o.Customer.CustomerId).Select(g => g.LongCount()).ToList();

            Assert.Single(list);
            Assert.Equal(6L, list[0]);
        });

        [Fact]
        public void TestGroupByWithElementSelectorSum() => Execute(context => {
            var list = context.Orders.Where(o => o.Customer.CustomerId == "ALFKI").GroupBy(o => o.Customer.CustomerId, o => (o.Customer.CustomerId == "ALFKI" ? 1 : 1)).Select(g => g.Sum()).ToList();

            Assert.Single(list);
            Assert.Equal(6, list[0]);
        });

        [Fact]
        public void TestGroupByWithElementSelector() => Execute(context => {
            // note: groups are retrieved through a separately execute subquery per row
            var list = context.Orders
                .Where(o => o.Customer.CustomerId == "ALFKI")
                .GroupBy(o => o.Customer.CustomerId, o => (o.Customer.CustomerId == "ALFKI" ? 1 : 1))
                .Select(x => x.Sum())
                .ToList();

            Assert.Single(list);
            Assert.Equal(6, list.Single());
        });

        [Fact]
        public void TestGroupByWithElementSelectorSumMax() => Execute(context => {
            var list = context.Orders.Where(o => o.Customer.CustomerId == "ALFKI").GroupBy(o => o.Customer.CustomerId, o => (o.Customer.CustomerId == "ALFKI" ? 1 : 1)).Select(g => new { Sum = g.Sum(), Max = g.Max() }).ToList();

            Assert.Single(list);
            Assert.Equal(6, list[0].Sum);
            Assert.Equal(1, list[0].Max);
        });

        [Fact]
        public void TestGroupByWithTwoPartKey() => Execute(context => {
            var result = context.Orders
                .Where(o => o.Customer.CustomerId == "ALFKI")
                .GroupBy(o => new { o.Customer.CustomerId, o.OrderDate })
                .Select(x => x.Count())
                .ToList();

            Assert.Equal(6, result.Count());
        });

        [Fact]
        public void TestSumWithNoArg() => Execute(context => {
            var sum = context.Orders.Where(o => o.Customer.CustomerId == "ALFKI").Select(o => (o.Customer.CustomerId == "ALFKI" ? 1 : 1)).Sum();

            Assert.Equal(6, sum);
        });

        [Fact]
        public void TestSumWithArg() => Execute(context => {
            var sum = context.Orders.Where(o => o.Customer.CustomerId == "ALFKI").Sum(o => (o.Customer.CustomerId == "ALFKI" ? 1 : 1));

            Assert.Equal(6, sum);
        });

        [Fact]
        public void TestCountWithNoPredicate() => Execute(context => {
            var cnt = context.Orders.Count();

            Assert.Equal(830, cnt);
        });

        [Fact]
        public void TestCountWithPredicate() => Execute(context => {
            var cnt = context.Orders.Count(o => o.Customer.CustomerId == "ALFKI");

            Assert.Equal(6, cnt);
        });

        [Fact]
        public void TestDistinctNoDupes() => Execute(context => {
            var list = context.Customers.Distinct().ToList();

            Assert.Equal(91, list.Count());
        });

        [Fact]
        public void TestDistinctScalar() => Execute(context => {
            var list = context.Customers.Select(c => c.Address.City).Distinct().ToList();

            Assert.Equal(69, list.Count());
        });

        [Fact]
        public void TestOrderByDistinct() => Execute(context => {
            var list = context.Customers.Where(c => c.Address.City.StartsWith("P")).OrderBy(c => c.Address.City).Select(c => c.Address.City).Distinct().ToList();
            var sorted = list.OrderBy(x => x).ToList();

            Assert.Equal(list[0], sorted[0]);
            Assert.Equal(list[list.Count - 1], sorted[list.Count - 1]);
        });

        [Fact]
        public void TestDistinctOrderBy() => Execute(context => {
            var list = context.Customers.Where(c => c.Address.City.StartsWith("P")).Select(c => c.Address.City).Distinct().OrderBy(c => c).ToList();
            var sorted = list.OrderBy(x => x).ToList();

            Assert.Equal(list[0], sorted[0]);
            Assert.Equal(list[list.Count - 1], sorted[list.Count - 1]);
        });

        [Fact]
        public void TestDistinctCount() => Execute(context => {
            var cnt = context.Customers.Distinct().Count();

            Assert.Equal(91, cnt);
        });

        [Fact]
        public void TestSelectDistinctCount() => Execute(context => {
            // cannot do: SELECT COUNT(DISTINCT some-colum) FROM some-table
            // because COUNT(DISTINCT some-column) does not count nulls
            var cnt = context.Customers.Select(c => c.Address.City).Distinct().Count();

            Assert.Equal(69, cnt);
        });

        [Fact]
        public void TestSelectSelectDistinctCount() => Execute(context => {
            var cnt = context.Customers.Select(c => c.Address.City).Select(c => c).Distinct().Count();

            Assert.Equal(69, cnt);
        });

        [Fact]
        public void TestDistinctCountPredicate() => Execute(context => {
            var cnt = context.Customers.Select(c => new { c.Address.City, c.Address.Country }).Distinct().Count(c => c.City == "London");

            Assert.Equal(1, cnt);
        });

        [Fact]
        public void TestDistinctSumWithArg() => Execute(context => {
            var sum = context.Orders.Where(o => o.Customer.CustomerId == "ALFKI").Distinct().Sum(o => (o.Customer.CustomerId == "ALFKI" ? 1 : 1));

            Assert.Equal(6, sum);
        });

        [Fact]
        public void TestSelectDistinctSum() => Execute(context => {
            var sum = context.Orders.Where(o => o.Customer.CustomerId == "ALFKI").Select(o => o.OrderId).Distinct().Sum();

            Assert.Equal(64835, sum);
        });

        [Fact]
        public void TestTake() => Execute(context => {
            var list = context.Orders.OrderBy(o => o.Customer.CustomerId).Take(5).ToList();

            Assert.Equal(5, list.Count());
        });

        [Fact]
        public void TestTakeDistinct() => Execute(context => {
            // distinct must be forced to apply after top has been computed
            var list = context.Orders.OrderBy(o => o.Customer.CustomerId).Select(o => o.Customer.CustomerId).Take(5).Distinct().ToList();

            Assert.Single(list);
        });

        [Fact]
        public void TestDistinctTake() => Execute(context => {
            // top must be forced to apply after distinct has been computed
            var list = context.Orders.Select(o => o.Customer.CustomerId).Distinct().OrderBy(o => o).Take(5).ToList();

            Assert.Equal(5, list.Count());
        });

        [Fact]
        public void TestDistinctTakeCount() => Execute(context => {
            var cnt = context.Orders.Distinct().OrderBy(o => o.Customer.CustomerId).Select(o => o.Customer.CustomerId).Take(5).Count();

            Assert.Equal(5, cnt);
        });

        [Fact]
        public void TestTakeDistinctCount() => Execute(context => {
            var cnt = context.Orders.OrderBy(o => o.Customer.CustomerId).Select(o => o.Customer.CustomerId).Take(5).Distinct().Count();

            Assert.Equal(1, cnt);
        });

        [Fact]
        public void TestFirst() => Execute(context => {
            var first = context.Customers.OrderBy(c => c.ContactName).First();

            Assert.NotNull(first);
            Assert.Equal("ROMEY", first.CustomerId);
        });

        [Fact]
        public void TestFirstPredicate() => Execute(context => {
            var first = context.Customers.OrderBy(c => c.ContactName).First(c => c.Address.City == "London");

            Assert.NotNull(first);
            Assert.Equal("EASTC", first.CustomerId);
        });

        [Fact]
        public void TestWhereFirst() => Execute(context => {
            var first = context.Customers.OrderBy(c => c.ContactName).Where(c => c.Address.City == "London").First();

            Assert.NotNull(first);
            Assert.Equal("EASTC", first.CustomerId);
        });

        [Fact]
        public void TestFirstOrDefault() => Execute(context => {
            var first = context.Customers.OrderBy(c => c.ContactName).FirstOrDefault();

            Assert.NotNull(first);
            Assert.Equal("ROMEY", first.CustomerId);
        });

        [Fact]
        public void TestFirstOrDefaultPredicate() => Execute(context => {
            var first = context.Customers.OrderBy(c => c.ContactName).FirstOrDefault(c => c.Address.City == "London");

            Assert.NotNull(first);
            Assert.Equal("EASTC", first.CustomerId);
        });

        [Fact]
        public void TestWhereFirstOrDefault() => Execute(context => {
            var first = context.Customers.OrderBy(c => c.ContactName).Where(c => c.Address.City == "London").FirstOrDefault();

            Assert.NotNull(first);
            Assert.Equal("EASTC", first.CustomerId);
        });

        [Fact]
        public void TestFirstOrDefaultPredicateNoMatch() => Execute(context => {
            var first = context.Customers.OrderBy(c => c.ContactName).FirstOrDefault(c => c.Address.City == "SpongeBob");

            Assert.Null(first);
        });

        [Fact]
        public void TestSinglePredicate() => Execute(context => {
            var single = context.Customers.OrderBy(c => c.CustomerId).Single(c => c.CustomerId == "ALFKI");

            Assert.NotNull(single);
            Assert.Equal("ALFKI", single.CustomerId);
        });

        [Fact]
        public void TestWhereSingle() => Execute(context => {
            var single = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).Single();

            Assert.NotNull(single);
            Assert.Equal("ALFKI", single.CustomerId);
        });

        [Fact]
        public void TestSingleOrDefaultPredicate() => Execute(context => {
            var single = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI");

            Assert.NotNull(single);
            Assert.Equal("ALFKI", single.CustomerId);
        });

        [Fact]
        public void TestWhereSingleOrDefault() => Execute(context => {
            var single = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault();

            Assert.NotNull(single);
            Assert.Equal("ALFKI", single.CustomerId);
        });

        [Fact]
        public void TestSingleOrDefaultNoMatches() => Execute(context => {
            var single = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "SpongeBob");

            Assert.Null(single);
        });

        [Fact]
        public void TestAnyTopLevel() => Execute(context => {
            var any = context.Customers.Any();

            Assert.True(any);
        });

        [Fact]
        public void TestAnyWithSubquery() => Execute(context => {
            var list = context.Customers.Where(c => c.Orders.Any(o => o.Customer.CustomerId == "ALFKI")).ToList();

            Assert.Single(list);
        });

        [Fact]
        public void TestAnyWithSubqueryNoPredicate() => Execute(context => {
            // customers with at least one order
            var list = context.Customers.Where(c => context.Orders.Where(o => o.Customer.CustomerId == c.CustomerId).Any()).ToList();

            Assert.Equal(89, list.Count());
        });

        [Fact]
        public void TestAnyWithLocalCollection() => Execute(context => {
            // get customers for any one of these IDs
            var ids = new[] { "ALFKI", "WOLZA", "NOONE" };
            var list = context.Customers.Where(c => ids.Any(id => c.CustomerId == id)).ToList();

            Assert.Equal(2, list.Count());
        });

        [Fact]
        public void TestAllTopLevel() => Execute(context => {
            // all customers have name length > 0?
            var all = context.Customers.All(c => c.ContactName.Length > 0);

            Assert.True(all);
        });

        [Fact]
        public void TestAllTopLevelNoMatches() => Execute(context => {
            // all customers have name with 'a'
            var all = context.Customers.All(c => c.ContactName.Contains("a"));

            Assert.False(all);
        });

        [Fact]
        public void TestContainsWithSubquery() => Execute(context => {
            // this is the long-way to determine all customers that have at least one order
            var list = context.Customers.Where(c => context.Orders.Select(o => o.Customer.CustomerId).Contains(c.CustomerId)).ToList();

            Assert.Equal(89, list.Count());
        });

        [Fact]
        public void TestContainsWithLocalCollection() => Execute(context => {
            var ids = new[] { "ALFKI", "WOLZA", "NOONE" };
            var list = context.Customers.Where(c => ids.Contains(c.CustomerId)).ToList();

            Assert.Equal(2, list.Count());
        });

        [Fact]
        public void TestSkipTake() => Execute(context => {
            var list = context.Customers.OrderBy(c => c.CustomerId).Select(c => c.CustomerId).Skip(5).Take(10).ToList();

            Assert.Equal(10, list.Count());
            Assert.Equal("BLAUS", list[0]);
            Assert.Equal("COMMI", list[9]);
        });

        [Fact]
        public void TestSkipTakeWhere() => Execute(context => {
            var list = context.Customers
                .Where(x => x.CustomerId.StartsWith("A"))
                .OrderBy(x => x.CustomerId)
                .Select(c => c.CustomerId)
                .Skip(1)
                .Take(1)
                .ToList();

            Assert.Single(list);
            Assert.Equal("ANATR", list[0]);
        });

        [Fact]
        public void TestDistinctSkipTake() => Execute(context => {
            var list = context.Customers.Select(c => c.Address.City).Distinct().OrderBy(c => c).Skip(5).Take(10).ToList();

            Assert.Equal(10, list.Count());

            var hs = new HashSet<string>(list);

            Assert.Equal(10, hs.Count());
        });

        [Fact]
        public void TestCoalesce() => Execute(context => {
            var list = context.Customers.Select(c => new { City = (c.Address.City == "London" ? null : c.Address.City), Country = (c.CustomerId == "EASTC" ? null : c.Address.Country) })
                         .Where(x => (x.City ?? "NoCity") == "NoCity").ToList();

            Assert.Equal(6, list.Count());
            Assert.Null(list[0].City);
        });

        [Fact]
        public void TestCoalesce2() => Execute(context => {
            var list = context.Customers.Select(c => new { City = (c.Address.City == "London" ? null : c.Address.City), Country = (c.CustomerId == "EASTC" ? null : c.Address.Country) })
                         .Where(x => (x.City ?? x.Country ?? "NoCityOrCountry") == "NoCityOrCountry").ToList();

            Assert.Single(list);
            Assert.Null(list[0].City);
            Assert.Null(list[0].Country);
        });

        [Fact]
        public void TestStringLength() => Execute(context => {
            var list = context.Customers.Where(c => c.Address.City.Trim().Length == 7).ToList();

            Assert.Equal(9, list.Count());
        });

        [Fact]
        public void TestStringStartsWithLiteral() => Execute(context => {
            var list = context.Customers.Where(c => c.ContactName.StartsWith("M")).ToList();

            Assert.Equal(12, list.Count());
        });

        [Fact]
        public void TestStringStartsWithColumn() => Execute(context => {
            var list = context.Customers.Where(c => c.ContactName.StartsWith(c.ContactName)).ToList();

            Assert.Equal(91, list.Count());
        });

        [Fact]
        public void TestStringEndsWithLiteral() => Execute(context => {
            var list = context.Customers.Where(c => c.ContactName.EndsWith("s")).ToList();

            Assert.Equal(9, list.Count());
        });

        [Fact]
        public void TestStringEndsWithColumn() => Execute(context => {
            var list = context.Customers.Where(c => c.ContactName.EndsWith(c.ContactName)).ToList();

            Assert.Equal(91, list.Count());
        });

        [Fact]
        public void TestStringContainsLiteral() => Execute(context => {
            var list = context.Customers.Where(c => c.ContactName.Contains("nd")).Select(c => c.ContactName).ToList();
            var local = context.Customers.AsEnumerable().Where(c => c.ContactName.ToLower().Contains("nd")).Select(c => c.ContactName).ToList();

            Assert.Equal(local.Count, list.Count());
        });

        [Fact]
        public void TestStringContainsColumn() => Execute(context => {
            var list = context.Customers.Where(c => c.ContactName.Contains(c.ContactName)).ToList();

            Assert.Equal(91, list.Count());
        });

        [Fact]
        public void TestStringConcatImplicit2Args() => Execute(context => {
            var list = context.Customers.Where(c => c.ContactName.Trim() + "X" == "Maria AndersX").ToList();

            Assert.Single(list);
        });

        [Fact]
        public void TestStringConcatExplicit2Args() => Execute(context => {
            var list = context.Customers.Where(c => string.Concat(c.ContactName.Trim(), "X") == "Maria AndersX").ToList();

            Assert.Single(list);
        });

        [Fact]
        public void TestStringIsNullOrEmpty() => Execute(context => {
            var list = context.Customers.Select(c => c.Address.City == "London" ? null : c.CustomerId).Where(x => string.IsNullOrEmpty(x)).ToList();

            Assert.Equal(6, list.Count());
        });

        [Fact]
        public void TestStringToUpper() => Execute(context => {
            var str = context.Customers.Where(c => c.CustomerId == "ALFKI").Max(c => (c.CustomerId == "ALFKI" ? "abc" : "abc").ToUpper());

            Assert.Equal("ABC", str);
        });

        [Fact]
        public void TestStringToLower() => Execute(context => {
            var str = context.Customers.Where(c => c.CustomerId == "ALFKI").Max(c => (c.CustomerId == "ALFKI" ? "ABC" : "ABC").ToLower());

            Assert.Equal("abc", str);
        });

        [Fact]
        public void TestStringSubstring() => Execute(context => {
            var list = context.Customers.Where(c => c.Address.City.Substring(0, 4) == "Seat").ToList();

            Assert.Single(list);
            Assert.Equal("Seattle", list[0].Address.City);
        });

        [Fact]
        public void TestStringSubstringNoLength() => Execute(context => {
            var list = context.Customers.Where(c => c.Address.City.Substring(4) == "tle").ToList();

            Assert.Single(list);
            Assert.Equal("Seattle", list[0].Address.City);
        });

        [Fact]
        public void TestStringIndexOf() => Execute(context => {
            var n = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => c.ContactName.IndexOf("ar"));

            Assert.Equal(1, n);
        });

        [Fact]
        public void TestStringTrim() => Execute(context => {
            var notrim = context.Customers.Where(c => c.CustomerId == "ALFKI").Max(c => ("  " + c.Address.City + " "));
            var trim = context.Customers.Where(c => c.CustomerId == "ALFKI").Max(c => ("  " + c.Address.City + " ").Trim());

            Assert.NotEqual(notrim, trim);
            Assert.Equal(notrim.Trim(), trim);
        });

        [Fact]
        public void TestMathAbs() => Execute(context => {
            var neg1 = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Abs((c.CustomerId == "ALFKI") ? -1 : 0));
            var pos1 = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Abs((c.CustomerId == "ALFKI") ? 1 : 0));

            Assert.Equal(Math.Abs(-1), neg1);
            Assert.Equal(Math.Abs(1), pos1);
        });

        [Fact]
        public void TestMathPow() => Execute(context => {
            // 2^n
            var zero = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Pow((c.CustomerId == "ALFKI") ? 2.0 : 2.0, 0.0));
            var one = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Pow((c.CustomerId == "ALFKI") ? 2.0 : 2.0, 1.0));
            var two = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Pow((c.CustomerId == "ALFKI") ? 2.0 : 2.0, 2.0));
            var three = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Pow((c.CustomerId == "ALFKI") ? 2.0 : 2.0, 3.0));

            Assert.Equal(1.0, zero);
            Assert.Equal(2.0, one);
            Assert.Equal(4.0, two);
            Assert.Equal(8.0, three);
        });

        [Fact]
        public void TestMathRoundDefault() => Execute(context => {
            var four = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Round((c.CustomerId == "ALFKI") ? 3.4 : 3.4));
            var six = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Round((c.CustomerId == "ALFKI") ? 3.6 : 3.6));

            Assert.Equal(3.0, four);
            Assert.Equal(4.0, six);
        });

        [Fact]
        public void TestMathFloor() => Execute(context => {
            // The difference between floor and truncate is how negatives are handled.  Floor drops the decimals and moves the
            // value to the more negative, so Floor(-3.4) is -4.0 and Floor(3.4) is 3.0.
            var four = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Floor((c.CustomerId == "ALFKI" ? 3.4 : 3.4)));
            var six = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Floor((c.CustomerId == "ALFKI" ? 3.6 : 3.6)));
            var nfour = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => Math.Floor((c.CustomerId == "ALFKI" ? -3.4 : -3.4)));

            Assert.Equal(Math.Floor(3.4), four);
            Assert.Equal(Math.Floor(3.6), six);
            Assert.Equal(Math.Floor(-3.4), nfour);
        });

        [Fact]
        public void TestDecimalFloor() => Execute(context => {
            var four = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Floor((c.CustomerId == "ALFKI" ? 3.4m : 3.4m)));
            var six = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Floor((c.CustomerId == "ALFKI" ? 3.6m : 3.6m)));
            var nfour = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Floor((c.CustomerId == "ALFKI" ? -3.4m : -3.4m)));

            Assert.Equal(decimal.Floor(3.4m), four);
            Assert.Equal(decimal.Floor(3.6m), six);
            Assert.Equal(decimal.Floor(-3.4m), nfour);
        });

        [Fact]
        public void TestStringCompareTo() => Execute(context => {
            var lt = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => c.Address.City.CompareTo("Seattle"));
            var gt = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => c.Address.City.CompareTo("Aaa"));
            var eq = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => c.Address.City.CompareTo("Berlin"));

            Assert.Equal(-1, lt);
            Assert.Equal(1, gt);
            Assert.Equal(0, eq);
        });

        [Fact]
        public void TestStringCompareToLT() => Execute(context => {
            var cmpLT = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Seattle") < 0);
            var cmpEQ = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Berlin") < 0);

            Assert.NotNull(cmpLT);
            Assert.Null(cmpEQ);
        });

        [Fact]
        public void TestStringCompareToLE() => Execute(context => {
            var cmpLE = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Seattle") <= 0);
            var cmpEQ = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Berlin") <= 0);
            var cmpGT = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Aaa") <= 0);

            Assert.NotNull(cmpLE);
            Assert.NotNull(cmpEQ);
            Assert.Null(cmpGT);
        });

        [Fact]
        public void TestStringCompareToGT() => Execute(context => {
            var cmpLT = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Aaa") > 0);
            var cmpEQ = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Berlin") > 0);

            Assert.NotNull(cmpLT);
            Assert.Null(cmpEQ);
        });

        [Fact]
        public void TestStringCompareToGE() => Execute(context => {
            var cmpLE = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Seattle") >= 0);
            var cmpEQ = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Berlin") >= 0);
            var cmpGT = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Aaa") >= 0);

            Assert.Null(cmpLE);
            Assert.NotNull(cmpEQ);
            Assert.NotNull(cmpGT);
        });

        [Fact]
        public void TestStringCompareToEQ() => Execute(context => {
            var cmpLE = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Seattle") == 0);
            var cmpEQ = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Berlin") == 0);
            var cmpGT = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Aaa") == 0);

            Assert.Null(cmpLE);
            Assert.NotNull(cmpEQ);
            Assert.Null(cmpGT);
        });

        [Fact]
        public void TestStringCompareToNE() => Execute(context => {
            var cmpLE = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Seattle") != 0);
            var cmpEQ = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Berlin") != 0);
            var cmpGT = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => c.Address.City.CompareTo("Aaa") != 0);

            Assert.NotNull(cmpLE);
            Assert.Null(cmpEQ);
            Assert.NotNull(cmpGT);
        });

        [Fact]
        public void TestStringCompare() => Execute(context => {
            var lt = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => string.Compare(c.Address.City, "Seattle"));
            var gt = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => string.Compare(c.Address.City, "Aaa"));
            var eq = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => string.Compare(c.Address.City, "Berlin"));

            Assert.Equal(-1, lt);
            Assert.Equal(1, gt);
            Assert.Equal(0, eq);
        });

        [Fact]
        public void TestStringCompareLT() => Execute(context => {
            var cmpLT = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Seattle") < 0);
            var cmpEQ = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Berlin") < 0);

            Assert.NotNull(cmpLT);
            Assert.Null(cmpEQ);
        });

        [Fact]
        public void TestStringCompareLE() => Execute(context => {
            var cmpLE = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Seattle") <= 0);
            var cmpEQ = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Berlin") <= 0);
            var cmpGT = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Aaa") <= 0);

            Assert.NotNull(cmpLE);
            Assert.NotNull(cmpEQ);
            Assert.Null(cmpGT);
        });

        [Fact]
        public void TestStringCompareGT() => Execute(context => {
            var cmpLT = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Aaa") > 0);
            var cmpEQ = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Berlin") > 0);

            Assert.NotNull(cmpLT);
            Assert.Null(cmpEQ);
        });

        [Fact]
        public void TestStringCompareGE() => Execute(context => {
            var cmpLE = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Seattle") >= 0);
            var cmpEQ = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Berlin") >= 0);
            var cmpGT = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Aaa") >= 0);

            Assert.Null(cmpLE);
            Assert.NotNull(cmpEQ);
            Assert.NotNull(cmpGT);
        });

        [Fact]
        public void TestStringCompareEQ() => Execute(context => {
            var cmpLE = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Seattle") == 0);
            var cmpEQ = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Berlin") == 0);
            var cmpGT = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Aaa") == 0);

            Assert.Null(cmpLE);
            Assert.NotNull(cmpEQ);
            Assert.Null(cmpGT);
        });

        [Fact]
        public void TestStringCompareNE() => Execute(context => {
            var cmpLE = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Seattle") != 0);
            var cmpEQ = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Berlin") != 0);
            var cmpGT = context.Customers.Where(c => c.CustomerId == "ALFKI").OrderBy(c => c.CustomerId).SingleOrDefault(c => string.Compare(c.Address.City, "Aaa") != 0);

            Assert.NotNull(cmpLE);
            Assert.Null(cmpEQ);
            Assert.NotNull(cmpGT);
        });

        [Fact]
        public void TestIntCompareTo() => Execute(context => {
            // prove that x.CompareTo(y) works for types other than string
            var eq = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => (c.CustomerId == "ALFKI" ? 10 : 10).CompareTo(10));
            var gt = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => (c.CustomerId == "ALFKI" ? 10 : 10).CompareTo(9));
            var lt = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => (c.CustomerId == "ALFKI" ? 10 : 10).CompareTo(11));

            Assert.Equal(0, eq);
            Assert.Equal(1, gt);
            Assert.Equal(-1, lt);
        });

        [Fact]
        public void TestDecimalCompare() => Execute(context => {
            // prove that type.Compare(x,y) works with decimal
            var eq = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Compare((c.CustomerId == "ALFKI" ? 10m : 10m), 10m));
            var gt = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Compare((c.CustomerId == "ALFKI" ? 10m : 10m), 9m));
            var lt = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Compare((c.CustomerId == "ALFKI" ? 10m : 10m), 11m));

            Assert.Equal(0, eq);
            Assert.Equal(1, gt);
            Assert.Equal(-1, lt);
        });

        [Fact]
        public void TestDecimalRoundDefault() => Execute(context => {
            var four = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Round((c.CustomerId == "ALFKI" ? 3.4m : 3.4m)));
            var six = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => decimal.Round((c.CustomerId == "ALFKI" ? 3.5m : 3.5m)));

            Assert.Equal(3.0m, four);
            Assert.Equal(4.0m, six);
        });

        [Fact]
        public void TestDecimalLT() => Execute(context => {
            // prove that decimals are treated normally with respect to normal comparison operators
            var alfki = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1.0m : 3.0m) < 2.0m);

            Assert.NotNull(alfki);
        });

        [Fact]
        public void TestIntLessThan() => Execute(context => {
            var alfki = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1 : 3) < 2);
            var alfkiN = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 3 : 1) < 2);

            Assert.NotNull(alfki);
            Assert.Null(alfkiN);
        });

        [Fact]
        public void TestIntLessThanOrEqual() => Execute(context => {
            var alfki = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1 : 3) <= 2);
            var alfki2 = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 2 : 3) <= 2);
            var alfkiN = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 3 : 1) <= 2);

            Assert.NotNull(alfki);
            Assert.NotNull(alfki2);
            Assert.Null(alfkiN);
        });

        [Fact]
        public void TestIntGreaterThan() => Execute(context => {
            var alfki = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 3 : 1) > 2);
            var alfkiN = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1 : 3) > 2);

            Assert.NotNull(alfki);
            Assert.Null(alfkiN);
        });

        [Fact]
        public void TestIntGreaterThanOrEqual() => Execute(context => {
            var alfki = context.Customers.OrderBy(c => c.CustomerId).Single(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 3 : 1) >= 2);
            var alfki2 = context.Customers.OrderBy(c => c.CustomerId).Single(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 3 : 2) >= 2);
            var alfkiN = context.Customers.OrderBy(c => c.CustomerId).OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1 : 3) > 2);

            Assert.NotNull(alfki);
            Assert.NotNull(alfki2);
            Assert.Null(alfkiN);
        });

        [Fact]
        public void TestIntEqual() => Execute(context => {
            var alfki = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1 : 1) == 1);
            var alfkiN = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 1 : 1) == 2);

            Assert.NotNull(alfki);
            Assert.Null(alfkiN);
        });

        [Fact]
        public void TestIntNotEqual() => Execute(context => {
            var alfki = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 2 : 2) != 1);
            var alfkiN = context.Customers.OrderBy(c => c.CustomerId).SingleOrDefault(c => c.CustomerId == "ALFKI" && (c.CustomerId == "ALFKI" ? 2 : 2) != 2);

            Assert.NotNull(alfki);
            Assert.Null(alfkiN);
        });

        [Fact]
        public void TestIntAdd() => Execute(context => {
            var three = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 1 : 1) + 2);

            Assert.Equal(3, three);
        });

        [Fact]
        public void TestIntSubtract() => Execute(context => {
            var negone = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 1 : 1) - 2);

            Assert.Equal(-1, negone);
        });

        [Fact]
        public void TestIntMultiply() => Execute(context => {
            var six = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 2 : 2) * 3);

            Assert.Equal(6, six);
        });

        [Fact]
        public void TestIntDivide() => Execute(context => {
            var one = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 3.0 : 3.0) / 2);

            Assert.Equal(1.5, one);
        });

        [Fact]
        public void TestIntModulo() => Execute(context => {
            var three = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 7 : 7) % 4);

            Assert.Equal(3, three);
        });

        [Fact]
        public void TestIntBitwiseAnd() => Execute(context => {
            var band = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 6 : 6) & 3);

            Assert.Equal(2, band);
        });

        [Fact]
        public void TestIntBitwiseOr() => Execute(context => {
            var eleven = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 10 : 10) | 3);

            Assert.Equal(11, eleven);
        });

        [Fact(Skip = "Unsupported Binary operator type specified")]
        public void TestIntBitwiseExclusiveOr() => Execute(context => {
            var zero = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => ((c.CustomerId == "ALFKI") ? 1 : 1) ^ 1);

            Assert.Equal(0, zero);
        });

        [Fact(Skip = "Unsupported Binary operator type specified")]
        public void TestIntBitwiseNot() => Execute(context => {
            var bneg = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => ~((c.CustomerId == "ALFKI") ? -1 : -1));

            Assert.Equal(~-1, bneg);
        });

        [Fact]
        public void TestIntNegate() => Execute(context => {
            var neg = context.Customers.Where(c => c.CustomerId == "ALFKI").Sum(c => -((c.CustomerId == "ALFKI") ? 1 : 1));

            Assert.Equal(-1, neg);
        });

        [Fact]
        public void TestAnd() => Execute(context => {
            var custs = context.Customers.Where(c => c.Address.Country == "USA" && c.Address.City.StartsWith("A")).Select(c => c.Address.City).ToList();

            Assert.Equal(2, custs.Count());
            Assert.True(custs.All(c => c.StartsWith("A")));
        });

        [Fact]
        public void TestOr() => Execute(context => {
            var custs = context.Customers.Where(c => c.Address.Country == "USA" || c.Address.City.StartsWith("A")).Select(c => c.Address.City).ToList();

            Assert.Equal(14, custs.Count());
        });

        [Fact]
        public void TestNot() => Execute(context => {
            var custs = context.Customers.Where(c => !(c.Address.Country == "USA")).Select(c => c.Address.Country).ToList();

            Assert.Equal(78, custs.Count());
        });

        [Fact]
        public void TestEqualLiteralNull() => Execute(context => {
            var query = context.Customers.Select(c => c.CustomerId == "ALFKI" ? null : c.CustomerId).Where(x => x == null);
            var queryText = query.ToQueryString();

            Assert.Contains("IS NULL", queryText);
            Assert.Equal(1, query.Count());
        });

        [Fact]
        public void TestEqualLiteralNullReversed() => Execute(context => {
            var query = context.Customers.Select(c => c.CustomerId == "ALFKI" ? null : c.CustomerId).Where(x => null == x);
            var queryText = query.ToQueryString();

            Assert.Contains("IS NULL", queryText);
            Assert.Equal(1, query.Count());
        });

        [Fact]
        public void TestNotEqualLiteralNull() => Execute(context => {
            var query = context.Customers.Select(c => c.CustomerId == "ALFKI" ? null : c.CustomerId).Where(x => x != null);
            var queryText = query.ToQueryString();

            Assert.Contains("IS NOT NULL", queryText);
            Assert.Equal(90, query.Count());
        });

        [Fact]
        public void TestNotEqualLiteralNullReversed() => Execute(context => {
            var query = context.Customers.Select(c => c.CustomerId == "ALFKI" ? null : c.CustomerId).Where(x => null != x);
            var queryText = query.ToQueryString();

            Assert.Contains("IS NOT NULL", queryText);
            Assert.Equal(90, query.Count());
        });

        [Fact]
        public void TestSelectManyJoined() => Execute(context => {
            var cods =
                (from c in context.Customers
                 from o in context.Orders.Where(o => o.Customer.CustomerId == c.CustomerId)
                 select new { c.ContactName, o.OrderDate }).ToList();

            Assert.Equal(830, cods.Count());
        });

        [Fact]
        public void TestSelectManyJoinedDefaultIfEmpty() => Execute(context => {
            var cods = (
                from c in context.Customers
                from o in context.Orders.Where(o => o.Customer.CustomerId == c.CustomerId).DefaultIfEmpty()
                select new { ContactName = c.ContactName, OrderDate = (o == null ? (DateTime?)null : o.OrderDate) }
                ).ToList();

            Assert.Equal(832, cods.Count());
        });

        [Fact]
        public void TestSelectWhereAssociation() => Execute(context => {
            var ords = (
                from o in context.Orders
                where o.Customer.Address.City == "Seattle"
                select o
                ).ToList();

            Assert.Equal(14, ords.Count());
        });

        [Fact]
        public void TestSelectWhereAssociationTwice() => Execute(context => {
            var n = context.Orders.Where(c => c.Customer.CustomerId == "WHITC").Count();
            var ords = (
                from o in context.Orders
                where o.Customer.Address.Country == "USA" && o.Customer.Address.City == "Seattle"
                select o
                ).ToList();

            Assert.Equal(n, ords.Count());
        });

        [Fact]
        public void TestSelectAssociation() => Execute(context => {
            var custs = (
                from o in context.Orders
                where o.Customer.CustomerId == "ALFKI"
                select o.Customer
                ).ToList();

            Assert.Equal(6, custs.Count());
            Assert.True(custs.All(c => c.CustomerId == "ALFKI"));
        });

        [Fact]
        public void TestSelectAssociations() => Execute(context => {
            var doubleCusts = (
                from o in context.Orders
                where o.Customer.CustomerId == "ALFKI"
                select new { A = o.Customer, B = o.Customer }
                ).ToList();

            Assert.Equal(6, doubleCusts.Count());
            Assert.True(doubleCusts.All(c => c.A.CustomerId == "ALFKI" && c.B.CustomerId == "ALFKI"));
        });

        [Fact]
        public void TestSelectAssociationsWhereAssociations() => Execute(context => {
            var stuff = (
                from o in context.Orders
                where o.Customer.Address.Country == "USA"
                where o.Customer.Address.City != "Seattle"
                select new { A = o.Customer, B = o.Customer }
                ).ToList();

            Assert.Equal(108, stuff.Count());
        });

        [Fact]
        public void TestCustomersIncludeOrders() => Execute(context => {
            var custs = context.Customers.Include("Orders").Where(c => c.CustomerId == "ALFKI").ToList();

            Assert.Single(custs);
            Assert.NotNull(custs[0].Orders);
            Assert.Equal(6, custs[0].Orders.Count());
        });

        [Fact]
        public void TestCustomersIncludeOrdersAndDetails() => Execute(context => {
            var custs = context.Customers.Include("Orders").Include("Orders.OrderDetails").Where(c => c.CustomerId == "ALFKI").ToList();

            Assert.Single(custs);
            Assert.NotNull(custs[0].Orders);
            Assert.Equal(6, custs[0].Orders.Count());
            Assert.Contains(custs[0].Orders, o => o.OrderId == 10643);
            Assert.NotNull(custs[0].Orders.Single(o => o.OrderId == 10643).OrderDetails);
            Assert.Equal(3, custs[0].Orders.Single(o => o.OrderId == 10643).OrderDetails.Count());
        });
    }
}
