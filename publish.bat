dotnet pack .\src\JT808.DotNetty.WebApiClientTool\JT808.DotNetty.WebApiClientTool.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.DotNetty.WebApi\JT808.DotNetty.WebApi.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.DotNetty.Udp\JT808.DotNetty.Udp.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.DotNetty.Tcp\JT808.DotNetty.Tcp.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.DotNetty.Kafka\JT808.DotNetty.Kafka.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.DotNetty.Core\JT808.DotNetty.Core.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.DotNetty.Client\JT808.DotNetty.Client.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.DotNetty.Abstractions\JT808.DotNetty.Abstractions.csproj -c Release --output nupkgs

echo 'push service pacakge...'
dotnet pack .\src\JT808.DotNetty.Services\JT808.DotNetty.MsgIdHandler\JT808.DotNetty.MsgIdHandler.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.DotNetty.Services\JT808.DotNetty.MsgLogging\JT808.DotNetty.MsgLogging.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.DotNetty.Services\JT808.DotNetty.ReplyMessage\JT808.DotNetty.ReplyMessage.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.DotNetty.Services\JT808.DotNetty.SessionNotice\JT808.DotNetty.SessionNotice.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.DotNetty.Services\JT808.DotNetty.Traffic\JT808.DotNetty.Traffic.csproj -c Release --output nupkgs
dotnet pack .\src\JT808.DotNetty.Services\JT808.DotNetty.Transmit\JT808.DotNetty.Transmit.csproj -c Release  --output nupkgs

pause