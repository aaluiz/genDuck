using LanguageExt;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Business
{
    public static class GenDucks
    {
        public static Either<Exception, ImmutableList<FileRecord>> GenerateStateFile(string CurrentDirectory)
            => HandleExpetion(() => {
                var a = CurrentDirectory;
                return new List<FileRecord>() { new FileRecord() }.ToImmutableList();
            });
        
        public static Either<Exception, T> HandleExpetion<T>(Func<T> function) {
            try
            {
                return function();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
