set artifacts=%~dp0artifacts

if exist %artifacts%  rd /q /s %artifacts%

call dotnet restore src/Es.Extensions.Logging.NLog

call dotnet pack src/Es.Extensions.Logging.NLog -c release   -o %artifacts%

pause