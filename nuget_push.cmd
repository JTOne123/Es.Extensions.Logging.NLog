set /p ver=<VERSION
set sourceUrl=-Source https://www.nuget.org/api/v2/package

nuget push artifacts/Es.Extensions.Logging.NLog.%ver%.nupkg %sourceUrl%