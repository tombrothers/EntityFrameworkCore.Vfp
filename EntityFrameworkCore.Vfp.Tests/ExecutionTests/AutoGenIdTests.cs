using EntityFrameworkCore.Vfp.Tests.Data.AutoGenId;
using EntityFrameworkCore.Vfp.Tests.Fixtures;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EntityFrameworkCore.Vfp.Tests.ExecutionTests {
    public class AutoGenIdTests : DbContextExecutionTestBase<AutoGenContextFixture, AutoGenContext> {
        public AutoGenIdTests(AutoGenContextFixture fixture, ITestOutputHelper output) : base(fixture, output) {
        }

        [Fact]
        public void AutoGenIdTests_CRUDTest() {
            Create();
            Read();
            Update();
            Delete();
        }

        private void Delete() {
            Execute(context => {
                var entity = context.AutoGens.FirstOrDefault(x => x.Value == "Y");

                context.AutoGens.Remove(entity);
                context.SaveChanges();
            });

            Execute(context => Assert.Null(context.AutoGens.FirstOrDefault(x => x.Value == "Y")));
        }

        private void Update() {
            Execute(context => {
                var entity = context.AutoGens.FirstOrDefault(x => x.Value == "X");

                entity.Value = "Y";
                context.SaveChanges();
            });

            Execute(context => Assert.NotNull(context.AutoGens.FirstOrDefault(x => x.Value == "Y")));
        }

        private void Read() => Execute(context => {
            Assert.NotNull(context.AutoGens.FirstOrDefault(x => x.Value == "X"));
        });

        private void Create() => Execute(context => {
            var entity = new AutoGen();

            Assert.Null(entity.Id);

            entity.Value = "X";
            context.AutoGens.Add(entity);
            context.SaveChanges();

            Assert.NotNull(entity.Id);
        });
    }
}
