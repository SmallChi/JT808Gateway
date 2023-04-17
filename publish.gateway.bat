dotnet pack .\src\JT808.Gateway\JT808.Gateway.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.Gateway.Kafka\JT808.Gateway.Kafka.csproj -c Release  --output nupkgs
dotnet pack .\src\JT808.Gateway.Abstractions\JT808.Gateway.Abstractions.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.Gateway.Client\JT808.Gateway.Client.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.Gateway.WebApiClientTool\JT808.Gateway.WebApiClientTool.csproj -c Release --output nupkgs

pause