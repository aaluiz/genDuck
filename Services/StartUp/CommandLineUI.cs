using Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StartUp
{
    public class CommandLineUI : ICommandLineUI
    {
        private readonly IGenDuckService? _genDuckService;
        private readonly IGenStateService? _genStateService;

        public CommandLineUI(IGenDuckService genDuckService, IGenStateService genStateService)
        {
            _genDuckService = genDuckService;
            _genStateService = genStateService;
        }
        public int ExecuteCommmand(string[] args)
        {
            var command = GetCommand(args);
            return (command != null) ? command.Execute(args) : -1;
        }


        private ICommand? GetCommand(string[] args)
        {
            if (args.Length != 0)
            {
                switch (args[0])
                {
                    case "gen-duck": return _genDuckService; 
                    case "gen-states": return _genStateService; 
                }
            }
            return null;
        }
    }
}
