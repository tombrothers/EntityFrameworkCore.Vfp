using EntityFrameworkCore.Vfp.Tests.Data.AllTypes;
using EntityFrameworkCore.Vfp.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using VfpClient;
using Xunit;
using Xunit.Abstractions;

namespace EntityFrameworkCore.Vfp.Tests.ExecutionTests {
    public class AllTypesTests : DbContextExecutionTestBase<AllTypesContextFixture, AllTypesContext> {
        public AllTypesTests(AllTypesContextFixture fixture, ITestOutputHelper output) : base(fixture, output) {
        }

        [Fact]
        public void Insert_WithEmptyValues_Update_WithValues_ShouldHaveExpectedValues() {
            var item = Insert_WithEmptyValues(true);

            Execute(context => {
                SetValues(item);
                context.Entry(item).State = EntityState.Modified;

                context.SaveChanges();
            });

            Execute(context => {
                var item2 = context.AllTypes.First(x => x.Id == item.Id);

                AssertEqual(item, item2);
            });
        }

        [Fact]
        public void Inserted_And_Retrieved_EmptyValues_With_SetNullOn() {
            var id = Insert_WithEmptyValues(true).Id;

            Execute(context => {
                var item = context.AllTypes.First(x => x.Id == id);

                Assert.Null(item.BinaryChar);
                Assert.Null(item.BinaryMemo);
                Assert.Null(item.BinaryVarChar);
                Assert.Null(item.Char);
                Assert.Null(item.Currency);
                Assert.Null(item.Date);
                Assert.Null(item.DateTime);
                Assert.Null(item.Decimal);
                Assert.Null(item.Double);
                Assert.Null(item.Float);
                Assert.Null(item.Integer);
                Assert.Null(item.Logical);
                Assert.Null(item.Long);
                Assert.Null(item.Memo);
                Assert.Null(item.VarChar);
            });
        }

        [Fact(Skip = "set null off isn't working")]
        public void Inserted_And_Retrieved_EmptyValues_With_SetNullOff() {
            var id = Insert_WithEmptyValues(false).Id;

            Execute(context => {
                var item = context.AllTypes.First(x => x.Id == id);

                Assert.Equal(string.Empty, item.BinaryChar);
                Assert.Equal(string.Empty, item.BinaryMemo);
                Assert.Equal(string.Empty, item.BinaryVarChar);

                Assert.Equal(string.Empty, item.Char);
                Assert.Equal(0, item.Currency);
                Assert.Equal(0, item.Decimal);
                Assert.Equal(0, item.Double);
                Assert.Equal(0, item.Float);
                Assert.Equal(0, item.Integer);
                Assert.Equal(false, item.Logical);
                Assert.Equal(0, item.Long);
                Assert.Equal(string.Empty, item.Memo);
                Assert.Equal(string.Empty, item.VarChar);

                var currentCulture = Thread.CurrentThread.CurrentCulture;

                try {
                    Assert.Equal("12/30/1899 12:00:00 AM", item.Date.ToString());
                    Assert.Equal("12/30/1899 12:00:00 AM", item.DateTime.ToString());
                }
                finally {
                    Thread.CurrentThread.CurrentCulture = currentCulture;
                }
            });
        }

        [Fact]
        public void AddedRecord_and_RetrievedRecord_ShouldHaveSameValues() {
            var item = Insert_WithValues();

            Execute(context => {
                var item2 = context.AllTypes.First(x => x.Id == item.Id);

                AssertEqual(item, item2);
            });
        }

        private static void AssertEqual(AllTypesTable item, AllTypesTable item2) {
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
        }

        private AllTypesTable Insert_WithEmptyValues(bool allowNulls) {
            var item = new AllTypesTable();
            
            Execute(context => {
                var connection = context.Database.GetDbConnection();
                var builder = new VfpConnectionStringBuilder(connection.ConnectionString) {
                    Null = allowNulls
                };

                connection.ConnectionString = builder.ConnectionString;

                item.Guid = Guid.NewGuid();

                context.AllTypes.Add(item);
                context.SaveChanges();
            });

            return item;
        }

        private AllTypesTable Insert_WithValues() {
            var item = new AllTypesTable();

            Execute(context => {
                SetValues(item);

                context.AllTypes.Add(item);

                context.SaveChanges();
            });

            return item;
        }

        private static void SetValues(AllTypesTable item) {
            var testTime = new DateTime(2012, 2, 6, 8, 51, 33);

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
        }
    }
}
