using EntityFrameworkCore.Vfp.Query;
using EntityFrameworkCore.Vfp.Tests.Data.Northwind;
using EntityFrameworkCore.Vfp.Tests.Data.Northwind.Models;
using EntityFrameworkCore.Vfp.Tests.Fixtures;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EntityFrameworkCore.Vfp.Tests.Tests {
    public class VfpFunctionsTests : NorthwindExecutionTestBase {
        public VfpFunctionsTests(NorthwindContextFixture northwindContextFixture, ITestOutputHelper output) : base(northwindContextFixture, output) {
        }
        [Fact]
        public void VfpFunctionsTests_IsDigit_True_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.IsDigit(x.ShipAddress.Substring(0, 1))).First();

            Assert.Equal(true, result);

            result = GetOrderQuery(context).Select(x => VfpFunctions.IsDigit(x.ShipCountry.Substring(0, 1))).First();

            Assert.Equal(false, result);
        });

        [Fact]
        public void VfpFunctionsTests_Sec_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Sec(x.OrderDate)).First();

            Assert.Equal(0, result);
        });

        [Fact]
        public void VfpFunctionsTests_CMonth_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.CMonth(x.OrderDate)).First();

            Assert.Equal("July", result);
        });

        [Fact]
        public void VfpFunctionsTests_Cdow_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Cdow(x.OrderDate)).First();

            Assert.Equal("Thursday", result);
        });

        [Fact]
        public void VfpFunctionsTests_Tan_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Tan((decimal)(x.EmployeeId * .1))).First();

            Assert.Equal(0.54630248984379, result);
        });

        [Fact]
        public void VfpFunctionsTests_Sqrt_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Sqrt((decimal)x.EmployeeId - 1)).First();

            Assert.Equal(2, result);
        });

        [Fact]
        public void VfpFunctionsTests_Sin_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Sin((double)x.EmployeeId * .1)).First();

            Assert.Equal(0.4794255386042, result);
        });

        [Fact]
        public void VfpFunctionsTests_Sign_Negative_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Sign((double)x.EmployeeId - (x.EmployeeId + 94))).First();

            Assert.Equal(-1, result);
        });

        [Fact]
        public void VfpFunctionsTests_Sign_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Sign((decimal)x.EmployeeId + 94)).First();

            Assert.Equal(1, result);
        });

        [Fact]
        public void VfpFunctionsTests_Rand_Seed_Test() => Execute(context => {
            var result = (int)GetOrderQuery(context).Select(x => VfpFunctions.Rand(x.EmployeeId - 4) * 100).First();

            Assert.NotEqual(0, result);
        });

        [Fact]
        public void VfpFunctionsTests_Dtor_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Dtor((decimal)x.EmployeeId + 85)).First();

            Assert.Equal(1.5707963267949M, result);
        });

        [Fact]
        public void VfpFunctionsTests_Log10_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Log10(x.EmployeeId * .1)).First();

            Assert.Equal(-0.301029995663981, result);
        });

        [Fact]
        public void VfpFunctionsTests_Log_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Log(x.EmployeeId * .1)).First();

            Assert.Equal(-0.693147180559945, result);
        });

        [Fact]
        public void VfpFunctionsTests_Exp_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Exp((decimal)x.EmployeeId - 4)).First();

            Assert.Equal(2.71828182845905, result);
        });

        [Fact]
        public void VfpFunctionsTests_Rtod_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Rtod((decimal)(x.EmployeeId - 4))).First();

            Assert.Equal(57.29577951308230M, result);
        });

        [Fact]
        public void VfpFunctionsTests_Cos_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Cos(x.EmployeeId * .1)).First();

            Assert.Equal(0.87758256189037, result);
        });

        [Fact]
        public void VfpFunctionsTests_Atn2_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Atn2(x.EmployeeId * .1, x.EmployeeId * .1)).First();

            Assert.Equal(0.78539816339745, result);
        });

        [Fact]
        public void VfpFunctionsTests_Atan_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Atan(x.EmployeeId * .1)).First();

            Assert.Equal(0.46364760900081, result);
        });

        [Fact]
        public void VfpFunctionsTests_Asin_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Asin(x.EmployeeId * .1)).First();

            Assert.Equal(0.5235987755983, result);
        });

        [Fact]
        public void VfpFunctionsTests_Acos_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Acos(x.EmployeeId * .1)).First();

            Assert.Equal(1.0471975511966, result);
        });

        [Fact]
        public void VfpFunctionsTests_Substr_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Substr(x.ShipName, 9, 7)).First();

            Assert.Equal("alcools", result);
        });

        [Fact]
        public void VfpFunctionsTests_Stuff_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Stuff(x.ShipName, 6, 2, "xx")).First();

            Assert.Equal("Vins xx alcools Chevalier", result);
        });

        [Fact]
        public void VfpFunctionsTests_Str_Length_DecimalPlaces_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Str(x.Freight, 15, 1)).First();

            Assert.Equal("           32.4", result);
        });

        [Fact]
        public void VfpFunctionsTests_Str_Length_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Str(x.Freight, 15)).First();

            Assert.Equal("             32", result);
        });

        [Fact]
        public void VfpFunctionsTests_Str_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Str(x.Freight)).First();

            Assert.Equal("        32", result);
        });

        [Fact]
        public void VfpFunctionsTests_Space_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => "x" + VfpFunctions.Space(x.EmployeeId) + "x").First();

            Assert.Equal("x     x", result);
        });

        [Fact]
        public void VfpFunctionsTests_AllTrimTest() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.AllTrim("  " + x.Customer.CustomerId)).First();

            Assert.Equal("VINET", result);
        });

        [Fact]
        public void VfpFunctionsTests_Replicate_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Replicate(x.ShipCountry.Substring(0, 1), 3)).First();

            Assert.Equal("FFF", result);
        });

        [Fact]
        public void VfpFunctionsTests_Strtran_CaseMatch_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Strtran(x.ShipName, "I", "X", 1, 1, 2)).First();

            Assert.Equal("Vins et alcools Chevalier", result);

            result = GetOrderQuery(context).Select(x => VfpFunctions.Strtran(x.ShipName, "i", "X", 1, 1, 2)).First();

            Assert.Equal("Vxns et alcools Chevalier", result);
        });

        [Fact]
        public void VfpFunctionsTests_Strtran_CaseInsensitive_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Strtran(x.ShipName, "I", "X", 1, -1, 1)).First();

            Assert.Equal("VXns et alcools ChevalXer", result);

            result = GetOrderQuery(context).Select(x => VfpFunctions.Strtran(x.ShipName, "i", "X", 1, -1, 1)).First();

            Assert.Equal("VXns et alcools ChevalXer", result);
        });

        [Fact]
        public void VfpFunctionsTests_Strtran_NumerOfOccurance_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Strtran(x.ShipName, "I", "X", 1, 1)).First();

            Assert.Equal("Vins et alcools Chevalier", result);

            result = GetOrderQuery(context).Select(x => VfpFunctions.Strtran(x.ShipName, "i", "X", 1, 1)).First();

            Assert.Equal("VXns et alcools Chevalier", result);
        });

        [Fact]
        public void VfpFunctionsTests_Strtran_StartOccurance_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Strtran(x.ShipName, "I", "X", 2)).First();

            Assert.Equal("Vins et alcools Chevalier", result);

            result = GetOrderQuery(context).Select(x => VfpFunctions.Strtran(x.ShipName, "i", "X", 2)).First();

            Assert.Equal("Vins et alcools ChevalXer", result);
        });

        [Fact]
        public void VfpFunctionsTests_Strtran_Default_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Strtran(x.ShipName, "I", "X")).First();

            Assert.Equal("Vins et alcools Chevalier", result);

            result = GetOrderQuery(context).Select(x => VfpFunctions.Strtran(x.ShipName, "i", "X")).First();

            Assert.Equal("VXns et alcools ChevalXer", result);
        });

        [Fact]
        public void VfpFunctionsTests_ATC_LongOccurance_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Atc("I", x.ShipName, (long)2)).First();

            Assert.Equal(23, result);

            result = GetOrderQuery(context).Select(x => VfpFunctions.Atc("i", x.ShipName, (long)2)).First();

            Assert.Equal(23, result);
        });

        [Fact]
        public void VfpFunctionsTests_ATC_IntOccurance_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Atc("I", x.ShipName, 2)).First();

            Assert.Equal(23, result);

            result = GetOrderQuery(context).Select(x => VfpFunctions.Atc("i", x.ShipName, 2)).First();

            Assert.Equal(23, result);
        });

        [Fact]
        public void VfpFunctionsTests_ATC_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Atc("I", x.ShipName)).First();

            Assert.Equal(2, result);

            result = GetOrderQuery(context).Select(x => VfpFunctions.Atc("i", x.ShipName)).First();

            Assert.Equal(2, result);
        });

        [Fact]
        public void VfpFunctionsTests_AT_LongOccurance_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.At("I", x.ShipName, (long)2)).First();
            Assert.Equal(0, result);

            result = GetOrderQuery(context).Select(x => VfpFunctions.At("i", x.ShipName, (long)2)).First();

            Assert.Equal(23, result);
        });

        [Fact]
        public void VfpFunctionsTests_AT_IntOccurance_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.At("I", x.ShipName, 2)).First();

            Assert.Equal(0, result);

            result = GetOrderQuery(context).Select(x => VfpFunctions.At("i", x.ShipName, 2)).First();

            Assert.Equal(23, result);
        });

        [Fact]
        public void VfpFunctionsTests_AT_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.At("I", x.ShipName)).First();

            Assert.Equal(0, result);

            result = GetOrderQuery(context).Select(x => VfpFunctions.At("i", x.ShipName)).First();

            Assert.Equal(2, result);
        });

        [Fact]
        public void VfpFunctionsTests_Chr_Test() => Execute(context => {
            var result = GetOrderQuery(context).Select(x => VfpFunctions.Chr(x.EmployeeId + 60)).First();

            Assert.Equal("A", result);
        });

        protected IQueryable<Order> GetOrderQuery(NorthwindContext context) => context.Orders.OrderBy(x => x.OrderId).Where(x => x.OrderId == 10248);
    }
}
