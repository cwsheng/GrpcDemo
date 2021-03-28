using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceDemo
{
    public class Program
    {
        public static readonly string SocketPath = Path.Combine(Path.GetTempPath(), "socket.tmp");

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        if (File.Exists(SocketPath))
                        {
                            File.Delete(SocketPath);
                        }
                        options.ListenUnixSocket(SocketPath);
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
