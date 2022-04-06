using Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commands
{
    public class GenStateService : IGenStateService
    {
        public int Execute(string[] args)
        {
            if (args[0] == "gen-states") return 1;

            return 0;
        }
    }
}
