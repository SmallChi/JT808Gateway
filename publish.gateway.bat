dotnet pack .\src\JT808.Gateway\JT808.Gateway.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.Gateway.Kafka\JT808.Gateway.Kafka.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.Gateway.InMemoryMQ\JT808.Gateway.InMemoryMQ.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.Gateway.Abstractions\JT808.Gateway.Abstractions.csproj --no-build --output ../../nupkgs

echo 'push service pacakge...'
dotnet pack .\src\JT808.Gateway.Services\JT808.Gateway.MsgIdHandler\JT808.Gateway.MsgIdHandler.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.Gateway.Services\JT808.Gateway.MsgLogging\JT808.Gateway.MsgLogging.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.Gateway.Services\JT808.Gateway.ReplyMessage\JT808.Gateway.ReplyMessage.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.Gateway.Services\JT808.Gateway.SessionNotice\JT808.Gateway.SessionNotice.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.Gateway.Services\JT808.Gateway.Traffic\JT808.Gateway.Traffic.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.Gateway.Services\JT808.Gateway.Transmit\JT808.Gateway.Transmit.csproj --no-build --output ../../nupkgs

pause