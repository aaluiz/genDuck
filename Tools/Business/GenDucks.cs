using LanguageExt;
using Models;
using System.Collections.Immutable;
using System.Text;
using static Newtonsoft.Json.JsonConvert;
using static System.IO.Directory;
using static System.IO.File;
using static System.IO.Path;

namespace Tools.Business
{
    public static class GenDucks
    {
        public static Either<Exception, ImmutableList<FileRecord>> GenerateStateFile(string CurrentDirectory)
            => HandleException(() =>
            {

                var files = GetFilesNamesFromDirectory(CurrentDirectory).GetEitherObject().Result.ToList();
                var fileNames = files.Select(x => GetFileNameWithoutExtension(x)).ToList();
                var dynamics = files.Select(x => GetDynamicObject(GetContentFile(x).GetEitherObject().Result).GetEitherObject().Result).ToList();
                var bodiesState = dynamics.Select(x => GetPropertiesFromState(x));

                List<FileRecord> records = new();

                foreach (var name in fileNames)
                {
                    foreach (var body in bodiesState)
                    {
                        string result = @$"export interface {name} {{
{body}
}}";
                        records.Add(new FileRecord(name, result));
                    }
                }
















                return records.ToImmutableList();
            });

        public static Either<Exception, Seq<string>> GetFilesNamesFromDirectory(string directory)
            => HandleException(() =>
            {
                static bool isJsonFile(string x) => x.Contains(".json");
                static bool isNotTestFile(string x) => !GetFileName(x).Contains("Tests");

                return GetFiles(directory)
                    .Where(isJsonFile)
                    .Where(isNotTestFile)
                    .ToSeq();
            });

        public static List<FileRecord> GetStateFileRecord(List<string>? fileNames, List<dynamic>? dynamics)
        {
            return fileNames!

                   .Select(x => new FileRecord(x, GetTypeScriptInterface(GetFileNameWithoutExtension(x), x))).ToList();
        }


        public static string GetTypeScriptInterface(string stateName, object state)
        {
            string resutl = @$"export interface {stateName} {{
        {GetPropertiesFromState(state)}
}}";
            return resutl;
        }

        private static string GetPropertiesFromState(object state)
        {
            StringBuilder result = new();

            state.GetType().GetProperties().ToList()
                .ForEach(x =>
                {
                    result.AppendLine($"{x.Name}: {x.GetValue(state)};");
                });

            return result.ToString();
        }

        public static Either<Exception, string> GetContentFile(string fileName)
            => HandleException(() =>
            {
                return ReadAllText(fileName);
            });


        public static Either<Exception, dynamic> GetDynamicObject(string content)
            => HandleException(() =>
            {
                return DeserializeObject<dynamic>(value: content)!;
            });


        public static Either<Exception, T> HandleException<T>(Func<T> function)
        {
            try
            {
                return function();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static EitherObject<T> GetEitherObject<T>(this Either<Exception, T> e) where T : new()
        {
            Exception exception = new();
            T result = new();


            e.Match(
                Left: l => exception = l,
                Right: r => result = r);

            return new EitherObject<T>(exception, result, e.IsLeft);
        }
        public static EitherObject<string> GetEitherObject(this Either<Exception, string> e)
        {
            Exception exception = new();
            string result = "";


            e.Match(
                Left: l => exception = l,
                Right: r => result = r);

            return new EitherObject<string>(exception, result, e.IsLeft);
        }
    }
}
