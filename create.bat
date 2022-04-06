dotnet new sln -n genDuck
dotnet new console -o  genDuck
dotnet new nunit -o Tests
dotnet new classlib -o Constracts
dotnet new classlib -o Models
dotnet new classlib -o Services
dotnet new classlib -o Tools
dotnet new classlib -o SourceGenerator
cd genDuck
dotnet add package Microsoft.Extensions.DependencyInjection --version 6.0.0
dotnet add package Microsoft.Extensions.Hosting
dotnet add reference ../Constracts/Constracts.csproj
dotnet add reference ../Models/Models.csproj
dotnet add reference ../Services/Services.csproj
dotnet add reference ../Tools/Tools.csproj
dotnet add package LanguageExt.Core 
cd ..
cd Tests
dotnet add reference ../Constracts/Constracts.csproj
dotnet add reference ../Models/Models.csproj
dotnet add reference ../Services/Services.csproj
dotnet add reference ../Tools/Tools.csproj
dotnet add package LanguageExt.Core 
cd ..
cd SouceGenerator
dotnet add reference ../Constracts/Constracts.csproj
dotnet add reference ../Models/Models.csproj
dotnet add reference ../Services/Services.csproj
dotnet add reference ../Tools/Tools.csproj
dotnet add package LanguageExt.Core 
cd ..
cd Services
dotnet add reference ../Constracts/Constracts.csproj
dotnet add reference ../Models/Models.csproj
dotnet add reference ../Tools/Tools.csproj
dotnet add reference ../Crea.Tools.ActiveDirectory/Crea.Tools.ActiveDirectory.csproj
dotnet add package LanguageExt.Core 
cd ..
dotnet sln genDuck.sln add genDuck/genDuck.csproj
dotnet sln genDuck.sln add Constracts/Constracts.csproj
dotnet sln genDuck.sln add Tests/Tests.csproj
dotnet sln genDuck.sln add Models/Models.csproj
dotnet sln genDuck.sln add Services/Services.csproj
