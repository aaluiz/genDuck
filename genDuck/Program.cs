using Contracts.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Spectre.Console;
using System.Text;
using genDuck.Extensions;

namespace genDuck
{
    class Program
    {
        static Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            using (IHost host = builder.Build())
            {

                using (IServiceScope serviceScope = host.Services.CreateScope())
                {
                    IServiceProvider provider = serviceScope.ServiceProvider;
                    ICommandLineUI commandLineUI = provider.GetRequiredService<ICommandLineUI>();
                    var result = commandLineUI.ExecuteCommmand(args);
                    if (result == -1) AnsiConsole.Markup(Commands());
                }
                return host.StartAsync();
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(config =>
                {
                    config.AddJsonStream(GetJsonInMemory());
                })
                .ConfigureServices((_, services) =>
                    services.AddServices()
                );

        static string Commands()
        {
            return "Command not found";
        }

        static MemoryStream GetJsonInMemory()
        {
            return new MemoryStream(Encoding.ASCII.GetBytes(GetJsonConfigContent()));
        }

        static string GetJsonConfigContent()
        {
            StringBuilder result = new StringBuilder();
            result.Append("{");
            result.Append("\"Logging\": {");
            result.Append("\"LogLevel\": {");
            result.Append("\"Default\": \"Information\",");
            result.Append("\"Microsoft\": \"Warning\",");
            result.Append("\"Microsoft.Hosting.Lifetime\": \"None\"");
            result.Append("}");
            result.Append("}");
            result.Append("}");
            return result.ToString();
        }

    }
}