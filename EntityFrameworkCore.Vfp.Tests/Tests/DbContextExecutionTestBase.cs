using EntityFrameworkCore.Vfp.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EntityFrameworkCore.Vfp.Tests.Tests {
    public abstract class DbContextExecutionTestBase<TFixture, TDbContext> : IClassFixture<TFixture>
        where TFixture : DbContextFixtureBase<TDbContext>
        where TDbContext : DbContext {
        private readonly TFixture _fixture;
        private readonly ITestOutputHelper _output;

        protected DbContextExecutionTestBase(TFixture fixture, ITestOutputHelper output) {
            _fixture = fixture;
            _output = output;
        }

        protected void Execute(Action<TDbContext> action) => _fixture.Execute(_output, action);
    }
}
