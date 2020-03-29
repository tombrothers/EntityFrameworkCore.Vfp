using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using VfpClient;
using Xunit.Abstractions;
using static System.String;

namespace EntityFrameworkCore.Vfp.Tests.Fixtures {
    public abstract class DbContextFixtureBase<TContext> : IDisposable
         where TContext : DbContext {
        private bool disposed = false;
        protected static readonly string RootDirectory;
        public string DataDirectory { get; }

        static DbContextFixtureBase() {
            RootDirectory = Path.GetDirectoryName(typeof(DbContextFixtureBase<>).GetTypeInfo().Assembly.Location);
        }

        protected DbContextFixtureBase() {
            this.DataDirectory = Path.Combine(RootDirectory, $"Data-{Guid.NewGuid().ToString().Substring(0, 8)}");

            Directory.CreateDirectory(this.DataDirectory);
        }

        public void Execute(ITestOutputHelper output, Action<TContext> action) {
            VfpClientTracing.Tracer.Switch.Level = SourceLevels.All;
            VfpClientTracing.Tracer.Listeners.Clear();            
            VfpClientTracing.Tracer.Listeners.Add(new TestOutputTraceListener(output));

            using(var context = CreateContext()) {
                var connection = (VfpConnection)context.Database.GetDbConnection();

                connection.CommandExecuting = details => log(output, details);
                connection.CommandFailed = details => log(output, details);
                connection.CommandFinished = details => log(output, details);

                action(context);
            }

            static void log(ITestOutputHelper output, VfpCommandExecutionDetails details) => output.WriteLine(Environment.NewLine + details.ToTraceString() + Environment.NewLine);
        }

        protected abstract TContext CreateContext();

        protected virtual void Dispose(bool disposing) {
            if(!disposed) {
                if(IsNullOrWhiteSpace(this.DataDirectory) || !Directory.Exists(this.DataDirectory)) {
                    return;
                }

                try {
                    Directory.Delete(this.DataDirectory, true);
                }
                catch(Exception ex) {
                    Console.WriteLine(ex.ToString());
                }

                disposed = true;
            }
        }

        ~DbContextFixtureBase() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private class TestOutputTraceListener : TraceListener {
            private readonly ITestOutputHelper _output;

            public TestOutputTraceListener(ITestOutputHelper output) {
                _output = output;
            }

            public override void Write(string message) {
                _output.WriteLine(message);
            }

            public override void WriteLine(string message) {
                _output.WriteLine(message.Replace("{", "{{").Replace("}", "}}"));
            }
        }
    }
}
