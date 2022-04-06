using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tools.Business.GenDucks;
using static System.IO.Path;
using static System.Reflection.Assembly;
using static System.Console;
using System.Collections.Immutable;
using Models;

namespace Tests
{
    [TestFixture]
    public class TestingFunctions
    {
        [Test]
        public void GenerateStateFile_ReturnFileContent()
        {
            string currentPath = GetDirectoryName(GetExecutingAssembly().Location)!;
            var resultFileState = GenerateStateFile(currentPath);
            List<FileRecord> result = new();

            resultFileState.Match(
                    Left: (ex) => WriteLine(ex.Message),
                    Right: (x) => result.AddRange(x.ToList())
                );  

            Assert.IsTrue(result[0].Name == "");
        }
    }
}
