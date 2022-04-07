using LanguageExt;
using Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using static System.IO.Path;
using static System.Reflection.Assembly;
using static Tools.Business.GenDucks;

namespace Tests
{
    [TestFixture]
    public class TestingFunctions
    {
        [Test]
        public void GenerateStateFile_ReturnFileContent()
        {
            string currentPath = GetCurrentDirectory();
            var resultFileState = GenerateStateFile(currentPath);
            List<FileRecord> result = new();

            resultFileState.Match(
                    Left: (ex) => WriteLine(ex.Message),
                    Right: (x) => result.AddRange(x.ToList())
                );

            Assert.IsTrue(result[0].Name == "");
        }

        [Test]
        public void GetFilesNamesFromDirectory_ReturnTwoFiles()
        {
            var resulFilesNames = GetFilesNamesFromDirectory(GetCurrentDirectory());

            List<string> result = new();

            resulFilesNames.Match(
                Left: x => WriteLine(x.Message),
                Right: y => result = y.ToList()
                );

            Assert.AreEqual(2, result.Count);

        }

        private static string GetCurrentDirectory()
        {
            return GetDirectoryName(GetExecutingAssembly().Location)!;
        }
        public static EitherObject<T> GetEitherObject<T>(Either<Exception, T> either) where T : new()
        {
            Exception exception = new();
            T result = new();


            either.Match(
                Left: l => exception = l,
                Right: r => result = r);

            return new EitherObject<T>(exception, result, either.IsLeft);
        }
    }
}
