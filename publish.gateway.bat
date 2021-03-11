dotnet pack .\src\JT808.Gateway\JT808.Gateway.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.Gateway.Kafka\JT808.Gateway.Kafka.csproj -c Release  --output nupkgs
dotnet pack .\src\JT808.Gateway.Abstractions\JT808.Gateway.Abstractions.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.Gateway.Client\JT808.Gateway.Client.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.Gateway.WebApiClientTool\JT808.Gateway.WebApiClientTool.csproj -c Release --output nupkgs

echo 'push service pacakge...'
dotnet pack .\src\JT808.Gateway.Services\JT808.Gateway.MsgLogging\JT808.Gateway.MsgLogging.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.Gateway.Services\JT808.Gateway.ReplyMessage\JT808.Gateway.ReplyMessage.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.Gateway.Services\JT808.Gateway.SessionNotice\JT808.Gateway.SessionNotice.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.Gateway.Services\JT808.Gateway.Transmit\JT808.Gateway.Transmit.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.Gateway.Services\JT808.Gateway.MsgIdHandler\JT808.Gateway.MsgIdHandler.csproj -c Release --output nupkgs
pause