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
    public class GenDuckService : IGenDuckService
    {
        public int Execute(string[] args)
        {
            if (args.Length != 2) {
                return -1;
            }

            if (args[0] == "gen-duck") {

                var resultCommand = GenerateDuckFile(args[1]).GetEitherObject();

                if (resultCommand.IsExpection)
                {
                    WriteLine(resultCommand.Exception.Message + " - preparation");
                    return -1;
                }

                var writeResult = WriteFileRecord(resultCommand.Result, Environment.CurrentDirectory).GetEitherObject();

                if (writeResult.IsExpection)
                {
                    WriteLine(writeResult.Exception.Message + "- writting");
                    return -1;
                } 

                WriteLine($"GENERATED {resultCommand.Result.Name}");

                 return 1;
            }
            return -1;
        }
    }
}
