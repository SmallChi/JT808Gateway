dotnet pack .\src\JT808.DotNetty.WebApiClientTool\JT808.DotNetty.WebApiClientTool.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.DotNetty.WebApi\JT808.DotNetty.WebApi.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.DotNetty.Udp\JT808.DotNetty.Udp.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.DotNetty.Tcp\JT808.DotNetty.Tcp.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.DotNetty.Kafka\JT808.DotNetty.Kafka.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.DotNetty.Core\JT808.DotNetty.Core.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.DotNetty.Client\JT808.DotNetty.Client.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.DotNetty.Abstractions\JT808.DotNetty.Abstractions.csproj --no-build --output ../../nupkgs

echo 'push service pacakge...'
dotnet pack .\src\JT808.DotNetty.Services\JT808.DotNetty.MsgIdHandler\JT808.DotNetty.MsgIdHandler.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.DotNetty.Services\JT808.DotNetty.MsgLogging\JT808.DotNetty.MsgLogging.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.DotNetty.Services\JT808.DotNetty.ReplyMessage\JT808.DotNetty.ReplyMessage.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.DotNetty.Services\JT808.DotNetty.SessionNotice\JT808.DotNetty.SessionNotice.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.DotNetty.Services\JT808.DotNetty.Traffic\JT808.DotNetty.Traffic.csproj --no-build --output ../../nupkgs
dotnet pack .\src\JT808.DotNetty.Services\JT808.DotNetty.Transmit\JT808.DotNetty.Transmit.csproj --no-build --output ../../nupkgs

pause