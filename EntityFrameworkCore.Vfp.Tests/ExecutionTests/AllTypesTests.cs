using EntityFrameworkCore.Vfp.Tests.Data.AllTypes;
using EntityFrameworkCore.Vfp.Tests.Fixtures;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EntityFrameworkCore.Vfp.Tests.ExecutionTests {
    public class AllTypesTests : DbContextExecutionTestBase<AllTypesContextFixture, AllTypesContext> {
        public AllTypesTests(AllTypesContextFixture fixture, ITestOutputHelper output) : base(fixture, output) {
        }

        [Fact]
        public void AddedRecord_and_RetrievedRecord_ShouldHaveSameValues() {
            var item = Insert();

            Execute(context => {
                var item2 = context.AllTypes.ToList().First();

                Assert.Equal(item.Id, item2.Id);
                Assert.Equal(item.BinaryChar, item2.BinaryChar);
                Assert.Equal(item.BinaryMemo, item2.BinaryMemo);
                Assert.Equal(item.BinaryVarChar, item2.BinaryVarChar);
                Assert.Equal(item.Char, item2.Char);
                Assert.Equal(item.Currency, item2.Currency);
                Assert.Equal(item.Date.Value.Date, item2.Date);
                Assert.Equal(item.DateTime, item2.DateTime);
                Assert.Equal(item.Decimal, item2.Decimal);
                Assert.Equal(item.Double, item2.Double);
                Assert.Equal(item.Float, item2.Float);
                Assert.Equal(item.Guid, item2.Guid);
                Assert.Equal(item.Integer, item2.Integer);
                Assert.Equal(item.Logical, item2.Logical);
                Assert.Equal(item.Long, item2.Long);
                Assert.Equal(item.Memo, item2.Memo);
                Assert.Equal(item.VarChar, item2.VarChar);
            });
        }

        private AllTypesTable Insert() {
            var item = new AllTypesTable();
            var testTime = new DateTime(2012, 2, 6, 8, 51, 33);

            Execute(context => {
                item.BinaryChar = "binarychar";
                item.BinaryMemo = "binarymemo";
                item.BinaryVarChar = "binaryvarchar";
                item.Char = "char";
                item.Currency = 1.2M;
                item.Date = testTime;
                item.DateTime = testTime;
                item.Decimal = 3.45M;
                item.Double = 5.6D;
                item.Float = 7.8F;
                item.Guid = new Guid("9211FB02-0654-41B7-82DA-6A38EC0DFD9A");
                item.Integer = 199;
                item.Logical = true;
                item.Long = (long)int.MaxValue + 1;
                item.Memo = "memo";
                item.VarChar = Guid.NewGuid().ToString().Substring(9);

                context.AllTypes.Add(item);

                context.SaveChanges();
            });

            return item;
        }
    }
}
