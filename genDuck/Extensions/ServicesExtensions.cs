using Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Services.Commands;
using Services.StartUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genDuck.Extensions
{
    public static class ServicesExtensions
    {
       public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ICommandLineUI, CommandLineUI>();
            services.AddTransient<IGenDuckService, GenDuckService>();
            services.AddTransient<IGenStateService, GenStateService>();
        } 
    }
}
