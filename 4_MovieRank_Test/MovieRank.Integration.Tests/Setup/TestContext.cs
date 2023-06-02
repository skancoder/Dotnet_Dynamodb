using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace MovieRank.Integration.Tests.Setup
{
    public class TestContext : IDisposable
    {
        private TestServer _server;// inmemory test server and http client for dynamodb

        public HttpClient Client { get; set; } 

        public TestContext()
        {
            SetupClient();

            //local dynamodb inside container
            RunCommandPromptCommand("docker pull amazon/dynamodb-local");
            RunCommandPromptCommand("docker run -d -p 8000:8000 amazon/dynamodb-local");
        }

        private void SetupClient()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>()); // add reference to movierank webapp for startup

            _server.BaseAddress = new Uri("http://localhost:8000");

            Client = _server.CreateClient();
        }

        public static void RunCommandPromptCommand(string argument)
        {
            using (var process = new Process())
            {
                var startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = $"/C {argument}"
                };
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
        }

        public void Dispose()
        {
            TestDataSetup.TearDownStore("MovieRank"); // delete table
            Client?.Dispose();
        }
    }
}
