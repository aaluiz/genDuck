using LanguageExt;
using Models;
using Newtonsoft.Json.Linq;
using System.Collections.Immutable;
using System.Dynamic;
using System.Text;
using System.Text.Json;
using static Newtonsoft.Json.JsonConvert;
using static System.IO.Directory;
using static System.IO.File;
using static System.IO.Path;
using static System.Reflection.Assembly;
using static Tools.Business.WriteDuck;

namespace Tools.Business
{
    public static class GenDucks
    {
        public static Either<Exception, bool> WriteFileRecord(FileRecord file, string path)
            => HandleException(() =>
            {
                WriteAllText($"{path}/{file.Name}", file.Content);
                return true;
            });

        public static Either<Exception, FileRecord> GenerateDuckFile(string name)
            => HandleException(() =>
            {
                StringBuilder content = new();
                string fileName = $"{Environment.CurrentDirectory}/{name}.json";
                var stateContent = GetContentFile(fileName).GetEitherObject().Result;
                var props = GetPropertiesFromStateQueryble(stateContent);

                content.AppendLine(SetUpImports(name));
                content.AppendLine(string.Join("\n",props.Select(x => CreateAction(x.Key, x.Value, name))));
                content.AppendLine(CreateInitialState(GetPropertiesFromStateWithInicializedValue(stateContent)));
                content.AppendLine(CreateSelectors(name));
                content.AppendLine(CreateReducer(name, props.Select(x => x.Key).ToImmutableList()));
                
                return new FileRecord($"{name}.duck.ts", content.ToString());
            });

        public static Either<Exception, Seq<FileRecord>> GenerateStateFile(string CurrentDirectory)
            => HandleException(() =>
            {
                var files = GetFilesNamesFromDirectory(CurrentDirectory).GetEitherObject().Result.ToList();
                return files.Select(x => {
                    string fileName = $"{GetFileNameWithoutExtension(x)}.model.ts";
                    string content = $@"export interface {GetFileNameWithoutExtension(x)} {{
{GetPropertiesFromState(GetContentFile(x).GetEitherObject().Result)}
}}";

                    return new FileRecord(fileName,content);
                    }).ToSeq();
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

        public static string GetPropertiesFromState(string fileContent)
        {
            var dictionary = DeserializeObject<Dictionary<string, object>>(fileContent)!.AsQueryable();

            return string.Join("\n", dictionary.Select(x => $"  {x.Key}:{x.Value};").ToList());
        }
        public static string GetPropertiesFromStateWithInicializedValue(string fileContent)
        {
            var dictionary = DeserializeObject<Dictionary<string, string>>(fileContent)!.AsQueryable();

            return string.Join("\n", dictionary.Select(x => $"  {x.Key}: { GetInicializedValue(x.Value)};").ToList());
        }

        private static string GetInicializedValue(string value)
        {
            return value.ToUpper() switch
            {
                "ANY" => "null",
                "BOOLEAN" => "false",
                "DATE" => "new Date()",
                "NUMBER" => "0",
                "STRING" => "\"\"",
                _ => "",
            };
        }

        public static IQueryable<KeyValuePair<string,string>> GetPropertiesFromStateQueryble(string fileContent)
        {
            return DeserializeObject<Dictionary<string, string>>(fileContent)!.AsQueryable();
        }

        public static Either<Exception, string> GetContentFile(string fileName)
            => HandleException(() =>
            {
                return ReadAllText(fileName);
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


        private static string GetCurrentDirectory()
        {
            return GetDirectoryName(GetExecutingAssembly().Location)!;
        }

    }
}
