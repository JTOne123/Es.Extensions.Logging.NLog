call dotnet restore src/Es.Extensions.Logging.NLog


call dotnet pack --configuration release src/Es.Extensions.Logging.NLog  -o artifacts