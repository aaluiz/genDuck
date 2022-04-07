using Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tools.Business.GenDucks;
using static System.Console;

namespace Services.Commands
{
    public class GenStateService : IGenStateService
    {
        public int Execute(string[] args)
        {
            if (args[0] == "gen-states")
            {
                var resultCommand = GenerateStateFile(GetCurrentDirectory()).GetEitherObject();

                if (resultCommand.IsExpection) { 
                    WriteLine(resultCommand.Exception.Message);
                    return -1;
                } 

                var results = resultCommand.Result.ToList().Select(result =>  WriteFileRecord(result, GetCurrentDirectory()).GetEitherObject());

                if (results.Where(x => x.IsExpection).Any()) {
                    results.ToList().ForEach(x => {
                        if (x.IsExpection)
                        {
                           WriteLine(x.Exception.Message);
                        }
                    });
                    return -1;
                };

                resultCommand.Result.ToList().ForEach(x => WriteLine($"GENERATED {x.Name}"));

                return 1;
            }
            return -1;
        }

        private static string GetCurrentDirectory()
        {
            return Environment.CurrentDirectory;
        }
    }
}
